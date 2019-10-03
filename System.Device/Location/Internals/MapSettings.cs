using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;

namespace System.Device.Location.Internals
{
    public class MapSettings
    {
        private static readonly string InternalDir = "/Internal/";
        private static readonly string MapSettingsFileName = InternalDir + "MapSetting.txt";

        public string TileServerUriFormat
        {
            get
            {
                IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
                if (isolatedStorageFile.FileExists(MapSettingsFileName))
                {
                    using (Stream stream = new IsolatedStorageFileStream(MapSettingsFileName,
                        FileMode.Open, isolatedStorageFile))
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            return streamReader.ReadLine();
                        }
                    }
                }
                else
                {
                    //string tileUriFormat = "http://localhost:8080/{QuadKey}.png"; //使用自己搭建的TileServer
                    //string tileUriFormat = "http://r3.tiles.ditu.live.com/tiles/r{QuadKey}.png?g=47";

                    return "http://r3.tiles.ditu.live.com/tiles/r{QuadKey}.png?g=47";
                }
            }
            set
            {
                ToucheInternalDir();
                IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
                using (Stream stream = new IsolatedStorageFileStream(MapSettingsFileName,
                    FileMode.Create, isolatedStorageFile))
                {
                    using (StreamWriter streamWriter = new StreamWriter(stream))
                    {
                        streamWriter.WriteLine(value);
                    }
                }
            }
        }

        /// <summary>
        /// Internal文件夹如果不存在则创建
        /// </summary>
        private static void ToucheInternalDir()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isolatedStorageFile.DirectoryExists(InternalDir))
            {
                isolatedStorageFile.CreateDirectory(InternalDir);
            }
        }
    }
}
