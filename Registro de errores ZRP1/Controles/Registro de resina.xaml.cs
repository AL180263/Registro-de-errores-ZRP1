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
using Entities;
using Registro_de_errores_ZRP1.Database_Context;


namespace Registro_de_errores_ZRP1.Controles
{
    /// <summary>
    /// Lógica de interacción para RegistroResina.xaml
    /// </summary>
    public partial class RegistroResina : Window
    {

        
        Brush colorOriginal;

        public RegistroResina()
        {
            InitializeComponent();
           
            this.snackbarNormal.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2500));
            this.SnackbarConsulta.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2500));
          
            colorOriginal = Dock.Background;
        }

        private async void ConsultarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            
            await ConsultarAsync();
            ConsultatxtBox.Focus();
            ConsultatxtBox.SelectAll();
        }

        private async void ConsultarButton_Click(object sender, RoutedEventArgs e)
        {
            await ConsultarAsync();
            ConsultatxtBox.Focus();
            ConsultatxtBox.SelectAll();
        }

      

        #region Funciones internas

        internal async Task ConsultarAsync()
        {
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {


                        if (!string.IsNullOrEmpty(ConsultatxtBox.Text))
                        {
                            using (ConexionDB db = new ConexionDB())
                            {
                                var resultado = from res in db.LotesOrdenes
                                                where res.Orden == ConsultatxtBox.Text
                                                orderby res.FechaInsercion descending
                                                select res;


                               
                                if (resultado.Count() > 0)
                                {
                                    Clipboard.SetText(resultado.First().Lote);
                                    SnackbarConsulta.MessageQueue.Enqueue(string.Format("Se copio el siguente Lote: {0} al portapapeles", resultado.First().Lote));
                                }
                                else
                                {
                                    Clipboard.SetText("N/A");
                                    SnackbarConsulta.MessageQueue.Enqueue("Orden sin registro de lote, N/A");
                                }

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
                });
            });
               
            
            
           


        }

        internal async Task RegistrarAsync()
        {

        await Task.Run(() =>
        {
            this.Dispatcher.Invoke(() =>
            {
                LoteOrden temp = new LoteOrden();
                try
                {
                    if (!(string.IsNullOrEmpty(OrdenTxtBox.Text) || string.IsNullOrEmpty(LoteTxtbox.Text)))
                    {

                        temp.Orden = OrdenTxtBox.Text;
                        temp.Lote = LoteTxtbox.Text;
                        temp.FechaInsercion = DateTime.Now;
                        using (ConexionDB db = new ConexionDB())
                        {
                            if (db.LotesOrdenes.Count() > 0)
                            {
                                db.LotesOrdenes.Add(temp);
                                db.SaveChanges();

                                snackbarNormal.Background = colorOriginal;
                                snackbarNormal.MessageQueue.Enqueue(string.Format("Se agrego un nuevo registro!"));




                            }
                            else
                            {
                                db.LotesOrdenes.Add(temp);
                                db.SaveChanges();
                                snackbarNormal.Background = colorOriginal;
                                snackbarNormal.MessageQueue.Enqueue(string.Format("Lote: {0}, Order: {1} Registrados!", temp.Lote, temp.Orden));

                            }
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
            });
        });



           
            
         
           
           

            

        }

    

     



        #endregion

        private async  void ResgistrarButton_Click(object sender, RoutedEventArgs e)
        {
            await RegistrarAsync();
            OrdenTxtBox.Clear();
            LoteTxtbox.Clear();
            OrdenTxtBox.Focus();
               
        }

        private async  void ResgistrarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            await RegistrarAsync();
            OrdenTxtBox.Clear();
            LoteTxtbox.Clear();
            OrdenTxtBox.Focus();
        }
    }
}
