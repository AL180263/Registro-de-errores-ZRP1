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
using Registro_de_errores_ZRP1.Tablas;
using MaterialDesignThemes.Wpf;
using System.Net.NetworkInformation;

namespace Registro_de_errores_ZRP1.Controles
{
    /// <summary>
    /// Lógica de interacción para Adicionar_Elementos.xaml
    /// </summary>
    public partial class Adicionar_Elementos : Window, IDisposable
    {
        CoreDataBaseAccess AdministradorDb = new CoreDataBaseAccess();

        private bool IsConected = false;

        public Adicionar_Elementos()
        {
            InitializeComponent();

            SnackbarMessageQueue messageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            UsuarioSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            ProblemasSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            DepartamentosSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            WifiSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            UsuarioPermisoCombo.ItemsSource = new List<string>() { "ADMINISTRADOR", "INTERMEDIO", "USUARIO" };

            if (!string.IsNullOrEmpty(Properties.Settings.Default.CadenaDeConexion))
            {
                AdministradorDb.CadenaDeConexion = Properties.Settings.Default.CadenaDeConexion;
            }

            if (!(AdministradorDb.Estado == Estados.SinCadenaDeConexion || AdministradorDb.Estado == Estados.Operacion_Fallida))
            {
                ActualizarAsync();
            }

        }

        #region Usuarios

