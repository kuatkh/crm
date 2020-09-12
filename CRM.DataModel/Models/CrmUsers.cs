using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmUsers : IdentityUser<long>
    {
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
        public string AboutMe { get; set; }
        public string JobPlace { get; set; }
        public long? DictGendersId { get; set; }
        public long? DictEnterprisesId { get; set; }
        public long? DictEnterpriseBranchesId { get; set; }
        public long? DictDepartmentsId { get; set; }
        public long? DictPositionsId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhotoB64 { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public virtual ICollection<CrmUserClaims> Claims { get; set; }
        public virtual ICollection<CrmUserLogins> Logins { get; set; }
        public virtual ICollection<CrmUserTokens> Tokens { get; set; }
        public virtual ICollection<CrmUserRoles> UserRoles { get; set; }

        [ForeignKey("DictGendersId")]
        [InverseProperty("Users")]
        public virtual DictGenders DictGender { get; set; }

        [ForeignKey("DictEnterpriseBranchesId")]
        [InverseProperty("Users")]
        public virtual DictEnterpriseBranches DictEnterpriseBranche { get; set; }

        [ForeignKey("DictEnterprisesId")]
        [InverseProperty("Users")]
        public virtual DictEnterpirses DictEnterpirse { get; set; }

        [ForeignKey("DictDepartmentsId")]
        [InverseProperty("Users")]
        public virtual DictDepartments DictDepartment { get; set; }

        [ForeignKey("DictPositionsId")]
        [InverseProperty("Users")]
        public virtual DictPositions DictPosition { get; set; }

        [InverseProperty("NotificationReceiver")]
        public virtual ICollection<Notifications> Notifications { get; set; }

        [InverseProperty("CrmUser")]
        public virtual ICollection<CrmClients> CrmClients { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<CrmClients> CrmClientsAuthors { get; set; }

        [InverseProperty("Editor")]
        public virtual ICollection<CrmClients> CrmClientsEditors { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<Cards> CardsAuthors { get; set; }

        [InverseProperty("Editor")]
        public virtual ICollection<Cards> CardsEditors { get; set; }

    }

    public class CrmRoles : IdentityRole<long>
    {
        public string Description { get; set; }

        public virtual ICollection<CrmUserRoles> UserRoles { get; set; }
        public virtual ICollection<CrmRoleClaims> RoleClaims { get; set; }
    }

    public class CrmUserRoles : IdentityUserRole<long>
    {
        public virtual CrmUsers User { get; set; }
        public virtual CrmRoles Role { get; set; }
    }

    public class CrmUserClaims : IdentityUserClaim<long>
    {
        public virtual CrmUsers User { get; set; }
    }

    public class CrmUserLogins : IdentityUserLogin<long>
    {
        public virtual CrmUsers User { get; set; }
    }

    public class CrmRoleClaims : IdentityRoleClaim<long>
    {
        public virtual CrmRoles Role { get; set; }
    }

    public class CrmUserTokens : IdentityUserToken<long>
    {
        public virtual CrmUsers User { get; set; }
    }
}
