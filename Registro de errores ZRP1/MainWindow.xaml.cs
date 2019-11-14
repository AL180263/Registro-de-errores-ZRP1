using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConsultaCore;
using ConsultaCore.ExcelInterop;
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
using System.Reflection;
using Entities;
using Registro_de_errores_ZRP1.Database_Context;
using System.Data.Entity;

namespace Registro_de_errores_ZRP1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        public  MainWindow()
        {
            InitializeComponent();

            LogManager.LogSuccessNotifyEvent += DialogHostExceptionNotify;
            
            ImpresoraSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1500));

            snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));

            try
            {
                db = new ConexionDB();

                #pragma warning disable CS0612 // El tipo o el miembro están obsoletos
                tema.SetLightDark(Settings.Default.IsOscuro);
                #pragma warning restore CS0612 // El tipo o el miembro están obsoletos
                #region Seleccionar elementos de los combos
                //Establecemos seleccion de las impresoras.
                ComboBoxImpresoras.SelectionChanged -= ComboBoxImpresoras_SelectionChanged;
                ComboBoxImpresoras.SelectedItem = Settings.Default.ImpresoraDefault;
                ImpresorasCombo.SelectedItem = Settings.Default.ImpresoraDefault;
                ComboBoxImpresoras.SelectionChanged += ComboBoxImpresoras_SelectionChanged;

             
                ///Establecemos la seleccion de el tipo de Etiqueta.
                ComboBoxEtiquetas.SelectionChanged -= ComboBoxEtiquetas_SelectionChanged;
                ComboBoxEtiquetas.SelectedItem = Settings.Default.LabelKind;
                ComboBoxEtiquetas.SelectionChanged += ComboBoxEtiquetas_SelectionChanged;

                //Establecemos los colores de la aplicacion.
                SwatchesProvider TemaApp = new SwatchesProvider();
                ComboBoxColores.ItemsSource = TemaApp.Swatches.OrderBy(o => o.Name);
                ComboBoxColores.SelectedIndex = Settings.Default.index;
                button.IsChecked = Settings.Default.IsOscuro;
                #endregion
                new LogManager("Se instancion correctamente ConexionDB()");

            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en MainWindow()",Error);

            }
        }

      

        private void DialogHostExceptionNotify(LogEventArgs args)
        {
            if (args.Exception != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    TextBlockInformacion.Text = args.Exception.Message;
                    DialogHostMessage.IsOpen = true;
                });
               
            }
        }





        #region Variables

        private List<Reporte> bufferError = new List<Reporte>();

        private Reporte context = new Reporte();

        PaletteHelper tema = new PaletteHelper();

        ConexionDB db;

        List<string> rellenar = new List<string>();

        public Reporte Old;

       

        #endregion

        #region Configuraciones

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (button.IsChecked ?? false)
            {
#pragma warning disable CS0612 // 'PaletteHelper.SetLightDark(bool)' está obsoleto
                tema.SetLightDark(true);
#pragma warning restore CS0612 // 'PaletteHelper.SetLightDark(bool)' está obsoleto
                Settings.Default.IsOscuro = true;
                Settings.Default.Save();

            }
            else
            {
#pragma warning disable CS0612 // 'PaletteHelper.SetLightDark(bool)' está obsoleto
                tema.SetLightDark(false);
#pragma warning restore CS0612 // 'PaletteHelper.SetLightDark(bool)' está obsoleto
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
#pragma warning disable CS0612 // 'PaletteHelper.ReplacePrimaryColor(Swatch)' está obsoleto
                    tema.ReplacePrimaryColor(ComboBoxColores.SelectedItem as Swatch);
#pragma warning restore CS0612 // 'PaletteHelper.ReplacePrimaryColor(Swatch)' está obsoleto
                    

                }
            }
            catch (Exception error)
            {

                new LogManager("Excepcion en funcion P_SelectionChanged", error);
            }

           

        }

        [Obsolete("Funcion Obsoleta",true)]
        private void ComboBoxProviders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

        }

        [Obsolete("Funcion obsoleta",true)]
        private void ButtonBuscarBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
            //using (OpenFileDialog db = new OpenFileDialog())
            //{
            //    db.Filter = "Archivos de Bases de datos Access (*.accdb;*.mdb)|*.accdb;*.mdb";
            //    db.DefaultExt = ".accdb";
            //    db.Title = "Selecciona una base de datos con el formato correspondiente";
            //    try
            //    {
            //        db.ShowDialog();

                   

                   

            //        if (!string.IsNullOrEmpty(db.FileName))
            //        {
                       
                       
                           
            //            //if (ConcurrentUsuario.PermisoEnum == PERMISO.Programador)
            //            //{
            //            //    AdministradorDb.NormalizeDatabase(new Usuarios(), new Departamento(), new Problemas(), new Errores(), new Turnos(), new Lote());
            //            //}
                          
                       
                          
                           
                       
                       
            //        }

            //        if (AdministradorDb.Conectar())
            //        {


            //            Settings.Default.CadenaDeConexion = AdministradorDb.CadenaDeConexion;
            //            Settings.Default.Save();
            //            Settings.Default.Upgrade();
            //            UpdateAllAsync();
            //        }




            //    }
            //    catch (Exception Error1A)
            //    {
            //        new Log(Error1A.ToString() + Environment.NewLine + Error1A.Message);
            //        TextBlockInformacion.Text = Error1A.Message;
            //        DialogHostMessage.IsOpen = true;

            //    }




            //}

        }

        [Obsolete("Funcion obsoleta", true)]
        private void ButtonGenerarBaseDeDatos_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private async void ButtonActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Limpiar();
                await UpdateAllAsync();
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en funcion ButtonActualizar_Click", Error); 
            }
           
        }

        private void ButtonAgregarUsuarios_Click(object sender, RoutedEventArgs e)
           
        {
            try
            {
                if (Usuario.UsuarioActual.Acceso == Access.Administrador || Usuario.UsuarioActual.Acceso == Access.Invitado)
                {
                    Adicionar_Elementos adicionar_Elementos = new Adicionar_Elementos(db);
                    adicionar_Elementos.Show();
                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("Esta Funcion Solo Esta Habilitada Para Administradores");
                }
            }
            catch (Exception Error)
            {

                new LogManager("Exepcion en funcion ButtonAgregarUsuarios_Click", Error);
            }
           
        }

        private void ComboBoxImpresoras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxImpresoras.SelectedItem != null)
                {
                    Properties.Settings.Default.ImpresoraDefault = ComboBoxImpresoras.SelectedItem as string;
                    Settings.Default.Save();
                }
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en Funcion ComboBoxImpresoras_SelectionChanged", Error);
            }
         
          

        }

        private void ComboBoxEtiquetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxEtiquetas.SelectedItem != null)
                {
                    Settings.Default.LabelKind = ComboBoxEtiquetas.SelectedItem as string;
                    Settings.Default.Save();
                }
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en Funcion ComboBoxEtiquetas_SelectionChanged", Error);
                
            }
           

        }

     

        #endregion

        #region Dialogo de informacion extra

        private async void BottonDialogAcceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await UpdateListViewAsync();
                DialogHostInformacionDelError.IsOpen = false;
            }
            catch (Exception Error)
            {

                new LogManager(Error);
            }
            
        }

        private  void BottonDialogEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Editar(ListviewErrores.SelectedItem as Reporte);
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en funcion BottonDialogEditar_Click", Error);
            }
        }

        private  void BottonDialogPDF_Click(object sender, RoutedEventArgs e)
        {
                try
                {
                    GeneradorDocument.GenerarPdf(ListviewErrores.SelectedItem as Reporte);
                }
                catch (Exception Error)
                {
                  new LogManager("Excepcion en funcion  BottonDialogPDF_Click", Error);

                }
        }

        private async void BottonDialogEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Usuario.UsuarioActual.Acceso == Access.Programador || Usuario.UsuarioActual.Acceso == Access.Administrador)
                {
                    using (ConexionDB db = new ConexionDB())
                    {
                        db.Reportes.Remove(ListviewErrores.SelectedItem as Reporte);
                    }


                    await UpdateListViewAsync();
                    DialogHostInformacionDelError.IsOpen = false;


                }
                else
                {
                    snackbarNormal.MessageQueue.Enqueue("Esta funcion solo esta habilitada para Administradores o Personal de alto rango");
                }
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en funcion BottonDialogEliminar_Click", Error);
                
            }
                
            
          
           
        }

        private void BottonDialogImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImpresorasCombo.ItemsSource = rellenar;
                MostrarImpresion.IsOpen = true;
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en funcion BottonDialogImprimir_Click", Error);
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

                               Entities.PrinterLabel.PrintThermalLabel(ListviewErrores.SelectedItem as Reporte, ImpresorasCombo.SelectedItem as string, Settings.Default.LabelKind);

                                MostrarImpresion.IsOpen = false;
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

                new LogManager("Excepcion en funcion ComenzarImprecionButton_Click", Error);
                    ImpresoraSnackbar.MessageQueue.Enqueue(Error.Message);

                }
  
        }

        #endregion

        #endregion

        #region Entrada de informacion

        private async void ButtonAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool IsManual = HoraManualCheckBox.IsChecked ?? false;

                Reporte t = ObtenerDatos(null,!IsManual);


                if (t != null)
                {
                    using (ConexionDB db = new ConexionDB())
                    {
                        db.Reportes.Add(t);

                        await db.SaveChangesAsync();
                    }

                    Entities.PrinterLabel.PrintThermalLabel(t, Usuario.UsuarioActual.UserName, Settings.Default.ImpresoraDefault, Settings.Default.LabelKind);
                    await UpdateListViewAsync();
                    Limpiar();
                }
                else
                {
                    return;
                }
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en funcion ButtonAgregar_Click", Error);
               
            }
            
           


        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Limpiar();
                ButtonAgregar.IsEnabled = true;
                ButtonUpdate.IsEnabled = false;
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en funcion ButtonClear_Click", Error);
            }
          
        }

        private async void TextBoxfiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await CriteriaAsync();
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en TextBoxfiltro_TextChanged", Error);
            }
          
        }

        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (context != null)
                {
                    await UpdateViewAsync(context);
                    DatosNuevosHora.IsEnabled = false;

                }
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en ButtonUpdate_Click", Error);
                
            }
               
          
           
        }

        #endregion

        #region Interfaz de informacion

        private async void ToggleButtonFiltrado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await UpdateListViewAsync();
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en ToggleButtonFiltrado_Click", Error);
            }
        }

        private async void ListviewErrores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                /*DatosShow = new Controles.InformacionAdicional((sender as System.Windows.Controls.ListView).SelectedItem as Problemas)*/
                try
                {
                   await BindAsync((sender as System.Windows.Controls.ListView).SelectedItem as Reporte);
                    DialogHostInformacionDelError.IsOpen = true;
                }
                catch (Exception Error)
                {
                    new LogManager("Excepcion en ListviewErrores_SelectionChanged", Error);
                }

            }

        }

        private async void ToggleButtonConfirmacionDeEstatus_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (db != null)
                {
                    context.Estatus = Estatus.Resuelto;
                    TextBlockDialogEstatus.Text = context.Estatus.ToString().Replace('_', ' ');
                    await db.SaveChangesAsync();
                    await UpdateListViewAsync();
                } 
            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en ToggleButtonConfirmacionDeEstatus_Checked", Error);
            }
            
        }

        private async void ToggleButtonConfirmacionDeEstatus_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (db != null)
                {
                    context.Estatus = Estatus.Sin_Resolver;
                    TextBlockDialogEstatus.Text = context.Estatus.ToString().Replace('_', ' ');
                    await db.SaveChangesAsync();
                    await UpdateListViewAsync();
                }

            }
            catch (Exception Error)
            {

                new LogManager("Excepcion en ToggleButtonConfirmacionDeEstatus_Unchecked", Error);
            }
           

        }

        private async void ListviewErrores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListviewErrores.SelectedItem != null)
            {
                try
                {
                   await BindAsync(ListviewErrores.SelectedItem as Reporte);
                    DialogHostInformacionDelError.IsOpen = true;
                }
                catch (Exception Error)
                {
                    new LogManager("Excepcion en ListviewErrores_MouseDoubleClick",Error);
                }
            }
        }

        #endregion

        #region  Funciones internas
        public async Task CriteriaAsync()
        {
          await Task.Run(() =>
            {
                this.Dispatcher.Invoke(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(TextBoxfiltro.Text))
                    {
                        if (bufferError.Count > 0)
                        {
                            ListviewErrores.SelectionChanged -= ListviewErrores_SelectionChanged;

                            string c = TextBoxfiltro.Text.ToLowerInvariant();
                            ListviewErrores.ItemsSource = (from o in bufferError
                                           where
                                           o.Hash.ToString().Contains(c) ||
                                           o.Orden.ToLowerInvariant().Contains(c) ||
                                           o.Material.ToLowerInvariant().Contains(c) ||
                                           o.Hu.Contains(c) ||
                                           o.Incidente.ToLowerInvariant().Contains(c) ||
                                           o.UserName.ToLowerInvariant().Contains(c) ||
                                           o.Estatus.ToString().ToLowerInvariant().Contains(c) ||
                                           o.Fecha.ToString("dd/MM/yyyy").Contains(c) ||
                                           o.Departamento.ToString().ToLowerInvariant().Contains(c)
                                           select o).ToList();

                            ListviewErrores.SelectionChanged += ListviewErrores_SelectionChanged;

                        }
                    }
                    else
                    {
                      await  UpdateListViewAsync();
                    }
                });
          });
        }

        public  void Editar(Reporte reporte)
        {

            ComboBoxTurnos.Text = reporte.Turno.ToString();
            ComboBoxUsuarios.SelectedItem = reporte.Usuario;
            DatosNuevosComboProblema.Text = reporte.Incidente.ToString();
            DatosNuevosComboResponsable.Text = reporte.Departamento.ToString();
            DatosNuevosHora.IsEnabled = true;
            DatosNuevosHora.Text = reporte.Fecha.ToString("h:mm tt", CultureInfo.InvariantCulture);
            DatosNuevosTextBoxHU.Text = reporte.Hu;
            DatosNuevosTextBoxInformacionAdicional.Text = reporte.Descripcion;
            DatosNuevosTextBoxMaterial.Text = reporte.Material;
            DatosNuevosTextBoxOrden.Text = reporte.Orden;
            Old = reporte;
            ButtonAgregar.IsEnabled = false;
            ButtonUpdate.IsEnabled = true;
            DialogHostInformacionDelError.IsOpen = false;
        }

        public async Task UpdateViewAsync(Reporte reporte)
        { 
                try
                {
                    Reporte buf = ObtenerDatos(reporte,false);
                    if (buf != null)
                    {
                        
                    
                      
                        await db.SaveChangesAsync();

                        snackbarNormal.MessageQueue.Enqueue(string.Format("Problema: {0}, Actualizado!", buf.Hash));
                        await UpdateListViewAsync();
                        Limpiar();
                        ButtonAgregar.IsEnabled = true;
                        ButtonUpdate.IsEnabled = false;

                    }
                }
                catch (Exception Error)
                {
                new LogManager("Excepcion en UpdateViewAsync",Error);
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

        public Reporte ObtenerDatos(Reporte buffer = null,bool Actual = true)
        {
            try
            {

                if (buffer == null)
                {
                    buffer = new Reporte();
                }
                if (ComboBoxTurnos.SelectedItem != null)
                {

                    buffer.Turno = (Turno)ComboBoxTurnos.SelectedItem;
                }
                else
                {
                  
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Turno\" no puede estar vacio");
                    ComboBoxTurnos.Focus();
                    return null;
                }
                if (ComboBoxUsuarios.SelectedItem != null)
                {
                    buffer.UserName = (ComboBoxUsuarios.SelectedItem as Usuario).UserName;
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Usuarios\" no puede estar vacio");
                    ComboBoxUsuarios.Focus();
                    return null;
                }
                if (DatosNuevosComboProblema.SelectedItem != null)
                {
                    buffer.Incidente = DatosNuevosComboProblema.SelectedItem.ToString();
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1500));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Problema\" no puede estar vacio");
                    DatosNuevosComboProblema.Focus();
                    return null;
                }


                buffer.Hu = DatosNuevosTextBoxHU.Text;
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
                    buffer.Departamento = (Departamento)DatosNuevosComboResponsable.SelectedItem;
                }
                else
                {
                    snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
                    snackbarNormal.MessageQueue.Enqueue("El campo \"Departamentos\" no puede estar vacio");
                    DatosNuevosComboResponsable.Focus();
                    return null;
                }

                buffer.Orden = DatosNuevosTextBoxOrden.Text;
                buffer.Estatus = Estatus.Sin_Resolver;
                buffer.Descripcion = DatosNuevosTextBoxInformacionAdicional.Text;


               

                return buffer;
                

            }
            catch (Exception Error)
            {

                new LogManager(Error);
                return new Reporte();
                
                    
            }
           
        }

        private async Task BindAsync(Reporte reporte)
        {
            
             await Task.Run(() =>
              {

                this.Dispatcher.Invoke( () =>
               
                {
                    context = reporte;
                    this.TextBlockDialogHashCode.Text = reporte.Hash.ToString();
                    this.TextBlockDialogProblema.Text = reporte.Incidente;
                    this.TextBlockDialogFecha.Text = reporte.Fecha.ToString();
                    this.TextBlockDialogOrden.Text = reporte.Orden;
                    this.TextBlockDialogMaterial.Text = reporte.Material;
                    this.TextBlockDialogHU.Text = reporte.Hu;
                    this.TextBlockDialogClerck.Text = reporte.Usuario.Nombre;
                    this.TextBlockDialogDepartamento.Text = reporte.Departamento.ToString();
                    if (reporte.Estatus == Estatus.Resuelto)
                    {
                        this.ToggleButtonConfirmacionDeEstatus.IsChecked = true;
                    }
                    else
                    {
                        this.ToggleButtonConfirmacionDeEstatus.IsChecked = false;
                    }
                   
                    this.spanderText.Text = reporte.Incidente;
                    this.TextBlockDialogInformacionAdicional.Text = reporte.Descripcion;
                    this.TextBlockDialogEstatus.Text = reporte.Estatus.ToString().Replace('_', ' ');
                }
                    
                    );
            }
            );
        }

        private async Task UpdateAllAsync()
        {
           
              
              
              await UpdatePrintersAsync();
              await UpdateLabelsAsync();

            var list = await db.Usuarios.ToListAsync();
            var result = list.Find(o => o.UserName.ToUpperInvariant() == Environment.UserName.ToUpperInvariant());
           

                    if (result != null)
                    {
                        Usuario.UsuarioActual = result;
                        if (ChipUsuario != null)
                        {
                    ChipUsuario.Content = Usuario.UsuarioActual.ToString();
                            if (Usuario.UsuarioActual.Imagen != null)
                            {
                                ChipUsuario.Icon = Usuario.UsuarioActual.Imagen.ToImage();
                            }
                        }

                    }
            else
            {
                Usuario.UsuarioActual = new Usuario("AA00", "Invitado", "000000", Access.Invitado);
                ChipUsuario.Content = Usuario.UsuarioActual.ToString();
            }


            this.ComboBoxUsuarios.ItemsSource = db.Usuarios.ToList();

                    this.ComboBoxTurnos.ItemsSource = Enum.GetValues(typeof(Turno));
                    this.DatosNuevosComboResponsable.ItemsSource = Enum.GetValues(typeof(Departamento));
                    this.DatosNuevosComboProblema.ItemsSource = Reporte.Incidentes;
                    HoraMargen.Text = DateTime.Now.ToString("dd/MM/yyyy");
                
 
                await UpdateListViewAsync();
               
          
        }

        private async Task UpdatePrintersAsync()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
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
                });
            });
          
        }

        private async Task UpdateLabelsAsync()
        {

            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
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

                });
            });
          
        }

        private async Task UpdateListViewAsync()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {

                    ListviewErrores.SelectionChanged -= ListviewErrores_SelectionChanged;

                   
                        if (ToggleButtonFiltrado.IsChecked ?? true)
                        {
                             var con = from consulta in db.Reportes
                                       where consulta.Estatus == Estatus.Resuelto
                                       select consulta;
                            this.ListviewErrores.ItemsSource = con.ToList();
                            bufferError = con.ToList();
                         
                        }
                        else
                        {
                            var con = from consulta in db.Reportes
                                      where consulta.Estatus == Estatus.Sin_Resolver
                                      select consulta;
                            this.ListviewErrores.ItemsSource = con.ToList();
                            bufferError = con.ToList();
                        }
                    
             
                    ListviewErrores.SelectionChanged += ListviewErrores_SelectionChanged;
                });
            });
                
                
            
          
           
          
          
        }

        private void ChipUsuario_Click(object sender, RoutedEventArgs e)
        {
           
                RegistroResina Resina = new RegistroResina();
                Resina.Show();
            
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Limpiar();
            }

        }

        private async void BotonGenerarExcel_Click(object sender, RoutedEventArgs e)
        {
            List<Reporte> pem = ListviewErrores.Items.Cast<Reporte>().ToList();


            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = string.Format("{0}\\Reporte_{1}", folder.SelectedPath, DateTime.Now.ToString("dd_MM_yyyy"));
                    await  pem.ToExcelDocumentAsync(path);


                }
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

                   
                    db.Dispose();
                    db = null;
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

        private async void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                await UpdateAllAsync();
            }
            catch (Exception Error)
            {
                new LogManager("Excepcion en Window_Loaded", Error);
            }

        }
    }
      
   
    
}
