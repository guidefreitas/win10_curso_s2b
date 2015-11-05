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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GerenciadorColecoes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetalheLivro : Page
    {

        Livro livro = new Livro();
        GerenciadorContext db = new GerenciadorContext();

        public DetalheLivro()
        {
            this.InitializeComponent();
            this.Loaded += DetalheLivro_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Int64 livroId = (Int64)e.Parameter;
                livro = db.Livros
                          .Where(m => m.Id == livroId)
                          .Include(a =>a.Categoria)
                          .FirstOrDefault();
                if(livro != null)
                {
                    String caminhoImagem = "ms-appx:///Assets/blank_cover.jpg";
                    if (!String.IsNullOrEmpty(livro.CaminhoImagem))
                    {
                        caminhoImagem = livro.CaminhoImagem;
                    }

                    imagemCapa.Source = new BitmapImage(new Uri(caminhoImagem, UriKind.Absolute));
                    tbNome.Text = livro.Nome;
                    if(livro.Categoria != null)
                    {
                        tbNomeCategoria.Text = livro.Categoria.Nome;
                    }
                    
                    tbDescricao.Text = livro.Descricao;

                    livro.UltimoAcesso = DateTime.Now;
                    btFavorito.IsChecked = livro.Favorito;
                    db.SaveChanges();
                }

            }
        }

        private void DetalheLivro_Loaded(object sender, RoutedEventArgs e)
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

        private void btEditar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditarLivro), livro.Id);
        }

        private async void btRemover_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Tem certeza que quer remover?");
            UICommand okBtn = new UICommand("OK");
            okBtn.Invoked = OkBtnClick;
            dialog.Commands.Add(okBtn);

            //Cancel Button
            UICommand cancelBtn = new UICommand("Cancel");
            cancelBtn.Invoked = CancelBtnClick;
            dialog.Commands.Add(cancelBtn);

            //Show message
            await dialog.ShowAsync();
        }

        private void CancelBtnClick(IUICommand command)
        {
            
        }

        private void OkBtnClick(IUICommand command)
        {
            Livro livroDb = db.Livros.Where(m => m.Id == livro.Id).FirstOrDefault();
            if (livroDb != null)
            {
                db.Livros.Remove(livroDb);
                db.SaveChanges();
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
        }

        private async void btFavorito_Checked(object sender, RoutedEventArgs e)
        {
            Livro livroDb = db.Livros.Where(m => m.Id == livro.Id).FirstOrDefault();
            livro.Favorito = true;
            await db.SaveChangesAsync();
        }

        private async void btFavorito_Unchecked(object sender, RoutedEventArgs e)
        {
            Livro livroDb = db.Livros.Where(m => m.Id == livro.Id).FirstOrDefault();
            livro.Favorito = false;
            await db.SaveChangesAsync();
        }
    }
}
