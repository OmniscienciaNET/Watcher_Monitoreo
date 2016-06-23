using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class beLog
    {
        public DateTime FechaHora { get; set; }
        public string Aplicacion { get; set; }
        public string Usuario { get; set; }
        public string NombrePC { get; set; }
        public string MensajeError { get; set; }
        public string DetalleError { get; set; }

        public beLog()
        {
            FechaHora = DateTime.Now;
            Aplicacion = /*Assembly.GetEntryAssembly().GetName().Name*/"";
            Usuario = Environment.UserName;
            NombrePC = Environment.MachineName;
        }
    }
}
