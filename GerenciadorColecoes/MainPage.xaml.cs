using AdventureWorks.Common;
using GerenciadorColecoes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GerenciadorColecoes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        IGerenciador ger = new Gerenciador();
        private NavigationHelper navigationHelper;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            navigationHelper = new NavigationHelper(this);
        }




        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            this.Categorias.ItemsSource = ger.BuscarCategorias().ToList();
            this.CarregaFavoritos();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void CarregaFavoritos()
        {
            //Altera o titulo da página para Favoritos
            TituloPagina.Text = "Favoritos";

            //Busca todos os livros ordenando pela data de último acesso
            this.Livros.ItemsSource = ger.BuscarLivros()
                                        .Where(m => m.Favorito == true)
                                        .OrderByDescending(m => m.UltimoAcesso)
                                        .ToList();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //Navega para a página Pesquisar
            //this.Frame.Navigate(typeof(Pesquisar), null);
            App.NavigationService.Navigate<Pesquisar>(null);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Navega para a página EditarLivro 
            //this.Frame.Navigate(typeof(EditarLivro), null);
            App.NavigationService.Navigate<EditarLivro>(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Navega para a página EditarCategoria
            //this.Frame.Navigate(typeof(EditarCategoria), null);
            App.NavigationService.Navigate<EditarCategoria>(null);
        }

        private void Categorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Mostra ou oculta o botão de editar se alguma categoria for selecionada
            if(Categorias.SelectedItems.Count > 0)
            {
                btEditCategoria.Visibility = Visibility.Visible;
            }
            else
            {
                btEditCategoria.Visibility = Visibility.Collapsed;
            }

            //Pega a categoria selecionada no momento no menu Categorias 
            Categoria categoria = Categorias.SelectedItem as Categoria;

            
            if(categoria != null)
            {
                //Busca no banco de dados todos os livros que tenham uma categoria com o id
                //igual ao id da categoria selecionada
                this.Livros.ItemsSource = ger.BuscarLivros()
                                        .Where(m => m.Categoria.Id == categoria.Id)
                                        .OrderBy(m => m.Nome)
                                        .ToList();

                //Altera o título da página para mostrar o nome da categoria selecionada
                TituloPagina.Text = categoria.Nome;
            }
            
        }

        private void Livros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Pega o livro selecionado na lista de livros
            Livro livro = Livros.SelectedItem as Livro;

            if(livro != null)
            {
                //Navega para a página de Detalhe do livro passando o Id do livro selecionado
                this.Frame.Navigate(typeof(DetalheLivro), livro.Id);
            }
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Categorias.SelectedItem = null;
            //Carrega os favoritos
            this.CarregaFavoritos();
        }

        private void btEditCategoria_Click(object sender, RoutedEventArgs e)
        {
            Categoria categoriaSelecionada = Categorias.SelectedItem as Categoria;
            //this.Frame.Navigate(typeof(EditarCategoria), categoriaSelecionada.Id);
            App.NavigationService.Navigate<EditarCategoria>(categoriaSelecionada.Id);

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsPaneOpen = !Menu.IsPaneOpen;
        }
    }
}
