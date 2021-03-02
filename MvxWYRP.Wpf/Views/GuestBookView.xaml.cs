using MvvmCross.Platforms.Wpf.Views;
using MvxWYRP.Core.Models;
using MvxWYRP.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvxWYRP.Wpf.Views
{
    /// <summary>
    /// Interaction logic for GuestBookView.xaml
    /// </summary>
    public partial class GuestBookView : MvxWpfView
    {

        public GuestBookViewModel ViewModel { get { return this.DataContext as GuestBookViewModel; } set { this.DataContext = value; } }
        public SQLAPI API = new SQLAPI();

        //Initialize all Tables

        public GuestBookView()
        {
            InitializeComponent();
            this.ViewModel = new GuestBookViewModel();
            SQLTable1();
            SQLTable2();
            SQLTable3();
        }

        //Set a Click component for the three Menu buttons

        private void LoadClientes(object sender, RoutedEventArgs e)
        {
            this.Clientes.Visibility = Visibility.Visible;
            this.Produtos.Visibility = Visibility.Collapsed;
            this.Pedido.Visibility = Visibility.Collapsed;
        }

        private void LoadProdutos(object sender, RoutedEventArgs e)
        {
            this.Produtos.Visibility = Visibility.Visible;
            this.Pedido.Visibility = Visibility.Collapsed;
            this.Clientes.Visibility = Visibility.Collapsed;
        }

        private void LoadVendas(object sender, RoutedEventArgs e)
        {
            this.Pedido.Visibility = Visibility.Visible;
            this.Produtos.Visibility = Visibility.Collapsed;
            this.Clientes.Visibility = Visibility.Collapsed;
            LoadClienteBox();
            LoadProdutoBox();
        }

        //Set the ComboBox for the third screen

        private void LoadClienteBox()
        {
            this.ClienteBox.Items.Clear();
            foreach (PersonModel name in this.ViewModel.People)
            {
                this.ClienteBox.Items.Add(name.Nome);
            }            
        }

        private void LoadProdutoBox()
        {
            this.ProdutoBox.Items.Clear();
            foreach (LojaModel product in this.ViewModel.Loja)
            {
                this.ProdutoBox.Items.Add(product.Produto);
            }
        }

        //Uses Command Line Arguments for restricting the typed inputs

        private void QuantidadeValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void IDValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AlphaValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Visually refresh the ObservableCollections uppon Delete/Add

        private void SQLTable1()
        {
            ClientesGrid.ItemsSource = API.ClientesDataReader();
        }

        private void SQLTable2()
        {
            ProdutosGrid.ItemsSource = API.ProdutosDataReader();
        }

        private void SQLTable3()
        {
            VendasGrid.ItemsSource = API.VendasDataReader();
        }

        private void addClient(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AddGuest();
            SQLTable1();
        }

        private void addProduct(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AddProduto();
            SQLTable2();
        }

        private void addOrder(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AddOrder();
            SQLTable3();
        }

        private void deleteClient(object sender, RoutedEventArgs e)
        {
            if (ClientesGrid.SelectedIndex != -1)
            {
                this.ViewModel.DeleteGuest(ClientesGrid.SelectedIndex);
                SQLTable1();
            }           
        }

        private void deleteProduct(object sender, RoutedEventArgs e)
        {
            if (ProdutosGrid.SelectedIndex != -1)
            {
                this.ViewModel.DeleteProduto(ProdutosGrid.SelectedIndex);
                SQLTable2();
            }
        }

        private void deleteOrder(object sender, RoutedEventArgs e)
        {
            if (VendasGrid.SelectedIndex != -1)
            {
                this.ViewModel.DeleteOrder(VendasGrid.SelectedIndex);
                SQLTable3();
            }
        }
    }
}
