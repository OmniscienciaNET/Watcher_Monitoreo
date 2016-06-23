using DataAccess;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessRules
{
   public class brParticionEstado:brGeneral
    {
       //public List<beParticionEstado> ListarParticionEstado(string idlocal, string Abonado, string TipoAbonado, string TipoParticion, string TipoEventop, string ExcesoTiempo, string Ubicacion)
       public List<beParticionEstado> ListarParticionEstado(string LocalId, string csid, string Tipo_Abonado_id, string TipoParticionid, string TipoEvento, string Dpto)
       {
           List<beParticionEstado> lbeParticionEstado = null;
           using (SqlConnection con = new SqlConnection(Conexion))
           {
               try
               {
                   con.Open();
                   daParticionEstado odaParticionEstado = new daParticionEstado();
                   //lbeParticionEstado = odaParticionEstado.ListarParticionEstado(con, idlocal, Abonado, TipoAbonado, TipoParticion, TipoEventop, ExcesoTiempo, Ubicacion);
                   lbeParticionEstado = odaParticionEstado.ListarParticionEstado(con, LocalId, csid, Tipo_Abonado_id, TipoParticionid, TipoEvento, Dpto);
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
           return (lbeParticionEstado);
       }

    }
}
