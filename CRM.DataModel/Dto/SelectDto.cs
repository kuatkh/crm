using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class SelectDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
    }

    public class SelectWithParentDto : SelectDto
    {
        public long? ParentId { get; set; }
        public string ParentNameRu { get; set; }
        public string ParentNameKz { get; set; }
        public string ParentNameEn { get; set; }
    }

    public class SelectWithPositionDto : SelectDto
    {
        public long? PositionId { get; set; }
        public string PositionNameRu { get; set; }
        public string PositionNameKz { get; set; }
        public string PositionNameEn { get; set; }
    }
}
