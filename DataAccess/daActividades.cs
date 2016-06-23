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
    public class daActividades
    {
        public List<beAlarmasResolution> ListarAlarmas(SqlConnection con)
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            string sql =    "select al.AlarmCode, al.Description from AlarmConditions al"+
                            " where al.AlarmCode in (select alXML.AlarmCode from AlarmConditions_XML alXML"+
                            " where alXML.bClienteXML = 1) Union select '$A$', 'TODAS LAS ALARMAS' order by 2";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

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

        public List<beActividad> ListarActividad(SqlConnection con, string DstbCode, string AbndCode, string CodeArea, string AlarmCode, string AlarmDesc, string BeginDate, string EndDate, string BeginHour, string EndHour, int CondEstab, int PeriodoTest, string Dpto, int TipoAbnd)
        {
            List<beActividad> lbeActvidad = null;

            SqlCommand cmd = new SqlCommand("ma_Actv", con);
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

            SqlParameter param4 = cmd.Parameters.Add("@pAlrmDesc", SqlDbType.VarChar, 10);
            param4.Direction = ParameterDirection.Input;
            param4.Value = AlarmDesc;

            SqlParameter param5 = cmd.Parameters.Add("@pDsdDate", SqlDbType.VarChar, 10);
            param5.Direction = ParameterDirection.Input;
            param5.Value = BeginDate != "" ? DateTime.ParseExact(BeginDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") : "";

            SqlParameter param6 = cmd.Parameters.Add("@pDsdHour", SqlDbType.VarChar, 10);
            param6.Direction = ParameterDirection.Input;
            param6.Value = BeginHour;

            SqlParameter param7 = cmd.Parameters.Add("@pHstDate", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = EndDate != "" ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") : "";

            SqlParameter param8 = cmd.Parameters.Add("@pHstHour", SqlDbType.VarChar, 10);
            param8.Direction = ParameterDirection.Input;
            param8.Value = EndHour;

            SqlParameter param9 = cmd.Parameters.Add("@pArea", SqlDbType.VarChar, 4);
            param9.Direction = ParameterDirection.Input;
            param9.Value = CodeArea.ToUpper();

            SqlParameter param10 = cmd.Parameters.Add("@pAlarmNoC", SqlDbType.Char, 1);
            param10.Direction = ParameterDirection.Input;
            param10.Value = CondEstab.ToString();

            SqlParameter param11 = cmd.Parameters.Add("@pSinPeriodoTest", SqlDbType.Int);
            param11.Direction = ParameterDirection.Input;
            param11.Value = PeriodoTest;

            SqlParameter param12 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param12.Direction = ParameterDirection.Input;
            param12.Value = Dpto;

            SqlParameter param13 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param13.Direction = ParameterDirection.Input;
            param13.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("dAlrmDate");
                int dAlmHour = drd.GetOrdinal("dAlrmHour");
                int csid = drd.GetOrdinal("CSID");
                int Oficina = drd.GetOrdinal("Oficina");
                int AlarmCod = drd.GetOrdinal("AlarmCode");
                int Condicion = drd.GetOrdinal("Condicion_Descrip");
                int User_Disp_Ubi = drd.GetOrdinal("User_Disp_Ubic");
                int Area = drd.GetOrdinal("Area");
                int AreaDesc = drd.GetOrdinal("Descripcion_Area");
                int Detalle = drd.GetOrdinal("Detalle");
                int IDSenal = drd.GetOrdinal("SignalIdentifier");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int ahid = drd.GetOrdinal("AlarmHistoryID");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                
                lbeActvidad = new List<beActividad>();
                beActividad obeActividad;
                while (drd.Read())
                {
                    obeActividad = new beActividad();
                    obeActividad.Fecha = drd.GetString(dAlmDate);
                    obeActividad.Hora = drd.GetString(dAlmHour);
                    obeActividad.AbndCode = drd.GetString(csid);
                    obeActividad.Oficina = drd.GetString(Oficina);
                    obeActividad.IDSenal = drd.GetString(IDSenal);
                    obeActividad.Evento = drd.GetString(PhysicalZone);
                    obeActividad.CodeCondicion = drd.GetString(AlarmCod);
                    obeActividad.Condicion = drd.GetString(Condicion);
                    obeActividad.AlarmHistoryID = int.Parse(drd.GetString(ahid));
                    obeActividad.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeActividad.Zonaphys_Zonadescri = drd.GetString(User_Disp_Ubi);
                    obeActividad.Area = drd.GetString(Area);
                    obeActividad.AreaDesc = drd.GetString(AreaDesc);
                    obeActividad.Detalle = drd.GetString(Detalle);
                    lbeActvidad.Add(obeActividad);
                }
                drd.Close();
            }
            return lbeActvidad;
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
