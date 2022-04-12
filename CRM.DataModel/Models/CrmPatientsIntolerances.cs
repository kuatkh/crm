using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatientsIntolerances
    {
        public long Id { get; set; }
        public long CrmPatientsId { get; set; }
        public long? DictIntolerancesId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmPatientsId")]
        [InverseProperty("CrmPatientsIntolerances")]
        public virtual CrmPatients CrmPatient { get; set; }

        [ForeignKey("DictIntolerancesId")]
        [InverseProperty("CrmPatientsIntolerances")]
        public virtual DictIntolerances DictIntolerance { get; set; }
    }
}
