using AdventureWorks.Common;
using GerenciadorColecoes.Models;
using Microsoft.Data.Entity;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GerenciadorColecoes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditarCategoria : Page
    {
        Categoria categoria = null;
        IGerenciador ger = new Gerenciador();
        private NavigationHelper navigationHelper;

        public EditarCategoria()
        {
            this.InitializeComponent();
            this.Loaded += EditarCategoria_Loaded;
            navigationHelper = new NavigationHelper(this);
            categoria = new Categoria();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                Int64 categoriaId = (Int64) e.Parameter;
                categoria = ger.BuscarCategorias().Where(m => m.Id == categoriaId).FirstOrDefault();
                tbNome.Text = categoria.Nome;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void EditarCategoria_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, ev) =>
            {
                if (this.Frame.CanGoBack)
                {
                    //this.Frame.GoBack();
                    App.NavigationService.GoBack();
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

        private async void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            String nomeCategoria = tbNome.Text;

            if (String.IsNullOrEmpty(nomeCategoria))
            {
                await new MessageDialog("Informe o nome").ShowAsync();
                return;
            }

            categoria.Nome = nomeCategoria;

            if (categoria.Id == 0)
            {
                ger.AdicionarCategoria(categoria);
            }
            else
            {
                ger.AtualizarCategoria(categoria);
            }

            if (this.Frame.CanGoBack)
            {
                //this.Frame.GoBack();
                App.NavigationService.GoBack();
            }
            

        }

        private void btApagar_Click(object sender, RoutedEventArgs e)
        {
            Categoria categoriaDb = ger.BuscarCategorias().Where(m => m.Id == categoria.Id).Include(m => m.Livros).FirstOrDefault();
            if(categoriaDb.Livros != null && categoriaDb.Livros.Count > 0)
            {
                foreach (Livro livro in categoriaDb.Livros.ToList())
                {
                    Categoria categoriaNaoClassificados = ger.BuscarCategorias().Where(m => m.Nome == "Não classificados").FirstOrDefault();
                    if (categoriaNaoClassificados == null)
                    {
                        categoriaNaoClassificados = new Categoria();
                        categoriaNaoClassificados.Nome = "Não classificados";
                        ger.AdicionarCategoria(categoriaNaoClassificados);
                    }

                    categoriaNaoClassificados.Livros.Add(livro);
                    ger.AtualizarCategoria(categoriaNaoClassificados);
                }
            }
            

            if(categoriaDb.Livros.Count == 0)
            {
                ger.RemoverCategoria(categoriaDb.Id);
            }

            if (this.Frame.CanGoBack)
            {
                //this.Frame.GoBack();
                App.NavigationService.GoBack();
            }
        }
    }
}
