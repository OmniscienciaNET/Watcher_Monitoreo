using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brTipo_Abonado:brGeneral
    {
        public List<beTipo_Abonado> ComboTipo_Abonado()
        {
            List<beTipo_Abonado> lbeTipo_Abonado = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daTipo_Abonado odaTipo_Abonado = new daTipo_Abonado();
                    lbeTipo_Abonado = odaTipo_Abonado.ComboTipo_Abonado(con);
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
            return (lbeTipo_Abonado);
        }
    }
}
