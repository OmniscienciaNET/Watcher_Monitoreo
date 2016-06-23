using BusinessRules;
using Entity;
using System;
using System.Web;
using System.Web.Mvc;
using General.Librerias.CodigoUsuario;

namespace Watcher.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public string ValidarAcceso()
        {
            string rpta = "";            
            string cabecera = Request.Headers["xhr"];
            if (cabecera != null && cabecera.Equals("OSC"))
            {
                string user = Request.QueryString["usuario"];
                string pass = Request.QueryString["pass"];
                string dstb = Request.QueryString["dstb"];
                string chk = Request.QueryString["rmbr"];
                brLogin obrLogin = new brLogin();
                beLogin lstLogin = obrLogin.ValidarLogin(user.ToLower(), pass, dstb.ToLower());
                if (lstLogin != null)
                {
                    if (lstLogin.exito != 0)
                    {
                        DateTime expira = DateTime.Now.AddMonths(1);
                        HttpCookie usuario = new HttpCookie("Usuario", user);
                        usuario.Expires = expira;
                        Response.Cookies.Add(usuario);
                        HttpCookie pu = new HttpCookie("PU", lstLogin.exito.ToString());
                        pu.Expires = expira;
                        Response.Cookies.Add(pu);
                        if (chk == "1")
                        {
                            HttpCookie rmbr = new HttpCookie("Chk", "1");
                            rmbr.Expires = expira;
                            Response.Cookies.Add(rmbr);
                        }

                        rpta = CustomSerializer.Serializar(lstLogin.lstDstAbnd, '@', '_', false).ToString();                        
                        Session["validar"] = 1;
                    }
                    else                    
                        rpta = "0";
                }
            }
            return rpta;
        }
	}
}