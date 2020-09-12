using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class Cards
    {
        public long Id { get; set; }
        public long CrmClientsId { get; set; }
        public long AuthorId { get; set; }
        public long? EditorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmClientsId")]
        [InverseProperty("Cards")]
        public virtual CrmClients CrmClient { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("CardsAuthors")]
        public virtual CrmUsers Author { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("CardsEditors")]
        public virtual CrmUsers Editor { get; set; }
    }
}
