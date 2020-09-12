using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmHolidays
    {
        public long Id { get; set; }
        public int HolidayDay { get; set; }
        public int HolidayMonth { get; set; }
        public int? HolidayYear { get; set; }
        public bool IsWork { get; set; } = false;
        public bool IsRepeatYearly { get; set; } = false;
        public string Description { get; set; }
        public long AuthorId { get; set; }
        public long? EditorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("CrmHolidaysAuthors")]
        public virtual CrmUsers Author { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("CrmHolidaysEditors")]
        public virtual CrmUsers Editor { get; set; }
    }
}
