using System.IO;

namespace Taxi.Prism.Helpers
{
    public class FilesHelper : IFilesHelper
    {
        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}