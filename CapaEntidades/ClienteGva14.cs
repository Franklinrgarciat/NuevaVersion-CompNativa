using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ClienteGva14
    {
        public string FILLER { get; set; }

        public string COD_CLIENT { get; set; } //

        public string COD_PROVIN { get; set; }

        public string COD_TRANSP { get; set; }

        public string COD_VENDED { get; set; }

        public string COD_ZONA { get; set; }

        public int COND_VTA { get; set; }

        public string CUIT { get; set; }


        public string DOMICILIO { get; set; }

        public string E_MAIL { get; set; }


        public DateTime FECHA_ALTA { get; set; }


        public string II_D { get; set; }

        public string II_L { get; set; }

        public string IVA_D { get; set; }

        public string IVA_L { get; set; }

        public string LOCALIDAD { get; set; }

        public int NRO_LISTA { get; set; }

        public string RAZON_SOCI { get; set; }

        public string TELEFONO_1 { get; set; }

        public string TIPO { get; set; }

        public int TIPO_DOC { get; set; }

        public string MAIL_DE { get; set; }

        public string COD_GVA14 { get; set; }


        public int ID_CATEGORIA_IVA { get; set; }


        public ClienteDireccionDeEntrega DireccionEntrega { get; set; }
    }
}
