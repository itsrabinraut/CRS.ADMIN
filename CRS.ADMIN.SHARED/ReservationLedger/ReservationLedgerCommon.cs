﻿namespace CRS.ADMIN.SHARED.ReservationLedger
{
    public class ReservationLedgerCommon : Common
    {
        public string ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubLogo { get; set; }
        public string ClubCategory { get; set; }
        public string AdminPayment { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionFormattedDate { get; set; }
        public string TotalVisitors { get; set; }
    }
    public class ReservationLedgerDetailCommon
    {
        public string ClubId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNickName { get; set; }
        public string PlanName { get; set; }
        public string NoOfPeople { get; set; }
        public string VisitTime { get; set; }
        public string VisitDate { get; set; }
        public string PaymentType { get; set; }
        public string PlanAmount { get; set; }
        public string TotalAmount { get; set; }
        public string CommissionAmount { get; set; }
        public string TotalCommissionAmount { get; set; }
        public string AdminPaymentAmount { get; set; }
        public string CustomerImage { get; set; }
        public string ReservationType { get; set; }
        public string ClubVerification { get; set; }
        public string TransactionStatus { get; set; }
    }
}
