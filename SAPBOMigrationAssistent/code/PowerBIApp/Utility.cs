using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace PowerBIApp
{
    public static class Utility
    {

        public static void CreatePbit(string layout, string fileName = "D:\\Sonata\\PowerBI\\Samples\\Report.pbit")
        {
            var archive = ZipFile.Open(fileName, ZipArchiveMode.Update);
            foreach (var item in archive.Entries)
            {
                if (item.Name == "Layout")
                {
                    using var stream = item.Open();
                    stream.Position = 0;
                    byte[] buffer = Encoding.UTF8.GetBytes(layout);
                    stream.Write(buffer, 0, buffer.Count());
                }
            }
            
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            return (T)serializer.Deserialize(reader)!;
        }


    }
}
