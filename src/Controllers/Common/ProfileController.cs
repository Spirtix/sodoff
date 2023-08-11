using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ProfileController : Controller {

    private readonly DBContext ctx;
    public ProfileController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetUserProfileByUserID")]
    public IActionResult GetUserProfileByUserID([FromForm] string apiToken, [FromForm] string userId) {
        Session session = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken);
        if (session?.User is null && session?.Viking is null) {
            // TODO: what response for not logged in?
            return Ok();
        }

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        return Ok(GetProfileDataFromViking(viking));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetUserProfile")]
    public IActionResult GetUserProfile([FromForm] string apiToken) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        User? user = viking?.User;
        if (user is null || viking is null) {
            // TODO: what response for not logged in?
            return Ok();
        }

		return Ok(GetProfileDataFromViking(viking));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetDetailedChildList")]
    public Schema.UserProfileDataList? GetDetailedChildList([FromForm] string parentApiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null)
            // TODO: what response for not logged in?
            return null;

        if (user.Vikings.Count <= 0)
            return null;

        UserProfileData[] profiles = user.Vikings.Select(GetProfileDataFromViking).ToArray();
        return new UserProfileDataList {
            UserProfiles = profiles
        };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetQuestions")]
    public IActionResult GetQuestions([FromForm] string apiToken) {
		return Ok(new ProfileQuestionData {
            Lists = new ProfileQuestionList[] {
                new ProfileQuestionList {
                    ID = 4,
                    Questions = new ProfileQuestion[] {
                        new ProfileQuestion {
                            CategoryID = 3,
                            IsActive = "true", // this is a string, which makes me sad
                            Locale = "en-US",
                            Ordinal = 1,
                            ID = 48,
                            DisplayText = "How Did You Hear About US ?",
                            Answers = new ProfileAnswer[] {
                                new ProfileAnswer {
                                    ID = 320,
                                    DisplayText = "TV Commercial",
                                    Locale = "en-US",
                                    Ordinal = 1,
                                    QuestionID = 48
                                },
                                new ProfileAnswer {
                                    ID = 324,
                                    DisplayText = "I bought the RIders Of Berk DVD",
                                    Locale = "en-US",
                                    Ordinal = 5,
                                    QuestionID = 48
                                },
                                new ProfileAnswer {
                                    ID = 325,
                                    DisplayText = "I bought the Defenders of Berk DVD",
                                    Locale = "en-US",
                                    Ordinal = 6,
                                    QuestionID = 48
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    private UserProfileData GetProfileDataFromViking(Viking viking) {
        // Get the avatar data
        AvatarData avatarData = null;
        if (viking.AvatarSerialized is not null) {
            avatarData = XmlUtil.DeserializeXml<AvatarData>(viking.AvatarSerialized);
        }

        // Build the AvatarDisplayData
        AvatarDisplayData avatar = new AvatarDisplayData {
            AvatarData = avatarData,
            UserInfo = new UserInfo {
                MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                UserID = viking.Id,
                ParentUserID = viking.UserId,
                Username = viking.Name,
                FirstName = viking.Name,
                MultiplayerEnabled = true,
                Locale = "en-US", // placeholder
                GenderID = Gender.Male, // placeholder
                OpenChatEnabled = true,
                IsApproved = true,
                RegistrationDate = new DateTime(DateTime.Now.Ticks), // placeholder
                CreationDate = new DateTime(DateTime.Now.Ticks), // placeholder
                FacebookUserID = 0
            },
            UserSubscriptionInfo = new UserSubscriptionInfo {
                UserID = viking.UserId,
                MembershipID = 130687131, // placeholder
                SubscriptionTypeID = 2, // placeholder
                SubscriptionDisplayName = "NonMember", // placeholder
                SubscriptionPlanID = 41, // placeholder
                SubscriptionID = -3, // placeholder
                IsActive = false, // placeholder
            },
            RankID = 0, // placeholder
            AchievementInfo = null, // placeholder
            Achievements = new UserAchievementInfo[] {
                new UserAchievementInfo {
                    UserID = Guid.Parse(viking.Id),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = AchievementPointTypes.PlayerXP
                },
                new UserAchievementInfo {
                    UserID = Guid.Parse(viking.Id),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = AchievementPointTypes.PlayerFarmingXP
                },
                new UserAchievementInfo {
                    UserID = Guid.Parse(viking.Id),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = AchievementPointTypes.PlayerFishingXP
                },
            }
        };

        return new UserProfileData {
            ID = viking.Id,
            AvatarInfo = avatar,
            AchievementCount = 0,
            MythieCount = 0,
            AnswerData = new UserAnswerData { UserID = viking.Id },
            GameCurrency = 65536,
            CashCurrency = 65536,
            ActivityCount = 0,
            BuddyCount = 0,
            UserGradeData = new UserGrade { UserGradeID = 0 }
        };
    }
}
