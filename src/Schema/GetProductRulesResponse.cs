using System.Xml.Serialization;

namespace sodoff.Schema;


[XmlRoot(ElementName = "getProductRulesResponse", Namespace = "")]
[Serializable]
public class GetProductRulesResponse {
    [XmlElement(ElementName = "globalKey")]
    public string GlobalSecretKey { get; set; }
}