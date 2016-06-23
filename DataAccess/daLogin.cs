using Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class daLogin
    {
        public beLogin ValidarLogin(SqlConnection con, string usuario, string password, string dstb)
        {
            beLogin obeLogin = new beLogin();
            SqlCommand cmd = new SqlCommand("spw_validarlogin", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@sUserIden", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = usuario;

            SqlParameter param2 = cmd.Parameters.Add("@sUserPswd", SqlDbType.VarChar,10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = password;

            SqlParameter param3 = cmd.Parameters.Add("@DealerCode", SqlDbType.VarChar,3);
            param3.Direction = ParameterDirection.Input;
            param3.Value = dstb;

            SqlDataReader drd = cmd.ExecuteReader();

            List<beDstbAbnd> lbeDstbAbnd = null;
            List<beLocal_subscriber> lbeLocal_subscriber = null;
            if (drd != null)
            {
                drd.Read();
                obeLogin.exito = drd.GetInt32(0);
                if (drd.NextResult())
                {
                    int DstbCode = drd.GetOrdinal("DstbCode");
                    int DstbName = drd.GetOrdinal("DstbName");
                    int AbndCode = drd.GetOrdinal("AbndCode");
                    int Oficina = drd.GetOrdinal("Oficina");
                    int AbndDireccion = drd.GetOrdinal("AbndDireccion");
                    lbeDstbAbnd = new List<beDstbAbnd>();
                    beDstbAbnd obeDstbAbnd;
                    while (drd.Read())
                    {
                        obeDstbAbnd = new beDstbAbnd();
                        obeDstbAbnd.DstbCode = drd.GetString(DstbCode);
                        obeDstbAbnd.DstbName = drd.GetString(DstbName);
                        obeDstbAbnd.AbndCode = drd.GetString(AbndCode);
                        obeDstbAbnd.Oficina = drd.GetString(Oficina);
                        obeDstbAbnd.AbndDireccion = drd.GetString(AbndDireccion);
                        lbeDstbAbnd.Add(obeDstbAbnd);
                    }

                    if (drd.NextResult())
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
                    }
                }
                obeLogin.lstDstAbnd = lbeDstbAbnd;
                obeLogin.lstLocal_subscriber = lbeLocal_subscriber;
                drd.Close();
            }
            return obeLogin;
        }

        public bool LogOut(SqlConnection con, string usuario)
        {
            bool exito = false; 
            SqlCommand cmd = new SqlCommand("Spw_ValidarLogOut", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@sUserIden", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = usuario;

            int update = cmd.ExecuteNonQuery();

            if (update > 0) exito = true;
            return exito;
        }
    }
}
