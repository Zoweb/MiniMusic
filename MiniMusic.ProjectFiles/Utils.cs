using System.IO;
using System.Text;

namespace MiniMusic.ProjectFiles
{
    public static class Utils
    {
        public static Stream AsStream(this string content)
        {
            if (content == null) return null;

            var byteArray = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(byteArray);
        }
    }
}