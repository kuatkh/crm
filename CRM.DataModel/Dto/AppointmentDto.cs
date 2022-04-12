using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Dto
{
    public class AppointmentDto
    {
        public long Id { get; set; }
        public long? CrmPatientsId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Complain { get; set; }
        public string DoctorsAppointment { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public SelectWithPositionDto ToEmployee { get; set; }
        public List<DictionaryDto> SelectedProcedures { get; set; }

        public string Iin { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public string PhoneNumber { get; set; }
    }
}
