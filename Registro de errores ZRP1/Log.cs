using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Entities;
namespace Registro_de_errores_ZRP1
{
  public class Log
    {
      

         private string m_exePath = string.Empty;
    public Log(string logMessage)
    {
        LogWrite(logMessage);
    }

    

    public void LogWrite(string logMessage)
    {
        m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        try
        {
            using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
            {
                StringLog(logMessage, w);
            }
        }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
        catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
        {
        }
    }

    public void StringLog(string logMessage, TextWriter txtWriter)
    {
        try
        {
            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            txtWriter.WriteLine("  :");
            txtWriter.WriteLine("  :{0}", logMessage);
            txtWriter.WriteLine("-------------------------------");
        }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
        catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
        {

        }
    }

        public void writerAll(string log)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "reporte.txt"))
                {
                    w.Write(log);
                }
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
            }

        }

    }
}
