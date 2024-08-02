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
        private AvailablePatientWindow _availablepatientWindow;

        public AppointmentCreatingWindow(AvailablePatientWindow availablepatientWindow)
        {
            InitializeComponent();
            _availablepatientWindow = availablepatientWindow;
        }

        private void btn_SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();
                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "insert Appointment(PatientID,Hospital,Section,Doctor,AppointmentDate,Note) values(@pid,@hl,@sn,@dr,@apd,@nt)";
                cmd.CommandText = "TakeAppointment";
                cmd.CommandType = CommandType.StoredProcedure;


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
                p3.ParameterName = "@dr";
                p3.SqlDbType = SqlDbType.NVarChar;
                p3.Size = 50;
                p3.SqlValue = txt_Doctor.Text.ToString();

                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "@apd";
                p4.SqlDbType = SqlDbType.DateTime;
                p4.Value = time_Appointment.Value;

                SqlParameter p5 = new SqlParameter();
                p5.ParameterName = "@nt";
                p5.SqlDbType = SqlDbType.NText;
                p5.Value = txtBox_Note.Text.ToString();

                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                cmd.Parameters.Add(p5);
                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Randevu Kaydı Başarılı");
                    txt_Hospital.Text = "";
                    txt_HospitalSection.Text = "";
                    txt_Doctor.Text = "";
                    txtBox_Note.Text = "";
                    time_Appointment.Text = "";
                    _availablepatientWindow.grdPatients.ItemsSource = _availablepatientWindow.GetPatientsFromDatabase(patientID).DefaultView;
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
