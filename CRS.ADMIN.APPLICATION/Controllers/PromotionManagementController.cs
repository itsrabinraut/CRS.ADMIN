﻿using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models.PromotionManagement;
using CRS.ADMIN.BUSINESS.PromotionManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PromotionManagement;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRS.ADMIN.APPLICATION.Controllers
{
    public class PromotionManagementController : BaseController
    {
        private readonly IPromotionManagementBusiness _business;
        public PromotionManagementController(IPromotionManagementBusiness business) => this._business = business;
        public ActionResult GetPromotionalImages()
        {
            Session["CurrentUrl"] = "/PromotionManagement/GetPromotionalImages";

            PromotionManagementCommonModel ResponseModel = new PromotionManagementCommonModel();
            var promotionalImages = _business.GetPromotionalImageLists();
            ResponseModel.PromotionManagementListModel = promotionalImages.MapObjects<PromotionManagementListModel>();
            foreach (var item in ResponseModel.PromotionManagementListModel)
            {
                item.Id = item.Id?.EncryptParameter();
                item.IsDeleted = item.IsDeleted.Trim().ToUpper() == "FALSE" ? "A" : "B";
            }

            string RenderId = "";
            if (TempData.ContainsKey("PromotionManagementModel")) ResponseModel.PromotionManagementModel = TempData["PromotionManagementModel"] as PromotionManagementModel;
            if (TempData.ContainsKey("RenderId")) RenderId = TempData["RenderId"].ToString();
            ViewBag.PopUpRenderValue = !string.IsNullOrEmpty(RenderId) ? RenderId : null;
            return View(ResponseModel);
        }
        [HttpGet]
        public ActionResult ManagePromotionalImage(string Id)
        {
            var i = !string.IsNullOrEmpty(Id) ? Id.DecryptParameter() : null;
            if (string.IsNullOrEmpty(i))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid details",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return RedirectToAction("GetPromotionalImages", "PromotionManagement");
            }
            var dbResponse = _business.GetPromotionalImageById(i);
            var model = dbResponse.MapObject<PromotionManagementModel>();
            model.Id = model.Id.EncryptParameter();
            TempData["PromotionManagementModel"] = model;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("GetPromotionalImages", "PromotionManagement");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManagePromotionalImage(PromotionManagementModel promotionManagementModel, HttpPostedFileBase ImagePathFile)
        {
            if (!ModelState.IsValid)
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Please fill all required fields",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                TempData["PromotionManagementModel"] = promotionManagementModel;
                TempData["RenderId"] = "Manage";
                return RedirectToAction("GetPromotionalImages", "PromotionManagement");
            }
            var id = !string.IsNullOrEmpty(promotionManagementModel.Id) ? promotionManagementModel.Id.DecryptParameter() : null;
            if (!string.IsNullOrEmpty(promotionManagementModel.Id) && string.IsNullOrEmpty(id))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid promotion details",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return RedirectToAction("GetPromotionalImages", "PromotionManagement");
            }

            if (ImagePathFile == null)
            {
                if (string.IsNullOrEmpty(promotionManagementModel.ImagePath))
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "Image required",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    TempData["PromotionManagementModel"] = promotionManagementModel;
                    TempData["RenderId"] = "Manage";
                    return RedirectToAction("GetPromotionalImages", "PromotionManagement");
                }
            }
            var promoImageCommon = promotionManagementModel.MapObject<PromotionManagementCommon>();
            string imgPath = string.Empty;
            var allowedContenttype = AllowedImageContentType();
            if (ImagePathFile != null)
            {
                var contentType = ImagePathFile.ContentType;
                var ext = Path.GetExtension(ImagePathFile.FileName);
                if (allowedContenttype.Contains(contentType.ToLower()))
                {
                    string datet = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string myfilename = "PromoImg_" + datet + ext.ToLower();
                    imgPath = Path.Combine(Server.MapPath("~/Content/userupload/PromotionalImages/"), myfilename);
                    promoImageCommon.ImagePath = "/Content/userupload/PromotionalImages/" + myfilename;
                }
                else
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "File Must be .jpg, .png, .jpeg, .heif",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    TempData["PromotionManagementModel"] = promotionManagementModel;
                    TempData["RenderId"] = "Manage";
                    return RedirectToAction("GetPromotionalImages", "PromotionManagement");
                }
            }
            promoImageCommon.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
            promoImageCommon.ActionIP = ApplicationUtilities.GetIP();
            promoImageCommon.Id = id;
            var serviceResp = _business.EditPromotionalImage(promoImageCommon);
            if (serviceResp != null && serviceResp.Code == ResponseCode.Success)
            {
                if (ImagePathFile != null) ApplicationUtilities.ResizeImage(ImagePathFile, imgPath);
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.SUCCESS,
                    Message = serviceResp.Message ?? "Success",
                    Title = NotificationMessage.SUCCESS.ToString(),
                });
                return RedirectToAction("GetPromotionalImages", "PromotionManagement");
            }
            this.AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.INFORMATION,
                Message = serviceResp.Message ?? "Failed",
                Title = NotificationMessage.INFORMATION.ToString()
            });
            TempData["PromotionManagementModel"] = promotionManagementModel;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("GetPromotionalImages", "PromotionManagement");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult BlockUnblockPromotionalImage(string Id)
        {
            var i = !string.IsNullOrEmpty(Id) ? Id.DecryptParameter() : null;
            if (string.IsNullOrEmpty(i))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid request",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json(new { Code = 1, Message = "Invalid Request." });
            }
            var promoImageCommon = new PromotionManagementCommon()
            {
                Id = i,
                ActionUser = ApplicationUtilities.GetSessionValue("Username")?.ToString(),
                ActionIP = ApplicationUtilities.GetIP()
            };
            var dbResponse = _business.DeletePromotionalImage(promoImageCommon);
            if (dbResponse.Code == ResponseCode.Success)
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.SUCCESS,
                    Message = dbResponse.Message ?? "Success",
                    Title = NotificationMessage.SUCCESS.ToString(),
                });
            }
            else
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = dbResponse.Message ?? "Failed",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
            }
            return Json(dbResponse.Message);
        }
    }
}