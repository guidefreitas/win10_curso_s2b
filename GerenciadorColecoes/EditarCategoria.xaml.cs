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
        GerenciadorContext db = new GerenciadorContext();
        public EditarCategoria()
        {
            this.InitializeComponent();
            this.Loaded += EditarCategoria_Loaded;
            categoria = new Categoria();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                Int64 categoriaId = (Int64) e.Parameter;
                categoria = db.Categorias.Where(m => m.Id == categoriaId).FirstOrDefault();
                tbNome.Text = categoria.Nome;
            }
        }

        private void EditarCategoria_Loaded(object sender, RoutedEventArgs e)
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
                db.Categorias.Add(categoria);
                db.SaveChanges();
            }
            else
            {
                db.Categorias.Update(categoria);
                db.SaveChanges();
            }

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            

        }

        private void btApagar_Click(object sender, RoutedEventArgs e)
        {
            Categoria categoriaDb = db.Categorias.Where(m => m.Id == categoria.Id).Include(m => m.Livros).FirstOrDefault();
            foreach(Livro livro in categoriaDb.Livros)
            {
                Categoria categoriaNaoClassificados = db.Categorias.Where(m => m.Nome == "Não classificados").FirstOrDefault();
                if(categoriaNaoClassificados == null)
                {
                    categoriaNaoClassificados = new Categoria();
                    categoriaNaoClassificados.Nome = "Não classificados";
                    db.Categorias.Add(categoriaNaoClassificados);
                    db.SaveChanges();
                }

                livro.Categoria = categoriaNaoClassificados;
                db.SaveChanges();
            }

            if(categoriaDb.Livros.Count == 0)
            {
                db.Categorias.Remove(categoriaDb);
                db.SaveChanges();
            }
        }
    }
}
