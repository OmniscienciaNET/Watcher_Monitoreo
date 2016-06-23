using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brVisor:brGeneral
    {
        public List<beVisor> ListarVisor(string Dstb, string Abnd, string Evitar, string Dpto, int TipoAbnd, string Localid, int TipoParticionid)
        {
            List<beVisor> lbeVisor = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daVisor odaVisor = new daVisor();
                    lbeVisor = odaVisor.ListarVisor(con, Dstb, Abnd, Evitar, Dpto, TipoAbnd, Localid, TipoParticionid);
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
            return (lbeVisor);
        }

        public beComentarioLlamada ListarComentariosLlamadas(int ahid)
        {
            beComentarioLlamada obeComentarioLlamada = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daVisor odaVisor = new daVisor();
                    obeComentarioLlamada = odaVisor.ListarComentariosLlamadas(con, ahid);
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
            return (obeComentarioLlamada);
        }
    }
}
