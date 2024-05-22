﻿using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models.StaticDataManagement;
using CRS.ADMIN.BUSINESS.StaticDataManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.StaticDataManagement;
using System.Web.Mvc;

namespace CRS.ADMIN.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class StaticDataManagementController : BaseController
    {
        private readonly IStaticDataManagementBusiness _business;
        public StaticDataManagementController(IStaticDataManagementBusiness business)
        {
            _business = business;
        }
        #region MANAGE STATIC DATA TYPE
        public ActionResult Index(string SearchText = "")
        {
            Session["CurrentURL"] = "/StaticDataManagement/Index";
            string RenderId = "";
            StaticDataManagement responseInfo = new StaticDataManagement();
            if (TempData.ContainsKey("ManageStaticDataType")) responseInfo.ManageStaticDataType = TempData["ManageStaticDataType"] as ManageStaticDataType;
            else responseInfo.ManageStaticDataType = new ManageStaticDataType();
            if (TempData.ContainsKey("RenderId")) RenderId = TempData["RenderId"].ToString();
            var dbResponseInfo = _business.GetStatiDataTypeList(SearchText);
            responseInfo.GetStaticDataTypeList = dbResponseInfo.MapObjects<StaticDataManagementModel>();
            foreach (var item in responseInfo.GetStaticDataTypeList)
            {
                item.Id = item.Id.EncryptParameter();
            }
            ViewBag.PopUpRenderValue = !string.IsNullOrEmpty(RenderId) ? RenderId : null;
            return View(responseInfo);
        }
        [HttpGet]
        public ActionResult ManageStaticDataType(string id = "")
        {
            ManageStaticDataType model = new ManageStaticDataType();
            if (!string.IsNullOrEmpty(id))
            {
                if (string.IsNullOrEmpty(id))
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "Invalid details",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                }
                string Id = id.DecryptParameter();
                var dbResponse = _business.GetStaticDataTypeDetail(Id);
                model = dbResponse.MapObject<ManageStaticDataType>();
                model.Id = Id.EncryptParameter();
            }
            TempData["ManageStaticDataType"] = model;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManageStaticDataType(ManageStaticDataType model)
        {
            if (ModelState.IsValid)
            {
                ManageStaticDataTypeCommon commonModel = model.MapObject<ManageStaticDataTypeCommon>();
                commonModel.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
                if (!string.IsNullOrEmpty(commonModel.Id))
                    commonModel.Id = commonModel.Id.DecryptParameter();

                var dbResponse = _business.ManageStaticDataType(commonModel);
                if (dbResponse != null)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Something went wrong try again later",
                        Title = NotificationMessage.WARNING.ToString()
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbResponse.Message ?? "Static Data Added Successfully",
                        Title = NotificationMessage.SUCCESS.ToString()
                    });
                }
            }
            TempData["ManageStaticDataType"] = model;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteStaticDataType(string id = "")
        {
            if (!string.IsNullOrEmpty(id))
            {
                var request = new ManageStaticDataTypeCommon()
                {
                    ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString(),
                    Id = id.DecryptParameter(),
                };
                var dbResponse = _business.DeleteStaticDataType(request);
                if (dbResponse == null)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.WARNING,
                        Message = dbResponse.Message ?? "Something went wrong try again later",
                        Title = NotificationMessage.WARNING.ToString()
                    });
                    return Json(dbResponse.SetMessageInTempData(this));
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbResponse.Message ?? "Staff has been deleted successfully",
                        Title = NotificationMessage.SUCCESS.ToString()
                    });
                    return Json(dbResponse.SetMessageInTempData(this));
                }
            }
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.SUCCESS,
                    Message = "Invalid Request",
                    Title = NotificationMessage.SUCCESS.ToString()
                });
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region MANAGE STATIC DATA
        public ActionResult StaticDataIndex()
        {
            return View();
        }
        #endregion
    }
}