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
    public class daVisor
    {
        public List<beVisor> ListarVisor(SqlConnection con, string DstbCode, string AbndCode, string chkEvitar, string Dpto, int TipoAbnd, string Localid, int TipoParticionid)
        {
            List<beVisor> lbeVisor = null;

            SqlCommand cmd = new SqlCommand("sp_web_ma_MntrAlrm", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = DstbCode.ToUpper();

            SqlParameter param2 = cmd.Parameters.Add("@pAbndCode", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = AbndCode;

            SqlParameter param3 = cmd.Parameters.Add("@pAlarmNoC", SqlDbType.VarChar, 10);
            param3.Direction = ParameterDirection.Input;
            param3.Value = chkEvitar;

            SqlParameter param4 = cmd.Parameters.Add("@Ubigeo", SqlDbType.VarChar, 3);
            param4.Direction = ParameterDirection.Input;
            param4.Value = Dpto;

            SqlParameter param5 = cmd.Parameters.Add("@TipoAbnd", SqlDbType.Int);
            param5.Direction = ParameterDirection.Input;
            param5.Value = TipoAbnd;

            SqlParameter param6 = cmd.Parameters.Add("@Localid", SqlDbType.VarChar, 15);
            param6.Direction = ParameterDirection.Input;
            param6.Value = Localid;

            SqlParameter param7 = cmd.Parameters.Add("@TipoParticionid", SqlDbType.Int);
            param7.Direction = ParameterDirection.Input;
            param7.Value = TipoParticionid;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int dAlmDate = drd.GetOrdinal("Fecha");
                int dAlmHour = drd.GetOrdinal("Hora");
                int csid = drd.GetOrdinal("Abonado");
                int Oficina = drd.GetOrdinal("Oficina");
                int IDSenal = drd.GetOrdinal("IDSenal");
                int Evento = drd.GetOrdinal("Evento");
                int Descripcion = drd.GetOrdinal("Descripcion");
                int Descripcion_Evento = drd.GetOrdinal("Descripcion_Evento");
                int Area = drd.GetOrdinal("Area");
                int Descripcion_Area = drd.GetOrdinal("Descripcion_Area");
                int AdminOEmergencia = drd.GetOrdinal("AdminOEmergencia");
                int ConComentario = drd.GetOrdinal("ConComentario");
                int AlarmHistoryID = drd.GetOrdinal("AlarmHistoryID");
                lbeVisor = new List<beVisor>();
                beVisor obeVisor;
                while (drd.Read())
                {
                    obeVisor = new beVisor();
                    obeVisor.Fecha = drd.GetString(dAlmDate);
                    obeVisor.Hora = drd.GetString(dAlmHour);
                    obeVisor.AbndCode = drd.GetString(csid);
                    obeVisor.Oficina = drd.GetString(Oficina);
                    obeVisor.IDSenal = drd.GetString(IDSenal);
                    obeVisor.Evento = drd.GetString(Evento);
                    obeVisor.Condicion = drd.GetString(Descripcion);
                    obeVisor.Descripcion_Evento = drd.GetString(Descripcion_Evento);
                    obeVisor.Area = drd.GetString(Area);
                    obeVisor.Descripcion_Area = drd.GetString(Descripcion_Area);
                    obeVisor.sAlarmId = drd.GetInt32(AlarmHistoryID);
                    obeVisor.ImagenRuta = NombreDeIcono(drd.GetString(AdminOEmergencia), drd.GetString(ConComentario));
                    lbeVisor.Add(obeVisor);
                }
                drd.Close();
            }
            return lbeVisor;
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

        public beComentarioLlamada ListarComentariosLlamadas(SqlConnection con, int ahid)
        {
            beComentarioLlamada obeComentarioLlamada = null;
            SqlCommand cmd = new SqlCommand("Sp_get_comentarios_llamadas", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@ahid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = ahid;

            List<beComentario> lbeComentario = null;
            List<beLlamada> lbeLlamadas = null;
            List<beEvento> lbeEvento = null;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int Datetimeoccurred = drd.GetOrdinal("Datetimeoccurred");
                int OperatorNote = drd.GetOrdinal("OperatorNote");
                int Operator = drd.GetOrdinal("Operator");
                obeComentarioLlamada = new beComentarioLlamada();
                lbeComentario = new List<beComentario>();

                beComentario obeComentario;
                while (drd.Read())
                {
                    obeComentario = new beComentario();
                    obeComentario.Fecha = drd.GetDateTime(Datetimeoccurred).ToString("dd/MM/yyyy HH:mm:ss");
                    obeComentario.Comentario = drd.GetString(OperatorNote);
                    obeComentario.Operador = drd.GetString(Operator);
                    lbeComentario.Add(obeComentario);
                }
                if (drd.NextResult())
                {
                    int Datetimeoccurred2 = drd.GetOrdinal("Datetimeoccurred");
                    int Observacion = drd.GetOrdinal("Observacion");
                    int Operator2 = drd.GetOrdinal("Operator");
                    int NomCategoria = drd.GetOrdinal("Categoria");
                    lbeLlamadas = new List<beLlamada>();
                    beLlamada obeLlamada;
                    while (drd.Read())
                    {
                        obeLlamada = new beLlamada();
                        obeLlamada.Fecha = drd.GetDateTime(Datetimeoccurred2).ToString("dd/MM/yyyy HH:mm:ss");
                        obeLlamada.Comentario = drd.GetString(Observacion);
                        obeLlamada.Operador = drd.GetString(Operator2);
                        obeLlamada.NomCategoria = drd.GetString(NomCategoria);
                        lbeLlamadas.Add(obeLlamada);
                    }

                    if (drd.NextResult())
                    {
                        int Fecha = drd.GetOrdinal("DateTimeOccurred");
                        int AlarmCode = drd.GetOrdinal("AlarmCode");
                        int AlarmDesc = drd.GetOrdinal("Description");                        
                        lbeEvento = new List<beEvento>();
                        beEvento obeEvento;
                        while (drd.Read())
                        {
                            obeEvento = new beEvento();
                            obeEvento.Fecha = drd.GetDateTime(Fecha).ToString("dd/MM/yyyy HH:mm:ss");
                            obeEvento.AlarmCode = drd.GetString(AlarmCode);
                            obeEvento.AlarmDesc = drd.GetString(AlarmDesc);
                            lbeEvento.Add(obeEvento);
                        }
                    }


                }
                obeComentarioLlamada.lstComentario = lbeComentario;
                obeComentarioLlamada.lstLlamada = lbeLlamadas;
                obeComentarioLlamada.lstEvento = lbeEvento;
                drd.Close();
            }
            return obeComentarioLlamada;
        }
    }
}
