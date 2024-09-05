﻿namespace CRS.ADMIN.APPLICATION.Models.ApiResponseMessage
{
    public class ApiResponseMessageModel
    {
        public string SearchFilter { get; set; }
        public string SearchFilterPending { get; set; }
        public string SearchFilterReject { get; set; }
        public string MessageId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string MessageEng { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MessageType { get; set; }
        public string HttpStatusCode { get; set; }
        public string ActionUser { get; set; }
    }
}