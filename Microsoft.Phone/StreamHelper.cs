using System;
using System.IO;
using System.IO.IsolatedStorage;

internal class StreamHelper
{
    private IsolatedStorageFile _appStorage;
    private bool _fTempFileCreated;
    private string _relativeFilePath;
    private Stream _sourceStream;

    public StreamHelper()
    {
        this._sourceStream = null;
        this._fTempFileCreated = false;
        this._relativeFilePath = null;
        this._appStorage = null;
    }

    public StreamHelper(Stream sourceStream)
    {
        this._sourceStream = sourceStream;
        this._fTempFileCreated = false;
        this._relativeFilePath = null;
        this._appStorage = null;
    }

    public void Cleanup()
    {
        if (this._fTempFileCreated)
        {
            this._appStorage.DeleteFile(this._relativeFilePath);
            this._fTempFileCreated = false;
        }
        if (this._appStorage != null)
        {
            this._appStorage.Dispose();
            Console.WriteLine("in StreamHelper:Cleanup after _appStorage dispose");
            this._appStorage = null;
        }
    }

    ~StreamHelper()
    {
        this.Cleanup();
    }

    public string GetTempFile()
    {
        this._appStorage = IsolatedStorageFile.GetUserStoreForApplication();
        string str = "tempFile" + Guid.NewGuid().ToString() + ".tmp";
        this._relativeFilePath = str;
        if (this._sourceStream != null)
        {
            IsolatedStorageFileStream stream = this._appStorage.CreateFile(this._relativeFilePath);
            this._fTempFileCreated = true;
            int count = 0x5000;
            byte[] buffer = new byte[count];
            while (true)
            {
                int num2 = this._sourceStream.Read(buffer, 0, count);
                if (num2 == 0)
                {
                    break;
                }
                stream.Write(buffer, 0, num2);
            }
            stream.Close();
        }
        return this._relativeFilePath;
    }

    public void WriteToStream(Stream targetStream)
    {
        int count = 0x5000;
        byte[] buffer = new byte[count];
        IsolatedStorageFileStream stream = new IsolatedStorageFileStream(this._relativeFilePath, FileMode.Open, FileAccess.Read, this._appStorage);
        this._fTempFileCreated = true;
        while (true)
        {
            int num2 = stream.Read(buffer, 0, count);
            if (num2 == 0)
            {
                break;
            }
            targetStream.Write(buffer, 0, num2);
        }
        stream.Close();
    }
}

