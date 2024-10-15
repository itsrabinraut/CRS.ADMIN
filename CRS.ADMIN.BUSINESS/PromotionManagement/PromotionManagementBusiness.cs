﻿using CRS.ADMIN.REPOSITORY.PromotionManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PaginationManagement;
using CRS.ADMIN.SHARED.PromotionManagement;
using System.Collections.Generic;

namespace CRS.ADMIN.BUSINESS.PromotionManagement
{
    public class PromotionManagementBusiness : IPromotionManagementBusiness
    {
        private readonly IPromotionManagementRepository _repo;
        public PromotionManagementBusiness(PromotionManagementRepository repo) => _repo = repo;

        public CommonDbResponse AddPromotionalImage(PromotionManagementCommon promotion)
        {
            return _repo.AddPromotionalImage(promotion);
        }

        public CommonDbResponse DeletePromotionalImage(PromotionManagementCommon promotion)
        {
            return _repo.DeletePromotionalImage(promotion);
        }

        public CommonDbResponse EditPromotionalImage(PromotionManagementCommon promotion)
        {
            return _repo.EditPromotionalImage(promotion);
        }

        public PromotionManagementCommon GetPromotionalImageById(string Id)
        {
            return _repo.GetPromotionalImageById(Id);
        }

        public List<PromotionManagementCommon> GetPromotionalImageLists(PaginationFilterCommon Request)
        {
            return _repo.GetPromotionalImageLists(Request);
        }
        public List<AdvertisementManagementCommon> GetAdvertisementImageLists(PaginationFilterCommon Request)
        {
            return _repo.GetAdvertisementImageLists(Request);
        }
        public AdvertisementDetailCommon GetAdvertisementImageById(string Id)
        {
            return _repo.GetAdvertisementImageById(Id);
        }
        public CommonDbResponse UpdateAdvertisementImage(AdvertisementDetailCommon promotion)
        {
            return _repo.UpdateAdvertisementImage(promotion);
        } 
        public CommonDbResponse BlockUnblockAdvertisementImage(AdvertisementDetailCommon promotion)
        {
            return _repo.BlockUnblockAdvertisementImage(promotion);
        }
    }
}