using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brControlArea:brGeneral
    {
        public List<beControlArea> ListarControlArea(string Dstb, string Abnd, string Alarm1, string Alrm2, string Zona, int Time, int BDays, string Dpto, int TipoAbnd)
        {
            List<beControlArea> lbeControlArea = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daControlArea odaControlArea = new daControlArea();
                    lbeControlArea = odaControlArea.ListarControlArea(con, Dstb, Abnd, Alarm1, Alrm2, Zona, Time, BDays, Dpto, TipoAbnd);
                }
                catch (SqlException ex)
                {
                    beLog obeLog;
                    foreach (SqlError err in ex.Errors)
                    {
                        obeLog = new beLog();
                        obeLog.MensajeError = err.Message;
                        obeLog.DetalleError = ex.StackTrace;
                        ucObjeto.Grabar(obeLog,ArchivoLog);
                    }
                }
                catch (Exception ex)
                {
                    beLog obeLog = new beLog();
                    obeLog.MensajeError = ex.Message;
                    obeLog.DetalleError = ex.StackTrace;
                    ucObjeto.Grabar(obeLog,ArchivoLog);
                }
            }
            return (lbeControlArea);
        }
        public List<beControlArea> ListarControlArea1864(string Dstb, string Abnd, string Alarm1, string Alrm2, string Zona, int Time, int BDays, string Dpto, int TipoAbnd)
        {
            List<beControlArea> lbeControlArea = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daControlArea odaControlArea = new daControlArea();
                    lbeControlArea = odaControlArea.ListarControlArea1864(con, Dstb, Abnd, Alarm1, Alrm2, Zona, Time, BDays, Dpto, TipoAbnd);
                }
                catch (SqlException ex)
                {
                    beLog obeLog;
                    foreach (SqlError err in ex.Errors)
                    {
                        obeLog = new beLog();
                        obeLog.MensajeError = err.Message;
                        obeLog.DetalleError = ex.StackTrace;
                        ucObjeto.Grabar(obeLog, ArchivoLog);
                    }
                }
                catch (Exception ex)
                {
                    beLog obeLog = new beLog();
                    obeLog.MensajeError = ex.Message;
                    obeLog.DetalleError = ex.StackTrace;
                    ucObjeto.Grabar(obeLog, ArchivoLog);
                }
            }
            return (lbeControlArea);
        }

        public List<beAlarmasResolution> ListarCondicionAlarmas()
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daControlArea odaControlArea = new daControlArea();
                    lbeAlarmas = odaControlArea.ListarCondicionAlarmas(con);
                }
                catch (SqlException ex)
                {
                    beLog obeLog;
                    foreach (SqlError err in ex.Errors)
                    {
                        obeLog = new beLog();
                        obeLog.MensajeError = err.Message;
                        obeLog.DetalleError = ex.StackTrace;
                        ucObjeto.Grabar(obeLog, ArchivoLog);
                    }
                }
                catch (Exception ex)
                {
                    beLog obeLog = new beLog();
                    obeLog.MensajeError = ex.Message;
                    obeLog.DetalleError = ex.StackTrace;
                    ucObjeto.Grabar(obeLog, ArchivoLog);
                }
            }
            return (lbeAlarmas);
        }
    }
}
