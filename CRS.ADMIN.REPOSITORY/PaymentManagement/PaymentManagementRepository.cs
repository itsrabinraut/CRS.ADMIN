﻿using CRS.ADMIN.SHARED.PaginationManagement;
using CRS.ADMIN.SHARED.PaymentManagement;
using System;
using System.Collections.Generic;
using System.Data;

namespace CRS.ADMIN.REPOSITORY.PaymentManagement
{
    public class PaymentManagementRepository : IPaymentManagementRepository
    {
        private readonly RepositoryDao _dao;
        public PaymentManagementRepository() => _dao = new RepositoryDao();

        public List<PaymentLedgerCommon> GetPaymentLedgerDetail(string clubId, string date, PaginationFilterCommon Request, string FromDate = "", string ToDate = "")
        {
            var paymentLogs = new List<PaymentLedgerCommon>();

            string sql = "sproc_admin_payment_management @Flag='gpld'";
            sql += " ,@ClubId =" + _dao.FilterString(clubId);
            if (!string.IsNullOrEmpty(date))
                sql += " ,@Date=" + _dao.FilterString(date);
            sql += !string.IsNullOrEmpty(Request.SearchFilter) ? ",@SearchFilter=N" + _dao.FilterString(Request.SearchFilter) : null;
            sql += !string.IsNullOrEmpty(FromDate) ? ",@FromDate=" + _dao.FilterString(FromDate) : null;
            sql += !string.IsNullOrEmpty(ToDate) ? ",@ToDate=" + _dao.FilterString(ToDate) : null;
            sql += ",@Skip=" + Request.Skip;
            sql += ",@Take=" + Request.Take;
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dbResp.Rows)
                {
                    if (dataRow["Code"].ToString() == "0")
                    {
                        paymentLogs.Add(new PaymentLedgerCommon()
                        {
                            CustomerName = dataRow["CustomerName"].ToString(),
                            CustomerNickName = dataRow["CustomerNickName"].ToString(),
                            CustomerImage = dataRow["CustomerImage"].ToString(),
                            PlanName = dataRow["PlanName"].ToString(),
                            NoOfPeople = dataRow["NoOfPeople"].ToString(),
                            VisitDate = dataRow["VisitDate"].ToString(),
                            VisitTime = dataRow["VisitTime"].ToString(),
                            PaymentType = dataRow["PaymentType"].ToString(),
                            ReservationType = dataRow["ReservationType"].ToString(),
                            TotalRecords = Convert.ToInt32(_dao.ParseColumnValue(dataRow, "TotalRecords").ToString()),
                            SNO = Convert.ToInt32(_dao.ParseColumnValue(dataRow, "SNO").ToString()),
                            PlanAmount = dataRow["PlanAmount"].ToString(),
                            TotalPlanAmount = dataRow["TotalPlanAmount"].ToString(),
                            TotalClubPlanAmount = dataRow["TotalClubPlanAmount"].ToString(),
                            AdminPlanCommissionAmount = dataRow["AdminPlanCommissionAmount"].ToString(),
                            TotalAdminPlanCommissionAmount = dataRow["TotalAdminPlanCommissionAmount"].ToString(),
                            AdminCommissionAmount = dataRow["AdminCommissionAmount"].ToString(),
                            TotalAdminCommissionAmount = dataRow["TotalAdminCommissionAmount"].ToString(),
                            TotalAdminPayableAmount = dataRow["TotalAdminPayableAmount"].ToString()
                        });
                    }
                }
            }

            return paymentLogs;
        }

        public List<PaymentLogsCommon> GetPaymentLogs(string clubId, string date, PaginationFilterCommon Request, string LocationId = "", string FromDate = "", string ToDate = "")
        {
            var paymentLogs = new List<PaymentLogsCommon>();
            string sql = "sproc_admin_payment_management @Flag='gpl'";
            if (!string.IsNullOrEmpty(clubId))
                sql += " ,@ClubId=" + _dao.FilterString(clubId);
            if (!string.IsNullOrEmpty(date))
                sql += " ,@Date=" + _dao.FilterString(date);
            sql += !string.IsNullOrEmpty(Request.SearchFilter) ? ",@SearchFilter=N" + _dao.FilterString(Request.SearchFilter) : null;
            sql += !string.IsNullOrEmpty(LocationId) ? ",@LocationId=" + _dao.FilterString(LocationId) : null;
            sql += !string.IsNullOrEmpty(FromDate) ? ",@FromDate=" + _dao.FilterString(FromDate) : null;
            sql += !string.IsNullOrEmpty(ToDate) ? ",@ToDate=" + _dao.FilterString(ToDate) : null;
            sql += ",@Skip=" + Request.Skip;
            sql += ",@Take=" + Request.Take;
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dbResp.Rows)
                {
                    paymentLogs.Add(new PaymentLogsCommon()
                    {
                        ClubId = dataRow["ClubId"].ToString(),
                        ClubName = dataRow["ClubName"].ToString(),
                        ClubLogo = dataRow["ClubLogo"].ToString(),
                        ClubCategory = dataRow["ClubCategory"].ToString(),
                        Location = dataRow["ClubLocation"].ToString(),
                        Date = dataRow["TransactionDate"].ToString(),
                        TransactionFormattedDate = !string.IsNullOrEmpty(dataRow["TransactionFormattedDate"].ToString()) ? DateTime.Parse(dataRow["TransactionFormattedDate"].ToString()).ToString("yyyy'年'MM'月'dd'日' HH:mm:ss") : dataRow["TransactionFormattedDate"].ToString() ,
                        TotalPlanAmount = Convert.ToInt64(dataRow["TotalPlanAmount"]).ToString("N0"),
                        TotalAdminPlanCommissionAmount =Convert.ToInt64( dataRow["TotalAdminPlanCommissionAmount"]).ToString("N0"),
                        TotalAdminCommissionAmount = Convert.ToInt64(dataRow["TotalAdminCommissionAmount"]).ToString("N0"),
                        GrandTotal = Convert.ToInt64(dataRow["GrandTotal"]).ToString("N0"),
                        TotalRecords = Convert.ToInt32(_dao.ParseColumnValue(dataRow, "TotalRecords").ToString()),
                        SNO = Convert.ToInt32(_dao.ParseColumnValue(dataRow, "SNO").ToString())
                    });
                }
            }

            return paymentLogs;
        }

        public PaymentOverviewCommon GetPaymentOverview()
        {
            var paymentOverview = new PaymentOverviewCommon();
            string sql = "sproc_admin_payment_management @Flag='gpmo'";
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dbResp.Rows)
                {
                    paymentOverview.ReceivedPayments = dataRow["ReceivedPayments"].ToString();
                    paymentOverview.DuePayments = dataRow["DuePayments"].ToString();
                    paymentOverview.AffiliatePayments = dataRow["AffiliatePayment"].ToString();
                }
            }
            return paymentOverview;
        }
    }
}
