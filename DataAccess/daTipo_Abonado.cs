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
    public class daTipo_Abonado
    {
        public List<beTipo_Abonado> ComboTipo_Abonado(SqlConnection con)
        {
            List<beTipo_Abonado> lbeTipo_Abonado= null;

            SqlCommand cmd = new SqlCommand("sp_web_combo_tipo_abonado", con);
            cmd.CommandType = CommandType.StoredProcedure;

            

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int Tipo_Abonado_id = drd.GetOrdinal("Tipo_Abonado_id");
                int Tipo_Abonado_des = drd.GetOrdinal("Tipo_Abonado_des");

                lbeTipo_Abonado = new List<beTipo_Abonado>();
                beTipo_Abonado obeTipo_Abonado;
                while (drd.Read())
                {
                    obeTipo_Abonado = new beTipo_Abonado();
                    obeTipo_Abonado.Tipo_Abonado_id = drd.GetInt32(Tipo_Abonado_id);
                    obeTipo_Abonado.Tipo_Abonado_des = drd.GetString(Tipo_Abonado_des);
                    lbeTipo_Abonado.Add(obeTipo_Abonado);
                }
                drd.Close();
            }
            return lbeTipo_Abonado;
        }
    }
}
