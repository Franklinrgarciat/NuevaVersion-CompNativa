using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormSeña : Form
    {
        public string sPedido;
        public bool bAcepto;
        public int iTalonario;
        public FormSeña()
        {
            InitializeComponent();
        }
        public int Talonario { get; set; }
        public string Pedido { get; set; }
        public string TotalPedido;
        string señaPedido = "";
        decimal importe;
        string importeSeña;
        public bool Mostrar(CapaEntidades.Pedido oPedido, CapaNegocios.NegociosPedidos oPedidoConexion)
        {
            importeSeña = oPedido.ImporteSeña;
            señaPedido = oPedidoConexion.TraerDato(false, "gva21", "NRO_O_COMP", true, "talon_ped=" + iTalonario + " AND nro_pedido", true, sPedido);
            if (señaPedido != "")
            {
                txtImporteSeña.Text = señaPedido;
            }
            ShowDialog();
            if (txtImporteSeña.Text != "0")
            {
                importe = Convert.ToDecimal(txtImporteSeña.Text);
                if (bAcepto)
                {
                    oPedido.ImporteSeña = importe.ToString();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El importe de seña que ingresó supera el total del pedido.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bAcepto = false;
                    txtImporteSeña.Select();
                }
            }
            else
            {
                oPedido.ImporteSeña = "0";
            }
            
            return bAcepto;
        }

        private void FormSeña_Load(object sender, EventArgs e)
        {
            txtImporteSeña.Select();
        }

        private void txtImporteSeña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.Focus();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
                bAcepto = true;
                this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bAcepto = false;
            this.Close();
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
    }
}
