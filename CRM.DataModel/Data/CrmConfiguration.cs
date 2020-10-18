using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Data
{
    public class CrmConfiguration
    {
        public string ConnectionString { get; set; }
        public string AuthServerUrl { get; set; } = "http://localhost:5000/";
        public string AttachmentsPath { get; set; } = "C:/_CrmAttachments";
        public string ProfilePhotoPath { get; set; } = "C:/_CrmProfilePhoto";
        public CrmClientConfiguration Client { get; set; }
        public string GrantType { get; set; }
        public string Audience { get; set; }
        public string CertPath { get; set; }
        public string CertSecret { get; set; }
        public List<string> AppointmentPositions { get; set; }
    }
}
