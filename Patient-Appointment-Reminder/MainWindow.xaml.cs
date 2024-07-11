using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Patient_Appointment_Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void yeniHastaGirisiButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Yeni Hasta Girişi Yapılacak");
            AddNewPatient addNewPatient = new AddNewPatient();
            addNewPatient.ShowDialog();
        }
    }
}