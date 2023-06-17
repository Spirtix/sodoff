using System.Xml.Serialization;

namespace sodoff.Schema;

public class SubscriptionNotification
{
	[XmlElement(ElementName = "Type")]
	public SubscriptionNotificationType Type;
}
