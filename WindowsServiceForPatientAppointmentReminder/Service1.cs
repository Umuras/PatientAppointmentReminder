using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceForPatientAppointmentReminder
{
    public partial class Service1 : ServiceBase
    {
        public ArrayList patientAppointmentReminderList;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //Server ile bağlantı hazırlığı yapılıyor.
                SqlConnection cnn = new SqlConnection();
                //Bağlantı için server bilgisi database bilgisi ve Windows Authentication ayarı yapılıyor.
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";

                //SqlCommand sınıfı ile SQL'e göndereceğimiz komut giriliyor.
                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "Select ap.AppointmentID, ap.PatientID, pt.PatientName + ' ' + pt.PatientSurname as PatientFullName, ap.AppointmentDate, ap.Hospital, ap.Section, ap.Doctor, ap.Note from Appointment as ap, Patient as pt where ap.PatientID = pt.PatientID";
                cmd.CommandText = "UpcomingAppointments";
                cmd.CommandType = CommandType.StoredProcedure;

                patientAppointmentReminderList = new ArrayList();
             
                SqlDataReader dr = cmd.ExecuteReader();
                //Eriştiğimiz kayıtları listede toplayıp ona göre işlem yapıyoruz.
                while (dr.Read())
                {
                    PatientAppointmentReminderEntity patientAppointmentReminder = new PatientAppointmentReminderEntity()
                    {
                        AppointmentID = dr.GetInt32(dr.GetOrdinal("AppointmentID")),
                        PatientID = dr.GetInt32(dr.GetOrdinal("PatientID")),
                        PatientFullName = dr.GetString(dr.GetOrdinal("PatientFullName")),
                        AppointmentDate = dr.GetDateTime(dr.GetOrdinal("AppointmentDate")),
                        Hospital = dr.GetString(dr.GetOrdinal("Hospital")),
                        Section = dr.GetString(dr.GetOrdinal("Section")),
                        Doctor = dr.GetString(dr.GetOrdinal("Doctor")),
                        Note = dr.GetString(dr.GetOrdinal("Note"))
                    };

                    patientAppointmentReminderList.Add(patientAppointmentReminder);
                }

                //Hastanın randevu bilgileri içinde dönüp eğer hastanın randevusu bugün, 1 gün kalmış ve 7 gün kalmış ise size mail gönderiyor.
                for (int i = 0; i < patientAppointmentReminderList.Count; i++)
                {
                    PatientAppointmentReminderEntity item = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]);

                    StringBuilder stb = new StringBuilder();
                    stb.AppendLine("Hasta Adı Soyadı: " + item.PatientFullName.ToString());
                    stb.AppendLine("Hastane: " + item.Hospital.ToString());
                    stb.AppendLine("Bölüm: " + item.Section.ToString());
                    stb.AppendLine("Doktor: " + item.Doctor.ToString());
                    stb.AppendLine("Randevu Tarihi: " + item.AppointmentDate.ToString());
                    stb.AppendLine("Notlar: " + item.Note.ToString());
                    if (CalculateAppointmentDay(i) == 0)
                    {
                        stb.AppendLine("RANDEVU BUGÜN UNUTMA!!!!!!!");
                        stb.AppendLine("----------------------------------------------------------------");
                        SendMailForUpcomingAppointments(stb, item.PatientFullName + " Randevu Bilgileri");
                    }
                    else if(CalculateAppointmentDay(i) == 1 || CalculateAppointmentDay(i) == 7)
                    {
                        stb.AppendLine("Randevuye Kalan Gün Sayısı: " + CalculateAppointmentDay(i));
                        stb.AppendLine("----------------------------------------------------------------");
                        SendMailForUpcomingAppointments(stb, item.PatientFullName + " Randevu Bilgileri");
                    }
                }

                dr.Close();
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        //Burada ise databaseden eriştiğimiz hastanın randevu tarihi ile bugünün tarihini çıkararak kalan gün sayısını hesaplıyoruz.
        private int CalculateAppointmentDay(int i)
        {
            TimeSpan timeSpanDay;
            int day = 0;

            timeSpanDay = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]).AppointmentDate.Date - DateTime.Now;

            if (timeSpanDay.Days == 0)
            {
                day = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]).AppointmentDate.Date.Day - DateTime.Now.Day;
            }
            else if (timeSpanDay.Days == 6)
            {
                day = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]).AppointmentDate.Date.Day - DateTime.Now.Day;
            }
            else
            {
                day = timeSpanDay.Days;
            }

            return day;
        }
        //Burada ise databaseden elde ettiğimiz hasta bilgileri üzerinden eğer randevu tarihi bugün, 1 gün veya 7 kalmış ise bunu mail
        //olarak randevu bilgilerini belirlediğimiz mail adreslerine gönderiyoruz.
        private void SendMailForUpcomingAppointments(StringBuilder stb, string appointmentTitle)
        {
            try
            {
                MailMessage mm = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.office365.com");

                mm.From = new MailAddress("example@hotmail.com", "");
                mm.To.Add("example@hotmail.com");
                mm.Subject = appointmentTitle;
                mm.Body = stb.ToString();

                client.Credentials = new NetworkCredential("example@hotmail.com", "");
                client.Port = 587;
                client.EnableSsl = true;
                client.Send(mm);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        protected override void OnStop()
        {
        }
    }
}
