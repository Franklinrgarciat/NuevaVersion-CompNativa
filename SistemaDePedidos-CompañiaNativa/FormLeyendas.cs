using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormLeyendas : Form
    {
        public string sPedido;
        public bool bAcepto;
        public int iTalonario;
        public FormLeyendas()
        {
            InitializeComponent();
        }
        public int Talonario { get; set; }
        public string Pedido { get; set; }
        public bool Mostrar(CapaEntidades.Pedido oPedido, CapaNegocios.NegociosPedidos oPedidoConexion)
        {
            ShowDialog();
            oPedido.Leyenda1 = txtLeyenda1.Text;
            oPedido.Leyenda2 = txtLeyenda2.Text;
            oPedido.Leyenda3 = txtLeyenda3.Text;
            oPedido.Leyenda4 = txtLeyenda4.Text;
            return bAcepto;
        }

        private void txtComentario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda1.Focus();
        }
        private void txtLeyenda1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda2.Focus();
        }

        private void txtLeyenda2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda3.Focus();
        }

        private void txtLeyenda3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtLeyenda4.Focus();
        }


        private void cmdAceptar_Click(object sender, EventArgs e)
        {
            bAcepto = true;
            this.Close();
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            bAcepto = false;
            this.Close();
        }

        private void FormLeyendas_Load(object sender, EventArgs e)
        {
            txtLeyenda1.Select();
        }

        private void txtLeyenda4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                cmdAceptar.Focus();
        }
    }
}