        private void DeleteUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (UsuariosListView.SelectedItem != null)
                {
                    AdministradorDb.DeleteAsync(UsuariosListView.SelectedItem as Usuarios);
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("Seleccione un Item para eliminar");
                }
            }
           
        }

        private void UsuariosListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (UsuariosListView.SelectedItem != null)
            {
                UsuarioNombretxt.Text = (UsuariosListView.SelectedItem as Usuarios).Nombre;
                UsuarioUserNametxt.Text = (UsuariosListView.SelectedItem as Usuarios).UserName;
                UsuarioPermisoCombo.Text = (UsuariosListView.SelectedItem as Usuarios).Permiso;
            }
            else
            {
                UsuarioSnackbar.MessageQueue.Enqueue("Seleccione un Item para Editar");
            }

        }

        private void GuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (UsuariosListView.SelectedItem != null)
                {
                    Usuarios temp = new Usuarios();

                    if (!string.IsNullOrEmpty(UsuarioNombretxt.Text))
                    {
                        temp.Nombre = UsuarioNombretxt.Text;
                    }
                    else
                    {
                        UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Nombre\" no puede esta vacio");
                        UsuarioNombretxt.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(UsuarioUserNametxt.Text))
                    {
                        temp.UserName = UsuarioUserNametxt.Text;
                    }
                    else
                    {
                        UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"UserName\" no puede esta vacio");
                        UsuarioUserNametxt.Focus();
                        return;
                    }
                    if (UsuarioPermisoCombo.SelectedItem != null)
                    {
                        temp.Permiso = UsuarioPermisoCombo.SelectedItem as string;
                    }
                    else
                    {
                        UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Permiso\" no puede esta vacio");
                        UsuarioPermisoCombo.Focus();
                        return;
                    }

                    AdministradorDb.UpdateAsyn(UsuariosListView.SelectedItem as Usuarios, temp);
                    ActualizarAsync();
                    Clear();
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("No se habia seleccionado un usuario previamente!");

                    return;
                }
            }
          

        }

        private void AdicionarNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            Usuarios temp = new Usuarios();

            if (WifiAvaible())
            {
                if (!string.IsNullOrEmpty(UsuarioNombretxt.Text))
                {
                    temp.Nombre = UsuarioNombretxt.Text;
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Nombre\" no puede esta vacio");
                    UsuarioNombretxt.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(UsuarioUserNametxt.Text))
                {
                    temp.UserName = UsuarioUserNametxt.Text;
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"UserName\" no puede esta vacio");
                    UsuarioUserNametxt.Focus();
                    return;
                }
                if (UsuarioPermisoCombo.SelectedItem != null)
                {
                    temp.Permiso = UsuarioPermisoCombo.SelectedItem as string;
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Permiso\" no puede esta vacio");
                    UsuarioPermisoCombo.Focus();
                    return;
                }

                AdministradorDb.InsertarAsync(temp);
                ActualizarAsync();
                Clear();
            }
           
        
    }

        

        #endregion

        #region Control Interno

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Clear();
            }
        }

        private async void ActualizarAsync()
        {
            if (WifiAvaible())
            {
                UsuariosListView.ItemsSource = await AdministradorDb.ReadAsync<Usuarios>();
                ProblemasListView.ItemsSource = await AdministradorDb.ReadAsync<Errores>();
                DepartamentosListview.ItemsSource = await AdministradorDb.ReadAsync<Departamento>();
            }
           
        }

        private void Clear()
        {
            UsuarioNombretxt.Clear();
            UsuarioUserNametxt.Clear();
            UsuarioPermisoCombo.SelectedItem = null;
            UsuariosListView.SelectedItem = null;
            ProblemasListView.SelectedItem = null;
            ProblemasTextBox.Clear();
            DepartamentosListview.SelectedItem = null;
            DepartamentosTexbox.Clear();
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                try
                {
                    ActualizarAsync();
                }
                catch (Exception Error)
                {
                    new Log(Error.ToString() + Environment.NewLine + Error.Message);

                }
            }
           
           
        }

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
                WifiSnackbar.Dispatcher.Invoke(() => { WifiSnackbar.MessageQueue.Enqueue("Sin conexion a Internet, imposible realizar la operacion"); });
                return false;
            }
        }


        #endregion

        #region Problemas

        private void ProblemasListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
           
                if (ProblemasListView.SelectedItem != null)
                {
                    ProblemasTextBox.Text = (ProblemasListView.SelectedItem as Errores).Error;
                }
                else
                {
                    ProblemasSnackbar.MessageQueue.Enqueue("No se tiene ningun item previamente seleccionado");
                }
            
           
        }

        private void GuardarProblem_Click(object sender, RoutedEventArgs e)
        {
            Errores errores = new Errores();
            if (WifiAvaible())
            {
                if (ProblemasListView.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(ProblemasTextBox.Text))
                    {
                        errores.Error = ProblemasTextBox.Text;

                        AdministradorDb.UpdateAsyn(ProblemasListView.SelectedItem as Errores, errores);
                        if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                        {
                            Clear();
                            ActualizarAsync();
                        }
                        else
                        {
                            ProblemasSnackbar.MessageQueue.Enqueue("Ah ocurrido un error, no se ha podido registrar");
                        }


                    }
                    else
                    {
                        ProblemasSnackbar.MessageQueue.Enqueue("El campo Problemas no puede estar vacio");
                        ProblemasTextBox.Focus();
                    }

                }
                else
                {
                    ProblemasSnackbar.MessageQueue.Enqueue("No se ha seleccionado un item!");
                    ProblemasListView.Focus();
                }

            }


        }

        private void AgregarProblema_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (!string.IsNullOrEmpty(ProblemasTextBox.Text))
                {
                    AdministradorDb.InsertarAsync(new Errores() { Error = ProblemasTextBox.Text });
                    if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                    {
                        ActualizarAsync();
                        Clear();
                    }
                    else
                    {
                        ProblemasSnackbar.MessageQueue.Enqueue("Ha ocurrido un error no se puede actualizar");
                    }

                }
                else
                {
                    ProblemasSnackbar.MessageQueue.Enqueue("El campo Problemas no puede estar vacio");
                    ProblemasTextBox.Focus();
                }
            }
           
        }

        private void DeletePoblem_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (ProblemasListView.SelectedItem != null)
                {
                    AdministradorDb.DeleteAsync(ProblemasListView.SelectedItem as Errores);
                    Clear();
                    ActualizarAsync();
                }
                else
                {
                    ProblemasSnackbar.MessageQueue.Enqueue("Ningun Item seleccionado para eliminiar!");
                    ProblemasListView.Focus();
                }
            }
          

        }


        #endregion

        #region Departamentos

        private void DepartamentosDelete_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (DepartamentosListview.SelectedItem != null)
                {
                    AdministradorDb.DeleteAsync(DepartamentosListview.SelectedItem as Departamento);
                }
                else
                {
                    DepartamentosSnackbar.MessageQueue.Enqueue("No se selecciono ningun item para eliminar");
                }
            }
          
        }

        private void DepartamentosGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (DepartamentosListview.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(DepartamentosTexbox.Text))
                    {

                        AdministradorDb.UpdateAsyn((DepartamentosListview.SelectedItem as Departamento), new Departamento() { DepartamentosResponsables = DepartamentosTexbox.Text });
                        if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                        {
                            Clear();
                            ActualizarAsync();
                        }
                        else
                        {
                            DepartamentosSnackbar.MessageQueue.Enqueue("Ah ocurrido un error, no se ha podido actualizar el item!");
                        }
    ;
                    }
                    else
                    {
                        DepartamentosSnackbar.MessageQueue.Enqueue("El campo departamento no puede estar vacio!");
                        DepartamentosTexbox.Focus();
                    }

                }
                else
                {
                    DepartamentosSnackbar.MessageQueue.Enqueue("No se ha seleccionado ningun item");

                }
            }
          
        }

        private void DepartamentosAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (WifiAvaible())
            {
                if (!string.IsNullOrEmpty(DepartamentosTexbox.Text))
                {
                    AdministradorDb.InsertarAsync(new Departamento() { DepartamentosResponsables = DepartamentosTexbox.Text });
                    if (AdministradorDb.Estado == Estados.Operacion_Exitosa)
                    {
                        Clear();
                        ActualizarAsync();
                    }
                    else
                    {
                        DepartamentosSnackbar.MessageQueue.Enqueue("Ah ocurrido un error, no se ha registrado el elemento!");
                    }
                }
                else
                {
                    DepartamentosSnackbar.MessageQueue.Enqueue("El campo Departamentos no puede esta vacio");
                    DepartamentosTexbox.Focus();
                }
            }
            

        }

        private void DepartamentosListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DepartamentosListview.SelectedItem != null)
            {
                DepartamentosTexbox.Text = (DepartamentosListview.SelectedItem as Departamento).DepartamentosResponsables;
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

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~Adicionar_Elementos() {
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

        ~Adicionar_Elementos()
        {
            Dispose();
        }
        #endregion

    }
}
