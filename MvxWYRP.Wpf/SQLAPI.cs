using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace MvxWYRP.Wpf
{
    public class SQLAPI
    {
        //SQL readers for Clients, Products and Orders databases

        public SqlDataReader ClientesDataReader()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Clientes", con);
            con.Open();
            SqlDataReader read = cmd.ExecuteReader();
            return read;
        }

        public SqlDataReader ProdutosDataReader()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Produtos", con);
            con.Open();
            SqlDataReader read = cmd.ExecuteReader();
            return read;
        }

        public SqlDataReader VendasDataReader()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Vendas", con);
            con.Open();
            SqlDataReader read = cmd.ExecuteReader();
            return read;
        }
    }
}
