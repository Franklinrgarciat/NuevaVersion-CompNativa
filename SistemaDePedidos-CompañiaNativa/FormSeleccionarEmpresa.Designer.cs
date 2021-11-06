
namespace SistemaDePedidos_CompañiaNativa
{
    partial class FormSeleccionarEmpresa
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
            this.btn_empresa2 = new System.Windows.Forms.Button();
            this.btn_empresa1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_empresa2
            // 
            this.btn_empresa2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_empresa2.Location = new System.Drawing.Point(55, 128);
            this.btn_empresa2.Name = "btn_empresa2";
            this.btn_empresa2.Size = new System.Drawing.Size(194, 51);
            this.btn_empresa2.TabIndex = 3;
            this.btn_empresa2.Text = "EMPRESA 2";
            this.btn_empresa2.UseVisualStyleBackColor = true;
            this.btn_empresa2.Click += new System.EventHandler(this.btn_empresa2_Click);
            // 
            // btn_empresa1
            // 
            this.btn_empresa1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_empresa1.Location = new System.Drawing.Point(55, 45);
            this.btn_empresa1.Name = "btn_empresa1";
            this.btn_empresa1.Size = new System.Drawing.Size(194, 51);
            this.btn_empresa1.TabIndex = 2;
            this.btn_empresa1.Text = "EMPRESA 1";
            this.btn_empresa1.UseVisualStyleBackColor = true;
            this.btn_empresa1.Click += new System.EventHandler(this.btn_empresa1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_empresa2);
            this.groupBox1.Controls.Add(this.btn_empresa1);
            this.groupBox1.Location = new System.Drawing.Point(27, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 219);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selecione una opción para continuar";
            // 
            // FormSeleccionarEmpresa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(356, 279);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSeleccionarEmpresa";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "| Seleccionar Empresa";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btn_empresa2;
        internal System.Windows.Forms.Button btn_empresa1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}