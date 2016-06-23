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
    public class daParticionEstado
    {        
        public List<beParticionEstado> ListarParticionEstado(SqlConnection con, string LocalId, string csdi, string Tipo_Abonado_id, string TipoParticionid, string TipoEvento, string Dpto)
        {
            List<beParticionEstado> lbeParticionEstado = null;

            SqlCommand cmd = new SqlCommand("sp_listar_ParticionesEstado", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@LocalId", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value =LocalId;

            SqlParameter param2 = cmd.Parameters.Add("@csid", SqlDbType.VarChar,10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = csdi;

            SqlParameter param3 = cmd.Parameters.Add("@Tipo_Abonado_id", SqlDbType.Int);
            param3.Direction = ParameterDirection.Input;
            param3.Value = Convert.ToInt32(Tipo_Abonado_id);

            SqlParameter param4 = cmd.Parameters.Add("@TipoParticionid", SqlDbType.Int);
            param4.Direction = ParameterDirection.Input;
            param4.Value = Convert.ToInt32(TipoParticionid);

            SqlParameter param5 = cmd.Parameters.Add("@TipoEvento", SqlDbType.VarChar,1);
            param5.Direction = ParameterDirection.Input;
            param5.Value = TipoEvento;

            SqlParameter param6 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param6.Direction = ParameterDirection.Input;
            param6.Value = Dpto;
            

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int idparticionEstado = drd.GetOrdinal("idparticionEstado");
                int Localid = drd.GetOrdinal("Localid");
                int Tipo_Abonado_des = drd.GetOrdinal("Tipo_Abonado_des");
                int SubscriberName = drd.GetOrdinal("SubscriberName");                
                int abonado = drd.GetOrdinal("abonado");
                int area = drd.GetOrdinal("area");
                int TipoParticiondes = drd.GetOrdinal("TipoParticiondes");
                int FechaHora = drd.GetOrdinal("FechaHora");
                int TipoEventoc = drd.GetOrdinal("TipoEvento");
                int HoraCaidaRed = drd.GetOrdinal("HoraCaidaRed");
                int Estado_CRed = drd.GetOrdinal("Estado_CRed");
                int HoraCaidaEnerg = drd.GetOrdinal("HoraCaidaEnerg");
                int Estado_CEnerg = drd.GetOrdinal("Estado_CEnerg");
                int HoraTest = drd.GetOrdinal("HoraTest");
                int Estado_Test = drd.GetOrdinal("Estado_Test");                
                lbeParticionEstado = new List<beParticionEstado>();
                beParticionEstado obeParticionEstado;
                while (drd.Read())
                {
                    obeParticionEstado = new beParticionEstado();
                    obeParticionEstado.idparticionEstado = drd.GetInt32(idparticionEstado);
                    obeParticionEstado.Localid = drd.GetString(Localid);
                    obeParticionEstado.Tipo_Abonado_des = drd.GetString(Tipo_Abonado_des);
                    obeParticionEstado.SubscriberName = drd.GetString(SubscriberName);
                    obeParticionEstado.abonado = drd.GetString(abonado);
                    obeParticionEstado.area = drd.GetString(area);
                    obeParticionEstado.TipoParticiondes = drd.GetString(TipoParticiondes);                    
                    obeParticionEstado.FechaHora = drd.GetString(FechaHora);
                    obeParticionEstado.TipoEvento = drd.GetString(TipoEventoc);
                    obeParticionEstado.HoraCaidaRed = drd.GetString(HoraCaidaRed);
                    obeParticionEstado.Estado_CRed = drd.GetString(Estado_CRed);
                    obeParticionEstado.HoraCaidaEnerg = drd.GetString(HoraCaidaEnerg);
                    obeParticionEstado.Estado_CEnerg = drd.GetString(Estado_CEnerg);
                    obeParticionEstado.HoraTest = drd.GetString(HoraTest);
                    obeParticionEstado.Estado_Test = drd.GetString(Estado_Test);
                    lbeParticionEstado.Add(obeParticionEstado);
                }
                drd.Close();
            }
            return lbeParticionEstado;
        }
    }
}
