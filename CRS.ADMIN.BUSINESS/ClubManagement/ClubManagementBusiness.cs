﻿using CRS.ADMIN.REPOSITORY.ClubManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.ClubManagement;
using CRS.ADMIN.SHARED.PaginationManagement;
using System.Collections.Generic;

namespace CRS.ADMIN.BUSINESS.ClubManagement
{
    public class ClubManagementBusiness : IClubManagementBusiness
    {
        IClubManagementRepository _REPO;
        public ClubManagementBusiness(ClubManagementRepository REPO)
        {
            _REPO = REPO;
        }

        public ClubDetailCommon GetClubDetails(string AgentId)
        {
            return _REPO.GetClubDetails(AgentId);
        }

        public List<ClubListCommon> GetClubList(PaginationFilterCommon Request)
        {
            return _REPO.GetClubList(Request);
        }

        public CommonDbResponse ManageClub(ManageClubCommon Request)
        {
            return _REPO.ManageClub(Request);
        }

        public CommonDbResponse ManageClubStatus(string AgentId, string Status, Common Request)
        {
            return _REPO.ManageClubStatus(AgentId, Status, Request);
        }

        public CommonDbResponse ResetClubUserPassword(string AgentId, string UserId, Common Request)
        {
            return _REPO.ResetClubUserPassword(AgentId, UserId, Request);
        }

        #region "Manage Tag
        public CommonDbResponse ManageTag(ManageTagCommon Request)
        {
            return _REPO.ManageTag(Request);
        }
        public List<LocationListCommon> GetLocationDDL(string clubID)
        {
            return _REPO.GetLocationDDL(clubID);
        }
        public ManageTagCommon GetTagDetails(string clubid)
        {
            return _REPO.GetTagDetails(clubid);
        }
        #endregion

        #region Manage gallery
        public List<GalleryManagementCommon> GetGalleryImage(string AgentId, PaginationFilterCommon request, string GalleryId = "")
        {
            return _REPO.GetGalleryImage(AgentId, request, GalleryId);
        }
        public CommonDbResponse ManageGalleryImage(ManageGalleryImageCommon Request)
        {
            return _REPO.ManageGalleryImage(Request);
        }
        public CommonDbResponse ManageGalleryImageStatus(string AgentId, string GalleryId, Common Request)
        {
            return _REPO.ManageGalleryImageStatus(AgentId, GalleryId, Request);
        }
        #endregion
    }
}
