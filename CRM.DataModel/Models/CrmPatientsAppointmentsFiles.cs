using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatientsAppointmentsFiles
    {
        public long Id { get; set; }
        public long CrmPatientsAppointmentsId { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmPatientsAppointmentsId")]
        [InverseProperty("CrmPatientsAppointmentsFiles")]
        public virtual CrmPatientsAppointments CrmPatientsAppointment { get; set; }
    }
}
