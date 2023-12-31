﻿using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ProfileController : Controller {

    private readonly DBContext ctx;
    private AchievementService achievementService;
    public ProfileController(DBContext ctx, AchievementService achievementService) {
        this.ctx = ctx;
        this.achievementService = achievementService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetUserProfileByUserID")]
    public IActionResult GetUserProfileByUserID([FromForm] Guid userId, [FromForm] string apiKey) {
        // NOTE: this is public info (for mmo) - no session check

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == userId);
        if (viking is null) {
            return Ok(new UserProfileData());
            // NOTE: do not return `Conflict("Viking not found")` due to client side error handling
            //       (not Ok response cause soft-lock client - can't close error message)
        }

        return Ok(GetProfileDataFromViking(viking, apiKey));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetUserProfile")]
    [VikingSession(UseLock=false)]
    public IActionResult GetUserProfile(Viking viking, [FromForm] string apiKey) {
        return Ok(GetProfileDataFromViking(viking, apiKey));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetDetailedChildList")]
    [VikingSession(Mode=VikingSession.Modes.USER, ApiToken="parentApiToken", UseLock=false)]
    public Schema.UserProfileDataList? GetDetailedChildList(User user, [FromForm] string apiKey) {
        if (user.Vikings.Count <= 0)
            return null;

        UserProfileData[] profiles = user.Vikings.Select(v => GetProfileDataFromViking(v, apiKey)).ToArray();
        return new UserProfileDataList {
            UserProfiles = profiles
        };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetQuestions")]
    public IActionResult GetQuestions() {
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
    
    [HttpPost]
    //[Produces("application/xml")]
    [Route("ProfileWebService.asmx/GetProfileTagAll")] // used by Magic & Mythies
    public IActionResult GetProfileTagAll() {
        // TODO: This is a placeholder
        return Ok("<?xml version='1.0' encoding='UTF-8'?><ArrayOfProfileTag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\"/>");
    }
    
    private UserProfileData GetProfileDataFromViking(Viking viking, [FromForm] string apiKey) {
        // Get the avatar data
        AvatarData avatarData = null;
        if (viking.AvatarSerialized is not null) {
            avatarData = XmlUtil.DeserializeXml<AvatarData>(viking.AvatarSerialized);
            avatarData.Id = viking.Id;
        }

        if (avatarData != null && ClientVersion.Use2019SoDTutorial(apiKey)) {
            if (avatarData.Part.FirstOrDefault(e => e.PartType == "Sword") is null) {
                var extraParts = new AvatarDataPart[] {
                    new AvatarDataPart {
                        PartType = "Sword",
                        Geometries = new string[] {"NULL"},
                        Textures = new string[] {"__EMPTY__"},
                        UserInventoryId = null,
                    }
                };
                avatarData.Part = extraParts.Concat(avatarData.Part).ToArray();
            }
        }

        // Build the AvatarDisplayData
        AvatarDisplayData avatar = new AvatarDisplayData {
            AvatarData = avatarData,
            UserInfo = new UserInfo {
                MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                UserID = viking.Uid.ToString(),
                ParentUserID = viking.UserId.ToString(),
                Username = viking.Name,
                FirstName = viking.Name,
                MultiplayerEnabled = !ClientVersion.IsOldSoD(apiKey),
                Locale = "en-US", // placeholder
                GenderID = Gender.Male, // placeholder
                OpenChatEnabled = true,
                IsApproved = true,
                RegistrationDate = new DateTime(DateTime.Now.Ticks), // placeholder
                CreationDate = new DateTime(DateTime.Now.Ticks), // placeholder
                FacebookUserID = 0
            },
            UserSubscriptionInfo = new UserSubscriptionInfo {
                UserID = viking.UserId.ToString(),
                MembershipID = 130687131, // placeholder
                SubscriptionTypeID = 1, // placeholder
                SubscriptionDisplayName = "Member", // placeholder
                SubscriptionPlanID = 41, // placeholder
                SubscriptionID = -3, // placeholder
                IsActive = true, // placeholder
            },
            RankID = 0, // placeholder
            AchievementInfo = null, // placeholder
            Achievements = new UserAchievementInfo[] {
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.UDTPoints),
            }
        };

        UserGameCurrency currency = achievementService.GetUserCurrency(viking);

        return new UserProfileData {
            ID = viking.Uid.ToString(),
            AvatarInfo = avatar,
            AchievementCount = 0,
            MythieCount = 0,
            AnswerData = new UserAnswerData { UserID = viking.Uid.ToString() },
            GameCurrency = currency.GameCurrency,
            CashCurrency = currency.CashCurrency,
            ActivityCount = 0,
            BuddyCount = 0,
            UserGradeData = new UserGrade { UserGradeID = 0 }
        };
    }
}
