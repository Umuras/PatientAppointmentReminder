using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceForPatientAppointmentReminder
{
    internal class PatientAppointmentReminderEntity
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientFullName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Hospital { get; set; }
        public string Section { get; set; }
        public string Doctor { get; set; }
        public string Note { get; set; }
    }
}
