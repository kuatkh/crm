using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictPositions
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionKz { get; set; }
        public string DescriptionEn { get; set; }
        public string Category { get; set; }
        public long? DictEnterprisesId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("DictEnterprisesId")]
        [InverseProperty("DictPositions")]
        public virtual DictEnterprises DictEnterprise { get; set; }

        [InverseProperty("DictPosition")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }
    }
}
