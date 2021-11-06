using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class DetallePedido
    {

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal Equivalencia { get; set; }

        public decimal Cantidad { get; set; }

        public decimal PrecioL { get; set; }

        public decimal PrecioV { get; set; }

        public decimal Bonif { get; set; }

        public decimal Total { get; set; }

        public string Base { get; set; }

        public int Renglon { get; set; }

        public bool EsAdicional { get; set; }

        public string Deposito { get; set; }

    }
}
