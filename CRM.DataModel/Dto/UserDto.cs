﻿using CRM.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public long? CrmEmployeesId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string Iin { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string SurnameRu { get; set; }
        public string SurnameKz { get; set; }
        public string SurnameEn { get; set; }
        public string MiddlenameRu { get; set; }
        public string MiddlenameKz { get; set; }
        public string MiddlenameEn { get; set; }
        public string FullNameRu { get; set; }
        public string FullNameKz { get; set; }
        public string FullNameEn { get; set; }
        public string ShortNameRu { get; set; }
        public string ShortNameKz { get; set; }
        public string ShortNameEn { get; set; }
        public string AboutMe { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        //public string JobPlace { get; set; }
        public long RoleId { get; set; }
        //public string CrmToken { get; set; }
        //public DateTime? CrmTokenExpiredDate { get; set; }
        public DateTime? BirthDate { get; set; }
        //public string BirthDateStr { get; set; }
        //public string PhotoB64 { get; set; }
        //public string PhotoPath { get; set; }
        public bool IsActive { get; set; } = true;
        public SelectDto Gender { get; set; }
        public SelectDto Position { get; set; }
        public SelectDto Department { get; set; }
        public SelectDto Enterprise { get; set; }
        public SelectDto City { get; set; }
    }
}
