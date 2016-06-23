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
    public class daMonitoreo
    {
        public beMonitoreo MensajeAlarma(SqlConnection con, string user, short perfil)
        {
            SqlCommand cmd = new SqlCommand("Sp_get_senal_pendiente", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@user", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = user;

            SqlParameter param2 = cmd.Parameters.Add("@profile", SqlDbType.SmallInt);
            param2.Direction = ParameterDirection.Input;
            param2.Value = perfil;

            SqlDataReader drd = cmd.ExecuteReader();
            beMonitoreo obeMonitoreo = null;
            if (drd != null)
            {
                obeMonitoreo = new beMonitoreo();
                int Descripcion = drd.GetOrdinal("Descripcion");
                int CSID = drd.GetOrdinal("CSID");
                int SignalIdentifier = drd.GetOrdinal("SignalIdentifier");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int AlarmHistoryID = drd.GetOrdinal("AlarmHistoryID");
                int SubscriberName = drd.GetOrdinal("SubscriberName");
                int DealerName = drd.GetOrdinal("DealerName");
                int Direccion = drd.GetOrdinal("Direccion");
                int Atencion = drd.GetOrdinal("Atencion");
                int Fecha = drd.GetOrdinal("Fecha");
                int FechaA = drd.GetOrdinal("FechaAssigned");
                int Telfonopri = drd.GetOrdinal("Telfonopri");
                int Telfonopriext = drd.GetOrdinal("Telfonopriext");
                int Telfonoalt1 = drd.GetOrdinal("Telfonoalt1");
                int Telfonoalt1ext = drd.GetOrdinal("Telefonoalt1ext");
                int Telfonoalt2 = drd.GetOrdinal("Telfonoalt2");
                int Telfonoalt2ext = drd.GetOrdinal("Telefonoalt2ext");
                int Alarmcode = drd.GetOrdinal("Alarmcode");
                int Nomzona = drd.GetOrdinal("Nomzona");
                int Tipoevento = drd.GetOrdinal("Tipoevento");
                int Usuario = drd.GetOrdinal("Usuario");
                int TipoParticiondes = drd.GetOrdinal("TipoParticiondes");

                beSenal obeSenal = null;
                List<beContactoSeguridad> lbeContactoSeguridad = null;
                List<beContactoLocacion> lbeContactoLocacion = null;
                List<beEmergencia> lbeEmergencia = null;
                if (drd.HasRows)
                {
                    drd.Read();
                    obeSenal = new beSenal();
                    obeSenal.Descripcion = drd.GetString(Descripcion);
                    obeSenal.AbndCode = drd.GetString(CSID);
                    obeSenal.SignalIdentifier = drd.GetString(SignalIdentifier);
                    obeSenal.PhysicalZone = drd.GetString(PhysicalZone);
                    obeSenal.AlarmHistoryID = drd.GetInt32(AlarmHistoryID);
                    obeSenal.Oficina = drd.GetString(SubscriberName);
                    obeSenal.DealerName = drd.GetString(DealerName);
                    obeSenal.Direccion = drd.GetString(Direccion);
                    obeSenal.Atencion = drd.GetString(Atencion);
                    obeSenal.Fecha = drd.GetDateTime(Fecha).ToString("dd/MM/yyyy HH:mm:ss");
                    obeSenal.FechaA = drd.GetDateTime(FechaA).ToString("dd/MM/yyyy HH:mm:ss");
                    obeSenal.Telfonopri = drd.GetString(Telfonopri);
                    obeSenal.Telfonopriext = drd.GetString(Telfonopriext);
                    obeSenal.Telfonoalt1 = drd.GetString(Telfonoalt1);
                    obeSenal.Telfonoalt1ext = drd.GetString(Telfonoalt1ext);
                    obeSenal.Telfonoalt2 = drd.GetString(Telfonoalt2);
                    obeSenal.Telfonoalt2ext = drd.GetString(Telfonoalt2ext);
                    obeSenal.Alarmcode = drd.GetString(Alarmcode);
                    obeSenal.Nomzona = drd.GetString(Nomzona);
                    obeSenal.Tipoevento = drd.GetString(Tipoevento);
                    obeSenal.Usuario = drd.GetString(Usuario);
                    obeSenal.TipoParticiondes = drd.GetString(TipoParticiondes);

                    if (drd.NextResult())
                    {
                        int NombreContact = drd.GetOrdinal("NombreContact");
                        int ApePatContact = drd.GetOrdinal("ApePatContact");
                        int ApeMatContact = drd.GetOrdinal("ApeMatContact");
                        int CargoContact = drd.GetOrdinal("CargoContact");
                        int TelefonoContact = drd.GetOrdinal("TelefonoContact");

                        lbeContactoSeguridad = new List<beContactoSeguridad>();
                        beContactoSeguridad obeContactoSeguridad;
                        while (drd.Read())
                        {
                            obeContactoSeguridad = new beContactoSeguridad();
                            obeContactoSeguridad.NombreContact = drd.GetString(NombreContact);
                            obeContactoSeguridad.ApePatContact = drd.GetString(ApePatContact);
                            obeContactoSeguridad.ApeMatContact = drd.GetString(ApeMatContact);
                            obeContactoSeguridad.CargoContact = drd.GetString(CargoContact);
                            obeContactoSeguridad.TelefonoContact = drd.GetString(TelefonoContact);
                            lbeContactoSeguridad.Add(obeContactoSeguridad);
                        }

                        if (drd.NextResult())
                        {
                            int pCsid = drd.GetOrdinal("Csid");
                            int pName = drd.GetOrdinal("Name");
                            int pTitle = drd.GetOrdinal("Title");
                            int pPhoneNumber = drd.GetOrdinal("PhoneNumber");

                            lbeContactoLocacion = new List<beContactoLocacion>();
                            beContactoLocacion obeContactoLocacion;
                            while (drd.Read())
                            {
                                obeContactoLocacion = new beContactoLocacion();
                                obeContactoLocacion.Csid = drd.GetString(pCsid);
                                obeContactoLocacion.Name = drd.GetString(pName);
                                obeContactoLocacion.Title = drd.GetString(pTitle);
                                obeContactoLocacion.PhoneNumber = drd.GetString(pPhoneNumber);
                                lbeContactoLocacion.Add(obeContactoLocacion);
                            }

                            if (drd.NextResult())
                            {
                                int idCategoria = drd.GetOrdinal("idCategoria");
                                int alarmhistoryid = drd.GetOrdinal("alarmhistoryid");
                                int identidad = drd.GetOrdinal("RecordType");
                                int nomentidad = drd.GetOrdinal("nomentidad");
                                int telefono01 = drd.GetOrdinal("telefono01");
                                int telefono02 = drd.GetOrdinal("telefono02");
                                int horarequerimiento = drd.GetOrdinal("DateTimeOccurred");
                                int horaarribo = drd.GetOrdinal("horaarribo");
                                int nroidenpersontrans = drd.GetOrdinal("IdPersonaUnidad");
                                int nomresponsable = drd.GetOrdinal("Responsable");
                                int observacion = drd.GetOrdinal("OperatorNote");
                                int idMotivo = drd.GetOrdinal("MotivoId");
                                int nomMotivo = drd.GetOrdinal("sNomMotivo");

                                lbeEmergencia = new List<beEmergencia>();
                                beEmergencia obeEmergencia;
                                while (drd.Read())
                                {
                                    obeEmergencia = new beEmergencia();
                                    obeEmergencia.IdCategoria = drd.GetInt32(idCategoria);
                                    obeEmergencia.alarmhistoryid = drd.GetInt32(alarmhistoryid);
                                    obeEmergencia.identidad = drd.GetInt32(identidad);
                                    obeEmergencia.nomentidad = drd.GetString(nomentidad);
                                    obeEmergencia.telefono01 = drd.GetString(telefono01);
                                    obeEmergencia.telefono02 = drd.GetString(telefono02);
                                    obeEmergencia.horarequerimiento = drd.GetDateTime(horarequerimiento).ToString("dd/MM/yyyy HH:mm:ss");
                                    obeEmergencia.horaarribo = drd.GetString(horaarribo);
                                    obeEmergencia.nroidenpersontrans = drd.GetString(nroidenpersontrans);
                                    obeEmergencia.nomresponsable = drd.GetString(nomresponsable);
                                    obeEmergencia.Observacion = drd.GetString(observacion);
                                    obeEmergencia.idMotivo = drd.GetInt32(idMotivo);
                                    obeEmergencia.nomMotivo = drd.GetString(nomMotivo);
                                    lbeEmergencia.Add(obeEmergencia);
                                }
                            }
                        }
                    }
                }
                drd.Close();
                obeMonitoreo.obeSenal = obeSenal;
                obeMonitoreo.lstContactoSeguridad = lbeContactoSeguridad;
                obeMonitoreo.lstContactoLocacion = lbeContactoLocacion;
                obeMonitoreo.lstEmergencia = lbeEmergencia;
            }
            return obeMonitoreo;
        }

        public List<beSenal> ListarAlarmasPendientes(SqlConnection con, string user, short perfil)
        {
            SqlCommand cmd = new SqlCommand("Sp_get_senales_pendientes", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@user", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = user;

            SqlParameter param2 = cmd.Parameters.Add("@profile", SqlDbType.SmallInt);
            param2.Direction = ParameterDirection.Input;
            param2.Value = perfil;

            SqlDataReader drd = cmd.ExecuteReader();
            List<beSenal> lbeSenal = null;
            if (drd != null)
            {
                lbeSenal = new List<beSenal>();
                int Descripcion = drd.GetOrdinal("Descripcion");
                int CSID = drd.GetOrdinal("CSID");
                int SignalIdentifier = drd.GetOrdinal("SignalIdentifier");
                int PhysicalZone = drd.GetOrdinal("PhysicalZone");
                int AlarmHistoryID = drd.GetOrdinal("AlarmHistoryID");
                int SubscriberName = drd.GetOrdinal("SubscriberName");
                int DealerName = drd.GetOrdinal("DealerName");
                int Direccion = drd.GetOrdinal("Direccion");
                int Atencion = drd.GetOrdinal("Atencion");
                int Fecha = drd.GetOrdinal("Fecha");
                int FechaA = drd.GetOrdinal("FechaA");
                int Telfonopri = drd.GetOrdinal("Telfonopri");
                int Telfonopriext = drd.GetOrdinal("Telfonopriext");
                int Telfonoalt1 = drd.GetOrdinal("Telfonoalt1");
                int Telfonoalt1ext = drd.GetOrdinal("Telefonoalt1ext");
                int Telfonoalt2 = drd.GetOrdinal("Telfonoalt2");
                int Telfonoalt2ext = drd.GetOrdinal("Telefonoalt2ext");
                int Alarmcode = drd.GetOrdinal("Alarmcode");
                int Nomzona = drd.GetOrdinal("Nomzona");
                int Tipoevento = drd.GetOrdinal("Tipoevento");
                int Usuario = drd.GetOrdinal("Usuario");
                int Priority = drd.GetOrdinal("Priority");
                int Estado = drd.GetOrdinal("Estado");
                int Escala = drd.GetOrdinal("bEscalado");
                int TipoParticiondes = drd.GetOrdinal("TipoParticiondes");

                beSenal obeSenal = null;
                while (drd.Read())
                {
                    obeSenal = new beSenal();
                    obeSenal.Descripcion = drd.GetString(Descripcion);
                    obeSenal.AbndCode = drd.GetString(CSID);
                    obeSenal.SignalIdentifier = drd.GetString(SignalIdentifier);
                    obeSenal.PhysicalZone = drd.GetString(PhysicalZone);
                    obeSenal.AlarmHistoryID = drd.GetInt32(AlarmHistoryID);
                    obeSenal.Oficina = drd.GetString(SubscriberName);
                    obeSenal.DealerName = drd.GetString(DealerName);
                    obeSenal.Direccion = drd.GetString(Direccion);
                    obeSenal.Atencion = drd.GetString(Atencion);
                    obeSenal.Fecha = drd.GetDateTime(Fecha).ToString("dd/MM/yyyy HH:mm:ss");
                    obeSenal.FechaA = drd.GetDateTime(FechaA).ToString("dd/MM/yyyy HH:mm:ss");
                    obeSenal.Telfonopri = drd.GetString(Telfonopri);
                    obeSenal.Telfonopriext = drd.GetString(Telfonopriext);
                    obeSenal.Telfonoalt1 = drd.GetString(Telfonoalt1);
                    obeSenal.Telfonoalt1ext = drd.GetString(Telfonoalt1ext);
                    obeSenal.Telfonoalt2 = drd.GetString(Telfonoalt2);
                    obeSenal.Telfonoalt2ext = drd.GetString(Telfonoalt2ext);
                    obeSenal.Alarmcode = drd.GetString(Alarmcode);
                    obeSenal.Nomzona = drd.GetString(Nomzona);
                    obeSenal.Tipoevento = drd.GetString(Tipoevento);
                    obeSenal.Usuario = drd.GetString(Usuario);
                    obeSenal.Priority = drd.GetInt16(Priority);
                    obeSenal.Estado = drd.GetString(Estado);
                    obeSenal.Escala = drd.GetInt32(Escala);
                    obeSenal.TipoParticiondes = drd.GetString(TipoParticiondes);
                    lbeSenal.Add(obeSenal);
                }
                drd.Close();
            }
            return lbeSenal;
        }

        public List<beAlarmasResolution> ListarResolucion(SqlConnection con)
        {
            List<beAlarmasResolution> lbeAlarmas = null;
            SqlCommand cmd = new SqlCommand("sp_listar_resolucion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int AlarmCode = drd.GetOrdinal("ResolutionCode");
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

        public bool SenalPendienteEnEspera(SqlConnection con, int AlarmHistoryID, int time)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("sp_postergar_senial_pendiente", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@palarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = AlarmHistoryID;

            SqlParameter param2 = cmd.Parameters.Add("@minutos", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = time;

            int n = cmd.ExecuteNonQuery();

            if (n > 0) exito = true;
            return exito;
        }

        public bool SenalPendienteCierre(SqlConnection con, int AlarmHistoryID, string ResolutionCode)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("sp_actualizar_senial_pendiente", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@palarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = AlarmHistoryID;

            SqlParameter param2 = cmd.Parameters.Add("@presolutioncode", SqlDbType.VarChar, 3);
            param2.Direction = ParameterDirection.Input;
            param2.Value = ResolutionCode;

            int n = cmd.ExecuteNonQuery();

            if (n > 0) exito = true;
            return exito;
        }

        public bool SenalAEscalar(SqlConnection con, int AlarmHistoryID)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("Sp_Escalar_SenalPendiente", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@AlarmHistoryID", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = AlarmHistoryID;

            int n = cmd.ExecuteNonQuery();

            if (n > 0) exito = true;
            return exito;
        }

        public beMonitoreo SenalExtraData(SqlConnection con, int ahid)
        {
            SqlCommand cmd = new SqlCommand("sp_get_data_senal", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@AlarmHistoryID", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = ahid;

            SqlDataReader drd = cmd.ExecuteReader();
            beMonitoreo obeMonitoreo = null;
            List<beContactoSeguridad> lbeContactoSeguridad = null;
            List<beContactoLocacion> lbeContactoLocacion = null;
            List<beEmergencia> lbeEmergencia = null;
            if (drd != null)
            {
                obeMonitoreo = new beMonitoreo();
                int NombreContact = drd.GetOrdinal("NombreContact");
                int ApePatContact = drd.GetOrdinal("ApePatContact");
                int ApeMatContact = drd.GetOrdinal("ApeMatContact");
                int CargoContact = drd.GetOrdinal("CargoContact");
                int TelefonoContact = drd.GetOrdinal("TelefonoContact");
                lbeContactoSeguridad = new List<beContactoSeguridad>();
                beContactoSeguridad obeContactoSeguridad;
                while (drd.Read())
                {
                    obeContactoSeguridad = new beContactoSeguridad();
                    obeContactoSeguridad.NombreContact = drd.GetString(NombreContact);
                    obeContactoSeguridad.ApePatContact = drd.GetString(ApePatContact);
                    obeContactoSeguridad.ApeMatContact = drd.GetString(ApeMatContact);
                    obeContactoSeguridad.CargoContact = drd.GetString(CargoContact);
                    obeContactoSeguridad.TelefonoContact = drd.GetString(TelefonoContact);
                    lbeContactoSeguridad.Add(obeContactoSeguridad);
                }
                if (drd.NextResult())
                {
                    int pCsid = drd.GetOrdinal("Csid");
                    int pName = drd.GetOrdinal("Name");
                    int pTitle = drd.GetOrdinal("Title");
                    int pPhoneNumber = drd.GetOrdinal("PhoneNumber");

                    lbeContactoLocacion = new List<beContactoLocacion>();
                    beContactoLocacion obeContactoLocacion;
                    while (drd.Read())
                    {
                        obeContactoLocacion = new beContactoLocacion();
                        obeContactoLocacion.Csid = drd.GetString(pCsid);
                        obeContactoLocacion.Name = drd.GetString(pName);
                        obeContactoLocacion.Title = drd.GetString(pTitle);
                        obeContactoLocacion.PhoneNumber = drd.GetString(pPhoneNumber);
                        lbeContactoLocacion.Add(obeContactoLocacion);
                    }
                    if (drd.NextResult())
                    {
                        int idCategoria = drd.GetOrdinal("idCategoria");
                        int alarmhistoryid = drd.GetOrdinal("alarmhistoryid");
                        int identidad = drd.GetOrdinal("RecordType");
                        int nomentidad = drd.GetOrdinal("nomentidad");
                        int telefono01 = drd.GetOrdinal("telefono01");
                        int telefono02 = drd.GetOrdinal("telefono02");
                        int horarequerimiento = drd.GetOrdinal("DateTimeOccurred");
                        int horaarribo = drd.GetOrdinal("horaarribo");
                        int nroidenpersontrans = drd.GetOrdinal("IdPersonaUnidad");
                        int nomresponsable = drd.GetOrdinal("Responsable");
                        int observacion = drd.GetOrdinal("OperatorNote");
                        int idMotivo = drd.GetOrdinal("MotivoId");
                        int nomMotivo = drd.GetOrdinal("sNomMotivo");

                        lbeEmergencia = new List<beEmergencia>();
                        beEmergencia obeEmergencia;
                        while (drd.Read())
                        {
                            obeEmergencia = new beEmergencia();
                            obeEmergencia.IdCategoria = drd.GetInt32(idCategoria);
                            obeEmergencia.alarmhistoryid = drd.GetInt32(alarmhistoryid);
                            obeEmergencia.identidad = drd.GetInt32(identidad);
                            obeEmergencia.nomentidad = drd.GetString(nomentidad);
                            obeEmergencia.telefono01 = drd.GetString(telefono01);
                            obeEmergencia.telefono02 = drd.GetString(telefono02);
                            obeEmergencia.horarequerimiento = drd.GetDateTime(horarequerimiento).ToString("dd/MM/yyyy HH:mm:ss");
                            obeEmergencia.horaarribo = drd.GetString(horaarribo);
                            obeEmergencia.nroidenpersontrans = drd.GetString(nroidenpersontrans);
                            obeEmergencia.nomresponsable = drd.GetString(nomresponsable);
                            obeEmergencia.Observacion = drd.GetString(observacion);
                            obeEmergencia.idMotivo = drd.GetInt32(idMotivo);
                            obeEmergencia.nomMotivo = drd.GetString(nomMotivo);
                            lbeEmergencia.Add(obeEmergencia);
                        }
                    }
                }
                drd.Close();
                obeMonitoreo.obeSenal = null;
                obeMonitoreo.lstContactoSeguridad = lbeContactoSeguridad;
                obeMonitoreo.lstContactoLocacion = lbeContactoLocacion;
                obeMonitoreo.lstEmergencia = lbeEmergencia;
            }
            return obeMonitoreo;
        }

        public List<beEvento> SenalHistoricoEvento(SqlConnection con, int ahid)
        {
            SqlCommand cmd = new SqlCommand("Sp_Listar_HistorialEventos", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@AlarmHistoryID", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = ahid;

            SqlDataReader drd = cmd.ExecuteReader();
            List<beEvento> lbeEvento = null;
            if (drd != null)
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
                drd.Close();
            }
            return lbeEvento;
        }

        public int  AsignarAlarma(SqlConnection con, int ahid, string user, int perfil)
        {
            int asignado = -1;
            SqlCommand cmd = new SqlCommand("Sp_Asignar_Usuario_Alarma", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@AlarmHistoryID", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = ahid;

            SqlParameter param2 = cmd.Parameters.Add("@User", SqlDbType.VarChar, 10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = user;

            SqlParameter param3 = cmd.Parameters.Add("@Profile", SqlDbType.Int);
            param3.Direction = ParameterDirection.Input;
            param3.Value = perfil;

            asignado = cmd.ExecuteNonQuery();
            return asignado;
        }

        public bool AgregarComentario(SqlConnection con, int palarmhistoryid, DateTime Hora, string pObservacion, string usuario)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "SP_Insertar_Comentario";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param3 = cmd.Parameters.Add("@horarequerimiento", SqlDbType.DateTime);
            param3.Direction = ParameterDirection.Input;
            param3.Value = Hora;

            SqlParameter param5 = cmd.Parameters.Add("@observacion", SqlDbType.VarChar, 500);
            param5.Direction = ParameterDirection.Input;
            param5.Value = pObservacion.ToUpper();

            SqlParameter param7 = cmd.Parameters.Add("@operador", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = usuario.ToUpper();

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0) bExito = true;

            return bExito;
        }

        public List<beEntidad> ComboEntidad(SqlConnection con, int palarmhistoryid, int pidcategoria, int pidentidad)
        {
            List<beEntidad> lbeEntidad = null;

            SqlCommand cmd = new SqlCommand("Sp_Listar_Entidad", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@idcategoria", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidcategoria;

            SqlParameter param3 = cmd.Parameters.Add("@identidad", SqlDbType.Int);
            param3.Direction = ParameterDirection.Input;
            param3.Value = pidentidad;

            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (drd != null)
            {
                int identidad = drd.GetOrdinal("identidad");
                int nomentidad = drd.GetOrdinal("nomentidad");
                int telefono01 = drd.GetOrdinal("telefono01");
                int telefono02 = drd.GetOrdinal("telefono02");


                lbeEntidad = new List<beEntidad>();
                beEntidad obeEntidad;
                while (drd.Read())
                {
                    obeEntidad = new beEntidad();
                    obeEntidad.identidad = drd.GetInt32(identidad);
                    obeEntidad.nomentidad = drd.GetString(nomentidad);
                    obeEntidad.telefono01 = drd.GetString(telefono01);
                    obeEntidad.telefono02 = drd.GetString(telefono02);
                    lbeEntidad.Add(obeEntidad);
                }
                drd.Close();
            }

            return lbeEntidad;
        }

        public List<beMotivo> ComboMotivo(SqlConnection con)
        {
            List<beMotivo> lbeMotivo = null;

            SqlCommand cmd = new SqlCommand("Sp_Listar_Motivo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (drd != null)
            {
                int idMotivo = drd.GetOrdinal("idMotivo");
                int nomMotivo = drd.GetOrdinal("sNomMotivo");
                lbeMotivo = new List<beMotivo>();
                beMotivo obeEntidad;
                while (drd.Read())
                {
                    obeEntidad = new beMotivo();
                    obeEntidad.idMotivo = drd.GetInt32(idMotivo);
                    obeEntidad.nomMotivo = drd.GetString(nomMotivo);
                    lbeMotivo.Add(obeEntidad);
                }
                drd.Close();
            }

            return lbeMotivo;
        }

        public List<beUserLogIn> ListarUsuariosEnLinea(SqlConnection con, string Usuario)
        {
            List<beUserLogIn> lbeUserLogIn = null;

            SqlCommand cmd = new SqlCommand("Sp_Listar_Usuarios_LogIn", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@user", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = Usuario;
            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleResult);

            if (drd != null)
            {
                lbeUserLogIn = new List<beUserLogIn>();
                beUserLogIn obeUserLogIn;
                while (drd.Read())
                {
                    obeUserLogIn = new beUserLogIn();
                    obeUserLogIn.User = drd.GetString(0);
                    obeUserLogIn.Nombres = drd.GetString(1);
                    obeUserLogIn.Cargo = drd.GetString(2);
                    lbeUserLogIn.Add(obeUserLogIn);
                }
                drd.Close();
            }

            return lbeUserLogIn;
        }

        public bool RelevarAlarmas(SqlConnection con, string UsuarioActual, string UserAsignado)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("Sp_Relevar_Alarmas", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@UserRelevado", SqlDbType.VarChar,10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = UsuarioActual;

            SqlParameter param2 = cmd.Parameters.Add("@UserAsignado", SqlDbType.VarChar,10);
            param2.Direction = ParameterDirection.Input;
            param2.Value = UserAsignado;

            int n = cmd.ExecuteNonQuery();

            if (n > 0) exito = true;
            return exito;
        }

        public List<beEmergencia> ConsultarEmergencia(SqlConnection con, int palarmhistoryid, int pidcategoria)
        {
            List<beEmergencia> lbeEmergencia = null;

            SqlCommand cmd = new SqlCommand("sp_consultar_emergencia", con);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@idcategoria", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidcategoria;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int IdCategoria = drd.GetOrdinal("IdCategoria");
                int alarmhistoryid = drd.GetOrdinal("alarmhistoryid");
                int identidad = drd.GetOrdinal("RecordType");
                int nomentidad = drd.GetOrdinal("nomentidad");
                int telefono01 = drd.GetOrdinal("telefono01");
                int telefono02 = drd.GetOrdinal("telefono02");
                int horarequerimiento = drd.GetOrdinal("DateTimeOccurred");
                int horaarribo = drd.GetOrdinal("horaarribo");
                int nroidenpersontrans = drd.GetOrdinal("IdPersonaUnidad");
                int nomresponsable = drd.GetOrdinal("Responsable");
                int observacion = drd.GetOrdinal("OperatorNote");
                int idMotivo = drd.GetOrdinal("MotivoId");
                int nomMotivo = drd.GetOrdinal("sNomMotivo");

                lbeEmergencia = new List<beEmergencia>();
                beEmergencia obeEmergencia = new beEmergencia();
                while (drd.Read())
                {
                    obeEmergencia = new beEmergencia();
                    obeEmergencia.IdCategoria = drd.GetInt32(IdCategoria);
                    obeEmergencia.alarmhistoryid = drd.GetInt32(alarmhistoryid);
                    obeEmergencia.identidad = drd.GetInt32(identidad);
                    obeEmergencia.nomentidad = drd.GetString(nomentidad);
                    obeEmergencia.telefono01 = drd.GetString(telefono01);
                    obeEmergencia.telefono02 = drd.GetString(telefono02);
                    obeEmergencia.horarequerimiento = drd.GetDateTime(horarequerimiento).ToString("dd/MM/yyyy HH:mm:ss");
                    obeEmergencia.horaarribo = drd.GetString(horaarribo);
                    obeEmergencia.nroidenpersontrans = drd.GetString(nroidenpersontrans);
                    obeEmergencia.nomresponsable = drd.GetString(nomresponsable);
                    obeEmergencia.Observacion = drd.GetString(observacion);
                    obeEmergencia.idMotivo = drd.GetInt32(idMotivo);
                    obeEmergencia.nomMotivo = drd.GetString(nomMotivo);
                    lbeEmergencia.Add(obeEmergencia);
                }
                drd.Close();
            }
            return lbeEmergencia;
        }

        public bool InsertarEmergencia(SqlConnection con, int palarmhistoryid, int pidentidad, DateTime phorarequerimiento, string pnroidenpersontrans, string pnomresponsable, string usuario)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "sp_insertar_emergencia";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@identidad", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidentidad;

            SqlParameter param3 = cmd.Parameters.Add("@horarequerimiento", SqlDbType.DateTime);
            param3.Direction = ParameterDirection.Input;
            param3.Value = phorarequerimiento;

            SqlParameter param5 = cmd.Parameters.Add("@nroidenpersontrans", SqlDbType.VarChar, 20);
            param5.Direction = ParameterDirection.Input;
            param5.Value = pnroidenpersontrans.ToUpper();

            SqlParameter param6 = cmd.Parameters.Add("@nomresponsable", SqlDbType.VarChar, 60);
            param6.Direction = ParameterDirection.Input;
            param6.Value = pnomresponsable.ToUpper();

            SqlParameter param7 = cmd.Parameters.Add("@operador", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = usuario.ToUpper();

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0) bExito = true;

            return bExito;
        }

        public bool InsertarEmergenciaStopFraude(SqlConnection con, int palarmhistoryid, int pidMotivo, DateTime phorarequerimiento, string pObservacion, string pnomresponsable, string usuario)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "Sp_insertar_Observacion";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@motivo", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidMotivo;

            SqlParameter param3 = cmd.Parameters.Add("@horarequerimiento", SqlDbType.DateTime);
            param3.Direction = ParameterDirection.Input;
            param3.Value = phorarequerimiento;

            SqlParameter param5 = cmd.Parameters.Add("@observacion", SqlDbType.VarChar, 500);
            param5.Direction = ParameterDirection.Input;
            param5.Value = pObservacion.ToUpper();

            SqlParameter param6 = cmd.Parameters.Add("@nomresponsable", SqlDbType.VarChar, 60);
            param6.Direction = ParameterDirection.Input;
            param6.Value = pnomresponsable.ToUpper();

            SqlParameter param7 = cmd.Parameters.Add("@operador", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = usuario.ToUpper();

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0) bExito = true;

            return bExito;
        }

        public bool ActualizarEmergencia(SqlConnection con, int palarmhistoryid, int pidentidadOld, int pidentidadNew, DateTime phoraarribo, string pnroidenpersontrans, string pnomresponsable, string usuario)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "sp_actualizar_emergencia";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@identidadold", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidentidadOld;

            SqlParameter param3 = cmd.Parameters.Add("@identidadnew", SqlDbType.Int);
            param3.Direction = ParameterDirection.Input;
            param3.Value = pidentidadNew;

            SqlParameter param4 = cmd.Parameters.Add("@horaarribo", SqlDbType.DateTime);
            param4.Direction = ParameterDirection.Input;
            param4.Value = phoraarribo;

            SqlParameter param5 = cmd.Parameters.Add("@nroidenpersontrans", SqlDbType.VarChar, 20);
            param5.Direction = ParameterDirection.Input;
            param5.Value = pnroidenpersontrans.ToUpper();

            SqlParameter param6 = cmd.Parameters.Add("@nomresponsable", SqlDbType.VarChar, 60);
            param6.Direction = ParameterDirection.Input;
            param6.Value = pnomresponsable.ToUpper();

            SqlParameter param7 = cmd.Parameters.Add("@operador", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = usuario.ToUpper();

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0)
            {
                bExito = true;
            }

            return bExito;
        }

        public bool ActualizarEmergenciaStopFraude(SqlConnection con, int palarmhistoryid, int pidMotivoOld, int pidMotivoNew, string pObservacion, string pnomresponsable, string usuario)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "Sp_actualizar_Observacion";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@motivoold", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidMotivoOld;

            SqlParameter param3 = cmd.Parameters.Add("@motivonew", SqlDbType.Int);
            param3.Direction = ParameterDirection.Input;
            param3.Value = pidMotivoNew;

            SqlParameter param5 = cmd.Parameters.Add("@observacion", SqlDbType.VarChar, 500);
            param5.Direction = ParameterDirection.Input;
            param5.Value = pObservacion.ToUpper();

            SqlParameter param6 = cmd.Parameters.Add("@nomresponsable", SqlDbType.VarChar, 60);
            param6.Direction = ParameterDirection.Input;
            param6.Value = pnomresponsable.ToUpper();

            SqlParameter param7 = cmd.Parameters.Add("@operador", SqlDbType.VarChar, 10);
            param7.Direction = ParameterDirection.Input;
            param7.Value = usuario.ToUpper();

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0)
            {
                bExito = true;
            }

            return bExito;
        }

        public bool EliminarEmergencia(SqlConnection con, int palarmhistoryid, int pidentidad)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "sp_eliminar_emergencia";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@identidad", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidentidad;

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0) bExito = true;
            return bExito;
        }

        public bool EliminarEmergenciaStopFraude(SqlConnection con, int palarmhistoryid, int pidMotivo)
        {
            bool bExito = false;
            int returnFilas = 0;
            string sql = "Sp_eliminar_Observacion";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@alarmhistoryid", SqlDbType.Int);
            param1.Direction = ParameterDirection.Input;
            param1.Value = palarmhistoryid;

            SqlParameter param2 = cmd.Parameters.Add("@idMotivo", SqlDbType.Int);
            param2.Direction = ParameterDirection.Input;
            param2.Value = pidMotivo;

            returnFilas = cmd.ExecuteNonQuery();

            if (returnFilas > 0) bExito = true;
            return bExito;
        }

        public int SenalPendienteNoMonitoreo(SqlConnection con, string user, short perfil)
        {
            int Exito = -1;
            string sql = "SP_get_senal_pendiente_NoMonitoreo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = cmd.Parameters.Add("@user", SqlDbType.VarChar, 10);
            param1.Direction = ParameterDirection.Input;
            param1.Value = user;

            SqlParameter param2 = cmd.Parameters.Add("@profile", SqlDbType.SmallInt);
            param2.Direction = ParameterDirection.Input;
            param2.Value = perfil;

            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if (drd != null)
            {
                drd.Read();
                Exito = drd.GetInt32(0);
            }
            return Exito;
        }
    }
}
