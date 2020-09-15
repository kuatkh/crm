using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class FilterDto
    {
        public int? StatusType { get; set; }
        public long? DepartmentId { get; set; }
        public string DictionaryName { get; set; }
        public bool filterByCreatedDate { get; set; } = false;
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? CardId { get; set; }
        public bool IsAgreement { get; set; } = false;

        public int Page { get; set; }
        public int RowsPerPage { get; set; }
        public string Order { get; set; }
        public string OrderBy { get; set; }
    }
}
