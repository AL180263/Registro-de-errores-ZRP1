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
using ConsultaCore;
using MaterialDesignThemes.Wpf;
using Registro_de_errores_ZRP1.Tablas;
using System.Net.NetworkInformation;


namespace Registro_de_errores_ZRP1.Controles
{
    /// <summary>
    /// Lógica de interacción para Registro_de_resina.xaml
    /// </summary>
    public partial class Registro_de_resina : Window
    {

        private bool IsConected = false;

        CoreDataBaseAccess Consulta = new CoreDataBaseAccess();

        Brush colorOriginal;

        public Registro_de_resina()
        {
            InitializeComponent();
            WifiAvaible();
            this.snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2500));
            this.SnackbarConsulta.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2500));
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CadenaDeConexion))
            {
                Consulta.CadenaDeConexion = Registro_de_errores_ZRP1.Properties.Settings.Default.CadenaDeConexion;
            }
            else
            {
                SnackbarConsulta.MessageQueue.Enqueue("No hay una cadena de conexion establecida!");
            }
           
           
            

            colorOriginal = Dock.Background;
        }

        private void ConsultarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            
            Consultar();
            ConsultatxtBox.Focus();
            ConsultatxtBox.SelectAll();
        }

        private void ConsultarButton_Click(object sender, RoutedEventArgs e)
        {
            Consultar();
            ConsultatxtBox.Focus();
            ConsultatxtBox.SelectAll();
        }

      

        #region Funciones internas

        internal void Consultar()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CadenaDeConexion))
            {
                try
                {


                    if (!string.IsNullOrEmpty(ConsultatxtBox.Text))
                    {
                        var listTemp = Consulta.ReadAsync<Lote>(string.Format("Orden=\"{0}\"", ConsultatxtBox.Text)).Result;
                        if (listTemp.Count > 0)
                        {
                            Clipboard.SetText(listTemp[0].NumeroDeLote);
                            SnackbarConsulta.MessageQueue.Enqueue(string.Format("Se copio el siguente Lote: {0} al portapapeles", listTemp[0].NumeroDeLote));
                        }
                        else
                        {
                            Clipboard.SetText("N/A");
                            SnackbarConsulta.MessageQueue.Enqueue("Orden sin registro de lote, N/A");
                        }

                    }
                    else
                    {
                        SnackbarConsulta.MessageQueue.Enqueue("El campo Consulta no puede estar vacio");
                        ConsultatxtBox.Focus();
                    }

                }
                catch (Exception error)
                {

                    SnackbarConsulta.MessageQueue.Enqueue(error.Message);
                }
            }
            else
            {
                SnackbarConsulta.MessageQueue.Enqueue("No hay una cadena de conexion establecida!");
            }
           


        }

        private  void Registrar()
        {
           
          
                Lote temp = new Lote();
                try
                {
                    if (!(string.IsNullOrEmpty(OrdenTxtBox.Text) || string.IsNullOrEmpty(LoteTxtbox.Text)))
                    {
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.CadenaDeConexion))
                    {
                        temp.Orden = OrdenTxtBox.Text;
                        temp.NumeroDeLote = LoteTxtbox.Text;
                        var ListaTemp = Consulta.ReadAsync<Lote>(string.Format("Orden=\"{0}\"", OrdenTxtBox.Text)).Result;
                        if (ListaTemp.Count > 0)
                        {
                            Consulta.UpdateAsyn(ListaTemp[0], temp);
                            if (Consulta.Estado == Estados.Operacion_Exitosa)
                            {
                                snackbarNormal.Background = colorOriginal;
                                snackbarNormal.MessageQueue.Enqueue(string.Format("La orden {0} se ha actualizado!", temp.Orden, temp.NumeroDeLote));
                            }
                            else
                            {
                                snackbarNormal.Background = Brushes.Red;
                                snackbarNormal.MessageQueue.Enqueue("El lote no se Actualizo correctamente");
                            }

                        }
                        else
                        {
                            Consulta.InsertarAsync(temp);
                            if (Consulta.Estado == Estados.Operacion_Exitosa)
                            {
                                snackbarNormal.Background = colorOriginal;
                                snackbarNormal.MessageQueue.Enqueue(string.Format("Lote: {0}, Order: {1} Registrados!", temp.NumeroDeLote, temp.Orden));
                            }
                            else
                            {
                                snackbarNormal.Background = Brushes.Red;
                                snackbarNormal.MessageQueue.Enqueue("No se pudo registrar el lote!");
                            }
                        }

                    }
                    else
                    {
                        SnackbarConsulta.MessageQueue.Enqueue("No hay una cadena de conexion establecida!");
                    }


                }
              

                    else
                    {
                        snackbarNormal.MessageQueue.Enqueue("No se puede dejar vacios los campos!");
                    }

                }
                catch (Exception error)
                {
                    snackbarNormal.Background = Brushes.Red;
                    snackbarNormal.MessageQueue.Enqueue(error.Message);

                }
            
         
           
           

            

        }

        private bool WifiAvaible()
        {

         
                  

                   



                        if (NetworkInterface.GetIsNetworkAvailable())
                        {
                            IsConected = true;
                            WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.Foreground = System.Windows.Media.Brushes.Green; });
                            WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.ToolTip = "Conectado a la red"; });
                            return true;
                        }
                        else
                        {
                            IsConected = false;
                            WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.Foreground = System.Windows.Media.Brushes.Red; });
                            WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.ToolTip = "Conexion no disponible!"; });
                            WifiSnackbar.Dispatcher.Invoke(() => { WifiSnackbar.MessageQueue.Enqueue("Sin conexion a Internet, imposible realizar la operacion"); });
                            return false;
                        }
                 








              
        }

     



        #endregion

        private  void ResgistrarButton_Click(object sender, RoutedEventArgs e)
        {
            Registrar();
            OrdenTxtBox.Clear();
            LoteTxtbox.Clear();
            OrdenTxtBox.Focus();
               
        }

        private  void ResgistrarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Registrar();
            OrdenTxtBox.Clear();
            LoteTxtbox.Clear();
            OrdenTxtBox.Focus();
        }
    }
}
