using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CRM.Services.Helpers
{
    public class UserHelper
    {
        public static async Task<UserDto> GetCurrentUser(CrmDbContext context, string username)
        {
            var currentUser = await context.Users
                .Include("CrmEmployee")
                .Include("CrmPatient")
                .Include("UserRoles")
                //.Include("CrmEmployee.DictEnterprise")
                //.Include("CrmEmployee.DictEnterpriseBranches")
                //.Include("CrmEmployee.DictDepartment")
                //.Include("CrmEmployee.DictPosition")
                //.Include("CrmEmployee.DictCity")
                //.Include("CrmPatient.DictCity")
                .Where(u => u.UserName == username && u.DeletedDateTime == null && 
                    (u.CrmEmployee != null && u.CrmEmployee.DeletedDateTime == null && u.CrmEmployee.IsActive == true || u.CrmPatient != null && u.CrmPatient.DeletedDateTime == null && u.CrmPatient.IsActive == true))
                .AsNoTracking()
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    CrmEmployeesId = u.CrmEmployeesId,
                    //CrmPatientsId = u.CrmPatientsId,
                    //Iin = u.CrmEmployee != null ? u.CrmEmployee.Iin : u.CrmPatient != null ? u.CrmPatient.Iin : null,
                    BirthDate = u.CrmEmployee != null ? u.CrmEmployee.BirthDate : u.CrmPatient != null ? u.CrmPatient.BirthDate : null,
                    MiddlenameKz = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameKz : u.CrmPatient != null ? u.CrmPatient.MiddlenameKz : null,
                    MiddlenameRu = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameRu : u.CrmPatient != null ? u.CrmPatient.MiddlenameRu : null,
                    MiddlenameEn = u.CrmEmployee != null ? u.CrmEmployee.MiddlenameEn : u.CrmPatient != null ? u.CrmPatient.MiddlenameEn : null,
                    NameKz = u.CrmEmployee != null ? u.CrmEmployee.NameKz : u.CrmPatient != null ? u.CrmPatient.NameKz : null,
                    NameRu = u.CrmEmployee != null ? u.CrmEmployee.NameRu : u.CrmPatient != null ? u.CrmPatient.NameRu : null,
                    NameEn = u.CrmEmployee != null ? u.CrmEmployee.NameEn : u.CrmPatient != null ? u.CrmPatient.NameEn : null,
                    SurnameKz = u.CrmEmployee != null ? u.CrmEmployee.SurnameKz : u.CrmPatient != null ? u.CrmPatient.SurnameKz : null,
                    SurnameRu = u.CrmEmployee != null ? u.CrmEmployee.SurnameRu : u.CrmPatient != null ? u.CrmPatient.SurnameRu : null,
                    SurnameEn = u.CrmEmployee != null ? u.CrmEmployee.SurnameEn : u.CrmPatient != null ? u.CrmPatient.SurnameEn : null,
                    //DictGendersId = u.CrmEmployee != null
                    //    ? u.CrmEmployee.DictGendersId
                    //    : u.CrmPatient != null && u.CrmPatient.DictGendersId != null
                    //        ? u.CrmPatient.DictGendersId
                    //        : null,
                    //DictEnterprisesId = u.CrmEmployee != null ? u.CrmEmployee.DictEnterprisesId : null,
                    //DictPositionsId = u.CrmEmployee != null ? u.CrmEmployee.DictPositionsId : null,
                    //DictDepartmentsId = u.CrmEmployee != null ? u.CrmEmployee.DictDepartmentsId : null,
                    IsActive = u.CrmEmployee != null ? u.CrmEmployee.IsActive : u.CrmPatient != null ? u.CrmPatient.IsActive : false,
                    //BirthDateStr = u.CrmEmployee != null && u.CrmEmployee.BirthDate != null 
                    //    ? u.CrmEmployee.BirthDate.Value.ToString("dd.MM.yyyy") 
                    //    : u.CrmPatient != null && u.CrmPatient.BirthDate != null 
                    //        ? u.CrmPatient.BirthDate.Value.ToString("dd.MM.yyyy") 
                    //        : null,
                    RoleId = u.UserRoles.Any() && u.UserRoles.FirstOrDefault() != null ? u.UserRoles.FirstOrDefault().RoleId : 0,
                    Address = u.CrmEmployee != null ? u.CrmEmployee.Address : u.CrmPatient != null ? u.CrmPatient.Address : null,
                    PhoneNumber = u.PhoneNumber,
                    AboutMe = u.CrmEmployee != null ? u.CrmEmployee.AboutMe : u.CrmPatient != null ? u.CrmPatient.AboutMe : null,
                    Position = u.CrmEmployee != null && u.CrmEmployee.DictPosition != null
                        ? new SelectDto()
                        {
                            //Id = u.CrmEmployee.DictPosition.Id,
                            NameKz = u.CrmEmployee.DictPosition.NameKz,
                            NameRu = u.CrmEmployee.DictPosition.NameRu,
                            NameEn = u.CrmEmployee.DictPosition.NameEn
                        }
                        : null,
                    Department = u.CrmEmployee != null && u.CrmEmployee.DictDepartment != null
                        ? new SelectDto()
                        {
                            //Id = u.CrmEmployee.DictDepartment.Id,
                            NameKz = u.CrmEmployee.DictDepartment.NameKz,
                            NameRu = u.CrmEmployee.DictDepartment.NameRu,
                            NameEn = u.CrmEmployee.DictDepartment.NameEn
                        }
                        : null,
                    Enterprise = u.CrmEmployee != null && u.CrmEmployee.DictEnterprise != null
                        ? new SelectDto()
                        {
                            //Id = u.CrmEmployee.DictEnterprise.Id,
                            NameKz = u.CrmEmployee.DictEnterprise.NameKz,
                            NameRu = u.CrmEmployee.DictEnterprise.NameRu,
                            NameEn = u.CrmEmployee.DictEnterprise.NameEn
                        }
                        : null,
                    City = u.CrmEmployee != null && u.CrmEmployee.DictCity != null
                        ? new SelectDto()
                        {
                            Id = u.CrmEmployee.DictCity.Id,
                            NameKz = u.CrmEmployee.DictCity.NameKz,
                            NameRu = u.CrmEmployee.DictCity.NameRu,
                            NameEn = u.CrmEmployee.DictCity.NameEn
                        }
                        : u.CrmPatient != null && u.CrmPatient.DictCity != null
                            ? new SelectDto()
                            {
                                Id = u.CrmPatient.DictCity.Id,
                                NameKz = u.CrmPatient.DictCity.NameKz,
                                NameRu = u.CrmPatient.DictCity.NameRu,
                                NameEn = u.CrmPatient.DictCity.NameEn
                            }
                            : null,
                    Gender = u.CrmEmployee != null && u.CrmEmployee.DictGender != null
                        ? new SelectDto()
                        {
                            Id = u.CrmEmployee.DictGender.Id,
                            NameKz = u.CrmEmployee.DictGender.NameKz,
                            NameRu = u.CrmEmployee.DictGender.NameRu,
                            NameEn = u.CrmEmployee.DictGender.NameEn
                        }
                        : u.CrmPatient != null && u.CrmPatient.DictGender != null
                            ? new SelectDto()
                            {
                                Id = u.CrmPatient.DictGender.Id,
                                NameKz = u.CrmPatient.DictGender.NameKz,
                                NameRu = u.CrmPatient.DictGender.NameRu,
                                NameEn = u.CrmPatient.DictGender.NameEn
                            }
                            : null
                })
                .FirstOrDefaultAsync();

            if (currentUser != null)
            {
                currentUser.ShortNameRu = GetUserShortName(currentUser.SurnameRu, currentUser.NameRu, currentUser.MiddlenameRu);
                currentUser.ShortNameKz = GetUserShortName(currentUser.SurnameKz, currentUser.NameKz, currentUser.MiddlenameKz);
                currentUser.ShortNameEn = GetUserShortName(currentUser.SurnameEn, currentUser.NameEn, currentUser.MiddlenameEn);
                currentUser.FullNameRu = GetUserFullName(currentUser.SurnameRu, currentUser.NameRu, currentUser.MiddlenameRu);
                currentUser.FullNameKz = GetUserFullName(currentUser.SurnameKz, currentUser.NameKz, currentUser.MiddlenameKz);
                currentUser.FullNameEn = GetUserFullName(currentUser.SurnameEn, currentUser.NameEn, currentUser.MiddlenameEn);
            }

            return currentUser;
        }

        public static async Task<string> GetUserPhotoByPath(string photoPath)
        {
            if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                    {
                var content = await System.IO.File.ReadAllBytesAsync(photoPath);
                return Convert.ToBase64String(content);
            }

            return null;
        }

        public static async Task<string> GetUserPhotoPathByUserName(CrmDbContext context, string username)
        {
            var currentUser = await context.Users
                .Include("CrmEmployee")
                .Include("CrmPatient")
                .Where(u => u.UserName == username && u.DeletedDateTime == null &&
                    (u.CrmEmployee != null && u.CrmEmployee.DeletedDateTime == null && u.CrmEmployee.IsActive == true || u.CrmPatient != null && u.CrmPatient.DeletedDateTime == null && u.CrmPatient.IsActive == true))
                .AsNoTracking()
                .Select(u => new UserEditDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    PhotoB64 = u.CrmEmployee != null ? u.CrmEmployee.PhotoB64 : u.CrmPatient != null ? u.CrmPatient.PhotoB64 : null,
                    PhotoPath = u.CrmEmployee != null ? u.CrmEmployee.PhotoPath : u.CrmPatient != null ? u.CrmPatient.PhotoPath : null
                })
                .FirstOrDefaultAsync();

            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PhotoPath) && System.IO.File.Exists(currentUser.PhotoPath))
            {
                return currentUser.PhotoPath;
            }

            return null;
        }

        public static async Task<string> GetUserPhotoByUserName(CrmDbContext context, string username)
        {
            var currentUser = await context.Users
                .Include("CrmEmployee")
                .Include("CrmPatient")
                .Where(u => u.UserName == username && u.DeletedDateTime == null &&
                    (u.CrmEmployee != null && u.CrmEmployee.DeletedDateTime == null && u.CrmEmployee.IsActive == true || u.CrmPatient != null && u.CrmPatient.DeletedDateTime == null && u.CrmPatient.IsActive == true))
                .AsNoTracking()
                .Select(u => new UserEditDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    PhotoB64 = u.CrmEmployee != null ? u.CrmEmployee.PhotoB64 : u.CrmPatient != null ? u.CrmPatient.PhotoB64 : null,
                    PhotoPath = u.CrmEmployee != null ? u.CrmEmployee.PhotoPath : u.CrmPatient != null ? u.CrmPatient.PhotoPath : null
                })
                .FirstOrDefaultAsync();

            if (currentUser != null && !string.IsNullOrEmpty(currentUser.PhotoPath) && System.IO.File.Exists(currentUser.PhotoPath))
            {
                var content = await System.IO.File.ReadAllBytesAsync(currentUser.PhotoPath);
                return Convert.ToBase64String(content);
            }
            else if (currentUser != null && !string.IsNullOrEmpty(currentUser.PhotoB64))
            {
                return currentUser.PhotoB64;
            }

            return null;
        }

        public static List<string> GetUserRoles(List<Claim> claims)
        {
            List<string> list = new List<string>();
            var roles = claims.FindAll(u => u.Type == ClaimsIdentity.DefaultRoleClaimType || u.Type == "role")
                .FirstOrDefault(c => c.Value != string.Empty)?.Value;
            if (!string.IsNullOrEmpty(roles))
            {
                if (roles.Contains(";"))
                {
                    list = roles.Split(";").Where(q => q != string.Empty).ToList();
                }
                else
                {
                    list = new List<string>
                    {
                        roles
                    };
                }
            }
            else
            {
                list = new List<string>
                {
                    "user"
                };
            }

            return list;
        }

        public static List<string> GetUserRolesFromToken(string access_token)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(access_token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(access_token) as JwtSecurityToken;
                var roles = token?.Claims
                    .Where(c => c.Value != string.Empty)
                    .FirstOrDefault(u => u.Type == ClaimsIdentity.DefaultRoleClaimType || u.Type == "role")?.Value;
                if (!string.IsNullOrWhiteSpace(roles))
                {
                    if (roles.Contains(";"))
                    {
                        list = roles.Split(";").Where(q => q != string.Empty).ToList();
                    }
                    else
                    {
                        list = new List<string>
                        {
                            roles
                        };
                    }
                }
                else
                {
                    list = new List<string>
                    {
                        "user"
                    };
                }
            }
