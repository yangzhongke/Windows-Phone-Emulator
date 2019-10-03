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
using System.IO.IsolatedStorage;
using System.IO;

namespace WP7SimulatorApp.Demo
{
    public partial class IsolatedStoragePage1 : PhoneApplicationPage
    {
        public IsolatedStoragePage1()
        {
            InitializeComponent();
        }

        private void btnWriteFile_Click(object sender, RoutedEventArgs e)
        {
            //为程序获取一个虚拟的本地存储
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //创建一个新的文件夹
            fileStorage.CreateDirectory("textFiles");

            //创建一个txt文件的流
            StreamWriter fileWriter = new StreamWriter(new IsolatedStorageFileStream("textFiles\\newText.txt", FileMode.OpenOrCreate, fileStorage));
            //向文件中写出内容
            fileWriter.WriteLine("hello");
            //关闭StreamWriter.
            fileWriter.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //为程序获取一个虚拟的本地存储
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            //创建一个新的StreamReader
            StreamReader fileReader = null;
            try
            {
                //读取文件
                fileReader = new StreamReader(new IsolatedStorageFileStream("textFiles\\newText.txt", FileMode.Open, fileStorage));
                //读取内容
                string textFile = fileReader.ReadToEnd();
                MessageBox.Show(textFile);
                fileReader.Close();
            }
            catch
            {
                MessageBox.Show("Need to create directory and the file first.");
            }


        }

        private IsolatedStorageSettings settings = 
            IsolatedStorageSettings.ApplicationSettings;


        private void btnReadSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settings.Contains("emailFlag"))
            {
                EmailFlag.IsChecked = (bool)settings["emailFlag"];
            }
            else settings.Add("emailFlag", false);
        }

        private void EmailFlag_Checked(object sender, RoutedEventArgs e)
        {
            settings["emailFlag"] = true;
        }

        private void EmailFlag_Unchecked(object sender, RoutedEventArgs e)
        {
            settings["emailFlag"] = false;
        }
    }
}
