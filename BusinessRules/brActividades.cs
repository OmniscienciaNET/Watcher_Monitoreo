using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brActividades:brGeneral
    {
        public List<beAlarmasResolution> ListarAlarmas()
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daActividades odaActividad = new daActividades();
                    lbeAlarmas = odaActividad.ListarAlarmas(con);
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
                    ucObjeto.Grabar(obeLog,ArchivoLog);
                }
            }
            return (lbeAlarmas);
        }

        public List<beActividad> ListarActividad(string DstbCode, string AbndCode, string CodeArea, string AlarmCode, string AlarmDesc, string BeginDate, string EndDate, string BeginHour, string EndHour, int CondEstab, int PeriodoTest, string Dpto, int TipoAbnd)
        {
            List<beActividad> lbeActividad = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daActividades odaActividad = new daActividades();
                    lbeActividad = odaActividad.ListarActividad(con, DstbCode, AbndCode, CodeArea, AlarmCode, AlarmDesc, BeginDate, EndDate, BeginHour, EndHour, CondEstab, PeriodoTest, Dpto, TipoAbnd);
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
            return (lbeActividad);
        }
    }
}
