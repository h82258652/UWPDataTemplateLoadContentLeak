using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPDataTemplateLoadContentLeak
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly List<WeakReference<object>> _references = new List<WeakReference<object>>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void FrameworkTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateReferences("FrameworkControlTemplate");
        }
        
        private void UserControlTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateReferences("UserControlTemplate");
        }

        private void FrameworkControlHostUserControlTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateReferences("FrameworkControlHostUserControlTemplate");
        }

        private void GCCollectButton_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private async void ShowAliveCountButton_Click(object sender, RoutedEventArgs e)
        {
            var aliveCount = _references.Count(reference => reference.TryGetTarget(out _));
            await new MessageDialog($"Alive reference count: {aliveCount}").ShowAsync();
        }

        private void PopulateReferences(string templateKey)
        {
            _references.Clear();
            var dataTemplate = (DataTemplate)Application.Current.Resources[templateKey];
            for (var i = 0; i < 100; i++)
            {
                var content = dataTemplate.LoadContent();
                _references.Add(new WeakReference<object>(content));
            }
        }
    }
}
