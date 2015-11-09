using GerenciadorColecoes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GerenciadorColecoes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Pesquisar : Page
    {

        GerenciadorContext db = new GerenciadorContext();


        public Pesquisar()
        {
            this.InitializeComponent();
            this.Loaded += Pesquisar_Loaded;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Livros.ItemsSource = db.Livros.OrderBy(m => m.UltimoAcesso).Take(10).ToList();
        }

        private void Pesquisar_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, ev) =>
            {
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            };

            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String busca = tbPesquisa.Text;
            Livros.ItemsSource = db.Livros
                                   .Where(m => m.Nome.Contains(busca))
                                   .OrderBy(m => m.Nome)
                                   .ToList();
        }

        private void Livros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Livro livro = Livros.SelectedItem as Livro;
            if(livro != null)
            {
                this.Frame.Navigate(typeof(DetalheLivro), livro.Id);
            }
            
        }
    }
}
