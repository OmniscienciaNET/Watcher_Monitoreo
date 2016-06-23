using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brSupervisionCaidas:brGeneral
    {
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarSupervisionCaidas(string Dstb, string Abnd, string Alr, string Estado, string Time, string Dpto, int TipoAbnd,string Localid, int TipoParticionid)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeSupervisionCaidas = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daSupervisionCaidas odaSupervisionCaidas = new daSupervisionCaidas();
                    lbeSupervisionCaidas = odaSupervisionCaidas.ListarSupervisionCaidas(con, Dstb, Abnd, Alr, Estado, Time, Dpto, TipoAbnd,Localid,TipoParticionid);
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
            return (lbeSupervisionCaidas);
        }

        public List<beAlarmasResolution> ListarCondicion()
        {
            List<beAlarmasResolution> lbeCondicion = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daSupervisionCaidas odaSupervisionCaidas = new daSupervisionCaidas();
                    lbeCondicion = odaSupervisionCaidas.ListarAlarmas(con);
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
            return (lbeCondicion);
        }
    }
}
