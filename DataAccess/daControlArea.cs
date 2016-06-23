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
    public class daControlArea
    {
        public List<beControlArea> ListarControlArea(SqlConnection con, string DstbCode, string AbndCode, string AlrCode1, string AlrCode2,string ZoneDesc, int Time, int BeforeDays, string Dpto, int TipoAbnd)
        {
            List<beControlArea> lbeControlArea = null;

            SqlCommand cmd = new SqlCommand("sp_web_ma_Area_mvc", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode.ToUpper();

            SqlParameter param3 = cmd.Parameters.Add("@pAlrm1Code", SqlDbType.VarChar, 3);
            param3.Direction = ParameterDirection.Input;
            param3.Value = AlrCode1.ToUpper();

            SqlParameter param4 = cmd.Parameters.Add("@pAlrm2Code", SqlDbType.VarChar, 3);
            param4.Direction = ParameterDirection.Input;
            param4.Value = AlrCode2.ToUpper();

            SqlParameter param5 = cmd.Parameters.Add("@pExcsNumb", SqlDbType.Int);
            param5.Direction = ParameterDirection.Input;
            param5.Value = Time;

            SqlParameter param6 = cmd.Parameters.Add("@pZonaDesc", SqlDbType.VarChar, 100);
            param6.Direction = ParameterDirection.Input;
            param6.Value = ZoneDesc;

            SqlParameter param7 = cmd.Parameters.Add("@pDiasPsds", SqlDbType.Int);
            param7.Direction = ParameterDirection.Input;
            param7.Value = BeforeDays;

            SqlParameter param8 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param8.Direction = ParameterDirection.Input;
            param8.Value = Dpto;

            SqlParameter param9 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param9.Direction = ParameterDirection.Input;
            param9.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha_Ocurrencia");
                int dAlmHour = drd.GetOrdinal("DateTimeOccurred");
                int csid = drd.GetOrdinal("CSID");
                int Oficina = drd.GetOrdinal("Oficina");
                int Condicion = drd.GetOrdinal("Condicion");
                int AlarmHistoryID = drd.GetOrdinal("AlarmHistoryID");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int IDSenal = drd.GetOrdinal("SignalIdentifier");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int UserNumber = drd.GetOrdinal("UserNumber");
                int UserName = drd.GetOrdinal("UserName");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int Detalle = drd.GetOrdinal("Detalle");
                lbeControlArea = new List<beControlArea>();
                beControlArea obeControlArea;
                while (drd.Read())
                {
                    obeControlArea = new beControlArea();
                    obeControlArea.Fecha = drd.GetString(dAlmDate);
                    obeControlArea.Hora = drd.GetString(dAlmHour);
                    obeControlArea.AbndCode = drd.GetString(csid);
                    obeControlArea.Oficina = drd.GetString(Oficina);
                    obeControlArea.Condicion = drd.GetString(Condicion);
                    obeControlArea.IDSenal = drd.GetString(IDSenal);
                    obeControlArea.PhysicalZone = drd.GetString(PhysicalZone);
                    obeControlArea.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeControlArea.UserNumber = drd.GetString(UserNumber);
                    obeControlArea.UserName = drd.GetString(UserName);
                    obeControlArea.ZonaDescri = drd.GetString(Detalle);
                    obeControlArea.Zonaphys_Zonadescri = obeControlArea.UserNumber!=""?obeControlArea.UserNumber+" - ":obeControlArea.PhysicalZone+" - "+obeControlArea.ZonaDescri;
                    lbeControlArea.Add(obeControlArea);
                }
                drd.Close();
            }
            return lbeControlArea;
        }

        public List<beControlArea> ListarControlArea1864(SqlConnection con, string DstbCode, string AbndCode, string AlrCode1, string AlrCode2, string ZoneDesc, int Time, int BeforeDays, string Dpto, int TipoAbnd)
        {
            List<beControlArea> lbeControlArea = null;

            SqlCommand cmd = new SqlCommand("sp_web_ma_Area1864_mvc", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode.ToUpper();

            SqlParameter param3 = cmd.Parameters.Add("@pAlrm1Code", SqlDbType.VarChar, 3);
            param3.Direction = ParameterDirection.Input;
            param3.Value = AlrCode1.ToUpper();

            SqlParameter param4 = cmd.Parameters.Add("@pAlrm2Code", SqlDbType.VarChar, 3);
            param4.Direction = ParameterDirection.Input;
            param4.Value = AlrCode2.ToUpper();

            SqlParameter param5 = cmd.Parameters.Add("@pExcsNumb", SqlDbType.Int);
            param5.Direction = ParameterDirection.Input;
            param5.Value = Time;

            SqlParameter param6 = cmd.Parameters.Add("@pZonaDesc", SqlDbType.VarChar, 100);
            param6.Direction = ParameterDirection.Input;
            param6.Value = ZoneDesc;

            SqlParameter param7 = cmd.Parameters.Add("@pDiasPsds", SqlDbType.Int);
            param7.Direction = ParameterDirection.Input;
            param7.Value = BeforeDays;

            SqlParameter param8 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param8.Direction = ParameterDirection.Input;
            param8.Value = Dpto;

            SqlParameter param9 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param9.Direction = ParameterDirection.Input;
            param9.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha_Ocurrencia");
                int dAlmHour = drd.GetOrdinal("DateTimeOccurred");
                int csid = drd.GetOrdinal("CSID");
                int Oficina = drd.GetOrdinal("Oficina");
                int Condicion = drd.GetOrdinal("Condicion");
                int AlarmHistoryID = drd.GetOrdinal("AlarmHistoryID");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int IDSenal = drd.GetOrdinal("SignalIdentifier");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int UserNumber = drd.GetOrdinal("UserNumber");
                int UserName = drd.GetOrdinal("UserName");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int Detalle = drd.GetOrdinal("Detalle");
                lbeControlArea = new List<beControlArea>();
                beControlArea obeControlArea;
                while (drd.Read())
                {
                    obeControlArea = new beControlArea();
                    obeControlArea.Fecha = drd.GetString(dAlmDate);
                    obeControlArea.Hora = drd.GetString(dAlmHour);
                    obeControlArea.AbndCode = drd.GetString(csid);
                    obeControlArea.Oficina = drd.GetString(Oficina);
                    obeControlArea.Condicion = drd.GetString(Condicion);
                    obeControlArea.IDSenal = drd.GetString(IDSenal);
                    obeControlArea.PhysicalZone = drd.GetString(PhysicalZone);
                    obeControlArea.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeControlArea.UserNumber = drd.GetString(UserNumber);
                    obeControlArea.UserName = drd.GetString(UserName);
                    obeControlArea.ZonaDescri = drd.GetString(Detalle);
                    obeControlArea.Zonaphys_Zonadescri = obeControlArea.UserNumber != "" ? obeControlArea.UserNumber + " - " : obeControlArea.PhysicalZone + " - " + obeControlArea.ZonaDescri;
                    lbeControlArea.Add(obeControlArea);
                }
                drd.Close();
            }
            return lbeControlArea;
        }

        public string NombreDeIcono(string sAdminOEmergencia, string sConComentario)
        {
            string s = "";
            switch (sAdminOEmergencia)
            {
                case "T":
                    s = "Rojo";
                    break;
                case "F":
                    s = "Negro";
                    break;
            }
            if (sConComentario == "1") s = s + "cAmarillo";
            string Carp = "../Images/";
            string NomArch = "Alr" + s + ".PNG";
            return Carp + NomArch;
        }

        public List<beAlarmasResolution> ListarCondicionAlarmas(SqlConnection con)
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            SqlCommand cmd = new SqlCommand("Sp_ListarCondicionAlarmas", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int AlarmCode = drd.GetOrdinal("AlarmCode");
                int Description = drd.GetOrdinal("Description");
                lbeAlarmas = new List<beAlarmasResolution>();
                beAlarmasResolution obeAlarmas;
                while (drd.Read())
                {
                    obeAlarmas = new beAlarmasResolution();
                    obeAlarmas.ResolutionCode = drd.GetString(AlarmCode);
                    obeAlarmas.ResolutionDesc = drd.GetString(Description);
                    lbeAlarmas.Add(obeAlarmas);
                }
                drd.Close();
            }
            return lbeAlarmas;
        }
    }
}
