using System.Reflection;
using System.Xml.Serialization;

namespace sodoff.Util;
public class XmlUtil {
    public static T DeserializeXml<T>(string xmlString) {
        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(xmlString))
            return (T)serializer.Deserialize(reader);
    }

    public static string SerializeXml<T>(T xmlObject) {
        var serializer = new XmlSerializer(typeof(T));
        using (var writer = new StringWriter()) {
            serializer.Serialize(writer, xmlObject);
            return writer.ToString();
        }
    }

    public static string ReadResourceXmlString(string name) {
        string result = "";
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"{name}.xml"));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
            result = reader.ReadToEnd();
        return result;
    }
}
