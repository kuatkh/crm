using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class SelectDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class SelectWithParentDto : SelectDto
    {
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
    }

    public class SelectWithPositionDto : SelectDto
    {
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
