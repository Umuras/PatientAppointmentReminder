using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace Patient_Appointment_Reminder
{
    /// <summary>
    /// Interaction logic for AddNewPatient.xaml
    /// </summary>
    public partial class AddNewPatient : Window
    {
        private string _male = "Erkek";
        private string _female = "Kadın";

        public AddNewPatient()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            //try catch içinde yazmamızın sebebi cnn.Open() fonksiyonu Exception fırlatıyor, programın çökmemesi için
            //try catch'e alıyoruz.
            try
            {
                //Sql Server'a bağlantı kurmak için SqlConnection türünde nesne oluşturuyoruz.
                SqlConnection cnn = new SqlConnection();
                //ConnectionStringe server database ve windows authentication ile bağlanmak için intergrated security true yapıyoruz.
                cnn.ConnectionString = "server=.; database=PatientAppointmentSystem; integrated security=true";
                //Bağlantıyı açıyoruz.
                cnn.Open();

                //SqlCommand ile Sql Komutlarımızı giriyoruz.
                SqlCommand cmd = new SqlCommand();
                //cmd ile temsil edilen komut cnn ile temsil edilen bağlantı üzerinden çalışsın. İkisini birbiri ile ilişkilendiriyoruz.
                cmd.Connection = cnn;

                //Ekleme işlemi yapıyoruz, Patient tablosundaki kolonlara ekleme yapıyoruz parametreler üzerinden
                cmd.CommandText = "insert Patient(PatientName,PatientSurname,PatientBirthDate,PatientGender) values(@pn,@psn,@pbd,@pg)";

                //Burada CommandText kısmında yazdığımız values(@pn,@psn,@pbd,@pg) parametrelerine değer yüklüyoruz.
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@pn";
                p1.SqlDbType = SqlDbType.NVarChar;
                p1.Size = 50;
                p1.SqlValue = txtbox_PatientName.Text.ToString();

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@psn";
                p2.SqlDbType = SqlDbType.NVarChar;
                p2.Size = 50;
                p2.SqlValue = txtbox_PatientSurname.Text.ToString();

                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "@pbd";
                p3.SqlDbType = SqlDbType.Date;
                if (datepicker_PatientBirthDate.SelectedDate != null)
                {
                    p3.SqlValue = datepicker_PatientBirthDate.SelectedDate.Value.Date;
                }
                else
                {
                    MessageBox.Show("Doğum tarihi boş bırakılamaz!!!");
                    return;
                }

                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "@pg";
                p4.SqlDbType = SqlDbType.Char;

                if (!rdrBtn_GenderMale.IsChecked.HasValue && !rdrBtn_GenderFemale.IsChecked.HasValue)
                {
                    MessageBox.Show("Cinsiyet boş bırakılamaz!!!");
                    return;
                }

                if (rdrBtn_GenderMale.IsChecked.HasValue && rdrBtn_GenderMale.IsChecked.Value)
                {
                    p4.SqlValue = _male;
                }
                else
                {
                    p4.SqlValue = _female;
                }
                //Command'ın parameters koleksiyonuna ekliyoruz.
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);

                //ExecuteNonQuery ile işlem eğer başarılı ise yani tabloya kayıt eklendi ise kayıt sayısınca veri dönüyor, eğer başarısız ise 0 
                //dönüyor.
                int success = cmd.ExecuteNonQuery();

                if (success != 0)
                {
                    MessageBox.Show("Kayıt Başarılı!");
                    //Bu şekilde yeni kayıt eklendiğinde veri girdiğimiz alanlar temizlenmiş oluyor.
                    txtbox_PatientName.Text = "";
                    txtbox_PatientSurname.Text = "";
                    datepicker_PatientBirthDate.SelectedDate = null;
                    rdrBtn_GenderFemale.IsChecked = false;
                    rdrBtn_GenderMale.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Kayıt Başarısız!!!");
                }

                if (cnn.State == ConnectionState.Broken)
                {
                    MessageBox.Show("Bağlantı bir hata yüzünden kapandı");
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı açılamadı");
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
