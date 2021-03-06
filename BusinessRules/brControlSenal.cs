﻿using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brControlSenal:brGeneral
    {
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarControlSenal(string Dstb, string Abnd, string Time, string Dpto, int TipoAbnd)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeControlSenal = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daControlSenal odaSupervisionTecnica = new daControlSenal();
                    lbeControlSenal = odaSupervisionTecnica.ListarControlSenal(con, Dstb, Abnd, Time, Dpto, TipoAbnd);
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
            return (lbeControlSenal);
        }
    }
}
