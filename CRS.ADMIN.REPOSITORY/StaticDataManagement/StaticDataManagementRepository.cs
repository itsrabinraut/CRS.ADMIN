﻿using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PaginationManagement;
using CRS.ADMIN.SHARED.StaticDataManagement;
using DocumentFormat.OpenXml.Office2016.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CRS.ADMIN.REPOSITORY.StaticDataManagement
{
    public class StaticDataManagementRepository : IStaticDataManagementRepository
    {
        RepositoryDao _dao;
        public StaticDataManagementRepository()
        {
            _dao = new RepositoryDao();
        }

        #region MANAGE STATIC DATA TYPE
        public CommonDbResponse DeleteStaticDataType(ManageStaticDataTypeCommon request)
        {
            string sp_name = "sproc_tbl_static_data_Type_Delete ";
            sp_name += "@Id=" + _dao.FilterString(request.Id);
            sp_name += ",@ActionUser=" + _dao.FilterString(request.ActionUser);
            return _dao.ParseCommonDbResponse(sp_name);
        }

        public ManageStaticDataTypeCommon GetStaticDataTypeDetail(string id)
        {
            string sp_name = "EXEC sproc_tbl_static_data_Type_Select ";
            sp_name += "@Id=" + _dao.FilterString(id);
            var dbResponseInfo = _dao.ExecuteDataRow(sp_name);
            if (dbResponseInfo != null)
            {
                return new ManageStaticDataTypeCommon()
                {
                    Id = _dao.ParseColumnValue(dbResponseInfo, "Id").ToString(),
                    StaticDataType = _dao.ParseColumnValue(dbResponseInfo, "StaticDataType").ToString(),
                    StaticDataName = _dao.ParseColumnValue(dbResponseInfo, "StaticDataName").ToString(),
                    StaticDataDescription = _dao.ParseColumnValue(dbResponseInfo, "StaticDataDescription").ToString(),
                    Status = _dao.ParseColumnValue(dbResponseInfo, "Status").ToString()
                };
            }
            return new ManageStaticDataTypeCommon();
        }

        public List<StaticDataTypeCommon> GetStatiDataTypeList(PaginationFilterCommon dbRequest)
        {
            string sp_name = "EXEC sproc_tbl_static_data_type_list ";
            sp_name += "@SearchFilter=" + _dao.FilterString(dbRequest.SearchFilter);
            sp_name += ",@Skip=" + dbRequest.Skip;
            sp_name += ",@Take=" + dbRequest.Take;
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<StaticDataTypeCommon>(dbResponseInfo).ToList();
            return new List<StaticDataTypeCommon>();
        }

        public CommonDbResponse ManageStaticDataType(ManageStaticDataTypeCommon commonModel)
        {
            string sp_name = "EXEC sproc_tbl_static_data_Type_InserUpdate ";
            sp_name += "@Id=" + _dao.FilterString(commonModel.Id);
            sp_name += ",@StaticDataType=" + _dao.FilterString(commonModel.StaticDataType);
            sp_name += ",@StaticDataName=N" + _dao.FilterString(commonModel.StaticDataName);
            sp_name += ",@StaticDataDescription=N" + _dao.FilterString(commonModel.StaticDataDescription);
            sp_name += ",@Status=" + _dao.FilterString("A");
            sp_name += ",@ActionUser=" + _dao.FilterString(commonModel.ActionUser);
            return _dao.ParseCommonDbResponse(sp_name);
        }
        #endregion

        #region MANAGE STATIC DATA
        public List<StaticDataModelCommon> GetStaticDataList(string staticDataTypeId)
        {
            string sp_name = "EXEC sproc_admin__manage_static_data @Flag='gsdl'";
            sp_name += ",@StaticDataType=" + _dao.FilterString(staticDataTypeId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<StaticDataModelCommon>(dbResponseInfo).ToList();
            return new List<StaticDataModelCommon>();
        }

        public ManageStaticDataCommon GetStaticDataDetail(string id)
        {
            string sp_name = "EXEC sproc_admin__manage_static_data @Flag='gsdd'";
            sp_name += ",@Id=" + _dao.FilterString(id);
            var dbResponse = _dao.ExecuteDataRow(sp_name);
            if (dbResponse != null)
            {
                return new ManageStaticDataCommon()
                {
                    Id = _dao.ParseColumnValue(dbResponse, "Id").ToString(),
                    StaticDataType = _dao.ParseColumnValue(dbResponse, "StaticDataType").ToString(),
                    StaticDataLabel = _dao.ParseColumnValue(dbResponse, "StaticDataLabel").ToString(),
                    StaticDataDescription = _dao.ParseColumnValue(dbResponse, "StaticDataDescription").ToString(),
                    Status = _dao.ParseColumnValue(dbResponse, "Status").ToString(),
                    StaticDataValue = _dao.ParseColumnValue(dbResponse, "StaticDataValue").ToString(),
                    AdditionalValue1 = _dao.ParseColumnValue(dbResponse, "AdditionalValue1").ToString(),
                    AdditionalValue2 = _dao.ParseColumnValue(dbResponse, "AdditionalValue2").ToString(),
                    AdditionalValue3 = _dao.ParseColumnValue(dbResponse, "AdditionalValue3").ToString(),
                    AdditionalValue4 = _dao.ParseColumnValue(dbResponse, "AdditionalValue4").ToString(),
                    InputType = _dao.ParseColumnValue(dbResponse, "InputType").ToString(),
                };
            }
            return new ManageStaticDataCommon();
        }

        public CommonDbResponse ManageStaticData(ManageStaticDataCommon commonModel)
        {
            string sp_name = "EXEC sproc_admin__manage_static_data ";
            sp_name += string.IsNullOrEmpty(commonModel.Id) ? "@Flag='msd'" : "@Flag='umsd'";
            sp_name += ",@Id=" + _dao.FilterString(commonModel.Id);
            sp_name += ",@StaticDataLabel=" + (!string.IsNullOrEmpty(commonModel.StaticDataLabel) ? "N" + _dao.FilterString(commonModel.StaticDataLabel) : _dao.FilterString(commonModel.StaticDataLabel));
            sp_name += ",@StaticDataDescription=" + (!string.IsNullOrEmpty(commonModel.StaticDataDescription) ? "N" + _dao.FilterString(commonModel.StaticDataDescription) : _dao.FilterString(commonModel.StaticDataDescription));
            sp_name += ",@StaticDataType=" + _dao.FilterString(commonModel.StaticDataType);
            sp_name += ",@StaticDataValue=" + _dao.FilterString(commonModel.StaticDataValue);
            sp_name += ",@AdditionalValue1=" + (!string.IsNullOrEmpty(commonModel.AdditionalValue1) ? "N" + _dao.FilterString(commonModel.AdditionalValue1) : _dao.FilterString(commonModel.AdditionalValue1));
            sp_name += ",@AdditionalValue2=" + (!string.IsNullOrEmpty(commonModel.AdditionalValue2) ? "N" + _dao.FilterString(commonModel.AdditionalValue2) : _dao.FilterString(commonModel.AdditionalValue2));
            sp_name += ",@AdditionalValue3=" + (!string.IsNullOrEmpty(commonModel.AdditionalValue3) ? "N" + _dao.FilterString(commonModel.AdditionalValue3) : _dao.FilterString(commonModel.AdditionalValue3));
            sp_name += ",@AdditionalValue4=" + (!string.IsNullOrEmpty(commonModel.AdditionalValue4) ? "N" + _dao.FilterString(commonModel.AdditionalValue4) : _dao.FilterString(commonModel.AdditionalValue4));
            sp_name += ",@InputType=" + (!string.IsNullOrEmpty(commonModel.InputType) ? "N" + _dao.FilterString(commonModel.InputType) : _dao.FilterString(commonModel.InputType));
            sp_name += ",@Status=" + _dao.FilterString("A");
            sp_name += ",@ActionUser=" + _dao.FilterString(commonModel.ActionUser);
            return _dao.ParseCommonDbResponse(sp_name);
        }

        public CommonDbResponse DeleteStaticData(ManageStaticDataCommon request)
        {
            string sp_name = "EXEC sproc_tbl_static_data_Delete ";
            sp_name += "@Id=" + _dao.FilterString(request.Id);
            sp_name += ",@ActionUser=" + _dao.FilterString(request.ActionUser);
            return _dao.ParseCommonDbResponse(sp_name);
        }
        #endregion
    }
}
