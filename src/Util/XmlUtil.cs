using System.Xml.Serialization;

namespace sodoff.Util;
public class XmlUtil {
    public static T DeserializeXml<T>(string xmlString) {
        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(xmlString))
            return (T)serializer.Deserialize(reader);
    }
}
