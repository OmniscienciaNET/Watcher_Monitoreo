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
    public class daFalloTest
    {
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarFalloTest(SqlConnection con, string DstbCode, string AbndCode, string Time, string Dpto, int TipoAbnd)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeFalloTest = null;

            SqlCommand cmd = new SqlCommand("sp_web_FalloTest", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode;

            SqlParameter param3 = cmd.Parameters.Add("@pExcsNumb", SqlDbType.VarChar, 10);
            param3.Direction = ParameterDirection.Input;
            param3.Value = Time;

            SqlParameter param4 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param4.Direction = ParameterDirection.Input;
            param4.Value = Dpto;

            SqlParameter param5 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param5.Direction = ParameterDirection.Input;
            param5.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha_Ocurrencia");
                int dAlmHour = drd.GetOrdinal("DateTimeOccurred");
                int csid = drd.GetOrdinal("AbonadoCode");
                int Oficina = drd.GetOrdinal("Oficina");
                int IDSenal = drd.GetOrdinal("IDSenal");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int ZonaDescri = drd.GetOrdinal("ZonaDescri");
                int Condicion = drd.GetOrdinal("Condicion");
                lbeFalloTest = new List<beControlSenalSupCaidasFalloTestControlCierre>();
                beControlSenalSupCaidasFalloTestControlCierre obeFalloTest;
                while (drd.Read())
                {
                    obeFalloTest = new beControlSenalSupCaidasFalloTestControlCierre();
                    obeFalloTest.Fecha = drd.GetString(dAlmDate);
                    obeFalloTest.Hora = drd.GetString(dAlmHour);
                    obeFalloTest.AbndCode = drd.GetString(csid);
                    obeFalloTest.Oficina = drd.GetString(Oficina);
                    obeFalloTest.IDSenal = drd.GetString(IDSenal);
                    obeFalloTest.PhysicalZone = drd.GetString(PhysicalZone);
                    obeFalloTest.ZonaDescri = drd.GetString(ZonaDescri);
                    obeFalloTest.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeFalloTest.Zonaphys_Zonadescri = obeFalloTest.PhysicalZone + " - " + obeFalloTest.ZonaDescri;
                    obeFalloTest.Condicion = drd.GetString(Condicion);
                    lbeFalloTest.Add(obeFalloTest);
                }
                drd.Close();
            }
            return lbeFalloTest;
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
