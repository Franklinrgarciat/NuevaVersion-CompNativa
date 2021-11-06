using CrystalDecisions.CrystalReports.Engine;
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

namespace Reporte
{
    public partial class FrmReport : Form
    {
        public FrmReport()
        {
            InitializeComponent();
        }

        public ReportDocument ReportSource { get; internal set; }

        private void FormReporte_Load(object sender, EventArgs e)
        {
            Text = ConfigurationManager.AppSettings.Get("TITULO") + " " + Text;
        }
    }
}
