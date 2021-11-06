using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocios;
using System.Configuration;
using SeinBuscador;

namespace SistemaDePedidos_CompañiaNativa
{

    public partial class FormFiltro : Form
    {
        CapaNegocios.NegociosPedidos oNegociosPedido = new NegociosPedidos();
        Reporte.Reporte oReporte = new Reporte.Reporte();
        private string Conexion;
        private string Conexion2;
        private string sConnDef;
        private int iTalonarioDef;
        public FormFiltro(string conexion, string conexion2)
        {
            InitializeComponent();
            Conexion = conexion;
            Conexion2 = conexion2;
        }

        private void FormFiltro_Load(object sender, EventArgs e)
        {
            if (oNegociosPedido.Conectar(Conexion, Conexion2))
            {
                Text = ConfigurationManager.AppSettings.Get("TITULO") + " " + Text;
            }
            sConnDef = Conexion;
            iTalonarioDef = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario"));
            txtDesdePedido.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDesdePedido_Click(object sender, EventArgs e)
        {
            cBuscador oBuscador = new cBuscador();
            if (oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtDesdePedido.Text) == "")
            {
                oBuscador.AgregarColumnaFill(0);
                oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;

                oBuscador.Mostrar(sConnDef, "SELECT  NRO_PEDIDO, TALON_PED, FECHA_PEDI  FROM GVA21 ORDER BY 3");

                if (oBuscador.devolverDato("NRO_PEDIDO") != "")
                {
                    txtDesdePedido.Text = oBuscador.devolverDato("NRO_PEDIDO");
                    txtHastaPedido.Focus();
                }
            }
            else
            {
                txtDesdePedido.Text = oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtDesdePedido.Text);
                txtHastaPedido.Focus();
            }
        }

        private void btnHastaPedidido_Click(object sender, EventArgs e)
        {
            cBuscador oBuscador = new cBuscador();
            if (oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtHastaPedido.Text) == "")
            {
                oBuscador.AgregarColumnaFill(0);
                oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                oBuscador.Mostrar(sConnDef, "SELECT  NRO_PEDIDO, TALON_PED, FECHA_PEDI  FROM GVA21 ORDER BY 3");
                if (oBuscador.devolverDato("NRO_PEDIDO") != "")
                    txtHastaPedido.Text = oBuscador.devolverDato("NRO_PEDIDO");
            }
            else
            {
                txtHastaPedido.Text = oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtHastaPedido.Text);
            }
        }

        private void txtDesdePedido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cBuscador oBuscador = new cBuscador();
                if (oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "nro_pedido", true, "nro_pedido", true, txtDesdePedido.Text) == "")
                {
                    oBuscador.AgregarColumnaFill(0);
                    oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                    oBuscador.Mostrar(sConnDef, "SELECT  NRO_PEDIDO, TALON_PED, FECHA_PEDI  FROM GVA21 ORDER BY 3");
                    if (oBuscador.devolverDato("NRO_PEDIDO") != "")
                    {
                        txtDesdePedido.Text = oBuscador.devolverDato("NRO_PEDIDO");
                        txtHastaPedido.Focus();
                    }
                }
                else
                {
                    txtDesdePedido.Text = oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtDesdePedido.Text);
                    txtHastaPedido.Focus();
                }
            }
        }

        private void txtHastaPedido_KeyDown(object sender, KeyEventArgs e)
        {
            cBuscador oBuscador = new cBuscador();
            if (oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "nro_pedido", true, "nro_pedido", true, txtHastaPedido.Text) == "")
            {
                oBuscador.AgregarColumnaFill(0);
                oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                oBuscador.Mostrar(sConnDef, "SELECT  NRO_PEDIDO, TALON_PED, FECHA_PEDI  FROM GVA21 ORDER BY 3");
                if (oBuscador.devolverDato("NRO_PEDIDO") != "")
                    txtHastaPedido.Text = oBuscador.devolverDato("NRO_PEDIDO");
            }
            else
            {
                txtHastaPedido.Text = oNegociosPedido.TraerDato(chkNegra.Checked, "GVA21", "NRO_PEDIDO", true, "NRO_PEDIDO", true, txtHastaPedido.Text);
            }
        }

        //private void btnImprimir_Click(object sender, EventArgs e)
        //{
        //    bool Continuar = true;
        //    bool UsaFechas = false;



        //    // Valido si el check de las fechas esta tildado
        //    if (chkFecha.Checked)
        //    {
        //        if (new DateTime(dtpDesde.Value.Year, dtpDesde.Value.Month, dtpDesde.Value.Day) > new DateTime(dtpHasta.Value.Year, dtpHasta.Value.Month, dtpHasta.Value.Day))
        //        {
        //            MessageBox.Show("La fecha de inicio tiene que ser menor o igual a la fecha de fin", "Fechas incorrectas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            Continuar = false;
        //        }
        //        UsaFechas = true;
        //    }
        //    // validar que el pedido sea mayor, igual en las fechas
        //    if (Continuar)
        //    {
        //        if (UsaFechas)
        //            oReporte.imprimirPedido(txtDesdePedido.Text, txtHastaPedido.Text, iTalonarioDef.ToString(), sConnDef, chkNegra.Checked, true, dtpDesde.Value, dtpHasta.Value);
        //        else if (txtHastaPedido.Text.Trim() != "" && txtDesdePedido.Text.Trim() != "")
        //            oReporte.imprimirPedido(txtDesdePedido.Text, txtHastaPedido.Text, iTalonarioDef.ToString(), sConnDef, chkNegra.Checked, false);
        //        else if (txtDesdePedido.Text.Trim() != "")
        //            oReporte.imprimirPedido(txtDesdePedido.Text, txtDesdePedido.Text, iTalonarioDef.ToString(), sConnDef, chkNegra.Checked, false);
        //        else if (txtHastaPedido.Text.Trim() != "")
        //            oReporte.imprimirPedido(txtHastaPedido.Text, txtHastaPedido.Text, iTalonarioDef.ToString(), sConnDef, chkNegra.Checked, false);
        //    }
        //}

        private void chkFecha_CheckedChanged(object sender, EventArgs e)
        {
            gFecha.Enabled = chkFecha.Checked;
            gPedido.Enabled = !chkFecha.Checked;
            txtDesdePedido.Focus();
        }

        private void chkNegra_CheckedChanged(object sender, EventArgs e)
        {
            txtDesdePedido.Text = "";
            txtHastaPedido.Text = "";
            if (chkNegra.Checked == false)
            {
                sConnDef = Conexion;
                iTalonarioDef = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario").ToString());
            }
            else
            {
                sConnDef = Conexion2;
                iTalonarioDef = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Talonario2").ToString());
            }
            txtDesdePedido.Focus();
        }
    }
}
