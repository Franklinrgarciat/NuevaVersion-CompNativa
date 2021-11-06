
namespace Reporte
{
    partial class FrmReport
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
            this.crv_viewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crv_viewer
            // 
            this.crv_viewer.ActiveViewIndex = -1;
            this.crv_viewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crv_viewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.crv_viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crv_viewer.Location = new System.Drawing.Point(0, 0);
            this.crv_viewer.Name = "crv_viewer";
            this.crv_viewer.ShowCloseButton = false;
            this.crv_viewer.ShowCopyButton = false;
            this.crv_viewer.ShowGroupTreeButton = false;
            this.crv_viewer.ShowLogo = false;
            this.crv_viewer.ShowRefreshButton = false;
            this.crv_viewer.ShowTextSearchButton = false;
            this.crv_viewer.Size = new System.Drawing.Size(909, 714);
            this.crv_viewer.TabIndex = 0;
            this.crv_viewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // FrmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 714);
            this.Controls.Add(this.crv_viewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "| Vista de reporte";
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crv_viewer;
    }
}