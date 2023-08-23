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
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
                    achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.UDTPoints),
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
    [Route("AchievementWebService.asmx/SetUserAchievementAndGetReward")]
    public IActionResult SetAchievementAndGetReward([FromForm] string apiToken, [FromForm] int achievementID) {
        Viking? viking = ctx.Sessions.FirstOrDefault(x => x.ApiToken == apiToken).Viking;

        if (viking is null) {
            return Unauthorized();
        }

        var rewards = achievementService.ApplyAchievementRewardsByID(viking, achievementID);
        ctx.SaveChanges();

        return Ok(rewards);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/AchievementWebService.asmx/SetUserAchievementTask")]
    [DecryptRequest("achievementTaskSetRequest")]
    public IActionResult SetUserAchievementTask([FromForm] string apiToken) {
        Viking? viking = ctx.Sessions.FirstOrDefault(x => x.ApiToken == apiToken).Viking;

        if (viking is null) {
            return Unauthorized();
        }
        
        AchievementTaskSetRequest request = XmlUtil.DeserializeXml<AchievementTaskSetRequest>(Request.Form["achievementTaskSetRequest"]);

        var response = new List<AchievementTaskSetResponse>();
        foreach (var task in request.AchievementTaskSet) {
            response.Add(
                new AchievementTaskSetResponse {
                    Success = true,
                    UserMessage = true, // TODO: placeholder
                    AchievementName = "Placeholder Achievement", // TODO: placeholder
                    Level = 1, // TODO: placeholder
                    AchievementTaskGroupID = 1279, // TODO: placeholder
                    LastLevelCompleted = true, // TODO: placeholder
                    AchievementInfoID = 1279, // TODO: placeholder
                    AchievementRewards = achievementService.ApplyAchievementRewardsByTask(viking, task)
                }
            );
        }
        ctx.SaveChanges();

        return Ok(new ArrayOfAchievementTaskSetResponse { AchievementTaskSetResponse = response.ToArray() });
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
