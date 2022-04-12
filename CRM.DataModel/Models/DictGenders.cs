using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictGenders
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [InverseProperty("DictGender")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }

        [InverseProperty("DictGender")]
        public virtual ICollection<CrmPatients> CrmPatients { get; set; }
    }
}
