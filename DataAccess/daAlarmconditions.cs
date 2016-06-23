using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace DataAccess
{
    public class daAlarmconditions
    {
        public List<beAlarmConditions> ListarAlarmAdm(SqlConnection con)
        {
            List<beAlarmConditions> lbeAlarmConditions = null;

            SqlCommand cmd = new SqlCommand("sp_web_listar_Alarmconditions_adm_mvc", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int AlarmCode = drd.GetOrdinal("AlarmCode");
                int Description = drd.GetOrdinal("Description");
                lbeAlarmConditions = new List<beAlarmConditions>();

                beAlarmConditions obeAlarmConditions;

                while (drd.Read())
                {
                    obeAlarmConditions = new beAlarmConditions();
                    obeAlarmConditions.AlarmCode = drd.GetString(AlarmCode);
                    obeAlarmConditions.Description = drd.GetString(Description);
                    lbeAlarmConditions.Add(obeAlarmConditions);
                }
                drd.Close();
            }
            return lbeAlarmConditions;
        }
    }
}
