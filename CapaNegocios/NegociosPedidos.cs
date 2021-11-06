using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using CapaDatos;
using CapaEntidades;
using CapaNegocios;
using Globales;

namespace CapaNegocios
{
    public class NegociosPedidos
    {
        CapaDatos.GuardoEnBaseDeDatos dbPedido = new GuardoEnBaseDeDatos();
        public int Talonario { get; set; }
        public int CantidadPedidos
        {
            get
            {
                return dbPedido.Registros;
            }
        }
        public void primero()
        {
            dbPedido.primero();
            CargarDatos();
        }
        public void Anterior()
        {
            dbPedido.Anterior();
            CargarDatos();
        }
        public void Siguiente()
        {
            dbPedido.Siguiente();
            CargarDatos();
        }
        public void Ultimo()
        {
            dbPedido.Ultimo();
            CargarDatos();
        }
        public void BuscarPedido(string sNumeroPedido, int iTalonario)
        {
            dbPedido.BuscarPedido(sNumeroPedido, iTalonario);
            CargarDatos();
        }
        public bool Conectar(string sConexString, string sConexString2)
        {
            dbPedido.Talonario = Talonario;
            return dbPedido.Conectar(sConexString, sConexString2);
        }

        #region "VALIDACIONES"

        public bool ComprobarDato(string sTabla, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            return dbPedido.ComprobarDato(sTabla, sCampoABuscar, bTexto, sValorAComparar);
        }
        public bool ValidarCliente(string sCliente)
        {
            if (sCliente == "000000")
            {
                return true;
            }
            else
            {
                return dbPedido.ComprobarDato("GVA14", "COD_CLIENT", true, sCliente);
            }
        }
        public bool ValidarCondicionVta(string sCondicionVenta)
        {
            return dbPedido.ComprobarDato("GVA01", "COND_VTA", true, sCondicionVenta);
        }
        public bool ValidarDeposito(string sDeposito)
        {
            return dbPedido.ComprobarDato("STA22", "COD_SUCURS", true, sDeposito);
        }
        public bool ValidarTalonario(string sTalonario)
        {
            return dbPedido.ComprobarDato("GVA43", "TALONARIO", true, sTalonario);
        }
        public bool ValidarTransporte(string sTransporte)
        {
            if (sTransporte == "")
            {
                return true;
            }
            else
            {
                return dbPedido.ComprobarDato("GVA24", "COD_TRANSP", true, sTransporte);
            }

        }
        public bool ValidarVendedor(string sVendedor)
        {
            return dbPedido.ComprobarDato("GVA23", "COD_VENDED", true, sVendedor);
        }

        public bool ValidarArticulosGrilla(List<CapaEntidades.DetallePedido> oDetalle)
        {
            bool bValido = true;
            for (int x = 0; x <= oDetalle.Count - 1; x++)
            {
                if (oDetalle[x].Codigo != "")
                {
                    if (dbPedido.ComprobarDato("STA11", "COD_ARTICU", true, oDetalle[x].Codigo) == false)
                        bValido = false;
                }
            }
            return bValido;
        }

        public bool ValidarArticulosRepetidosGrilla(List<CapaEntidades.DetallePedido> oDetalle)
        {
            bool bValido = true;
            //var du = oDetalle.GroupBy(x => new { x.Codigo, x.Deposito }).ToList();
            //var du = oDetalle.Select(x => oDetalle.Where(y => x.Codigo == y.Codigo && x.Deposito == y.Deposito).Count() > 1).Distinct().ToList();
            var duplicados = oDetalle.Where(x => oDetalle.Where(y => x.Codigo == y.Codigo && x.Deposito == y.Deposito).Count() > 1).Distinct().ToList();
            if (duplicados.Count > 0)
                bValido = false;
            return bValido;
        }

        public bool ValidarLongitudDescripciones(List<CapaEntidades.DetallePedido> oDetalle)
        {
            bool bValido = true;
            for (int x = 0; x <= oDetalle.Count - 1; x++)
            {
                if (oDetalle[x].Descripcion.Length > 50)
                    bValido = false;
            }
            return bValido;
        }

