using System.Xml.Serialization;

namespace sodoff.Schema;

// Token: 0x020008AB RID: 2219
[XmlRoot(ElementName = "ATSR", Namespace = "")]
[Serializable]
public class AchievementTaskSetResponse
{
	public AchievementTaskSetResponse()
	{
		this.AchievementRewards = null;
	}

	[XmlElement(ElementName = "s")]
	public bool Success;

	[XmlElement(ElementName = "u")]
	public bool UserMessage;

	[XmlElement(ElementName = "a")]
	public string AchievementName;

	[XmlElement(ElementName = "l")]
	public int Level;

	[XmlElement(ElementName = "AR", IsNullable = true)]
	public AchievementReward[] AchievementRewards;

	[XmlElement(ElementName = "aid", IsNullable = true)]
	public int? AchievementTaskGroupID;

	[XmlElement(ElementName = "LL", IsNullable = true)]
	public bool? LastLevelCompleted;

	[XmlElement(ElementName = "aiid", IsNullable = true)]
	public int? AchievementInfoID;
}