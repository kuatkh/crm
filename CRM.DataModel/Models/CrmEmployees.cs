using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmEmployees
    {
        public long Id { get; set; }
        public long CrmUsersId { get; set; }
        public string Iin { get; set; }
        public string DocumentNumber { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string SurnameRu { get; set; }
        public string SurnameKz { get; set; }
        public string SurnameEn { get; set; }
        public string MiddlenameRu { get; set; }
        public string MiddlenameKz { get; set; }
        public string MiddlenameEn { get; set; }
        public string AboutMe { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public long? DictGendersId { get; set; }
        public long? DictEnterprisesId { get; set; }
        public long? DictEnterpriseBranchesId { get; set; }
        public long? DictDepartmentsId { get; set; }
        public long? DictPositionsId { get; set; }
        public long? DictCitiesId { get; set; }
        [Column("BirthDate", TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        public string PhotoB64 { get; set; }
        public string PhotoPath { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("CrmUsersId")]
        [InverseProperty("CrmEmployee")]
        public virtual CrmUsers CrmUser { get; set; }

        [ForeignKey("DictGendersId")]
        [InverseProperty("CrmEmployees")]
        public virtual DictGenders DictGender { get; set; }

        [ForeignKey("DictEnterprisesId")]
        [InverseProperty("CrmEmployees")]
        public virtual DictEnterprises DictEnterprise { get; set; }

        [ForeignKey("DictEnterpriseBranchesId")]
        [InverseProperty("BranchesCrmEmployees")]
        public virtual DictEnterprises DictEnterpriseBranches { get; set; }

        [ForeignKey("DictDepartmentsId")]
        [InverseProperty("CrmEmployees")]
        public virtual DictDepartments DictDepartment { get; set; }

        [ForeignKey("DictPositionsId")]
        [InverseProperty("CrmEmployees")]
        public virtual DictPositions DictPosition { get; set; }

        [ForeignKey("DictCitiesId")]
        [InverseProperty("CrmEmployees")]
        public virtual DictCities DictCity { get; set; }

        [InverseProperty("CrmEmployee")]
        public virtual ICollection<CrmEmployeesWorkPlans> CrmEmployeesWorkPlans { get; set; }

        [InverseProperty("ToCrmEmployee")]
        public virtual ICollection<CrmPatientsAppointments> CrmPatientsAppointments { get; set; }
    }
}
