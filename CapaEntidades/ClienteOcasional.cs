using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ClienteOcasional
    {

        public string DOMICILIO { get; set; }//

        public string E_MAIL { get; set; }//

        public string IVA_D { get; set; }//S

        public string IVA_L { get; set; }//S

        public string N_COMP { get; set; } //

        public string N_CUIT { get; set; }

        public string RAZON_SOCI { get; set; }//

        public int TALONARIO { get; set; }//

        public string TELEFONO_1 { get; set; }//

        public int TIPO_DOC { get; set; }// 96

        public string T_COMP { get; set; } // PED

        public string DIRECCION_ENTREGA { get; set; }//

        public string TELEFONO1_ENTREGA { get; set; }//

        public int ID_CATEGORIA_IVA { get; set; } // CONSUMIDOR FINAL ID CATEGORIA IVA

        public string MAIL_DE { get; set; }//

        public DateTime FECHA_NACIMIENTO { get; set; }

        public string SEXO { get; set; }
    }
}
