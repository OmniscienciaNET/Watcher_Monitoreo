
using System.Collections.Generic;
namespace Entity
{
    public class beMonitoreo
    {
        public beSenal obeSenal { get; set; }
        public List<beContactoSeguridad> lstContactoSeguridad { get; set; }
        public List<beContactoLocacion> lstContactoLocacion { get; set; }
        public List<beEmergencia> lstEmergencia { get; set; }
        public List<beAlarmasResolution> lstResolution { get; set; }

    }
}
