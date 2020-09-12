using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictLoyaltyPrograms
    {
        public long Id { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionKz { get; set; }
        public string DescriptionEn { get; set; }
        public float DiscountAmount { get; set; }
        public long? DictEnterprisesId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("DictEnterprisesId")]
        [InverseProperty("DictLoyaltyPrograms")]
        public virtual DictEnterprises DictEnterprise { get; set; }

        [InverseProperty("DictLoyaltyProgram")]
        public virtual ICollection<CrmPatientsLoyaltyPrograms> CrmPatientsLoyaltyPrograms { get; set; }
    }
}
