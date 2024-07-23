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
    /// Interaction logic for PatientEditOrDeleteWindow.xaml
    /// </summary>
    public partial class PatientEditOrDeleteWindow : Window
    {
        private int _patientID;
        private string _gender = "";
        private string _male = "Erkek";
        private string _female = "Kadın";
        private AvailablePatientWindow _availablePatient;

        public PatientEditOrDeleteWindow(int patientID,AvailablePatientWindow availablePatient)
        {
            InitializeComponent();
            _patientID = patientID;
            _availablePatient = availablePatient;
            if (_patientID == 0)
            {
                MessageBox.Show("Önce Hasta Seçmelisin!!!");
            }
            GetPatientInfosFromDatabase();
        }

        private void GetPatientInfosFromDatabase()
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Select * from Patient where PatientID = @pid";

                SqlParameter p1 = new SqlParameter("@pid", _patientID);
                cmd.Parameters.Add(p1);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtbox_PatientName.Text = dr.GetString(dr.GetOrdinal("PatientName"));
                    txtbox_PatientSurname.Text = dr.GetString(dr.GetOrdinal("PatientSurname"));
                    datepicker_PatientBirthDate.Text = dr.GetDateTime(dr.GetOrdinal("PatientBirthDate")).ToString();
                    _gender = dr.GetString(dr.GetOrdinal("PatientGender"));
                }

                if (_gender == _male)
                {
                    rdrBtn_GenderMale.IsChecked = true;
                }
                else
                {
                    rdrBtn_GenderFemale.IsChecked= true;
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

        private void btn_PatientUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Update Patient Set PatientName=@pn, PatientSurname=@psn, PatientBirthDate=@pbd, PatientGender=@pg where PatientID=@pid";

                SqlParameter p0 = new SqlParameter("@pid", _patientID);
                SqlParameter p1 = new SqlParameter("@pn", txtbox_PatientName.Text);
                SqlParameter p2 = new SqlParameter("@psn", txtbox_PatientSurname.Text);
                SqlParameter p3 = new SqlParameter("@pbd", datepicker_PatientBirthDate.SelectedDate.Value.Date);
                
                if (rdrBtn_GenderMale.IsChecked.HasValue)
                {
                    if (rdrBtn_GenderMale.IsChecked.Value)
                    {
                        _gender = _male;
                    }
                }
                else if (rdrBtn_GenderFemale.IsChecked.HasValue)
                {
                    if (rdrBtn_GenderFemale.IsChecked.Value)
                    {
                        _gender = _female;
                    }
                }
                SqlParameter p4 = new SqlParameter("@pg", _gender);

                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);

                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Hasta Bilgileri Başarıyla Güncellendi!!!");
                }
                else
                {
                    MessageBox.Show("Hasta Bilgileri Güncellenemedi!!!");
                }

                cnn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void btn_PatientDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Delete From Appointment Where PatientID = @pid Delete From PastAppointment Where PatientID = @pid Delete Patient where PatientID = @pid";

                SqlParameter p1 = new SqlParameter("@pid", _patientID);
                cmd.Parameters.Add(p1);

                MessageBoxResult question1 = MessageBox.Show("Hasta Kaydını Silmek istediğinden emin misin? \n " +
                    "Eğer silersen o hasta ile ilgili tüm kayıtlar silinecek!!!", "Hasta Kaydı Silme", MessageBoxButton.YesNo);
                MessageBoxResult question2 = MessageBox.Show("Emin Misin?!!!", "SON KARAR", MessageBoxButton.YesNo);

                if (question1 == MessageBoxResult.Yes && question2 == MessageBoxResult.Yes)
                {
                    int success = cmd.ExecuteNonQuery();

                    if (success != 0)
                    {
                        MessageBox.Show("Hasta Kaydı Başarıyla Silindi!!!");
                        txtbox_PatientName.Text = "";
                        txtbox_PatientSurname.Text = "";
                        datepicker_PatientBirthDate.Text = "";
                        rdrBtn_GenderFemale.IsChecked = false;
                        rdrBtn_GenderMale.IsChecked = false;
                        _availablePatient.dtbl.Reset();
                        _availablePatient.FillCboAvailablePatients(selectedValueNull: true);
                    }
                    else
                    {
                        MessageBox.Show("Hasta Kaydı Silinemedi!!!");
                    }
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
