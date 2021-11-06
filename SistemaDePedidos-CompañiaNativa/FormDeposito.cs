using CapaNegocios;
using Microsoft.VisualBasic;
using SeinBuscador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormDeposito : Form
    {
        string CodArticulo;
        NegociosPedidos oNegociosPedidos = new NegociosPedidos();
        private static string Conexion = ConfigurationManager.AppSettings.Get("Conexion").ToString();
        public FormDeposito(string codigoArticulo)
        {
            this.CodArticulo = codigoArticulo;
            InitializeComponent();
        }

        private void FormDeposito_Load(object sender, EventArgs e)
        {
            txtDeposito.Focus();
        }

        private void txtDeposito_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                
                cBuscador oBuscador = new cBuscador();
                
                    if (e.KeyChar == Strings.ChrW(13))
                    {
                        e.Handled = true;
                        if (oNegociosPedidos.ComprobarDato("sta22", "cod_sucurs", true, txtDeposito.Text) == false)
                        {
                            oBuscador.AgregarColumnaFill(1);
                            oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                            oBuscador.Mostrar(Conexion, "SELECT COD_SUCURS, NOMBRE_SUC, ISNULL(FORMAT(STA19.CANT_STOCK, '0.00'),'NO EXISTE') AS [STOCK DISP] FROM STA22 LEFT JOIN STA19 "+
                            "ON STA19.COD_DEPOSI = STA22.COD_SUCURS AND STA19.COD_ARTICU='" + CodArticulo + "' WHERE INHABILITA=0 AND COD_SUCURS IN(" + ConfigurationManager.AppSettings.Get("CODIGO_DEPOSITO").ToString() + ")");
                            if (oBuscador.devolverDato("Codigo") != "")
                            {
                                txtDeposito.Text = oBuscador.devolverDato("Codigo");
                                lblDeposito.Text = oBuscador.devolverDato("Nombre");
                                decimal cantidad = Convert.ToDecimal(oNegociosPedidos.TraerDato(false, "STA19", "CANT_STOCK", false, "COD_DEPOSI", true, txtDeposito.Text + "' AND COD_ARTICU='" + CodArticulo));
                                //txtStockDisponible.Text = cantidad.ToString("N2");
                            }
                            else
                            {
                                txtDeposito.Text = "";
                                lblDeposito.Text = "";
                                txtStockDisponible.Text = "";
                                txtDeposito.Focus();
                            }
                        }
                        else
                        {
                            lblDeposito.Text = oNegociosPedidos.TraerDato(false, "sta22", "nombre_suc", true, "cod_sucurs", true, txtDeposito.Text.Trim());
                        }
                    }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void FormDeposito_Shown(object sender, EventArgs e)
        {
            txtDeposito.Focus();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            if (txtDeposito.Text.Length == 0 && lblDeposito.Text.Length == 0)
            {
                this.Close();
            }
            else
            {
                txtDeposito.Text = "";
                lblDeposito.Text = "";
                txtStockDisponible.Text = "";
            }
        }
    }
}
