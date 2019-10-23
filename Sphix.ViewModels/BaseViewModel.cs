using System;

namespace Sphix.ViewModels
{
    public class BaseModel: ResultMessage
    {
        public Int64 Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
        public Int32 TotalCount { get; set; }
        public long LogedInUserId { get; set; }
    }
    public class ResultMessage
    {
        public bool Status { get; set; }
        public string Messsage { get; set; }
        public string Data { get; set; }
    }
    public class SearchFilter
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string PageNumber { get; set; }
        public string PageLimit { get; set; }
        public string OrderBy { get; set; }
        public string SearchValue { get; set; }
    }
   
}
