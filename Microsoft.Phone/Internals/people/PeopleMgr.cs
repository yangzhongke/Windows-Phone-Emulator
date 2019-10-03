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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.IO;

namespace Microsoft.Phone.Internals.people
{
    public class PeopleMgr
    {
        private static readonly string InternalDir = "/Internal/";
        private static readonly string PeopleMgrFileName = InternalDir+"Peoples.xml";

        public static List<PeopleItemInfo> LoadPeoples()
        {            
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            //isolatedStorageFile.DeleteFile(PeopleMgrFileName);
            if (isolatedStorageFile.FileExists(PeopleMgrFileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PeopleItemInfo>));
                using (Stream stream = new IsolatedStorageFileStream(PeopleMgrFileName,
                    FileMode.Open, isolatedStorageFile))
                {
                    List<PeopleItemInfo> list = (List<PeopleItemInfo>)xmlSerializer.Deserialize(stream);
                    return list;
                }
            }
            else
            {
                return new List<PeopleItemInfo>();
            }
        }

        public static void SavePeoples(List<PeopleItemInfo> peopleList)
        {
            ToucheInternalDir();
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PeopleItemInfo>));
            using (Stream stream = new IsolatedStorageFileStream(PeopleMgrFileName, 
                FileMode.OpenOrCreate, isolatedStorageFile))
            {
                xmlSerializer.Serialize(stream, peopleList);
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
