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
    public class daSupervisionCaidas
    {
        public List<beAlarmasResolution> ListarAlarmas(SqlConnection con)
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            string sql = "SELECT cAlrmCode0,cAlrmCode1,sAlrmDesc FROM ma_SpCLAlrm ORDER BY sAlrmDesc";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int AlarmCode = drd.GetOrdinal("cAlrmCode0");
                int AlarmCode1 = drd.GetOrdinal("cAlrmCode1");
                int Description = drd.GetOrdinal("sAlrmDesc");
                lbeAlarmas = new List<beAlarmasResolution>();
                beAlarmasResolution obeAlarmas;
                while (drd.Read())
                {
                    obeAlarmas = new beAlarmasResolution();
                    obeAlarmas.ResolutionCode = drd.GetString(AlarmCode) + "|"+drd.GetString(AlarmCode1);
                    obeAlarmas.ResolutionDesc = drd.GetString(Description);
                    lbeAlarmas.Add(obeAlarmas);
                }
                drd.Close();
            }
            return lbeAlarmas;
        }
        
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarSupervisionCaidas(SqlConnection con, string DstbCode, string AbndCode, string AlrCode,string Estado, string Time, string Dpto, int TipoAbnd, string Localid, int TipoParticionid)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeSupervisionCaidas = null;

            SqlCommand cmd = new SqlCommand("ma_SpCR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode;

            SqlParameter param3 = cmd.Parameters.Add("@pAlr1Code", SqlDbType.VarChar, 3);
            param3.Direction = ParameterDirection.Input;
            param3.Value = AlrCode.Split('|')[0];

            SqlParameter param4 = cmd.Parameters.Add("@pAlr2Code", SqlDbType.VarChar, 3);
            param4.Direction = ParameterDirection.Input;
            param4.Value = AlrCode.Split('|')[1];

            SqlParameter param5 = cmd.Parameters.Add("@pCondCode", SqlDbType.VarChar, 1);
            param5.Direction = ParameterDirection.Input;
            param5.Value = Estado;

            SqlParameter param6 = cmd.Parameters.Add("@pExcsNumb", SqlDbType.VarChar, 10);
            param6.Direction = ParameterDirection.Input;
            param6.Value = Time;

            SqlParameter param7 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param7.Direction = ParameterDirection.Input;
            param7.Value = Dpto;

            SqlParameter param8 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param8.Direction = ParameterDirection.Input;
            param8.Value = TipoAbnd;

            SqlParameter param9 = cmd.Parameters.Add("@Localid", SqlDbType.VarChar, 15);
            param9.Direction = ParameterDirection.Input;
            param9.Value = Localid;

            SqlParameter param10 = cmd.Parameters.Add("@TipoParticionid", SqlDbType.Int);
            param10.Direction = ParameterDirection.Input;
            param10.Value = TipoParticionid;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha_Ocurrencia");
                int dAlmHour = drd.GetOrdinal("DateTimeOccurred");
                int csid = drd.GetOrdinal("CSID");
                int Oficina = drd.GetOrdinal("Oficina");
                int IDSenal = drd.GetOrdinal("SignalIdentifier");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int ZonaDescri = drd.GetOrdinal("ZonaDescri");
                int Condicion = drd.GetOrdinal("Condicion");
                lbeSupervisionCaidas = new List<beControlSenalSupCaidasFalloTestControlCierre>();
                beControlSenalSupCaidasFalloTestControlCierre obeSupervisionCaidas;
                while (drd.Read())
                {
                    obeSupervisionCaidas = new beControlSenalSupCaidasFalloTestControlCierre();
                    obeSupervisionCaidas.Fecha = drd.GetString(dAlmDate);
                    obeSupervisionCaidas.Hora = drd.GetString(dAlmHour);
                    obeSupervisionCaidas.AbndCode = drd.GetString(csid);
                    obeSupervisionCaidas.Oficina = drd.GetString(Oficina);
                    obeSupervisionCaidas.IDSenal = drd.GetString(IDSenal);
                    obeSupervisionCaidas.PhysicalZone = drd.GetString(PhysicalZone);
                    obeSupervisionCaidas.ZonaDescri = drd.GetString(ZonaDescri);
                    obeSupervisionCaidas.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeSupervisionCaidas.Zonaphys_Zonadescri = obeSupervisionCaidas.PhysicalZone + " - " + obeSupervisionCaidas.ZonaDescri;
                    obeSupervisionCaidas.Condicion = drd.GetString(Condicion);
                    lbeSupervisionCaidas.Add(obeSupervisionCaidas);
                }
                drd.Close();
            }
            return lbeSupervisionCaidas;
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
    }
}
