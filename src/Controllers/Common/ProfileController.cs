using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
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
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null) {
            // TODO: what response for not logged in?
            return Ok();
        }

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);

        return Ok(new UserProfileData {
            ID = viking.Id,
            AvatarInfo = new AvatarDisplayData {
                UserInfo = new UserInfo {
                    UserID = viking.Id,
                    ParentUserID = user.Id,
                    Username = viking.Name,
                    MultiplayerEnabled = true,
                    GenderID = Gender.Male,
                    OpenChatEnabled = true,
                    CreationDate = DateTime.Now
                },
                UserSubscriptionInfo = new UserSubscriptionInfo { SubscriptionTypeID = 2 }, // TODO: figure out what this is
                Achievements = new UserAchievementInfo[] {
                    new UserAchievementInfo {
                        UserID = Guid.Parse(viking.Id),
                        AchievementPointTotal = 0,
                        RankID = 1,
                        PointTypeID = 1, // TODO: what is this?
                    }
                }
            },
            AchievementCount = 0,
            MythieCount = 0,
            AnswerData = new UserAnswerData { UserID = viking.Id },
            GameCurrency = 0,
            CashCurrency = 0
        });
    }
}
