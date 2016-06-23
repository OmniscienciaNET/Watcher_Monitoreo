using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brEventoAdministrativo:brGeneral
    {
        public bool RegistrarAlarma(string alarmcode, string csid, string PhysicalZone,string UserAssigned, out  string outIdAlarmhistory, out string outDateTimeOccurred)
        {
            bool updated = false;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daEventoAdministrativo odadaEventoAdministrativo = new daEventoAdministrativo();
                    updated = odadaEventoAdministrativo.RegistrarAlarma(con, alarmcode, csid, PhysicalZone,UserAssigned, out outIdAlarmhistory, out outDateTimeOccurred);
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
                    outDateTimeOccurred = "";
                    outIdAlarmhistory = "";
                }
                catch (Exception ex)
                {
                    beLog obeLog = new beLog();
                    obeLog.MensajeError = ex.Message;
                    obeLog.DetalleError = ex.StackTrace;
                    ucObjeto.Grabar(obeLog, ArchivoLog);
                    outDateTimeOccurred = "";
                    outIdAlarmhistory = "";
                }
            }
            return (updated);
        }
    }
}
