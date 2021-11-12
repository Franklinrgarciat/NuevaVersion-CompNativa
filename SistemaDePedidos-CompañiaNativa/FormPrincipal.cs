using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using CapaEntidades;
using CapaNegocios;
using Nest;
using System.Globalization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SeinBuscador;
using System.Data.SqlClient;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormPrincipal : Form
    {
        private int iTalonario;
        private int iTalonario2;
        private bool bNuevo = false;
        private bool bModificarPedido = false;
        private bool bCancelo;
        private NegociosPedidos oNegociosPedido = new NegociosPedidos();
        Reporte.Reporte oReporte = new Reporte.Reporte();
        private static string Conexion = ConfigurationManager.AppSettings.Get("Conexion").ToString();
        private static string Conexion2 = ConfigurationManager.AppSettings.Get("Conexion").ToString();
        private bool precargado = false;
        private bool bMovioCelda = false;
        public string Edita;
        private bool brenglon;
        public List<decimal> listaDeStock = new List<decimal>();

        public FormPrincipal()
        {
            // CARGO CULTURA

            CultureInfo cultura = new CultureInfo("es-AR");
            cultura.NumberFormat.NumberGroupSeparator = ",";
            cultura.NumberFormat.NumberDecimalSeparator = ".";
            cultura.NumberFormat.CurrencyGroupSeparator = ",";
            cultura.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultura;
            // Llamada necesaria para el diseñador.
            InitializeComponent();

            dgv_Principal.Columns["ColCantidad"].ValueType = typeof(double);
            dgv_Principal.Columns["ColCantidad"].DefaultCellStyle.Format = "N2";
            dgv_Principal.Columns["ColPrecioVenta"].ValueType = typeof(double);

            dgv_Principal.Columns["ColPrecioVenta"].DefaultCellStyle.Format = "N2";

            SqlConnectionStringBuilder sConnA = new SqlConnectionStringBuilder(ConfigurationManager.AppSettings.Get("Conexion"));
            Globales.Global.ServidorA = sConnA.DataSource;
            Globales.Global.BaseA = sConnA.InitialCatalog;
            Globales.Global.usuarioA = sConnA.UserID;
            Globales.Global.passA = sConnA.Password;
        }


        public CultureInfo CargarCultura()
        {
            CultureInfo cultura = new CultureInfo("es-AR");
            cultura.NumberFormat.NumberGroupSeparator = ",";
            cultura.NumberFormat.NumberDecimalSeparator = ".";
            cultura.NumberFormat.CurrencyGroupSeparator = ",";
            cultura.NumberFormat.CurrencyDecimalSeparator = ".";

            cultura.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";

            return cultura;
        }

        public void CargarConexion()
        {
            Conexion = ConfigurationManager.AppSettings.Get("Conexion");
            Conexion2 = ConfigurationManager.AppSettings.Get("Conexion2");
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CargarCultura();
                if (bool.Parse(ConfigurationManager.AppSettings.Get("FILLER2")))
                    btnModificar.Visible = true;
                else
                    btnModificar.Visible = false;

                iTalonario = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"));
                iTalonario2 = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario2"));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                CargarConexion();

                oNegociosPedido.Talonario = iTalonario;
                if (oNegociosPedido.Conectar(Conexion, Conexion2))
                {
                    txtDireccionClienteOcasional.Visible = false;
                    btnSacarReserva.Visible = false;
                    lblPedido1.Visible = false;
                    lblPedido2.Visible = false;
                    lblPedido3.Visible = false;
                    lblprimerPed.Visible = false;
                    lblSegundoPed.Visible = false;
                    lblTercerPed.Visible = false;
                    ParametrizarGrilla();
                    HabilitarControles(false);
                    CargarControles();
                    dgv_Principal.Visible = true;
                    lblEmpresa.Visible = false;
                    txt_empresa.Visible = false;
                    //----------------------------- OCULTAR CONTROLES CLIENTE OCASIONAL-------------------
                    txtNombreClienteOcasional.Visible = false;
                    lblEmailClienteOcasional.Visible = false;
                    txtEmailClienteOcasional.Visible = false;
                    lblTelefonoClienteOcasional.Visible = false;
                    TxtTelefonoClienteOcasional.Visible = false;
                    lblAltaClienteNuevo.Visible = false;
                    txtLeyenda1.ReadOnly = true;
                    txtLeyenda2.ReadOnly = true;
                    txtLeyenda3.ReadOnly = true;
                    txtLeyenda4.ReadOnly = true;
                    txt_Leyenda5.ReadOnly = true;
                    txtImporteSeña.ReadOnly = true;

                }
                else
                {
                    Interaction.MsgBox("No se pudo conectar a la base de datos", MsgBoxStyle.Critical);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #region ----------------------- FUNCIONES GENERALES Y VALIDACIONES ----------------------------------------------------

        public DateTime TraerProximoDiaHabil96hs()
        {
            //int dias = 0;
            try
            {
                DateTime fecha = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss tt"));
                DateTime HoraConfig = Convert.ToDateTime(ConfigurationManager.AppSettings.Get("HoraAverificar").ToString());

                //if (bool.Parse(ConfigurationManager.AppSettings.Get("Plus")))
                //    dias = 3;
                //else
                //    dias = 3;
                DateTime dFecha;
                dFecha = dtpFecha.Value;

                // Para evitar que alguien modifique el config de AM por PM.
                if (Strings.Right(Convert.ToString(HoraConfig), 4) != "a.m.")
                    HoraConfig = Convert.ToDateTime(Convert.ToDateTime("02:00:00 PM").ToString("hh:mm:ss tt"));

                // Entra solo si la fecha del sistema es mayor a las 2 de la tarde
                if (fecha > HoraConfig)
                {
                    bool SDOk = false;
                    while (SDOk == false)
                    {
                        dFecha = dFecha.AddDays(1);
                        if (dFecha.DayOfWeek != DayOfWeek.Saturday)
                            SDOk = true;
                        if (dFecha.DayOfWeek != DayOfWeek.Sunday)
                            SDOk = true;
                    }
                }

                dFecha = TraerFechaNoLaborables(dtpFecha.Value);

                return dFecha;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DateTime TraerFechaNoLaborables(DateTime fechaCarga)
        {
            int contador = 3;
            bool suficiente = false;
            while ((contador >= 0 | suficiente))
            {
                //if (!(fechaCarga.DayOfWeek == DayOfWeek.Saturday | fechaCarga.DayOfWeek == DayOfWeek.Sunday | fijarseFeriados(fechaCarga.Day, fechaCarga.Month, fechaCarga.Year)))
                //{
                contador -= 1;
                if ((contador < 0))
                    break;
                //}
                fechaCarga = fechaCarga.AddDays(1);
            }
            return fechaCarga;
        }

        public DateTime TraerProximoDiaHabilZona(string NombreZona)
        {
            //int dias;
            try
            {
                //if (bool.Parse(ConfigurationManager.AppSettings.Get("Plus")))
                //    dias = 3;
                //else
                //    dias = 3;
                DateTime dFecha;
                dFecha = dtpFecha.Value;

                dFecha = TraerFechaNoLaborables(dtpFecha.Value);

                return dFecha;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private bool ComprobarTalonario()
        {
            bool ban;
            try
            {
                string sIvaD;
                string sIvaL;
                if (txtCliente.Text == "000000" || txtTalonario.Text == "0")
                {
                    sIvaD = oNegociosPedido.TraerDato(false, "GVA14", "IVA_D", true, "COD_CLIENT", true, txtCliente.Text.Trim());
                    sIvaL = oNegociosPedido.TraerDato(false, "GVA14", "IVA_L", true, "COD_CLIENT", true, txtCliente.Text.Trim());
                }
                else
                {
                    sIvaD = oNegociosPedido.TraerDato(false, "GVA14", "IVA_D", true, "COD_CLIENT", true, txtCliente.Text.Trim());
                    sIvaL = oNegociosPedido.TraerDato(false, "GVA14", "IVA_L", true, "COD_CLIENT", true, txtCliente.Text.Trim());
                }

                string sTipo = oNegociosPedido.TraerDato(false, "GVA43", "TIPO", true, "TALONARIO", true, txtTalonario.Text.Trim());

                // Verificar que el talonario exista
                if (oNegociosPedido.TraerDato(false, "GVA43", "TIPO", true, "TALONARIO", true, txtTalonario.Text.Trim()) != "-")
                {
                    // varificar el if para sabner si correspoinde o no al cliente
                    if ((sIvaD == "S" & sIvaL == "S" & sTipo == "A") | (sIvaD == "N" & sIvaL == "S" & sTipo == "B") | (sIvaD == "N" & sIvaL == "N" & (sTipo == "E")))
                        ban = true;
                    else if (Interaction.MsgBox("El talonario no corresponde al cliente ¿Desea corregir?", Constants.vbYesNo) == Constants.vbYes)
                        ban = false;
                    else
                        ban = true;
                }
                else
                {
                    Interaction.MsgBox("El talonario ingresado es incorrecto.", Constants.vbInformation);
                    ban = false;
                }
                return ban;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void ParametrizarGrilla()
        {
            dgv_Principal.Columns["ColCantidad"].ValueType = typeof(double);
            dgv_Principal.Columns["ColPrecioLista"].ValueType = typeof(double);
            dgv_Principal.Columns["ColPrecioVenta"].ValueType = typeof(double);
        }

        private void CargarDatos(string sCodCliente)
        {
            try
            {
                // CONDICION VENTA
                txtCondicionVenta.Text = oNegociosPedido.TraerDato(false, "GVA14", "COND_VTA", false, "COD_CLIENT", true, sCodCliente);
                lblCondicionVenta.Text = oNegociosPedido.TraerDato(false, "GVA01", "DESC_COND", true, "COND_VTA", false, txtCondicionVenta.Text.Trim());


                // TRANSPORTE
                txtTransporte.Text = oNegociosPedido.TraerDato(false, "GVA14", "COD_TRANSP", false, "COD_CLIENT", true, sCodCliente);
                lblTransporte.Text = oNegociosPedido.TraerDato(false, "GVA24", "NOMBRE_TRA", true, "COD_TRANSP", true, txtTransporte.Text.Trim());

                // VENDEDOR
                //txtVendedor.Text = oNegociosPedido.TraerDato(false, "GVA14", "COD_VENDED", false, "COD_CLIENT", true, sCodCliente);
                //lblVendedor.Text = oNegociosPedido.TraerDato(false, "GVA23", "NOMBRE_VEN", true, "COD_VENDED", false, txtVendedor.Text.Trim());

                // BONIFICACION

                txtBonif.Text = oNegociosPedido.TraerDato(false, "GVA14", "PORC_DESC", false, "COD_CLIENT", true, sCodCliente);
                txtBonif.Text = Convert.ToString(txtBonif.Text);

                // DIRECCION
                cmbDirEntrega.DataSource = oNegociosPedido.CargarDirecciones(txtCliente.Text).Tables[0];
                cmbDirEntrega.ValueMember = "ID_DIRECCION_ENTREGA";
                cmbDirEntrega.DisplayMember = "Direccion";

                cmbDirEntrega.SelectedIndex = 0;

                // ITEMS PRECARGADOS
                if (!precargado)
                {
                    precargado = true;
                    dgv_Principal.AllowUserToAddRows = true;
                    dgv_Principal.AllowUserToDeleteRows = true;
                }



                // Recorro del las zonas que se encuentra en el INI 
                //string CodZonaCliente = oNegociosPedido.TraerDato(false, "GVA14", "COD_ZONA", false, "COD_CLIENT", true, sCodCliente);
                //string NombreZona = oNegociosPedido.TraerDato(false, "GVA05", "NOMBRE_ZON", false, "COD_ZONA", true, CodZonaCliente);
                ////var Estado = false;
                //string value = ConfigurationManager.AppSettings.Get("Zonas");
                //char delimiter = ',';
                //string[] substrings = value.Split(delimiter);
                //foreach (string Zonasp in substrings)
                //{
                //    if (NombreZona.Trim() == Zonasp.Trim())
                //        Estado = true;
                //}

                //if (Estado == true)
                //{
                //    if (NombreZona.Trim() == "NORTE")
                //        dtpFechaEntrega.Value = TraerProximoDiaHabilZona(NombreZona.Trim());
                //    else if (NombreZona.Trim() == "SUR")
                //        dtpFechaEntrega.Value = TraerProximoDiaHabilZona(NombreZona.Trim());
                //    else if (NombreZona.Trim() == "OESTE")
                //        dtpFechaEntrega.Value = TraerProximoDiaHabilZona(NombreZona.Trim());
                //}
                //else
                //    dtpFechaEntrega.Value = TraerProximoDiaHabil96hs();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ValidarFormatoEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public Pedido LinkearControles(bool esPedidoNuevo)
        {
            dgv_Principal.EndEdit();
            CapaEntidades.Pedido oPedidoNuevo = new CapaEntidades.Pedido();
            if (Pedido.PedidoClienteOcasional == false)
            {
                oPedidoNuevo.Cliente = txtCliente.Text;
                oPedidoNuevo.CondicionVta = txtCondicionVenta.Text;
                oPedidoNuevo.Deposito = txtDeposito.Text;
                oPedidoNuevo.Fecha = dtpFecha.Value.Date;
                oPedidoNuevo.FechaEntrega = dtpFechaEntrega.Value.Date;
                oPedidoNuevo.Talonario = txtTalonario.Text;
                oPedidoNuevo.Transporte = txtTransporte.Text;
                oPedidoNuevo.Vendedor = txtVendedor.Text;
                oPedidoNuevo.EstadoPedido = ConfigurationManager.AppSettings.Get("EstadoPedido");
                oPedidoNuevo.Numero = txtPedido.Text;
                oPedidoNuevo.Numero2 = txtPedido.Text;
                oPedidoNuevo.Bonif = Convert.ToDecimal(txtBonif.Text);
                oPedidoNuevo.IdDirEntrega = Convert.ToInt32(cmbDirEntrega.SelectedValue);
                oPedidoNuevo.Cotiz = Convert.ToDecimal(lblCotizacion.Text);
                oPedidoNuevo.Leyenda1 = txtLeyenda1.Text;
                oPedidoNuevo.Leyenda2 = txtLeyenda2.Text;
                oPedidoNuevo.Leyenda3 = txtLeyenda3.Text;
                oPedidoNuevo.Leyenda4 = txtLeyenda4.Text;
                if (txtImporteSeña.Text == "")
                {
                    txtImporteSeña.Text = "0";
                }
                oPedidoNuevo.ImporteSeña = txtImporteSeña.Text;
                
            }
            else
            {
                oPedidoNuevo.Cliente = "000000";
                oPedidoNuevo.CondicionVta = txtCondicionVenta.Text;
                oPedidoNuevo.Deposito = txtDeposito.Text;
                oPedidoNuevo.Fecha = dtpFecha.Value.Date;
                oPedidoNuevo.FechaEntrega = dtpFechaEntrega.Value.Date;
                oPedidoNuevo.FechaEntrega = dtpFechaEntrega.Value.Date;
                oPedidoNuevo.Talonario = txtTalonario.Text;
                oPedidoNuevo.Transporte = "1";
                oPedidoNuevo.Vendedor = txtVendedor.Text;
                oPedidoNuevo.EstadoPedido = ConfigurationManager.AppSettings.Get("EstadoPedido");
                oPedidoNuevo.Numero = txtPedido.Text;
                oPedidoNuevo.Numero2 = txtPedido.Text;
                oPedidoNuevo.Bonif = 0;
                oPedidoNuevo.Cotiz = Convert.ToDecimal(lblCotizacion.Text);
                oPedidoNuevo.Leyenda1 = txtLeyenda1.Text;
                oPedidoNuevo.Leyenda2 = txtLeyenda2.Text;
                oPedidoNuevo.Leyenda3 = txtLeyenda3.Text;
                oPedidoNuevo.Leyenda4 = txtLeyenda4.Text;
                if (txtImporteSeña.Text == "")
                {
                    txtImporteSeña.Text = "0";
                }
                oPedidoNuevo.ImporteSeña = txtImporteSeña.Text;

                ClienteOcasional cliente = new ClienteOcasional();
                cliente.DOMICILIO = txtDireccionClienteOcasional.Text;
                cliente.E_MAIL = txtEmailClienteOcasional.Text;
                cliente.IVA_D = "S";
                cliente.IVA_L = "S";
                cliente.RAZON_SOCI = txtNombreClienteOcasional.Text;
                cliente.TELEFONO_1 = TxtTelefonoClienteOcasional.Text;
                cliente.DIRECCION_ENTREGA = txtDireccionClienteOcasional.Text;
                cliente.ID_CATEGORIA_IVA = 2;
                cliente.MAIL_DE = txtEmailClienteOcasional.Text;
                oPedidoNuevo.ClienteOcasional = cliente;

            }

            dgv_Principal.EndEdit();
            if (dgv_Principal.Rows.Count > 0)
            {
                // VARIABLE PARA GUARDAR EL ARTICULO PARA UTILIZARLO CON LAS DESCRIPCIONES ADICIONALES
                string Art = "";
                string Base = "";

                for (int x = 0; x <= dgv_Principal.Rows.Count - 1; x++)
                {
                    DetallePedido oDetalle = new DetallePedido();
                    try
                    {
                        if (dgv_Principal.Rows[x].Cells["ColCodigo"].Value == null && dgv_Principal.Rows[x].Cells["ColDescripcion"].Value == null)
                        {
                        }
                        else
                        {
                            if (dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString() != "")
                            {
                                Art = dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString();
                                Base = Pedido.Empresa;
                                oDetalle.Codigo = dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString();
                                oDetalle.Base = Pedido.Empresa;
                                oDetalle.EsAdicional = false;
                            }
                            else
                            {
                                oDetalle.Codigo = Art;
                                oDetalle.Base = Base;
                                oDetalle.EsAdicional = true;
                            }

                            oDetalle.Descripcion = dgv_Principal.Rows[x].Cells["ColDescripcion"].Value.ToString();
                            if (esPedidoNuevo == true)
                            {
                                oDetalle.Deposito = dgv_Principal.Rows[x].Cells["ColDeposito"].Value.ToString();
                            }
                            oDetalle.Cantidad = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColCantidad"].Value);
                            oDetalle.PrecioL = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPrecioLista"].Value);
                            oDetalle.PrecioV = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPrecioVenta"].Value);
                            oDetalle.Bonif = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPorcentajeBonif"].Value);
                            oDetalle.Total = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColTotal"].Value);

                            oDetalle.Renglon = x + 1;

                            if (oDetalle.Base == "Empresa2")
                                oPedidoNuevo.Base2 = true;
                            else
                                oPedidoNuevo.Base1 = true;
                            oPedidoNuevo.Detalle.Add(oDetalle);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogDeErrores(ex, "LinkearControles");
                        throw;
                    }
                }
            }
            oPedidoNuevo.TalonarioPedido = iTalonario;
            oPedidoNuevo.TalonarioPedido2 = iTalonario2;
            oPedidoNuevo.Total = Convert.ToDecimal(txtTotal.Text);
            if (Convert.ToDecimal(txtTotalGeneralSumado.Text) == Convert.ToDecimal(oPedidoNuevo.ImporteSeña) && bModificarPedido)
            {
                oPedidoNuevo.Leyenda5 = "ABONADO";
            }
            else if (Convert.ToDecimal(txtTotal.Text) == Convert.ToDecimal(oPedidoNuevo.ImporteSeña) && bModificarPedido == false && bNuevo)
            {
                oPedidoNuevo.Leyenda5 = "ABONADO";
            }
            else if(bNuevo)
            {
                oPedidoNuevo.Leyenda5 = "PRESUPUESTADO";
            }
            return oPedidoNuevo;
        }

        public Pedido ReservarContinuacionPedido(string continuacionPedido)
        {
            CapaEntidades.Pedido oPedidoNuevo = new CapaEntidades.Pedido();
            oPedidoNuevo.Cliente = txtCliente.Text;
            oPedidoNuevo.CondicionVta = txtCondicionVenta.Text;
            string depositoPedido = oNegociosPedido.TraerDato(false, "GVA21", "COD_SUCURS", true, "NRO_PEDIDO", true, continuacionPedido);
            oPedidoNuevo.Deposito = depositoPedido;
            oPedidoNuevo.Fecha = dtpFecha.Value.Date;
            oPedidoNuevo.FechaEntrega = dtpFechaEntrega.Value.Date;
            oPedidoNuevo.Talonario = txtTalonario.Text;
            oPedidoNuevo.Transporte = txtTransporte.Text;
            oPedidoNuevo.Vendedor = txtVendedor.Text;
            oPedidoNuevo.EstadoPedido = ConfigurationManager.AppSettings.Get("EstadoPedido");
            oPedidoNuevo.Numero = continuacionPedido;
            oPedidoNuevo.Numero2 = txtPedido.Text;
            oPedidoNuevo.Bonif = Convert.ToDecimal(txtBonif.Text);
            oPedidoNuevo.IdDirEntrega = Convert.ToInt32(cmbDirEntrega.SelectedValue);
            oPedidoNuevo.Cotiz = Convert.ToDecimal(lblCotizacion.Text);

            oPedidoNuevo.Detalle = oNegociosPedido.TraerDetallesSegundoPedido(continuacionPedido);
            oPedidoNuevo.TalonarioPedido = iTalonario;
            oPedidoNuevo.TalonarioPedido2 = iTalonario2;
            decimal totalPed = 0;
            foreach (var total in oPedidoNuevo.Detalle.Select(x => x.Total))
            {
                totalPed += total;
            }
            oPedidoNuevo.Total = Convert.ToDecimal(totalPed.ToString("N2"));
            return oPedidoNuevo;
        }

        public void LimpiarControles()
        {
            bNuevo = false;
            bModificarPedido = false;
            txtCliente.Text = "";
            lblCliente.Text = "";
            dtpFecha.Value = DateTime.Now;
            txtCondicionVenta.Text = "";
            lblCondicionVenta.Text = "";
            txtVendedor.Text = "";
            lblVendedor.Text = "";
            txtTransporte.Text = "";
            lblTransporte.Text = "";
            txtTalonario.Text = "";
            lblTalonario.Text = "";
            txtDeposito.Text = "";
            lblDeposito.Text = "";
            cmbDirEntrega.SelectedText = "";
            txt_empresa.Text = "";
            txtEstadoPedido.Text = "";
            txtNombreClienteOcasional.Text = "";
            txtEmailClienteOcasional.Text = "";
            TxtTelefonoClienteOcasional.Text = "";
            txtDireccionClienteOcasional.Text = "";
            txtImporteSeña.Text = "";
            txtTotalGeneralSumado.Text = "0";
            //dtpFechaEntrega.Value = DateTime.Now;
            try
            {
                dgv_Principal.Rows.Clear();
            }
            catch
            {
            }
            txtTotal.Text = "0.00";
            txtSubtotal.Text = "0.00";
        }

        public void HabilitarControles(bool bEstado)
        {
            try
            {
                dgv_Principal.AllowUserToAddRows = false;
                dgv_Principal.AllowUserToDeleteRows = false;
                btnAceptar.Enabled = bEstado;
                btnCancelar.Enabled = bEstado;
                // btnNuevo.Enabled = !bEstado;
                btnModificar.Enabled = !bEstado;
                btnPrimero.Enabled = !bEstado;
                btnAnterior.Enabled = !bEstado;
                btnSiguiente.Enabled = !bEstado;
                btnUltimo.Enabled = !bEstado;
                btnBuscar.Enabled = !bEstado;
                btn_anular.Enabled = !bEstado;
                btnImprimir.Enabled = !bEstado;
                btn_reservar.Enabled = !bEstado;
                txtCliente.Enabled = bEstado;
                dtpFecha.Enabled = bEstado;
                txtCondicionVenta.Enabled = bEstado;
                txtVendedor.Enabled = bEstado;
                txtTransporte.Enabled = bEstado;
                txtTalonario.Enabled = bEstado;
                txtDeposito.Enabled = bEstado;
                dtpFechaEntrega.Enabled = bEstado;
                txtBonif.Enabled = bEstado;
                cmbDirEntrega.Enabled = bEstado;
                dgv_Principal.Columns["ColCodigo"].ReadOnly = !bEstado;
                dgv_Principal.Columns["ColDescripcion"].ReadOnly = !bEstado;
                dgv_Principal.Columns["ColDeposito"].Visible = !bEstado;
                dgv_Principal.Columns["ColCantidad"].ReadOnly = !bEstado;
                dgv_Principal.Columns["ColPrecioVenta"].ReadOnly = !bEstado;
            }
            catch (Exception ex)
            {
                LogDeErrores(ex, "habilitarControles");
                throw;
            }

        }

        public void ActualizarTotales()
        {
            try
            {
                double valorTemp1 = Convert.ToDouble(ObtenerSubtotal(true));
                //double valorTemp2Iva = Convert.ToDouble(ObtenerSubtotal(false));
                double subTotal = (100 - Convert.ToDouble(txtBonif.Text)) / 100 * valorTemp1;
                //double subTotal = valorTemp1 - Convert.ToDouble(txtBonif.Text);
                txtSubtotal.Text = subTotal.ToString("N2");
                //dIva = Convert.ToDouble(txtSubtotal.Text) * valorTemp2Iva / 100;
                double total = Convert.ToDouble(txtSubtotal.Text);
                txtTotal.Text = total.ToString("N2");
            }
            catch (Exception ex)
            {
                LogDeErrores(ex, "actualizarTotales");
                throw;
            }

        }

        public double ObtenerSubtotal(bool blanco)
        {
            double valor = 0;
            try
            {
                for (int x = 0; x <= dgv_Principal.Rows.Count - 1; x++)
                {
                    if (dgv_Principal.Rows[x].Cells["ColCodigo"].Value != null)
                    {
                        if (blanco)
                        {
                            valor += Convert.ToDouble(dgv_Principal.Rows[x].Cells["ColTotal"].Value);
                        }
                        else
                        {
                            valor = Convert.ToDouble(oNegociosPedido.TraerIva(dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString()));
                        }
                    }
                }

                return valor;
            }
            catch (Exception ex)
            {
                LogDeErrores(ex, "obtenerSubtotal");
                throw ex;
            }

        }

        public void CargarControles()
        {
            Pedido oPedido = new Pedido();
            try
            {
                #region OCULTAR Y MOSTRAR CONTROLES
                txtImporteSeña.ReadOnly = true;
                txtLeyenda1.BorderStyle = BorderStyle.None;
                txtLeyenda1.BorderStyle = BorderStyle.None;
                txtLeyenda2.BorderStyle = BorderStyle.None;
                txtLeyenda3.BorderStyle = BorderStyle.None;
                txtLeyenda4.BorderStyle = BorderStyle.None;
                //txtLeyenda1.Visible = true;
                //txtLeyenda2.Visible = true;
                //txtLeyenda3.Visible = true;
                //txtLeyenda4.Visible = true;
                //txt_Leyenda5.Visible = true;
                lblPedido1.Visible = false;
                lblPedido2.Visible = false;
                lblPedido3.Visible = false;
                lblprimerPed.Visible = false;
                lblSegundoPed.Visible = false;
                lblTercerPed.Visible = false;
                lblAltaClienteNuevo.Visible = false;
                txtTransporte.Visible = true;
                txtBonif.Visible = true;
                LabelTransporte.Visible = true;
                txtNombreClienteOcasional.Visible = false;
                txtEmailClienteOcasional.Visible = false;
                txtDireccionClienteOcasional.Visible = false;
                TxtTelefonoClienteOcasional.Visible = false;
                lblEmailClienteOcasional.Visible = false;
                #endregion

                oPedido = oNegociosPedido.CargarDatos();
                bool Bandera = true;
                string value = ConfigurationManager.AppSettings.Get("InhabilitarbtnModificar");
                dgv_Principal.Columns["ColDeposito"].Visible = false;
                if (lblDeposito.Visible == false)
                {
                    lblDeposi.Visible = true;
                    txtDeposito.Visible = true;
                    lblDeposito.Visible = true;
                }
                btnModificar.Enabled = Bandera;
                txtEstadoPedido.Text = oPedido.EstadoPedido;
                txtPedido.Text = oPedido.Numero;
                txtCliente.Text = oPedido.Cliente;
                dtpFecha.Value = oPedido.Fecha;
                txtCondicionVenta.Text = oPedido.CondicionVta;
                txtVendedor.Text = oPedido.Vendedor;
                txtTransporte.Text = oPedido.Transporte;
                txtTalonario.Text = oPedido.Talonario;
                txtDeposito.Text = oPedido.Deposito;
                dtpFechaEntrega.Value = oPedido.FechaEntrega;
                txtBonif.Text = Convert.ToString(oPedido.Bonif);
                txtLeyenda1.Text = oPedido.Leyenda1;
                txtLeyenda2.Text = oPedido.Leyenda2;
                txtLeyenda3.Text = oPedido.Leyenda3;
                txtLeyenda4.Text = oPedido.Leyenda4;
                txt_Leyenda5.Text = oPedido.Leyenda5;
                //txtImporteSeña.Text = oPedido.ImporteSeña;
                cmbDirEntrega.DataSource = oNegociosPedido.CargarDirecciones(txtCliente.Text).Tables[0];
                cmbDirEntrega.ValueMember = "ID_DIRECCION_ENTREGA";
                cmbDirEntrega.DisplayMember = "Direccion";
                oPedido.IdDirEntrega = Convert.ToInt32(cmbDirEntrega.SelectedValue);

                if (oPedido.IdDirEntrega == 0 && txtCliente.Text == "000000")
                {
                    txtDireccionClienteOcasional.Visible = true;
                    txtDireccionClienteOcasional.ReadOnly = true;
                    txtDireccionClienteOcasional.Text = oPedido.Domicilio;
                }


                //bool existe = oNegociosPedido.ComprobarDato("STA14", "FILLER", true, oPedido.Numero + "'AND ESTADO_MOV='P");
                if (txtEstadoPedido.Text == "Anulado")
                {
                    btnImprimir.Enabled = false;
                    btnSacarReserva.Enabled = false;
                    btn_reservar.Enabled = false;
                    btn_anular.Enabled = false;
                    btnModificar.Enabled = false;
                    txt_Leyenda5.Visible = false;
                }
                else
                {
                    btn_reservar.Visible = true;
                    btn_reservar.Enabled = true;
                    btn_anular.Enabled = true;
                    btnModificar.Enabled = true;
                    btnImprimir.Enabled = true;
                    lblTotalGeneralSumado.Visible = true;
                    txtTotalGeneralSumado.Visible = true;
                    txt_Leyenda5.Visible = true;
                    if (txt_Leyenda5.Text == "RESERVADO")
                    {
                        btnSacarReserva.Visible = true;
                        btnImprimir.Enabled = true;
                        btnSacarReserva.Enabled = true;
                        btn_reservar.Visible = false;
                        btn_anular.Enabled = true;
                        btnModificar.Enabled = false;
                    }
                    if (txt_Leyenda5.Text == "ABONADO" && txtEstadoPedido.Text =="Aprobado")
                    {
                        btnSacarReserva.Visible = false;
                    }
                }
                string Horario = oNegociosPedido.TraerDato(false, "DIRECCION_ENTREGA", "HORARIO_ENTREGA", true, "ID_DIRECCION_ENTREGA", true, oPedido.IdDirEntrega.ToString());
                lblHorario.Text = Interaction.IIf(Horario.Trim() == string.Empty, "Sin dato horario", Horario).ToString();
                lblDias.Text = oNegociosPedido.TraerDias(oPedido.IdDirEntrega);

                lblCotizacion.Text = Convert.ToString(oPedido.Cotiz);
                txtSubtotal.Text = "0";
                txtTotal.Text = "0";
                //lblCliente.Text = oNegociosPedido.TraerDato(false, "gva14", "razon_Soci", true, "Cod_client", true, oPedido.Cliente);
                lblCliente.Text = oPedido.RazonSocial;
                lblVendedor.Text = oNegociosPedido.TraerDato(false, "gva23", "nombre_ven", true, "cod_vended", true, oPedido.Vendedor);
                lblTransporte.Text = oNegociosPedido.TraerDato(false, "gva24", "nombre_tra", true, "cod_transp", true, oPedido.Transporte);
                lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "descrip", true, "talonario", true, oPedido.Talonario);
                lblCondicionVenta.Text = oNegociosPedido.TraerDato(false, "gva01", "desc_cond", true, "cond_vta", true, oPedido.CondicionVta);
                lblDeposito.Text = oNegociosPedido.TraerDato(false, "sta22", "nombre_suc", true, "cod_sucurs", true, oPedido.Deposito);

                dgv_Principal.Rows.Clear();

                decimal dSubTotal = 0;
                double precio;
                if (oPedido.Detalle.Count > 0)
                {
                    dgv_Principal.Rows.Clear();
                    dgv_Principal.Rows.Add(oPedido.Detalle.Count);

                    for (int x = 0; x <= oPedido.Detalle.Count - 1; x++)
                    {
                        dgv_Principal.Rows[x].Cells["ColCodigo"].Value = oPedido.Detalle[x].Codigo;
                        dgv_Principal.Rows[x].Cells["ColDescripcion"].Value = oPedido.Detalle[x].Descripcion;
                        oPedido.Detalle[x].Deposito = oPedido.Deposito;
                        dgv_Principal.Rows[x].Cells["ColDeposito"].Value = oPedido.Detalle[x].Deposito;
                        dgv_Principal.Rows[x].Cells["ColCantidad"].Value = oPedido.Detalle[x].Cantidad;
                        dgv_Principal.Rows[x].Cells["ColPrecioLista"].Value = oPedido.Detalle[x].PrecioL;

                        dgv_Principal.Rows[x].Cells["ColPorcentajeBonif"].Value = Math.Round(Convert.ToDecimal(oPedido.Detalle[x].Bonif), 2);

                        dgv_Principal.Rows[x].Cells["ColPrecioVenta"].Value = oPedido.Detalle[x].PrecioV;


                        precio = Convert.ToDouble(dgv_Principal.Rows[x].Cells["ColPrecioVenta"].Value) * Convert.ToDouble(dgv_Principal.Rows[x].Cells["ColCantidad"].Value) * Convert.ToDouble(oPedido.Detalle[x].Equivalencia);
                        dgv_Principal.Rows[x].Cells["ColTotal"].Value = precio;

                        //dgv_Principal.Rows[x].Cells["ColBase"].Value = oPedido.Detalle[x].Base;
                        dgv_Principal.Rows[x].Tag = oPedido.Detalle[x].Renglon;

                        if (dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString() == "")
                        {
                            dgv_Principal.Rows[x].ReadOnly = true;
                            dgv_Principal.Rows[x].DefaultCellStyle.BackColor = Color.LightGray;
                            dgv_Principal.Rows[x].DefaultCellStyle.ForeColor = Color.LightGray;
                            dgv_Principal.Rows[x].Cells["ColDescripcion"].Style.ForeColor = Color.Black;
                        }

                        dSubTotal = dSubTotal + oPedido.Detalle[x].Total;
                    }
                }

                    List<PedidosTablaPropia> PedidosReferencia = oNegociosPedido.TraerPedidosDeReferencia(txtPedido.Text);
                    foreach (var item in PedidosReferencia)
                    {
                        if (item.PrimerPedido != "" && item.PrimerPedido != txtPedido.Text)
                        {
                            lblprimerPed.Visible = true;
                            lblPedido1.Visible = true;
                            lblPedido1.Text = item.PrimerPedido;
                        }
                        if (item.SegundoPedido != "" && item.SegundoPedido != txtPedido.Text)
                        {
                            lblSegundoPed.Visible = true;
                            lblPedido2.Visible = true;
                            lblPedido2.Text = item.SegundoPedido;
                        }
                        if (item.TercerPedido != "" && item.TercerPedido != txtPedido.Text)
                        {
                            lblTercerPed.Visible = true;
                            lblPedido3.Visible = true;
                            lblPedido3.Text = item.TercerPedido;
                        }
                        txtTotalGeneralSumado.Text = item.ImporteTotalPedido;
                        txtImporteSeña.Text = item.Seña;

                    }
                
               
                ActualizarTotales();
            }
            catch (Exception ex)
            {
                LogDeErrores(ex, "cargarControles");
                throw;
            }
        }

        public void LogDeErrores(Exception ex, string funcion)
        {
            using (StreamWriter LogError = new StreamWriter(Application.StartupPath + "\\Errores.log", true))
            {
                LogError.WriteLine(DateTime.Now.ToString("dd-MM-yyy HH:mm") + " - " + ex.Message + funcion);
            }
        }

        #endregion


        #region ----------------------------------- BOTONES -----------------------------------------

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                txtLeyenda1.BorderStyle = BorderStyle.FixedSingle;
                txtLeyenda1.BorderStyle = BorderStyle.FixedSingle;
                txtLeyenda2.BorderStyle = BorderStyle.FixedSingle;
                txtLeyenda3.BorderStyle = BorderStyle.FixedSingle;
                txtLeyenda4.BorderStyle = BorderStyle.FixedSingle;
                txtLeyenda1.ReadOnly = false;
                txtLeyenda2.ReadOnly = false;
                txtLeyenda3.ReadOnly = false;
                txtLeyenda4.ReadOnly = false;
                txt_Leyenda5.ReadOnly = true;
                txtImporteSeña.ReadOnly = false;
                listaDeStock = new List<decimal>();
                lblDeposito.Visible = true;
                txtDeposito.Visible = true;
                lblDeposi.Visible = true;
                if (txtDireccionClienteOcasional.Visible == true)
                {
                    txtDireccionClienteOcasional.ReadOnly = true;
                    txtCliente.ReadOnly = true;
                }
                if (oNegociosPedido.CantidadPedidos > 0)
                {
                    //FormSeleccionPrecio frm = new FormSeleccionPrecio();
                    //frm.ShowDialog();

                    HabilitarControles(true);
                    bCancelo = false;
                    bModificarPedido = true;
                    Pedido.PedidoClienteOcasional = false;
                    // guardo todo en mi lista de pedidos
                    dgv_Principal.AllowUserToAddRows = false;
                    for (int x = 0; x <= dgv_Principal.Rows.Count - 1; x++)
                    {
                        CapaEntidades.DetallePedido detPed = new CapaEntidades.DetallePedido();
                        detPed.Codigo = dgv_Principal.Rows[x].Cells["ColCodigo"].Value.ToString();
                        detPed.Descripcion = dgv_Principal.Rows[x].Cells["ColDescripcion"].Value.ToString();
                        decimal cantidad = Convert.ToDecimal(oNegociosPedido.TraerDato(false, "STA19", "CANT_STOCK", false, "COD_DEPOSI", true, txtDeposito.Text + "' AND COD_ARTICU='" + detPed.Codigo));
                        listaDeStock.Add(cantidad);
                        //detPed.Deposito = dgv_Principal.Rows[x].Cells["ColDeposito"].Value.ToString();
                        detPed.Cantidad = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColCantidad"].Value);
                        detPed.PrecioL = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPrecioLista"].Value);
                        detPed.PrecioV = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPrecioVenta"].Value);
                        detPed.Bonif = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColPorcentajeBonif"].Value);
                        detPed.Total = Convert.ToDecimal(dgv_Principal.Rows[x].Cells["ColTotal"].Value);
                        detPed.Renglon = x + 1;
                    }

                    dgv_Principal.BeginEdit(true);
                    dgv_Principal.AllowUserToAddRows = true;
                    dgv_Principal.ReadOnly = false;

                }
                else
                    Interaction.MsgBox("No hay pedidos para modificar", MsgBoxStyle.Exclamation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            oNegociosPedido.primero();
            CargarControles();
            Cursor = Cursors.Default;
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            oNegociosPedido.Anterior();
            CargarControles();
            Cursor = Cursors.Default;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            oNegociosPedido.Siguiente();
            CargarControles();
            Cursor = Cursors.Default;
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            oNegociosPedido.Ultimo();
            CargarControles();
            Cursor = Cursors.Default;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string numeroPedido = oNegociosPedido.TraerDato(false, "NUMEROS_PEDIDOS_SISTEMA_VENTAS", "PRIMER_PEDIDO", true, "PRIMER_PEDIDO", true, txtPedido.Text + "' OR SEGUNDO_PEDIDO='" + txtPedido.Text + "' OR TERCER_PEDIDO='" + txtPedido.Text + "");
            int talonarioPedido = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"));
            oReporte.imprimirPedido(numeroPedido, talonarioPedido.ToString(), Conexion);
            Cursor.Current = Cursors.Default;
        }

        private void btn_anular_Click(object sender, EventArgs e)
        {
            try
            {
                string ValidarEstado = oNegociosPedido.TraerDato(false, "GVA21", "estado", true, "TALON_PED='" + ConfigurationManager.AppSettings.Get("Talonario") + "' AND nro_pedido", true, txtPedido.Text);

                if (ValidarEstado != "5")
                {
                    DialogResult result = MessageBox.Show("¿Esta seguro de anular el pedido?", "Atención!!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (ValidarEstado == "2" | ValidarEstado == "6" | ValidarEstado == "1")
                        {
                            if (oNegociosPedido.AnularPedido(txtPedido.Text, Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"))))
                            {
                                if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                {
                                    CargarControles();
                                    MessageBox.Show("El pedido fue ANULADO Correctamente!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("ATENCIÓN!! Error al anular el pedido", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else
                            MessageBox.Show("Error!! El pedido que intenta anular no tiene el estado para realizar esta acción", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                    MessageBox.Show("El pedido ya se encuentra anulado.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Else MsgBox("No se puede exportar,compruebe que exista el directorio de los archivos", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Titulo)

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btn_reservar_Click(object sender, EventArgs e)
        {
            try
            {
                Pedido.PedidoClienteOcasional = false;
                DialogResult result = MessageBox.Show($"¿Esta seguro desea reservar el pedido?", "Atención!!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (txtEstadoPedido.Text == "Aprobado" || txtEstadoPedido.Text == "Revisado")
                    {
                        Pedido oPedido1 = LinkearControles(false);
                        List<PedidosTablaPropia> PedidosReferencia = oNegociosPedido.TraerPedidosDeReferencia(txtPedido.Text);

                        if (PedidosReferencia.Count <= 3)
                        {
                            List<Pedido> lPedidos = new List<Pedido>();
                            lPedidos.Add(oPedido1);


                            foreach (var item in PedidosReferencia)
                            {
                                if (item.PrimerPedido != oPedido1.Numero && item.PrimerPedido != "")
                                {
                                    Pedido oPedidoContinuacion1 = ReservarContinuacionPedido(item.PrimerPedido);
                                    lPedidos.Add(oPedidoContinuacion1);

                                }
                                if (item.SegundoPedido != oPedido1.Numero && item.SegundoPedido != "")
                                {
                                    Pedido oPedidoContinuacion2 = ReservarContinuacionPedido(item.SegundoPedido);
                                    lPedidos.Add(oPedidoContinuacion2);

                                }
                                if (item.TercerPedido != oPedido1.Numero && item.TercerPedido != "")
                                {
                                    Pedido oPedidoContinuacion3 = ReservarContinuacionPedido(item.TercerPedido);

                                    lPedidos.Add(oPedidoContinuacion3);

                                }
                            }


                            if (oNegociosPedido.ReservarPedido(lPedidos))
                            {
                                if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                {
                                    CargarControles();
                                    //oNegociosPedido.EditarEstadoPedidoActual(ConfigurationManager.AppSettings.Get("EstadoPedidoReserva"));
                                    MessageBox.Show("Pedido RESERVADO correctamente.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("ERROR!! No se reservó el pedido.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                        MessageBox.Show("Error!! El pedido que intenta RESERVAR no tiene el estado para realizar esta acción", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnSacarReserva_Click(object sender, EventArgs e)
        {
            try
            {
                Pedido.PedidoClienteOcasional = false;
                DialogResult result = MessageBox.Show($"¿Esta seguro ELIMINAR LA RESERVA del pedido?", "Atención!!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (txtEstadoPedido.Text != "Anulado")
                    {
                        Pedido oPedido1 = LinkearControles(false);
                        List<PedidosTablaPropia> PedidosReferencia = oNegociosPedido.TraerPedidosDeReferencia(txtPedido.Text);
                        if (PedidosReferencia.Count <= 3)
                        {
                            List<Pedido> lPedidos = new List<Pedido>();
                            lPedidos.Add(oPedido1);
                            foreach (var item in PedidosReferencia)
                            {
                                if (item.PrimerPedido != oPedido1.Numero && item.PrimerPedido != "")
                                {
                                    Pedido oPedidoContinuacion1 = ReservarContinuacionPedido(item.PrimerPedido);
                                    lPedidos.Add(oPedidoContinuacion1);
                                }
                                if (item.SegundoPedido != oPedido1.Numero && item.SegundoPedido != "")
                                {
                                    Pedido oPedidoContinuacion2 = ReservarContinuacionPedido(item.SegundoPedido);
                                    lPedidos.Add(oPedidoContinuacion2);
                                }
                                if (item.TercerPedido != oPedido1.Numero && item.TercerPedido != "")
                                {
                                    Pedido oPedidoContinuacion3 = ReservarContinuacionPedido(item.TercerPedido);
                                    lPedidos.Add(oPedidoContinuacion3);
                                }
                            }

                            if (oNegociosPedido.EliminarReserva(lPedidos))
                            {
                                if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                {
                                    CargarControles();
                                    MessageBox.Show("La reserva del pedido fue ELIMANADA CORRECTAMENTE!!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("ERROR!! No eliminó la reserva.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                        MessageBox.Show("Error!! El pedido no tiene el estado para realizar esta acción", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                oBuscador.AgregarColumnaFill(0);
                oBuscador.Mostrar(Conexion, "SELECT nro_pedido  AS Numero,Fecha_pedi as Fecha FROM gva21 WHERE talon_ped=" + iTalonario + " ORDER BY 1 desc");
                if (oBuscador.devolverDato("Numero") != "")
                {
                    oNegociosPedido.BuscarPedido(oBuscador.devolverDato("Numero"), iTalonario);
                    CargarControles();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtTotal.Text) > 0)
            {
                string mensaje;
                mensaje = Convert.ToString(MessageBox.Show("¿Esta seguro que desea guardar el pedido?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation));
                if (mensaje == "Yes")
                {
                    Pedido oPedidoNuevo;
                    FormLeyendas oFormLeyenda;
                    dgv_Principal.EndEdit();
                    try
                    {
                        if (bModificarPedido)
                        {
                            oPedidoNuevo = LinkearControles(false);
                        }
                        else
                        {
                            oPedidoNuevo = LinkearControles(true);
                        }

                        double seña = Convert.ToDouble(txtImporteSeña.Text);
                        if (txtTotalGeneralSumado.Text == "")
                        {
                            txtTotalGeneralSumado.Text = "0";
                        }
                        double total = Convert.ToDouble(txtTotalGeneralSumado.Text);
                        if (seña <= total || seña <= Convert.ToDouble(txtTotal.Text))
                        {
                            if (txtEmailClienteOcasional.Text != "" && txtDireccionClienteOcasional.Text != "" && TxtTelefonoClienteOcasional.Text != "" && txtNombreClienteOcasional.Text != ""
                            || Pedido.PedidoClienteOcasional == false)
                            {
                                if (ValidarFormatoEmail(txtEmailClienteOcasional.Text) || Pedido.PedidoClienteOcasional == false)
                                {
                                    if (oPedidoNuevo.IdDirEntrega != 0 || Pedido.PedidoClienteOcasional || txtCliente.Text == "000000")
                                    {
                                        if (oNegociosPedido.ValidarCliente(txtCliente.Text))
                                        {
                                            if (oNegociosPedido.ValidarCondicionVta(txtCondicionVenta.Text))
                                            {
                                                //if (oNegociosPedido.ValidarDeposito())
                                                //{
                                                if (oNegociosPedido.ValidarTalonario(txtTalonario.Text) || txtTalonario.Text == "0")
                                                {
                                                    if (oNegociosPedido.ValidarTransporte(txtTransporte.Text))
                                                    {
                                                        if (oNegociosPedido.ValidarVendedor(txtVendedor.Text))
                                                        {
                                                            if (oPedidoNuevo.Detalle.Count > 0)
                                                            {
                                                                if (oNegociosPedido.ValidarArticulosGrilla(oPedidoNuevo.Detalle))
                                                                {
                                                                    if (oNegociosPedido.ValidarArticulosRepetidosGrilla(oPedidoNuevo.Detalle))
                                                                    {
                                                                        if (oNegociosPedido.ValidarLongitudDescripciones(oPedidoNuevo.Detalle))
                                                                        {
                                                                            if (oNegociosPedido.ValidarPreciosGrilla(oPedidoNuevo.Detalle))
                                                                            {
                                                                                bCancelo = true;
                                                                                oFormLeyenda = new FormLeyendas();
                                                                                if (bNuevo)
                                                                                {
                                                                                    if (oFormLeyenda.Mostrar(oPedidoNuevo, oNegociosPedido))
                                                                                    {
                                                                                        Cursor.Current = Cursors.WaitCursor;
                                                                                        if (txt_empresa.Text == ConfigurationManager.AppSettings.Get("Empresa1"))
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                if (oNegociosPedido.AgregarPedidoBase1(oPedidoNuevo))
                                                                                                {
                                                                                                    lblEmpresa.Visible = false;
                                                                                                    txt_empresa.Visible = false;
                                                                                                    Pedido.PedidoClienteOcasional = false;
                                                                                                    Pedido.PedidoClienteRegistradoEnTango = false;
                                                                                                    if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                                                                                    {
                                                                                                        LimpiarControles();
                                                                                                        HabilitarControles(false);
                                                                                                        bNuevo = false;
                                                                                                        bCancelo = true;
                                                                                                        CargarControles();
                                                                                                    }
                                                                                                    MessageBox.Show("El pedido fue grabado con exito.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    MessageBox.Show("No se pudo agregar el pedido, compruebe los datos y vuelva a intentarlo.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                                                                                                }
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                LogDeErrores(ex, "AgregarPedidoBase1");
                                                                                                throw;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                if (oNegociosPedido.AgregarPedidoBase2(oPedidoNuevo))
                                                                                                {
                                                                                                    lblEmpresa.Visible = false;
                                                                                                    txt_empresa.Visible = false;
                                                                                                    Pedido.PedidoClienteOcasional = false;
                                                                                                    Pedido.PedidoClienteRegistradoEnTango = false;
                                                                                                    if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                                                                                    {
                                                                                                        LimpiarControles();
                                                                                                        HabilitarControles(false);
                                                                                                        bNuevo = false;
                                                                                                        bCancelo = true;
                                                                                                        CargarControles();
                                                                                                    }
                                                                                                    MessageBox.Show("El pedido fue grabado con exito.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    MessageBox.Show("No se pudo agregar el pedido, compruebe los datos y vuelva a intentarlo.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                                                                }
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                LogDeErrores(ex, "AgregarPedidoBase2");
                                                                                                throw;
                                                                                            }
                                                                                        }
                                                                                        Cursor.Current = Cursors.Default;
                                                                                    }
                                                                                }
                                                                                else if (bModificarPedido)
                                                                                {
                                                                                    Cursor.Current = Cursors.WaitCursor;
                                                                                    if (oNegociosPedido.ModificarPedido(oPedidoNuevo))
                                                                                    {
                                                                                        if (oNegociosPedido.Conectar(Conexion, Conexion2))
                                                                                        {
                                                                                            LimpiarControles();
                                                                                            HabilitarControles(false);
                                                                                            bModificarPedido = false;
                                                                                            bCancelo = true;
                                                                                            CargarControles();
                                                                                        }
                                                                                        MessageBox.Show("El pedido fue modificado correctamente!!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                                    }
                                                                                    else
                                                                                        MessageBox.Show("No se pudo modificar el pedido, compruebe los datos y vuelva a intentarlo.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                                                    Cursor.Current = Cursors.Default;
                                                                                }
                                                                            }
                                                                            else
                                                                                MessageBox.Show("El precio de venta de los artículos debe ser mayor a 0", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                                        }
                                                                        else
                                                                            MessageBox.Show("Hay descripciones en los renglones que pasan el máximo permitido de 50 caracteres en la descripción.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                                    }
                                                                    else
                                                                        MessageBox.Show("Hay artículos con un mismo deposito repetidos en la grilla, verifique o cambie de deposito.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                                }
                                                                else
                                                                    MessageBox.Show("Hay artículos en la grilla incorrectos.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                            }
                                                            else
                                                                MessageBox.Show("Debe ingresar un artículo en la grilla.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                        }
                                                        else
                                                            MessageBox.Show("El vendedor es incorrecto.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                    }
                                                    else
                                                        MessageBox.Show("El transporte es incorrecto.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                }
                                                else
                                                    MessageBox.Show("El talonario es incorrecto.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            }
                                            else
                                                MessageBox.Show("La condición de venta es incorrecta.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                        else
                                            MessageBox.Show("El cliente es incorrecto.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Seleccione una dirección de entrega para continuar.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        cmbDirEntrega.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Los caracteres del E-MAIL, son incorrectos.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtEmailClienteOcasional.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Por favor completar los campos que estan vacios para continuar.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error!! El importe de seña ingresado supera el monto total del pedido.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtImporteSeña.Select();
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }
            else
            {
                MessageBox.Show("El importe total del pedido es inválido.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bCancelo = true;
            LimpiarControles();
            HabilitarControles(false);
            bModificarPedido = false;
            CargarControles();
            dgv_Principal.EndEdit();
            if (lblEmpresa.Visible == true && txt_empresa.Visible == true)
            {
                lblEmpresa.Visible = false;
                txt_empresa.Visible = false;
            }
            LabelTransporte.Visible = true;
            //----------------------------- OCULTAR CONTROLES CLIENTE OCASIONAL-------------------
            txtNombreClienteOcasional.Visible = false;
            lblEmailClienteOcasional.Visible = false;
            txtEmailClienteOcasional.Visible = false;
            lblTelefonoClienteOcasional.Visible = false;
            TxtTelefonoClienteOcasional.Visible = false;
            txtDireccionClienteOcasional.Visible = false;
            txtTransporte.Visible = true;
            txtBonif.Visible = true;
        }

        private void btnPedidoNuevoClienteOcasional_Click(object sender, EventArgs e)
        {
            #region
            lblTotalGeneralSumado.Visible = false;
            txtTotalGeneralSumado.Visible = false;
            lblprimerPed.Visible = false;
            lblSegundoPed.Visible = false;
            lblTercerPed.Visible = false;
            lblPedido1.Visible = false;
            lblPedido2.Visible = false;
            lblPedido3.Visible = false;
            txtLeyenda1.Visible = false;
            txtLeyenda2.Visible = false;
            txtLeyenda3.Visible = false;
            txtLeyenda4.Visible = false;
            txt_Leyenda5.Visible = false;
            txtImporteSeña.ReadOnly = false;
            lblAltaClienteNuevo.Visible = false;
            txtTransporte.Visible = false;
            txtBonif.Visible = false;
            LabelTransporte.Visible = false;
            //------------------ MUESTRO CONTROLES DE CLIENTE OCASIONAL-----------------
            txtNombreClienteOcasional.Visible = true;
            lblEmailClienteOcasional.Visible = true;
            txtEmailClienteOcasional.Visible = true;
            lblTelefonoClienteOcasional.Visible = true;
            TxtTelefonoClienteOcasional.Visible = true;
            txtDireccionClienteOcasional.Visible = true;
            txtDireccionClienteOcasional.ReadOnly = false;
            #endregion

            //----------------------------------------------------------------------------
            listaDeStock = new List<decimal>();
            btnSacarReserva.Visible = false;
            btn_reservar.Visible = true;
            txt_Leyenda5.Visible = false;
            //FormSeleccionPrecio frm = new FormSeleccionPrecio();
            //frm.ShowDialog();
            FormSeleccionarEmpresa formEmpresa = new FormSeleccionarEmpresa();
            formEmpresa.ShowDialog();
            Pedido.PedidoClienteOcasional = true;
            lblEmpresa.Visible = true;
            txt_empresa.Visible = true;
            lblDeposito.Visible = false;
            txtDeposito.Visible = false;
            lblDeposi.Visible = false;

            HabilitarControles(true);
            LimpiarControles();
            dtpFecha.Value = DateTime.Now;
            dtpFechaEntrega.Value = new DateTime(1800, 1, 1);
            //dtpFechaEntrega.Value = TraerProximoDiaHabil96hs();
            txt_empresa.Text = Pedido.Empresa;
            txtPedido.Text = oNegociosPedido.TraerUltimoNumeroPedido(iTalonario.ToString());
            bModificarPedido = false;
            bCancelo = false;
            txtCliente.Text = "000000";
            txtEstadoPedido.Text = "Cargando";
            txtBonif.Text = "0";
            lblCotizacion.Text = "1";

            bNuevo = true;
            dgv_Principal.ReadOnly = false;

            dgv_Principal.Columns["ColPorcentajeBonif"].ReadOnly = true;
            dgv_Principal.Columns["ColTotal"].ReadOnly = true;
            dgv_Principal.Columns["ColDeposito"].Visible = true;
            txtNombreClienteOcasional.Focus();
            dgv_Principal.AllowUserToAddRows = true;
        }

        private void btnPedidoNuevoClienteRegistrado_Click(object sender, EventArgs e)
        {
            #region OCULTAR Y MOSTRAR CONTROLES
            lblTotalGeneralSumado.Visible = false;
            txtTotalGeneralSumado.Visible = false;
            lblprimerPed.Visible = false;
            lblSegundoPed.Visible = false;
            lblTercerPed.Visible = false;
            lblPedido1.Visible = false;
            lblPedido2.Visible = false;
            lblPedido3.Visible = false;
            txtLeyenda1.Visible = false;
            txtLeyenda2.Visible = false;
            txtLeyenda3.Visible = false;
            txtLeyenda4.Visible = false;
            txt_Leyenda5.Visible = false;
            txtImporteSeña.ReadOnly = false;
            txtTransporte.Visible = true;
            txtBonif.Visible = true;
            LabelTransporte.Visible = true;
            //----------------------------- OCULTAR CONTROLES CLIENTE OCASIONAL-------------------
            txtNombreClienteOcasional.Visible = false;
            lblEmailClienteOcasional.Visible = false;
            txtEmailClienteOcasional.Visible = false;
            lblTelefonoClienteOcasional.Visible = false;
            TxtTelefonoClienteOcasional.Visible = false;
            txtDireccionClienteOcasional.Visible = false;
            #endregion

            //----------------------------------------------------------------------------
            listaDeStock = new List<decimal>();
            btnSacarReserva.Visible = false;
            btn_reservar.Visible = true;
            txt_Leyenda5.Visible = false;
            FormSeleccionarEmpresa formEmpresa = new FormSeleccionarEmpresa();
            formEmpresa.ShowDialog();

            lblEmpresa.Visible = true;
            txt_empresa.Visible = true;
            lblDeposito.Visible = false;
            txtDeposito.Visible = false;
            lblDeposi.Visible = false;
            Pedido.PedidoClienteOcasional = false;
            HabilitarControles(true);
            LimpiarControles();
            dtpFecha.Value = DateTime.Now;
            dtpFechaEntrega.Value = new DateTime(1800,1,1);
            //dtpFechaEntrega.Value = TraerProximoDiaHabil96hs();
            txt_empresa.Text = Pedido.Empresa;
            txtPedido.Text = oNegociosPedido.TraerUltimoNumeroPedido(iTalonario.ToString());
            bModificarPedido = false;
            bCancelo = false;
            txtCliente.Focus();
            txtEstadoPedido.Text = "Cargando";
            txtBonif.Text = "0";
            lblCotizacion.Text = "1";
            //lblCotizacion.Text = oNegociosPedido.traerDato(false, ConfigurationManager.AppSettings.Get("BasePrecios") + "..cotizacionPedido", "valor", false, "1", false, "1");
            lblAltaClienteNuevo.Visible = true;
            bNuevo = true;
            dgv_Principal.ReadOnly = false;

            dgv_Principal.Columns["ColPorcentajeBonif"].ReadOnly = true;
            dgv_Principal.Columns["ColTotal"].ReadOnly = true;
            dgv_Principal.Columns["ColDeposito"].Visible = true;
        }
        #endregion


        #region -------------------------- CONTROLES GENERALES (TXT, DATE_TIME, GRILLA)-------------------------------------------

        private void txtLeyenda1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda2.Focus();
        }

        private void txtImporteSeña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (txtImporteSeña.Text == "")
                    txtImporteSeña.Text = "0";
        }

        private void txtLeyenda3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda3.Focus();
        }

        private void txtLeyenda4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtImporteSeña.Focus();
        }

        private void txtImporteSeña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            // condicion que no permite dar salto de linea
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
            }
            // condicion que no permite utilizar letras en el campo
            else if (char.IsLetter(e.KeyChar))
            {
                e.Handled = false;

            }
            // condicion que no permite utilizar la tecla de borrado
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // condicion que nos permite utilizar los espacios
            else if (char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            cBuscador oBuscador = new cBuscador();
            string categoriaIva = "";
            int id_categoria_iva = 0;
            try
            {
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("gva14", "cod_client", true, txtCliente.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(0);

                            oBuscador.Filtrado = cBuscador.MiFiltrado.Medio;

                            oBuscador.Mostrar(Conexion, "SELECT razon_Soci as Nombre , gva14.cod_Client as Codigo  FROM gva14");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {

                                if ((txtCliente.Text == oBuscador.devolverDato("Codigo")) && (txtCliente.Text != ""))
                                    precargado = true;
                                else
                                    precargado = false;

                                txtCliente.Text = oBuscador.devolverDato("Codigo");
                                lblCliente.Text = oBuscador.devolverDato("Nombre");

                                if (txtCliente.Text != "")
                                {
                                    CargarDatos(txtCliente.Text.Trim());

                                    id_categoria_iva = Convert.ToInt32(oNegociosPedido.TraerDato(false, "gva14", "id_categoria_iva", false, "cod_client", true, txtCliente.Text.ToString()));
                                    categoriaIva = oNegociosPedido.TraerDato(false, "categoria_iva", "COD_CATEGORIA_IVA", true, "id_categoria_iva", false, id_categoria_iva.ToString());

                                    switch (categoriaIva)
                                    {
                                        case "RI":
                                            {
                                                txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaA").ToString();
                                                lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                                break;
                                            }

                                        case "EX":
                                        case "RS":
                                        case "CF":
                                            {
                                                txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaB").ToString();
                                                lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                                break;
                                            }

                                        case "EXE":
                                            {
                                                txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaE").ToString();
                                                lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                                break;
                                            }

                                        default:
                                            {
                                                oBuscador.Mostrar(Conexion, "SELECT gva43.Talonario,descrip as Descripcion FROM gva43 where comprob = 'FAC'");
                                                if (oBuscador.devolverDato("Talonario") != "")
                                                {
                                                    txtTalonario.Text = oBuscador.devolverDato("Talonario");
                                                    lblTalonario.Text = oBuscador.devolverDato("Descripcion");
                                                    if (ComprobarTalonario())
                                                        txtVendedor.Focus();
                                                    else
                                                        txtTalonario.Focus();
                                                }
                                                else
                                                {
                                                    txtTalonario.Text = "";
                                                    lblTalonario.Text = "";
                                                }

                                                break;
                                            }
                                    }
                                    txtTalonario.Focus();

                                    string Horario = oNegociosPedido.TraerDato(false, "DIRECCION_ENTREGA", "HORARIO_ENTREGA", true, "ID_DIRECCION_ENTREGA", true, cmbDirEntrega.SelectedValue.ToString());
                                    lblHorario.Text = (string)Interaction.IIf(Horario.Trim() == string.Empty, "Sin dato horario", Horario);
                                    lblDias.Text = oNegociosPedido.TraerDias(Convert.ToInt32(cmbDirEntrega.SelectedValue.ToString()));
                                    //lblDias.Text = oNegociosPedido.TraerDias(cmbDirEntrega.SelectedIndex);

                                }
                            }
                            else
                            {
                                txtCliente.Text = "";
                                lblCliente.Text = "";
                                txtCliente.Focus();
                            }
                        }
                        else
                        {
                            precargado = false;

                            lblCliente.Text = oNegociosPedido.TraerDato(false, "gva14", "razon_Soci", true, "Cod_client", true, txtCliente.Text);
                            if (txtCliente.Text != "")
                            {
                                CargarDatos(txtCliente.Text.Trim());
                                id_categoria_iva = Convert.ToInt32(oNegociosPedido.TraerDato(false, "gva14", "id_categoria_iva", false, "cod_client", true, txtCliente.Text.ToString()));
                                categoriaIva = oNegociosPedido.TraerDato(false, "categoria_iva", "COD_CATEGORIA_IVA", true, "id_categoria_iva", false, id_categoria_iva.ToString());

                                switch (categoriaIva)
                                {
                                    case "RI":
                                        {
                                            txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaA").ToString();
                                            lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                            break;
                                        }

                                    case "EX":
                                    case "RS":
                                    case "CF":
                                        {
                                            txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaB").ToString();
                                            lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                            break;
                                        }

                                    case "EXE":
                                        {
                                            txtTalonario.Text = ConfigurationManager.AppSettings.Get("FacturaE").ToString();
                                            lblTalonario.Text = oNegociosPedido.TraerDato(false, "gva43", "DESCRIP", true, "Talonario", true, txtTalonario.Text).ToString();
                                            break;
                                        }

                                    default:
                                        {
                                            oBuscador.Mostrar(Conexion, "SELECT gva43.Talonario,descrip as Descripcion FROM gva43 where comprob = 'FAC'");

                                            if (oBuscador.devolverDato("Talonario") != "")
                                            {
                                                txtTalonario.Text = oBuscador.devolverDato("Talonario");
                                                lblTalonario.Text = oBuscador.devolverDato("Descripcion");
                                                if (ComprobarTalonario())
                                                    txtVendedor.Focus();
                                                else
                                                    txtTalonario.Focus();
                                            }
                                            else
                                            {
                                                txtTalonario.Text = "";
                                                lblTalonario.Text = "";
                                            }



                                            break;
                                        }
                                }

                                txtTalonario.Focus();

                                string Horario = oNegociosPedido.TraerDato(false, "DIRECCION_ENTREGA", "HORARIO_ENTREGA", true, "ID_DIRECCION_ENTREGA", true, cmbDirEntrega.SelectedValue.ToString());
                                lblHorario.Text = (string)Interaction.IIf(Horario.Trim() == string.Empty, "Sin dato horario", Horario);
                                lblDias.Text = oNegociosPedido.TraerDias(Convert.ToInt32(cmbDirEntrega.SelectedValue.ToString()));
                            }

                        }


                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void txtCondicionVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("gva01", "cond_Vta", true, txtCondicionVenta.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT gva01.cond_Vta as Codigo,desc_cond as Nombre FROM gva01");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {
                                txtCondicionVenta.Text = oBuscador.devolverDato("Codigo");
                                lblCondicionVenta.Text = oBuscador.devolverDato("Nombre");
                                if (TxtTelefonoClienteOcasional.Visible == true)
                                {
                                    txtEmailClienteOcasional.Focus();
                                }
                                else
                                {
                                    txtTransporte.Focus();
                                }

                            }
                            else
                            {
                                lblCondicionVenta.Text = "";
                                txtCondicionVenta.Text = "";
                                txtCondicionVenta.Focus();
                            }
                        }
                        else
                        {
                            lblCondicionVenta.Text = oNegociosPedido.TraerDato(false, "gva01", "desc_cond", true, "cond_vta", true, txtCondicionVenta.Text.Trim());
                            if (TxtTelefonoClienteOcasional.Visible == true)
                            {
                                txtEmailClienteOcasional.Focus();
                            }
                            else
                            {
                                txtTransporte.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void txtVendedor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("gva23", "cod_vended", true, txtVendedor.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT gva23.cod_vended as Codigo,nombre_ven as Nombre FROM gva23 WHERE INHABILITA=0");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {
                                txtVendedor.Text = oBuscador.devolverDato("Codigo");
                                lblVendedor.Text = oBuscador.devolverDato("Nombre");
                                txtCondicionVenta.Focus();
                            }
                            else
                            {
                                txtVendedor.Text = "";
                                lblVendedor.Text = "";
                                txtVendedor.Focus();
                            }
                        }
                        else
                        {
                            lblVendedor.Text = oNegociosPedido.TraerDato(false, "GVA23", "nombre_ven", true, "cod_vended", true, txtVendedor.Text.Trim());
                            txtCondicionVenta.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void txtTransporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("gva24", "cod_Transp", true, txtTransporte.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT gva24.cod_Transp as Codigo,nombre_Tra as Nombre FROM gva24");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {
                                txtTransporte.Text = oBuscador.devolverDato("Codigo");
                                lblTransporte.Text = oBuscador.devolverDato("Nombre");
                                //txtDeposito.Focus();
                                txtBonif.Focus();
                            }
                            else
                            {
                                txtTransporte.Text = "";
                                lblTransporte.Text = "";
                                txtTransporte.Focus();
                            }
                        }
                        else
                        {
                            lblTransporte.Text = oNegociosPedido.TraerDato(false, "gva24", "nombre_tra", true, "cod_transp", true, txtTransporte.Text.Trim());
                            //txtDeposito.Focus();
                            txtBonif.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void txtTalonario_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("gva43", "talonario", true, txtTalonario.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT gva43.Talonario,descrip as Descripcion FROM gva43 WHERE COMPROB='FAC' AND FECHA_VTO >= GETDATE()");
                            if (oBuscador.devolverDato("Talonario") != "")
                            {
                                txtTalonario.Text = oBuscador.devolverDato("Talonario");
                                lblTalonario.Text = oBuscador.devolverDato("Descripcion");
                                if (ComprobarTalonario())
                                    txtVendedor.Focus();
                                else
                                    txtTalonario.Focus();
                            }
                            else
                            {
                                txtTalonario.Text = "";
                                lblTalonario.Text = "";
                            }
                        }
                        else
                        {
                            lblTalonario.Text = oNegociosPedido.TraerDato(false, "GVA43", "descrip as Descripcion", true, "Talonario", true, txtTalonario.Text.Trim());
                            if (ComprobarTalonario())
                            {
                                txtVendedor.Focus();
                            }
                            else
                            {
                                txtTalonario.Focus();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void txtBonif_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
            dtpFechaEntrega.Focus();

        }

        private void txtDeposito_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();
                if (bCancelo == false)
                {
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedido.ComprobarDato("sta22", "cod_sucurs", true, txtDeposito.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT sta22.cod_sucurs as Codigo,nombre_suc as Nombre FROM sta22 WHERE INHABILITA=0");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {
                                txtDeposito.Text = oBuscador.devolverDato("Codigo");
                                lblDeposito.Text = oBuscador.devolverDato("Nombre");
                                txtBonif.Focus();
                            }
                            else
                            {
                                txtDeposito.Text = "";
                                lblDeposito.Text = "";
                                txtDeposito.Focus();
                            }
                        }
                        else
                        {
                            lblDeposito.Text = oNegociosPedido.TraerDato(false, "sta22", "nombre_suc", true, "cod_sucurs", true, txtDeposito.Text.Trim());
                            txtBonif.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dtpFecha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtCliente.Focus();
            e.Handled = true;
        }

        private void dtpFechaEntrega_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDirEntrega.Focus();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                    txtBonif.Focus();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void dgv_Principal_CellValidating_1(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                dgv_Principal.EndEdit();
                Edita = CapaEntidades.Pedido.sTipoPrecio;

                if (bNuevo || bModificarPedido)
                {

                    if (dgv_Principal.Columns[e.ColumnIndex].Name == "ColCodigo" && dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value == null)
                    {
                        //dgv_Principal.Rows[e.RowIndex].Cells["ColDeposito"].Value = "";
                        dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value = 0;
                        dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].Value = 0;
                        dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].Value = 0;
                        dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = 0;
                        dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].Value = 0;
                        ActualizarTotales();
                    }


                    if (e.ColumnIndex == 4 && bMovioCelda == false && dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value == null)
                    {
                        var cantidad = int.TryParse(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value.ToString(), out int result);
                        if (dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value.Equals(0) || cantidad == false)
                            e.Cancel = true;

                        ActualizarTotales();
                    }

                }
                bMovioCelda = false;


                if (bNuevo | bModificarPedido)
                {
                    CapaEntidades.DetallePedido detPedido = new CapaEntidades.DetallePedido();
                    if (dgv_Principal.Rows[e.RowIndex].ReadOnly == false)
                    {
                        if (e.ColumnIndex == 0 & bMovioCelda == false)
                        {
                            dgv_Principal.Rows[e.RowIndex].Cells["ColDescripcion"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColDeposito"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].ReadOnly = false;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].ReadOnly = false;

                            // If Not precargado Then
                            if (!oNegociosPedido.ComprobarDato("sta11 INNER JOIN GVA17 on GVA17.COD_ARTICU = sta11.COD_ARTICU", "GVA17.PRECIO > 0 AND STA11.PERFIL <> 'N' AND STA11.USA_ESC <> 'B' AND STA11.COD_ARTICU", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value == null ? "" : dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString().Replace("'", "''")))
                            {
                                cBuscador oBuscador = new cBuscador();
                                oBuscador.AgregarColumnaFill(0);
                                oBuscador.Filtrado = SeinBuscador.cBuscador.MiFiltrado.Medio;

                                string ListaDePrecio = ConfigurationManager.AppSettings.Get("ListaPrecio");
                                string[] separar = ListaDePrecio.Split(',');
                                string consulta = "GVA17.NRO_DE_LIS in (";

                                for (int value = 0; value <= separar.Length - 1; value++)
                                {
                                    if ((separar.Length - 1) != value)
                                        consulta += "'" + separar[value].ToString() + "',";
                                    else
                                        consulta += "'" + separar[value].ToString() + "'";
                                }

                                consulta += ")";

                                if (brenglon != true)
                                {
                                    oBuscador.Mostrar(Conexion, "SELECT STA11.DESCRIPCIO  + DESC_ADIC AS Descripcion, STA11.COD_ARTICU AS Codigo FROM STA11 INNER JOIN GVA17 on GVA17.COD_ARTICU = sta11.COD_ARTICU WHERE GVA17.PRECIO > 0 AND  " + consulta + " and STA11.PERFIL <> 'N' AND STA11.USA_ESC <> 'B'");

                                    if (oBuscador.devolverDato("Codigo") != "")
                                    {
                                        dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value = oBuscador.devolverDato("Codigo");
                                        dgv_Principal.Rows[e.RowIndex].Cells["ColDescripcion"].Value = oBuscador.devolverDato("Descripcion");

                                        decimal dEquivalencia;
                                        if (dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value != null)
                                        {
                                            //dEquivalencia = Convert.ToDecimal(oNegociosPedido.TraerDato(false, "STA11", "EQUIVALE_V", false, "cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString()));

                                            // aca le pongo 1 HAY QUE REVISRA SI ESASI PEO PARECE QUE SI

                                            dEquivalencia = 1;

                                            SendKeys.Send("{tab}");
                                            dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value = 0;
                                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Math.Round(Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "precio", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio1") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString())), 2);

                                            if (dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value.Equals(0))
                                                dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Math.Round(Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "precio", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio2") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString())) * Convert.ToDecimal(lblCotizacion.Text), 2);

                                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].Value = 0;
                                            dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].Value = 0;
                                            dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].Value = 0;
                                        }
                                    }
                                    return;
                                }
                            }
                            else
                            {
                                //decimal dEquivalencia = Convert.ToDecimal(oNegociosPedido.TraerDato(false, "STA11", "EQUIVALE_V", false, "cod_Articu", true, Convert.ToString(dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value)));
                                decimal dEquivalencia = 1;
                                dgv_Principal.Rows[e.RowIndex].Cells["ColDescripcion"].Value = oNegociosPedido.TraerDato(false, "STA11", "Descripcio  + desc_Adic", true, "cod_articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString());
                                dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Math.Round(Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "precio", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio1") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString())), 2);

                                if (dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value.Equals(0))
                                    dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Math.Round(Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "precio", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio2") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString())) * Convert.ToDecimal(lblCotizacion.Text), 2);


                                if (!dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value.ToString().Equals(""))
                                {
                                    if (System.Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value) == 0)
                                        dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "precio", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio1") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString()));
                                }
                                else
                                    dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = dEquivalencia * Convert.ToDecimal(oNegociosPedido.TraerDato(false, "GVA17", "GVA17", false, "nro_de_lis=" + ConfigurationManager.AppSettings.Get("ListaPrecio1") + " AND cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString()));

                                SendKeys.Send("{tab}");
                            }
                            dgv_Principal.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            dgv_Principal.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            if (e.ColumnIndex == 2 & bMovioCelda == false)
                            {
                                //dgv_Principal.Columns["ColDeposito"].ReadOnly = true;
                                if (dgv_Principal.Columns["ColDeposito"].Visible == true)
                                {
                                    dgv_Principal.Rows[e.RowIndex].Cells["ColDeposito"].Value = "";
                                    cBuscador oBuscador = new cBuscador();

                                    //if (oNegociosPedido.ComprobarDato("sta22", "cod_sucurs", true, ConfigurationManager.AppSettings.Get("CODIGO_DEPOSITO").ToString()) == false)
                                    //{
                                        oBuscador.AgregarColumnaFill(1);
                                        oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                                        oBuscador.Mostrar(Conexion, "SELECT COD_SUCURS as Codigo, NOMBRE_SUC as Nombre, FORMAT(ISNULL(STA19.CANT_STOCK,0),'0.00') AS [Stock Disp] FROM STA22 LEFT JOIN STA19 " +
                                        "ON STA19.COD_DEPOSI = STA22.COD_SUCURS AND STA19.COD_ARTICU='" + dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value + "' WHERE INHABILITA=0 AND COD_SUCURS IN(" + ConfigurationManager.AppSettings.Get("CODIGO_DEPOSITO").ToString() + ")");
                                        if (oBuscador.devolverDato("Codigo") != "")
                                        {
                                            dgv_Principal.Rows[e.RowIndex].Cells["ColDeposito"].Value = oBuscador.devolverDato("Codigo");
                                            listaDeStock.Add(Convert.ToDecimal(oBuscador.devolverDato("Stock Disp")));
                                        }
                                    //}
                                }
                            }
                            else
                            {
                                if (e.ColumnIndex == 3 & bMovioCelda == false && bNuevo)
                                {
                                    if (Convert.ToDecimal(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value) > listaDeStock[e.RowIndex])
                                    {
                                        MessageBox.Show("La cantidad ingresada supera el stock disponible", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                            if (bModificarPedido && bNuevo == false)
                            {

                                if (e.ColumnIndex == 3 & bMovioCelda == false)
                                {
                                    decimal cantidad = Convert.ToDecimal(oNegociosPedido.TraerDato(false, "STA19", "CANT_STOCK", false, "COD_DEPOSI", true, txtDeposito.Text + "' AND COD_ARTICU='" + dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString()));
                                    if (listaDeStock.Count >= e.RowIndex + 1)
                                    {
                                        listaDeStock[e.RowIndex] = cantidad;
                                    }
                                    else
                                    {
                                        listaDeStock.Add(cantidad);
                                    }
                                    if (Convert.ToDecimal(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value) > listaDeStock[e.RowIndex])
                                    {
                                        MessageBox.Show("La cantidad ingresada supera el stock disponible: " + listaDeStock[e.RowIndex].ToString("N2"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }

                        }
                    }
                    bMovioCelda = false;

                    if (dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value != null)
                    {
                        if (e.ColumnIndex == 1 && dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString() == "" && e.RowIndex > 0)
                        {
                            dgv_Principal.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                            dgv_Principal.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.LightGray;

                            dgv_Principal.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Transparent;

                            dgv_Principal.Rows[e.RowIndex].Cells["ColDescripcion"].Style.ForeColor = Color.Black;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColDescripcion"].Style.SelectionForeColor = Color.Black;

                            brenglon = true;

                            dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value = 0;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value = 0;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].Value = 0;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].Value = 0;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].Value = 0;


                            dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].ReadOnly = true;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].ReadOnly = false;
                            dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].ReadOnly = true;


                            SendKeys.Send("{tab}");
                            SendKeys.Send("{tab}");
                            SendKeys.Send("{tab}");
                        }
                        else
                            brenglon = false;
                    }
                    if (e.ColumnIndex == 4 | e.ColumnIndex == 5 | e.ColumnIndex == 6 | e.ColumnIndex == 7)
                    {
                        //decimal dEquivalencia = Convert.ToDecimal(oNegociosPedido.TraerDato(false, "STA11", "EQUIVALE_V", false, "cod_Articu", true, dgv_Principal.Rows[e.RowIndex].Cells["ColCodigo"].Value.ToString()));
                        decimal dEquivalencia = 1;
                        double precioVentaLuegoDeBonificacion = 0;
                        if (string.IsNullOrEmpty(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value.ToString()))
                            dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value = 0;

                        // Acá tengo mi cálculo
                        precioVentaLuegoDeBonificacion = Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value) - (Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioLista"].Value) * Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColPorcentajeBonif"].Value) / (double)100);

                        dgv_Principal.Rows[e.RowIndex].Cells["ColPrecioVenta"].Value = precioVentaLuegoDeBonificacion;
                        dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].Value = Convert.ToDouble(precioVentaLuegoDeBonificacion) * Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value) * Convert.ToDouble(dEquivalencia);

                        dgv_Principal.Rows[e.RowIndex].Cells["ColTotal"].Value = precioVentaLuegoDeBonificacion * Convert.ToDouble(dgv_Principal.Rows[e.RowIndex].Cells["ColCantidad"].Value) * Convert.ToDouble(dEquivalencia);
                        ActualizarTotales();
                    }

                }
                bMovioCelda = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgv_Principal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                bMovioCelda = true;
            else
                bMovioCelda = false;
        }

        private void dgv_Principal_KeyDown(object sender, KeyEventArgs e)
        {
            try

            {
                dgv_Principal.EndEdit();

                if (bNuevo | bModificarPedido)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        //int indice = dgv_Principal.CurrentRow.Index;
                        dgv_Principal.Rows.Remove(dgv_Principal.CurrentRow);
                    }

                    if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down | e.KeyCode == Keys.Left | e.KeyCode == Keys.Right)
                        bMovioCelda = true;
                    else
                        bMovioCelda = false;

                    if (e.KeyCode == Keys.Enter)
                        bMovioCelda = false;
                }
                ActualizarTotales();
            }
            catch
            {
                MessageBox.Show("Por favor, complete la fila antes de eliminar", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv_Principal_CellLeave(object sender, DataGridViewCellEventArgs e)

        {
            dgv_Principal.EndEdit();
        }

        private void dgv_Principal_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            if (dgv_Principal.IsCurrentCellDirty)
            {
                dgv_Principal.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje;
            try
            {

                if (e.CloseReason == CloseReason.UserClosing)
                {
                    mensaje = Convert.ToString(MessageBox.Show("¿Esta seguro que desea salir?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information));
                    if (mensaje == "Yes")
                        Application.Exit();
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void TxtTelefonoClienteOcasional_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            // condicion que no permite dar salto de linea
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
            }
            // condicion que no permite utilizar letras en el campo
            else if (char.IsLetter(e.KeyChar))
            {
                e.Handled = true;

            }
            // condicion que no permite utilizar la tecla de borrado
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // condicion que nos permite utilizar los espacios
            else if (char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtTelefonoClienteOcasional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtDireccionClienteOcasional.Visible == true)
                {
                    txtDireccionClienteOcasional.Focus();
                }
                else
                {
                    cmbDirEntrega.Focus();
                }
            }
            e.Handled = true;
        }

        private void txtDireccionClienteOcasional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SelectNextControl((Control)sender, true, true, true, true);
                if ((dgv_Principal.Rows.Count == 0))
                {
                    dgv_Principal.Rows.Add();
                    dgv_Principal.CurrentCell = dgv_Principal.Rows[0].Cells["ColCodigo"];
                    dgv_Principal.BeginEdit(true);
                    precargado = false;
                }
                else
                {
                    dgv_Principal.CurrentCell = dgv_Principal.Rows[dgv_Principal.Rows.Count - 1].Cells["ColCodigo"];
                    dgv_Principal.BeginEdit(true);
                    dgv_Principal.CurrentCell.Selected = true;
                }

            }
            else if (e.KeyCode == Keys.Escape)
                TxtTelefonoClienteOcasional.Focus();
        }

        private void txtNombreClienteOcasional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTalonario.Focus();
            }
            e.Handled = true;
        }

        private void txtEmailClienteOcasional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtTelefonoClienteOcasional.Focus();
            }
            e.Handled = true;
        }

        private void FormPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (lblAltaClienteNuevo.Visible == true)
            {
                if (e.KeyCode == Keys.F6)
                {
                    lblAltaClienteNuevo.Visible = false;
                    FormAltaClienteNuevoGva14 altaDeClienteTago = new FormAltaClienteNuevoGva14();
                    altaDeClienteTago.ShowDialog();
                    if (Pedido.PedidoClienteRegistradoEnTango)
                    {
                        txtCliente.Text = altaDeClienteTago.CodigoCliente;
                        lblCliente.Text = altaDeClienteTago.RazonSocial;
                        CargarDatos(txtCliente.Text);
                        txtTalonario.Text = "0";
                        txtVendedor.Focus();
                        precargado = false;
                        lblAltaClienteNuevo.Visible = true;
                    }
                    else
                    {
                        lblAltaClienteNuevo.Visible = true;
                    }

                }
            }
        }

        private void cmbDirEntrega_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SelectNextControl((Control)sender, true, true, true, true);
                if ((dgv_Principal.Rows.Count == 0))
                {
                    dgv_Principal.Rows.Add();
                    dgv_Principal.CurrentCell = dgv_Principal.Rows[0].Cells["ColCodigo"];
                    dgv_Principal.BeginEdit(true);
                    precargado = false;
                }
                else
                {
                    dgv_Principal.CurrentCell = dgv_Principal.Rows[dgv_Principal.Rows.Count - 1].Cells["ColCodigo"];
                    dgv_Principal.BeginEdit(true);
                    dgv_Principal.CurrentCell.Selected = true;

                }
            }
        }


        #endregion


    }

}
