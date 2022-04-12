using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmPatients
    {
        public long Id { get; set; }
        public long? CrmUsersId { get; set; }
        public string Iin { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public string AboutMe { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string JobPlace { get; set; }
        public long? DictGendersId { get; set; }
        public long? DictCitiesId { get; set; }
        [Column("BirthDate", TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        public string PhotoB64 { get; set; }
        public string PhotoPath { get; set; }
        public bool IsActive { get; set; } = true;
        public long? AuthorId { get; set; }
        public long? EditorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmUsersId")]
        public virtual CrmUsers CrmUser { get; set; }

        [ForeignKey("DictGendersId")]
        [InverseProperty("CrmPatients")]
        public virtual DictGenders DictGender { get; set; }

        [ForeignKey("DictCitiesId")]
        [InverseProperty("CrmPatients")]
        public virtual DictCities DictCity { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("CrmPatientsAuthors")]
        public virtual CrmUsers Author { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("CrmPatientsEditors")]
        public virtual CrmUsers Editor { get; set; }

        [InverseProperty("CrmPatient")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }

        [InverseProperty("CrmPatient")]
        public virtual ICollection<CrmPatientsIntolerances> CrmPatientsIntolerances { get; set; }

        [InverseProperty("CrmPatient")]
        public virtual ICollection<CrmPatientsLoyaltyPrograms> CrmPatientsLoyaltyPrograms { get; set; }

        [InverseProperty("CrmPatient")]
        public virtual ICollection<CrmPatientsAppointments> CrmPatientsAppointments { get; set; }
    }
}
