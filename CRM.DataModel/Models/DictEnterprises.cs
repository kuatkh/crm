using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictEnterprises
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("EnterpriseBranches")]
        public virtual DictEnterprises ParentEnterprise { get; set; }

        [InverseProperty("ParentEnterprise")]
        public virtual ICollection<DictEnterprises> EnterpriseBranches { get; set; }

        [InverseProperty("DictEnterprise")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }

        [InverseProperty("DictEnterpriseBranches")]
        public virtual ICollection<CrmEmployees> BranchesCrmEmployees { get; set; }

        [InverseProperty("DictEnterprise")]
        public virtual ICollection<DictDepartments> DictDepartments { get; set; }

        [InverseProperty("DictEnterprise")]
        public virtual ICollection<DictPositions> DictPositions { get; set; }

        [InverseProperty("DictEnterprise")]
        public virtual ICollection<DictLoyaltyPrograms> DictLoyaltyPrograms { get; set; }
    }
}
