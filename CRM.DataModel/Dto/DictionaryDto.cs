using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class DictionaryDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionKz { get; set; }
        public string DescriptionEn { get; set; }
        public long? ParentId { get; set; }
        public string ParentNameRu { get; set; }
        public string ParentNameKz { get; set; }
        public string ParentNameEn { get; set; }
        public string PositionCategory { get; set; }
        public float? Amount { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? EditedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string CreatedDateTimeStr { get; set; }
        public string EditedDateTimeStr { get; set; }
        public string DeletedDateTimeStr { get; set; }

        public List<DictionaryDto> Children { get; set; }
    }
}
