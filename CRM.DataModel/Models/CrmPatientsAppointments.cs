﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatientsAppointments
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public long CrmPatientsId { get; set; }
        public long ToCrmEmployeesId { get; set; }
        public long DictStatusesId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Complain { get; set; }
        public string DoctorsAppointment { get; set; }
        public float ServicePrice { get; set; }
        public bool? IsOutOfLine { get; set; }
        public string OutOfLineReason { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("ChildCrmPatientsAppointments")]
        public virtual CrmPatientsAppointments ParentCrmPatientsAppointment { get; set; }

        [InverseProperty("ParentCrmPatientsAppointment")]
        public virtual ICollection<CrmPatientsAppointments> ChildCrmPatientsAppointments { get; set; }

        [ForeignKey("CrmPatientsId")]
        [InverseProperty("CrmPatientsAppointments")]
        public virtual CrmPatients CrmPatient { get; set; }

        [ForeignKey("ToCrmEmployeesId")]
        [InverseProperty("CrmPatientsAppointments")]
        public virtual CrmEmployees ToCrmEmployee { get; set; }

        [ForeignKey("DictStatusesId")]
        [InverseProperty("CrmPatientsAppointments")]
        public virtual DictStatuses DictStatus { get; set; }

        [InverseProperty("CrmPatientsAppointment")]
        public virtual ICollection<CrmPatientsAppointmentsServices> CrmPatientsAppointmentsServices { get; set; }

        [InverseProperty("CrmPatientsAppointment")]
        public virtual ICollection<CrmPatientsAppointmentsFiles> CrmPatientsAppointmentsFiles { get; set; }
    }
}
