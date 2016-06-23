using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace BusinessRules
{
    public class brAlarmconditions:brGeneral
    {
        public List<beAlarmConditions> ListarAlarmAdm()
        {
            List<beAlarmConditions> lbeAlarmConditions = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daAlarmconditions odaAlarmconditions = new daAlarmconditions();
                    lbeAlarmConditions = odaAlarmconditions.ListarAlarmAdm(con);
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
            return (lbeAlarmConditions);
        }
    }
}
