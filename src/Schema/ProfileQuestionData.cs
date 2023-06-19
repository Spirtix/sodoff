using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "QuestionData", IsNullable = true, Namespace = "")]
[Serializable]
public class ProfileQuestionData
{
	[XmlElement(ElementName = "QL")]
	public ProfileQuestionList[] Lists;

	public const int ANSWER_BOY = 227;

	public const int ANSWER_GIRL = 228;

	public const int ANSWER_UNKNOWN = 229;

	public const int ANSWER_TRUE = 32;

	public const int ANSWER_FALSE = 33;

	public const int COUNTRY_QUESTIONS = 33;

	public const int GENDER_QUESTIONS = 32;

	public const int LIST_ID_FAVORITES = 1;

	public const int LIST_ID_STATUS = 2;

	public const int LIST_ID_GROUPS = 3;
}
