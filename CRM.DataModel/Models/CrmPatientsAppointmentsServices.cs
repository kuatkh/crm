using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatientsAppointmentsServices
    {
        public long Id { get; set; }
        public long CrmPatientsAppointmentsId { get; set; }
        public long? DictServicesId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmPatientsAppointmentsId")]
        [InverseProperty("CrmPatientsAppointmentsServices")]
        public virtual CrmPatientsAppointments CrmPatientsAppointment { get; set; }

        [ForeignKey("DictServicesId")]
        [InverseProperty("CrmPatientsAppointmentsServices")]
        public virtual DictServices DictService { get; set; }
    }
}
