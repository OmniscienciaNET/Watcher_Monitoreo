using BusinessRules;
using Entity;
using General.Librerias.CodigoUsuario;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Watcher.Controllers
{
    public class ModuloController : Controller
    {
        int acceso = 0;
        public string LogOut()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brLogin br = new brLogin();
                rpta = br.LogOut(Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper())) ? "1" : "0";
                Session.Clear();
            }
            return rpta;
        }

        public string ListarAlarmas()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brActividades br = new brActividades();
                List<beAlarmasResolution> lstAlarmas = br.ListarAlarmas();
                lstAlarmas.Insert(0, new beAlarmasResolution { ResolutionCode = "", ResolutionDesc = "Seleccione" });
                if (lstAlarmas != null & lstAlarmas.Count > 0) rpta = CustomSerializer.Serializar(lstAlarmas, '@', '_', false).ToString();
            }
            return rpta;
        }

        public string ListarCondicion()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brSupervisionCaidas br = new brSupervisionCaidas();
                List<beAlarmasResolution> lstAlarmas = br.ListarCondicion();
                if (lstAlarmas != null & lstAlarmas.Count > 0) rpta = CustomSerializer.Serializar(lstAlarmas, '@', '_', false).ToString();
            }
            return rpta;
        }

        public string ListarCondicionAlarmas()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brControlArea br = new brControlArea();
                List<beAlarmasResolution> lstAlarmas = br.ListarCondicionAlarmas();
                lstAlarmas.Insert(0, new beAlarmasResolution { ResolutionCode = "0", ResolutionDesc = "TODOS" });
                if (lstAlarmas != null & lstAlarmas.Count > 0) rpta = CustomSerializer.Serializar(lstAlarmas, '@', '_', false).ToString();
            }
            return rpta;
        }

        public string ComentariosLlamadas(int ahid)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brVisor br = new brVisor();
                beComentarioLlamada obeCLL = br.ListarComentariosLlamadas(ahid);
                if (obeCLL != null) rpta = CustomSerializer.Serializar(obeCLL.lstComentario, '@', '_', false).ToString() + "◄" + CustomSerializer.Serializar(obeCLL.lstLlamada, '@', '_', false).ToString() + "◄" + CustomSerializer.Serializar(obeCLL.lstEvento, '@', '_', false).ToString();
            }
            return rpta;
        }

        /*public string ListarCombosMonitoreo()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brMonitoreo br = new brMonitoreo();
                beCombosMonitoreo obeCombosMonitoreo = br.CombosMonitoreo();
                if (obeCombosMonitoreo != null) rpta = CustomSerializer.Serializar(obeCombosMonitoreo.lbeResolucion, '@', '_', false).ToString() + "◄" + CustomSerializer.Serializar(obeCombosMonitoreo.lbeEntidad, '@', '_', false).ToString() + "◄" + CustomSerializer.Serializar(obeCombosMonitoreo.lbeMotivo, '@', '_', false).ToString();
            }
            return rpta;
        }*/

        public int SenalPendienteNoMonitoreo()
        {
            brMonitoreo br = new brMonitoreo();
            int estado = br.SenalPendienteNoMonitoreo(Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), short.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));
            return estado;
        }

        //Metodos Generales
        int registrosPagina = 25;
        int indiceActualPagina = 0;
        int indiceUltimaPagina;

        private List<T> Paginar<T>(string Pagina, List<T> lista)
        {
            if (Pagina == null)
            {
                Session["Modulo"] = lista;
                indiceUltimaPagina = lista.Count / registrosPagina;
                if (lista.Count % registrosPagina == 0) indiceUltimaPagina--;
                TempData["indiceActualPagina"] = indiceActualPagina;
                TempData["indiceUltimaPagina"] = indiceUltimaPagina;
            }
            else
            {
                lista = (List<T>)Session["Modulo"];
                indiceActualPagina = (int)TempData["indiceActualPagina"];
                indiceUltimaPagina = (int)TempData["indiceUltimaPagina"];
                if (Pagina.Equals("<<")) indiceActualPagina = 0;
                else
                {
                    if (Pagina.Equals("<"))
                    {
                        if (indiceActualPagina > 0) indiceActualPagina--;
                    }
                    else
                    {
                        if (Pagina.Equals(">"))
                        {
                            if (indiceActualPagina < indiceUltimaPagina) indiceActualPagina++;
                        }
                        else
                        {
                            if (Pagina.Equals(">>")) indiceActualPagina = indiceUltimaPagina;
                            else
                            {
                                indiceActualPagina = int.Parse(Pagina);
                            }
                        }
                    }
                }
                TempData["indiceUltimaPagina"] = indiceUltimaPagina;
                TempData["indiceActualPagina"] = indiceActualPagina;
            }
            return lista;
        }

        private List<T> mostrarPagina<T>(List<T> lista)
        {
            ViewBag.IndiceActualPagina = indiceActualPagina;
            ViewBag.IndiceUltimaPagina = indiceUltimaPagina;
            List<T> lbePagina = new List<T>();
            int inicio = indiceActualPagina * registrosPagina;
            int fin = inicio + registrosPagina;
            for (int i = inicio; i < fin; i++)
            {
                if (i < lista.Count) lbePagina.Add(lista[i]);
                else break;
            }
            return (lbePagina);
        }

        public FilePathResult Exportar()
        {
            string view = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            string archivoTxt = @"C:\Pruebas\" + view + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".txt";
            switch (view)
            {
                case "ControlArea":
                    exportarTxt(archivoTxt, (List<beControlArea>)Session["Modulo"], view);
                    break;
                case "ReporteActividades":
                    exportarTxt(archivoTxt, (List<beActividad>)Session["Modulo"], view);
                    break;
                case "EstadisticoAlarmas":
                    exportarTxt(archivoTxt, (List<beEstadisticoAlarmas>)Session["Modulo"], view);
                    break;
                default:
                    exportarTxt(archivoTxt, (List<beControlSenalSupCaidasFalloTestControlCierre>)Session["Modulo"], view);
                    break;
            }
            exportarExcel(archivoTxt);
            string tipo = "application/vnd.ms-excel;";
            Response.Clear();
            Response.ContentType = tipo;
            Response.AppendHeader("Content-Disposition", "attachmen;filename=" + Path.GetFileName(archivoTxt.Replace("txt", "xlsx")));
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(archivoTxt.Replace("txt", "xlsx"));
            Response.End();
            return File(archivoTxt, tipo, Path.GetFileName(archivoTxt));
        }

        public void exportarTxt<T>(string archivo, List<T> lista, string view)
        {
            using (StreamWriter sw = new StreamWriter(archivo))
            {
                PropertyInfo[] propiedades;
                switch (view)
                {
                    case "ControlArea":
                        sw.WriteLine("Fecha,Hora,Abonado,Oficina,Condicion,ID Senal,Usuario/Descripcion");
                        foreach (T obe in lista)
                        {
                            propiedades = obe.GetType().GetProperties();
                            sw.WriteLine((propiedades[5].GetValue(obe, null) == null ? "" : propiedades[5].GetValue(obe, null).ToString()) + "," + (propiedades[6].GetValue(obe, null) == null ? "" : propiedades[6].GetValue(obe, null).ToString()) + "," + (propiedades[10].GetValue(obe, null) == null ? "" : propiedades[10].GetValue(obe, null).ToString()) + "," + (propiedades[11].GetValue(obe, null) == null ? "" : propiedades[11].GetValue(obe, null).ToString()) + "," + (propiedades[8].GetValue(obe, null) == null ? "" : propiedades[8].GetValue(obe, null).ToString()) + "," + (propiedades[7].GetValue(obe, null) == null ? "" : propiedades[7].GetValue(obe, null).ToString()) + "," + (propiedades[9].GetValue(obe, null) == null ? "" : propiedades[9].GetValue(obe, null).ToString()));
                        }
                        break;
                    case "ReporteActividades":
                        sw.WriteLine("Fecha,Hora,Abonado,Oficina,ID Senal,Evento,Condicion,Descripcion Condicion,Usuario/Dispositivo/Ubicacion,Area,Descripcion Area");
                        foreach (T obe in lista)
                        {
                            propiedades = obe.GetType().GetProperties();
                            sw.WriteLine((propiedades[6].GetValue(obe, null) == null ? "" : propiedades[6].GetValue(obe, null).ToString()) + "," + (propiedades[7].GetValue(obe, null) == null ? "" : propiedades[7].GetValue(obe, null).ToString()) + "," + (propiedades[11].GetValue(obe, null) == null ? "" : propiedades[11].GetValue(obe, null).ToString()) + "," + (propiedades[12].GetValue(obe, null) == null ? "" : propiedades[12].GetValue(obe, null).ToString()) + "," + (propiedades[8].GetValue(obe, null) == null ? "" : propiedades[8].GetValue(obe, null).ToString()) + "," + (propiedades[1].GetValue(obe, null) == null ? "" : propiedades[1].GetValue(obe, null).ToString()) + "," + (propiedades[0].GetValue(obe, null) == null ? "" : propiedades[0].GetValue(obe, null).ToString()) + "," + (propiedades[9].GetValue(obe, null) == null ? "" : propiedades[9].GetValue(obe, null).ToString()) + "," + (propiedades[10].GetValue(obe, null) == null ? "" : propiedades[10].GetValue(obe, null).ToString()) + "," + (propiedades[2].GetValue(obe, null) == null ? "" : propiedades[2].GetValue(obe, null).ToString()) + "," + (propiedades[3].GetValue(obe, null) == null ? "" : propiedades[3].GetValue(obe, null).ToString()));
                        }
                        break;
                    case "EstadisticoAlarmas":
                        sw.WriteLine("Fecha,Hora,Abonado,Oficina,ID Senal,Evento,Descripcion,Descripcion Evento,Area,Descripcion Area");
                        foreach (T obe in lista)
                        {
                            propiedades = obe.GetType().GetProperties();
                            sw.WriteLine((propiedades[0].GetValue(obe, null) == null ? "" : propiedades[0].GetValue(obe, null).ToString()) + "," + (propiedades[4].GetValue(obe, null) == null ? "" : propiedades[4].GetValue(obe, null).ToString()) + "," + (propiedades[5].GetValue(obe, null) == null ? "" : propiedades[5].GetValue(obe, null).ToString()) + "," + (propiedades[1].GetValue(obe, null) == null ? "" : propiedades[1].GetValue(obe, null).ToString()) + "," + (propiedades[2].GetValue(obe, null) == null ? "" : propiedades[2].GetValue(obe, null).ToString()) + "," + (propiedades[3].GetValue(obe, null) == null ? "" : propiedades[3].GetValue(obe, null).ToString()));
                        }
                        break;
                    default:
                        sw.WriteLine("Fecha,Hora,Abonado,Oficina,Condicion,ID Senal,Usuario/Descripcion");
                        foreach (T obe in lista)
                        {
                            propiedades = obe.GetType().GetProperties();
                            sw.WriteLine((propiedades[3].GetValue(obe, null) == null ? "" : propiedades[3].GetValue(obe, null).ToString()) + "," + (propiedades[4].GetValue(obe, null) == null ? "" : propiedades[4].GetValue(obe, null).ToString()) + "," + (propiedades[8].GetValue(obe, null) == null ? "" : propiedades[8].GetValue(obe, null).ToString()) + "," + (propiedades[9].GetValue(obe, null) == null ? "" : propiedades[9].GetValue(obe, null).ToString()) + "," + (propiedades[6].GetValue(obe, null) == null ? "" : propiedades[6].GetValue(obe, null).ToString()) + "," + (propiedades[5].GetValue(obe, null) == null ? "" : propiedades[5].GetValue(obe, null).ToString()) + "," + (propiedades[7].GetValue(obe, null) == null ? "" : propiedades[7].GetValue(obe, null).ToString()));
                        }
                        break;
                }
            }

            /*using (StreamWriter sw = new StreamWriter(String.Format("{0}//{1}",Path.GetDirectoryName(archivo),"schema.ini")))
            {
                sw.WriteLine("[" + Path.GetFileName(archivo) + "]\nColNameHeader=True");
            }*/
        }

        private void exportarExcel(string archivoTxt)
        {
            string ruta = Path.GetDirectoryName(archivoTxt);
            string nombre = Path.GetFileNameWithoutExtension(archivoTxt);
            string archivoXlsx = String.Format("{0}\\{1}.xlsx", ruta, nombre);
            using (OleDbConnection con = new OleDbConnection("provider=Microsoft.Ace.oledb.12.0;data source=" + ruta + ";extended properties=Text;"))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select * Into Hoja1 In ''[Excel 12.0 xml;Database=" + archivoXlsx + "]From " + nombre + "#TXT", con);
                cmd.ExecuteNonQuery();
            }
        }

        //Monitoreo
        public ActionResult Monitoreo()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
            //if (acceso == 0) return Redirect("~/Login/Login");
            //else
            //{
                brMonitoreo br = new brMonitoreo();
                List<beAlarmasResolution> lstResolution = br.ListarResolution();
                ViewBag.ListaResolution = lstResolution;
                if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "2" || int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())) > 4) ViewBag.Perfil = 1;
                else if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "3" || Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "1") ViewBag.Perfil = 2;
                return View();
            //}
        }

        public ActionResult PopupMonitoreo()
        {
            brMonitoreo br = new brMonitoreo();
            int pendiente = -1;
            if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "2" || int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())) > 4)
            {
                beMonitoreo obeMonitoreo = br.MensajeAlarma(Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), short.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));
                if (obeMonitoreo != null & obeMonitoreo.obeSenal != null)
                {
                    if (obeMonitoreo.obeSenal.Usuario == Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper())) pendiente = 1;
                    else pendiente = 0;
                    Session["Monitoreo"] = obeMonitoreo.lstEmergencia;
                    //return Json(new { Perfil = Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()), Pend = pendiente, Data = obeMonitoreo.obeSenal, Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.Parse(x.horarequerimiento)) });
                    return Json(new { Perfil = Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()), Pend = pendiente, Data = obeMonitoreo.obeSenal, Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)) });
                }
            }
            else if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "3" || Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "1")
            {
                List<beSenal> lstSenal = br.ListarAlarmasPendientes(Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), short.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));
                if (lstSenal != null)
                {
                    if (lstSenal.Count > 0)
                    {
                        if (lstSenal[0].Usuario == Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper())) pendiente = 1;
                        else pendiente = 0;
                        int pag = 0;
                        if (lstSenal.Count > 1)
                            if (TempData["indiceActualPagina"] != null) pag = (int)TempData["indiceActualPagina"];
                        lstSenal = Paginar(null, lstSenal);
                        indiceActualPagina = pag;
                        TempData["indiceActualPagina"] = pag;
                    }
                    return Json(new { Perfil = Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()), Pend = pendiente, Data = mostrarPagina(lstSenal), current = indiceActualPagina, last = indiceUltimaPagina });
                }
            }
            return Json(new { Perfil = Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()), Pend = pendiente });
        }

        public ActionResult BusquedaAlarmas(string Pagina)
        {
            List<beSenal> lbeSenal = null;
            lbeSenal = Paginar(Pagina, lbeSenal);
            return Json(new { Data = mostrarPagina(lbeSenal), current = indiceActualPagina, last = indiceUltimaPagina });
        }

        public ActionResult DataPopupSenal(int AlarmHistoryID)
        {
            brMonitoreo br = new brMonitoreo();
            beMonitoreo obeMonitoreo = br.SenalExtraData(AlarmHistoryID);
            Session["Monitoreo"] = obeMonitoreo.lstEmergencia;
            //return Json(new { Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.Parse(x.horarequerimiento)) });//DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
            return Json(new { Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)) });//DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
        }

        public ActionResult SenalHistoricoEvento(int AlarmHistoryID)
        {
            brMonitoreo br = new brMonitoreo();
            List<beEvento> lstEvento = br.SenalHistoricoEvento(AlarmHistoryID);
            return Json(lstEvento);
        }

        public ActionResult AsignarAlarma(int AlarmHistoryID,string user)
        {
            brMonitoreo br = new brMonitoreo();
            string men = "";
            int updated = br.AsignarAlarma(AlarmHistoryID, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));
            //int updated = br.AsignarAlarma(AlarmHistoryID, user, int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));
            if (updated == -1) men = "Hubo un problema en la red y no se pudo conectar, se intentará de nuevo";
            else if (updated == 0) men = "La alarma ya fue asignada a otro usuario, se buscará otra alarma";
            TempData["indiceActualPagina"] = null;
            return Json(men);
        }

        public ActionResult ConsultarEmergencia(int pidcategoria, string Pagina)
        {
            List<beEmergencia> lstConsultaremergencia = null;
            if (Pagina == null) lstConsultaremergencia = ((List<beEmergencia>)Session["Monitoreo"]).FindAll(x => x.IdCategoria == pidcategoria);
            return Json(lstConsultaremergencia);
        }

        public ActionResult AgregarComentario(int AlarmHistoryID, string HoraRequerimiento, string Comentario)
        {
            brMonitoreo br = new brMonitoreo();
            string men = "";
            bool updated = br.AgregarComentario(AlarmHistoryID, DateTime.ParseExact(HoraRequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Comentario.ToUpper(), Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            if (!updated) men = "La alarma ha sido escalada, se volverá a buscar una alarma sin asignar";
            return Json(men);
        }

        public ActionResult ComboEntidad(int palarmhistoryid, int pidcategoria, int pidentidad)
        {
            brMonitoreo br = new brMonitoreo();
            List<beEntidad> lstEntidad = br.ComboEntidad(palarmhistoryid, pidcategoria, pidentidad);
            return Json(lstEntidad);
        }

        public ActionResult ComboMotivo()
        {
            brMonitoreo br = new brMonitoreo();
            List<beMotivo> lstMotivo = br.ComboMotivo();
            return Json(lstMotivo);
        }

        //public ActionResult ComboLocal_Subscriber()
        //{
        //    brLocal_Subscriber br = new brLocal_Subscriber();
        //    List<beLocal_subscriber> lstLocal_subscriber = br.ComboLocal_subscriber();
        //    return Json(lstLocal_subscriber);
        //}

        //public ActionResult ComboTipoParticion()
        //{
        //    brTipoParticion br = new brTipoParticion();
        //    List<beTipoParticion> lstTipo_Particion = br.ComboTipoParticion();
        //    return Json(lstTipo_Particion);
        //}

        public string ComboLocal_Subscriber()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brLocal_Subscriber br = new brLocal_Subscriber();
                List<beLocal_subscriber> lstlocal_Subscriber = br.ComboLocal_subscriber();
                lstlocal_Subscriber.Insert(0, new beLocal_subscriber { Localid = "", Localdes = "TODOS" });
                if (lstlocal_Subscriber != null & lstlocal_Subscriber.Count > 0) rpta = CustomSerializer.Serializar(lstlocal_Subscriber, '@', '_', false).ToString();
            }
            return rpta;
        }

        public ActionResult ListarLocal_Subscriber()
        {
            brLocal_Subscriber br = new brLocal_Subscriber();
            List<beLocal_subscriber> lstlocal_Subscriber = br.ComboLocal_subscriber();
            return Json(lstlocal_Subscriber);
        }

        public ActionResult Buscar_Local_Subscriber(string pLocalid, string pLocaldesc)
        {
            brLocal_Subscriber br = new brLocal_Subscriber();
            List<beLocal_subscriber> BusLocal_Subscriber = br.Buscar_Local_subscriber(pLocalid,pLocaldesc);
            return Json(BusLocal_Subscriber);
        }

        public string ComboTipoParticion()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brTipoParticion br = new brTipoParticion();
                List<beTipoParticion> lstTipoParticion = br.ComboTipoParticion();
                lstTipoParticion.Insert(0, new beTipoParticion { TipoParticionid  = 0, TipoParticiondes= "TODOS" });
                if (lstTipoParticion != null & lstTipoParticion.Count > 0) rpta = CustomSerializer.Serializar(lstTipoParticion, '@', '_', false).ToString();
            }
            return rpta;
        }

        public string ComboTipo_Abonado()
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                brTipo_Abonado br = new brTipo_Abonado();
                List<beTipo_Abonado> lstTipo_Abonado= br.ComboTipo_Abonado();
                lstTipo_Abonado.Insert(0, new beTipo_Abonado { Tipo_Abonado_id = 0, Tipo_Abonado_des = "TODOS" });
                if (lstTipo_Abonado != null & lstTipo_Abonado.Count > 0) rpta = CustomSerializer.Serializar(lstTipo_Abonado, '@', '_', false).ToString();
            }
            return rpta;
        }


        public ActionResult InsertarEditarEmergencia(int Alarmhistoryid, int IdentidadOld, int IdentidadNew, string Horarequerimiento, string Horaarribo, string Nroidenpersontrans, string Nomresponsable)
        {
            bool Exito = false;
            if (IdentidadOld == 0)
            {
                brMonitoreo br = new brMonitoreo();
                Exito = br.InsertarEmergencia(Alarmhistoryid, IdentidadNew, DateTime.ParseExact(Horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Nroidenpersontrans, Nomresponsable, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            }
            else
            {
                brMonitoreo br = new brMonitoreo();
                Exito = br.ActualizarEmergencia(Alarmhistoryid, IdentidadOld, IdentidadNew, DateTime.ParseExact(Horaarribo, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Nroidenpersontrans, Nomresponsable, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            }
            return Json(Exito);
        }

        public ActionResult InsertarEditarEmergenciaStopFraude(int Alarmhistoryid, int IdMotivoOld, int IdMotivoNew, string Horarequerimiento, string Observacion, string Nomresponsable)
        {
            bool Exito = false;
            if (IdMotivoOld == 0)
            {
                brMonitoreo br = new brMonitoreo();
                Exito = br.InsertarEmergenciaStopFraude(Alarmhistoryid, IdMotivoNew, DateTime.ParseExact(Horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Observacion, Nomresponsable, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            }
            else
            {
                brMonitoreo br = new brMonitoreo();
                Exito = br.ActualizarEmergenciaStopFraude(Alarmhistoryid, IdMotivoOld, IdMotivoNew, Observacion, Nomresponsable, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            }
            return Json(Exito);
        }

        public ActionResult EliminarEmergencia(int alarmhistoryid, int identidad)
        {
            bool Exito;
            brMonitoreo br = new brMonitoreo();
            Exito = br.EliminarEmergencia(alarmhistoryid, identidad);
            return Json(Exito);
        }

        public ActionResult EliminarEmergenciaStopFraude(int alarmhistoryid, int idMotivo)
        {
            bool Exito;
            brMonitoreo br = new brMonitoreo();
            Exito = br.EliminarEmergenciaStopFraude(alarmhistoryid, idMotivo);
            return Json(Exito);
        }

        public ActionResult ActualizarDataEmergencia(int palarmhistoryid, int pidcategoria, string Pagina)
        {
            brMonitoreo br = new brMonitoreo();
            Session["Monitoreo"] = br.ConsultarEmergencia(palarmhistoryid, 0);
            return ConsultarEmergencia(pidcategoria, Pagina);
        }

        public ActionResult GestionAlarma(int AlarmHistoryID, int Minutes, string Resolution, int opc)
        {
            brMonitoreo br = new brMonitoreo();
            bool exito = false;
            if (opc == 0) exito = br.SenalPendienteEnEspera(AlarmHistoryID, Minutes);
            else if (opc == 1) exito = br.SenalPendienteCierre(AlarmHistoryID, Resolution);
            else exito = br.SenalAEscalar(AlarmHistoryID);
            return Json(exito);
        }

        public ActionResult ListarUsuarioEnLinea()
        {
            brMonitoreo br = new brMonitoreo();
            List<beUserLogIn> lstUserLogIn = br.ListarUsuariosEnLinea(Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()));
            return Json(lstUserLogIn);
        }

        public ActionResult RelevarAlarmas(string UsuarioAsignado)
        {
            brMonitoreo br = new brMonitoreo();
            bool exito = br.RelevarAlarmas (Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), UsuarioAsignado);
            return Json(exito);
        }

        //Visor
        public ActionResult Visor()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
            //if (acceso == 0) return Redirect("~/Login/Login");
            //else
            //{
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("Visor");                
            //}

            //if (SenalPendienteNoMonitoreo() == 1) return Redirect("~/Modulo/Monitoreo");
            //else return View("Visor");            

        }

        public string GrillaVisor(string DstbCod, string AbndCod, string Evitar, string InOutSide, string Ubigeo, string TipoAbonado, string Localid, string TipoParticionid)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brVisor br = new brVisor();
                List<beVisor> lstVisor = br.ListarVisor(DstbCod, AbndCod, Evitar, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado), Localid, int.Parse(TipoParticionid));
                if (lstVisor != null & lstVisor.Count > 0) rpta = CustomSerializer.Serializar(lstVisor, '@', '_', false).ToString();
            }
            return rpta;
        }

        //Particion Estado

        public ActionResult ParticionEstado()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
            //if (acceso == 0) return Redirect("~/Login/Login");
            //else
           // {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ParticionEstado");
           // }
        }

        //public string GrillaParticion(string idlocal, string Abonado, string TipoAbonado, string TipoParticion, string TipoEventop, string ExcesoTiempo, string Ubicacion)
        public string GrillaParticionEstado(string LocalId, string csid, string Tipo_Abonado_id, string TipoParticionid, string TipoEvento, string InOutSide, string Ubigeo)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brParticionEstado br = new brParticionEstado();
                List<beParticionEstado> lstParticionEstado = br.ListarParticionEstado(LocalId, csid, Tipo_Abonado_id == "" ? "0" : Tipo_Abonado_id, TipoParticionid == "" ? "0" : TipoParticionid, TipoEvento, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo);
                if (lstParticionEstado != null & lstParticionEstado.Count > 0) rpta = CustomSerializer.Serializar(lstParticionEstado, '@', '_', false).ToString();
            }
            return rpta;
        }


        //Actividad
        public ActionResult ReporteActividades()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
           // if (acceso == 0) return Redirect("~/Login/Login");
           // else
          //  {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ReporteActividades");
          //  }
        }

        public string GrillaActividades(string DstbCod, string AbndCod, string AreaCod, string CtrlFecha, string FechaIni, string HoraIni, string FechaFin, string HoraFin, string CodAlarma, string DesAlarma, string EvitarTest, string EvitarCond, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brActividades br = new brActividades();
                List<beActividad> lstActividad = br.ListarActividad(DstbCod, AbndCod, AreaCod, CodAlarma, DesAlarma, CtrlFecha == "0" ? "" : FechaIni, CtrlFecha == "0" ? "" : FechaFin, CtrlFecha == "0" ? "" : HoraIni, CtrlFecha == "0" ? "" : HoraFin, int.Parse(EvitarTest), int.Parse(EvitarCond), InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstActividad != null & lstActividad.Count > 0) rpta = CustomSerializer.Serializar(lstActividad, '@', '_', false).ToString();
            }
            return rpta;
        }

        //AREA
        public ActionResult ControlArea()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
            //if (acceso == 0) return Redirect("~/Login/Login");
           // else
           // {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ControlArea");
           // }
        }


        public string GrillaControlArea(string DstbCod, string AbndCod, string CondPrimCod, string CondUltCod, string Zona, string Time, string DatosP, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brControlArea br = new brControlArea();
                List<beControlArea> lstControlArea = br.ListarControlArea(DstbCod, AbndCod, CondPrimCod, CondUltCod, Zona, int.Parse(Time), int.Parse(DatosP), InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstControlArea != null & lstControlArea.Count > 0) rpta = CustomSerializer.Serializar(lstControlArea, '@', '_', false).ToString();
            }
            return rpta;
        }

        //AREA 1864
        public ActionResult ControlArea1864()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
         //   if (acceso == 0) return Redirect("~/Login/Login");
         //   else
          //  {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ControlArea1864");
          //  }
        }

        public string GrillaControlArea1864(string DstbCod, string AbndCod, string CondPrimCod, string CondUltCod, string Zona, string Time, string DatosP, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brControlArea br = new brControlArea();
                List<beControlArea> lstControlArea = br.ListarControlArea1864(DstbCod, AbndCod, CondPrimCod, CondUltCod, Zona, int.Parse(Time), int.Parse(DatosP), InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstControlArea != null & lstControlArea.Count > 0) rpta = CustomSerializer.Serializar(lstControlArea, '@', '_', false).ToString();
            }
            return rpta;
        }
        
        //Cierre
        public ActionResult ControlCierre()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
        //    if (acceso == 0) return Redirect("~/Login/Login");
         //   else
         //   {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ControlCierre");
        //    }
        }

        public string GrillaControlCierre(string DstbCod, string AbndCod, string InOutSide, string Ubigeo,
        string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brControlCierre br = new brControlCierre();
                List<beControlSenalSupCaidasFalloTestControlCierre> lstControlCierre = br.ListarControlCierre(DstbCod, AbndCod, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstControlCierre != null & lstControlCierre.Count > 0) rpta = CustomSerializer.Serializar(lstControlCierre, '@', '_', false).ToString();
            }
            return rpta;
        }

        //No Respondidas
        public ActionResult ControlSenal()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
        //    if (acceso == 0) return Redirect("~/Login/Login");
        //    else
         //   {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("ControlSenal");
         //   }
        }

        public string GrillaControlSenal(string DstbCod, string AbndCod, string Time, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brControlSenal br = new brControlSenal();
                List<beControlSenalSupCaidasFalloTestControlCierre> lstControlSenal = br.ListarControlSenal(DstbCod, AbndCod, Time, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstControlSenal != null & lstControlSenal.Count > 0) rpta = CustomSerializer.Serializar(lstControlSenal, '@', '_', false).ToString();
            }
            return rpta;
        }

        //Fallo Test
        public ActionResult FalloTest()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
        //    if (acceso == 0) return Redirect("~/Login/Login");
         //   else
        //    {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("FalloTest");
         //   }
        }

        public string GrillaFalloTest(string DstbCod, string AbndCod, string Time, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brFalloTest br = new brFalloTest();
                List<beControlSenalSupCaidasFalloTestControlCierre> lstFalloTest = br.ListarFalloTest(DstbCod, AbndCod, Time, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstFalloTest != null & lstFalloTest.Count > 0) rpta = CustomSerializer.Serializar(lstFalloTest, '@', '_', false).ToString();
            }
            return rpta;
        }

        //Supervisión Caídas
        public ActionResult SupervisionCaidas()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
       //     if (acceso == 0) return Redirect("~/Login/Login");
        //    else
      //      {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("SupervisionCaidas");
      //      }
        }

        public string GrillaSupervisionCaidas(string DstbCod, string AbndCod, string Cond, string Opc, string Time, string InOutSide, string Ubigeo, string TipoAbonado, string Localid, string TipoParticionid)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brSupervisionCaidas br = new brSupervisionCaidas();
                List<beControlSenalSupCaidasFalloTestControlCierre> lstSupervisionCaidas = br.ListarSupervisionCaidas(DstbCod.ToUpper(), AbndCod.ToUpper(), Cond, Opc, Time, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado), Localid, int.Parse(TipoParticionid));
                if (lstSupervisionCaidas != null & lstSupervisionCaidas.Count > 0) rpta = CustomSerializer.Serializar(lstSupervisionCaidas, '@', '_', false).ToString();
            }
            return rpta;
        }
       

        //Reporte Estadístico
        public ActionResult EstadisticoAlarmas()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
       //     if (acceso == 0) return Redirect("~/Login/Login");
        //    else
        //    {
                if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
                else return View("EstadisticoAlarmas");
         //   }
        }

        public string GrillaEstadisticoAlarmas(string DstbCod, string AbndCod, string CtrlFecha, string FechaIni, string HoraIni, string FechaFin, string HoraFin, string CodAlarma, string InOutSide, string Ubigeo, string TipoAbonado)
        {
            string rpta = "";
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                Session["validar"] = 1;
                brEstadisticioAlarmas br = new brEstadisticioAlarmas();
                List<beEstadisticoAlarmas> lstEstadisticoAlarmas = br.ListarEstadisticoAlarmas(DstbCod, AbndCod, CodAlarma, CtrlFecha == "0" ? "" : FechaIni, CtrlFecha == "0" ? "" : HoraIni, CtrlFecha == "0" ? "" : FechaFin, CtrlFecha == "0" ? "" : HoraFin, InOutSide == "≠" ? "-" + Ubigeo : Ubigeo, int.Parse(TipoAbonado));
                if (lstEstadisticoAlarmas != null & lstEstadisticoAlarmas.Count > 0) rpta = CustomSerializer.Serializar(lstEstadisticoAlarmas, '@', '_', false).ToString();
            }
            return rpta;
        }

        //Evento Administrativo

        //public ActionResult EventoAdministrativo()
        //{
        //    if (Session["validar"] != null)
        //    {
        //        acceso = int.Parse(Session["validar"].ToString());
        //        Session["validar"] = acceso;
        //    }
        //    if (acceso == 0) return Redirect("~/Login/Login");
        //    else
        //    {
        //        if (SenalPendienteNoMonitoreo() > 0) return Redirect("~/Modulo/Monitoreo");
        //        else return View("EventoAdministrativo");
        //    }
        //}

        public ActionResult EventoAdministrativo()
        {
            if (Session["validar"] != null)
            {
                acceso = int.Parse(Session["validar"].ToString());
                Session["validar"] = acceso;
            }
            //if (acceso == 0) return Redirect("~/Login/Login");
            //else
            //{


                brMonitoreo br = new brMonitoreo();
                List<beAlarmasResolution> lstResolution = br.ListarResolution();
                ViewBag.ListaResolution = lstResolution;
                
                if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "2" || int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())) > 4) ViewBag.Perfil = 1;
                else if (Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "3" || Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()) == "1") ViewBag.Perfil = 2;
                return View();
            //}
        }

        public ActionResult EventoAdministrativoup(string alarmcode, string csid, string Physicalzone)
        {
            brEventoAdministrativo br = new brEventoAdministrativo();
            string outIdAlarmhistory="";
            string outDateTimeOccurred="";
            string men = "";            
            //int updated = br.AsignarAlarma(AlarmHistoryID, Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), int.Parse(Server.HtmlEncode(Request.Cookies["PU"].Value.ToString())));            
            bool updated = br.RegistrarAlarma(alarmcode, csid, Physicalzone,Server.HtmlEncode(Request.Cookies["Usuario"].Value.ToString().ToUpper()), out outIdAlarmhistory, out outDateTimeOccurred);
            if (updated == false) 
                men = "Hubo un problema al momento de intentar registrar la alarma";                
            else
            {
                men = "La alarma se registro correctamente";
            }
            
         //   return Json(men);
            brMonitoreo brm = new brMonitoreo();
            beMonitoreo obeMonitoreo = brm.SenalExtraData(Convert.ToInt32(outIdAlarmhistory));
            Session["Monitoreo"] = obeMonitoreo.lstEmergencia;
            //return Json(new { codigo = outIdAlarmhistory, fecha = outDateTimeOccurred, msm = men, Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.Parse(x.horarequerimiento)) }); //DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))

            return Json(new { codigo = outIdAlarmhistory, fecha = outDateTimeOccurred, msm = men, Data2 = obeMonitoreo.lstContactoLocacion, Data3 = obeMonitoreo.lstContactoSeguridad, Data4 = obeMonitoreo.lstEmergencia.FindAll(x => x.nomresponsable == "").OrderByDescending(x => DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)) }); //DateTime.ParseExact(x.horarequerimiento, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))

            //return Json(new { codigo = outIdAlarmhistory, fecha=outDateTimeOccurred, msm=men});
            //return Json(new { Perfil = Server.HtmlEncode(Request.Cookies["PU"].Value.ToString()), Pend = pendiente });
        }

        public ActionResult ListarAlarmAdm()
        {
            brAlarmconditions br = new brAlarmconditions();
            List<beAlarmConditions> lstAlarmAdm= br.ListarAlarmAdm();
            return Json(lstAlarmAdm);
        }

        public ActionResult ListarSubscriber(string csid, string subscribername)
        {
            brSubscriber br = new brSubscriber();
            List<beSubscriber> lstSubscriber = br.ListarSusbcriber(csid,subscribername);
            return Json(lstSubscriber);
        }

    }
}