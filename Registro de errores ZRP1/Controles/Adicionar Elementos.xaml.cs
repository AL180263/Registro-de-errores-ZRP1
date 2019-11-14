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
using Entities;
using Registro_de_errores_ZRP1.Database_Context;
using MaterialDesignThemes.Wpf;

namespace Registro_de_errores_ZRP1.Controles
{
    /// <summary>
    /// Lógica de interacción para Adicionar_Elementos.xaml
    /// </summary>
    public partial class Adicionar_Elementos : Window, IDisposable
    {

        private ConexionDB db;


        public Adicionar_Elementos(ConexionDB conexion = null)
        {
            InitializeComponent();


            SnackbarMessageQueue messageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            UsuarioSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            ProblemasSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            UsuarioPermisoCombo.ItemsSource = Enum.GetValues(typeof(Access));
            try
            {
                if (conexion != null)
                {
                    db = conexion;
                }
                else
                {
                    db = new ConexionDB();
                }
               
            }
            catch (Exception Error)
            {
                new LogManager(Error);
               
            }
       
        }

        

        #region Usuarios

        private async void DeleteUsuario_Click(object sender, RoutedEventArgs e)
        {
           
                if (UsuariosListView.SelectedItem != null)
                {
                try
                {
                    db.Usuarios.Remove(UsuariosListView.SelectedItem as Usuario);
                    await db.SaveChangesAsync();

                    await ActualizarAsync();
                    Clear();
                }
                catch (Exception Error)
                {

                    new LogManager(Error);
                }
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("Seleccione un Item para eliminar");
                }
           
           
        }

        private void UsuariosListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (UsuariosListView.SelectedItem != null)
            {
                UsuarioNombretxt.Text = (UsuariosListView.SelectedItem as Usuario).Nombre;
                UsuarioUserNametxt.Text = (UsuariosListView.SelectedItem as Usuario).UserName;
                UsuarioUserNametxt.IsEnabled = false;
                UsuarioPermisoCombo.Text = (UsuariosListView.SelectedItem as Usuario).Acceso.ToString();
                usuarioRelojtxt.Text = (UsuariosListView.SelectedItem as Usuario).Reloj;
            }
            else
            {
                UsuarioSnackbar.MessageQueue.Enqueue("Seleccione un Item para Editar");
            }

        }

        private async void GuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            
                if (UsuariosListView.SelectedItem != null)
                {
                Usuario temp = UsuariosListView.SelectedItem as Usuario;

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
                    //if (!string.IsNullOrEmpty(UsuarioUserNametxt.Text))
                    //{
                    //temp.UserName = UsuarioUserNametxt.Text.ToUpperInvariant();
                    //}
                    //else
                    //{
                    //    UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"UserName\" no puede esta vacio");
                    //    UsuarioUserNametxt.Focus();
                    //    return;
                    //}
                if (!string.IsNullOrEmpty(usuarioRelojtxt.Text) && usuarioRelojtxt.Text.Length == 6)
                {
                    temp.Reloj = usuarioRelojtxt.Text;
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Reloj\" no puede esta vacio");
                    usuarioRelojtxt.Focus();
                    return;
                }
                    if (UsuarioPermisoCombo.SelectedItem != null)
                    {
                  
                    temp.Acceso = (Access)UsuarioPermisoCombo.SelectedItem;
                    }
                    else
                    {
                        UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Permiso\" no puede esta vacio");
                        UsuarioPermisoCombo.Focus();
                        return;
                    }

                try
                {
                    await db.SaveChangesAsync();
                    Clear();
                   await  ActualizarAsync();
                }
                catch (Exception Error)
                {

                    new LogManager(Error);
                }
                   
                   
                }
                else
                {
                    UsuarioSnackbar.MessageQueue.Enqueue("No se habia seleccionado un usuario previamente!");

                    return;
                }
           
          

        }

        private async void AdicionarNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            Usuario temp = new Usuario();


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
            if (!string.IsNullOrEmpty(usuarioRelojtxt.Text) && usuarioRelojtxt.Text.Length == 6)
            {
                temp.Reloj = usuarioRelojtxt.Text;
            }
            else
            {
                UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Reloj\" no puede esta vacio");
                usuarioRelojtxt.Focus();
                return;
            }
            if (UsuarioPermisoCombo.SelectedItem != null)
            {
                temp.Acceso = (Access)UsuarioPermisoCombo.SelectedItem;
            }
            else
            {
                UsuarioSnackbar.MessageQueue.Enqueue("El Campo \"Permiso\" no puede esta vacio");
                UsuarioPermisoCombo.Focus();
                return;
            }

            try
            {
                db.Usuarios.Add(temp);
                await db.SaveChangesAsync();
                await  ActualizarAsync();
                Clear();
            }
            catch (Exception Error)
            {

                new LogManager(Error);
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

        private async Task ActualizarAsync()
        {
           
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        UsuariosListView.ItemsSource = db.Usuarios.ToList();
                    }
                    catch (Exception Error)
                    {

                        new LogManager(Error);
                    }
                });
            });

        }

        private void Clear()
        {
            UsuarioNombretxt.Clear();
            UsuarioUserNametxt.Clear();
            UsuarioUserNametxt.IsEnabled = true;
            UsuarioPermisoCombo.SelectedItem = null;
            UsuariosListView.SelectedItem = null;
            usuarioRelojtxt.Clear();
         
        }

        private async void Actualizar_Click(object sender, RoutedEventArgs e)
        {
                await ActualizarAsync();
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

        private async void Window_Initialized(object sender, EventArgs e)
        {
            await ActualizarAsync();
        }

        private void DeleteProfileImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GuardarImageProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CambiarImageProfile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
