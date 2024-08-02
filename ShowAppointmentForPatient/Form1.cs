using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShowAppointmentForPatient
{
    public partial class Form1 : Form
    {
        public ArrayList patientAppointmentReminderList;
        public int upcomingAppointments = 0;
        public Rectangle originalFormSize;
        public Rectangle textOriginalRectangle;
        StringBuilder stb = new StringBuilder();
        public List<string> patient = new List<string>();

        //Bu kısımda formumuzun ve textboxımızın programın açılış anındaki konum ve boyut bilgilerini alıyoruz.
        public Form1()
        {
            InitializeComponent();
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            textOriginalRectangle = new Rectangle(txt_AppointmentInfos.Location.X, txt_AppointmentInfos.Location.Y, txt_AppointmentInfos.Width, txt_AppointmentInfos.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Database'e bağlantı kurup hasta bilgilerini alıyoruz.
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Select ap.AppointmentID, ap.PatientID, pt.PatientName + ' ' + pt.PatientSurname as PatientFullName, ap.AppointmentDate, ap.Hospital, ap.Section, ap.Doctor, ap.Note from Appointment as ap, Patient as pt where ap.PatientID = pt.PatientID";

                //Elde ettiğimiz hasta bilgilerini oluşturduğumuz listede tutarak o liste içinden işlem yapacağız.
                patientAppointmentReminderList = new ArrayList();
                //Her kaydı teker teker okumak için SqlDataReader'ı kullanıyoruz.
                SqlDataReader dr = cmd.ExecuteReader();
                //While döngüsü ile son kayda kadar tüm kayıtları okuyoruz ve bilgileri PatientAppointmentReminderEntity
                //sınıfı türünde nesne oluşturup oraya ekliyoruz en sonunda da listeye ekliyoruz.
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

                //Burada ise listedeki hastaları dönüyoruz ve eğer hastanın randevusunun tarihine 1 gün veya 7 gün
                //kaldıysa veya bugün ise o bilgileri textbox'ımıza yazdırıyoruz.
                for (int i = 0; i < patientAppointmentReminderList.Count; i++)
                {
                    PatientAppointmentReminderEntity item = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]);

                    stb.AppendLine("Hasta Adı Soyadı: " + item.PatientFullName.ToString());
                    stb.AppendLine("Hastane: " + item.Hospital.ToString());
                    stb.AppendLine("Bölüm: " + item.Section.ToString());
                    stb.AppendLine("Doktor: " + item.Doctor.ToString());
                    stb.AppendLine("Randevu Tarihi: " + item.AppointmentDate.ToString());
                    stb.AppendLine("Notlar: " + item.Note.ToString());
                    if (CalculateAppointmentDay(i) == 0)
                    {
                        upcomingAppointments++;
                        patient.Add(item.PatientFullName);
                        stb.AppendLine("RANDEVU BUGÜN UNUTMA!!!!!!!");
                        stb.AppendLine("----------------------------------------------------------------");
                        txt_AppointmentInfos.Text = stb.ToString();
                        txt_AppointmentInfos.ReadOnly = true;
                    }
                    else if (CalculateAppointmentDay(i) == 1 || CalculateAppointmentDay(i) == 7)
                    {
                        upcomingAppointments++;
                        patient.Add(item.PatientFullName);
                        stb.AppendLine("Randevuye Kalan Gün Sayısı: " + CalculateAppointmentDay(i));
                    }
                }
                //Burada eğer textboxda birden fazla hasta randevu bilgisi gösterilecekse lbl_appointmentTitle'a ikisininde ismini yazıyoruz.
                if (patient.Count > 1)
                {
                    lbl_appointmentTitle.Text = "";
                    for (int i = 0; i < patient.Count; i++)
                    {
                        if (i != patient.Count-1)
                        {
                            lbl_appointmentTitle.Text += patient[i] + " " + "ve ";
                        }
                        else
                        {
                            lbl_appointmentTitle.Text += patient[i] + " ";
                        }
                    }
                    lbl_appointmentTitle.Text += "Randevu Bilgileri";
                }
                else
                {
                    lbl_appointmentTitle.Text = patient[0] + " Randevu Bilgisi";
                }

                dr.Close();
                cnn.Close();

                //Burada ise eğer program çalıştığında hiç şartımızı sağlayan randevuye ulaşmamışsa
                //uygulamamızı kapatıyoruz.
                if (upcomingAppointments == 0)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        //Burada randevu tarihi ile bugünün tarihini çıkararak randevuya kaç gün kaldığını hesaplıyoruz.
        private int CalculateAppointmentDay(int i)
        {
            TimeSpan day;

            day = ((PatientAppointmentReminderEntity)patientAppointmentReminderList[i]).AppointmentDate.Date - DateTime.Now;

            return day.Days;
        }

        //Burada ise ekran boyutu değiştiği anda yeni ekran boyutuna göre textboxımızın boyutunun tekrar ayarlan-
        //masını sağlıyoruz.
        private void ResizeControl(Rectangle r, Control c)
        {
            float xRatio = (float)(this.Width) / (float)(originalFormSize.Width);
            float yRatio = (float)(this.Height) / (float)(originalFormSize.Height);

            int newX = (int)(r.Location.X * xRatio);
            int newY = (int)(r.Location.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }

        //Burada ise Form1_Resize eventi sayesinde formun boyutu her değiştiğinde event tetiklenip fonksiyonumuzu
        //çalıştırıyoruz.
        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeControl(textOriginalRectangle, txt_AppointmentInfos);
        }
    }
}
