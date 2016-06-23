using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brControlCierre:brGeneral
    {
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarControlCierre(string Dstb, string Abnd, string Dpto, int TipoAbnd)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeControlCierre = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daControlCierre odaControlCierre = new daControlCierre();
                    lbeControlCierre = odaControlCierre.ListarControlCierre(con, Dstb, Abnd, Dpto, TipoAbnd);
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
            return (lbeControlCierre);
        }
    }
}
