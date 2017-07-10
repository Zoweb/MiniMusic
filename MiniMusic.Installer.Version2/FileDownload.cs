namespace MiniMusic.Installer.Version2
{
    public class FileDownload
    {
        public string URL;
        public string Path;

        public FileDownload() {}

        public FileDownload(string url, string path)
        {
            URL = url;
            Path = path;
        }
    }
}