        public bool ValidarPreciosGrilla(List<CapaEntidades.DetallePedido> oDetalle)
        {
            bool bValido = true;
            for (int x = 0; x <= oDetalle.Count - 1; x++)
            {
                if (oDetalle[x].PrecioV <= 0)
                    bValido = false;
            }
            return bValido;
        }

        public string TraerDato(bool bBase, string sTabla, string sCampoAtraer, bool bTextoCampoAtraer, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            return dbPedido.TraerDato(bBase, sTabla, sCampoAtraer, bTextoCampoAtraer, sCampoABuscar, bTexto, sValorAComparar);
        }
        #endregion

        public decimal ActualizarTotales(Pedido oPedido, List<DetallePedido> detallePedido)
        {
            decimal totalFinal = 0;
            try
            {

                foreach (var item in detallePedido)
                {
                    //valorIva = TraerIva(item.Codigo);
                    //totalProducto = (100 - oPedido.Bonif) / 100 * item.Total;
                    //decimal TotalDeImpuestos = totalProducto * valorIva / 100;
                    totalFinal += Convert.ToDecimal(item.Total.ToString("N2"));
                }
                return totalFinal;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool AgregarPedidoBase1(CapaEntidades.Pedido oPedido)
        {
            try
            {
                List<DetallePedido> listaDetallePedido = new List<DetallePedido>();
                List<string> listaDepositos = oPedido.Detalle.Select(x => x.Deposito).Distinct().ToList();
                string totalGeneralPedido = oPedido.Total.ToString("N2");
                string primerPedido = "";
                string segundoPedido = "";
                string tercerPedido = "";

                int pedido = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (var deposito in listaDepositos)
                    {
                        pedido++;
                        if (listaDepositos.Count >= 2)
                        {
                            switch (pedido)
                            {
                                case 1:
                                    primerPedido = oPedido.Numero;
                                    break;
                                case 2:
                                    segundoPedido = oPedido.Numero;
                                    break;
                                case 3:
                                    tercerPedido = oPedido.Numero;
                                    break;
                            }
                            listaDetallePedido = oPedido.Detalle.Where(x => x.Deposito == deposito).ToList();
                            oPedido.Total = ActualizarTotales(oPedido, listaDetallePedido);
                            dbPedido.AgregarABaseEmpresa1(oPedido, listaDetallePedido);
                            ActualizarProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                            if (pedido < 3)
                            {
                                oPedido.Numero = dbPedido.TraerProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario  
                            }    
                        }
                        else
                        {
                            //listaDetallePedido = oPedido.Detalle.Where(x => x.Deposito == deposito).ToList();
                            primerPedido = oPedido.Numero;
                            //oPedido.Total = ActualizarTotales(oPedido, listaDetallePedido);
                            dbPedido.AgregarABaseEmpresa1(oPedido, oPedido.Detalle);
                            ActualizarProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                        }
                    }
                    if (Pedido.PedidoClienteOcasional)
                    {
                        oPedido.ClienteOcasional.N_COMP = oPedido.Numero;
                        oPedido.ClienteOcasional.TALONARIO = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"));
                        dbPedido.GuardarClienteOcasional(true, oPedido.ClienteOcasional);
                        Pedido.PedidoClienteOcasional = false;
                    }
                    dbPedido.IngresarTablaPropiaDeReferencia(true, primerPedido, segundoPedido, tercerPedido, totalGeneralPedido);
                    scope.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool AgregarPedidoBase2(CapaEntidades.Pedido oPedido)
        {
            try
            {
                List<DetallePedido> listaDetallePedido = new List<DetallePedido>();
                List<string> listaDepositos = oPedido.Detalle.Select(x => x.Deposito).Distinct().ToList();
                string totalGeneralPedido = oPedido.Total.ToString("N2");
                string primerPedido = "";
                string segundoPedido = "";
                string tercerPedido = "";
                int pedido = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (var deposito in listaDepositos)
                    {
                        pedido++;
                        if (listaDepositos.Count >= 2)
                        {
                            switch (pedido)
                            {
                                case 1:
                                    primerPedido = oPedido.Numero;
                                    break;
                                case 2:
                                    segundoPedido = oPedido.Numero;
                                    break;
                                case 3:
                                    tercerPedido = oPedido.Numero;
                                    break;
                            }

                            listaDetallePedido = oPedido.Detalle.Where(x => x.Deposito == deposito).ToList();

                            oPedido.Total = ActualizarTotales(oPedido, listaDetallePedido);
                            dbPedido.AgregarABaseEmpresa2(oPedido, listaDetallePedido);
                            dbPedido.AgregarABaseEmpresa1(oPedido, listaDetallePedido);

                            ActualizarProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                            if (pedido < 3)
                            {
                                oPedido.Numero = dbPedido.TraerProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                            }
                        }
                        else
                        {
                            //listaDetallePedido = oPedido.Detalle.Where(x => x.Deposito == deposito).ToList();

                            //oPedido.Total = ActualizarTotales(oPedido, listaDetallePedido);
                            dbPedido.AgregarABaseEmpresa2(oPedido, oPedido.Detalle);
                            dbPedido.AgregarABaseEmpresa1(oPedido, oPedido.Detalle);
                            ActualizarProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                            //oPedido.Numero = dbPedido.TraerProximoNumeroDePedido(ConfigurationManager.AppSettings.Get("Talonario")); //oPedido.Talonario
                        }
                    }
                    if (Pedido.PedidoClienteOcasional)
                    {
                        oPedido.ClienteOcasional.N_COMP = oPedido.Numero;
                        oPedido.ClienteOcasional.TALONARIO = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"));
                        dbPedido.GuardarClienteOcasional(true, oPedido.ClienteOcasional);
                        oPedido.ClienteOcasional.TALONARIO = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario2"));
                        dbPedido.GuardarClienteOcasional(false, oPedido.ClienteOcasional);
                        Pedido.PedidoClienteOcasional = false;
                    }
                    dbPedido.IngresarTablaPropiaDeReferencia(true, primerPedido, segundoPedido, tercerPedido, totalGeneralPedido);
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ActualizarProximoNumeroDePedido(string talonario)
        {
            dbPedido.ActualizarProximoGva43(talonario);
        }

        public List<DetallePedido> TraerDetallesSegundoPedido(string numeroPedido)
        {
            List<DetallePedido> detallesSegundoPedido = new List<DetallePedido>();
            detallesSegundoPedido = dbPedido.CargarDetalle(numeroPedido);
            return detallesSegundoPedido;
        }

        public CapaEntidades.Pedido CargarDatos()
        {
            return dbPedido.CargarDatos();
        }

        public Decimal TraerIva(string sCodigo)
        {
            return dbPedido.TraerIva(sCodigo);
        }

        public string TraerDias(int idDirEntrega)
        {
            return dbPedido.TraerDias(idDirEntrega);
        }

        public DataSet CargarDirecciones(string sCodigoCliente)
        {
            return dbPedido.CargarDirecciones(sCodigoCliente);
        }

        public string TraerUltimoNumeroPedido(string iTalonario)
        {
            try
            {
                string iNumero;

                iNumero = dbPedido.TraerProximoNumeroDePedido(iTalonario);

                return iNumero.PadLeft(13, '0');
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool ModificarPedido(CapaEntidades.Pedido oPedido)
        {
            bool guardo = false;
            using (TransactionScope scope = new TransactionScope())
            {
                if (oPedido.Talonario == ConfigurationManager.AppSettings.Get("TalonarioPedidoCopia1"))
                {
                        dbPedido.ModificarBase(true, oPedido);
                        dbPedido.ModificarBase(false, oPedido);
                        guardo = true;
                }
                else
                {
                        dbPedido.ModificarBase(true, oPedido);
                        guardo = true;
                }
                scope.Complete();
            }

            return guardo;
        }

        public bool AnularPedido(string nroPedido, int talonario)
        {
            try
            {
                bool guardo = false;
                int talonarioCopia = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario2"));

                if (dbPedido.ComprobarExistePedidoEnBase2("GVA21", "TALON_PED", false, talonarioCopia + " AND NRO_PEDIDO='" + nroPedido + "'"))
                {
                    if (dbPedido.AnularPedidoBase2(nroPedido, talonarioCopia))
                    {
                        if (dbPedido.AnularPedido(nroPedido, talonario))
                        {
                            guardo = true;
                        }
                    }
                }
                else
                {
                    if (dbPedido.AnularPedido(nroPedido, talonario))
                    {
                        guardo = true;
                    }
                }
                return guardo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool ReservarPedido(Pedido oPedido)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string Interno = dbPedido.TraerInterno();
                    string n_comp = dbPedido.TraerNumeroComprobante(ConfigurationManager.AppSettings.Get("TalonarioTransferencia"));
                    Interno = Interno.PadLeft(8, '0');
                    int renglon = 1;
                    dbPedido.InsertarEncabezadoReservaStock(oPedido, Interno, n_comp);

                    foreach (var item in oPedido.Detalle)
                    {
                        int id_medida_stock = Convert.ToInt32(TraerDato(false, "STA11", "ID_MEDIDA_STOCK", false, "COD_ARTICU", true, item.Codigo));
                        int id_medida_ventas = Convert.ToInt32(TraerDato(false, "STA11", "ID_MEDIDA_VENTAS", false, "COD_ARTICU", true, item.Codigo));
                        decimal cantidad = item.Cantidad;
                        string codigo = item.Codigo;
                        dbPedido.InsertarDetalleMovimientoReserva(oPedido, Interno, id_medida_stock, id_medida_ventas, cantidad, codigo, renglon);
                        renglon += 2;
                        dbPedido.RestarStockSTA19(cantidad, codigo, oPedido.Deposito);
                    }
                    dbPedido.ActualizarEstadoDePedidoYLeyenda5(oPedido.Numero, Convert.ToInt32(oPedido.TalonarioPedido), Convert.ToInt32(ConfigurationManager.AppSettings.Get("EstadoPedidoReserva")), true);

                    dbPedido.InsertarSiguienteSTA17(Convert.ToInt32(ConfigurationManager.AppSettings.Get("TalonarioTransferencia")));

                    scope.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EliminarReserva(string nroPedido, string talonario)
        {
            try
            {
                bool EliminoConExito = false;
                if (dbPedido.EliminarReservaDePedido(nroPedido))
                {
                    dbPedido.ActualizarEstadoDePedidoYLeyenda5(nroPedido, Convert.ToInt32(talonario), Convert.ToInt32(ConfigurationManager.AppSettings.Get("EstadoPedido")), false);
                    EliminoConExito = true;
                }
                return EliminoConExito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SumarStockSta19(Pedido oPedido)
        {
            try
            {
                bool OperacionConExito = false;
                foreach (var item in oPedido.Detalle)
                {
                    if (dbPedido.SumarStockSTA19(item.Cantidad, item.Codigo, oPedido.Deposito))
                    {
                        OperacionConExito = true;
                    }
                }

                return OperacionConExito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GuardarClienteGva14(ClienteGva14 cliente)
        {
            try
            {
                if (Pedido.Empresa == ConfigurationManager.AppSettings.Get("Empresa1"))
                {
                    dbPedido.GuardarClienteGva14(true, cliente);
                }
                else
                {
                    dbPedido.GuardarClienteGva14(false, cliente);
                    dbPedido.GuardarClienteGva14(true, cliente);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TraerProximoCodigoClienteTango()
        {
            string proximo = dbPedido.TraerProximoCodigoCliente();
            return proximo;
        }

        public List<PedidosTablaPropia> TraerPedidosDeReferencia(string numeroPedido)
        {
            List<PedidosTablaPropia> NumerosPedidosReferencia = new List<PedidosTablaPropia>();
            NumerosPedidosReferencia = dbPedido.TraerPedidosDeReferencia(numeroPedido);
            return NumerosPedidosReferencia;
        }
        public void EditarEstadoPedidoActual(string estado)
        {
            dbPedido.EditarEstadoPedidoActual(estado);
        }
    }
}
