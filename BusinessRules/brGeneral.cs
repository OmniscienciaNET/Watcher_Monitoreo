using General.Librerias.CodigoUsuario;
using System;
using System.Configuration;
using Entity;

namespace BusinessRules
{
    public class brGeneral
    {
        public string Conexion { get; set; }
        public string ArchivoLog { get; set; }

        public brGeneral()
        {
            try
            {
                Conexion = ConfigurationManager.ConnectionStrings["ConWatcher"].ConnectionString;
                string rutaLog = ConfigurationManager.AppSettings["rutaLog"];
                ArchivoLog = String.Format("{0}{1}.txt", rutaLog, "Log" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString());
            }
            catch (Exception ex)
            {
                beLog obeLog = new beLog();
                obeLog.MensajeError = ex.Message;
                obeLog.DetalleError = ex.StackTrace;
                ucObjeto.Grabar(obeLog,ArchivoLog);
            }
        }
    }
}
