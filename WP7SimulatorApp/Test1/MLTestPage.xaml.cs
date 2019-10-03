using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Framework.Media;
using Microsoft.Xna.Framework.Framework;

namespace WP7SimulatorApp.Test1
{
    public partial class MLTestPage : PhoneApplicationPage
    {
        public MLTestPage()
        {
            InitializeComponent();

            MediaLibrary ml = new MediaLibrary();
            listBox1.DataContext = ml.Songs;
        }

        private void listBox1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //FrameworkDispatcher.Update();
            //MediaLibrary ml = new MediaLibrary();
            ////随机播放一首歌
            //int songIndex = new Random().Next(0, ml.Songs.Count - 1);
            //var song = ml.Songs[songIndex];
            //MediaPlayer.Play(song);

            //wp7中没有listBox1.SelectedValue这个属性
            //Album album = ml.Albums[listBox1.SelectedIndex];
            
            //MediaPlayer.Play(album.Songs[0]);//todo：在新的一页中选择这个专辑中的歌曲
            
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            FrameworkDispatcher.Update();
            Song song = (Song)listBox1.SelectedItem;
            MediaPlayer.Play(song);
            //wp7中没有listBox1.SelectedValue这个属性
        }

        private void btnPlayCtrl_Click(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
        }
    }
}
