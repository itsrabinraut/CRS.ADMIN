using CRS.ADMIN.APPLICATION.Helper;
using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models;
using CRS.ADMIN.APPLICATION.Models.PointsManagement;
using CRS.ADMIN.BUSINESS.PointsManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PaginationManagement;
using CRS.ADMIN.SHARED.PointsManagement;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRS.ADMIN.APPLICATION.Controllers
{
    public class PointsManagementController : BaseController
    {
        private readonly IPointsManagementBusiness _BUSS;
        public PointsManagementController(IPointsManagementBusiness BUSS)
        {
            _BUSS = BUSS;
        }

        [HttpGet]
        public ActionResult PointsTransferList(PointBalanceStatementRequestModel modelRequest, string UserType = "", string UserName = "", string TransferTypeId = "", string FromDate = "", string ToDate = "", string SearchFilter = "", string value = "", int StartIndex = 0, int PageSize = 10, int StartIndex2 = 0, int PageSize2 = 10, int StartIndex3 = 0, int PageSize3 = 10, int StartIndex4 = 0, int PageSize4 = 10, string SearchFilter4 = "", string LocationId4 = "", string ClubName4 = "", string PaymentMethodId4 = "", string FromDate4 = "", string ToDate4 = "", int TotalData3 = 0)
        {
            ViewBag.SearchFilter = SearchFilter;
            Session["CurrentURL"] = "/PointsManagement/PointsTransferList";
            string RenderId = "";
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;

            var objPointsManagementModel = new PointsManagementModel();
            PaginationFilterCommon dbRequest = new PaginationFilterCommon()
            {
                Skip = StartIndex,
                Take = PageSize,
                SearchFilter = !string.IsNullOrEmpty(SearchFilter) ? SearchFilter : null
            };
            ViewBag.UserTypeList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("USERTYPELIST") as Dictionary<string, string>, null, "--- Select ---");
            ViewBag.TransferTypeIdList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("TRANSACTIONTYPE") as Dictionary<string, string>, null, "--- Select ---");
            ViewBag.LocationIdList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("LOCATIONLIST") as Dictionary<string, string>, null, "--- Select ---");
            ViewBag.CLUBTOADMINPaymentMethodListIdList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("CLUBTOADMINPAYMENTMETHODLIST") as Dictionary<string, string>, null, "--- Select ---");
            var dictionaryempty = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(UserType))
            {
                ViewBag.FilterUserList = ApplicationUtilities.SetDDLValue(dictionaryempty, null, "--- Select ---");
            }
            else
            {
                ViewBag.FilterUserList = ApplicationUtilities.SetDDLValue(ApplicationUtilities.LoadDropdownList("USERTYPENAME", UserType.DecryptParameter()) as Dictionary<string, string>, null, "--- Select ---");
            }

            ViewBag.UserList = ApplicationUtilities.SetDDLValue(dictionaryempty, null, "--- Select ---");
            PointsManagementCommon Commonmodel = new PointsManagementCommon();
            Commonmodel.UserType = !string.IsNullOrEmpty(UserType) ? UserType.DecryptParameter() : null;
            Commonmodel.UserName = !string.IsNullOrEmpty(UserName) ? UserName.DecryptParameter() : null;
            Commonmodel.TransferTypeId = !string.IsNullOrEmpty(TransferTypeId) ? TransferTypeId.DecryptParameter() : null;
            Commonmodel.FromDate = !string.IsNullOrEmpty(FromDate) ? FromDate : null;
            Commonmodel.ToDate = !string.IsNullOrEmpty(ToDate) ? ToDate : null;
            ViewBag.UserTypeIdKey = !string.IsNullOrEmpty(UserType) ? UserType : null;
            ViewBag.UsernameIdKey = !string.IsNullOrEmpty(UserName) ? UserName : null;
            ViewBag.TransferTypeIdKey = !string.IsNullOrEmpty(TransferTypeId) ? TransferTypeId : null;
            var dbResponse = _BUSS.GetPointTransferList(Commonmodel, dbRequest);
            objPointsManagementModel.PointsTansferReportList = dbResponse.MapObjects<PointsTansferReportModel>();
            if (TempData.ContainsKey("ManagePointsModel")) objPointsManagementModel.ManagePointsTansfer = TempData["ManagePointsModel"] as PointsTansferModel;
            else objPointsManagementModel.ManagePointsTansfer = new PointsTansferModel();
            if (TempData.ContainsKey("ManagePointsRequestModel")) objPointsManagementModel.ManagePointsRequest = TempData["ManagePointsRequestModel"] as PointsRequestModel;
            else objPointsManagementModel.ManagePointsRequest = new PointsRequestModel();
            if (TempData.ContainsKey("RenderId")) RenderId = TempData["RenderId"].ToString();
            ViewBag.PopUpRenderValue = !string.IsNullOrEmpty(RenderId) ? RenderId : null;
            ViewBag.StartIndex = StartIndex;
            ViewBag.PageSize = PageSize;
            ViewBag.StartIndex2 = StartIndex2;
            ViewBag.PageSize2 = PageSize2;
            ViewBag.StartIndex3 = StartIndex3;
            ViewBag.PageSize3 = PageSize3;
            ViewBag.StartIndex4 = StartIndex4;
            ViewBag.PageSize4 = PageSize4;
            objPointsManagementModel.ListType = value;
            ViewBag.TotalData = dbResponse != null && dbResponse.Any() ? dbResponse[0].TotalRecords : 0;
            ViewBag.TotalData2 = 0;
            ViewBag.TotalData3 = TotalData3;
            objPointsManagementModel.FromDate = FromDate;
            objPointsManagementModel.ToDate = ToDate;
            objPointsManagementModel.PointRequestCommonModel = new PointRequestCommonModel()
            {
                LocationId = LocationId4,
                PaymentMethodId = PaymentMethodId4,
                SearchFilter = SearchFilter4,
                ClubName = ClubName4,
                FromDate = FromDate4,
                ToDate = ToDate4
            };
            objPointsManagementModel.PointBalanceStatementRequest = new PointBalanceStatementRequestModel()
            {
                UserTypeList = UserType.DecryptParameter(),
                UserNameList = UserName.DecryptParameter(),
                TransferTypeList = TransferTypeId.DecryptParameter(),
                From_Date = FromDate,
                To_Date = ToDate
            };
            if (!string.IsNullOrEmpty(TransferTypeId))
                ViewBag.TransferTypeIdKey = TransferTypeId;
            if (!string.IsNullOrEmpty(UserName))
                ViewBag.UsernameIdKey = UserName;
            if (!string.IsNullOrEmpty(UserType))
                ViewBag.UserTypeIdKey = UserType;


            objPointsManagementModel.SystemTransferModel = new SystemTransferRequestModel()
            {
                UserType = UserType.DecryptParameter(),
                UserName = UserName.DecryptParameter(),
                TransferType = TransferTypeId.DecryptParameter(),
                From_Date1 = FromDate,
                To_Date1 = ToDate
            };
            if (!string.IsNullOrEmpty(TransferTypeId))
                ViewBag.TransferTypeIdKey = TransferTypeId;
            if (!string.IsNullOrEmpty(UserName))
                ViewBag.UsernameIdKey = UserName;
            if (!string.IsNullOrEmpty(UserType))
                ViewBag.UserTypeIdKey = UserType;

            PointRequestListFilterCommon getPointRequest = objPointsManagementModel.PointRequestCommonModel.MapObject<PointRequestListFilterCommon>();
            getPointRequest.LocationId = getPointRequest?.LocationId?.DecryptParameter() ?? string.Empty;
            getPointRequest.PaymentMethodId = getPointRequest?.PaymentMethodId?.DecryptParameter() ?? string.Empty;
            getPointRequest.Skip = StartIndex4;
            getPointRequest.Take = PageSize4;
            var getPointRequestListDBResponse = _BUSS.GetPointRequestList(getPointRequest);
            objPointsManagementModel.PointRequestCommonModel.PointRequestsListModel = getPointRequestListDBResponse.MapObjects<PointRequestsListModel>();
            objPointsManagementModel.PointRequestCommonModel.PointRequestsListModel.ForEach(x =>
            {
                x.AgentId = x?.AgentId?.EncryptParameter();
                x.UserId = x?.UserId?.EncryptParameter();
            });
            ViewBag.TotalData4 = objPointsManagementModel?.PointRequestCommonModel?.PointRequestsListModel.Count > 0 ? objPointsManagementModel?.PointRequestCommonModel?.PointRequestsListModel?.FirstOrDefault().RowsTotal ?? "0" : "0";
            return View(objPointsManagementModel);
        }

        //[HttpGet]
        //public ActionResult ManagePoints( string Id = "")
        //{
        //    CategoryModel model = new CategoryModel();
        //    var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
        //    TempData["ManagePointsModel"] = model;
        //    TempData["RenderId"] = "Manage";

        //    return RedirectToAction("PointsTransferList", "PointsManagement");

        //}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ManagePoints(PointsTansferModel objPointsTansferModel, HttpPostedFileBase Image_Certificate)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            string ErrorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                if (Image_Certificate == null)
                {
                    bool allowRedirect = false;

                    if (Image_Certificate == null && string.IsNullOrEmpty(objPointsTansferModel.Image))
                    {
                        ErrorMessage = "Image required";
                        allowRedirect = true;
                    }
                    if (allowRedirect)
                    {
                        this.AddNotificationMessage(new NotificationModel()
                        {
                            NotificationType = NotificationMessage.INFORMATION,
                            Message = ErrorMessage ?? "Something went wrong. Please try again later.",
                            Title = NotificationMessage.INFORMATION.ToString(),
                        });
                        TempData["ManagePointsModel"] = objPointsTansferModel;
                        TempData["RenderId"] = "Manage";
                        return RedirectToAction("PointsTransferList", "PointsManagement");
                    }
                }
                string fileName = string.Empty;
                bool allowRedirectfile = false;
                var allowedContentType = AllowedImageContentType();
                if (Image_Certificate != null)
                {
                    var contentType = Image_Certificate.ContentType;
                    var ext = Path.GetExtension(Image_Certificate.FileName);
                    if (allowedContentType.Contains(contentType.ToLower()))
                    {
                        fileName = $"{AWSBucketFolderNameModel.DOCUMENTS}/ImageCertificate_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{ext.ToLower()}";
                        objPointsTansferModel.Image = $"/{fileName}";
                    }
                    else
                    {
                        this.AddNotificationMessage(new NotificationModel()
                        {
                            NotificationType = NotificationMessage.INFORMATION,
                            Message = "Invalid image format.",
                            Title = NotificationMessage.INFORMATION.ToString(),
                        });
                        allowRedirectfile = true;
                    }
                }
                PointsTansferCommon commonModel = objPointsTansferModel.MapObject<PointsTansferCommon>();
                commonModel.ActionUser = ApplicationUtilities.GetSessionValue("Userid").ToString().DecryptParameter();
                commonModel.ActionIP = ApplicationUtilities.GetIP();
                commonModel.UserTypeId = objPointsTansferModel.UserTypeId.DecryptParameter();
                commonModel.UserId = objPointsTansferModel.UserId.DecryptParameter();
                var dbResponse = _BUSS.ManagePoints(commonModel);
                if (dbResponse != null && dbResponse.Code == 0)
                {
                    if (Image_Certificate != null) await ImageHelper.ImageUpload(fileName, Image_Certificate);
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Failed",
                        Title = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString()
                    });

                    return RedirectToAction("PointsTransferList", "PointsManagement");
                }
                else
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Failed",
                        Title = NotificationMessage.INFORMATION.ToString()
                    });

                    TempData["ManagePointsModel"] = objPointsTansferModel;
                    TempData["RenderId"] = "Manage";
                    return RedirectToAction("PointsTransferList", "PointsManagement");
                }
            }
            TempData["ManagePointsModel"] = objPointsTansferModel;
            TempData["RenderId"] = "Manage";
            return RedirectToAction("PointsTransferList", "PointsManagement");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManagePointsRequest(PointsRequestModel objPointsRequestModel)
        {

            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            string ErrorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PointsRequestCommon commonModel = objPointsRequestModel.MapObject<PointsRequestCommon>();
                commonModel.ActionUser = ApplicationUtilities.GetSessionValue("Userid").ToString().DecryptParameter();
                commonModel.ActionIp = ApplicationUtilities.GetIP();
                var dbResponse = _BUSS.ManagePointsRequest(commonModel);
                if (dbResponse != null && dbResponse.Code == 0)
                {

                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Failed",
                        Title = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString()
                    });

                    return RedirectToAction("PointsTransferList", "PointsManagement");
                }
                else
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = dbResponse.Message ?? "Failed",
                        Title = NotificationMessage.INFORMATION.ToString()
                    });

                    TempData["ManagePointsRequestModel"] = objPointsRequestModel;
                    TempData["RenderId"] = "ManagePoints";
                    return RedirectToAction("PointsTransferList", "PointsManagement");
                }
            }
            TempData["ManagePointsRequestModel"] = objPointsRequestModel;
            TempData["RenderId"] = "ManagePoints";
            return RedirectToAction("PointsTransferList", "PointsManagement");
        }

        [HttpPost, ValidateAntiForgeryToken, OverrideActionFilters]
        public async Task<JsonResult> ManageClubPointRequest(ManageClubPointRequestModel request, HttpPostedFileBase Image)
        {
            var dbRequest = request.MapObject<ManageClubPointRequestCommon>();
            dbRequest.AgentId = dbRequest?.AgentId?.DecryptParameter() ?? string.Empty;
            dbRequest.UserId = dbRequest?.UserId?.DecryptParameter() ?? string.Empty;
            if (string.IsNullOrEmpty(dbRequest.AgentId) ||
                string.IsNullOrEmpty(dbRequest.UserId) ||
                string.IsNullOrEmpty(dbRequest.TxnId))
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid request",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json("Invalid request", JsonRequestBehavior.AllowGet);
            }
            if (dbRequest.Status.Trim().ToUpper() == "S" && Image == null)
            {
                this.AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Image required",
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json("Image required", JsonRequestBehavior.AllowGet);
            }
            string fileName = string.Empty;
            var allowedContentType = AllowedImageContentType();
            if (Image != null)
            {
                var contentType = Image.ContentType;
                var ext = Path.GetExtension(Image.FileName);
                if (allowedContentType.Contains(contentType.ToLower()))
                {
                    fileName = $"{AWSBucketFolderNameModel.DOCUMENTS}/ClubPointRequestReceiptImage_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{ext.ToLower()}";
                    dbRequest.ImageURL = $"/{fileName}";
                }
                else
                {
                    this.AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "Invalid image format.",
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    return Json("Invalid image format.", JsonRequestBehavior.AllowGet);
                }
            }
            dbRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
            dbRequest.ActionIP = ApplicationUtilities.GetIP();
            var dbResponse = _BUSS.ManageClubPointRequest(dbRequest);
            if (dbResponse != null && dbResponse.Code == ResponseCode.Success)
                if (Image != null) await ImageHelper.ImageUpload(fileName, Image);
            this.AddNotificationMessage(new NotificationModel()
            {
                NotificationType = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS : NotificationMessage.INFORMATION,
                Message = dbResponse.Message ?? "Something went wrong. Please try again later",
                Title = dbResponse.Code == ResponseCode.Success ? NotificationMessage.SUCCESS.ToString() : NotificationMessage.INFORMATION.ToString()
            });
            return Json(dbResponse.Message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet, OverrideActionFilters]
        public async Task<ActionResult> GetPointBalanceStatementDetails(PointBalanceStatementRequestModel requestModel)
        {
            ViewBag.SearchFilter = requestModel.SearchFilter;
            Session["CurrentURL"] = "/PointsManagement/PointsTransferList";
            string RenderId = "";
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;

            ViewBag.UserTypeList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("USERTYPELIST") as Dictionary<string, string>, null, "--- All ---");
            if (!string.IsNullOrEmpty(requestModel.UserTypeList))
                ViewBag.FilterUserList = ApplicationUtilities.SetDDLValue(ApplicationUtilities.LoadDropdownList("USERTYPENAME", requestModel.UserTypeList.DecryptParameter()) as Dictionary<string, string>, null, "--- All ---");
            ViewBag.TransferTypeIdList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("TRANSACTIONTYPE") as Dictionary<string, string>, null, "--- All ---");

            requestModel.UserTypeList = !string.IsNullOrEmpty(requestModel.UserTypeList) ? requestModel.UserTypeList.DecryptParameter() : null;
            requestModel.UserNameList = !string.IsNullOrEmpty(requestModel.UserNameList) ? requestModel.UserNameList.DecryptParameter() : null;
            requestModel.TransferTypeList = !string.IsNullOrEmpty(requestModel.TransferTypeList) ? requestModel.TransferTypeList.DecryptParameter() : null;

            var mappedObject = requestModel.MapObject<PointBalanceStatementRequestCommon>();
            mappedObject.Skip = requestModel.StartIndex2;
            mappedObject.Take = requestModel.PageSize2;

      
            var response = _BUSS.GetPointBalanceStatementDetailsAsync(mappedObject);

            var mappedResponseObjects = response.MapObjects<PointBalanceStatementResponseModel>();

            ViewBag.PageSize2 = requestModel.PageSize2;
            ViewBag.TotalData2 = response.FirstOrDefault().RowTotal;
            ViewBag.StartIndex2 = requestModel.StartIndex2;

            TempData["ListModel"] = mappedResponseObjects;
            return RedirectToAction("PointsTransferList", "PointsManagement", new { value = "pbs", FromDate = requestModel.From_Date, ToDate = requestModel.To_Date, UserType = requestModel.UserTypeList.EncryptParameter(), UserName = requestModel.UserNameList.EncryptParameter(), TransferTypeId = requestModel.TransferTypeList.EncryptParameter(), SearchFilter = requestModel.SearchFilter, TotalData2 = response.FirstOrDefault().RowTotal});
            //return PartialView("PointsTransferList", mappedResponseObjects);
        }
        [HttpGet, OverrideActionFilters]
        public async Task<ActionResult> GetSystemTransferReportAsync(SystemTransferRequestModel requestModel)
        {
            ViewBag.SearchFilter = requestModel.SearchFilter;
            Session["CurrentURL"] = "/PointsManagement/PointsTransferList";
            string RenderId = "";
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;

            ViewBag.UserTypeList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("USERTYPELIST") as Dictionary<string, string>, null, "--- All ---");
            if (!string.IsNullOrEmpty(requestModel.UserType))
                ViewBag.FilterUserList = ApplicationUtilities.SetDDLValue(ApplicationUtilities.LoadDropdownList("USERTYPENAME", requestModel.UserType.DecryptParameter()) as Dictionary<string, string>, null, "--- All ---");
            ViewBag.TransferTypeIdList = ApplicationUtilities.SetDDLValue(DDLHelper.LoadDropdownList("TRANSACTIONTYPE") as Dictionary<string, string>, null, "--- All ---");

            requestModel.UserType = !string.IsNullOrEmpty(requestModel.UserType) ? requestModel.UserType.DecryptParameter() : null;
            requestModel.UserName = !string.IsNullOrEmpty(requestModel.UserName) ? requestModel.UserName.DecryptParameter() : null;
            requestModel.TransferType = !string.IsNullOrEmpty(requestModel.TransferType) ? requestModel.TransferType.DecryptParameter() : null;

            var mappedObject = requestModel.MapObject<SystemTransferRequestCommon>();
            mappedObject.Skip = requestModel.StartIndex3;
            mappedObject.Take = requestModel.PageSize3;

            var response = _BUSS.GetSystemTransferDetailsAsync(mappedObject);


            var mappedResponseObjects = response.MapObjects<SystemTransferReponseModel>();

            ViewBag.PageSize3 = requestModel.PageSize3;
            ViewBag.TotalData3 = response.FirstOrDefault().RowTotal;
            ViewBag.StartIndex3 = requestModel.StartIndex3;
            TempData["ListModel"] = mappedResponseObjects;
            return RedirectToAction("PointsTransferList", "PointsManagement", new { value = "st", FromDate = requestModel.From_Date1, ToDate = requestModel.To_Date1, UserType = requestModel.UserType.EncryptParameter(), UserName = requestModel.UserName.EncryptParameter(), TransferTypeId = requestModel.TransferType.EncryptParameter(), SearchFilter = requestModel.SearchFilter , TotalData3 = response.FirstOrDefault().RowTotal });
        }
    }
}