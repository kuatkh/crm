using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatientsLoyaltyPrograms
    {
        public long Id { get; set; }
        public long CrmPatientsId { get; set; }
        public long DictLoyaltyProgramsId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmPatientsId")]
        [InverseProperty("CrmPatientsLoyaltyPrograms")]
        public virtual CrmPatients CrmPatient { get; set; }

        [ForeignKey("DictLoyaltyProgramsId")]
        [InverseProperty("CrmPatientsLoyaltyPrograms")]
        public virtual DictLoyaltyPrograms DictLoyaltyProgram { get; set; }
    }
}
