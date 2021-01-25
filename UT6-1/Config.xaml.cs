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

namespace UT6_1
{
    /// <summary>
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            InitializeComponent();
            colFondoComboBox.ItemsSource = typeof(Colors).GetProperties();
            colUsuarioComboBox.ItemsSource = typeof(Colors).GetProperties();
            colRobotComboBox.ItemsSource = typeof(Colors).GetProperties();
            List<String> sexo = new List<String>() { "hombre", "mujer" };
            sexoComboBox.ItemsSource = sexo;


            colFondoComboBox.SelectedItem = Properties.Settings.Default.fondoColor;
        }

        private void Button_Click_Aceptar(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Properties.Settings.Default.fondoColor = colFondoComboBox.SelectedItem.ToString().Split(' ')[1];
            //selectedvalue
            Properties.Settings.Default.usuarioColor = colUsuarioComboBox.SelectedItem.ToString().Split(' ')[1];
            Properties.Settings.Default.robotColor = colRobotComboBox.SelectedItem.ToString().Split(' ')[1];
            Properties.Settings.Default.sexo = sexoComboBox.SelectedItem.ToString();
            Properties.Settings.Default.Save();

        }

        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
