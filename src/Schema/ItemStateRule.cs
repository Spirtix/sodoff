using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateRule", Namespace = "")]
[Serializable]
public class ItemStateRule
{
	[XmlElement(ElementName = "Criterias")]
	public List<ItemStateCriteria> Criterias;

	[XmlElement(ElementName = "CompletionAction")]
	public CompletionAction CompletionAction;
}
