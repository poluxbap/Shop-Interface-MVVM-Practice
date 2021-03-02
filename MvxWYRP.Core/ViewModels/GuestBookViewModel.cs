using Dapper;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxWYRP.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MvxWYRP.Core.ViewModels
{ 
    public class GuestBookViewModel : MvxViewModel
    {   
        private ObservableCollection<PersonModel> _people = new ObservableCollection<PersonModel>();
        private ObservableCollection<LojaModel> _loja = new ObservableCollection<LojaModel>();
        private ObservableCollection<OrderModel> _order = new ObservableCollection<OrderModel>();
        private string _orderNome;
        private string _orderProduto;
        private string _orderQuantidade;
        private string _name;
        private string _id;
        private string _produto;
        private string _info;
        private string _valor;

        // Upon start, update the content of all three ObservableCollections
        public GuestBookViewModel() 
        {
            DataTable ClienteDataTable = new DataTable();
            ClienteDataTable.Load(ClientesDataReader());
            for (int i = 0; i < ClienteDataTable.Rows.Count; i++)
            {
                PersonModel p = new PersonModel
                {
                    Nome = ClienteDataTable.Rows[i][1].ToString(),
                    ID = ClienteDataTable.Rows[i][0].ToString()
                };
                People.Add(p);
            }

            DataTable ProdutoDataTable = new DataTable();
            ProdutoDataTable.Load(ProdutosDataReader());
            for (int i = 0; i < ProdutoDataTable.Rows.Count; i++)
            {
                LojaModel p = new LojaModel
                {
                    Produto = ProdutoDataTable.Rows[i][0].ToString(),
                    Info = ProdutoDataTable.Rows[i][1].ToString(),
                    Valor = ProdutoDataTable.Rows[i][2].ToString()
                };
                Loja.Add(p);
            }

            DataTable VendasDataTable = new DataTable();
            VendasDataTable.Load(VendasDataReader());
            for (int i = 0; i < VendasDataTable.Rows.Count; i++)
            {
                OrderModel p = new OrderModel
                {
                    OrderNome = VendasDataTable.Rows[i][0].ToString(),
                    OrderProduto = VendasDataTable.Rows[i][1].ToString(),
                    OrderQuantidade = VendasDataTable.Rows[i][2].ToString(),
                    OrderTotal = VendasDataTable.Rows[i][0].ToString()
                };
                Order.Add(p);
            }
        }

        //SQL writer for Clients, Products and Orders databases

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

        // Function for Add an added line to one of the tables

        public static void SavePerson(PersonModel person)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Clientes (ID,Nome) VALUES (@ID,@Nome)", con);
            cmd.Parameters.AddWithValue("@ID", person.ID);
            cmd.Parameters.AddWithValue("@Nome", person.Nome);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void SaveProduto(LojaModel product)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Produtos (Produto,Info,Valor) VALUES (@Produto,@Info,@Valor)", con);
            cmd.Parameters.AddWithValue("@Produto", product.Produto);
            cmd.Parameters.AddWithValue("@Info", product.Info);
            cmd.Parameters.AddWithValue("@Valor", product.Valor);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void SaveOrder(OrderModel order)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Vendas (OrderNome,OrderProduto,OrderQuantidade,OrderTotal) VALUES (@OrderNome,@OrderProduto,@OrderQuantidade,@OrderTotal)", con);
            cmd.Parameters.AddWithValue("@OrderNome", order.OrderNome);
            cmd.Parameters.AddWithValue("@OrderProduto", order.OrderProduto);
            cmd.Parameters.AddWithValue("@OrderQuantidade", order.OrderQuantidade);
            cmd.Parameters.AddWithValue("@OrderTotal", order.OrderTotal);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Function for Delete an added line to one of the tables

        public void DeleteGuest(int index)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Clientes WHERE CONVERT (VARCHAR, ID) = @ID", con);
            cmd.Parameters.AddWithValue("@ID", People[index].ID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            People.RemoveAt(index);
        }

        public void DeleteProduto(int index)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Produtos WHERE CONVERT (VARCHAR, Produto) = @Produto", con);
            cmd.Parameters.AddWithValue("@Produto", Loja[index].Produto);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Loja.RemoveAt(index);
        }

        public void DeleteOrder(int index)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Usuario\source\repos\MvxWYRP\MvxWYRP.Core\ViewModels\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Vendas WHERE CONVERT (VARCHAR, OrderNome) = @OrderNome AND CONVERT (VARCHAR, OrderProduto) = @OrderProduto AND CONVERT (VARCHAR, OrderQuantidade) = @OrderQuantidade", con);
            cmd.Parameters.AddWithValue("@OrderNome", Order[index].OrderNome);
            cmd.Parameters.AddWithValue("@OrderProduto", Order[index].OrderProduto);
            cmd.Parameters.AddWithValue("@OrderQuantidade", Order[index].OrderQuantidade);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Order.RemoveAt(index);
        }

        //----------------------FIRST PAGE----------------------//

        // Add to both the Database and the ObservableCollection

        public void AddGuest()
        {
            PersonModel p = new PersonModel
            {
                Nome = Nome,
                ID = ID
            };

            Nome = string.Empty;
            ID = string.Empty;

            SavePerson(p);
            People.Add(p);
        }

        // Desactivate button if conditions match

        public bool CanAddGuest => Nome?.Length > 0 && ID?.Length > 0;

        public ObservableCollection<PersonModel> People
        {
            get { return _people; }
            set { SetProperty(ref _people, value); }
        }

        public string Nome
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                RaisePropertyChanged(() => CanAddGuest);
            }
        }

        public string ID
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
                RaisePropertyChanged(() => CanAddGuest);
            }
        }

        //----------------------SECOND PAGE----------------------//

        // Add to both the Database and the ObservableCollection

        public void AddProduto()
        {
            LojaModel p = new LojaModel
            {
                Produto = Produto,
                Info = Info,
                Valor = Valor
            };

            Produto = string.Empty;
            Info = string.Empty;
            Valor = string.Empty;

            SaveProduto(p);
            Loja.Add(p);
        }

        // Desactivate button if conditions match

        public bool CanAddProduto => Produto?.Length > 0 && Info?.Length > 0 && Valor?.Length > 00;

        public ObservableCollection<LojaModel> Loja
        {
            get { return _loja; }
            set { SetProperty(ref _loja, value); }
        }

        public string Produto
        {
            get { return _produto; }
            set
            {
                SetProperty(ref _produto, value);
                RaisePropertyChanged(() => CanAddProduto);
            }
        }

        public string Info
        {
            get { return _info; }
            set
            {
                SetProperty(ref _info, value);
                RaisePropertyChanged(() => CanAddProduto);
            }
        }

        public string Valor
        {
            get { return _valor; }
            set
            {
                SetProperty(ref _valor, value);
                RaisePropertyChanged(() => CanAddProduto);
            }
        }

        //----------------------THIRD PAGE----------------------//

        // Takes the amount ordered and multiply by the price of the product

        public float PriceCalc()
        {
            float Value = 0.0f;
            for (int i = 0; i < Loja.Count; i++)
            {
                if (Loja[i].Produto == OrderProduto)
                {
                    Value = float.Parse(Loja[i].Valor);
                }
            }
            return Value * float.Parse(OrderQuantidade.ToString());
        }

        // Add to both the Database and the ObservableCollection

        public void AddOrder()
        {
            OrderModel p = new OrderModel
            {
                OrderNome = OrderNome,
                OrderProduto = OrderProduto,
                OrderQuantidade = OrderQuantidade,
                OrderTotal = PriceCalc().ToString()
            };

            OrderNome = string.Empty;
            OrderProduto = string.Empty;
            OrderQuantidade = string.Empty;

            SaveOrder(p);
            Order.Add(p);
        }

        // Desactivate button if conditions match

        public bool CanAddOrder => OrderNome?.Length > 0 && OrderProduto?.Length > 0 && OrderQuantidade?.Length > 0;

        public ObservableCollection<OrderModel> Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        public int OrderTotal;

        public string OrderNome
        {
            get { return _orderNome; }
            set
            {
                SetProperty(ref _orderNome, value);
                RaisePropertyChanged(() => CanAddOrder);
            }
        }

        public string OrderProduto
        {
            get { return _orderProduto; }
            set
            {
                SetProperty(ref _orderProduto, value);
                RaisePropertyChanged(() => CanAddOrder);
            }
        }       

        public string OrderQuantidade
        {
            get { return _orderQuantidade; }
            set
            {
                SetProperty(ref _orderQuantidade, value);
                RaisePropertyChanged(() => CanAddOrder);
            }
        }
    }
}
