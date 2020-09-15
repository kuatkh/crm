using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.DataModel.Models
{
    public class DictCities
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public long DictCountriesId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        [ForeignKey("DictCountriesId")]
        [InverseProperty("DictCities")]
        public virtual DictCountries DictCountry { get; set; }

        [InverseProperty("DictCity")]
        public virtual ICollection<CrmPatients> CrmPatients { get; set; }

        [InverseProperty("DictCity")]
        public virtual ICollection<CrmEmployees> CrmEmployees { get; set; }
    }
}
