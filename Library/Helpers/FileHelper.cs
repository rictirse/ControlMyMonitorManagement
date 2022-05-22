using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Library.Helpers
{
    internal static class FileHelper
    {
        public static byte[] ResourceToByteArray(this string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                        assembly.GetManifestResourceNames().
                        Where(str => str.Contains(fileName)).FirstOrDefault();
            if (resourceName == null) return null;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
