using CRS.ADMIN.APPLICATION.Models.PaginationManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PaginationManagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRS.ADMIN.APPLICATION.Models.PointsManagement
{
    public class PointsManagementModel
    {
        public List<PointsTansferReportModel> PointsTansferReportList = new List<PointsTansferReportModel>();
        public PointsTansferModel ManagePointsTansfer { get; set; }
        public PointsRequestModel ManagePointsRequest { get; set; }
        public string ListType { get; set; }
        public string SearchFilter { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string TransferTypeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public PointRequestCommonModel PointRequestCommonModel { get; set; } = new PointRequestCommonModel();
        public PointBalanceStatementRequestModel PointBalanceStatementRequest { get; set; } = new PointBalanceStatementRequestModel();
        public SystemTransferRequestModel SystemTransferModel { get; set; } = new SystemTransferRequestModel();
    }

    public class PointsTansferModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string UserTypeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string TransferType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The field must be a number.")]
        public string Points { get; set; }
        public string Remarks { get; set; }
        public string Image { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
    }

    public class PointsTansferReportModel
    {
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string UserType { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Points { get; set; }
        public string Remarks { get; set; }
        public string TransferTypeId { get; set; }
        public string UserTypeId { get; set; }
    }

    public class PointRequestCommonModel
    {
        public string SearchFilter { get; set; }
        public string LocationId { get; set; }
        public string ClubName { get; set; }
        public string PaymentMethodId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public PointsRequestModel PointsRequestModel { get; set; } = new PointsRequestModel();
        public List<PointRequestsListModel> PointRequestsListModel { get; set; } = new List<PointRequestsListModel>();
    }
    public class PointsRequestModel
    {
        public string TransactionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The field must be a number.")]
        public string Points { get; set; }
        public string Remarks { get; set; }
        public string ActionIp { get; set; }
        public string ActionUser { get; set; }
    }

    public class PointRequestsListModel
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public string RequestId { get; set; }
        public string RequestedDate { get; set; }
        public string ClubName { get; set; }
        public string PaymentMethod { get; set; }
        public string AmountTransferred { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string RowsTotal { get; set; }
    }

    public class PointBalanceStatementRequestModel
    {
        public string SearchFilter { get; set; }
        public string UserTypeList { get; set; }
        public string UserNameList { get; set; }
        public string TransferTypeList { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public string userid { get; set; }
        public int StartIndex2 { get; set; } = 0;
        public int PageSize2 { get; set; } = 10;


    }
    public class PointBalanceStatementResponseModel : PaginationResponseCommon
    {
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string User { get; set; }
        public string TotalPrice { get; set; }
        public string TotalCommission { get; set; }
        public string Remarks { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string RowTotal { get; set; }

    }

    public class SystemTransferRequestModel
    {

        public string SearchFilter { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string TransferType { get; set; }
        public string From_Date1 { get; set; }
        public string To_Date1 { get; set; }
        public int StartIndex3 { get; set; } = 0;
        public int PageSize3 { get; set; } = 10;

    }
    public class SystemTransferReponseModel
    {

        public string TransactionId { get; set; }
        public int SNO { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Points { get; set; }
        public string Remarks { get; set; }
    }

}