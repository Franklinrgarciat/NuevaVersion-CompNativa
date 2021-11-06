using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Pedido
    {

        public string Numero { get; set; }

        public string Numero2 { get; set; }

        public string Cliente { get; set; }

        public string RazonSocial { get; set; }

        public string Domicilio { get; set; }

        public DateTime Fecha { get; set; }

        public string CondicionVta { get; set; }

        public string Vendedor { get; set; }

        public string EstadoPedido { get; set; }

        public string Deposito { get; set; }

        public string ListaPrecio { get; set; }

        public string Transporte { get; set; }

        public string Talonario { get; set; }

        public DateTime FechaEntrega { get; set; }

        public decimal Cotiz { get; set; }

        public decimal Total { get; set; }

        public decimal Total2 { get; set; }

        public decimal Bonif { get; set; }

        public int TalonarioPedido { get; set; }

        public int TalonarioPedido2 { get; set; }

        public int IdDirEntrega { get; set; }

        public bool Base2 { get; set; }

        public bool Base1 { get; set; }

        public string Comentario { get; set; }
        public string Leyenda1 { get; set; }

        public string Leyenda2 { get; set; }

        public string Leyenda3 { get; set; }

        public string Leyenda4 { get; set; }

        public string Leyenda5 { get; set; }

        public string ImporteSeña { get; set; }
        public static string sTipoPrecio { get; set; }
        public static string Empresa { get; set; }
        public static bool PedidoClienteOcasional { get; set; }

        public static bool PedidoClienteRegistradoEnTango { get; set; }

        public List<DetallePedido> Detalle { get; set; }
        public ClienteOcasional ClienteOcasional { get; set; }
        public Pedido()
        {
            this.Detalle = new List<DetallePedido>();
            this.ClienteOcasional = new ClienteOcasional();
        }
    }
}
