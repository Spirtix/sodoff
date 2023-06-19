using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class CompletionAction
{
	[XmlElement(ElementName = "Transition")]
	public StateTransition Transition;
}
