using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class CertDto
    {
        public string DnsName { get; set; }
        public string CertPath { get; set; }
        public string FileName { get; set; }
        public int CertType { get; set; }
        public string Secret { get; set; }
        public int ValidityPeriodInDays { get; set; }
    }
}
