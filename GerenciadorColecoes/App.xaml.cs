using AdventureWorks.Common;
using GerenciadorColecoes.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GerenciadorColecoes
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            using (var db = new GerenciadorContext())
            {
                db.Database.Migrate();

                if(db.Categorias.Count() == 0)
                {
                    db.Categorias.Add(new Categoria() {  Nome = "Tecnologia" });
                    db.Categorias.Add(new Categoria() { Nome = "Ficção Científica" });
                    db.Categorias.Add(new Categoria() { Nome = "Negócios" });
                    db.SaveChanges();
                }

                //db.Livros.RemoveRange(db.Livros);
                //db.SaveChanges();

                if(db.Livros.Count() == 0)
                {
                    for(int i = 0; i < 10; i++) {
                        Livro livro = new Livro();
                        livro.Nome = "Livro " + i;
                        livro.Descricao = "Teste";
                        livro.Favorito = true;
                        livro.UltimoAcesso = DateTime.Now;
                        livro.Categoria = db.Categorias.FirstOrDefault();
                        livro.CaminhoImagem = "ms-appx:///Assets/livro_1_cover.jpg";

                        db.Livros.Add(livro);
                        db.SaveChanges();
                    } 
                }
            }
        }

/*
#if WINDOWS_APP
            SearchPane searchPane = SearchPane.GetForCurrentView();
            searchPane.SuggestionsRequested += SearchPane_SuggestionsRequested;
#endif
#if WINDOWS_APP
        private void SearchPane_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            args.Request.SearchSuggestionCollection.AppendQuerySuggestion("livroteste");
        }
#endif
*/
        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }


        public static NavigationService NavigationService { get; private set; }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            //Configura para o aplicativo responder aos comandos de voltar da barra de tarefas e hardware
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                App.NavigationService = new NavigationService(rootFrame);
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            base.OnSearchActivated(args);
        }

        
    }
}
