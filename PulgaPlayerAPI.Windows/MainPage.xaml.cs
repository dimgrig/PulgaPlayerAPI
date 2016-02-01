using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
//using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VK.WindowsPhone.SDK;
using VK.WindowsPhone.SDK.API;
using VK.WindowsPhone.SDK.API.Model;
using Windows.UI.Xaml.Media.Imaging;
// The WebView Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace PulgaPlayerAPI
{
    public sealed partial class MainPage : Page
    {
        // TODO: Replace with your URL here.
        private static readonly Uri HomeUri = new Uri("ms-appx-web:///Html/index.html", UriKind.Absolute);
        private List<string> _scope = new List<string> { VKScope.AUDIO };
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            VKSDK.Initialize("5226529");
            VKSDK.Authorize(_scope, false, false);
        }

        public string GetTrueURL(string inputstring)
        {
            return inputstring.Substring(0, inputstring.IndexOf('?'));
        }

        private void textRequest_TextChanged(object sender, TextChangedEventArgs e)
        {
            VKRequest.Dispatch<VKList<VKAudio>>(new VKRequestParameters(
                "audio.search", "q", textRequest.Text),
                (result) =>
                {
                    audioView.ItemsSource = result.Data.items;                    
                }
                );
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            /*string tempURL = (sender as TextBlock).Tag.ToString(); // из ListBox любую выбранную строчку преобразуем в string
            //ругается на верхнюю строчку хз почему
            Player.Source = new Uri(GetTrueURL(tempURL)); // выделяем нужную часть ссылки
            Player.Play(); // проигрываем*/
            int n = audioView.SelectedIndex;
            PlayTrack((audioView.Items[n] as VKAudio).url);
        }

        private void PlayTrack(string tempURL)
        {
            Player.Source = new Uri(GetTrueURL(tempURL)); // выделяем нужную часть ссылки
            Player.Play(); // проигрываем
        }

        private void imagePrev_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int n = --audioView.SelectedIndex;
            PlayTrack((audioView.Items[n] as VKAudio).url);
        }

        private void imageNext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int n = ++audioView.SelectedIndex;
            PlayTrack((audioView.Items[n] as VKAudio).url);
        }

        private void imagePause_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Player.CurrentState == MediaElementState.Playing)
            {
                Player.Pause();
                Image_Loaded(@"Resources/Play.jpg");
            }
            else if (Player.CurrentState == MediaElementState.Paused)
                {
                    Player.Play();
                    Image_Loaded(@"Resources/Pause.jpg");
                }
        }

        void Image_Loaded(string Icon)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.UriSource = new Uri(imagePause.BaseUri, Icon);
            imagePause.Source = bmp;
        }

        private void ProgressPos_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ;
        }

        /*        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
                {
                    VKRequest.Dispatch<VKList<VKAudio>>(new VKRequestParameters(
                        "audio.search", "q", "faded zhu"), 
                        (result)=>
                        {
                            foreach (var item in result.Data.items)
                            {
                                audioView.Items.Add(item.title);
                            }
                        }
                        );
                }


                private void Button_Click(object sender, RoutedEventArgs e)
                {

                }
        */
    }
}
