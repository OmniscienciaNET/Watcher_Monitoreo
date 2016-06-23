using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brLocal_Subscriber:brGeneral
    {
        public List<beLocal_subscriber> ComboLocal_subscriber()
        {
            List<beLocal_subscriber> lbeLocal_subscriber = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daLocal_subscriber odaLocal_subscriber = new daLocal_subscriber();
                    lbeLocal_subscriber = odaLocal_subscriber.ComboLocal_subscriber(con);
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
            return (lbeLocal_subscriber);
        }


        public List<beLocal_subscriber> Buscar_Local_subscriber(string pLocalid, string pLocaldesc)
        {
            List<beLocal_subscriber> lbeLocal_subscriber = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daLocal_subscriber odaLocal_subscriber = new daLocal_subscriber();
                    lbeLocal_subscriber = odaLocal_subscriber.Buscar_Local_Subscriber(con,pLocalid,pLocaldesc);
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
            return (lbeLocal_subscriber);
        }

    }
}
