using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
   public class daEventoAdministrativo
    {
       public bool RegistrarAlarma(SqlConnection con, string alarmcode, string csid, string PhysicalZone,string UserAssigned, out string outIdAlarmhistory, out string outDateTimeOccurred)
       {
           bool registro = false;
           SqlCommand cmd = new SqlCommand("sp_web_registrar_alarma_adm", con);
           cmd.CommandType = CommandType.StoredProcedure;

           SqlParameter param1 = cmd.Parameters.Add("@AlarmCode", SqlDbType.VarChar,3);
           param1.Direction = ParameterDirection.Input;
           param1.Value = alarmcode;

           SqlParameter param2 = cmd.Parameters.Add("@CSID", SqlDbType.VarChar, 10);
           param2.Direction = ParameterDirection.Input;
           param2.Value = csid;

           SqlParameter param3 = cmd.Parameters.Add("@PhysicalZone", SqlDbType.VarChar, 5);
           param3.Direction = ParameterDirection.Input;
           param3.Value = PhysicalZone;

           SqlParameter param4 = cmd.Parameters.Add("@UserAssigned", SqlDbType.VarChar, 10);
           param4.Direction = ParameterDirection.Input;
           param4.Value = UserAssigned;

           //PARAMETROS DE SALIDA

           SqlParameter param5 = cmd.Parameters.Add("@out_IdAlarmhistory", SqlDbType.VarChar,25);
           param5.Direction = ParameterDirection.Output;

           SqlParameter param6 = cmd.Parameters.Add("@out_DateTimeOccurred", SqlDbType.VarChar,25);
           param6.Direction = ParameterDirection.Output;
           
           //registro = cmd.ExecuteNonQuery();

           int n = cmd.ExecuteNonQuery();

           if (n > 0)
           {
               registro = true;               
               outIdAlarmhistory = param5.Value.ToString();
               outDateTimeOccurred = param6.Value.ToString();
           }
           else
           {               
               outIdAlarmhistory = "";
               outDateTimeOccurred = "";
           }
               
           return registro;

       }
    }
}
