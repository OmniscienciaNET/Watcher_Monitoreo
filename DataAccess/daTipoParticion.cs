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
    public class daTipoParticion
    {
        public List<beTipoParticion> ComboTipoParticion(SqlConnection con)
        {
            List<beTipoParticion> lbeTipoParticion = null;

            SqlCommand cmd = new SqlCommand("sp_web_combo_Tipoparticion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            //SqlParameter param1 = cmd.Parameters.Add("@pDstbCode", SqlDbType.VarChar, 10);
            //param1.Direction = ParameterDirection.Input;
            //param1.Value = DstbCode.ToUpper();

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int TipoParticionid = drd.GetOrdinal("TipoParticionid");
                int TipoParticiondes = drd.GetOrdinal("TipoParticiondes");

                lbeTipoParticion = new List<beTipoParticion>();
                beTipoParticion obeTipoParticion;
                while (drd.Read())
                {
                    obeTipoParticion = new beTipoParticion();
                    obeTipoParticion.TipoParticionid = drd.GetInt32(TipoParticionid);
                    obeTipoParticion.TipoParticiondes = drd.GetString(TipoParticiondes);
                    lbeTipoParticion.Add(obeTipoParticion);
                }
                drd.Close();
            }
            return lbeTipoParticion;
        }
    }
}
