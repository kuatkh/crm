using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class Notifications
    {
        public long Id { get; set; }
        public long ReceiverId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("Notifications")]
        public virtual CrmUsers NotificationReceiver { get; set; }
    }
}
