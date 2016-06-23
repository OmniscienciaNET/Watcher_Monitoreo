using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
    public class brMonitoreo:brGeneral
    {
        public beMonitoreo MensajeAlarma(string user, short perfil)
        {
            beMonitoreo obeMonitoreo = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    obeMonitoreo = odaMonitoreo.MensajeAlarma(con, user, perfil);
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
            return (obeMonitoreo);
        }

        public List<beSenal> ListarAlarmasPendientes(string user, short perfil)
        {
            List<beSenal> lbeSenal = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    lbeSenal = odaMonitoreo.ListarAlarmasPendientes(con, user, perfil);
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
            return (lbeSenal);
        }

        public List<beAlarmasResolution> ListarResolution()
        {
            List<beAlarmasResolution> lbeResolution = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    lbeResolution = odaMonitoreo.ListarResolucion(con);
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
            return (lbeResolution);
        }

        public bool SenalPendienteEnEspera(int AlarmHistoryID, int time)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    exito = odaMonitoreo.SenalPendienteEnEspera(con, AlarmHistoryID, time);
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
            return exito;
        }

        public bool SenalPendienteCierre(int AlarmHistoryID, string ResolutionCode)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    exito = odaMonitoreo.SenalPendienteCierre(con, AlarmHistoryID, ResolutionCode);
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
            return exito;
        }

        public bool SenalAEscalar(int AlarmHistoryID)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    exito = odaMonitoreo.SenalAEscalar(con, AlarmHistoryID);
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
            return exito;
        }

        public beMonitoreo SenalExtraData(int ahid)
        {
            beMonitoreo obeMonitoreo = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    obeMonitoreo = odaMonitoreo.SenalExtraData(con, ahid);
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
            return (obeMonitoreo);
        }

        public List<beEvento> SenalHistoricoEvento(int ahid)
        {
            List<beEvento> lbeEvento = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    lbeEvento = odaMonitoreo.SenalHistoricoEvento(con, ahid);
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
            return (lbeEvento);
        }

        public int AsignarAlarma(int ahid, string user, int perfil)
        {
            int updated = -1;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    updated = odaMonitoreo.AsignarAlarma(con, ahid, user, perfil);
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
            return (updated);
        }

        public bool AgregarComentario(int palarmhistoryid, DateTime phorarequerimiento, string pObservacion, string usuario)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.AgregarComentario(con, palarmhistoryid, phorarequerimiento, pObservacion, usuario);
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
            return (Resultado);
        }

        public List<beEntidad> ComboEntidad(int palarmhistoryid, int pidcategoria, int pidentidad)
        {
            List<beEntidad> lbeEntidad = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEntidad = new daMonitoreo();
                    lbeEntidad = odaEntidad.ComboEntidad(con, palarmhistoryid, pidcategoria, pidentidad);
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
            return (lbeEntidad);
        }

        public List<beMotivo> ComboMotivo()
        {
            List<beMotivo> lbeMotivo = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEntidad = new daMonitoreo();
                    lbeMotivo = odaEntidad.ComboMotivo(con);
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
            return (lbeMotivo);
        }

        public List<beUserLogIn> ListarUsuariosEnLinea(string Usuario)
        {
            List<beUserLogIn> lbeUserLogIn = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEntidad = new daMonitoreo();
                    lbeUserLogIn = odaEntidad.ListarUsuariosEnLinea(con, Usuario);
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
            return (lbeUserLogIn);
        }

        public bool RelevarAlarmas(string UsuarioActual, string UserAsignado)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    exito = odaMonitoreo.RelevarAlarmas(con, UsuarioActual, UserAsignado);
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
            return exito;
        }

        public List<beEmergencia> ConsultarEmergencia(int palarmhistoryid, int pidcategoria)
        {
            List<beEmergencia> lbeConsultarEmergencia = null;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    lbeConsultarEmergencia = odaEmergencia.ConsultarEmergencia(con, palarmhistoryid, pidcategoria);
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
            return (lbeConsultarEmergencia);
        }

        public bool InsertarEmergencia(int palarmhistoryid, int pidentidad, DateTime phorarequerimiento, string pnroidenpersontrans, string pnomresponsable, string usuario)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.InsertarEmergencia(con, palarmhistoryid, pidentidad, phorarequerimiento, pnroidenpersontrans, pnomresponsable, usuario);
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
            return (Resultado);
        }

        public bool InsertarEmergenciaStopFraude(int palarmhistoryid, int pidMotivo, DateTime phorarequerimiento, string pObservacion, string pnomresponsable, string usuario)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.InsertarEmergenciaStopFraude(con, palarmhistoryid, pidMotivo, phorarequerimiento, pObservacion, pnomresponsable, usuario);
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
            return (Resultado);
        }

        public bool ActualizarEmergencia(int palarmhistoryid, int pidentidadOld, int pidentidadNew, DateTime phoraarribo, string pnroidenpersontrans, string pnomresponsable, string usuario)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.ActualizarEmergencia(con, palarmhistoryid, pidentidadOld, pidentidadNew, phoraarribo, pnroidenpersontrans, pnomresponsable, usuario);
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
            return (Resultado);
        }

        public bool ActualizarEmergenciaStopFraude(int palarmhistoryid, int pidMotivoOld, int pidMotivoNew, string pObservacion, string pnomresponsable, string usuario)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.ActualizarEmergenciaStopFraude(con, palarmhistoryid, pidMotivoOld, pidMotivoNew, pObservacion, pnomresponsable, usuario);
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
            return (Resultado);
        }

        public bool EliminarEmergencia(int palarmhistoryid, int pidentidad)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.EliminarEmergencia(con, palarmhistoryid, pidentidad);
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
            return (Resultado);
        }

        public bool EliminarEmergenciaStopFraude(int palarmhistoryid, int pidMotivo)
        {
            bool Resultado = false;

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaEmergencia = new daMonitoreo();
                    Resultado = odaEmergencia.EliminarEmergenciaStopFraude(con, palarmhistoryid, pidMotivo);
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
            return (Resultado);
        }

        public int SenalPendienteNoMonitoreo(string user, short perfil)
        {
            int updated = -1;
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    con.Open();
                    daMonitoreo odaMonitoreo = new daMonitoreo();
                    updated = odaMonitoreo.SenalPendienteNoMonitoreo(con, user, perfil);
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
            return (updated);
        }
    }
}
