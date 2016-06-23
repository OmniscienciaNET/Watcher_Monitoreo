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
    public class daLocal_subscriber
    {
        public List<beLocal_subscriber> ComboLocal_subscriber(SqlConnection con)
        {
            List<beLocal_subscriber> lbeLocal_subscriber = null;

            SqlCommand cmd = new SqlCommand("sp_web_combo_Local_subscriber", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int Localid = drd.GetOrdinal("Localid");
                int Localdes = drd.GetOrdinal("Localdes");
                
                lbeLocal_subscriber = new List<beLocal_subscriber>();
                beLocal_subscriber obeLocal_subscriber;
                while (drd.Read())
                {
                    obeLocal_subscriber = new beLocal_subscriber();
                    obeLocal_subscriber.Localid = drd.GetString(Localid);
                    obeLocal_subscriber.Localdes = drd.GetString(Localdes);
                    lbeLocal_subscriber.Add(obeLocal_subscriber);
                }
                drd.Close();
            }
            return lbeLocal_subscriber;
        }

        public List<beLocal_subscriber> Buscar_Local_Subscriber(SqlConnection con, string pLocalid, string pLocaldesc)
        {
            List<beLocal_subscriber> lbeLocal_subscriber = null;

            SqlCommand cmd = new SqlCommand("sp_web_Buscar_Local_Subscriber", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@Localid", SqlDbType.VarChar, 15);
            param1.Direction = ParameterDirection.Input;
            param1.Value = pLocalid;

            SqlParameter param2 = cmd.Parameters.Add("@Localdes", SqlDbType.VarChar, 100);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pLocaldesc;


            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int Localid = drd.GetOrdinal("Localid");
                int Localdes = drd.GetOrdinal("Localdes");

                lbeLocal_subscriber = new List<beLocal_subscriber>();
                beLocal_subscriber obeLocal_subscriber;
                while (drd.Read())
                {
                    obeLocal_subscriber = new beLocal_subscriber();
                    obeLocal_subscriber.Localid = drd.GetString(Localid);
                    obeLocal_subscriber.Localdes = drd.GetString(Localdes);
                    lbeLocal_subscriber.Add(obeLocal_subscriber);
                }
                drd.Close();
            }
            return lbeLocal_subscriber;
        }
    }
}
