﻿using CRS.ADMIN.APPLICATION.Library;
using CRS.ADMIN.APPLICATION.Models;
using CRS.ADMIN.APPLICATION.Models.NotificationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CRS.ADMIN.APPLICATION.Helper
{
    public class NotificationHelper
    {
        private static SignalRConfigruationModel _signalRConfigruation = ApplicationUtilities.GetAppDataJsonConfigValue<SignalRConfigruationModel>("SignalRConfigruation");
        private readonly SignalRStringCipher _stringCipher;
        private const string ErrorMessage = "Something went wrong. Please try again later";
        private const string ActionPlatform = "CustomerWeb";
        public NotificationHelper(SignalRStringCipher stringCipher)
        {
            _stringCipher = stringCipher;
        }

        public async Task<NotificationHelperCommonAPIResponseModel> MarkNotificationAsReadHelperAsync(NotificationReadRequestModel request)
        {
            var apiRequest = new NotificationReadRequestModel
            {
                agentId = _stringCipher.Encrypt(request.agentId),
                notificationId = !string.IsNullOrEmpty(request.notificationId) ? _stringCipher.Encrypt(request.notificationId) : string.Empty,
                actionPlatform = ActionPlatform,
                actionUser = _stringCipher.Encrypt(request.actionUser)
            };

            var signalRServiceToken = ApplicationUtilities.GetSessionValue("SignalRServiceToken")?.ToString();
            if (string.IsNullOrEmpty(signalRServiceToken))
            {
                var tokenResponse = await GetAPITokenAsync();
                if (tokenResponse.Item1)
                    signalRServiceToken = tokenResponse.Item2;
                else
                    return new NotificationHelperCommonAPIResponseModel { code = "1", message = ErrorMessage };
            }

            var apiResponse = await HttpClientHelper.HttpPutRequestWithTokenAsync<NotificationHelperCommonAPIResponseModel>(
            $"{_signalRConfigruation.baseURL.TrimEnd('/')}/api/customer-notification/mark-read", apiRequest, signalRServiceToken);

            if (apiResponse?.code == "0")
            {
                var dataObject = apiResponse.data?.MapObject<NotificationReadResponseModel>();
                var notificationIds = dataObject.notificationId?.Select(id => _stringCipher.Decrypt(id)).ToList() ?? new List<string>();
                var encryptedNotificationIds = notificationIds?.Select(id => ApplicationUtilities.EncryptParameter(id)).ToList() ?? new List<string>();
                dataObject.notificationId = encryptedNotificationIds.ToArray();
                return new NotificationHelperCommonAPIResponseModel
                {
                    code = "0",
                    message = apiResponse.message ?? "Success",
                    data = dataObject
                };
            }
            return new NotificationHelperCommonAPIResponseModel { code = "1", message = apiResponse?.message ?? ErrorMessage };
        }

        public async Task SendClubNotificationHelperAsync(NotificationManagementModel request) =>
           await SendNotificationHelperAsync(request, "club-notification/send");

        public async Task SendCustomerNotificationHelperAsync(NotificationManagementModel request) =>
            await SendNotificationHelperAsync(request, "customer-notification/send");

        public async Task SendAdminNotificationHelperAsync(NotificationManagementModel request) =>
            await SendNotificationHelperAsync(request, "admin-notification/send");

        public async Task SendNotificationHelperAsync(NotificationManagementModel request, string endpoint)
        {
            var apiRequest = new NotificationManagementModel
            {
                agentId = _stringCipher.Encrypt(request.agentId),
                notificationType = request.notificationType,
                extraId1 = !string.IsNullOrEmpty(request.extraId1) ? _stringCipher.Encrypt(request.extraId1) : string.Empty,
                actionPlatform = ActionPlatform,
                actionUser = _stringCipher.Encrypt(request.actionUser)
            };

            var signalRServiceToken = ApplicationUtilities.GetSessionValue("SignalRServiceToken")?.ToString();
            if (string.IsNullOrEmpty(signalRServiceToken))
            {
                var tokenResponse = await GetAPITokenAsync();
                if (tokenResponse.Item1)
                    signalRServiceToken = tokenResponse.Item2;
                else
                    return;
            }

            var apiResponse = await HttpClientHelper.HttpPostRequestWithTokenAsync<NotificationHelperCommonAPIResponseModel>(
            $"{_signalRConfigruation.baseURL.TrimEnd('/')}/api/{endpoint}", apiRequest, signalRServiceToken);
            //if (apiResponse?.code == "0")
            //{
            //    var dataObject = apiResponse.data?.MapObject<NotificationReadResponseModel>();

            //    if (!string.IsNullOrEmpty(dataObject?.notificationId))
            //        dataObject.notificationId = _stringCipher.Encrypt(dataObject.notificationId);
            //}
            return;
        }

        private async Task<Tuple<bool, string>> GetAPITokenAsync()
        {
            var apiRequest = new LoginRequestModel
            {
                userName = _signalRConfigruation.username,
                password = _signalRConfigruation.password,
                actionPlatform = ActionPlatform
            };

            var apiResponse = await HttpClientHelper.HttpPostRequestAsync<NotificationHelperCommonAPIResponseModel>(
                $"{_signalRConfigruation.baseURL.TrimEnd('/')}/api/login", apiRequest);

            if (apiResponse != null && apiResponse.code == "0")
            {
                var dataObject = apiResponse.data.MapObject<LoginResponseModel>();
                HttpContext.Current.Session["SignalRServiceToken"] = dataObject.token;
                return new Tuple<bool, string>(true, dataObject.token);
            }

            return new Tuple<bool, string>(false, string.Empty);
        }
    }
}