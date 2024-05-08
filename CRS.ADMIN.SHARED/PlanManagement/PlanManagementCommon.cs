﻿using CRS.ADMIN.SHARED.PaginationManagement;
using System.Web;

namespace CRS.ADMIN.SHARED.PlanManagement
{
    public class PlanManagementCommon : PaginationResponseCommon
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string PlanTime { get; set; }
        public string Price { get; set; }
        public string Liquor { get; set; }
        public string Nomination { get; set; }
        public string Remarks { get; set; }
        public string PlanStatus { get; set; }
        public string PlanImage { get; set; }
        public string PlanImage2 { get; set; }
        public string ExtraField1 { get; set; }
        public string ExtraField2 { get; set; }
        public string ExtraField3 { get; set; }
        public string PlanCategory { get; set; }
        public int NoOfPeople { get; set; }
        public string StrikePrice { get; set; }
        public string IsStrikeOut { get; set; }
    }
}