﻿using CRS.ADMIN.APPLICATION.Helper;
using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models;
using CRS.ADMIN.APPLICATION.Models.GroupManagement;
using CRS.ADMIN.BUSINESS.GroupManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.GroupManagement;
using CRS.ADMIN.SHARED.PaginationManagement;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRS.ADMIN.APPLICATION.Controllers
{
    public class GroupManagementController : BaseController
    {
        private readonly IGroupManagementBusiness _business;
        public GroupManagementController(IGroupManagementBusiness business)
        {
            _business = business;
        }
        [HttpGet]
        public ActionResult Index(string SearchFilter = "", int StartIndex = 0, int PageSize = 10)
        {
            Session["CurrentURL"] = "GroupManagement/Index";
            string RenderId = string.Empty;
            ViewBag.SearchFilter = null;
            PaginationFilterCommon paginationFilter = new PaginationFilterCommon()
            {
                Skip = StartIndex,
                Take = PageSize,
                SearchFilter = !string.IsNullOrEmpty(SearchFilter) ? SearchFilter : null
            };
            GroupOverviewModel commonResponse = new GroupOverviewModel();
            if (TempData.ContainsKey("ManageGroup")) commonResponse.ManageGroup = TempData["ManageGroup"] as ManageGroupModel;
            else commonResponse.ManageGroup = new ManageGroupModel();
            if (TempData.ContainsKey("RenderId")) RenderId = TempData["RenderId"].ToString();
            commonResponse.SearchFilter = !string.IsNullOrEmpty(SearchFilter) ? SearchFilter : string.Empty;
            var dbGroupResponse = _business.GetGroupList(paginationFilter);
            var dbGroupAnalyticResponse = _business.GetGroupAnalytic();
            commonResponse.GetGroupList = dbGroupResponse.MapObjects<GroupInfoModel>();
            foreach (var item in commonResponse.GetGroupList)
            {
                item.groupId = ApplicationUtilities.EncryptParameter(item.groupId);
                item.groupImage = ImageHelper.ProcessedImage(item.groupImage);
            }
            commonResponse.GetGroupAnalytic = dbGroupAnalyticResponse.MapObject<GroupAnalyticModel>();
            ViewBag.PopUpRenderValue = !string.IsNullOrEmpty(RenderId) ? RenderId : null;
            ViewBag.StartIndex = StartIndex;
            ViewBag.PageSize = PageSize;
            return View(commonResponse);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageGroup(ManageGroupModel model, HttpPostedFileBase groupCoverImage)
        {
            ActionResult redirectUrl = null;
            bool allowRedirect = false;
            ActionResult redirectresult = null;
            redirectUrl = RedirectToAction("Index", "GroupManagement", new
            {
                SearchFilter = model.SearchFilter,
                StartIndex = model.Skip,
                PageSize = model.Take == 0 ? 10 : model.Take
            });
            string ErrorMessage = string.Empty;
            string coverPhotoPath = string.Empty;
            var allowedContentType = AllowedImageContentType();
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(model.GroupCoverPhoto))
                {

                    if (groupCoverImage == null && string.IsNullOrEmpty(model.GroupCoverPhoto))
                    {
                        ErrorMessage = "Cover Image Required";
                        allowRedirect = true;
                    }
                    if (allowRedirect)
                    {
                        this.AddNotificationMessage(new SHARED.NotificationModel()
                        {
                            NotificationType = SHARED.NotificationMessage.INFORMATION,
                            Message = ErrorMessage ?? "Something went wrong. Please try again later.",
                            Title = SHARED.NotificationMessage.INFORMATION.ToString(),
                        });
                        TempData["ManageGroup"] = model;
                        TempData["RenderId"] = "Manage";
                        return redirectUrl;
                    }
                }
                string coverFileName = string.Empty;
                if (groupCoverImage != null)
                {
                    var contentType = groupCoverImage.ContentType;
                    var extension = Path.GetExtension(groupCoverImage.FileName);
                    if (allowedContentType.Contains(contentType.ToLower()))
                    {
                        coverFileName = $"{AWSBucketFolderNameModel.ADMIN}/Cover_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{extension.ToLower()}";
                        model.GroupCoverPhoto = $"/{coverFileName}";
                    }
                    else
                    {
                        this.AddNotificationMessage(new SHARED.NotificationModel()
                        {
                            NotificationType = SHARED.NotificationMessage.INFORMATION,
                            Message = "Invalid Image Formate",
                            Title = SHARED.NotificationMessage.INFORMATION.ToString(),
                        });
                        allowRedirect = true;
                    }
                }
                if (allowRedirect == true)
                {
                    TempData["ManageGroup"] = model;
                    TempData["RenderId"] = "Manage";
                    return redirectUrl;
                }
                ManageGroupModelCommon commonModel = model.MapObject<ManageGroupModelCommon>();
                commonModel.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
                commonModel.ActionIP = ApplicationUtilities.GetIP();
                if (!string.IsNullOrEmpty(commonModel.GroupId))
                {
                    commonModel.GroupId = commonModel.GroupId.DecryptParameter();
                    if (string.IsNullOrEmpty(commonModel.GroupId))
                    {
                        this.AddNotificationMessage(new SHARED.NotificationModel()
                        {
                            NotificationType = SHARED.NotificationMessage.INFORMATION,
                            Message = "Invalid group detail",
                            Title = SHARED.NotificationMessage.INFORMATION.ToString(),
                        });
                        TempData["ManageGroup"] = model;
                        TempData["RenderId"] = "Manage";
                        return redirectUrl;
                    }
                }
                var dbResponse = _business.ManageGroup(commonModel);
                if (dbResponse != null && dbResponse.Code == 0)
                {
                    if (groupCoverImage != null)
                    {
                        await ImageHelper.ImageUpload(coverFileName, groupCoverImage);
                        this.AddNotificationMessage(new SHARED.NotificationModel()
                        {
                            NotificationType = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : SHARED.NotificationMessage.INFORMATION,
                            Message = dbResponse.Message ?? "Failed",
                            Title = dbResponse.Code == SHARED.ResponseCode.Success ? SHARED.ResponseCode.Success.ToString() : SHARED.NotificationMessage.INFORMATION.ToString()
                        });
                        return redirectUrl;
                    }
                }
                else
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Failed",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    TempData["ManageGroup"] = model;
                    TempData["RenderId"] = "Manage";
                    return redirectUrl;
                }
            }
            var errorMessage = ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}:{e.ErrorMessage}")).ToList();
            TempData["ManageGroup"] = model;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ManageGroup(string GroupId = "", string SearchFilter = "", int StartIndex = 0, int PageSize = 10)
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;

            ManageGroupModel model = new ManageGroupModel();
            if (!string.IsNullOrEmpty(GroupId))
            {
                string groupId = GroupId.DecryptParameter();
                if (string.IsNullOrEmpty(groupId))
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "Invalid group detail",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    return RedirectToAction("Index", "GroupManagement", new
                    {
                        SearchFilter = model.SearchFilter,
                        StartIndex = model.Skip,
                        PageSize = model.Take == 0 ? 10 : model.Take
                    });
                }
                var dbResponse = _business.GetGroupDetail(groupId);
                model = dbResponse.MapObject<ManageGroupModel>();
                model.GroupId = model.GroupId.EncryptParameter();
            }
            TempData["RenderId"] = "Manage";
            TempData["ManageGroup"] = model;
            return RedirectToAction("Index", "GroupManagement", new
            {
                SearchFilter = model.SearchFilter,
                StartIndex = model.Skip,
                PageSize = model.Take == 0 ? 10 : model.Take
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteGroup(string GroupId = "", string SearchFilter = "", int StartIndex = 0, int PageSize = 10)
        {
            var response = new CommonDbResponse();
            var groupId = !string.IsNullOrEmpty(GroupId) ? GroupId.DecryptParameter() : string.Empty;
            if (string.IsNullOrEmpty(groupId))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid detail",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
            }
            var request = new Common()
            {
                ActionIP = ApplicationUtilities.GetIP(),
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString()
            };
            var dbResponse = _business.DeleteGroup(groupId, request);
            response = dbResponse;
            this.AddNotificationMessage(new NotificationModel()
            {
                NotificationType = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                Message = response.Message ?? "something went wrong. Please try again later",
                Title = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString(),
            });
            return Json(response.Message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult BlockGroup(string GroupId = "", string SearchFilter = "", int StartIndex = 0, int PageSize = 10)
        {
            var response = new CommonDbResponse();
            string groupId = !string.IsNullOrEmpty(GroupId) ? GroupId.DecryptParameter() : string.Empty;
            if (string.IsNullOrEmpty(groupId))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid details",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
            }
            var request = new Common()
            {
                ActionIP = ApplicationUtilities.GetIP(),
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString()
            };
            var dbResponse = _business.BlockGroup(groupId, request);
            response = dbResponse;
            this.AddNotificationMessage(new NotificationModel()
            {
                NotificationType = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                Message = response.Message ?? "something went wrong. Please try again later",
                Title = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString(),
            });
            return Json(response.Message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult UnblockGroup(string GroupId = "", string SearchFilter = "", int StartIndex = 0, int PageSize = 10)
        {
            var response = new CommonDbResponse();
            string groupId = !string.IsNullOrEmpty(GroupId) ? GroupId.DecryptParameter() : string.Empty;
            if (string.IsNullOrEmpty(groupId))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid details",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
            }
            var request = new Common()
            {
                ActionIP = ApplicationUtilities.GetIP(),
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString()
            };
            var dbResponse = _business.UnblockGroup(groupId, request);

            response = dbResponse;
            this.AddNotificationMessage(new NotificationModel()
            {
                NotificationType = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                Message = response.Message ?? "something went wrong. Please try again later",
                Title = response.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString(),
            });
            return Json(response.Message, JsonRequestBehavior.AllowGet);
        }
    }
}