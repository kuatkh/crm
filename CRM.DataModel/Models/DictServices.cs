using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictServices
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public long? DictDepartmentsId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("DictDepartmentsId")]
        [InverseProperty("DictServices")]
        public virtual DictDepartments DictDepartment { get; set; }

        [InverseProperty("DictService")]
        public virtual ICollection<CrmPatientsAppointmentsServices> CrmPatientsAppointmentsServices { get; set; }

    }
}
