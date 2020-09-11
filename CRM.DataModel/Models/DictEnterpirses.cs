using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictEnterpirses
    {
        public long Id { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [InverseProperty("DictEnterpirse")]
        public virtual ICollection<CrmUsers> Users { get; set; }

        [InverseProperty("DictEnterpirse")]
        public virtual ICollection<DictDepartments> DictDepartments { get; set; }

        [InverseProperty("DictEnterpirse")]
        public virtual ICollection<DictPositions> DictPositions { get; set; }
    }
}
