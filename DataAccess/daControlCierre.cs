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
    public class daControlCierre
    {
        public List<beControlSenalSupCaidasFalloTestControlCierre> ListarControlCierre(SqlConnection con, string DstbCode, string AbndCode, string Dpto, int TipoAbnd)
        {
            List<beControlSenalSupCaidasFalloTestControlCierre> lbeControlCierre = null;

            SqlCommand cmd = new SqlCommand("sp_web_Control_de_Cierres", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@cDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@cAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode;

            SqlParameter param3 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param3.Direction = ParameterDirection.Input;
            param3.Value = Dpto;

            SqlParameter param4 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param4.Direction = ParameterDirection.Input;
            param4.Value = TipoAbnd;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha_Ocurrencia");
                int dAlmHour = drd.GetOrdinal("dteDateTimeOccurred");
                int csid = drd.GetOrdinal("AbonadoCode");
                int Oficina = drd.GetOrdinal("Oficina");
                int IDSenal = drd.GetOrdinal("IDSenal");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int ConComentario = drd.GetOrdinal("nInciNumb");
                int Condicion = drd.GetOrdinal("Condicion");
                int ZonaPhys_ZonaDescri = drd.GetOrdinal("ZonaPhys_ZonaDescri");
                lbeControlCierre = new List<beControlSenalSupCaidasFalloTestControlCierre>();
                beControlSenalSupCaidasFalloTestControlCierre obeControlCierre;
                while (drd.Read())
                {
                    obeControlCierre = new beControlSenalSupCaidasFalloTestControlCierre();
                    obeControlCierre.Fecha = drd.GetString(dAlmDate);
                    obeControlCierre.Hora = drd.GetString(dAlmHour);
                    obeControlCierre.AbndCode = drd.GetString(csid);
                    obeControlCierre.Oficina = drd.GetString(Oficina);
                    obeControlCierre.IDSenal = drd.GetString(IDSenal);
                    obeControlCierre.Zonaphys_Zonadescri = drd.GetString(ZonaPhys_ZonaDescri);
                    obeControlCierre.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    obeControlCierre.Condicion = drd.GetString(Condicion);
                    lbeControlCierre.Add(obeControlCierre);
                }
                drd.Close();
            }
            return lbeControlCierre;
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
