using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class AchievementController : Controller {

    private readonly DBContext ctx;
    private RankService rankService;
    public AchievementController(DBContext ctx, RankService rankService) {
        this.ctx = ctx;
        this.rankService = rankService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetPetAchievementsByUserID")]
    public IActionResult GetPetAchievementsByUserID([FromForm] string apiToken, [FromForm] string userId) {
        // TODO: check session

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        if (viking is null) {
            return null;
        }

        List<UserAchievementInfo> dragonsAchievement = new List<UserAchievementInfo>();
        foreach (Dragon dragon in viking.Dragons) {
            dragonsAchievement.Add(
                rankService.userAchievementInfo(dragon.EntityId, dragon.PetXP, AchievementPointTypes.DragonXP)
            );
        }

        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = dragonsAchievement.ToArray()
        };

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
        // TODO: check session
        
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);

        if (viking is null) {
            return null;
        }

        ArrayOfUserAchievementInfo arrAchievements = new ArrayOfUserAchievementInfo {
            UserAchievementInfo = new UserAchievementInfo[]{
                rankService.userAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                rankService.userAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                rankService.userAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
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
                PointTypeID = AchievementPointTypes.CashCurrency,
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
                    PointTypeID = AchievementPointTypes.PlayerXP,
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
                PointTypeID = AchievementPointTypes.PlayerXP,
                EntityID = Guid.Parse(viking.Id),
                EntityTypeID = 1,
                RewardID = 552
            }
        });
    }
}
