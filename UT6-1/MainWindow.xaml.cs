using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UT6_1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Mensaje> chatList = new ObservableCollection<Mensaje>();
        //cliente QnA
        private QnAMakerRuntimeClient cliente;
        string EndPoint = Properties.Settings.Default.EndPoint;
        string EndPointKey = Properties.Settings.Default.EndPointKey;
        string KnowledgeBaseId = Properties.Settings.Default.KnowledgeBaseId;
        public MainWindow()
        {
            InitializeComponent();  
            mensajesListBox.DataContext = chatList;
        }

        private void CommandBinding_Nueva(object sender, ExecutedRoutedEventArgs e)
        {
            chatList.Clear();
        }

        private void CommandBinding_CanExecute_Nueva(object sender, CanExecuteRoutedEventArgs e)
        {
            if (chatList != null && chatList.Count > 0)
            {
                e.CanExecute = true;

            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_Guardar(object sender, ExecutedRoutedEventArgs e)
        {
            string texto = "";
            foreach (Mensaje msg in chatList) {
                texto = texto + msg.Nombre +" : " + msg.Texto + "\n";
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.txt|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, texto);
            }
        }

        private void CommandBinding_CanExecute_Guardar(object sender, CanExecuteRoutedEventArgs e)
        {
            if (chatList != null && chatList.Count != 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            } 
        }

        private void CommandBinding_Salir(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void CommandBinding_Conexion(object sender, ExecutedRoutedEventArgs e)
        {
            ConexionAsync();
        }

        private async Task ConexionAsync() {
            
            try
            {
                cliente = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(EndPointKey)) { RuntimeEndpoint = EndPoint };
                string pregunta = "Hola";
                QnASearchResultList response = await cliente.Runtime.GenerateAnswerAsync(KnowledgeBaseId, new QueryDTO { Question = pregunta });
  
                MessageBox.Show("Conexión correcta con el servidor del Bot");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error en la conexión: " + exception);
            }
            
        }


        private async Task enviarMensajeASync() {
            Mensaje usuario = new Mensaje("Usuario", mensajeTextBox.Text);
            chatList.Add(usuario);

            Mensaje bot = new Mensaje();
            bot.Nombre = "Robot";

            try
            {
                cliente = new QnAMakerRuntimeClient(new EndpointKeyServiceClientCredentials(EndPointKey)) { RuntimeEndpoint = EndPoint };
                QnASearchResultList response = await cliente.Runtime.GenerateAnswerAsync(KnowledgeBaseId, new QueryDTO { Question = mensajeTextBox.Text });           
                bot.Texto = response.Answers[0].Answer;
                     
            }
            catch (Exception exception)
            {
                bot.Texto = "Error en la comunicación";
            }

            chatList.Add(bot);     
        }


        private void CommandBinding_Enviar(object sender, ExecutedRoutedEventArgs e)
        {
            enviarMensajeASync();
            mensajeTextBox.Clear();
        }

        private void CommandBinding_CanExecute_Enviar(object sender, CanExecuteRoutedEventArgs e)
        {
            if (mensajesListBox != null && mensajeTextBox.Text.Length != 0)
            {
                e.CanExecute = true;

            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_Configuracion(object sender, ExecutedRoutedEventArgs e)
        {
            Config confW = new Config();
            confW.Owner = this;
            confW.ShowDialog();
        }

        //Apuntes para la 2ºparte------->
        //En la nueva ventana
        //---------------------------
        //para el sexo hacer una lista hombre mujer y meterla en el combo box con el .itemssource=;
        //para la ventana de config fondo_combobox .itemssource = typeof(colors)getproperties();
        //en el boton aceptar DialogResult = true; <- antes de esto definir el codigo que queremos hacer, porque se cerrara la ventana.
        //En la clase de la nueva ventana : public string FondoColor{get;set;} -> Sexo = (string)sexoComboBox.SelectedItem <- Antes de afirmar dialogresult -->
        //---> pero esto lo podemos hacer con bindings para ahorrar codigo ... <ComboBox x:name=sexo selectedvalue=binding path = sexo> --->
        //--> en la nueva ventana this.DataContext=this;
        //podemos definir las variables de la ventana en la config del proyecto Properties.Settings.Default.sexo = ventana.Sexo
        //antes de abrir la ventana Properties.Settings.Default.fondoColor = ventana.fondoColor;
        //<itemscontrol background="{binding source={x:static properties.settings.default}. path=fondoColor
        /*
            en la clase main if ventana.ShowDialog() == true{MessageBox.Show(ventana.Sexo);}
         */
    }
}
