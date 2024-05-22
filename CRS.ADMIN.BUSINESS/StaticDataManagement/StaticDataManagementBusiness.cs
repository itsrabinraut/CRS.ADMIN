﻿using CRS.ADMIN.REPOSITORY.StaticDataManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.StaticDataManagement;
using System.Collections.Generic;

namespace CRS.ADMIN.BUSINESS.StaticDataManagement
{
    public class StaticDataManagementBusiness : IStaticDataManagementBusiness
    {
        private readonly IStaticDataManagementRepository _repo;
        public StaticDataManagementBusiness(StaticDataManagementRepository repo)
        {
            _repo = repo;
        }
        #region MANAGE STATIC DATA TYPE
        public CommonDbResponse DeleteStaticDataType(ManageStaticDataTypeCommon request)
        {
            return _repo.DeleteStaticDataType(request);
        }

        public ManageStaticDataTypeCommon GetStaticDataTypeDetail(string id)
        {
            return _repo.GetStaticDataTypeDetail(id);
        }

        public List<StaticDataManagementCommon> GetStatiDataTypeList(string SearchText = "")
        {
            return _repo.GetStatiDataTypeList(SearchText);
        }

        public CommonDbResponse ManageStaticDataType(ManageStaticDataTypeCommon commonModel)
        {
           return _repo.ManageStaticDataType(commonModel);
        }
        #endregion
    }
}
