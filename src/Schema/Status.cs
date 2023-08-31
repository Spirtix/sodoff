using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ST")]
[Serializable]
public enum Status {
	[XmlEnum("1")]
	Success = 1,
	
	[XmlEnum("2")]
	Failure,
	
	[XmlEnum("3")]
	ItemNotFound,
	
	[XmlEnum("4")]
	PoolItemsNotFound,
	
	[XmlEnum("5")]
	InvalidItemMap,
	
	[XmlEnum("6")]
	ItemNotFiltered,
	
	[XmlEnum("7")]
	InvalidInput,
	
	[XmlEnum("8")]
	InvalidGameMetaData,
	
	[XmlEnum("9")]
	InvalidItemRarity,
	
	[XmlEnum("10")]
	ItemStatsPersistFailed,
	
	[XmlEnum("11")]
	InvalidItemPayout,
	
	[XmlEnum("12")]
	InvalidStatsMap,
	
	[XmlEnum("13")]
	InvalidPossibleStatsMap,
	
	[XmlEnum("14")]
	ItemStatsNotExist,
	
	[XmlEnum("15")]
	ItemsNotMapped,
	
	[XmlEnum("16")]
	EventDataNotFound,
	
	[XmlEnum("17")]
	MissionDataNotFound,
	
	[XmlEnum("18")]
	SeasonRewardsNotFound,
	
	[XmlEnum("19")]
	ItemNotFoundInInventory,
	
	[XmlEnum("20")]
	BluePrintItemNotFound,
	
	[XmlEnum("21")]
	LowDeductibleItemQuantity,
	
	[XmlEnum("22")]
	InvalidBlueprintIngredients
}
