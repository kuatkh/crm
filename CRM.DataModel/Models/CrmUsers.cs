using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class CrmUsers : IdentityUser<long>
    {
        public long? CrmEmployeesId { get; set; }
        public long? CrmPatientsId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public virtual ICollection<CrmUserClaims> Claims { get; set; }
        public virtual ICollection<CrmUserLogins> Logins { get; set; }
        public virtual ICollection<CrmUserTokens> Tokens { get; set; }
        public virtual ICollection<CrmUserRoles> UserRoles { get; set; }

        [ForeignKey("CrmEmployeesId")]
        public virtual CrmEmployees CrmEmployee { get; set; }

        [ForeignKey("CrmPatientsId")]
        public virtual CrmPatients CrmPatient { get; set; }

        [InverseProperty("NotificationReceiver")]
        public virtual ICollection<Notifications> Notifications { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<CrmPatients> CrmPatientsAuthors { get; set; }

        [InverseProperty("Editor")]
        public virtual ICollection<CrmPatients> CrmPatientsEditors { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<CrmEmployeesWorkPlans> CrmSystemSettingsAuthors { get; set; }

        [InverseProperty("Editor")]
        public virtual ICollection<CrmEmployeesWorkPlans> CrmSystemSettingsEditors { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<CrmHolidays> CrmHolidaysAuthors { get; set; }

        [InverseProperty("Editor")]
        public virtual ICollection<CrmHolidays> CrmHolidaysEditors { get; set; }
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
