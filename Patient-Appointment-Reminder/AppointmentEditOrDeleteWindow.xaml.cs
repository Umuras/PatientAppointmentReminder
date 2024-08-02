using System;
using System.Collections;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Patient_Appointment_Reminder
{
    /// <summary>
    /// Interaction logic for AppointmentEditOrDeleteWindow.xaml
    /// </summary>
    public partial class AppointmentEditOrDeleteWindow : Window
    {
        public int patientID;
        public bool hasPatientAppointment = false;
        private int _appointmentCount = 0;
        private AppointmentEntity appointmentEntity;
        private AvailablePatientWindow _availablepatientWindow;

        public AppointmentEditOrDeleteWindow(int patientID, AvailablePatientWindow availablepatientWindow)
        {
            InitializeComponent();
            this.patientID = patientID;
            FillCboAppoinments();
            _availablepatientWindow = availablepatientWindow;
        }

        private void FillCboAppoinments()
        {
            cbo_Appointments.ItemsSource = GetAllAppointments();
            cbo_Appointments.SelectedValuePath = "AppointmentID";
            cbo_Appointments.DisplayMemberPath = "DisplayAppointmentNo";
        }

        private ArrayList GetAllAppointments()
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "Select * from Appointment where PatientID = @pid";
                cmd.CommandText = "GetAllAppointmentsForSelectedPatient";
                cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter p1 = new SqlParameter("@pid", patientID);
                cmd.Parameters.Add(p1);

                ArrayList lst = new ArrayList();

                SqlDataReader dr = cmd.ExecuteReader();
                
                while(dr.Read())
                {
                    _appointmentCount++;
                    AppointmentEntity appointmentEntity = new AppointmentEntity()
                    {
                        AppointmentID = dr.GetInt32(dr.GetOrdinal("AppointmentID")),
                        PatientID = dr.GetInt32(dr.GetOrdinal("PatientID")),
                        Hospital = dr.GetString(dr.GetOrdinal("Hospital")),
                        Section = dr.GetString(dr.GetOrdinal("Section")),
                        Doctor = dr.GetString(dr.GetOrdinal("Doctor")),
                        AppointmentDate = dr.GetDateTime(dr.GetOrdinal("AppointmentDate")),
                        Note = dr.GetString(dr.GetOrdinal("Note")),
                        DisplayAppointmentNo = "Randevu No: " + _appointmentCount
                    };
                    lst.Add(appointmentEntity);
                }

                if (lst.Count == 0)
                {
                    if (!hasPatientAppointment)
                    {
                        MessageBox.Show("Hastanın hiç randevusu yok önce randevu oluştur");
                    }
                    hasPatientAppointment = false;
                    _appointmentCount = 0;
                }
                else
                {
                    hasPatientAppointment = true;
                    _appointmentCount = 0;
                }

                dr.Close();
                cnn.Close();

                return lst;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void ShowAppointmentInfo(int selectionAppointmentID)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "Select * from Appointment where AppointmentID = @aid";
                cmd.CommandText = "GetSelectedAppointment";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter("@aid", selectionAppointmentID);
                cmd.Parameters.Add(p1);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txt_Hospital.Text = dr.GetString(dr.GetOrdinal("Hospital"));
                    txt_HospitalSection.Text = dr.GetString(dr.GetOrdinal("Section"));
                    txt_Doctor.Text = dr.GetString(dr.GetOrdinal("Doctor"));
                    txtBox_Note.Text = dr.GetString(dr.GetOrdinal("Note"));
                    time_Appointment.Value = dr.GetDateTime(dr.GetOrdinal("AppointmentDate"));
                }


                dr.Close();
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void cbo_Appointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            appointmentEntity = (AppointmentEntity)cbo_Appointments.SelectedItem;
            if (appointmentEntity != null)
            {
                ShowAppointmentInfo(appointmentEntity.AppointmentID);
            }
        }

        private void btn_UpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "Update Appointment set Hospital=@hp, Section=@sc, Doctor=@dr, AppointmentDate=@apdt, Note=@nt where AppointmentID = @apid";
                cmd.CommandText = "UpdateSelectedAppointment";
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlParameter p0 = new SqlParameter("apid",appointmentEntity.AppointmentID);
                SqlParameter p1 = new SqlParameter("@hp", txt_Hospital.Text);
                SqlParameter p2 = new SqlParameter("@sc", txt_HospitalSection.Text);
                SqlParameter p3 = new SqlParameter("@dr", txt_Doctor.Text);
                SqlParameter p4 = new SqlParameter("@apdt", time_Appointment.Value);
                SqlParameter p5 = new SqlParameter("@nt", txtBox_Note.Text);

                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                cmd.Parameters.Add(p5);

                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Randevu Başarıyla Güncellendi!!!");
                }
                else
                {
                    MessageBox.Show("Randevu Güncellenemedi");
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void btn_DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection("server=.; database=PatientAppointmentSystem; integrated security=true");
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandText = "Delete Appointment where AppointmentID = @apid";
                cmd.CommandText = "DeleteSelectedAppointment";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter(parameterName: "@apid", appointmentEntity.AppointmentID);
                cmd.Parameters.Add(p1);

                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Randevu başarıyla silindi!!!");
                    txt_Hospital.Text = "";
                    txt_HospitalSection.Text = "";
                    txt_Doctor.Text = "";
                    txtBox_Note.Text = "";
                    time_Appointment.Text = "";
                    _availablepatientWindow.grdPatients.ItemsSource = _availablepatientWindow.GetPatientsFromDatabase(patientID).DefaultView;
                    cbo_Appointments.SelectedValue = null;
                    FillCboAppoinments();
                }
                else
                {
                    MessageBox.Show("Randevu silinemedi!!!");
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
