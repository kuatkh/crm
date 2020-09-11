using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class LogInDto
    {
        public string UserName { get; set; }
        public string UserSecret { get; set; }
        public string UserEmail { get; set; }
        public bool IsMobile { get; set; } = false;
    }

    public class LogInResultDto
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
