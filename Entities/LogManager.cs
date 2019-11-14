using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.
namespace Entities
{
    /// <summary>
    /// Controla y transmite eventos y sucesos de importancia.
    /// </summary>
    public class LogManager
    {


        public delegate void LogEventHandler(LogEventArgs args);
        /// <summary>
        /// Notifica cuando un error o un evento de suma relevancia debe ser registrado.
        /// </summary>
        public static event LogEventHandler LogSuccessNotifyEvent;

        /// <summary>
        /// Registra un nuevo evento con fecha y mensaje.
        /// </summary>
        /// <param name="Message"></param>
        public  LogManager(string Message)
        {

            LogManager.LogSuccessNotifyEvent += LogManager_LogSuccessNotifyEvent;
           
            OnLogEventHandler(new LogEventArgs() { Event = Message, EventTime = DateTime.Now });
        }

        private void LogManager_LogSuccessNotifyEvent(LogEventArgs args)
        {
            try
            {
              #if !WINDOWS_UWP
                using (TextWriter writer = File.AppendText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\LogRegister.txt"))
                {

                    writer.WriteLine($"--- --- --- --- {args.EventTime.ToString()} --- --- --- ---");
                    writer.WriteLine($"Entry : {args.Event}");
                    if (args.Exception != null)
                    {
                        writer.WriteLine("Se ha producido una excepcion interna con la siguiente informacion :");
                        writer.WriteLine("--- * --- * ---- * --- * --- * ---- * --- * --- * ---- * --- * --- * ---- *");
                        writer.WriteLine(args.Exception.ToString());
                        writer.WriteLine("--- * --- * ---- * --- * --- * ---- * --- * --- * ---- * --- * --- * ---- *");
                    }
                    writer.WriteLine($"--- --- --- --- Fin del registro --- --- --- ---");
                }
             #endif

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Registra un nuevo evento con un mensaje referente a la exception y su respectiva fecha.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="exception"></param>
        public LogManager(string Message,Exception exception)
        {
            OnLogEventHandler(new LogEventArgs() { Event = Message, Exception = exception, EventTime = DateTime.Now });
        }
        /// <summary>
        /// Regista un nuevo evento producido meramente por una exepcion.
        /// </summary>
        /// <param name="exception"></param>
        public LogManager(Exception exception)
        {
            OnLogEventHandler(new LogEventArgs() { Exception = exception, EventTime = DateTime.Now, Event = exception.Message });
        }

        protected virtual void OnLogEventHandler(LogEventArgs e)
        {
            LogEventHandler handler = LogSuccessNotifyEvent;
            handler?.Invoke(e);
        }

    }

    /// <summary>
    /// Contiene datos de registro de interes, o suceso de la aplicacion.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Mensaje o evento a registrar.
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// Registro de excepcion en caso de que el evento que registro el suceso produsca <see cref="System.Exception"/>.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Momento en que se registro el evento;
        /// </summary>
        public DateTime EventTime { get; set; }
      

    }

}
