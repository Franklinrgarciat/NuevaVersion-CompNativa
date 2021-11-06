using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CapaEntidades;
using System.Transactions;
using System.Windows.Forms;

namespace CapaDatos
{
    public class GuardoEnBaseDeDatos
    {
        private string Conexion = ConfigurationManager.AppSettings.Get("Conexion").ToString();
        private string Conexion2 = ConfigurationManager.AppSettings.Get("Conexion2").ToString();
        private SqlDataAdapter da;
        private DataSet ds = new DataSet();
        private Decimal equivalencia;
        private int iRegistros, iLugar;

        public int Talonario { get; set; }
        public void primero()
        {
            if (iRegistros > 0)
                iLugar = 0;
            else
                iLugar = -1;
        }
        public void Anterior()
        {
            if (iLugar - 1 > -1)
                iLugar = iLugar - 1;
        }
        public void Siguiente()
        {
            if (iLugar + 1 < iRegistros)
                iLugar = iLugar + 1;
        }
        public void Ultimo()
        {
            iLugar = iRegistros - 1;
        }
        public int Registros
        {
            get
            {
                return (iRegistros);
            }
        }

        public void BuscarPedido(string sNumeroPedido, int iTalonario)
        {
            try
            {
                int x = 0;
                bool bEncontro = false;
                while (x <= ds.Tables[0].Rows.Count - 1 && bEncontro == false)
                {
                    if (ds.Tables[0].Rows[x]["nro_pedido"].ToString() == sNumeroPedido)
                        bEncontro = true;
                    x = x + 1;
                }
                if (bEncontro == true)
                    iLugar = x - 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool ComprobarExistePedidoEnBase2(string sTabla, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            SqlConnection cnn = new SqlConnection();
            bool bEncontro;
            string sSQL;
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;

            cnn.ConnectionString = Conexion2;

            try
            {
                cnn.Open();
                if (bTexto == false)
                    sSQL = "SELECT 1 FROM " + sTabla + " WHERE " + sCampoABuscar + "=" + sValorAComparar;
                else
                    sSQL = "SELECT 1 FROM " + sTabla + " WHERE " + sCampoABuscar + "='" + sValorAComparar + "'";
                com.Connection = cnn;
                com.CommandText = sSQL;

                dr = com.ExecuteReader();
                if (dr.Read() == true)
                    bEncontro = true;
                else
                    bEncontro = false;
            }
            catch
            {
                bEncontro = false;
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bEncontro;
        }

        public bool ComprobarDato(string sTabla, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            SqlConnection cnn = new SqlConnection();
            bool bEncontro;
            string sSQL;
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;

            cnn.ConnectionString = Conexion;


            try
            {
                cnn.Open();
                if (bTexto == false)
                    sSQL = "SELECT 1 FROM " + sTabla + " WHERE " + sCampoABuscar + "=" + sValorAComparar;
                else
                    sSQL = "SELECT 1 FROM " + sTabla + " WHERE " + sCampoABuscar + "='" + sValorAComparar + "'";
                com.Connection = cnn;
                com.CommandText = sSQL;
                dr = com.ExecuteReader();
                if (dr.Read() == true)
                    bEncontro = true;
                else
                    bEncontro = false;
            }
            catch
            {
                bEncontro = false;
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bEncontro;
        }

        public string TraerDato(bool bBase, string sTabla, string sCampoAtraer, bool bTextoCampoAtraer, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            SqlConnection cnn = new SqlConnection();

            string sSQL;
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            string sValor;
            if (bBase == false)
                cnn.ConnectionString = Conexion;
            else
                cnn.ConnectionString = Conexion2;
            try
            {
                cnn.Open();
                if (bTexto == false)
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "=" + sValorAComparar;
                else
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "='" + sValorAComparar + "'";
                com.Connection = cnn;
                com.CommandText = sSQL;
                dr = com.ExecuteReader();



                if (dr.Read())
                    sValor = dr[0].ToString();
                else if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = Convert.ToString(0);
            }
            catch
            {

                if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = Convert.ToString(0);
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return sValor;
        }
        
        public string TraerDato(SqlConnection cnn, bool bBase, string sTabla, string sCampoAtraer, bool bTextoCampoAtraer, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {

            string sSQL;
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            string sValor;
            try
            {
                if (bTexto == false)
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "=" + sValorAComparar;
                else
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "='" + sValorAComparar + "'";
                com.Connection = cnn;
                com.CommandText = sSQL;
                dr = com.ExecuteReader();



                if (dr.Read())
                    sValor = dr[0].ToString();
                else if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = Convert.ToString(0);
                dr.Close();
            }
            catch
            {
                if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = Convert.ToString(0);
            }
            return sValor;
        }

        public bool AgregarABaseEmpresa1(Pedido oPedido, List<DetallePedido> detallePedido)
        {
            SqlConnection cnn = new SqlConnection(Conexion);
            string sSQL;
            SqlCommand com = new SqlCommand();
            bool bGuardo = true;
            int iRenglon = 0;
            cnn.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                //string s;
                com.Connection = cnn;
                try
                {
                    sSQL = " INSERT INTO gva21 " +
                        " (FILLER, APRUEBA,CIRCUITO,COD_CLIENT,COD_SUCURS,COD_TRANSP,COD_VENDED," +
                        " COMENTARIO,COMP_STK,COND_VTA,COTIZ,ESTADO,EXPORTADO,FECHA_APRU,FECHA_ENTR,FECHA_PEDI," +
                        " HORA_APRUE,ID_EXTERNO,LEYENDA_1,LEYENDA_2,LEYENDA_3,LEYENDA_4,LEYENDA_5," +
                        " MON_CTE,N_LISTA,N_REMITO,NRO_O_COMP,NRO_PEDIDO,NRO_SUCURS,ORIGEN,PORC_DESC, " +
                        " REVISO_FAC,REVISO_PRE,REVISO_STK,TALONARIO,TALON_PED,TOTAL_PEDI,TIPO_ASIEN,ID_DIRECCION_ENTREGA,ID_ASIENTO_MODELO_GV,FECHA_INGRESO,HORA_INGRESO,ID_NEXO_PEDIDOS_ORDEN)" +
                        " VALUES " +
                        " (@FILLER, @APRUEBA,@CIRCUITO,@COD_CLIENT,@COD_SUCURS,@COD_TRANSP,@COD_VENDED," +
                        " @COMENTARIO,@COMP_STK,@COND_VTA,@COTIZ,@ESTADO,@EXPORTADO,@FECHA_APRU,@FECHA_ENTR,@FECHA_PEDI," +
                        " @HORA_APRUE,@ID_EXTERNO,@LEYENDA_1,@LEYENDA_2,@LEYENDA_3,@LEYENDA_4,@LEYENDA_5," +
                        " @MON_CTE,@N_LISTA,@N_REMITO,@NRO_O_COMP,@NRO_PEDIDO,@NRO_SUCURS,@ORIGEN,@PORC_DESC, " +
                        " @REVISO_FAC,@REVISO_PRE,@REVISO_STK,@TALONARIO,@TALON_PED,@TOTAL_PEDI,@TIPO_ASIEN,@ID_DIRECCION_ENTREGA,@ID_ASIENTO_MODELO_GV,@FECHA_INGRESO,@HORA_INGRESO,@ID_NEXO_PEDIDOS_ORDEN)";

                    com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@APRUEBA", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@CIRCUITO", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oPedido.Cliente;
                    com.Parameters.Add("@COD_SUCURS", SqlDbType.VarChar).Value = detallePedido.First().Deposito;
                    com.Parameters.Add("@COD_TRANSP", SqlDbType.VarChar).Value = oPedido.Transporte;
                    com.Parameters.Add("@COD_VENDED", SqlDbType.VarChar).Value = oPedido.Vendedor;
                    com.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = oPedido.Comentario;
                    com.Parameters.Add("@COMP_STK", SqlDbType.Bit).Value = 1;
                    com.Parameters.Add("@COND_VTA", SqlDbType.Int).Value = oPedido.CondicionVta;
                    com.Parameters.Add("@COTIZ", SqlDbType.Decimal).Value = 1;
                    // com.Parameters.Add("@ESTADO", SqlDbType.Int).Value = 1
                    com.Parameters.Add("@ESTADO", SqlDbType.Int).Value = oPedido.EstadoPedido;
                    com.Parameters.Add("@EXPORTADO", SqlDbType.Bit).Value = 0;
                    com.Parameters.Add("@FECHA_APRU", SqlDbType.DateTime).Value = new DateTime(1800, 1, 1);
                    com.Parameters.Add("@FECHA_ENTR", SqlDbType.DateTime).Value = oPedido.FechaEntrega;
                    com.Parameters.Add("@FECHA_PEDI", SqlDbType.DateTime).Value = oPedido.Fecha;
                    com.Parameters.Add("@HORA_APRUE", SqlDbType.VarChar).Value = "0000";
                    com.Parameters.Add("@ID_EXTERNO", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@LEYENDA_1", SqlDbType.VarChar).Value = oPedido.Leyenda1;
                    com.Parameters.Add("@LEYENDA_2", SqlDbType.VarChar).Value = oPedido.Leyenda2;
                    com.Parameters.Add("@LEYENDA_3", SqlDbType.VarChar).Value = oPedido.Leyenda3;
                    com.Parameters.Add("@LEYENDA_4", SqlDbType.VarChar).Value = oPedido.Leyenda4;
                    com.Parameters.Add("@LEYENDA_5", SqlDbType.VarChar).Value = oPedido.Leyenda5;
                    com.Parameters.Add("@MON_CTE", SqlDbType.Bit).Value = 1;
                    // com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = traerDato(False, "GVA14", "NRO_LISTA", False, "COD_CLIENT", True, oPedido.Cliente)
                    com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@N_REMITO", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@NRO_O_COMP", SqlDbType.VarChar).Value = oPedido.ImporteSeña;
                    com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                    com.Parameters.Add("@NRO_SUCURS", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@PORC_DESC", SqlDbType.Decimal).Value = oPedido.Bonif;
                    if (oPedido.EstadoPedido == "1")
                    {
                        com.Parameters.Add("@REVISO_FAC", SqlDbType.VarChar).Value = "I";
                        com.Parameters.Add("@REVISO_PRE", SqlDbType.VarChar).Value = "I";
                        com.Parameters.Add("@REVISO_STK", SqlDbType.VarChar).Value = "I";
                    }
                    else
                    {
                        com.Parameters.Add("@REVISO_FAC", SqlDbType.VarChar).Value = "A";
                        com.Parameters.Add("@REVISO_PRE", SqlDbType.VarChar).Value = "A";
                        com.Parameters.Add("@REVISO_STK", SqlDbType.VarChar).Value = "A";
                    }
                    if (Pedido.Empresa == ConfigurationManager.AppSettings.Get("Empresa1"))
                    {
                        com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = oPedido.Talonario;
                    }
                    else
                    {
                        com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = ConfigurationManager.AppSettings.Get("TalonarioPedidoCopia1");
                    }
                    com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                    com.Parameters.Add("@TOTAL_PEDI", SqlDbType.Decimal).Value = oPedido.Total;
                    com.Parameters.Add("@TIPO_ASIEN", SqlDbType.VarChar).Value = 1;
                    if (Pedido.PedidoClienteOcasional)
                    {
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = oPedido.IdDirEntrega;
                    }
                    com.Parameters.Add("@ID_ASIENTO_MODELO_GV", SqlDbType.Int).Value = ConfigurationManager.AppSettings.Get("ID_ASIENTO_MODELO_GV");
                    com.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = DateTime.Now;
                    com.Parameters.Add("@HORA_INGRESO", SqlDbType.VarChar).Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                    com.Parameters.Add("@ID_NEXO_PEDIDOS_ORDEN", SqlDbType.Int).Value = 0;
                    com.CommandText = sSQL;
                    com.ExecuteNonQuery();


                    for (int x = 0; x <= detallePedido.Count - 1; x++)
                    {
                        iRenglon = iRenglon + 1;

                        com.CommandText = " INSERT INTO gva03(FILLER,CAN_EQUI_V,CANT_A_DES,CANT_A_FAC,CANT_PEDID,CANT_PEN_D,CANT_PEN_F,COD_ARTICU,DESCUENTO,N_RENGLON,NRO_PEDIDO,PEN_REM_FC,PEN_FAC_RE,PRECIO,TALON_PED,ID_MEDIDA_VENTAS,ID_MEDIDA_STOCK, UNIDAD_MEDIDA_SELECCIONADA,PRECIO_BONIF,ID_NEXO_PEDIDOS_RENGLON_ORDEN) " +
                            " VALUES (@FILLER,@CAN_EQUI_V,@CANT_A_DES,@CANT_A_FAC,@CANT_PEDID,@CANT_PEN_D,@CANT_PEN_F,@COD_ARTICU,@DESCUENTO,@N_RENGLON,@NRO_PEDIDO,@PEN_REM_FC,@PEN_FAC_RE,@PRECIO,@TALON_PED,@ID_MEDIDA_VENTAS,@ID_MEDIDA_STOCK, @UNIDAD_MEDIDA_SELECCIONADA,@PRECIO_BONIF,@ID_NEXO_PEDIDOS_RENGLON_ORDEN)";
                        com.Parameters.Clear();
                        com.Connection = cnn;

                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                        equivalencia = 1;
                        com.Parameters.Add("@CAN_EQUI_V", SqlDbType.Decimal).Value = equivalencia;
                        com.Parameters.Add("@CANT_A_DES", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_A_FAC", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEDID", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_D", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_F", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;

                        if (detallePedido[x].EsAdicional)
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = "";
                        else
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = detallePedido[x].Codigo;

                        com.Parameters.Add("ID_MEDIDA_VENTAS", SqlDbType.Int).Value = Convert.ToInt32(TraerDato(cnn, false, "sta11", "ID_MEDIDA_VENTAS", false, "cod_articu", true, detallePedido[x].Codigo));//
                        com.Parameters.Add("ID_MEDIDA_STOCK", SqlDbType.Int).Value = Convert.ToInt32(TraerDato(cnn, false, "sta11", "ID_MEDIDA_STOCK", false, "cod_articu", true, detallePedido[x].Codigo)); //
                        com.Parameters.Add("UNIDAD_MEDIDA_SELECCIONADA", SqlDbType.VarChar).Value = "V";
                        com.Parameters.Add("@DESCUENTO", SqlDbType.Decimal).Value = detallePedido[x].Bonif;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.Int).Value = iRenglon;
                        com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                        com.Parameters.Add("@PEN_REM_FC", SqlDbType.Decimal).Value = detallePedido[x].Cantidad;
                        com.Parameters.Add("@PEN_FAC_RE", SqlDbType.Decimal).Value = detallePedido[x].Cantidad;
                        com.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = detallePedido[x].PrecioL;
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                        com.Parameters.Add("@PRECIO_BONIF", SqlDbType.Decimal).Value = detallePedido[x].PrecioV;
                        com.Parameters.Add("@ID_NEXO_PEDIDOS_RENGLON_ORDEN", SqlDbType.Int).Value = 0;

                        com.ExecuteNonQuery();

                        com.CommandText = " INSERT INTO gva45 (filler,cod_modelo,[desc],desc_adic,n_comp,n_Renglon,talonario,t_comp) " + " VALUES(@FILLER,@cod_modelo,@desc,@desc_adic,@n_comp,@N_RENGLON,@talonario,@t_comp)";

                        com.Parameters.Clear();
                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                        com.Parameters.Add("@cod_modelo", SqlDbType.VarChar).Value = "";

                        if (detallePedido[x].Descripcion.Length >= 30)
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = detallePedido[x].Descripcion.Substring(0, 30);
                            string desc_adic = detallePedido[x].Descripcion.Substring(30, detallePedido[x].Descripcion.Length - 30).Trim();
                            if (desc_adic.Length >= 20)
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic.Substring(0, 20).Trim();
                            else
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic;
                        }
                        else
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = detallePedido[x].Descripcion;
                            com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = "";
                        }


                        com.Parameters.Add("@n_comp", SqlDbType.VarChar).Value = oPedido.Numero;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.Int).Value = iRenglon;
                        com.Parameters.Add("@talonario", SqlDbType.SmallInt).Value = oPedido.TalonarioPedido;
                        com.Parameters.Add("@t_comp", SqlDbType.VarChar).Value = "PED";

                        com.ExecuteNonQuery();
                    }

                    if (cnn.State == ConnectionState.Open)
                    {
                        cnn.Close();
                    }

                }
                catch (Exception ex)
                {
                    bGuardo = false;
                    throw new Exception(ex.Message);
                }
                bGuardo = true;
                scope.Complete();
            }
            return bGuardo;
        }

        public bool AgregarABaseEmpresa2(Pedido oPedido, List<DetallePedido> detallePedido)
        {
            SqlConnection cnn = new SqlConnection(Conexion2);
            string sSQL;
            SqlCommand com = new SqlCommand();
            bool bGuardo = true;
            int iRenglon = 0;
            cnn.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                com.Connection = cnn;
                try
                {
                    sSQL = " INSERT INTO gva21 " +
                        " (FILLER, APRUEBA,CIRCUITO,COD_CLIENT,COD_SUCURS,COD_TRANSP,COD_VENDED," +
                        " COMENTARIO,COMP_STK,COND_VTA,COTIZ,ESTADO,EXPORTADO,FECHA_APRU,FECHA_ENTR,FECHA_PEDI," +
                        " HORA_APRUE,ID_EXTERNO,LEYENDA_1,LEYENDA_2,LEYENDA_3,LEYENDA_4,LEYENDA_5," +
                        " MON_CTE,N_LISTA,N_REMITO,NRO_O_COMP,NRO_PEDIDO,NRO_SUCURS,ORIGEN,PORC_DESC, " +
                        " REVISO_FAC,REVISO_PRE,REVISO_STK,TALONARIO,TALON_PED,TOTAL_PEDI,TIPO_ASIEN,ID_DIRECCION_ENTREGA,ID_ASIENTO_MODELO_GV,FECHA_INGRESO,HORA_INGRESO,ID_NEXO_PEDIDOS_ORDEN)" +
                        " VALUES " +
                        " (@FILLER, @APRUEBA,@CIRCUITO,@COD_CLIENT,@COD_SUCURS,@COD_TRANSP,@COD_VENDED," +
                        " @COMENTARIO,@COMP_STK,@COND_VTA,@COTIZ,@ESTADO,@EXPORTADO,@FECHA_APRU,@FECHA_ENTR,@FECHA_PEDI," +
                        " @HORA_APRUE,@ID_EXTERNO,@LEYENDA_1,@LEYENDA_2,@LEYENDA_3,@LEYENDA_4,@LEYENDA_5," +
                        " @MON_CTE,@N_LISTA,@N_REMITO,@NRO_O_COMP,@NRO_PEDIDO,@NRO_SUCURS,@ORIGEN,@PORC_DESC, " +
                        " @REVISO_FAC,@REVISO_PRE,@REVISO_STK,@TALONARIO,@TALON_PED,@TOTAL_PEDI,@TIPO_ASIEN,@ID_DIRECCION_ENTREGA,@ID_ASIENTO_MODELO_GV,@FECHA_INGRESO,@HORA_INGRESO,@ID_NEXO_PEDIDOS_ORDEN)";

                    com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@APRUEBA", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@CIRCUITO", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oPedido.Cliente;
                    com.Parameters.Add("@COD_SUCURS", SqlDbType.VarChar).Value = detallePedido.First().Deposito;
                    com.Parameters.Add("@COD_TRANSP", SqlDbType.VarChar).Value = oPedido.Transporte;
                    com.Parameters.Add("@COD_VENDED", SqlDbType.VarChar).Value = oPedido.Vendedor;
                    com.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = oPedido.Comentario;
                    com.Parameters.Add("@COMP_STK", SqlDbType.Bit).Value = 1;
                    com.Parameters.Add("@COND_VTA", SqlDbType.Int).Value = oPedido.CondicionVta;
                    com.Parameters.Add("@COTIZ", SqlDbType.Decimal).Value = 1;
                    com.Parameters.Add("@ESTADO", SqlDbType.Int).Value = oPedido.EstadoPedido;
                    com.Parameters.Add("@EXPORTADO", SqlDbType.Bit).Value = 0;
                    com.Parameters.Add("@FECHA_APRU", SqlDbType.DateTime).Value = new DateTime(1800, 1, 1);
                    com.Parameters.Add("@FECHA_ENTR", SqlDbType.DateTime).Value = oPedido.FechaEntrega;
                    com.Parameters.Add("@FECHA_PEDI", SqlDbType.DateTime).Value = oPedido.Fecha;
                    com.Parameters.Add("@HORA_APRUE", SqlDbType.VarChar).Value = "0000";
                    com.Parameters.Add("@ID_EXTERNO", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@LEYENDA_1", SqlDbType.VarChar).Value = oPedido.Leyenda1;
                    com.Parameters.Add("@LEYENDA_2", SqlDbType.VarChar).Value = oPedido.Leyenda2;
                    com.Parameters.Add("@LEYENDA_3", SqlDbType.VarChar).Value = oPedido.Leyenda3;
                    com.Parameters.Add("@LEYENDA_4", SqlDbType.VarChar).Value = oPedido.Leyenda4;
                    com.Parameters.Add("@LEYENDA_5", SqlDbType.VarChar).Value = oPedido.Leyenda5;
                    com.Parameters.Add("@MON_CTE", SqlDbType.Bit).Value = 1;
                    // com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = traerDato(False, "GVA14", "NRO_LISTA", False, "COD_CLIENT", True, oPedido.Cliente)
                    com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@N_REMITO", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@NRO_O_COMP", SqlDbType.VarChar).Value = oPedido.ImporteSeña;
                    com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                    com.Parameters.Add("@NRO_SUCURS", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("@PORC_DESC", SqlDbType.Decimal).Value = oPedido.Bonif;
                    if (oPedido.EstadoPedido == "1")
                    {
                        com.Parameters.Add("@REVISO_FAC", SqlDbType.VarChar).Value = "I";
                        com.Parameters.Add("@REVISO_PRE", SqlDbType.VarChar).Value = "I";
                        com.Parameters.Add("@REVISO_STK", SqlDbType.VarChar).Value = "I";
                    }
                    else
                    {
                        com.Parameters.Add("@REVISO_FAC", SqlDbType.VarChar).Value = "A";
                        com.Parameters.Add("@REVISO_PRE", SqlDbType.VarChar).Value = "A";
                        com.Parameters.Add("@REVISO_STK", SqlDbType.VarChar).Value = "A";
                    }
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = oPedido.Talonario;
                    com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                    com.Parameters.Add("@TOTAL_PEDI", SqlDbType.Decimal).Value = oPedido.Total;
                    com.Parameters.Add("@TIPO_ASIEN", SqlDbType.VarChar).Value = 1;
                    if (Pedido.PedidoClienteOcasional)
                    {
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = oPedido.IdDirEntrega;
                    }
                    com.Parameters.Add("@ID_ASIENTO_MODELO_GV", SqlDbType.Int).Value = ConfigurationManager.AppSettings.Get("ID_ASIENTO_MODELO_GV");
                    com.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = DateTime.Now;
                    com.Parameters.Add("@HORA_INGRESO", SqlDbType.DateTime).Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                    com.Parameters.Add("@ID_NEXO_PEDIDOS_ORDEN", SqlDbType.Int).Value = 0;
                    com.CommandText = sSQL;
                    com.ExecuteNonQuery();

                    for (int x = 0; x <= detallePedido.Count - 1; x++)
                    {
                        // Validamos que si es mayor a cero, pregunte por precios para ver si llega aca el error.
                        if (detallePedido[x].Bonif > 0)
                        {
                            if (detallePedido[x].PrecioV == detallePedido[x].PrecioL)
                                throw new Exception("ERROR NO CALCULO DESCUENTO . ");
                        }
                        iRenglon = iRenglon + 1;

                        com.CommandText = " INSERT INTO gva03(FILLER,CAN_EQUI_V,CANT_A_DES,CANT_A_FAC,CANT_PEDID,CANT_PEN_D,CANT_PEN_F,COD_ARTICU,DESCUENTO,N_RENGLON,NRO_PEDIDO,PEN_REM_FC,PEN_FAC_RE,PRECIO,TALON_PED,ID_MEDIDA_VENTAS,ID_MEDIDA_STOCK, UNIDAD_MEDIDA_SELECCIONADA,PRECIO_BONIF,ID_NEXO_PEDIDOS_RENGLON_ORDEN) " +
                            " VALUES (@FILLER,@CAN_EQUI_V,@CANT_A_DES,@CANT_A_FAC,@CANT_PEDID,@CANT_PEN_D,@CANT_PEN_F,@COD_ARTICU,@DESCUENTO,@N_RENGLON,@NRO_PEDIDO,@PEN_REM_FC,@PEN_FAC_RE,@PRECIO,@TALON_PED,@ID_MEDIDA_VENTAS,@ID_MEDIDA_STOCK, @UNIDAD_MEDIDA_SELECCIONADA,@PRECIO_BONIF,@ID_NEXO_PEDIDOS_RENGLON_ORDEN)";
                        com.Parameters.Clear();
                        com.Connection = cnn;

                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                        equivalencia = 1;
                        com.Parameters.Add("@CAN_EQUI_V", SqlDbType.Decimal).Value = equivalencia;
                        com.Parameters.Add("@CANT_A_DES", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_A_FAC", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEDID", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_D", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_F", SqlDbType.Decimal).Value = detallePedido[x].Cantidad * equivalencia;

                        if (detallePedido[x].EsAdicional)
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = "";
                        else
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = detallePedido[x].Codigo;

                        com.Parameters.Add("ID_MEDIDA_VENTAS", SqlDbType.Int).Value = TraerDato(cnn, false, "sta11", "ID_MEDIDA_VENTAS", false, "cod_articu", true, detallePedido[x].Codigo);
                        com.Parameters.Add("ID_MEDIDA_STOCK", SqlDbType.Int).Value = TraerDato(cnn, false, "sta11", "ID_MEDIDA_STOCK", false, "cod_articu", true, detallePedido[x].Codigo);
                        com.Parameters.Add("UNIDAD_MEDIDA_SELECCIONADA", SqlDbType.VarChar).Value = "V";

                        com.Parameters.Add("@DESCUENTO", SqlDbType.Decimal).Value = detallePedido[x].Bonif;
                        //com.Parameters.Add("@DESCUENTO", SqlDbType.Decimal).Value = 0;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.Int).Value = iRenglon;
                        com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                        // Dim d As Decimal = oPedido.Detalle(x).Bonif
                        com.Parameters.Add("@PEN_REM_FC", SqlDbType.Decimal).Value = detallePedido[x].Cantidad;
                        com.Parameters.Add("@PEN_FAC_RE", SqlDbType.Decimal).Value = detallePedido[x].Cantidad;
                        com.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = detallePedido[x].PrecioL;
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                        com.Parameters.Add("@PRECIO_BONIF", SqlDbType.Decimal).Value = detallePedido[x].PrecioV;
                        com.Parameters.Add("@ID_NEXO_PEDIDOS_RENGLON_ORDEN", SqlDbType.Int).Value = 0;
                        com.ExecuteNonQuery();

                        com.CommandText = " INSERT INTO gva45 (filler,cod_modelo,[desc],desc_adic,n_comp,n_Renglon,talonario,t_comp) " + " VALUES(@FILLER,@cod_modelo,@desc,@desc_adic,@n_comp,@N_RENGLON,@talonario,@t_comp)";

                        com.Parameters.Clear();
                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                        com.Parameters.Add("@cod_modelo", SqlDbType.VarChar).Value = "";

                        if (detallePedido[x].Descripcion.Length >= 30)
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = detallePedido[x].Descripcion.Substring(0, 30);
                            string desc_adic = detallePedido[x].Descripcion.Substring(30, detallePedido[x].Descripcion.Length - 30).Trim();
                            if (desc_adic.Length >= 20)
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic.Substring(0, 20).Trim();
                            else
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic;
                        }
                        else
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = detallePedido[x].Descripcion;
                            com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = "";
                        }
                        com.Parameters.Add("@n_comp", SqlDbType.VarChar).Value = oPedido.Numero;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.Int).Value = iRenglon;
                        com.Parameters.Add("@talonario", SqlDbType.SmallInt).Value = oPedido.TalonarioPedido2;
                        com.Parameters.Add("@t_comp", SqlDbType.VarChar).Value = "PED";

                        com.ExecuteNonQuery();
                    }
                    if (cnn.State == ConnectionState.Open)
                    {
                        cnn.Close();
                    }

                }
                catch (Exception ex)
                {
                    bGuardo = false;
                    throw new Exception(ex.Message);
                }
                bGuardo = true;
                scope.Complete();
            }
            return bGuardo;
        }

