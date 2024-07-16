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

namespace Patient_Appointment_Reminder
{
    /// <summary>
    /// Interaction logic for AvailablePatientWindow.xaml
    /// </summary>
    public partial class AvailablePatientWindow : Window
    {
        public AvailablePatientWindow()
        {
            InitializeComponent();
            //Burada ComboBoxın itemssource'ına döndürdüğümüz listeyi yüklüyoruz. Arka planda çalışacak SelectedValuePath'a PatientIDleri, Ekran gözükecek
            //DisplayMemberPath'a ise de PatientNameSurname'i atıyoruz. Bunlar Patient sınıfındaki PatientID ve PatientNameSurname propertylerine
            //yüklenmiş veriler oluyor.
            cboAvailablePatients.ItemsSource = GetPatientsInfosFromDatabase();
            cboAvailablePatients.SelectedValuePath = "PatientID";
            cboAvailablePatients.DisplayMemberPath = "PatientNameSurname";
        }

        private ArrayList GetPatientsInfosFromDatabase()
        {
            try
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Select PatientID, PatientName, PatientSurname from Patient";
                //SqlDataReader üzerinden kayıtlarımızı okuyoruz.
                SqlDataReader dr = cmd.ExecuteReader();
                //Listeye SqlDataReader üzerinden okuduğumuz hasta bilgilerini Patient sınıfındaki propertyle yükleyip ekliyoruz.
                ArrayList lst = new ArrayList();

                //Teker teker okuyoruz son kayda kadar
                while (dr.Read())
                {
                    //Patient türünde nesne oluşturup Object Initialinizer kullanarak propertylerimize verileri yükleyip arrayliste ekliyoruz.
                    Patient patient = new Patient()
                    {
                        //dr.GetInt32 dendiğinde o kolonunun numarasını yazdığında getiriyor, dr.GetOrdinal ile adını yazarak o numaraya
                        //dönüştürüp o şekilde erişiyorsun, numarasını hatırlamazsan
                        PatientID = dr.GetInt32(dr.GetOrdinal("PatientID")),
                        PatientNameSurname = dr.GetString(dr.GetOrdinal("PatientName")) + " " + dr.GetString(dr.GetOrdinal("PatientSurname"))
                    };
                    lst.Add(patient);
                }

                cnn.Close();

                return lst;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private DataTable GetPatientsFromDatabase(int selectedPatientID)
        {
            try
            {
                //Burada ise veritabanına erişip Hastanın verilerine ComboBoxdan gelen seçilen idye göre erişip DataGrid üzerinde listelenmesini
                //sağlıyoruz verilerin
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                cnn.Open();

                //Buranın CommandTextinde tablolar türkçe görünsün diye isimleri değiştirildi, boşluklu yazabilmek için [] kullanıldı.
                //where şartı ile de seçilen id üzerinden sadece o hastanın bilgilerinin gözükmesi sağlandı.
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Select PatientID as HastaID, PatientName as [Hasta Adı], PatientSurname as [Hasta Soyadı], PatientBirthDate as [Hasta Doğum Tarihi], PatientGender as [Hasta Cinsiyeti], NumberOfAppointments as [Mevcut Randevu Sayısı], NumberOfPastAppointments as [Geçmiş Randevu Sayısı] from Patient where PatientID = @pid";

                SqlParameter prm = new SqlParameter();
                prm.ParameterName = "@pid";
                prm.SqlDbType = SqlDbType.Int;
                prm.SqlValue = selectedPatientID;
                prm.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(prm);

                SqlDataReader dr = cmd.ExecuteReader();
                //Burada ise okunan tüm veriler dataTable'a yüklendi.
                DataTable dtbl = new DataTable();
                dtbl.Load(dr);

                cnn.Close();

                return dtbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }

        private void cboAvailablePatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Burada şu oluyor comboBox üzerinde seçilen item aslında Category sınıfının bir nesnesi olduğu için biz onu Category sınıfına
            //Cast edebiliyoruz, bize ilk başta obje olarak döndürdüğü için. Sonra onun üzerinden ID veya isme erişim sağlayabileceğiz.
            Patient selectedPatient = (Patient)cboAvailablePatients.SelectedItem;

            //DataGrid'in ItemsSourceına GetPatientsFromDatabase gelen DataTableı atıyoruz.
            grdPatients.ItemsSource = GetPatientsFromDatabase(selectedPatient.PatientID).DefaultView;
        }

        private void btn_takeAppointment_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
