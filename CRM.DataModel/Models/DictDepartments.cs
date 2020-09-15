using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictDepartments
    {
        public long Id { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public long? DictEnterprisesId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("DictEnterprisesId")]
        [InverseProperty("DictDepartments")]
        public virtual DictEnterprises DictEnterprise { get; set; }

        [InverseProperty("DictDepartment")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }

        [InverseProperty("DictDepartment")]
        public virtual ICollection<DictServices> DictServices { get; set; }
    }
}
