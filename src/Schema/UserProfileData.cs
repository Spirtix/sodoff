using System.Xml.Serialization;

namespace sodoff.Schema;

// Token: 0x02000622 RID: 1570
[XmlRoot(ElementName = "UserProfileDisplayData", IsNullable = true, Namespace = "")]
[Serializable]
public class UserProfileData
{
	public string ID;

	[XmlElement(ElementName = "Avatar")]
	public AvatarDisplayData AvatarInfo;

	[XmlElement(ElementName = "Ach")]
	public int AchievementCount;

	[XmlElement(ElementName = "Mth")]
	public int MythieCount;

	[XmlElement(ElementName = "Answer", IsNullable = true)]
	public UserAnswerData AnswerData;

	[XmlElement(ElementName = "Game", IsNullable = true)]
	public GameDataSummary GameInfo;

	[XmlElement(ElementName = "gc", IsNullable = true)]
	public int? GameCurrency;

	[XmlElement(ElementName = "cc", IsNullable = true)]
	public int? CashCurrency;

	[XmlElement(ElementName = "BuddyCount", IsNullable = true)]
	public int? BuddyCount;

	[XmlElement(ElementName = "ActivityCount", IsNullable = true)]
	public int? ActivityCount;

	[XmlElement(ElementName = "Groups")]
	public UserProfileGroupData[] Groups;

	[XmlElement(ElementName = "UPT")]
	public UserProfileTag UserProfileTag;

	[XmlElement(ElementName = "UG", IsNullable = true)]
	public UserGrade UserGradeData;
}
