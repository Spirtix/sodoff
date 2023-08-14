using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaReplenishable", Namespace = "")]
[Serializable]
public class ItemStateCriteriaReplenishable : ItemStateCriteria
{
	[XmlElement(ElementName = "ApplyRank")]
	public bool ApplyRank;

	[XmlElement(ElementName = "PointTypeID", IsNullable = true)]
	public AchievementPointTypes? PointTypeID;

	[XmlElement(ElementName = "ReplenishableRates")]
	public List<ReplenishableRate> ReplenishableRates;
}
