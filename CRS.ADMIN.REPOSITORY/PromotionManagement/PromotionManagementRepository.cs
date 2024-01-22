﻿using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PromotionManagement;
using System.Collections.Generic;
using System.Data;

namespace CRS.ADMIN.REPOSITORY.PromotionManagement
{
    public class PromotionManagementRepository : IPromotionManagementRepository
    {
        private readonly RepositoryDao _dao;
        public PromotionManagementRepository() => _dao = new RepositoryDao();

        public CommonDbResponse AddPromotionalImage(PromotionManagementCommon promotion)
        {
            var sql = "Exec sproc_promotion_management @Flag='a'";
            sql += ",@Title = " + _dao.FilterString(promotion.Title);
            sql += ",@Description = " + _dao.FilterString(promotion.Description);
            sql += ",@ImagePath = " + _dao.FilterString(promotion.ImagePath);
            sql += ",@ActionUser = " + _dao.FilterString(promotion.ActionUser);
            sql += ",@ActionIP = " + _dao.FilterString(promotion.IpAddress);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse DeletePromotionalImage(PromotionManagementCommon promotion)
        {
            var sql = "Exec sproc_promotion_management";
            sql += " @Flag='bu'";
            sql += ",@Id=" + _dao.FilterString(promotion.Id);
            sql += ", @ActionUser=" + _dao.FilterString(promotion.ActionUser);
            sql += ", @ActionIP=" + _dao.FilterString(promotion.ActionIP);
            return _dao.ParseCommonDbResponse(sql);
        }

        public CommonDbResponse EditPromotionalImage(PromotionManagementCommon promotion)
        {
            var sql = "Exec sproc_promotion_management ";
            sql += !string.IsNullOrEmpty(promotion.Id) ? "@Flag='u'" : "@Flag='a'";
            sql += ",@Id=" + _dao.FilterString(promotion.Id);
            sql += ",@Title = " + _dao.FilterString(promotion.Title);
            sql += ",@Description = " + _dao.FilterString(promotion.Description);
            sql += ",@ImagePath = " + _dao.FilterString(promotion.ImagePath);
            sql += ",@ActionUser=" + _dao.FilterString(promotion.ActionUser);
            sql += ",@ActionIP=" + _dao.FilterString(promotion.IpAddress);
            return _dao.ParseCommonDbResponse(sql);
        }

        public PromotionManagementCommon GetPromotionalImageById(string Id)
        {
            var sql = "exec sproc_promotion_management @Flag='gp'";
            sql += ",@Id=" + _dao.FilterString(Id);
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                return new PromotionManagementCommon()
                {
                    Id = Id,
                    Title = dbResp.Rows[0]["Title"]?.ToString(),
                    Description = dbResp.Rows[0]["ImgDescription"]?.ToString(),
                    ImagePath = dbResp.Rows[0]["ImgPath"]?.ToString(),
                    IsDeleted = dbResp.Rows[0]["IsDeleted"]?.ToString(),
                    ActionUser = dbResp.Rows[0]["ActionUser"]?.ToString(),
                    ActionDate = dbResp.Rows[0]["ActionDate"]?.ToString()
                };
            }
            return new PromotionManagementCommon();
        }

        public List<PromotionManagementCommon> GetPromotionalImageLists()
        {
            var promotionManagements = new List<PromotionManagementCommon>();
            var sql = "Exec sproc_promotion_management @Flag='s'";
            var dt = _dao.ExecuteDataTable(sql);
            if (null != dt)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var common = new PromotionManagementCommon()
                    {
                        Id = item["Sno"].ToString(),
                        Title = item["Title"].ToString(),
                        Description = item["ImgDescription"].ToString(),
                        ImagePath = item["ImgPath"].ToString(),
                        IsDeleted = item["IsDeleted"].ToString(),
                        ActionUser = item["ActionUser"].ToString(),
                        ActionDate = item["ActionDate"].ToString(),
                    };
                    promotionManagements.Add(common);
                }
            }
            return promotionManagements;
        }
    }
}