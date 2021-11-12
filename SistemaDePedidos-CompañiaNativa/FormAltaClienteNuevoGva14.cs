using Microsoft.VisualBasic;
using SeinBuscador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDePedidos_CompañiaNativa
{
    public partial class FormAltaClienteNuevoGva14 : Form
    {
        CapaNegocios.NegociosPedidos oNegocioPedido = new CapaNegocios.NegociosPedidos();
        private string Conexion = ConfigurationManager.AppSettings.Get("Conexion").ToString();
        public string CodigoCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Domicilio { get; set; }
        public string CodigoProvincia { get; set; }
        public string Provincia { get; set; }
        public FormAltaClienteNuevoGva14()
        {
            InitializeComponent();
        }

        private void btn_cancelarRegistro_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            this.Close();
        }

        private void FormAltaClienteNuevoGva14_Load(object sender, EventArgs e)
        {
            txtCodProvincia.ReadOnly = true;
            btn_GuardarRegistro.Enabled = false;
            txtProvinciaCliente.ReadOnly = true;
            string codigoCliente = "C" + oNegocioPedido.TraerDato(false, "GVA14", "ISNULL(MAX(SUBSTRING(COD_CLIENT,2,5)),0) + 1", false, "ISNUMERIC(SUBSTRING(COD_CLIENT,2,5)) = 1 AND SUBSTRING(COD_CLIENT,1,1)", true, "C").PadLeft(5, char.Parse("0"));
            if (codigoCliente != "")
            {
                txt_codigoCliente.Text = codigoCliente;
                txtNombreCliente.Select();
            }
            else
            {
                MessageBox.Show("Se genero un error al generar el código del cliente, vuelva a intertarlo", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void btn_GuardarRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombreCliente.Text != "" && txt_dniCliente.Text != "" && txtTelefonoCliente.Text != "" && txtEmailCliente.Text != "" && txtCondicionVentaCliente.Text != ""
                && txtDireccionCliente.Text != "" && txtProvinciaCliente.Text != "" && txtCodProvincia.Text != "" && txtLocalidadCliente.Text != "" && txt_codigoCliente.Text != "")
                {
                    CapaEntidades.ClienteGva14 clienteNuevo = new CapaEntidades.ClienteGva14();
                    clienteNuevo.COD_CLIENT = txt_codigoCliente.Text;
                    clienteNuevo.RAZON_SOCI = txtNombreCliente.Text;
                    clienteNuevo.TELEFONO_1 = txtTelefonoCliente.Text;
                    clienteNuevo.CUIT = txt_dniCliente.Text;
                    clienteNuevo.E_MAIL = txtEmailCliente.Text;
                    clienteNuevo.COD_PROVIN = CodigoProvincia;
                    clienteNuevo.LOCALIDAD = txtLocalidadCliente.Text;
                    clienteNuevo.DOMICILIO = txtDireccionCliente.Text;
                    clienteNuevo.COND_VTA = Convert.ToInt32(txtCondicionVentaCliente.Text);
                    clienteNuevo.COD_PROVIN = txtCodProvincia.Text;

                    if (ValidarFormatoEmail(txtEmailCliente.Text))
                    {
                        if (oNegocioPedido.GuardarClienteGva14(clienteNuevo))
                        {
                            CodigoCliente = clienteNuevo.COD_CLIENT;
                            RazonSocial = clienteNuevo.RAZON_SOCI;
                            Domicilio = clienteNuevo.DOMICILIO;
                            MessageBox.Show($"El registro del cliente {clienteNuevo.COD_CLIENT} se completo correctamente.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CapaEntidades.Pedido.PedidoClienteRegistradoEnTango = true;
                            LimpiarControles();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error!! No se completó el registro, verifique los datos.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CapaEntidades.Pedido.PedidoClienteRegistradoEnTango = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error!! El campo Email, no tiene el formato correcto.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }
                else
                {
                    MessageBox.Show("Complete los campos que se encuentran vacios para continuar...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void LimpiarControles()
        {
            txt_codigoCliente.Text = "";
            txtNombreCliente.Text = "";
            txt_dniCliente.Text = "";
            txtTelefonoCliente.Text = "";
            txtEmailCliente.Text = "";
            txtCondicionVentaCliente.Text = "";
            txtlabelCondicionVentaCliente.Text = "";
            txtDireccionCliente.Text = "";
            txtProvinciaCliente.Text = "";
            txtLocalidadCliente.Text = "";
        }

        public bool ValidarFormatoEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        private void txtNombreCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_dniCliente.Focus();
            }
            e.Handled = true;
        }

        private void txt_dniCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEmailCliente.Focus();
            }
            e.Handled = true;
        }

        private void txtTelefonoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDireccionCliente.Focus();
            }
            e.Handled = true;
        }

        private void txtEmailCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTelefonoCliente.Focus();
            }
            e.Handled = true;
        }

        private void txtProvinciaCliente_KeyDown(object sender, KeyEventArgs e)
        {
            btn_GuardarRegistro.Enabled = true;
            if (e.KeyCode == Keys.Enter)
            {
                btn_GuardarRegistro.Select();
            }
            e.Handled = true;
        }

        private void txtDireccionCliente_KeyDown(object sender, KeyEventArgs e)
        {
            btn_GuardarRegistro.Enabled = true;
            if (e.KeyCode == Keys.Enter)
            {
                txtLocalidadCliente.Focus();
            }
            e.Handled = true;
        }

        private void txtLocalidadCliente_KeyDown(object sender, KeyEventArgs e)
        {
            btn_GuardarRegistro.Enabled = true;
            if (e.KeyCode == Keys.Enter)
            {
                txtCodProvincia.Focus();
            }
            e.Handled = true;
        }

        private void txt_dniCliente_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTelefonoCliente_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCodProvincia_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                cBuscador oBuscador = new cBuscador();

                if (e.KeyChar == Strings.ChrW(13))
                {
                    e.Handled = true;
                    if (oNegocioPedido.ComprobarDato("gva18", "cod_provin", true, txtCodProvincia.Text) == false)
                    {
                        oBuscador.AgregarColumnaFill(1);
                        oBuscador.Filtrado = cBuscador.MiFiltrado.Inicio;
                        oBuscador.Mostrar(Conexion, "SELECT COD_PROVIN as Codigo,NOMBRE_PRO as Nombre FROM GVA18");
                        if (oBuscador.devolverDato("Codigo") != "")
                        {
                            txtCodProvincia.Text = oBuscador.devolverDato("Codigo");
                            txtProvinciaCliente.Text = oBuscador.devolverDato("Nombre");
                            txtLocalidadCliente.Focus();
                        }
                        else
                        {
                            txtCodProvincia.Text = "";
                            txtProvinciaCliente.Text = "";
                            txtLocalidadCliente.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
