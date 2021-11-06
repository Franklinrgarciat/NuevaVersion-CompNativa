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
using CapaEntidades;



namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormSeleccionarEmpresa : Form
    {
        public FormSeleccionarEmpresa()
        {
            InitializeComponent();
        }

        private void btn_empresa1_Click(object sender, EventArgs e)
        {
            Pedido.Empresa = ConfigurationManager.AppSettings.Get("Empresa1");
            //Globales.Global.Empresa = "Empresa1";
            Close();
        }

        private void btn_empresa2_Click(object sender, EventArgs e)
        {
            Pedido.Empresa = ConfigurationManager.AppSettings.Get("Empresa2");
            Close();
        }
    }
}
