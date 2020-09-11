using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class ResultDto<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public int RowsCount { get; set; }
        public T Data { get; set; }
    }
}
