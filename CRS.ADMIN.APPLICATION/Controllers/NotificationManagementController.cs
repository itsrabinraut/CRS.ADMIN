﻿using CRS.ADMIN.APPLICATION.Helper;
using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models.NotificationManagement;
using CRS.ADMIN.BUSINESS.NotificationManagement;
using CRS.ADMIN.SHARED.NotificationManagement;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CRS.ADMIN.APPLICATION.Controllers
{
    public class NotificationManagementController : BaseController
    {
        private readonly INotificationManagementBusiness _buss;
        public NotificationManagementController(INotificationManagementBusiness buss)
        {
            _buss = buss;
        }
        [HttpGet]
        public ActionResult ViewAllNotifications(string NotificationId = "")
        {
            Session["CurrentURL"] = "/NotificationManagement/ViewAllNotifications";
            var requestCommon = new ManageNotificationCommon()
            {
                AdminId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter(),
                NotificationId = !string.IsNullOrEmpty(NotificationId) ? NotificationId.DecryptParameter() : null,
            };
            var dbResponse = _buss.GetAllNotification(requestCommon);
            List<NotificationDetailModel> response = dbResponse.MapObjects<NotificationDetailModel>();
            response.ForEach(x =>
            {
                x.NotificationId = x.NotificationId.EncryptParameter();
                x.NotificationImageURL = ImageHelper.ProcessedImage(x.NotificationImageURL);
                x.CreatedDate = !string.IsNullOrEmpty(x.CreatedDate) ? DateTime.Parse(x.CreatedDate).ToString("yyyy'年'MM'月'dd'日' HH:mm:ss") : x.CreatedDate;
                x.UpdatedDate = !string.IsNullOrEmpty(x.UpdatedDate) ? DateTime.Parse(x.UpdatedDate).ToString("yyyy'年'MM'月'dd'日' HH:mm:ss") : x.UpdatedDate;
            });
            return View(response);
        }
    }
}