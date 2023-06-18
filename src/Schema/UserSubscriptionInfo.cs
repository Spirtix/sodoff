using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserSubscriptionInfo", IsNullable = true, Namespace = "")]
[Serializable]
public class UserSubscriptionInfo
{
	[XmlElement(ElementName = "PID")]
	public string PID;

	[XmlElement(ElementName = "BillFrequency", IsNullable = true)]
	public short? BillFrequency;

	[XmlElement(ElementName = "CardExpirationDate", IsNullable = true)]
	public DateTime? CardExpirationDate;

	[XmlElement(ElementName = "CardReferenceNumber")]
	public string CardReferenceNumber;

	[XmlElement(ElementName = "IsActive", IsNullable = true)]
	public bool? IsActive;

	[XmlElement(ElementName = "LastBillDate", IsNullable = true)]
	public DateTime? LastBillDate;

	[XmlElement(ElementName = "MembershipID", IsNullable = true)]
	public int? MembershipID;

	[XmlElement(ElementName = "ProfileCurrency")]
	public string ProfileCurrency;

	[XmlElement(ElementName = "ProfileID")]
	public string ProfileID;

	[XmlElement(ElementName = "Recurring", IsNullable = true)]
	public bool? Recurring;

	[XmlElement(ElementName = "RecurringAmount", IsNullable = true)]
	public float? RecurringAmount;

	[XmlElement(ElementName = "Status")]
	public string Status;

	[XmlElement(ElementName = "SubscriptionDisplayName")]
	public string SubscriptionDisplayName;

	[XmlElement(ElementName = "SubscriptionEndDate", IsNullable = true)]
	public DateTime? SubscriptionEndDate;

	[XmlElement(ElementName = "SubscriptionID", IsNullable = true)]
	public int? SubscriptionID;

	[XmlElement(ElementName = "SubscriptionPlanID", IsNullable = true)]
	public int? SubscriptionPlanID;

	[XmlElement(ElementName = "SubscriptionProvider")]
	public UserSubscriptionProvider SubscriptionProvider;

	[XmlElement(ElementName = "SubscriptionTypeID", IsNullable = true)]
	public int? SubscriptionTypeID;

	[XmlElement(ElementName = "UserID", IsNullable = true)]
	public string UserID;

	[XmlElement(ElementName = "UserSubscriptionNotification", IsNullable = true)]
	public SubscriptionNotification UserSubscriptionNotification;
}
