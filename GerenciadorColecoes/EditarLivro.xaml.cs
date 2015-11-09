using AdventureWorks.Common;
using GerenciadorColecoes.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class EditarLivro : Page
    {
        private NavigationHelper navigationHelper;
        IGerenciador ger = new Gerenciador();
        Livro livro = null;

        public EditarLivro()
        {
            this.InitializeComponent();
            this.Loaded += EditarLivro_Loaded;
            navigationHelper = new NavigationHelper(this);
            livro = new Livro();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            cbCategorias.ItemsSource = ger.BuscarCategorias().ToList();

            if (e.Parameter != null)
            {
                Int64 livroId = (Int64)e.Parameter;
                livro = ger.BuscarLivros().Where(m => m.Id == livroId)
                                 .Include(m => m.Categoria)
                                 .FirstOrDefault();
                tbNome.Text = livro.Nome;
                tbDescricao.Text = livro.Descricao;
                ImagemCapa.Source = new BitmapImage(new Uri(livro.CaminhoImagem, UriKind.Absolute));
                foreach (Categoria c in cbCategorias.Items)
                {
                    if(c.Id == livro.Categoria.Id)
                    {
                        cbCategorias.SelectedItem = c;
                    }
                }
            }
        }

        private void EditarLivro_Loaded(object sender, RoutedEventArgs e)
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
            String nomeLivro = tbNome.Text;
            Categoria categoria = cbCategorias.SelectedItem as Categoria;
            String descricaoLivro = tbDescricao.Text;
            String caminhoImagem = (ImagemCapa.Source as BitmapImage).UriSource.AbsoluteUri;
            if(String.IsNullOrEmpty(nomeLivro))
            {
                await new MessageDialog("Informe o nome").ShowAsync();
                return;
            }

            if (categoria == null)
            {
                await new MessageDialog("Informe a categoria").ShowAsync();
                return;
            }

            livro.Nome = nomeLivro;
            livro.CaminhoImagem = caminhoImagem;
            livro.Descricao = descricaoLivro;
            livro.Categoria = categoria;
            
            if(livro.Id == 0)
            {
                ger.AdicionarLivro(livro);
            }
            else
            {
                ger.AtualizarLivro(livro);
            }
            
            if (this.Frame.CanGoBack)
            {
                //this.Frame.GoBack();
                App.NavigationService.GoBack();
            }

        }

        private async Task<StorageFile> DownloadImage(String url)
        {
            var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(new Uri(url));
            String newName = Guid.NewGuid().ToString();

            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var coversFolder = await local.CreateFolderAsync("Covers", CreationCollisionOption.OpenIfExists);
            var file = await coversFolder.CreateFileAsync(newName + ".jpg");
            var targetStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            await targetStream.AsStreamForWrite().WriteAsync(data, 0, data.Length);
            await targetStream.FlushAsync();
            return file;
        }

        private async void tbImagemUrl_Click(object sender, RoutedEventArgs e)
        {
            String urlDownload = "";
            var dialog = new ContentDialog()
            {
                Title = "Informe a URL",
                //RequestedTheme = ElementTheme.Dark,
                //FullSizeDesired = true,
                MaxWidth = this.ActualWidth // Required for Mobile!
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "Informe a URL da imagem.",
                TextWrapping = TextWrapping.Wrap,
            });

            var tbUrl = new TextBox()
            {
                PlaceholderText = "URL"
            };

            panel.Children.Add(tbUrl);

            dialog.Content = panel;

            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += delegate {
                urlDownload = tbUrl.Text;
            };

            dialog.SecondaryButtonText = "Cancel";
            dialog.SecondaryButtonClick += delegate {

            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (!String.IsNullOrEmpty(urlDownload))
                {
                    StorageFile file = await this.DownloadImage(urlDownload);
                    ImagemCapa.Source = new BitmapImage(new Uri(file.Path, UriKind.Absolute));
                }
            }
        }

        private async void tbImagemArquivo_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            String newName = Guid.NewGuid().ToString();

            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var coversFolder = await local.CreateFolderAsync("Covers", CreationCollisionOption.OpenIfExists);
            var newFile = await coversFolder.CreateFileAsync(newName + file.FileType);

            await file.CopyAndReplaceAsync(newFile);

            if (file != null)
            {
                ImagemCapa.Source = new BitmapImage(new Uri(newFile.Path, UriKind.Absolute));
            }
        }

        private async void tbImagemCamera_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI camera = new CameraCaptureUI();
            CameraCaptureUI dialog = new CameraCaptureUI();
            Size aspectRatio = new Size(9, 16);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                ImagemCapa.Source = new BitmapImage(new Uri(file.Path, UriKind.Absolute));
            }
        }
    }
}
