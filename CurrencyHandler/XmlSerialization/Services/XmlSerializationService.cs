using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlSerialization.Services
{
    public static class XmlSerializationService
    {
        public static Task Serialize<TObject>(TObject instance, Stream stream) => Task.Run(() =>
        {
            var serializer = new XmlSerializer(typeof(TObject));
            using (stream) serializer.Serialize(stream, instance);
        });

        public static Task<TObject> Deserialize<TObject>(Stream stream) => Task.Run(() =>
        {
            var serializer = new XmlSerializer(typeof(TObject));
            using (stream) return (TObject)serializer.Deserialize(stream);
        });
    }
}