#if DEBUG
            else
            {
                list = new List<string>
                {
                    "user"
                };
            }
#endif
            return list;
        }

        public static bool TryGetCurrentName(ClaimsPrincipal identityUser, out string name)
        {
            name = null;
            if (!string.IsNullOrEmpty(identityUser.Identity.Name))
            {
                var nameSplit = identityUser.Identity.Name.Split('\\');
                if (nameSplit.Length > 1)
                {
                    name = nameSplit[1];
                }
                else
                {
                    name = nameSplit[0];
                }

                return true;
            }
            else if (identityUser.Claims.Any(c => c.Type == JwtClaimTypes.Name || c.Type == ClaimTypes.Name))
            {
                var nameSplit = identityUser.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name || c.Type == ClaimTypes.Name)?.Value.Split('\\') ?? null;
                if (nameSplit == null)
                {
                    name = null;
                    return false;
                }
                else
                {
                    if (nameSplit.Length > 1)
                    {
                        name = nameSplit[1];
                    }
                    else
                    {
                        name = nameSplit[0];
                    }

                    return true;
                }
            }
#if DEBUG
            else if (!string.IsNullOrWhiteSpace(WindowsIdentity.GetCurrent().Name))
            {
                name = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                return true;
            }
#endif
            return false;
        }

        public static string GetUserShortName(string surname, string name, string middlename = null)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(name))
            {
                result = $"{surname} {name.First()}.";
                if (!string.IsNullOrEmpty(middlename))
                {
                    result = $"{result} {middlename.First()}.";
                }
            }

            return result;
        }

        public static string GetUserFullName(string surname, string name, string middlename)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(name))
            {
                result = $"{surname} {name}";
                if (!string.IsNullOrEmpty(middlename))
                {
                    result = $"{result} {middlename}";
                }
            }

            return result;
        }

    }
}
