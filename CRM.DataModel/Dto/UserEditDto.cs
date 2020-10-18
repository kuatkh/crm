using CRM.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class UserEditDto: UserDto
    {
        public long? CrmEmployeesId { get; set; }
        public long? CrmPatientsId { get; set; }
        public string Iin { get; set; }
        public string UserSecret { get; set; }
        public string UserSecretConfirmation { get; set; }
        public string RoleName { get; set; }
        public string DepartmentNameRu { get; set; }
        public string DepartmentNameKz { get; set; }
        public string DepartmentNameEn { get; set; }
        public string PositionNameRu { get; set; }
        public string PositionNameKz { get; set; }
        public string PositionNameEn { get; set; }
        public string CreatedDateTimeStr { get; set; }
        public string PhotoB64 { get; set; }
        public string PhotoPath { get; set; }
        public long? DictCitiesId { get; set; }
        public long? DictGendersId { get; set; }
        public long? DictEnterprisesId { get; set; }
        public long? DictDepartmentsId { get; set; }
        public long? DictPositionsId { get; set; }
        //public virtual DictEnterprises SelectedEnterprise { get; set; }
        //public virtual DictDepartments SelectedDepartment { get; set; }
        //public virtual DictPositions SelectedPosition { get; set; }
    }
}
