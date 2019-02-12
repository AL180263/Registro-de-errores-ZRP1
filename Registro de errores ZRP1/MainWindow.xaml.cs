using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConsultaCore;
using Registro_de_errores_ZRP1.Tablas;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;
using System.Data;
using System.Windows.Forms;
using Registro_de_errores_ZRP1.Properties;
using System.Globalization;
using Registro_de_errores_ZRP1.Controles;
using System.Printing;
using System.IO;
using System.Net.NetworkInformation;


namespace Registro_de_errores_ZRP1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {


                NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

              

                tema.SetLightDark(Settings.Default.IsOscuro);

                if (!string.IsNullOrEmpty(Settings.Default.CadenaDeConexion))
                {
                    AdministradorDb.CadenaDeConexion = Settings.Default.CadenaDeConexion;
                
                    IconoDatabase.Foreground = System.Windows.Media.Brushes.Green;
                    IconoDatabase.ToolTip = Settings.Default.CadenaDeConexion;

                }
                else
                {
                   
                    IconoDatabase.Foreground = System.Windows.Media.Brushes.Red;
                    IconoDatabase.ToolTip = "No se ha especificado una cadena de conexion";
                }
                Actualizar();

                ImpresoraSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1500));

                HoraMargen.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (AdministradorDb.Conectar())
                {
                    if (AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", Environment.UserName.ToUpper())).Result.Count > 0)
                    {
                        ConcurrentUsuario = AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", Environment.UserName.ToLowerInvariant())).Result[0];
                        ChipUsuario.Content = ConcurrentUsuario.Nombre;
                    }
                    else
                    {
                        ConcurrentUsuario.PermisoEnum = PERMISO.Usuario;
                        ChipUsuario.Content = Environment.UserName.ToUpperInvariant();
                    }
                   
                }
                else
                {
                    ChipUsuario.Content = Environment.UserName.ToUpperInvariant();
                    ConcurrentUsuario.PermisoEnum = PERMISO.Usuario;
                }

                snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));

                #region Seleccionar elementos de los combos
                ComboBoxImpresoras.SelectionChanged -= ComboBoxImpresoras_SelectionChanged;
                ComboBoxImpresoras.SelectedItem = Settings.Default.ImpresoraDefault;
                ImpresorasCombo.SelectedItem = Settings.Default.ImpresoraDefault;
                ComboBoxImpresoras.SelectionChanged += ComboBoxImpresoras_SelectionChanged;

                ComboBoxProviders.SelectionChanged -= ComboBoxProviders_SelectionChanged;
                ComboBoxProviders.SelectedIndex = Settings.Default.Provedor;
                ComboBoxProviders.SelectionChanged += ComboBoxProviders_SelectionChanged;

                ComboBoxEtiquetas.SelectionChanged -= ComboBoxEtiquetas_SelectionChanged;
                ComboBoxEtiquetas.SelectedItem = Settings.Default.LabelKind;
                ComboBoxEtiquetas.SelectionChanged += ComboBoxEtiquetas_SelectionChanged;

                SwatchesProvider l = new SwatchesProvider();
                ComboBoxColores.ItemsSource = l.Swatches.OrderBy(o => o.Name);
                ComboBoxColores.SelectedIndex = Settings.Default.index;
                button.IsChecked = Settings.Default.IsOscuro;
                #endregion


            }
            catch (Exception main)
            {

                new Log(main.ToString());

            }



        }

     

        #region Variables

        private List<Error> bufferError = new List<Error>();

        private Error context = new Error();

        private Usuario ConcurrentUsuario = new Usuario() { PermisoEnum = PERMISO.Usuario };

        PaletteHelper tema = new PaletteHelper();

        CoreDataBaseAccess AdministradorDb = new CoreDataBaseAccess();

        List<string> rellenar = new List<string>();

        public Error Old;

        public bool IsConected = false;

        #endregion

        #region Configuraciones

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (button.IsChecked ?? false)
            {
                tema.SetLightDark(true);
                Settings.Default.IsOscuro = true;
                Settings.Default.Save();

            }
            else
            {
                tema.SetLightDark(false);
                Settings.Default.IsOscuro = false;
                Settings.Default.Save();
            }

        }

        private void P_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxColores.SelectedItem != null)
                {
                    Properties.Settings.Default.index = ComboBoxColores.SelectedIndex;
                    Properties.Settings.Default.Save();
                    tema.ReplacePrimaryColor(ComboBoxColores.SelectedItem as Swatch);

                }
            }
            catch (Exception error)
            {

                new Log(error.ToString() + Environment.NewLine + error.Message);
            }

           

        }

        private void ComboBoxProviders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxProviders.SelectedItem != null)
                {
                    Settings.Default.Provedor = ComboBoxProviders.SelectedIndex;
                    Settings.Default.Save();
                    AdministradorDb.Provider = ComboBoxProviders.SelectedItem as string;
                }
            }
            catch (Exception errores)
            {

                new Log(errores.ToString() + Environment.NewLine + errores.Message);
                TextBlockInformacion.Text = errores.Message;
                DialogHostMessage.IsOpen = true;
            }

        }

        private void ButtonBuscarBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog db = new OpenFileDialog())
            {
                db.Filter = "Archivos de Bases de datos Access (*.accdb;*.mdb)|*.accdb;*.mdb";
                db.DefaultExt = ".accdb";
                db.Title = "Selecciona una base de datos con el formato correspondiente";
                try
                {
                    db.ShowDialog();

                    AdministradorDb.Provider = ComboBoxProviders.SelectedItem as string;

                   

                    if (!string.IsNullOrEmpty(db.FileName))
                    {
                       
                       
                            AdministradorDb.PathDataBase = db.FileName;
                        //if (ConcurrentUsuario.PermisoEnum == PERMISO.Programador)
                        //{
                        //    AdministradorDb.NormalizeDatabase(new Usuario(), new Departamento(), new Error(), new Errores(), new Turnos(), new Lote());
                        //}
                          
                       
                          
                           
                       
                       
                    }

                    if (AdministradorDb.Conectar())
                    {


                        Settings.Default.CadenaDeConexion = AdministradorDb.CadenaDeConexion;
                        Settings.Default.Save();
                        Settings.Default.Upgrade();
                        Actualizar();
                    }




                }
                catch (Exception Error1A)
                {
                    new Log(Error1A.ToString() + Environment.NewLine + Error1A.Message);
                    TextBlockInformacion.Text = Error1A.Message;
                    DialogHostMessage.IsOpen = true;

                }




            }

        }

        private void ButtonGenerarBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConcurrentUsuario.PermisoEnum == PERMISO.Administrador)
                {
                    using (SaveFileDialog generar = new SaveFileDialog())
                    {
                        generar.Filter = "Archivos de Bases de datos Access (*.accdb;*.mdb)|*.accdb;*.mdb";

                        try
                        {
                            if (generar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                AdministradorDb.CreateDataBase(generar.FileName, new Error(), new Errores(), new Usuario(), new Turnos(), new Departamento(), new Lote());
                                if (AdministradorDb.Conectar())
                                {
                                    Settings.Default.CadenaDeConexion = AdministradorDb.CadenaDeConexion;
                                    Settings.Default.Save();
                                }
                            }
                          

                          

                        }
                        catch (Exception Error2A)
                        {
                            new Log(Error2A.ToString() + Environment.NewLine + Error2A.Message);
                            TextBlockInformacion.Text = Error2A.Message;
                            DialogHostMessage.IsOpen = true;

                        }



                    }
                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("Esta Funcion Solo Esta Habilitada Para Administradores");
                }
               
            }
            catch (Exception exception)
            {

                new Log(exception.ToString());
                TextBlockInformacion.Text = exception.Message;
                DialogHostMessage.IsOpen = true;
             
            }
           

        }

        private void ButtonActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Limpiar();
                Actualizar();
            }
            catch ( Exception ar)
            {
                new Log(ar.ToString() + Environment.NewLine + ar.Message);
                TextBlockInformacion.Text = ar.Message;
                DialogHostMessage.IsOpen = true;

                
            }
           
        }

        private void ButtonAgregarUsuarios_Click(object sender, RoutedEventArgs e)
           
        {
            try
            {
                if (ConcurrentUsuario.PermisoEnum == PERMISO.Administrador || ConcurrentUsuario.PermisoEnum == PERMISO.Programador)
                {
                    Adicionar_Elementos adicionar_Elementos = new Adicionar_Elementos();
                    adicionar_Elementos.Show();
                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("Esta Funcion Solo Esta Habilitada Para Administradores");
                }
            }
            catch (Exception error)
            {

                new Log(error.ToString() + Environment.NewLine + error.Message);
                snackbarNormal.MessageQueue.Enqueue(error.Message);
            }
           
        }

        private void ComboBoxImpresoras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImpresoras.SelectedItem != null)
            {
                Properties.Settings.Default.ImpresoraDefault = ComboBoxImpresoras.SelectedItem as string;
                Settings.Default.Save();
            }
          

        }

        private void ComboBoxEtiquetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxEtiquetas.SelectedItem != null)
            {
                Settings.Default.LabelKind = ComboBoxEtiquetas.SelectedItem as string;
                Settings.Default.Save();
            }
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            WifiAvaible(e.IsAvailable);
        }

        #endregion

        #region Dialogo de informacion extra

        private  void BottonDialogAcceptar_Click(object sender, RoutedEventArgs e)
        {
           
            if ( WifiAvaible())
            {
                AdministradorDb.UpdateAsyn((ListviewErrores.SelectedItem as Error), context);

                actualizarListview();

                DialogHostInformacionDelError.IsOpen = false;
            }
           

        }

        private  void BottonDialogEditar_Click(object sender, RoutedEventArgs e)
        {
        
            if ( WifiAvaible())
            {
                Editar(ListviewErrores.SelectedItem as Error);
            }
           
        }

        private  void BottonDialogPDF_Click(object sender, RoutedEventArgs e)
        {
            if ( WifiAvaible())
            {
                try
                {
                    GeneradorDocument.GenerarPdf(ListviewErrores.SelectedItem as Error, AdministradorDb);
                }
                catch (Exception A5)
                {
                    new Log(A5.ToString());
                    TextBlockInformacion.Text = A5.ToString();
                    DialogHostMessage.IsOpen = true;

                }
            }
          
           

        }

        private  void BottonDialogEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (ConcurrentUsuario.PermisoEnum == PERMISO.Programador || ConcurrentUsuario.PermisoEnum == PERMISO.Administrador || ConcurrentUsuario.PermisoEnum == PERMISO.Intermedio)
                {
                    if (IsConected == true)
                    {
                        AdministradorDb.DeleteAsync(ListviewErrores.SelectedItem as Error);
                        actualizarListview();
                        DialogHostInformacionDelError.IsOpen = false;
                    }
                    else
                    {
                        snackbarNormal.MessageQueue.Enqueue("No hay conexion a la red, imposible Eliminar el registro");
                    }

                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("Esta funcion solo esta habilitada para Administradores o Personal de alto rango");
                }
            }
          
           
        }

        private void BottonDialogImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               

                ImpresorasCombo.ItemsSource = rellenar;
                MostrarImpresion.IsOpen = true;
            }
            catch (Exception error)
            {

                new Log(error.ToString() + Environment.NewLine + error.Message);
                snackbarNormal.MessageQueue.Enqueue(error.Message);
            }

           
           
        }

        #region Impresion

        private void CancelarImpresionButton_Click(object sender, RoutedEventArgs e)
        {
            MostrarImpresion.IsOpen = false;
            DialogHostInformacionDelError.IsOpen = true;

        }

    

        private  void ComenzarImprecionButton_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                try
                {
                    if (ListviewErrores.SelectedItem != null)
                    {

                        if (ImpresorasCombo.SelectedItem != null)
                        {

                            if (!string.IsNullOrEmpty(Settings.Default.LabelKind))
                            {
                                string filtro = ImpresorasCombo.SelectedItem as string;

                                PrintServer server = new PrintServer();

                                var listPrint = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

                                var seleccionada = listPrint.Where(o => o.FullName == filtro).ToList()[0];

                                PrinterLabel.PrintThermalLabel(ListviewErrores.SelectedItem as Error, ConcurrentUsuario.Nombre, ImpresorasCombo.SelectedItem as string, Settings.Default.LabelKind);

                                MostrarImpresion.IsOpen = false;

                                //new PrinterLabel().PrintLabel(ListviewErrores.SelectedItem as Error, ConcurrentUsuario.Nombre, seleccionada);



                                //new PrinterLabel().Print(ListviewErrores.SelectedItem as Error,ConcurrentUsuario.Nombre, ImpresorasCombo.SelectedItem.ToString());
                            }
                            else
                            {
                                ImpresoraSnackbar.MessageQueue.Enqueue("Seleccione una Etiqueta!");
                            }

                        }
                        else
                        {
                            ImpresoraSnackbar.MessageQueue.Enqueue("Seleccione una impresora!");
                        }
                    }
                    else
                    {
                        ImpresoraSnackbar.MessageQueue.Enqueue("Ningun reporte seleccionado");
                    }

                }
                catch (Exception Error)
                {

                    new Log(Error.ToString() + Environment.NewLine + Error.Message);
                    ImpresoraSnackbar.MessageQueue.Enqueue(Error.Message);

                }
            }

           
         

            //try
            //{
            //    string filtro = ImpresorasCombo.SelectedItem as string;

            //    PrintServer server = new PrintServer();

            //    var listPrint = server.GetPrintQueues(new[] {EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

            //    var seleccionada = listPrint.Where(o => o.Name == filtro).ToList()[0];

            //    new PrinterLabel(ListviewErrores.SelectedItem as Error, seleccionada);
            //    //ImpresionDeEtiqueta();
            //}
            //catch (Exception error)
            //{

            //    ImpresoraSnackbar.MessageQueue.Enqueue(error.Message);
            //    new Log(error.Message);
            //}
        }

        #endregion

        #endregion

        #region Entrada de informacion

        private void ButtonAgregar_Click(object sender, RoutedEventArgs e)
        {
            bool IsManual = HoraManualCheckBox.IsChecked ?? false;

            Error t = ObtenerDatos(!IsManual);
            
            if (WifiAvaible())
            {
                if (t != null)
                {
                    t.HashCode = t.GetHashCode();
                    AdministradorDb.InsertarAsync(t);
                    if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                    {
                        PrinterLabel.PrintThermalLabel(t, ConcurrentUsuario.Nombre, Settings.Default.ImpresoraDefault,Settings.Default.LabelKind);
                        actualizarListview();
                        Limpiar();
                    }
                    else
                    {
                        snackbarNormal.MessageQueue.Enqueue("A ocurrido un error, no se ha podido registrar el reporte");
                    }


                }
                else
                {
                    return;
                }
            }
            else
            {
                snackbarNormal.MessageQueue.Enqueue("No hay una conexion a la red, imposible registrar los datos");
            }


        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
            ButtonAgregar.IsEnabled = true;
            ButtonUpdate.IsEnabled = false;
        }

        private void TextBoxfiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Criteria();
        }

        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (Old != null)
                {
                    Actualizar(Old);
                    DatosNuevosHora.IsEnabled = false;

                }
            }
            else
            {
                snackbarNormal.MessageQueue.Enqueue("No hay conexion a la red, imposible Actualizar los datos!");
            
            }
           
        }

        #endregion

        #region Interfaz de informacion

        private void ToggleButtonFiltrado_Click(object sender, RoutedEventArgs e)
        {

            actualizarListview();

        }

        private void ListviewErrores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                /*DatosShow = new Controles.InformacionAdicional((sender as System.Windows.Controls.ListView).SelectedItem as Error)*/
                try
                {
                    Enlazar((sender as System.Windows.Controls.ListView).SelectedItem as Error);
                    DialogHostInformacionDelError.IsOpen = true;
                }
                catch (Exception Error)
                {
                    new Log(Error.ToString() + Environment.NewLine + Error.Message);
                    TextBlockInformacion.Text = Error.Message;
                    DialogHostMessage.IsOpen = true;

                }
               

            }

        }

        private void ToggleButtonConfirmacionDeEstatus_Checked(object sender, RoutedEventArgs e)
        {

            context.Estatus = true;
            TextBlockDialogEstatus.Text = context.EstatusString;
        }

        private void ToggleButtonConfirmacionDeEstatus_Unchecked(object sender, RoutedEventArgs e)
        {
            
            context.Estatus = false;
            TextBlockDialogEstatus.Text = context.EstatusString;
        }

        private void ListviewErrores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListviewErrores.SelectedItem != null)
            {
                try
                {
                    Enlazar(ListviewErrores.SelectedItem as Error);
                    DialogHostInformacionDelError.IsOpen = true;
                }
                catch (Exception Error)
                {
                    new Log(Error.ToString() + Environment.NewLine + Error.Message);
                    TextBlockInformacion.Text = Error.Message;
                    DialogHostMessage.IsOpen = true;
                }
              

            }
        }

        #endregion

        #region  Funciones internas

        private async void WifiAvaible(bool Isconected)
        {
             

          
          



            if (Isconected)
            {
               
                Ping ping = new Ping();

                try
                {
                    PingReply reply = await ping.SendPingAsync("www.google.com");

                    if (reply.Status == IPStatus.Success)
                    {
                        IsConected = true;
                        WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.Foreground = System.Windows.Media.Brushes.Green; });
                        WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.ToolTip = "Conectado a la red"; });
                    }
                    else
                    {
                        IsConected = false;
                        WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.Foreground = System.Windows.Media.Brushes.Red; });
                        WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.ToolTip = "Conexion no disponible!"; });
                    }
                }
                catch (Exception Error)
                {

                    new Log(Error.ToString());
                    IsConected = false;
                    
                }
             

                
                


               

            }
            else
            {
                IsConected = false;
                WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.Foreground = System.Windows.Media.Brushes.Red; });
                WifiEstatus.Dispatcher.Invoke(() => { WifiEstatus.ToolTip = "Conexion no disponible!"; });

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
                snackbarNormal.Dispatcher.Invoke(() => { snackbarNormal.MessageQueue.Enqueue("Sin conexion a Internet, imposible realizar la operacion"); });
                return false;
            }
        }

        public async void Criteria()
        {

          await Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (!string.IsNullOrWhiteSpace(TextBoxfiltro.Text))
                    {
                        if (bufferError.Count > 0)
                        {
                            ListviewErrores.SelectionChanged -= ListviewErrores_SelectionChanged;
                            ListviewErrores.ItemsSource = bufferError.Where(o => o.Departamento.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.HashCode.ToString().ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.Orden.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.Material.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.HU.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.Problema.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.Usuario.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant()) ||
                            o.EstatusString.ToLowerInvariant().Contains(TextBoxfiltro.Text.ToLowerInvariant())

                             );
                            ListviewErrores.SelectionChanged += ListviewErrores_SelectionChanged;

                        }






                    }
                    else
                    {
                        actualizarListview();
                    }
                });
          });
           

        }

        public  void Editar(Error error)
        {
            if ( WifiAvaible())
            {
                ComboBoxTurnos.Text = error.Turno;
                ComboBoxUsuarios.SelectedItem = AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", error.Usuario)).Result[0];
                DatosNuevosComboProblema.Text = error.Problema;
                DatosNuevosComboResponsable.Text = error.Departamento;
                DatosNuevosHora.IsEnabled = true;
                DatosNuevosHora.Text = error.Fecha.ToString("h:mm tt", CultureInfo.InvariantCulture);
                DatosNuevosTextBoxHU.Text = error.HU;
                DatosNuevosTextBoxInformacionAdicional.Text = error.Informacion_Extra;
                DatosNuevosTextBoxMaterial.Text = error.Material;
                DatosNuevosTextBoxOrden.Text = error.Orden;

                Old = new Error { Id = error.Id, Estatus = error.Estatus, Fecha = error.Fecha, HashCode = error.HashCode };
                ButtonAgregar.IsEnabled = false;
                ButtonUpdate.IsEnabled = true;
                DialogHostInformacionDelError.IsOpen = false;
            }
          

        }

        public void Actualizar(Error old)
        {
            if ( WifiAvaible())
            {
                try
                {
                    Error buf = ObtenerDatos(false);
                    if (buf != null)
                    {
                        buf.Estatus = old.Estatus;
                        buf.Fecha = new DateTime(old.Fecha.Year, old.Fecha.Month, old.Fecha.Day, buf.Fecha.Hour, buf.Fecha.Minute, 0);
                        buf.HashCode = old.HashCode;
                        AdministradorDb.UpdateAsyn(old, buf);
                        if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                        {
                            snackbarNormal.MessageQueue.Enqueue(string.Format("Problema: {0}, Actualizado!", buf.HashCode));
                            actualizarListview();
                            Limpiar();
                            ButtonAgregar.IsEnabled = true;
                            ButtonUpdate.IsEnabled = false;
                        }
                        else
                        {
                            snackbarNormal.MessageQueue.Enqueue("A ocurrido un error, No se actualizo el reporte, intente de nuevo!");
                        }



                    }
                    else
                    {
                        return;
                    }

                }
                catch (Exception A5)
                {
                    new Log(A5.Message);
                    TextBlockInformacion.Text = A5.Message;
                    DialogHostMessage.IsOpen = true;
                }
            }
          




        }

        public void Limpiar()
        {
            DatosNuevosHora.IsEnabled = false;
            ComboBoxTurnos.SelectedItem = null;
            ComboBoxUsuarios.SelectedItem = null;
            DatosNuevosComboProblema.SelectedItem = null;
            DatosNuevosComboResponsable.SelectedItem = null;
            DatosNuevosHora.SelectedTime = null;
            DatosNuevosTextBoxHU.Text = "";
            DatosNuevosTextBoxInformacionAdicional.Text = "";
            DatosNuevosTextBoxMaterial.Text = "";
            DatosNuevosTextBoxOrden.Text = "";
            if (!DialogHostInformacionDelError.IsOpen)
            {
                ListviewErrores.SelectionChanged -= ListviewErrores_SelectionChanged;
                ListviewErrores.SelectedItem = null;
                ListviewErrores.SelectionChanged += ListviewErrores_SelectionChanged;
            }
          

        }

        public Error ObtenerDatos(bool Actual = true)
        {
            try
            {
                Error buffer = new Error();

                if (ComboBoxTurnos.SelectedItem != null)
                {
                    buffer.Turno = ComboBoxTurnos.SelectedItem.ToString();
                }
                else
                {
                  
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Turno\" no puede estar vacio");
                    ComboBoxTurnos.Focus();
                    return null;
                }
                if (ComboBoxUsuarios.SelectedItem != null)
                {
                    buffer.Usuario = (ComboBoxUsuarios.SelectedItem as Usuario).UserName;
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Usuario\" no puede estar vacio");
                    ComboBoxUsuarios.Focus();
                    return null;
                }
                if (DatosNuevosComboProblema.SelectedItem != null)
                {
                    buffer.Problema = DatosNuevosComboProblema.SelectedItem.ToString();
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1500));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Problema\" no puede estar vacio");
                    DatosNuevosComboProblema.Focus();
                    return null;
                }


                buffer.HU = DatosNuevosTextBoxHU.Text;
                buffer.Material = DatosNuevosTextBoxMaterial.Text;

                if (Actual)
                {
                    buffer.Fecha = DateTime.Now;
                }
                else
                {
                    if (DatosNuevosHora.SelectedTime != null)
                    {
                        buffer.Fecha = DatosNuevosHora.SelectedTime.Value;
                    }
                    else
                    {
                        snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
                        snackbarNormal.MessageQueue.Enqueue("El campo \"Fecha\" no puede estar vacio");
                        DatosNuevosHora.Focus();
                        return null;
                    }
                }




                if (DatosNuevosComboResponsable.SelectedItem != null)
                {
                    buffer.Departamento = DatosNuevosComboResponsable.SelectedItem.ToString();
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Departamento\" no puede estar vacio");
                    DatosNuevosComboResponsable.Focus();
                    return null;
                }

                buffer.Orden = DatosNuevosTextBoxOrden.Text;
                buffer.Estatus = false;
                buffer.Informacion_Extra = DatosNuevosTextBoxInformacionAdicional.Text;


               

                return buffer;
                

            }
            catch (Exception b2)
            {

                new Log(b2.Message);
                return new Error();
                
                    
            }
           
        }

        private async  void Enlazar(Error data)
        {
            
          await Task.Factory.StartNew(() =>
              {

                this.Dispatcher.Invoke( () =>
               
                {
                    context = data;
                    this.TextBlockDialogHashCode.Text = data.HashCode.ToString();
                    this.TextBlockDialogProblema.Text = data.Problema;
                    this.TextBlockDialogFecha.Text = data.Fecha.ToString();
                    this.TextBlockDialogOrden.Text = data.Orden;
                    this.TextBlockDialogMaterial.Text = data.Material;
                    this.TextBlockDialogHU.Text = data.HU;
                    if ( WifiAvaible())
                    {
                        if (AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", data.Usuario)).Result.Count > 0)
                        {
                            this.TextBlockDialogClerck.Text = AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", data.Usuario)).Result[0].Nombre;
                        }
                    }
                    else
                    {
                        this.TextBlockDialogClerck.Text = data.Usuario;
                    }
                  


                    this.TextBlockDialogDepartamento.Text = data.Departamento;
                    this.ToggleButtonConfirmacionDeEstatus.IsChecked = data.Estatus;
                    this.spanderText.Text = data.Problema;
                    this.TextBlockDialogInformacionAdicional.Text = data.Informacion_Extra;
                    this.TextBlockDialogEstatus.Text = data.EstatusString;
                }
                    
                    );

            }
            );
                   
              
            

        }

        private async void Actualizar()
        {
            try
            {
              
                ComboBoxProviders.SelectionChanged -= ComboBoxProviders_SelectionChanged;
                ComboBoxProviders.ItemsSource = AdministradorDb.GetProviders();
                ComboBoxProviders.SelectionChanged += ComboBoxProviders_SelectionChanged;
                ActualizarImpresoras();
                ActualizarEtiquetas();
                if (WifiAvaible())
                {
                    if (!string.IsNullOrEmpty(AdministradorDb.CadenaDeConexion))
                    {
                        this.ComboBoxUsuarios.ItemsSource = await AdministradorDb.ReadAsync<Usuario>();
                        this.ComboBoxTurnos.ItemsSource = await AdministradorDb.ReadAsync<Turnos>();
                        IconoDatabase.ToolTip = AdministradorDb.CadenaDeConexion;
                        DatosNuevosComboProblema.ItemsSource = await AdministradorDb.ReadAsync<Errores>();
                        DatosNuevosComboResponsable.ItemsSource = await AdministradorDb.ReadAsync<Departamento>();
                        HoraMargen.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        actualizarListview();
                     
                      
                        
                        IconoDatabase.Foreground = System.Windows.Media.Brushes.Green;


                        if (AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", Environment.UserName.ToUpper())).Result.Count > 0)
                        {
                            ConcurrentUsuario = AdministradorDb.ReadAsync<Usuario>(string.Format("UserName=\"{0}\"", Environment.UserName.ToLowerInvariant())).Result[0];
                            ChipUsuario.Content = ConcurrentUsuario.Nombre;
                            if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                            {
                                IconoDatabase.Foreground = System.Windows.Media.Brushes.Green;
                                IconoDatabase.ToolTip = AdministradorDb.CadenaDeConexion;
                            }
                        }
                        else
                        {
                            ConcurrentUsuario.PermisoEnum = PERMISO.Usuario;
                            ChipUsuario.Content = Environment.UserName.ToUpperInvariant();
                        }

                    }
                    else
                    {
                        IconoDatabase.Foreground = System.Windows.Media.Brushes.Red;
                        IconoDatabase.ToolTip = "No se ha encontrado o especificado una cadena de conexion.";
                        throw new Exception("No se ha encontrado o especificado una cadena de conexion.");
                    }

                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("No hay conexion a la red, imposible Actualizar los datos!");
                }
               


               
            }
            catch (Exception b3)
            {
                new Log(b3.ToString());
                TextBlockInformacion.Text = b3.ToString();
                DialogHostMessage.IsOpen = true;
                
            }
          

        }

        private void ActualizarImpresoras()
        {
            ComboBoxImpresoras.SelectionChanged -= ComboBoxImpresoras_SelectionChanged;
            ComboBoxImpresoras.ItemsSource = null;
            ComboBoxImpresoras.Items.Clear();
            rellenar.Clear();
            var server = new PrintServer();

            var con = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });



            foreach (var item in con)
            {
                //if (item.Name.Contains("MP"))
                //{
                //    rellenar.Add(item.FullName);
                //}

                rellenar.Add(item.FullName);

            }

           

            ComboBoxImpresoras.ItemsSource = rellenar;
            ComboBoxImpresoras.SelectedItem = Settings.Default.ImpresoraDefault;
            ComboBoxImpresoras.SelectionChanged += ComboBoxImpresoras_SelectionChanged;
        }

        private void ActualizarEtiquetas()
        {
            ComboBoxEtiquetas.SelectionChanged -= ComboBoxEtiquetas_SelectionChanged;
            ComboBoxEtiquetas.ItemsSource = null;
            ComboBoxEtiquetas.Items.Clear();
           
            string[] ubicacion = Directory.GetFiles(Environment.CurrentDirectory + @"\Etiquetas");

            for (int i = 0; i < ubicacion.Length; i++)
            {
                ComboBoxEtiquetas.Items.Add(Path.GetFileName(ubicacion[i]));

            }
            ComboBoxEtiquetas.SelectedItem = Settings.Default.LabelKind;
            ComboBoxEtiquetas.SelectionChanged += ComboBoxEtiquetas_SelectionChanged;
        }

        async void actualizarListview()
        {
            if(WifiAvaible())
            {
                try
                {
                    ListviewErrores.SelectionChanged -= ListviewErrores_SelectionChanged;
                    if (ToggleButtonFiltrado.IsChecked ?? false)
                    {
                        this.ListviewErrores.ItemsSource = from n in AdministradorDb.ReadAsync<Error>().Result where n.Estatus == true select n;
                        bufferError = await AdministradorDb.ReadAsync<Error>();

                        var t = ListviewErrores.Items;




                    }
                    else
                    {
                        this.ListviewErrores.ItemsSource = from a in AdministradorDb.ReadAsync<Error>().Result where a.Estatus == false select a;
                        bufferError = await AdministradorDb.ReadAsync<Error>();
                    }
                    ListviewErrores.SelectionChanged += ListviewErrores_SelectionChanged;
                }
                catch (Exception b1)
                {
                    new Log(b1.ToString());
                    TextBlockInformacion.Text = b1.Message;
                    DialogHostMessage.IsOpen = true;
                }
            }
          
           
          
          
        }

        private void ChipUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (Environment.UserName.ToUpperInvariant() == "AH1075" || Environment.UserName.ToUpperInvariant() == "MC1139" || Environment.UserName.ToUpperInvariant() == "ALEXI")
            {
                Registro_de_resina Resina = new Registro_de_resina();
                Resina.Show();
            }
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Limpiar();
            }

        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AdministradorDb.Dispose();
                   
                }

                bufferError = null;
                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~MainWindow() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }

        ~MainWindow()
        {

            Dispose();




        }


        #endregion

        private void BotonGenerarExcel_Click(object sender, RoutedEventArgs e)
        {
            List<Error> pem = ListviewErrores.Items.Cast<Error>().ToList();

            if (WifiAvaible())
            {
                var iner = InteropExcelDataBase.ConvertIDateableToDataTable(pem.ToArray());
            }
           
            
        }
    }
      
   
    
}
