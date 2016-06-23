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
    public class daEstadisticoAlarmas
    {
        public List<beEstadisticoAlarmas> ListarEstadisticoAlarmas(SqlConnection con, string DstbCode, string AbndCode, string AlarmCode, string BeginDate, string BeginTime, string EndDate, string EndTime, string Dpto, int TipoAbnd)
        {
            List<beEstadisticoAlarmas> lbeEstadisticoAlarmas = null;

            SqlCommand cmd = new SqlCommand("sp_web_ma_RptAlrm", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode.ToUpper();

            SqlParameter param3 = cmd.Parameters.Add("@pAlrmCode", SqlDbType.VarChar, 10);
            param3.Direction = ParameterDirection.Input;
            param3.Value = AlarmCode;

            SqlParameter param4 = cmd.Parameters.Add("@pDsdDate", SqlDbType.VarChar, 10);
            param4.Direction = ParameterDirection.Input;
            param4.Value = BeginDate != "" ? DateTime.ParseExact(BeginDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") : "";

            SqlParameter param5 = cmd.Parameters.Add("@pDsdHour", SqlDbType.VarChar, 10);
            param5.Direction = ParameterDirection.Input;
            param5.Value = BeginTime;

            SqlParameter param6 = cmd.Parameters.Add("@pHstDate", SqlDbType.VarChar, 10);
            param6.Direction = ParameterDirection.Input;
            param6.Value = EndDate != "" ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") : "";

            SqlParameter param7 = cmd.Parameters.Add("@pHstHour", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = EndTime;

            SqlParameter param8 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param8.Direction = ParameterDirection.Input;
            param8.Value = Dpto;

            SqlParameter param9 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param9.Direction = ParameterDirection.Input;
            param9.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int DealerCode = drd.GetOrdinal("DealerCode");
                int AbonadoCode = drd.GetOrdinal("AbonadoCode");
                int Oficina = drd.GetOrdinal("Oficina");
                int Actividad = drd.GetOrdinal("Actividad");
                int AlarmDescrip = drd.GetOrdinal("AlarmDescrip");
                int nCantidad = drd.GetOrdinal("nCantidad");
                lbeEstadisticoAlarmas = new List<beEstadisticoAlarmas>();
                beEstadisticoAlarmas obeEstadisticoAlarmas;
                while (drd.Read())
                {
                    obeEstadisticoAlarmas = new beEstadisticoAlarmas();
                    obeEstadisticoAlarmas.DealerCode = drd.GetString(DealerCode);
                    obeEstadisticoAlarmas.AbndCode = drd.GetString(AbonadoCode);
                    obeEstadisticoAlarmas.Oficina = drd.GetString(Oficina);
                    obeEstadisticoAlarmas.Actividad = drd.GetString(Actividad);
                    obeEstadisticoAlarmas.AlarmDescrip = drd.GetString(AlarmDescrip);
                    obeEstadisticoAlarmas.Cantidad = drd.GetInt32(nCantidad);
                    lbeEstadisticoAlarmas.Add(obeEstadisticoAlarmas);
                }
                drd.Close();
            }
            return lbeEstadisticoAlarmas;
        }
    }
}
