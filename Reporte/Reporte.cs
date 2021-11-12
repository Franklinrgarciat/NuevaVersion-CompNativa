using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CrystalDecisions.CrystalReports.Engine;

using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace Reporte
{
    public class Reporte
    {
        private static CrystalDecisions.Shared.ConnectionInfo loginfo;
        private static string[] nombreTabla = new string[16];
        public void conectar(string servidor, string @base, string usuario, string password)
        {
            loginfo = new CrystalDecisions.Shared.ConnectionInfo();
            loginfo.ServerName = servidor;
            loginfo.DatabaseName = @base;
            loginfo.UserID = usuario;
            loginfo.Password = password;
        }
        public void mostrarRPT(string tituloReporte, string archivoReporte, DataSet ds)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            FrmReport oViewer = new FrmReport();
            ReportDocument rpt = new ReportDocument();
            Tables tablas;

            try
            {
                rpt.Load(archivoReporte, OpenReportMethod.OpenReportByDefault);
                tablas = loginRPT(ref rpt);
                rpt.SetDataSource(ds.Tables[0]);
                oViewer.crv_viewer.ReportSource = rpt;
                PrintOptions prn;
                prn = rpt.PrintOptions;

                oViewer.Text = tituloReporte;
                // oViewer.Viewer.Zoom(2)
                oViewer.Show();
                oViewer.crv_viewer.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
        private static Tables loginRPT(ref ReportDocument reporte)
        {
            try
            {
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;
                crConnectionInfo = loginfo;
                CrTables = reporte.Database.Tables;
                int x = 1;
                foreach (Table CrTable in CrTables)
                {
                    nombreTabla[x] = CrTable.Name;
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    nombreTabla[0] = x.ToString();
                    x = x + 1;
                }
                return CrTables;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void imprimirPedido(string numeroPedido, string talonarioPedido, string sConnStr)
        {
            var oReporte = new Reporte();
            System.Data.SqlClient.SqlDataAdapter Adap;
            var oSet = new DataSet();
            string SQL = "";
            try
            {
                SQL = @"
                    SELECT
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN gva21.COD_CLIENT + ' - '+ c.razon_soci collate Latin1_General_BIN 
	                    ELSE gva14.cod_client+ ' - '+ gva14.razon_soci collate Latin1_General_BIN END as CLIENTE,
	                    gva21.fecha_pedi, 
	                    (gva23.cod_vended + ' - ' + gva23.nombre_ven collate Latin1_General_BIN) AS VENDEDOR, 
	                    (STA22.cod_sucurs  + ' - ' + sta22.nombre_suc collate Latin1_General_BIN) as deposito, 
	                    gva21.fecha_Entr, 
	                    ISNULL(sta11.cod_Articu, '') AS cod_Articu, 
	                    ISNULL(STA11.descripcio  + sta11.desc_adic collate Latin1_General_BIN, 
	                    ISNULL(GVA45.[DESC] + gva45.desc_adic, '')) AS ARTICULO,  
	                    GVA03.CANT_PEDID / CAN_EQUI_V AS CANT_PEDID, 
	                    GVA03.PRECIO, 
	                    gva21.leyenda_1,
	                    gva21.leyenda_2,
	                    gva21.leyenda_3,
	                    gva21.leyenda_4,
	                    gva21.leyenda_5, 
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN c.DIRECCION_ENTREGA ELSE DIRECCION_ENTREGA.DIRECCION END AS DIRECCION, 
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN c.LOCALIDAD ELSE DIRECCION_ENTREGA.LOCALIDAD END AS LOCALIDAD,
	                    GVA03.PRECIO - GVA03.PRECIO_BONIF AS DESCUENTO,
	                    GVA21.FECHA_ENTR,
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN c.E_MAIL ELSE GVA14.E_MAIL END AS E_MAIL,
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN c.TELEFONO_1 ELSE GVA14.TELEFONO_1 END AS TELEFONO_1,
	                    CASE WHEN GVA14.COD_CLIENT IS NULL THEN c.N_CUIT ELSE GVA14.CUIT END AS CUIT,
	                    gva18.NOMBRE_PRO,
	                    NUMEROS_PEDIDOS_SISTEMA_VENTAS.PRIMER_PEDIDO,
	                    NUMEROS_PEDIDOS_SISTEMA_VENTAS.SEGUNDO_PEDIDO,
	                    NUMEROS_PEDIDOS_SISTEMA_VENTAS.TERCER_PEDIDO,
	                    NUMEROS_PEDIDOS_SISTEMA_VENTAS.SEÑA,
	                    NUMEROS_PEDIDOS_SISTEMA_VENTAS.TOTAL_IMPORTE_PEDIDO
                    FROM NUMEROS_PEDIDOS_SISTEMA_VENTAS 
                    INNER JOIN GVA21 ON 
	                    GVA21.NRO_PEDIDO = NUMEROS_PEDIDOS_SISTEMA_VENTAS.PRIMER_PEDIDO COLLATE LATIN1_GENERAL_BIN AND 
	                    GVA21.TALON_PED = 23
                    LEFT JOIN GVA21 GVA21_2 ON 
	                    GVA21_2.NRO_PEDIDO = NUMEROS_PEDIDOS_SISTEMA_VENTAS.SEGUNDO_PEDIDO COLLATE LATIN1_GENERAL_BIN AND 
	                    GVA21_2.TALON_PED = 23
                    LEFT JOIN GVA21 GVA21_3 ON 
	                    GVA21_3.NRO_PEDIDO = NUMEROS_PEDIDOS_SISTEMA_VENTAS.TERCER_PEDIDO COLLATE LATIN1_GENERAL_BIN AND 
	                    GVA21_3.TALON_PED = 23
                    LEFT JOIN gva14 ON gva14.cod_client=gva21.cod_Client 
                    INNER JOIN 
	                    (
		                    SELECT
			                    GVA03.*,
			                    GVA21.COD_SUCURS
		                    FROM GVA03
		                    INNER JOIN GVA21 ON 
			                    GVA21.NRO_PEDIDO = GVA03.NRO_PEDIDO AND
			                    GVA21.TALON_PED = GVA03.TALON_PED
	                    ) GVA03 ON
	                    GVA03.TALON_PED = GVA21.TALON_PED AND
	                    (
		                    GVA03.NRO_PEDIDO = GVA21.NRO_PEDIDO OR
		                    GVA03.NRO_PEDIDO = GVA21_2.NRO_PEDIDO OR
		                    GVA03.NRO_PEDIDO = GVA21_3.NRO_PEDIDO 
	                    )
                    LEFT JOIN sta11 ON sta11.cod_articu=gva03.cod_articu 
                    LEFT JOIN GVA24 ON GVA24.COD_TRANSP=GVA21.COD_TRANSP 
                    LEFT JOIN GVA23 ON GVA23.COD_VENDED=GVA21.COD_VENDED 
                    LEFT JOIN STA22 ON STA22.COD_SUCURS=GVA03.COD_SUCURS 
                    LEFT JOIN DIRECCION_ENTREGA ON DIRECCION_ENTREGA.ID_DIRECCION_ENTREGA = GVA21.ID_DIRECCION_ENTREGA 
                    LEFT JOIN GVA41 ON STA11.COD_IVA=GVA41.COD_ALICUO 
                    LEFT JOIN GVA45 ON GVA21.NRO_PEDIDO=GVA45.N_COMP AND GVA03.N_RENGLON=GVA45.N_RENGLON AND GVA21.TALON_PED=GVA45.TALONARIO AND T_COMP='PED'
                    left join gva38 c on c.TALONARIO = gva21.TALON_PED and c.T_COMP = 'PED' and c.N_COMP = gva21.NRO_PEDIDO
                    LEFT JOIN GVA18 ON GVA14.COD_PROVIN=GVA18.COD_PROVIN
                    WHERE gva21.nro_pedido= @NumeroPedido AND GVA21.TALON_PED=@TalonarioPedido
                ";
                Adap = new SqlDataAdapter(SQL, sConnStr);
                Adap.SelectCommand.Parameters.Add("@NumeroPedido", SqlDbType.VarChar).Value = numeroPedido;
                Adap.SelectCommand.Parameters.Add("@TalonarioPedido", SqlDbType.VarChar).Value = talonarioPedido;
                Adap.Fill(oSet);

                conectar(Globales.Global.ServidorA, Globales.Global.BaseA, Globales.Global.usuarioA, Globales.Global.passA);
                mostrarRPT("| Pedido", Application.StartupPath + @"\Reportes\rptNuevo.rpt", oSet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
