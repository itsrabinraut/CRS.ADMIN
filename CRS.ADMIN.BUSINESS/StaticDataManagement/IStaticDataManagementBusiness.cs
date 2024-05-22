﻿using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.StaticDataManagement;
using System.Collections.Generic;

namespace CRS.ADMIN.BUSINESS.StaticDataManagement
{
    public interface IStaticDataManagementBusiness
    {
        #region MANAGE STATIC DATA TYPE
        ManageStaticDataTypeCommon GetStaticDataTypeDetail(string id);
        List<StaticDataManagementCommon> GetStatiDataTypeList(string SearchText);
        CommonDbResponse ManageStaticDataType(ManageStaticDataTypeCommon commonModel);
        CommonDbResponse DeleteStaticDataType(ManageStaticDataTypeCommon request);
        #endregion
    }
}
