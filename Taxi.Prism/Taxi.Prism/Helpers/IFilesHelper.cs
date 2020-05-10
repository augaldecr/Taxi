using System.IO;

namespace Taxi.Prism.Helpers
{
    public interface IFilesHelper
    {
        byte[] ReadFully(Stream input);
    }
}