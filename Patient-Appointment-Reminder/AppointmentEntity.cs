using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Appointment_Reminder
{
    public class AppointmentEntity
    {
        public required int AppointmentID { get; set; }
        public required int PatientID { get; set; }
        public required string Hospital { get; set; }
        public required string Section { get; set; }
        public required string Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Note { get; set; }
        public required string DisplayAppointmentNo { get; set; }
    }
}
