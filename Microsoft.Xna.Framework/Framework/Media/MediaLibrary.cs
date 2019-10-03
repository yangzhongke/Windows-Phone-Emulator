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
using System.IO;

namespace Microsoft.Xna.Framework.Framework.Media
{
    public class MediaLibrary
    {
        private readonly static string MyPicturesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private readonly static string SavedDir = System.IO.Path.Combine(MyPicturesDir, "saved pictures");

        public AlbumCollection Albums 
        {
            get
            {
                AlbumCollection albums = new AlbumCollection();
                string myMusicDir = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                foreach (string dirPath in Directory.EnumerateDirectories(myMusicDir))
                {
                    Album album = new Album();
                    album.dirPath = dirPath;
                    albums.Add(album);
                }
                return albums;
            }
        }


        public PictureAlbum RootPictureAlbum
        {
            get
            {
                return new PictureAlbum(MyPicturesDir, null);
            }
        }

        public SongCollection Songs
        {
            get
            {
                SongCollection songs = new SongCollection();
                foreach (var album in Albums)
                {
                    foreach (var song in album.Songs)
                    {
                        songs.Add(song);
                    }
                }
                return songs;
            }
        }

        public PictureCollection Pictures
        {
            get
            {
                PictureCollection pictures = new PictureCollection();
                LoadPictures(RootPictureAlbum, pictures);
                return pictures;
            }
        }

        private void LoadPictures(PictureAlbum album, PictureCollection pictures)
        {
            foreach (Picture pic in album.Pictures)
            {
                pictures.Add(pic);
            }
            foreach (PictureAlbum childAlbum in album.Albums)
            {
                LoadPictures(childAlbum, pictures);
            }
        }
 
        public Picture SavePicture(string name, byte[] imageBuffer)
        {
            string filepath = System.IO.Path.Combine(SavedDir,name);
            File.WriteAllBytes(filepath, imageBuffer);
            return new Picture(filepath, null);//todo:需要验证SavePicture的PictureAlbum是什么
        }
        public Picture SavePicture(string name, Stream source)
        {
            string filepath = System.IO.Path.Combine(SavedDir, name);
            if (!Directory.Exists(SavedDir))
            {
                Directory.CreateDirectory(SavedDir);
            }
            using (Stream stream = File.OpenWrite(filepath))
            {
                source.CopyTo(stream);
            }            
            return new Picture(filepath, null);//todo:需要验证SavePicture的PictureAlbum是什么
        }

        public PictureCollection SavedPictures 
        {
            get
            {
                return new PictureAlbum(SavedDir, null).Pictures;
            }
        }
    }
}