        public CapaEntidades.Pedido CargarDatos()
        {
            try
            {
                CapaEntidades.Pedido oDatos = new CapaEntidades.Pedido();
                if (iLugar > -1 & iLugar <= iRegistros - 1)
                {
                    if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "1")
                        oDatos.EstadoPedido = "Ingresado";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "2")
                        oDatos.EstadoPedido = "Aprobado";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "3")
                        oDatos.EstadoPedido = "Cumplido";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "4")
                        oDatos.EstadoPedido = "Cerrado";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "5")
                        oDatos.EstadoPedido = "Anulado";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "6")
                        oDatos.EstadoPedido = "Revisado";
                    else if (ds.Tables[0].Rows[iLugar]["Estado"].ToString() == "7")
                        oDatos.EstadoPedido = "Desaprobado";
                    else if (true)
                        oDatos.EstadoPedido = "Ingresado";
                    oDatos.Numero = ds.Tables[0].Rows[iLugar]["nro_pedido"].ToString();
                    oDatos.Cliente = ds.Tables[0].Rows[iLugar]["cod_client"].ToString();
                    oDatos.RazonSocial = ds.Tables[0].Rows[iLugar]["razon_soci"].ToString();//
                    oDatos.Fecha = Convert.ToDateTime(ds.Tables[0].Rows[iLugar]["Fecha_pedi"].ToString());
                    oDatos.CondicionVta = ds.Tables[0].Rows[iLugar]["Cond_Vta"].ToString();
                    oDatos.Deposito = ds.Tables[0].Rows[iLugar]["cod_sucurs"].ToString();
                    oDatos.FechaEntrega = Convert.ToDateTime(ds.Tables[0].Rows[iLugar]["Fecha_Entr"].ToString());
                    oDatos.ListaPrecio = ds.Tables[0].Rows[iLugar]["n_lista"].ToString();
                    oDatos.Talonario = ds.Tables[0].Rows[iLugar]["Talonario"].ToString();
                    oDatos.Transporte = ds.Tables[0].Rows[iLugar]["Cod_Transp"].ToString();
                    oDatos.Vendedor = ds.Tables[0].Rows[iLugar]["cod_vended"].ToString();
                    oDatos.Total = Convert.ToDecimal(ds.Tables[0].Rows[iLugar]["Total_pedi"].ToString());
                    oDatos.Bonif = Convert.ToDecimal(ds.Tables[0].Rows[iLugar]["porc_desc"].ToString());
                    oDatos.Leyenda5 = ds.Tables[0].Rows[iLugar]["leyenda_5"].ToString();
                    oDatos.Domicilio = ds.Tables[0].Rows[iLugar]["domicilio"].ToString();
                    if (!String.IsNullOrEmpty(ds.Tables[0].Rows[iLugar]["id_direccion_entrega"].ToString()))
                        oDatos.IdDirEntrega = Convert.ToInt32(ds.Tables[0].Rows[iLugar]["id_direccion_entrega"].ToString());
                    else
                        oDatos.IdDirEntrega = 0;

                    oDatos.Detalle = CargarDetalle(oDatos.Numero);
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[iLugar]["cotiz"].ToString()))
                        oDatos.Cotiz = 1;
                    //oDatos.Cotiz = Convert.ToDecimal(ds.Tables[0].Rows[iLugar]["cotiz"]);
                }
                else
                {
                    oDatos.Numero = "";
                    oDatos.Cliente = "";
                    oDatos.Fecha = new DateTime(1800, 1, 1);
                    oDatos.CondicionVta = "";
                    oDatos.Deposito = "";
                    oDatos.FechaEntrega = new DateTime(1800, 1, 1);
                    oDatos.ListaPrecio = "";
                    oDatos.Talonario = "";
                    oDatos.Transporte = "";
                    oDatos.Vendedor = "";
                    oDatos.Bonif = 0;
                    oDatos.Total = 0;
                    oDatos.IdDirEntrega = 0;
                }
                return oDatos;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<DetallePedido> CargarDetalle(string sNumero)
        {
            try
            {
                SqlDataAdapter daDetalle;
                List<CapaEntidades.DetallePedido> oDetalleLista = new List<CapaEntidades.DetallePedido>();
                DataSet dsDetalle = new DataSet();
                string sSQL;

                //(case when gva03.filler = '' then gva03.Descuento else gva03.filler end) as descuento ,

                sSQL = "SELECT gva03.descuento ,cant_pedid / CAN_EQUI_V as cant_pedid , CASE WHEN sta11.descripcio IS NULL THEN '' ELSE sta11.descripcio + sta11.desc_adic END as descripcio, " +
                    " CASE WHEN gva45.[desc] IS NULL THEN '' ELSE gva45.[desc] END as desc2, " +
                    " CASE WHEN gva45.desc_adic IS NULL THEN '' ELSE gva45.desc_adic END as descAd, " +
                    " case WHEN ltrim(gva03.cod_articu) = '' THEN 'DM' ELSE  'M ' END  as color, " +
                    " precio_bonif,gva03.cod_articu as codigo, precio as importe, precio as precioVta , gva03.N_RENGLON ,CAN_EQUI_V as equivalencia" +
                    " From gva03 " + " LEFT JOIN sta11 ON sta11.cod_Articu=gva03.cod_articu " +
                    " LEFT JOIN gva45 ON gva45.n_renglon=gva03.n_renglon and gva45.n_comp=gva03.nro_pedido AND GVA45.TALONARIO=GVA03.TALON_PED " +
                    " WHERE  gva03.nro_pedido= '" + sNumero + "' AND (gva45.t_Comp='PED' OR gva45.t_Comp IS NULL) " +
                    " ORDER BY gva03.n_renglon ";


                daDetalle = new SqlDataAdapter(sSQL, Conexion);

                daDetalle.Fill(dsDetalle);
                for (int x = 0; x <= dsDetalle.Tables[0].Rows.Count - 1; x++)
                {
                    CapaEntidades.DetallePedido oDetalle = new CapaEntidades.DetallePedido();
                    oDetalle.Codigo = dsDetalle.Tables[0].Rows[x]["codigo"].ToString();
                    oDetalle.Descripcion = dsDetalle.Tables[0].Rows[x]["descripcio"].ToString();
                    oDetalle.Cantidad = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["cant_pedid"].ToString());
                    oDetalle.PrecioL = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["precioVta"].ToString());
                    // oDetalle.PrecioV = dsDetalle.Tables(0).Rows(x).Item("precio")
                    //oDetalle.PrecioV = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["precioVta"].ToString());
                    oDetalle.Bonif = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["descuento"].ToString());
                    oDetalle.PrecioV = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["precio_bonif"].ToString());
                    decimal equivalencia = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["equivalencia"].ToString());
                    decimal cantidad = Convert.ToDecimal(dsDetalle.Tables[0].Rows[x]["cant_pedid"].ToString());
                    oDetalle.Total = oDetalle.PrecioV * equivalencia * cantidad;
                    oDetalle.Renglon = Convert.ToInt32(dsDetalle.Tables[0].Rows[x]["N_RENGLON"].ToString());
                    oDetalle.Equivalencia = equivalencia;
                    oDetalleLista.Add(oDetalle);
                }
                return oDetalleLista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public decimal TraerIva(string sCodigo)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            decimal dValor;
            cnn.ConnectionString = Conexion;
            try
            {
                cnn.Open();

                com.Connection = cnn;
                com.CommandText = "SELECT porcentaje FROM sta11 INNER JOIN gva41 on sta11.cod_iva=cod_alicuo where cod_Articu='" + sCodigo + "'";
                dr = com.ExecuteReader();
                if (dr.Read() == true)
                    dValor = Convert.ToDecimal(dr[0]);
                else
                    dValor = 0;
            }
            catch
            {
                dValor = 0;
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return dValor;
        }

        public string TraerDatoEspecial(string sTabla, string sCampoAtraer, bool bTextoCampoAtraer, string sCampoABuscar, bool bTexto, string sValorAComparar)
        {
            SqlConnection cnn = new SqlConnection();

            string sSQL;
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            string sValor;
            cnn.ConnectionString = ConfigurationManager.AppSettings.Get("Conexion").ToString();

            try
            {
                cnn.Open();
                if (bTexto == false)
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "=" + sValorAComparar;
                else
                    sSQL = "SELECT " + sCampoAtraer + " FROM " + sTabla + " WHERE " + sCampoABuscar + "='" + sValorAComparar + "'";
                com.Connection = cnn;
                com.CommandText = sSQL;
                dr = com.ExecuteReader();



                if (dr.Read())
                    sValor = dr[0].ToString();
                else if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = "0";
            }
            catch
            {
                if (bTextoCampoAtraer == true)
                    sValor = "";
                else
                    sValor = "0";
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return sValor;
        }

        public DataSet CargarDirecciones(string sCodigoCliente)
        {
            try
            {
                SqlDataAdapter daDetalle;
                DataSet dsDetalle = new DataSet();
                string sSQL;


                sSQL = "select DIRECCION_ENTREGA.ID_DIRECCION_ENTREGA,COD_DIRECCION_ENTREGA + ' - ' + direccion + ', ' + localidad + ', ' + gva18.NOMBRE_PRO as Direccion,HABITUAL " +
                    " from DIRECCION_ENTREGA " +
                    " left join gva18 on gva18.COD_PROVIN collate Latin1_General_BIN=DIRECCION_ENTREGA.COD_PROVINCIA " +
                    " where COD_CLIENTE='" + sCodigoCliente + "'" +
                    " UNION ALL" + " select 0,'SIN SELECCIONAR','' " +
                    " order by 3 desc";
                daDetalle = new SqlDataAdapter(sSQL, Conexion);
                daDetalle.Fill(dsDetalle);
                return dsDetalle;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public DataSet CargarDepositos(string sCodigoCliente)
        {
            try
            {
                SqlDataAdapter daDetalle;
                DataSet dsDetalle = new DataSet();
                string sSQL;


                sSQL = "select  DIRECCION_ENTREGA.ID_DIRECCION_ENTREGA	,COD_DIRECCION_ENTREGA + ' - ' + direccion + ', ' + localidad + ', ' + gva18.NOMBRE_PRO as Direccion,HABITUAL " +
                    " from DIRECCION_ENTREGA " +
                    " left join gva18 on gva18.COD_PROVIN collate Latin1_General_BIN=DIRECCION_ENTREGA.COD_PROVINCIA " +
                    " where COD_CLIENTE='" + sCodigoCliente + "' " +
                    " UNION ALL" + " select 0,'SIN SELECCIONAR','' " +
                    " order by 3 desc";

                daDetalle = new SqlDataAdapter(sSQL, Conexion);
                daDetalle.Fill(dsDetalle);
                return dsDetalle;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string TraerDias(int idDirEntrega)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            string dValor;
            cnn.ConnectionString = Conexion;
            try
            {
                cnn.Open();

                com.Connection = cnn;
                com.CommandText = " SELECT " + " case when ENTREGA_LUNES = 'S' then 'LUN ' else '' end  +" +
                    " case when ENTREGA_MARTES = 'S' then ' MAR ' else '' end +" +
                    " case when ENTREGA_MIERCOLES = 'S' then ' MIE ' else '' end +" +
                    " case when ENTREGA_JUEVES = 'S' then ' JUE ' else '' end +" +
                    " case when ENTREGA_VIERNES = 'S' then ' VIE ' else '' end +" +
                    " case when ENTREGA_SABADO = 'S' then ' SAB ' else '' end +" +
                    " case when ENTREGA_DOMINGO = 'S' then ' DOM ' else '' end AS DIAS" +
                    " FROM DIRECCION_ENTREGA WHERE ID_DIRECCION_ENTREGA=" + idDirEntrega.ToString() + "";

                dr = com.ExecuteReader();
                if (dr.Read() == true)
                {
                    dValor = dr[0].ToString();
                    if (dValor == string.Empty)
                        dValor = "No se registrarón dias de entrega";
                }
                else
                    dValor = "Sin dato";
            }
            catch
            {
                dValor = "Sin dato";
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return dValor;
        }

        public bool ModificarBase(bool conexion, Pedido oPedido)
        {
            SqlConnection cnn = new SqlConnection();
            string sSQL;
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            int iRenglon = 0;
            if (conexion)
            {
                cnn.ConnectionString = Conexion;
            }
            else
            {
                cnn.ConnectionString = Conexion2;
                //oPedido.TalonarioPedido = oPedido.TalonarioPedido2;
                string talonario = TraerDato(true, "GVA21", "TALONARIO", false, "NRO_PEDIDO", true, oPedido.Numero.ToString() + "' AND TALON_PED='" + oPedido.TalonarioPedido2.ToString());
                oPedido.Talonario = talonario;
            }
            com.Connection = cnn;
            cnn.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    sSQL = "UPDATE gva21 " +
                        " SET COD_CLIENT=@COD_CLIENT,COD_SUCURS=@COD_SUCURS,COD_TRANSP=@COD_TRANSP,COD_VENDED=@COD_VENDED," +
                        " COND_VTA=@COND_VTA,FECHA_ENTR=@FECHA_ENTR,FECHA_PEDI=@FECHA_PEDI," +
                        " N_LISTA=@N_LISTA,TALONARIO=@TALONARIO,TOTAL_PEDI=@TOTAL_PEDI,Leyenda_1=@Leyenda_1,Leyenda_2=@Leyenda_2,Leyenda_3=@Leyenda_3,Leyenda_4=@Leyenda_4,Leyenda_5=@Leyenda_5,COMENTARIO=@COMENTARIO,ID_DIRECCION_ENTREGA=@ID_DIRECCION_ENTREGA,NRO_O_COMP=@NRO_O_COMP" +
                        " WHERE NRO_PEDIDO=@NRO_PEDIDO AND TALON_PED=@TALON_PED";
                    com.CommandText = sSQL;
                    com.Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oPedido.Cliente;
                    com.Parameters.Add("@COD_SUCURS", SqlDbType.VarChar).Value = oPedido.Deposito;
                    com.Parameters.Add("@COD_TRANSP", SqlDbType.VarChar).Value = oPedido.Transporte;
                    com.Parameters.Add("@COD_VENDED", SqlDbType.VarChar).Value = oPedido.Vendedor;
                    com.Parameters.Add("@COND_VTA", SqlDbType.Int).Value = oPedido.CondicionVta;
                    com.Parameters.Add("@FECHA_ENTR", SqlDbType.DateTime).Value = oPedido.FechaEntrega;
                    com.Parameters.Add("@FECHA_PEDI", SqlDbType.DateTime).Value = oPedido.Fecha;
                    // com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = traerDato(False, "GVA14", "NRO_LISTA", False, "COD_CLIENT", True, oPedido.Cliente)
                    com.Parameters.Add("@N_LISTA", SqlDbType.Int).Value = 1;
                    com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = oPedido.Talonario;
                    if (conexion)
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                    }
                    else
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                    }
                    com.Parameters.Add("@TOTAL_PEDI", SqlDbType.Decimal).Value = oPedido.Total;
                    com.Parameters.Add("@TIPO_ASIEN", SqlDbType.VarChar).Value = 1;
                    com.Parameters.Add("@LEYENDA_1", SqlDbType.VarChar).Value = oPedido.Leyenda1;
                    com.Parameters.Add("@LEYENDA_2", SqlDbType.VarChar).Value = oPedido.Leyenda2;
                    com.Parameters.Add("@LEYENDA_3", SqlDbType.VarChar).Value = oPedido.Leyenda3;
                    com.Parameters.Add("@LEYENDA_4", SqlDbType.VarChar).Value = oPedido.Leyenda4;
                    com.Parameters.Add("@LEYENDA_5", SqlDbType.VarChar).Value = oPedido.Leyenda5;
                    com.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = oPedido.Comentario;
                    if (oPedido.IdDirEntrega == 0)
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = DBNull.Value;
                    else
                        com.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = oPedido.IdDirEntrega;
                    com.Parameters.Add("@NRO_O_COMP", SqlDbType.VarChar).Value = oPedido.ImporteSeña;

                    da.UpdateCommand = com;
                    ds.Tables[0].Rows[iLugar]["nro_pedido"] = oPedido.Numero;
                    ds.Tables[0].Rows[iLugar]["cod_client"] = oPedido.Cliente;
                    ds.Tables[0].Rows[iLugar]["Fecha_pedi"] = Convert.ToString(oPedido.Fecha);
                    ds.Tables[0].Rows[iLugar]["Cond_Vta"] = oPedido.CondicionVta;
                    ds.Tables[0].Rows[iLugar]["cod_sucurs"] = oPedido.Deposito;
                    ds.Tables[0].Rows[iLugar]["Fecha_Entr"] = Convert.ToString(oPedido.FechaEntrega);
                    // ds.Tables(0).Rows(iLugar).Item("n_lista") = traerDato(False, "GVA14", "NRO_LISTA", False, "COD_CLIENT", True, oPedido.Cliente)
                    ds.Tables[0].Rows[iLugar]["n_lista"] = "1";
                    ds.Tables[0].Rows[iLugar]["Talonario"] = oPedido.Talonario;
                    ds.Tables[0].Rows[iLugar]["Cod_Transp"] = oPedido.Transporte;
                    ds.Tables[0].Rows[iLugar]["cod_vended"] = oPedido.Vendedor;
                    ds.Tables[0].Rows[iLugar]["total_pedi"] = Convert.ToString(oPedido.Total);
                    ds.Tables[0].Rows[iLugar]["porc_desc"] = Convert.ToString(oPedido.Bonif);
                    da.Update(ds.Tables[0]);
                    // DETALLE
                    com = new SqlCommand();
                    com.CommandText = "DELETE FROM gva03 WHERE talon_ped=@TALON_PED AND nro_pedido=@NRO_PEDIDO";
                    com.Parameters.Clear();
                    if (conexion)
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                    }
                    else
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                    }
                    com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                    com.Connection = cnn;
                    com.ExecuteNonQuery();

                    com = new SqlCommand();
                    com.CommandText = "DELETE FROM gva45 WHERE talonario=@TALON_PED AND n_comp=@NRO_PEDIDO";
                    com.Parameters.Clear();
                    if (conexion)
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                    }
                    else
                    {
                        com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                    }
                    com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                    com.Connection = cnn;
                    com.ExecuteNonQuery();


                    for (int x = 0; x <= oPedido.Detalle.Count - 1; x++)
                    {
                        iRenglon = iRenglon + 1;
                        com.CommandText = " INSERT INTO gva03(FILLER,CAN_EQUI_V,CANT_A_DES,CANT_A_FAC,CANT_PEDID,CANT_PEN_D,CANT_PEN_F,COD_ARTICU,DESCUENTO,N_RENGLON,NRO_PEDIDO,PEN_REM_FC,PEN_FAC_RE,PRECIO,TALON_PED,PRECIO_BONIF) " +
                            " VALUES (@FILLER,@CAN_EQUI_V,@CANT_A_DES,@CANT_A_FAC,@CANT_PEDID,@CANT_PEN_D,@CANT_PEN_F,@COD_ARTICU,@DESCUENTO,@N_RENGLON,@NRO_PEDIDO,@PEN_REM_FC,@PEN_FAC_RE,@PRECIO,@TALON_PED,@PRECIO_BONIF)";
                        com.Parameters.Clear();
                        com.Connection = cnn;
                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "Interfaz Seincomp";
                        equivalencia = 1;
                        com.Parameters.Add("@CAN_EQUI_V", SqlDbType.Decimal).Value = equivalencia;
                        com.Parameters.Add("@CANT_A_DES", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_A_FAC", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEDID", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_D", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad * equivalencia;
                        com.Parameters.Add("@CANT_PEN_F", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad * equivalencia;
                        com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = oPedido.Detalle[x].Codigo;
                        com.Parameters.Add("@DESCUENTO", SqlDbType.Decimal).Value = oPedido.Detalle[x].Bonif;
                        //com.Parameters.Add("@DESCUENTO", SqlDbType.Decimal).Value = 0;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.Int).Value = iRenglon;
                        com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                        com.Parameters.Add("@PEN_REM_FC", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad;
                        com.Parameters.Add("@PEN_FAC_RE", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad;
                        com.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = oPedido.Detalle[x].PrecioL;
                        if (conexion)
                        {
                            com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                        }
                        else
                        {
                            com.Parameters.Add("@TALON_PED", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                        }
                        com.Parameters.Add("@PRECIO_BONIF", SqlDbType.Decimal).Value = oPedido.Detalle[x].PrecioV;
                        com.ExecuteNonQuery();

                        com.CommandText = " INSERT INTO gva45 (filler,cod_modelo,[desc],desc_adic,n_comp,n_Renglon,talonario,t_comp) " +
                            " VALUES(@FILLER,@cod_modelo,@desc,@desc_adic,@n_comp,@N_RENGLON,@talonario,@t_comp)";

                        com.Parameters.Clear();
                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "Interfaz Seincomp";
                        com.Parameters.Add("@cod_modelo", SqlDbType.VarChar).Value = "";
                        if (oPedido.Detalle[x].Descripcion.Length >= 30)
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = oPedido.Detalle[x].Descripcion.Substring(0, 30);
                            string desc_adic = oPedido.Detalle[x].Descripcion.Substring(30, oPedido.Detalle[x].Descripcion.Length - 30).Trim();
                            if (desc_adic.Length >= 20)
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic.Substring(0, 20).Trim();
                            else
                                com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = desc_adic;
                        }
                        else
                        {
                            com.Parameters.Add("@desc", SqlDbType.VarChar).Value = oPedido.Detalle[x].Descripcion;
                            com.Parameters.Add("@desc_adic", SqlDbType.VarChar).Value = "";
                        }
                        com.Parameters.Add("@n_comp", SqlDbType.VarChar).Value = oPedido.Numero;
                        com.Parameters.Add("@N_RENGLON", SqlDbType.VarChar).Value = iRenglon;
                        if (conexion)
                        {
                            com.Parameters.Add("@talonario", SqlDbType.Int).Value = oPedido.TalonarioPedido;
                        }
                        else
                        {
                            com.Parameters.Add("@talonario", SqlDbType.Int).Value = oPedido.TalonarioPedido2;
                        }
                        com.Parameters.Add("@t_comp", SqlDbType.VarChar).Value = "PED";
                        com.ExecuteNonQuery();
                        bool existe = ComprobarDato("STA20", "FILLER", true, oPedido.Numero);
                        if (existe)
                        {
                            com.CommandText = "UPDATE STA20 SET " +
                                          "CANTIDAD=@CANTIDAD,COD_ARTICU=@COD_ARTICU WHERE FILLER=@NRO_PEDIDO";
                            com.Parameters.Clear();
                            com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = oPedido.Detalle[x].Cantidad;
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = oPedido.Detalle[x].Codigo;
                            com.Parameters.Add("@NRO_PEDIDO", SqlDbType.VarChar).Value = oPedido.Numero;
                            com.Connection = cnn;
                            com.ExecuteNonQuery();
                        }

                    }
                    bGuardo = true;
                }
                catch (Exception ex)
                {
                    bGuardo = false;
                    throw ex;
                }
                scope.Complete();
            }


            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool Conectar(string conexion, string conexion2)
        {
            Conexion = conexion;
            Conexion2 = conexion2;

            string sConsulta = @"select a.Estado,a.nro_pedido ,a.talonario,fecha_pedi,a.cod_client,isnull(b.razon_soci, c.RAZON_SOCI) as razon_soci ,a.cod_transp , 
                n_remito as remito, a.cond_vta as cond_vta,a.cod_vended , a.fecha_entr, 
                a.n_lista ,cod_sucurs,a.tipo_asien, 
                a.porc_desc,a.cotiz,a.TOTAL_PEDI,a.cotiz,isnull(id_direccion_entrega, 0) as id_direccion_entrega,leyenda_5, c.domicilio
                from gva21 a
                left join gva14 b on a.COD_CLIENT = b.COD_CLIENT
                left join gva38 c on c.TALONARIO = a.TALON_PED and c.T_COMP = 'PED' and c.N_COMP = a.NRO_PEDIDO
                where TALON_PED =" + Talonario + " ORDER BY fecha_ingreso"; 

            da = new SqlDataAdapter(sConsulta, conexion);
            try
            {
                da.SelectCommand.CommandTimeout = 0;

                da.Fill(ds);

                iRegistros = ds.Tables[0].Rows.Count;
                iLugar = ds.Tables[0].Rows.Count - 1;
                // If iRegistros > 0 Then
                // iLugar = 0
                // Else
                // iLugar = -1
                // End If
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable TraerArticulosDeCliente(string codigo)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            string sConsulta = "";
            try
            {
                sConsulta = "select SEIN_ARTICULOS_CLIENTE_DEFECTO.CodigoArticulo as Codigo, sta11.DESCRIPCIO + ' ' + sta11.desc_adic as Descripcion" + " from SEIN_ARTICULOS_CLIENTE_DEFECTO" + " left join sta11 on SEIN_ARTICULOS_CLIENTE_DEFECTO.CodigoArticulo = sta11.COD_ARTICU" + " where SEIN_ARTICULOS_CLIENTE_DEFECTO.CodigoCliente = '" + codigo + "' and SEIN_ARTICULOS_CLIENTE_DEFECTO.Borrado = 0 ";


                da = new SqlDataAdapter(sConsulta, ConfigurationManager.AppSettings.Get("Conexion").ToString());
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public bool AnularPedido(string nroPedido, int talonario)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            SqlTransaction trans;
            List<PedidosTablaPropia> PedidosReferencia = TraerPedidosDeReferencia(nroPedido);
            cnn.ConnectionString = Conexion;
            com.Connection = cnn;
            cnn.Open();
            trans = cnn.BeginTransaction();
            string pedido1 = "";
            string pedido2 = "";
            string pedido3 = "";
            try
            {
                // TODAS ESTAS QUERYS SE TIENEN Q HACER ASI POR QUE SI EL SISTEMA TIENE 3 PEDIDOS DE UN MISMO CLIENTE DEBE ANULAR LOS 3 PEDIDOS...
                // Y CUANDO EL USUARIO ANULA PODRIA HACERLO DESDE CUALQUIER PEDIDO QUE SE ENCUENTRE.
                int pedido = 0;
                foreach (var item in PedidosReferencia)
                {
                    pedido++;
                    pedido1 = item.PrimerPedido;
                    pedido2 = item.SegundoPedido;
                    pedido3 = item.TercerPedido;
                }

                com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                com.Parameters.Clear();
                com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                com.Parameters.Add("TALONARIO", SqlDbType.Int).Value = talonario;
                com.Connection = cnn;
                com.Transaction = trans;
                com.ExecuteNonQuery();

                if (pedido1 != "" && pedido1 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido1;
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();

                }
                if (pedido2 != "" && pedido2 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido2;
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();
                }
                if(pedido3 != "" && pedido3 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido3;
                    com.Parameters.Add("TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();
                }


                // VALIDO QUE EXISTA EL MOVIMIENTO DE PEDIDO PARA ELIMINAR EL MOVIMIENTO DE STOCK
                if (ComprobarDato("STA14", "FILLER", true, nroPedido))
                {
                    com.CommandText = "UPDATE STA14 SET ESTADO_MOV='A', FECHA_ANU=@FECHA WHERE FILLER=@NUMERO_PEDIDO";
                    com.Parameters.Clear();
                    com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();


                    com.CommandText = "DELETE FROM STA20 WHERE FILLER=@NUMERO_PEDIDO";
                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();

                    if (pedido1 != "" && pedido1 != nroPedido)
                    {
                        com.CommandText = "UPDATE STA14 SET ESTADO_MOV='A', FECHA_ANU=@FECHA WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido1;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();


                        com.CommandText = "DELETE FROM STA20 WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido1;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();

                    }
                    if (pedido2 != "" && pedido2 != nroPedido)
                    {
                        com.CommandText = "UPDATE STA14 SET ESTADO_MOV='A', FECHA_ANU=@FECHA WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido2;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();


                        com.CommandText = "DELETE FROM STA20 WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido2;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();
                    }
                    if (pedido3 != "" && pedido3 != nroPedido)
                    {
                        com.CommandText = "UPDATE STA14 SET ESTADO_MOV='A', FECHA_ANU=@FECHA WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido3;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();


                        com.CommandText = "DELETE FROM STA20 WHERE FILLER=@NUMERO_PEDIDO";
                        com.Parameters.Clear();
                        com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido3;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();
                    }
                }

                bGuardo = true;
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                bGuardo = false;
            }

            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool AnularPedidoBase2(string nroPedido, int talonario)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            SqlTransaction trans;
            List<PedidosTablaPropia> PedidosReferencia = TraerPedidosDeReferencia(nroPedido);
            cnn.ConnectionString = Conexion2;
            com.Connection = cnn;
            string pedido1 = "";
            string pedido2 = "";
            string pedido3 = "";
            cnn.Open();
            trans = cnn.BeginTransaction();
            try
            {

                // TODAS ESTAS QUERYS SE TIENEN Q HACER ASI POR QUE SI EL SISTEMA TIENE 3 PEDIDOS DE UN MISMO CLIENTE DEBE ANULAR LOS 3 PEDIDOS...
                // Y CUANDO EL USUARIO ANULA PODRIA HACERLO DESDE CUALQUIER PEDIDO QUE SE ENCUENTRE.
                int pedido = 0;
                foreach (var item in PedidosReferencia)
                {
                    pedido++;
                    pedido1 = item.PrimerPedido;
                    pedido2 = item.SegundoPedido;
                    pedido3 = item.TercerPedido;
                }

                com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                com.Parameters.Clear();
                com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                com.Parameters.Add("TALONARIO", SqlDbType.Int).Value = talonario;
                com.Connection = cnn;
                com.Transaction = trans;
                com.ExecuteNonQuery();

                if (pedido1 != "" && pedido1 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido1;
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();

                }
                if (pedido2 != "" && pedido2 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido2;
                    com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();
                }
                if (pedido3 != "" && pedido3 != nroPedido)
                {
                    com.CommandText = "UPDATE GVA21 SET ESTADO=5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = pedido3;
                    com.Parameters.Add("TALONARIO", SqlDbType.Int).Value = talonario;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();
                }
                bGuardo = true;
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                bGuardo = false;
            }

            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool ActualizarLeyendas(bool conex, string nroPedido, int talonario, decimal importeDePedido)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            List<PedidosTablaPropia> PedidosReferencia = TraerPedidosDeReferencia(nroPedido);
            string importeTotalNuevo = TraerDato(false, "GVA21", "TOTAL_PEDI", true, "NRO_PEDIDO", true, nroPedido);
            string pedido1 = "";
            string pedido2 = "";
            string pedido3 = "";
            decimal importe = 0;
            if (conex)
            cnn.ConnectionString = Conexion;
            else
            cnn.ConnectionString = Conexion2;
            foreach (var item in PedidosReferencia)
            {
                pedido1 = item.PrimerPedido;
                pedido2 = item.SegundoPedido;
                pedido3 = item.TercerPedido;
                if (pedido1 != "" && pedido1 != nroPedido)
                {
                    importe += Convert.ToDecimal(TraerDato(false, "GVA21", "TOTAL_PEDI", false, "NRO_PEDIDO", true, pedido1 + "' AND TALON_PED='" + talonario));

                }else if(pedido2 != "" && pedido2 != nroPedido)
                {
                    importe += Convert.ToDecimal(TraerDato(false, "GVA21", "TOTAL_PEDI", false, "NRO_PEDIDO", true, pedido2 + "' AND TALON_PED='" + talonario));

                }else if (pedido3 != "" && pedido3 != nroPedido)
                {
                    importe += Convert.ToDecimal(TraerDato(false, "GVA21", "TOTAL_PEDI", false, "NRO_PEDIDO", true, pedido3 + "' AND TALON_PED='" + talonario));
                }
            }
            using (TransactionScope scope = new TransactionScope())
            {
                com.Connection = cnn;
                cnn.Open();
                try
                {
                    com.CommandText = "UPDATE NUMEROS_PEDIDOS_SISTEMA_VENTAS SET IMPORTE_TOTAL_PEDIDO=@IMPORTE_NUEVO WHERE PRIMER_PEDIDO=@NUMERO_PEDIDO OR SEGUNDO_PEDIDO=@NUMERO_PEDIDO OR TERCER_PEDIDO=@NUMERO_PEDIDO";
                    com.Parameters.Clear();
                    com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                    com.Parameters.Add("@IMPORTE_NUEVO", SqlDbType.VarChar).Value = Math.Round(importe, 2);
                    com.Connection = cnn;
                    com.ExecuteNonQuery();

                    bGuardo = true;
                }
                catch(Exception ex)
                {
                    bGuardo = false;
                    throw ex;
                }
                scope.Complete();
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool InsertarEncabezadoReservaStock(Pedido oPedido, string Interno, string n_comp)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            bool bGuardo;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (oConn)
                    {
                        oConn.Open();
                        oComm.Connection = oConn;

                        oComm.CommandText = "INSERT INTO [STA14] " + "(FILLER,COD_PRO_CL,COTIZ,ESTADO_MOV,EXPORTADO,FECHA_MOV,LISTA_REM,LOTE,MON_CTE,MOTIVO_REM,NRO_SUCURS,N_COMP,NCOMP_IN_S,N_REMITO,OBSERVACIO,TALONARIO,T_COMP,TCOMP_IN_S,FECHA_ANU,HORA,LOTE_ANU,NCOMP_ORIG,SUC_ORIG,TCOMP_ORIG,USUARIO,EXP_STOCK,COD_TRANSP,HORA_COMP,ID_A_RENTA,DOC_ELECTR,COD_CLASIF,AUDIT_IMP,IMP_IVA,IMP_OTIMP,IMPORTE_BO,IMPORTE_TO,DIFERENCIA,SUC_DESTIN,T_DOC_DTE,LEYENDA1,LEYENDA2,LEYENDA3,LEYENDA4,LEYENDA5,DCTO_CLIEN,T_INT_ORI,N_INT_ORI,FECHA_INGRESO,HORA_INGRESO,USUARIO_INGRESO,TERMINAL_INGRESO,IMPORTE_TOTAL_CON_IMPUESTOS,CANTIDAD_KILOS,ID_DIRECCION_ENTREGA,IMPORTE_GRAVADO,IMPORTE_EXENTO)" +
                            " VALUES " +
                            "(@FILLER,@COD_PRO_CL,@COTIZ,@ESTADO_MOV,@EXPORTADO,@FECHA_MOV,@LISTA_REM,@LOTE,@MON_CTE,@MOTIVO_REM,@NRO_SUCURS,@N_COMP,@NCOMP_IN_S,@N_REMITO,@OBSERVACIO,@TALONARIO,@T_COMP,@TCOMP_IN_S,@FECHA_ANU,@HORA,@LOTE_ANU,@NCOMP_ORIG,@SUC_ORIG,@TCOMP_ORIG,@USUARIO,@EXP_STOCK,@COD_TRANSP,@HORA_COMP,@ID_A_RENTA,@DOC_ELECTR,@COD_CLASIF,@AUDIT_IMP,@IMP_IVA,@IMP_OTIMP,@IMPORTE_BO,@IMPORTE_TO,@DIFERENCIA,@SUC_DESTIN,@T_DOC_DTE,@LEYENDA1,@LEYENDA2,@LEYENDA3,@LEYENDA4,@LEYENDA5,@DCTO_CLIEN,@T_INT_ORI,@N_INT_ORI,@FECHA_INGRESO,@HORA_INGRESO,@USUARIO_INGRESO,@TERMINAL_INGRESO,@IMPORTE_TOTAL_CON_IMPUESTOS,@CANTIDAD_KILOS,@ID_DIRECCION_ENTREGA,@IMPORTE_GRAVADO,@IMPORTE_EXENTO)";

                        oComm.Parameters.Clear();
                        oComm.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = oPedido.Numero;
                        oComm.Parameters.Add("@COD_PRO_CL", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@COTIZ", SqlDbType.Decimal).Value = oPedido.Cotiz;
                        oComm.Parameters.Add("@ESTADO_MOV", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("ESTADO_MOV").ToString();
                        oComm.Parameters.Add("@EXPORTADO", SqlDbType.Bit).Value = 0;
                        oComm.Parameters.Add("@FECHA_MOV", SqlDbType.DateTime).Value = oPedido.Fecha;
                        oComm.Parameters.Add("@LISTA_REM", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@LOTE", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@MON_CTE", SqlDbType.Bit).Value = 1;
                        oComm.Parameters.Add("@MOTIVO_REM", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@NRO_SUCURS", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@N_COMP", SqlDbType.VarChar).Value = n_comp;
                        oComm.Parameters.Add("@NCOMP_IN_S", SqlDbType.VarChar).Value = Interno;
                        oComm.Parameters.Add("@N_REMITO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@OBSERVACIO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = ConfigurationManager.AppSettings.Get("TalonarioTransferencia").ToString();
                        oComm.Parameters.Add("@T_COMP", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("TCOMP").ToString();
                        oComm.Parameters.Add("@TCOMP_IN_S", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("TCOMP_IN_S").ToString();
                        oComm.Parameters.Add("@FECHA_ANU", SqlDbType.DateTime).Value = new DateTime(1800, 1, 1);
                        oComm.Parameters.Add("@HORA", SqlDbType.VarChar).Value = "0000";
                        oComm.Parameters.Add("@LOTE_ANU", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@NCOMP_ORIG", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@SUC_ORIG", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@TCOMP_ORIG", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = "Seincomp";
                        oComm.Parameters.Add("@EXP_STOCK", SqlDbType.Bit).Value = 0;
                        oComm.Parameters.Add("@COD_TRANSP", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@HORA_COMP", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@ID_A_RENTA", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@DOC_ELECTR", SqlDbType.Bit).Value = 0;
                        oComm.Parameters.Add("@COD_CLASIF", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@AUDIT_IMP", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@IMP_IVA", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@IMP_OTIMP", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@IMPORTE_BO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@IMPORTE_TO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@DIFERENCIA", SqlDbType.VarChar).Value = "N";
                        oComm.Parameters.Add("@SUC_DESTIN", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@T_DOC_DTE", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@LEYENDA1", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@LEYENDA2", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@LEYENDA3", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@LEYENDA4", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@LEYENDA5", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@DCTO_CLIEN", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@T_INT_ORI", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@N_INT_ORI", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = oPedido.Fecha;
                        oComm.Parameters.Add("@HORA_INGRESO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@USUARIO_INGRESO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@TERMINAL_INGRESO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@IMPORTE_TOTAL_CON_IMPUESTOS", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANTIDAD_KILOS", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@ID_DIRECCION_ENTREGA", SqlDbType.Int).Value = DBNull.Value;
                        oComm.Parameters.Add("@IMPORTE_GRAVADO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@IMPORTE_EXENTO", SqlDbType.Decimal).Value = 0;
                        oComm.ExecuteNonQuery();


                    }
                    scope.Complete();
                    bGuardo = true;
                    return bGuardo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertarDetalleMovimientoReserva(Pedido oPedido, string interno, int idMedidaStock, int idMedidaVentas, decimal cantidad, string codigo, int renglon)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            bool bGuardo;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (oConn)
                    {
                        oConn.Open();
                        oComm.Connection = oConn;

                        oComm.CommandText = "INSERT INTO [STA20] " +
                            "(FILLER,CANTIDAD,CANT_DEV,CANT_OC,CANT_PEND,CANT_SCRAP,CAN_EQUI_V,COD_ARTICU,COD_DEPOSI,DEPOSI_DDE,EQUIVALENC,FECHA_MOV,NCOMP_IN_S,N_ORDEN_CO,N_RENGL_OC,N_RENGL_S,PLISTA_REM,PPP_EX,PPP_LO,PRECIO,PRECIO_REM,TIPO_MOV,TCOMP_IN_S,COD_CLASIF,CANT_FACTU,DCTO_FACTU,CANT_DEV_2,CANT_PEND_2,CANTIDAD_2,CANT_FACTU_2,ID_MEDIDA_STOCK_2,ID_MEDIDA_STOCK,ID_MEDIDA_VENTAS,ID_MEDIDA_COMPRA,UNIDAD_MEDIDA_SELECCIONADA,PRECIO_REMITO_VENTAS,CANT_OC_2,RENGL_PADR,COD_ARTICU_KIT,PROMOCION,PRECIO_ADICIONAL_KIT,TALONARIO_OC)" +
                            " VALUES " +
                            "(@FILLER,@CANTIDAD,@CANT_DEV,@CANT_OC,@CANT_PEND,@CANT_SCRAP,@CAN_EQUI_V,@COD_ARTICU,@COD_DEPOSI,@DEPOSI_DDE,@EQUIVALENC,@FECHA_MOV,@NCOMP_IN_S,@N_ORDEN_CO,@N_RENGL_OC,@N_RENGL_S,@PLISTA_REM,@PPP_EX,@PPP_LO,@PRECIO,@PRECIO_REM,@TIPO_MOV,@TCOMP_IN_S,@COD_CLASIF,@CANT_FACTU,@DCTO_FACTU,@CANT_DEV_2,@CANT_PEND_2,@CANTIDAD_2,@CANT_FACTU_2,@ID_MEDIDA_STOCK_2,@ID_MEDIDA_STOCK,@ID_MEDIDA_VENTAS,@ID_MEDIDA_COMPRA,@UNIDAD_MEDIDA_SELECCIONADA,@PRECIO_REMITO_VENTAS,@CANT_OC_2,@RENGL_PADR,@COD_ARTICU_KIT,@PROMOCION,@PRECIO_ADICIONAL_KIT,@TALONARIO_OC)";

                        oComm.Parameters.Clear();
                        oComm.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = oPedido.Numero;
                        oComm.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        oComm.Parameters.Add("@CANT_DEV", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_OC", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_PEND", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_SCRAP", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CAN_EQUI_V", SqlDbType.Decimal).Value = 1;
                        oComm.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = codigo;
                        oComm.Parameters.Add("@COD_DEPOSI", SqlDbType.VarChar).Value = oPedido.Deposito;
                        oComm.Parameters.Add("@DEPOSI_DDE", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@EQUIVALENC", SqlDbType.Decimal).Value = 1;
                        oComm.Parameters.Add("@FECHA_MOV", SqlDbType.DateTime).Value = oPedido.Fecha;
                        oComm.Parameters.Add("@NCOMP_IN_S", SqlDbType.VarChar).Value = interno;
                        oComm.Parameters.Add("@N_ORDEN_CO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@N_RENGL_OC", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@N_RENGL_S", SqlDbType.Int).Value = renglon;
                        oComm.Parameters.Add("@PLISTA_REM", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PPP_EX", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PPP_LO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PRECIO_REM", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@TIPO_MOV", SqlDbType.VarChar).Value = "S";
                        oComm.Parameters.Add("@TCOMP_IN_S", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("TCOMP_IN_S").ToString();
                        oComm.Parameters.Add("@COD_CLASIF", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@CANT_FACTU", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@DCTO_FACTU", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_DEV_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_PEND_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANTIDAD_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_FACTU_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@ID_MEDIDA_STOCK_2", SqlDbType.Int).Value = DBNull.Value; // traerDato(false, "STA11", "ID_MEDIDA_STOCK_2", false, "COD_ARTICU", true, item.Codigo);/// traer dato, de sta11. 
                        oComm.Parameters.Add("@ID_MEDIDA_STOCK", SqlDbType.Int).Value = idMedidaStock; /// traer dato, de sta11. 
                        oComm.Parameters.Add("@ID_MEDIDA_VENTAS", SqlDbType.Int).Value = idMedidaVentas; /// traer dato, de sta11.  codarticu
                        oComm.Parameters.Add("@ID_MEDIDA_COMPRA", SqlDbType.Int).Value = DBNull.Value;
                        oComm.Parameters.Add("@UNIDAD_MEDIDA_SELECCIONADA", SqlDbType.VarChar).Value = "P";
                        oComm.Parameters.Add("@PRECIO_REMITO_VENTAS", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_OC_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@RENGL_PADR", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@COD_ARTICU_KIT", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@PROMOCION", SqlDbType.Bit).Value = 0;
                        oComm.Parameters.Add("@PRECIO_ADICIONAL_KIT", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@TALONARIO_OC", SqlDbType.Int).Value = DBNull.Value;
                        //oComm.Parameters.Add("@ID_STA11", SqlDbType.Int).Value = idSta11;
                        //oComm.Parameters.Add("@ID_STA14", SqlDbType.Int).Value = idSta14;//traerDato id
                        oComm.ExecuteNonQuery();

                        renglon++;

                        oComm.Parameters.Clear();
                        oComm.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = oPedido.Numero;
                        oComm.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        oComm.Parameters.Add("@CANT_DEV", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_OC", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_PEND", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_SCRAP", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CAN_EQUI_V", SqlDbType.Decimal).Value = 1;
                        oComm.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = codigo;
                        oComm.Parameters.Add("@COD_DEPOSI", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                        oComm.Parameters.Add("@DEPOSI_DDE", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@EQUIVALENC", SqlDbType.Decimal).Value = 1;
                        oComm.Parameters.Add("@FECHA_MOV", SqlDbType.DateTime).Value = oPedido.Fecha;
                        oComm.Parameters.Add("@NCOMP_IN_S", SqlDbType.VarChar).Value = interno;
                        oComm.Parameters.Add("@N_ORDEN_CO", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@N_RENGL_OC", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@N_RENGL_S", SqlDbType.Int).Value = renglon;
                        oComm.Parameters.Add("@PLISTA_REM", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PPP_EX", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PPP_LO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@PRECIO_REM", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@TIPO_MOV", SqlDbType.VarChar).Value = "E";
                        oComm.Parameters.Add("@TCOMP_IN_S", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("TCOMP_IN_S").ToString();
                        oComm.Parameters.Add("@COD_CLASIF", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@CANT_FACTU", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@DCTO_FACTU", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_DEV_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_PEND_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANTIDAD_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_FACTU_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@ID_MEDIDA_STOCK_2", SqlDbType.Int).Value = DBNull.Value; //traerDato(false, "STA11", "ID_MEDIDA_STOCK_2", false, "COD_ARTICU", true ,item.Codigo);/// traer dato, de sta11. 
                        oComm.Parameters.Add("@ID_MEDIDA_STOCK", SqlDbType.Int).Value = idMedidaStock; /// traer dato, de sta11. 
                        oComm.Parameters.Add("@ID_MEDIDA_VENTAS", SqlDbType.Int).Value = idMedidaVentas;
                        oComm.Parameters.Add("@ID_MEDIDA_COMPRA", SqlDbType.Int).Value = DBNull.Value;
                        oComm.Parameters.Add("@UNIDAD_MEDIDA_SELECCIONADA", SqlDbType.VarChar).Value = "P";
                        oComm.Parameters.Add("@PRECIO_REMITO_VENTAS", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@CANT_OC_2", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@RENGL_PADR", SqlDbType.Int).Value = 0;
                        oComm.Parameters.Add("@COD_ARTICU_KIT", SqlDbType.VarChar).Value = "";
                        oComm.Parameters.Add("@PROMOCION", SqlDbType.Bit).Value = 0;
                        oComm.Parameters.Add("@PRECIO_ADICIONAL_KIT", SqlDbType.Decimal).Value = 0;
                        oComm.Parameters.Add("@TALONARIO_OC", SqlDbType.Int).Value = DBNull.Value;
                        //oComm.Parameters.Add("@ID_STA11", SqlDbType.Int).Value = idSta11;
                        //oComm.Parameters.Add("@ID_STA14", SqlDbType.Int).Value = idSta14;//traerDato id
                        oComm.ExecuteNonQuery();

                    }
                    scope.Complete();
                    bGuardo = true;
                    return bGuardo;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void RecomponerSaldos()
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;
                    oComm.CommandText =
                           @"
                             exec sp_RecomposicionSaldosStock
                            ";
                    oComm.Parameters.Clear();
                    oComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TraerNumeroComprobante(string talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string numeroVenta = "";
            string sucursal = "";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT PROXIMO, SUCURSAL FROM STA17 WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        numeroVenta = reader["PROXIMO"].ToString();
                        sucursal = reader["SUCURSAL"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            numeroVenta = numeroVenta.PadLeft(8, '0');
            sucursal = sucursal.PadLeft(5, '0');
            string N_COMP = " " + sucursal + numeroVenta;
            return N_COMP;
        }

        public string TraerProximoParaReferenciaDePedido(string talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string tipo = "";
            string numeroVenta = "";
            string sucursal = "";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT CASE WHEN TIPO = '' THEN ' ' ELSE TIPO END as TIPO,SUCURSAL,dbo.fn_obtenerproximonumero(PROXIMO)+1 AS PROXIMO FROM GVA43 WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = reader["TIPO"].ToString();
                        numeroVenta = reader["PROXIMO"].ToString();
                        sucursal = reader["SUCURSAL"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            numeroVenta = numeroVenta.PadLeft(8, '0');
            sucursal = sucursal.PadLeft(5, '0');
            string NRO_PEDIDO = tipo + sucursal + numeroVenta;
            return NRO_PEDIDO;
        }

        public string TraerProximoNumeroDePedido(string talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string tipo = "";
            string numeroVenta = "";
            string sucursal = "";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT CASE WHEN TIPO = '' THEN ' ' ELSE TIPO END as TIPO,SUCURSAL,dbo.fn_obtenerproximonumero(PROXIMO) AS PROXIMO FROM GVA43 WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = reader["TIPO"].ToString();
                        numeroVenta = reader["PROXIMO"].ToString();
                        sucursal = reader["SUCURSAL"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            numeroVenta = numeroVenta.PadLeft(8, '0');
            sucursal = sucursal.PadLeft(5, '0');
            string NRO_PEDIDO = tipo + sucursal + numeroVenta;
            return NRO_PEDIDO;
        }

        public string TraerProximoNumeroPedidoIngresado(string talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string numeroVenta = "";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT dbo.fn_obtenerproximonumero(PROXIMO) AS PROXIMO FROM GVA43 WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        numeroVenta = reader["PROXIMO"].ToString();
                    }
                }
                numeroVenta = numeroVenta.PadLeft(8, '0');
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numeroVenta;
        }

        public void ActualizarProximoGva43(string talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            try
            {
                string nroVenta = TraerProximoNumeroPedidoIngresado(talonario);
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;
                    oComm.CommandText =
                           @"
                             UPDATE GVA43 SET PROXIMO = dbo.fn_encryptarProximoNumero(RIGHT('00000000' + LTRIM(RTRIM(@NumeroPedidoUltimaParte + 1)),8))  where talonario = @TALONARIO
                            ";
                    oComm.Parameters.Clear();
                    oComm.Parameters.AddWithValue("@NumeroPedidoUltimaParte", nroVenta);
                    oComm.Parameters.AddWithValue("@TALONARIO", talonario);
                    oComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TraerInterno()
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string proximo = "";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT isnull(CONVERT(VarChar, CAST(MAX(NCOMP_IN_S) AS INT) + 1),1) AS PROXIMO FROM STA14 WHERE TCOMP_IN_S = '" + ConfigurationManager.AppSettings.Get("TCOMP_IN_S").ToString() + "'";
                    oComm.Parameters.Clear();
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        proximo = reader["PROXIMO"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return proximo;
        }

        public bool InsertarSiguienteSTA17(int talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            bool bGuardo;
            try
            {
                int ProximoAinsertar = TraerProximoSTA17(talonario) + 1;
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = @"UPDATE [STA17] SET PROXIMO=@PROXIMO WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.AddWithValue("@PROXIMO", ProximoAinsertar);
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    oComm.ExecuteNonQuery();
                }
                bGuardo = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bGuardo;
        }

        public int TraerProximoSTA17(int talonario)
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            int proximo = 0;
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT isnull(CONVERT(VarChar, CAST(MAX(PROXIMO) AS INT)),1) AS PROXIMO FROM STA17 WHERE TALONARIO=@TALONARIO";
                    oComm.Parameters.Clear();
                    oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        proximo = Convert.ToInt32(reader["PROXIMO"].ToString());
                    }
                    oComm.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proximo;
        }

        public bool EliminarReservaDePedido(string nroPedido)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            SqlTransaction trans;

            cnn.ConnectionString = Conexion;
            com.Connection = cnn;
            cnn.Open();
            trans = cnn.BeginTransaction();
            try
            {
                com.CommandText = @"delete from sta20 where filler  in (select filler from sta14 where filler=@NUMERO_PEDIDO AND TALONARIO=@TALONARIO)
                                        delete from sta14 where filler = @NUMERO_PEDIDO AND TALONARIO=@TALONARIO";

                com.Parameters.Clear();
                com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = ConfigurationManager.AppSettings.Get("TalonarioTransferencia").ToString();

                com.Connection = cnn;
                com.Transaction = trans;
                com.ExecuteNonQuery();

                bGuardo = true;
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                bGuardo = false;
                throw ex;
            }

            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool RestarStockSTA19(decimal cantidad, string codigo, string deposito)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            cnn.ConnectionString = Conexion;
            com.Connection = cnn;
            bool existe = ComprobarDato("STA19", "COD_ARTICU", true, codigo + "' AND COD_DEPOSI='" + deposito + "");
            bool ExisteEnDepositoReserva = ComprobarDato("STA19", "COD_ARTICU", true, codigo + "' AND COD_DEPOSI='" + ConfigurationManager.AppSettings.Get("DepositoDestino").ToString() + "");
            cnn.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //COMPRUEBO Q EXISTA EL ARTICULO EN EL DEPOSITO DONDE SACARE LA MERCADERIA. SI EXISTE UPDATEO, SI NO, INSERTO.
                    if (existe)
                    {
                        com.CommandText = "UPDATE STA19 SET CANT_STOCK= CANT_STOCK - @CANTIDAD, FECHA_ANT=@FECHA WHERE COD_ARTICU=@CODIGO AND COD_DEPOSI=@DEPOSITO";

                        com.Parameters.Clear();
                        com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        com.Parameters.Add("@DEPOSITO", SqlDbType.VarChar).Value = deposito;
                        com.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Connection = cnn;
                        com.ExecuteNonQuery();
                        
                        //COMPRUEBO Q EXISTA EL ARTICULO EN EL DEPOSITO DE DESTINO DE LA MERCADERIA. SI EXISTE UPDATEO, SI NO, INSERTO.
                        if (ExisteEnDepositoReserva)
                        {
                            com.CommandText = "UPDATE STA19 SET CANT_STOCK= CANT_STOCK + @CANTIDAD, FECHA_ANT=@FECHA WHERE COD_ARTICU=@CODIGO AND COD_DEPOSI=@DEPOSITO";

                            com.Parameters.Clear();
                            com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                            com.Parameters.Add("@DEPOSITO", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                            com.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                            com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                            com.Connection = cnn;
                            com.ExecuteNonQuery();
                        }
                        else
                        {
                            com.CommandText = "INSERT INTO STA19(FILLER, CANT_COMP, CANT_PEND, CANT_STOCK, COD_ARTICU, COD_DEPOSI, FECHA_ANT)" +
                           "VALUES" +
                           "(@FILLER, @CANT_COMP, @CANT_PEND, @CANTIDAD, @COD_ARTICU, @COD_DEPOSI, @FECHA)";

                            com.Parameters.Clear();
                            com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "SISTEMA";
                            com.Parameters.Add("@CANT_COMP", SqlDbType.Decimal).Value = 0;
                            com.Parameters.Add("@CANT_PEND", SqlDbType.Decimal).Value = 0;
                            com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = codigo;
                            com.Parameters.Add("@COD_DEPOSI", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                            com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                            com.Connection = cnn;
                            com.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        com.CommandText = "INSERT INTO STA19(FILLER, CANT_COMP, CANT_PEND, CANT_STOCK, COD_ARTICU, COD_DEPOSI, FECHA_ANT)" +
                            "VALUES" +
                            "(@FILLER, @CANT_COMP, @CANT_PEND, @CANTIDAD, @COD_ARTICU, @COD_DEPOSI, @FECHA)";

                        com.Parameters.Clear();
                        com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "SISTEMA";
                        com.Parameters.Add("@CANT_COMP", SqlDbType.Decimal).Value = 0;
                        com.Parameters.Add("@CANT_PEND", SqlDbType.Decimal).Value = 0;
                        com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = -cantidad;
                        com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = codigo;
                        com.Parameters.Add("@COD_DEPOSI", SqlDbType.VarChar).Value = deposito;
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Connection = cnn;
                        com.ExecuteNonQuery();

                        if (ExisteEnDepositoReserva)
                        {
                            com.CommandText = "UPDATE STA19 SET CANT_STOCK= CANT_STOCK + @CANTIDAD, FECHA_ANT=@FECHA WHERE COD_ARTICU=@CODIGO AND COD_DEPOSI=@DEPOSITO";

                            com.Parameters.Clear();
                            com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                            com.Parameters.Add("@DEPOSITO", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                            com.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                            com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                            com.Connection = cnn;
                            com.ExecuteNonQuery();
                        }
                        else
                        {
                            com.CommandText = "INSERT INTO STA19(FILLER, CANT_COMP, CANT_PEND, CANT_STOCK, COD_ARTICU, COD_DEPOSI, FECHA_ANT)" +
                           "VALUES" +
                           "(@FILLER, @CANT_COMP, @CANT_PEND, @CANTIDAD, @COD_ARTICU, @COD_DEPOSI, @FECHA)";

                            com.Parameters.Clear();
                            com.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "SISTEMA";
                            com.Parameters.Add("@CANT_COMP", SqlDbType.Decimal).Value = 0;
                            com.Parameters.Add("@CANT_PEND", SqlDbType.Decimal).Value = 0;
                            com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                            com.Parameters.Add("@COD_ARTICU", SqlDbType.VarChar).Value = codigo;
                            com.Parameters.Add("@COD_DEPOSI", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                            com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                            com.Connection = cnn;
                            com.ExecuteNonQuery();
                        }

                    }
                    bGuardo = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool SumarStockSTA19(decimal cantidad, string codigo, string deposito)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            SqlTransaction trans;

            cnn.ConnectionString = Conexion;
            com.Connection = cnn;
            bool existe = ComprobarDato("STA19", "COD_ARTICU", true, codigo + "' AND COD_DEPOSI='" + deposito + "");
            bool ExisteEnDepositoReserva = ComprobarDato("STA19", "COD_ARTICU", true, codigo + "' AND COD_DEPOSI='" + ConfigurationManager.AppSettings.Get("DepositoDestino").ToString() + "");
            cnn.Open();
            trans = cnn.BeginTransaction();
            try
            {
                //COMPRUEBO Q EXISTA EL ARTICULO EN EL DEPOSITO DONDE SACARE LA MERCADERIA. SI EXISTE UPDATEO, SI NO, INSERTO.
                if (existe)
                {
                    com.CommandText = "UPDATE STA19 SET CANT_STOCK= CANT_STOCK + @CANTIDAD, FECHA_ANT=@FECHA WHERE COD_ARTICU =@CODIGO AND COD_DEPOSI=@DEPOSITO";

                    com.Parameters.Clear();
                    com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                    com.Parameters.Add("@DEPOSITO", SqlDbType.VarChar).Value = deposito;
                    com.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                    com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                    com.Connection = cnn;
                    com.Transaction = trans;
                    com.ExecuteNonQuery();

                    //COMPRUEBO Q EXISTA EL ARTICULO EN EL DEPOSITO DE DESTINO. SI EXISTE UPDATEO, SI NO, INSERTO.
                    if (ExisteEnDepositoReserva)
                    {
                        com.CommandText = "UPDATE STA19 SET CANT_STOCK= CANT_STOCK - @CANTIDAD, FECHA_ANT=@FECHA WHERE COD_ARTICU =@CODIGO AND COD_DEPOSI=@DEPOSITO";

                        com.Parameters.Clear();
                        com.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        com.Parameters.Add("@DEPOSITO", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings.Get("DepositoDestino").ToString();
                        com.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                        com.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                        com.Connection = cnn;
                        com.Transaction = trans;
                        com.ExecuteNonQuery();
                    }
                    bGuardo = true;
                    trans.Commit();
                }
                else
                {
                    bGuardo = false;
                }
            }
            catch
            {
                trans.Rollback();
                bGuardo = false;
            }

            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool GuardarClienteOcasional(bool baseEmpresa1, ClienteOcasional cliente)
        {
            SqlConnection oConn = new SqlConnection("");
            SqlCommand oComm = new SqlCommand();
            SqlTransaction oTrans;
            bool bGuardo;
            if (baseEmpresa1)
            {
                oConn.ConnectionString = Conexion;
            }
            else
            {
                oConn.ConnectionString = Conexion2;
            }
            oConn.Open();
            oTrans = oConn.BeginTransaction();
            try
            {
                oComm.CommandText = "INSERT INTO [GVA38] " +
                    "(DOMICILIO,E_MAIL,IVA_D,IVA_L,LOCALIDAD,N_COMP,N_CUIT,RAZON_SOCI,TALONARIO,TELEFONO_1,TELEFONO_2,TIPO_DOC,T_COMP,DIRECCION_ENTREGA,TELEFONO1_ENTREGA,ID_CATEGORIA_IVA,MAIL_DE,FECHA_NACIMIENTO)" +
                    " VALUES " +
                    "(@DOMICILIO,@E_MAIL,@IVA_D,@IVA_L,@LOCALIDAD,@N_COMP,@N_CUIT,@RAZON_SOCI,@TALONARIO,@TELEFONO_1,@TELEFONO_2,@TIPO_DOC,@T_COMP,@DIRECCION_ENTREGA,@TELEFONO1_ENTREGA,@ID_CATEGORIA_IVA,@MAIL_DE,@FECHA_NACIMIENTO)";

                oComm.Parameters.Clear();
                oComm.Parameters.Add("@DOMICILIO", SqlDbType.VarChar).Value = cliente.DOMICILIO;
                oComm.Parameters.Add("@E_MAIL", SqlDbType.VarChar).Value = cliente.E_MAIL;
                oComm.Parameters.Add("@IVA_D", SqlDbType.VarChar).Value = cliente.IVA_D;
                oComm.Parameters.Add("@IVA_L", SqlDbType.VarChar).Value =cliente.IVA_L;
                oComm.Parameters.Add("@LOCALIDAD", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@N_COMP", SqlDbType.VarChar).Value = cliente.N_COMP;
                oComm.Parameters.Add("@N_CUIT", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@RAZON_SOCI", SqlDbType.VarChar).Value = cliente.RAZON_SOCI;
                oComm.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = cliente.TALONARIO;
                oComm.Parameters.Add("@TELEFONO_1", SqlDbType.VarChar).Value = cliente.TELEFONO_1;
                oComm.Parameters.Add("@TELEFONO_2", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@TIPO_DOC", SqlDbType.Int).Value = 96;
                oComm.Parameters.Add("@T_COMP", SqlDbType.VarChar).Value = "PED";
                oComm.Parameters.Add("@DIRECCION_ENTREGA", SqlDbType.VarChar).Value = cliente.DIRECCION_ENTREGA;
                oComm.Parameters.Add("@TELEFONO1_ENTREGA", SqlDbType.VarChar).Value = cliente.TELEFONO_1;
                oComm.Parameters.Add("@ID_CATEGORIA_IVA", SqlDbType.Int).Value = cliente.ID_CATEGORIA_IVA;// traer dato
                oComm.Parameters.Add("@MAIL_DE", SqlDbType.VarChar).Value = cliente.E_MAIL;
                oComm.Parameters.Add("@FECHA_NACIMIENTO", SqlDbType.DateTime).Value = new DateTime(1800,01,01);
                //oComm.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = DBNull.Value;
                oComm.Transaction = oTrans;
                oComm.Connection = oConn;
                oComm.ExecuteNonQuery();
                oComm.Transaction.Commit();
                bGuardo = true;
            }
            catch (Exception ex)
            {
                oComm.Transaction.Rollback();
                bGuardo = false;
                throw ex;
            }
            finally
            {
                if (oConn.State == ConnectionState.Open)
                {
                    oConn.Close();
                }
            }

            return bGuardo;
        }

        public bool GuardarClienteGva14(bool Base, ClienteGva14 cliente)
        {

            SqlConnection oConn = new SqlConnection();
            SqlCommand oComm = new SqlCommand();
            SqlTransaction oTrans;
            bool bGuardo;
            if (Base)
            {
                oConn.ConnectionString = Conexion;
            }
            else
            {
                oConn.ConnectionString = Conexion2;
            }
            oComm.Connection = oConn;
            oConn.Open();
            oTrans = oConn.BeginTransaction();
            try
            {
                oComm.CommandText = "INSERT INTO [GVA14] " + 
                    "(FILLER,COD_CLIENT,COD_PROVIN,COD_TRANSP,COD_VENDED,COD_ZONA,COND_VTA,CUIT,DOMICILIO,E_MAIL,FECHA_ALTA,II_D,II_L,IVA_D,IVA_L,LOCALIDAD,NRO_LISTA,RAZON_SOCI,TELEFONO_1,TIPO,TIPO_DOC,MAIL_DE,COD_GVA14,TELEFONO_MOVIL,ID_CATEGORIA_IVA,INHABILITADO_NEXO_PEDIDOS,EXPORTA,COD_GVA18,COD_GVA05)" +
                    " VALUES " +
                    "(@FILLER,@COD_CLIENT,@COD_PROVIN,@COD_TRANSP,@COD_VENDED,@COD_ZONA,@COND_VTA,@CUIT,@DOMICILIO,@E_MAIL,@FECHA_ALTA,@II_D,@II_L,@IVA_D,@IVA_L,@LOCALIDAD,@NRO_LISTA,@RAZON_SOCI,@TELEFONO_1,@TIPO,@TIPO_DOC,@MAIL_DE,@COD_GVA14,@TELEFONO_MOVIL,@ID_CATEGORIA_IVA,@INHABILITADO_NEXO_PEDIDOS,@EXPORTA,@COD_GVA18,@COD_GVA05)";
                
                oComm.Parameters.Clear();
                oComm.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = cliente.COD_CLIENT;
                oComm.Parameters.Add("@COD_PROVIN", SqlDbType.VarChar).Value = cliente.COD_PROVIN;
                oComm.Parameters.Add("@COD_TRANSP", SqlDbType.VarChar).Value = "01";
                oComm.Parameters.Add("@COD_VENDED", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@COD_ZONA", SqlDbType.VarChar).Value = "UN";
                oComm.Parameters.Add("@COND_VTA", SqlDbType.Int).Value = cliente.COND_VTA;
                oComm.Parameters.Add("@CUIT", SqlDbType.VarChar).Value = cliente.CUIT;
                oComm.Parameters.Add("@DOMICILIO", SqlDbType.VarChar).Value = cliente.DOMICILIO;
                oComm.Parameters.Add("@E_MAIL", SqlDbType.VarChar).Value = cliente.E_MAIL;
                oComm.Parameters.Add("@FECHA_ALTA", SqlDbType.DateTime).Value = DateTime.Now;
                oComm.Parameters.Add("@II_D", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@II_L", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@IVA_D", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@IVA_L", SqlDbType.VarChar).Value = "S";
                oComm.Parameters.Add("@LOCALIDAD", SqlDbType.VarChar).Value = cliente.LOCALIDAD;
                oComm.Parameters.Add("@NRO_LISTA", SqlDbType.Int).Value = 0;
                oComm.Parameters.Add("@RAZON_SOCI", SqlDbType.VarChar).Value = cliente.RAZON_SOCI;
                oComm.Parameters.Add("@TELEFONO_1", SqlDbType.VarChar).Value = cliente.TELEFONO_1;
                oComm.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@TIPO_DOC", SqlDbType.Int).Value = 96;
                oComm.Parameters.Add("@MAIL_DE", SqlDbType.VarChar).Value = cliente.E_MAIL; ;
                oComm.Parameters.Add("@COD_GVA14", SqlDbType.VarChar).Value = cliente.COD_CLIENT;
                oComm.Parameters.Add("@TELEFONO_MOVIL", SqlDbType.VarChar).Value = cliente.TELEFONO_1;
                oComm.Parameters.Add("@ID_CATEGORIA_IVA", SqlDbType.Int).Value = 2;
                oComm.Parameters.Add("@INHABILITADO_NEXO_PEDIDOS", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@EXPORTA", SqlDbType.Bit).Value = 1;
                oComm.Parameters.Add("@COD_GVA18", SqlDbType.VarChar).Value = cliente.COD_PROVIN;
                oComm.Parameters.Add("@COD_GVA05", SqlDbType.VarChar).Value = "UN";
                oComm.Transaction = oTrans;
                oComm.ExecuteNonQuery();

                oComm.CommandText = "INSERT INTO [DIRECCION_ENTREGA] " + 
                    "(COD_DIRECCION_ENTREGA,COD_CLIENTE,DIRECCION,COD_PROVINCIA,LOCALIDAD,HABITUAL,CODIGO_POSTAL,TELEFONO1,TELEFONO2,TOMA_IMPUESTO_HABITUAL,FILLER,OBSERVACIONES,AL_FIJ_IB3,ALI_ADI_IB,ALI_FIJ_IB,IB_L,IB_L3,II_IB3,LIB,PORC_L,HABILITADO,HORARIO_ENTREGA,ENTREGA_LUNES,ENTREGA_MARTES,ENTREGA_MIERCOLES,ENTREGA_JUEVES,ENTREGA_VIERNES,ENTREGA_SABADO,ENTREGA_DOMINGO,CONSIDERA_IVA_BASE_CALCULO_IIBB,CONSIDERA_IVA_BASE_CALCULO_IIBB_ADIC,WEB_ADDRESS_ID)" + 
                    " VALUES " + 
                    "(@COD_DIRECCION_ENTREGA,@COD_CLIENTE,@DIRECCION,@COD_PROVINCIA,@LOCALIDAD,@HABITUAL,@CODIGO_POSTAL,@TELEFONO1,@TELEFONO2,@TOMA_IMPUESTO_HABITUAL,@FILLER,@OBSERVACIONES,@AL_FIJ_IB3,@ALI_ADI_IB,@ALI_FIJ_IB,@IB_L,@IB_L3,@II_IB3,@LIB,@PORC_L,@HABILITADO,@HORARIO_ENTREGA,@ENTREGA_LUNES,@ENTREGA_MARTES,@ENTREGA_MIERCOLES,@ENTREGA_JUEVES,@ENTREGA_VIERNES,@ENTREGA_SABADO,@ENTREGA_DOMINGO,@CONSIDERA_IVA_BASE_CALCULO_IIBB,@CONSIDERA_IVA_BASE_CALCULO_IIBB_ADIC,@WEB_ADDRESS_ID)";
                
                oComm.Parameters.Clear();
                oComm.Parameters.Add("@COD_DIRECCION_ENTREGA", SqlDbType.VarChar).Value = "PRINCIPAL";
                oComm.Parameters.Add("@COD_CLIENTE", SqlDbType.VarChar).Value = cliente.COD_CLIENT;
                oComm.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = cliente.DOMICILIO;
                oComm.Parameters.Add("@COD_PROVINCIA", SqlDbType.VarChar).Value = "01";
                oComm.Parameters.Add("@LOCALIDAD", SqlDbType.VarChar).Value = cliente.LOCALIDAD;
                oComm.Parameters.Add("@HABITUAL", SqlDbType.VarChar).Value = "S";
                oComm.Parameters.Add("@CODIGO_POSTAL", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@TELEFONO1", SqlDbType.VarChar).Value = cliente.TELEFONO_1;
                oComm.Parameters.Add("@TELEFONO2", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@TOMA_IMPUESTO_HABITUAL", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@FILLER", SqlDbType.VarChar).Value = "Sistema de Ventas";
                oComm.Parameters.Add("@OBSERVACIONES", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@AL_FIJ_IB3", SqlDbType.Int).Value = 0;
                oComm.Parameters.Add("@ALI_ADI_IB", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@ALI_FIJ_IB", SqlDbType.VarChar).Value = "";
                oComm.Parameters.Add("@IB_L", SqlDbType.Bit).Value = 0;
                oComm.Parameters.Add("@IB_L3", SqlDbType.Bit).Value = 0;
                oComm.Parameters.Add("@II_IB3", SqlDbType.Bit).Value = 0;
                oComm.Parameters.Add("@LIB", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@PORC_L", SqlDbType.Decimal).Value = 0;
                oComm.Parameters.Add("@HABILITADO", SqlDbType.VarChar).Value = "S";
                oComm.Parameters.Add("@HORARIO_ENTREGA", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_LUNES", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_MARTES", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_MIERCOLES", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_JUEVES", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_VIERNES", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_SABADO", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@ENTREGA_DOMINGO", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@CONSIDERA_IVA_BASE_CALCULO_IIBB", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@CONSIDERA_IVA_BASE_CALCULO_IIBB_ADIC", SqlDbType.VarChar).Value = "N";
                oComm.Parameters.Add("@WEB_ADDRESS_ID", SqlDbType.Int).Value = "0";

                oComm.Transaction = oTrans;
                oComm.ExecuteNonQuery();
                oComm.Transaction.Commit();
                bGuardo = true;
            }
            catch (Exception ex)
            {
                oComm.Transaction.Rollback();
                bGuardo = false;
                throw ex;
            }
            finally
            {
                if (oConn.State == ConnectionState.Open)
                {
                    oConn.Close();
                }
            }

            return bGuardo;
        }

        public string TraerProximoCodigoCliente()
        {
            SqlConnection oConn = new SqlConnection(Conexion);
            SqlCommand oComm = new SqlCommand();
            string proximo="";
            try
            {
                using (oConn)
                {
                    oConn.Open();
                    oComm.Connection = oConn;

                    oComm.CommandText = "SELECT MAX(COD_CLIENT)+1 as COD_CLIENT FROM GVA14 WHERE ISNUMERIC(COD_CLIENT)=1 AND LEN(COD_CLIENT)=4";
                    oComm.ExecuteNonQuery();
                    SqlDataReader reader = oComm.ExecuteReader();
                    if (reader.Read())
                    {
                        proximo = reader["COD_CLIENT"].ToString();
                        proximo = proximo.PadLeft(6, '0');
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proximo;
        }

        public bool ActualizarEstadoDePedidoYLeyenda5(string nroPedido, int talonario, int estado, bool reservando)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand com = new SqlCommand();
            bool bGuardo;
            SqlTransaction trans;
            cnn.ConnectionString = Conexion;
            com.Connection = cnn;
            cnn.Open();
            trans = cnn.BeginTransaction();
            string leyenda5;
            if (reservando)
            {
                leyenda5 = ConfigurationManager.AppSettings.Get("Reservando");
            }
            else
            {
                leyenda5 = ConfigurationManager.AppSettings.Get("EliminandoReserva");
            }
            try
            {
                com.CommandText = "UPDATE GVA21 SET ESTADO=@ESTADO,LEYENDA_5=@LEYENDA_5 WHERE NRO_PEDIDO=@NUMERO_PEDIDO AND TALON_PED=@TALONARIO";

                com.Parameters.Clear();
                com.Parameters.Add("@NUMERO_PEDIDO", SqlDbType.VarChar).Value = nroPedido;
                com.Parameters.Add("@TALONARIO", SqlDbType.Int).Value = talonario;
                com.Parameters.Add("@ESTADO", SqlDbType.Int).Value = estado;
                com.Parameters.Add("@LEYENDA_5", SqlDbType.VarChar).Value = leyenda5;
                com.Connection = cnn;
                com.Transaction = trans;
                com.ExecuteNonQuery();
                bGuardo = true;
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                bGuardo = false;
            }

            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            return bGuardo;
        }

        public bool IngresarTablaPropiaDeReferencia(bool conex, string primerPedido, string segundoPedido, string tercerPedido, string totalGeneralPedido)
        {
            SqlConnection oConn = new SqlConnection("");
            SqlCommand oComm = new SqlCommand();
            SqlTransaction oTrans;
            bool bGuardo= false;
            if (conex)
            {
                oConn.ConnectionString = Conexion;
            }
            else
            {
                oConn.ConnectionString = Conexion2;
            }
            oConn.Open();
            oTrans = oConn.BeginTransaction();
            oComm.Connection = oConn;
            try
            {
                oComm.CommandText = "INSERT INTO [NUMEROS_PEDIDOS_SISTEMA_VENTAS] " + "(PRIMER_PEDIDO,SEGUNDO_PEDIDO,TERCER_PEDIDO,TOTAL_IMPORTE_PEDIDO)" +
                    " VALUES " +
                    "(@PRIMER_PEDIDO,@SEGUNDO_PEDIDO,@TERCER_PEDIDO,@TOTAL_IMPORTE_PEDIDO)";
                oComm.Parameters.Clear();
                oComm.Parameters.Add("@PRIMER_PEDIDO", SqlDbType.VarChar).Value = primerPedido;
                oComm.Parameters.Add("@SEGUNDO_PEDIDO", SqlDbType.VarChar).Value = segundoPedido;
                oComm.Parameters.Add("@TERCER_PEDIDO", SqlDbType.VarChar).Value = tercerPedido;
                oComm.Parameters.Add("@TOTAL_IMPORTE_PEDIDO", SqlDbType.VarChar).Value = totalGeneralPedido;
                oComm.Transaction = oTrans;
                oComm.ExecuteNonQuery();
                oComm.Transaction.Commit();
                bGuardo = true;      
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(oConn.State == ConnectionState.Open)
                {
                    oConn.Close();
                }
            }

            return bGuardo;
        }

        public List<PedidosTablaPropia> TraerPedidosDeReferencia(string nroPedido)
        {
            SqlDataAdapter daDetalle;
            List<CapaEntidades.DetallePedido> oDetalleLista = new List<CapaEntidades.DetallePedido>();
            DataSet dsDetalle = new DataSet();
            string sSQL;
            List<PedidosTablaPropia> ListaDePedidos = new List<PedidosTablaPropia>();
            try
            {
                sSQL = "SELECT PRIMER_PEDIDO,SEGUNDO_PEDIDO,TERCER_PEDIDO,TOTAL_IMPORTE_PEDIDO FROM NUMEROS_PEDIDOS_SISTEMA_VENTAS"+
                    " WHERE PRIMER_PEDIDO='"+nroPedido+"' OR SEGUNDO_PEDIDO='"+nroPedido+"' OR TERCER_PEDIDO='"+nroPedido+"'";

                daDetalle = new SqlDataAdapter(sSQL, Conexion);
                daDetalle.Fill(dsDetalle);
                for (int x = 0; x <= dsDetalle.Tables[0].Rows.Count - 1; x++)
                {
                    PedidosTablaPropia oDetalle = new PedidosTablaPropia();
                    oDetalle.PrimerPedido = dsDetalle.Tables[0].Rows[x]["PRIMER_PEDIDO"].ToString();
                    oDetalle.SegundoPedido = dsDetalle.Tables[0].Rows[x]["SEGUNDO_PEDIDO"].ToString();
                    oDetalle.TercerPedido = dsDetalle.Tables[0].Rows[x]["TERCER_PEDIDO"].ToString();
                    oDetalle.ImporteTotalPedido = dsDetalle.Tables[0].Rows[x]["TOTAL_IMPORTE_PEDIDO"].ToString();
                    ListaDePedidos.Add(oDetalle);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListaDePedidos;
        }

        public void EditarEstadoPedidoActual(string estado)
        {
            if (iLugar > -1 & iLugar <= iRegistros - 1)
            {
                ds.Tables[0].Rows[iLugar]["Estado"] = estado;
            }
        }
    }
}
