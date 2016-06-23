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
    public class daSubscriber
    {
        public List<beSubscriber> ListarSubscriber(SqlConnection con, string csid, string subscribername)
        {
            List<beSubscriber> lbeSubscriber = null;

            SqlCommand cmd = new SqlCommand("sp_web_ListarSubscriber_mvc", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@csid", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = csid;

            SqlParameter param2 = cmd.Parameters.Add("@SubscriberName", SqlDbType.VarChar, 20);
            param2.Direction = ParameterDirection.Input;
            param2.Value = subscribername;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int CSID = drd.GetOrdinal("CSID");
                int SubscriberName = drd.GetOrdinal("SubscriberName");
                int AddressStreet = drd.GetOrdinal("AddressStreet");
                int dpto = drd.GetOrdinal("dpto");
                int prov = drd.GetOrdinal("prov");
                int dist = drd.GetOrdinal("dist");
                int TipoParticiondes = drd.GetOrdinal("TipoParticiondes");
                lbeSubscriber = new List<beSubscriber>();

                beSubscriber obeSubscriber;

                while (drd.Read())
                {
                    obeSubscriber = new beSubscriber();
                    obeSubscriber.CSID = drd.GetString(CSID);
                    obeSubscriber.SubscriberName = drd.GetString(SubscriberName);
                    obeSubscriber.AddressStreet = drd.GetString(AddressStreet);
                    obeSubscriber.dpto = drd.GetString(dpto);
                    obeSubscriber.prov = drd.GetString(prov);
                    obeSubscriber.dist = drd.GetString(dist);
                    obeSubscriber.TipoParticiondes = drd.GetString(TipoParticiondes);
                    lbeSubscriber.Add(obeSubscriber);
                }
                drd.Close();
            }
            return lbeSubscriber;
        }
    }
}
