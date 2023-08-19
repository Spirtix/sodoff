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
    private AchievementService achievementService;
    public AchievementController(DBContext ctx, AchievementService achievementService) {
        this.ctx = ctx;
        this.achievementService = achievementService;
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
                achievementService.CreateUserAchievementInfo(dragon.EntityId, dragon.PetXP, AchievementPointTypes.DragonXP)
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
    [Route("AchievementWebService.asmx/SetDragonXP")]  // used by dragonrescue-import
    public IActionResult SetDragonXP([FromForm] string apiToken, [FromForm] string dragonId, [FromForm] int value) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return Unauthorized();
        }

        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.EntityId == dragonId);
        if (dragon is null) {
            return Conflict("Dragon not found");
        }

        dragon.PetXP = value;
        ctx.SaveChanges();

        return Ok("OK");
    }

    [HttpPost]
    [Route("AchievementWebService.asmx/SetPlayerXP")]  // used by dragonrescue-import
    public IActionResult SetDragonXP([FromForm] string apiToken, [FromForm] int type, [FromForm] int value) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return Unauthorized();
        }

        if (!Enum.IsDefined(typeof(AchievementPointTypes), type)) {
            return Conflict("Invalid XP type");
        }

        AchievementPointTypes xpType = (AchievementPointTypes)type;
        // TODO: we allow set currencies here, do we want this?
        achievementService.SetAchievementPoints(viking, xpType, value);
        ctx.SaveChanges();
        return Ok("OK");
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAchievementsByUserID")]
    public IActionResult GetAchievementsByUserID([FromForm] string userId) {
        // TODO: check session
        
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        if (viking != null) {
            return Ok(new ArrayOfUserAchievementInfo {
                UserAchievementInfo = new UserAchievementInfo[]{
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                    achievementService.CreateUserAchievementInfo(viking.Id, 60000, AchievementPointTypes.PlayerFarmingXP), // TODO: placeholder until there is no leveling for farm XP
                    achievementService.CreateUserAchievementInfo(viking.Id, 20000, AchievementPointTypes.PlayerFishingXP), // TODO: placeholder until there is no leveling for fishing XP
                }
            });
        }

        Dragon? dragon = ctx.Dragons.FirstOrDefault(e => e.EntityId == userId);
        if (dragon != null) {
            return Ok(new ArrayOfUserAchievementInfo {
                UserAchievementInfo = new UserAchievementInfo[]{
                    achievementService.CreateUserAchievementInfo(dragon.EntityId, dragon.PetXP, AchievementPointTypes.DragonXP),
                }
            });
        }

        return null;
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
