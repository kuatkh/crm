using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmClients
    {
        public long Id { get; set; }
        public long? CrmUsersId { get; set; }
        public string Iin { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string SurnameRu { get; set; }
        public string SurnameKz { get; set; }
        public string SurnameEn { get; set; }
        public string MiddlenameRu { get; set; }
        public string MiddlenameKz { get; set; }
        public string MiddlenameEn { get; set; }
        public string JobPlace { get; set; }
        public long? DictGendersId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhotoB64 { get; set; }
        public string PhotoPath { get; set; }
        public long? AuthorId { get; set; }
        public long? EditorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmUsersId")]
        [InverseProperty("CrmClients")]
        public virtual CrmUsers CrmUser { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("CrmClientsAuthors")]
        public virtual CrmUsers Author { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("CrmClientsEditors")]
        public virtual CrmUsers Editor { get; set; }

        [InverseProperty("CrmClient")]
        public virtual ICollection<Cards> Cards { get; set; }
    }
}
