using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brEstadisticioAlarmas:brGeneral
    {
        public List<beEstadisticoAlarmas> ListarEstadisticoAlarmas(string Dstb, string Abnd, string Alarm, string BDate, string BTime, string EDate, string ETime, string Dpto, int TipoAbnd)
        {
            List<beEstadisticoAlarmas> lbeEstadisticoAlarmas = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daEstadisticoAlarmas odaEstadisticoAlarmas = new daEstadisticoAlarmas();
                    lbeEstadisticoAlarmas = odaEstadisticoAlarmas.ListarEstadisticoAlarmas(con, Dstb, Abnd, Alarm, BDate, BTime, EDate, ETime, Dpto, TipoAbnd);
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
            return (lbeEstadisticoAlarmas);
        }
    }
}
