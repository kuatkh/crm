using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmEmployeesWorkPlans
    {
        public long Id { get; set; }
        public long CrmEmployeesId { get; set; }
        public TimeSpan? WorkTimeFrom { get; set; }
        public TimeSpan? WorkTimeTo { get; set; }
        public TimeSpan? MondayWorkTimeFrom { get; set; }
        public TimeSpan? MondayWorkTimeTo { get; set; }
        public TimeSpan? TuesdayWorkTimeFrom { get; set; }
        public TimeSpan? TuesdayWorkTimeTo { get; set; }
        public TimeSpan? WebnesdayWorkTimeFrom { get; set; }
        public TimeSpan? WebnesdayWorkTimeTo { get; set; }
        public TimeSpan? ThursdayWorkTimeFrom { get; set; }
        public TimeSpan? ThursdayWorkTimeTo { get; set; }
        public TimeSpan? FridayWorkTimeFrom { get; set; }
        public TimeSpan? FridayWorkTimeTo { get; set; }
        public TimeSpan? SaturdayWorkTimeFrom { get; set; }
        public TimeSpan? SaturdayWorkTimeTo { get; set; }
        public TimeSpan? SundayWorkTimeFrom { get; set; }
        public TimeSpan? SundayWorkTimeTo { get; set; }
        public int? WorkPeriodInDays { get; set; }
        public int? DutyPeriodInDays { get; set; }
        [Column("BirthDate", TypeName = "date")]
        public DateTime? DutyDate { get; set; }
        public string Description { get; set; }
        public long AuthorId { get; set; }
        public long? EditorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmEmployeesId")]
        [InverseProperty("CrmEmployeesWorkPlans")]
        public virtual CrmEmployees CrmEmployee { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("CrmSystemSettingsAuthors")]
        public virtual CrmUsers Author { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("CrmSystemSettingsEditors")]
        public virtual CrmUsers Editor { get; set; }
    }
}
