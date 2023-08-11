using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class AchievementController : Controller {

    private readonly DBContext ctx;
    public AchievementController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetPetAchievementsByUserID")]
    public IActionResult GetPetAchievementsByUserID([FromForm] string apiToken, [FromForm] string userId) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return null;
        }

        List<UserAchievementInfo> dragonsAchievement = new List<UserAchievementInfo>();
        foreach (Dragon dragon in viking.Dragons) { // TODO (multiplayer) we should use userId
            dragonsAchievement.Add(new UserAchievementInfo{
                PointTypeID = (int) AchievementPointTypes.DragonXP,
                UserID = Guid.Parse(dragon.EntityId),
                AchievementPointTotal = dragon.PetXP,
                RankID = (int)dragon.PetXP/1024 + 1 // TODO: placeholder
            });
        }

        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = dragonsAchievement.ToArray()
        };

        // TODO, this is a placeholder
        return Ok(arrAchievements);
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAllRanks")]
    public IActionResult GetAllRanks() {
        // TODO, this is a placeholder
        return Ok(XmlUtil.ReadResourceXmlString("allranks"));
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAchievementTaskInfo")]
    public IActionResult GetAchievementTaskInfo() {
        // TODO
        return Ok(XmlUtil.ReadResourceXmlString("achievementtaskinfo"));
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAllRewardTypeMultiplier")]
    public IActionResult GetAllRewardTypeMultiplier() {
        // TODO
        return Ok(XmlUtil.ReadResourceXmlString("rewardmultiplier"));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAchievementsByUserID")]
    public IActionResult GetAchievementsByUserID([FromForm] string userId) {
        // TODO: this is a placeholder
        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = new UserAchievementInfo[]{
                new UserAchievementInfo {
                    UserID = Guid.Parse(userId),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = (int) AchievementPointTypes.PlayerXP
                },
                new UserAchievementInfo {
                    UserID = Guid.Parse(userId),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = (int) AchievementPointTypes.PlayerFarmingXP
                },
                new UserAchievementInfo {
                    UserID = Guid.Parse(userId),
                    AchievementPointTotal = 5000,
                    RankID = 30,
                    PointTypeID = (int) AchievementPointTypes.PlayerFishingXP
                },
            }
        };

        return Ok(arrAchievements);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/SetAchievementAndGetReward")]
    public IActionResult SetAchievementAndGetReward([FromForm] string apiToken, [FromForm] int achievementID) {
        // TODO: This is a placeholder; returns 5 gems
        Viking? viking = ctx.Sessions.FirstOrDefault(x => x.ApiToken == apiToken).Viking;
        return Ok(new AchievementReward[1] {
            new AchievementReward {
                Amount = 5,
                PointTypeID = (int) AchievementPointTypes.CashCurrency,
                EntityID = Guid.Parse(viking.Id),
                EntityTypeID = 1,
                RewardID = 552
            }
        });
    }

        [HttpPost]
    [Produces("application/xml")]
    [Route("V2/AchievementWebService.asmx/SetUserAchievementTask")]
    [DecryptRequest("achievementTaskSetRequest")]
    public IActionResult SetUserAchievementTask([FromForm] string apiToken, [FromForm] int achievementID) {
        // TODO: This is a placeholder
        string xml = Request.Form["achievementTaskSetRequest"];
        AchievementTaskSetResponse response = new AchievementTaskSetResponse {
            Success = true,
            UserMessage = true,
            AchievementName = "Placeholder Achievement",
            Level = 1,
            AchievementTaskGroupID = 1279,
            LastLevelCompleted = true,
            AchievementInfoID = 1279,
            AchievementRewards = new AchievementReward[1] {
                new AchievementReward {
                    Amount = 25,
                    PointTypeID = (int) AchievementPointTypes.PlayerXP,
                    RewardID = 910,
                    EntityTypeID =1
                }
            }
        };
        return Ok(new ArrayOfAchievementTaskSetResponse { AchievementTaskSetResponse = new AchievementTaskSetResponse[1] { response } });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/SetAchievementByEntityIDs")]
    public IActionResult SetAchievementByEntityIDs([FromForm] string apiToken, [FromForm] int achievementID) {
        // TODO: This is a placeholder
        Viking? viking = ctx.Sessions.FirstOrDefault(x => x.ApiToken == apiToken).Viking;
        return Ok(new AchievementReward[1] {
            new AchievementReward {
                Amount = 25,
                PointTypeID = (int) AchievementPointTypes.PlayerXP,
                EntityID = Guid.Parse(viking.Id),
                EntityTypeID = 1,
                RewardID = 552
            }
        });
    }
}
