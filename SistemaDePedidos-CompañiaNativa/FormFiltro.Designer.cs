
namespace SistemaDePedidos_CompañiaNativa
{
    partial class FormFiltro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFiltro));
            this.btnHastaPedidido = new System.Windows.Forms.Button();
            this.btnDesdePedido = new System.Windows.Forms.Button();
            this.txtHastaPedido = new System.Windows.Forms.TextBox();
            this.txtDesdePedido = new System.Windows.Forms.TextBox();
            this.lblDesde = new System.Windows.Forms.Label();
            this.gPedido = new System.Windows.Forms.GroupBox();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.Label2 = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.Label1 = new System.Windows.Forms.Label();
            this.chkFecha = new System.Windows.Forms.CheckBox();
            this.chkNegra = new System.Windows.Forms.CheckBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.gFecha = new System.Windows.Forms.GroupBox();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.gPedido.SuspendLayout();
            this.gFecha.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHastaPedidido
            // 
            this.btnHastaPedidido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHastaPedidido.Image = ((System.Drawing.Image)(resources.GetObject("btnHastaPedidido.Image")));
            this.btnHastaPedidido.Location = new System.Drawing.Point(274, 38);
            this.btnHastaPedidido.Name = "btnHastaPedidido";
            this.btnHastaPedidido.Size = new System.Drawing.Size(29, 20);
            this.btnHastaPedidido.TabIndex = 5;
            this.btnHastaPedidido.UseVisualStyleBackColor = true;
            this.btnHastaPedidido.Click += new System.EventHandler(this.btnHastaPedidido_Click);
            // 
            // btnDesdePedido
            // 
            this.btnDesdePedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesdePedido.Image = ((System.Drawing.Image)(resources.GetObject("btnDesdePedido.Image")));
            this.btnDesdePedido.Location = new System.Drawing.Point(107, 38);
            this.btnDesdePedido.Name = "btnDesdePedido";
            this.btnDesdePedido.Size = new System.Drawing.Size(29, 20);
            this.btnDesdePedido.TabIndex = 4;
            this.btnDesdePedido.UseVisualStyleBackColor = true;
            this.btnDesdePedido.Click += new System.EventHandler(this.btnDesdePedido_Click);
            // 
            // txtHastaPedido
            // 
            this.txtHastaPedido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHastaPedido.Location = new System.Drawing.Point(173, 38);
            this.txtHastaPedido.Name = "txtHastaPedido";
            this.txtHastaPedido.Size = new System.Drawing.Size(104, 20);
            this.txtHastaPedido.TabIndex = 1;
            this.txtHastaPedido.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHastaPedido_KeyDown);
            // 
            // txtDesdePedido
            // 
            this.txtDesdePedido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesdePedido.Location = new System.Drawing.Point(6, 38);
            this.txtDesdePedido.Name = "txtDesdePedido";
            this.txtDesdePedido.Size = new System.Drawing.Size(103, 20);
            this.txtDesdePedido.TabIndex = 0;
            this.txtDesdePedido.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDesdePedido_KeyDown);
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(6, 22);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(38, 13);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Text = "Desde";
            // 
            // gPedido
            // 
            this.gPedido.Controls.Add(this.btnHastaPedidido);
            this.gPedido.Controls.Add(this.btnDesdePedido);
            this.gPedido.Controls.Add(this.txtHastaPedido);
            this.gPedido.Controls.Add(this.txtDesdePedido);
            this.gPedido.Controls.Add(this.lblHasta);
            this.gPedido.Controls.Add(this.lblDesde);
            this.gPedido.Location = new System.Drawing.Point(12, 12);
            this.gPedido.Name = "gPedido";
            this.gPedido.Size = new System.Drawing.Size(309, 80);
            this.gPedido.TabIndex = 0;
            this.gPedido.TabStop = false;
            this.gPedido.Text = "Pedido Nº";
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(170, 22);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(35, 13);
            this.lblHasta.TabIndex = 1;
            this.lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(173, 38);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(130, 20);
            this.dtpHasta.TabIndex = 1;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(170, 22);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(35, 13);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "Hasta";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(9, 38);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(127, 20);
            this.dtpDesde.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 22);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(38, 13);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "Desde";
            // 
            // chkFecha
            // 
            this.chkFecha.AutoSize = true;
            this.chkFecha.Location = new System.Drawing.Point(18, 115);
            this.chkFecha.Name = "chkFecha";
            this.chkFecha.Size = new System.Drawing.Size(15, 14);
            this.chkFecha.TabIndex = 3;
            this.chkFecha.UseVisualStyleBackColor = true;
            this.chkFecha.CheckedChanged += new System.EventHandler(this.chkFecha_CheckedChanged);
            // 
            // chkNegra
            // 
            this.chkNegra.AutoSize = true;
            this.chkNegra.Location = new System.Drawing.Point(306, 98);
            this.chkNegra.Name = "chkNegra";
            this.chkNegra.Size = new System.Drawing.Size(15, 14);
            this.chkNegra.TabIndex = 2;
            this.chkNegra.UseVisualStyleBackColor = true;
            this.chkNegra.CheckedChanged += new System.EventHandler(this.chkNegra_CheckedChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Location = new System.Drawing.Point(256, 201);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(65, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Location = new System.Drawing.Point(185, 201);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(65, 23);
            this.btnImprimir.TabIndex = 0;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
//            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // gFecha
            // 
            this.gFecha.Controls.Add(this.dtpHasta);
            this.gFecha.Controls.Add(this.Label2);
            this.gFecha.Controls.Add(this.dtpDesde);
            this.gFecha.Controls.Add(this.Label1);
            this.gFecha.Enabled = false;
            this.gFecha.Location = new System.Drawing.Point(12, 115);
            this.gFecha.Name = "gFecha";
            this.gFecha.Size = new System.Drawing.Size(309, 80);
            this.gFecha.TabIndex = 1;
            this.gFecha.TabStop = false;
            this.gFecha.Text = "     Fecha";
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.chkFecha);
            this.Panel1.Controls.Add(this.chkNegra);
            this.Panel1.Controls.Add(this.btnCancelar);
            this.Panel1.Controls.Add(this.btnImprimir);
            this.Panel1.Controls.Add(this.gFecha);
            this.Panel1.Controls.Add(this.gPedido);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(330, 236);
            this.Panel1.TabIndex = 1;
            // 
            // FormFiltro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(330, 236);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFiltro";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "| Filtrar";
            this.Load += new System.EventHandler(this.FormFiltro_Load);
            this.gPedido.ResumeLayout(false);
            this.gPedido.PerformLayout();
            this.gFecha.ResumeLayout(false);
            this.gFecha.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnHastaPedidido;
        internal System.Windows.Forms.Button btnDesdePedido;
        internal System.Windows.Forms.TextBox txtHastaPedido;
        internal System.Windows.Forms.TextBox txtDesdePedido;
        internal System.Windows.Forms.Label lblDesde;
        internal System.Windows.Forms.GroupBox gPedido;
        internal System.Windows.Forms.Label lblHasta;
        internal System.Windows.Forms.DateTimePicker dtpHasta;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.DateTimePicker dtpDesde;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckBox chkFecha;
        internal System.Windows.Forms.CheckBox chkNegra;
        internal System.Windows.Forms.Button btnCancelar;
        internal System.Windows.Forms.Button btnImprimir;
        internal System.Windows.Forms.GroupBox gFecha;
        internal System.Windows.Forms.Panel Panel1;
    }
}