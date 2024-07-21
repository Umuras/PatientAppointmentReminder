using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Patient_Appointment_Reminder
{
    /// <summary>
    /// Interaction logic for AppointmentCreatingWindow.xaml
    /// </summary>
    public partial class AppointmentCreatingWindow : Window
    {
        public int patientID;

        public AppointmentCreatingWindow()
        {
            InitializeComponent();
        }

        private void btn_SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "insert Appointment(PatientID,Hospital,Section,AppointmentDate,Note) values(@pid,@hl,@sn,@apd,@nt)";

                SqlParameter p0 = new SqlParameter();
                p0.ParameterName = "@pid";
                p0.SqlDbType = SqlDbType.Int;
                p0.SqlValue = patientID;

                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@hl";
                p1.SqlDbType = SqlDbType.NVarChar;
                p1.Size = 50;
                p1.SqlValue = txt_Hospital.Text.ToString();

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@sn";
                p2.SqlDbType = SqlDbType.NVarChar;
                p2.Size = 50;
                p2.SqlValue = txt_HospitalSection.Text.ToString();

                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "@apd";
                p3.SqlDbType = SqlDbType.DateTime;
                p3.Value = time_Appointment.Value;

                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "@nt";
                p4.SqlDbType = SqlDbType.NText;
                p4.Value = txtBox_Note.Text.ToString();

                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);

                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Randevu Kaydı Başarılı");
                    txt_Hospital.Text = "";
                    txt_HospitalSection.Text = "";
                    txtBox_Note.Text = "";
                    time_Appointment.Text = "";
                }
                else
                {
                    MessageBox.Show("Randevu Kaydı Başarısız!!!");
                }

                if (cnn.State == ConnectionState.Broken)
                {
                    MessageBox.Show("Bağlantı bir hata yüzünden kapandı");
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
