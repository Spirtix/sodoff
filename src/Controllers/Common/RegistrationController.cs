using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RegistrationController : Controller {

    private readonly DBContext ctx;
    private ItemService itemService;
    private MissionService missionService;
    private RoomService roomService;

    public RegistrationController(DBContext ctx, ItemService itemService, MissionService missionService, RoomService roomService) {
        this.ctx = ctx;
        this.itemService = itemService;
        this.missionService = missionService;
        this.roomService = roomService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/RegistrationWebService.asmx/DeleteProfile")]
    public IActionResult DeleteProfile([FromForm] string apiToken, [FromForm] string userID) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null) {
            return Ok(DeleteProfileStatus.OWNER_ID_NOT_FOUND);
        }

        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userID);
        if (viking is null) {
            return Ok(DeleteProfileStatus.PROFILE_NOT_FOUND);
        }

        if (user != viking.User) {
            return Ok(DeleteProfileStatus.PROFILE_NOT_OWNED_BY_THIS_OWNER);
        }

        ctx.Vikings.Remove(viking);
        ctx.SaveChanges();

        return Ok(DeleteProfileStatus.SUCCESS);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/RegistrationWebService.asmx/RegisterParent")]
    [DecryptRequest("parentRegistrationData")]
    [EncryptResponse]
    public IActionResult RegisterParent() {
        ParentRegistrationData data = XmlUtil.DeserializeXml<ParentRegistrationData>(Request.Form["parentRegistrationData"]);
        User u = new User {
            Id = Guid.NewGuid().ToString(),
            Username = data.ChildList[0].ChildName,
            Password = new PasswordHasher<object>().HashPassword(null, data.Password),
            Email = data.Email
        };

        // Check if user exists
        if (ctx.Users.Count(e => e.Email == u.Email) > 0 || ctx.Users.Count(e => e.Username== u.Username) > 0) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateEmail });
        }

        ctx.Users.Add(u);
        ctx.SaveChanges();

        ParentLoginInfo pli = new ParentLoginInfo {
            UserName = u.Username,
            ApiToken = Guid.NewGuid().ToString(),
            UserID = u.Id,
            Status = MembershipUserStatus.Success,
            UnAuthorized = false
        };

        var response = new RegistrationResult {
            ParentLoginInfo = pli,
            UserID = u.Id,
            Status = MembershipUserStatus.Success,
            ApiToken = Guid.NewGuid().ToString()
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V4/RegistrationWebService.asmx/RegisterChild")]
    [DecryptRequest("childRegistrationData")]
    [EncryptResponse]
    public IActionResult RegisterChild([FromForm] string parentApiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null) {
            return Ok(new RegistrationResult{
                Status = MembershipUserStatus.InvalidApiToken
            });
        }

        // Check if name populated
        ChildRegistrationData data = XmlUtil.DeserializeXml<ChildRegistrationData>(Request.Form["childRegistrationData"]);
        if (String.IsNullOrWhiteSpace(data.ChildName)) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.ValidationError });
        }

        // Check if viking exists
        if (ctx.Vikings.Count(e => e.Name == data.ChildName) > 0) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateUserName });
        }

        Inventory inv = new Inventory { InventoryItems = new List<InventoryItem>() };
        inv.InventoryItems.Add(new InventoryItem { ItemId = 8977, Quantity = 1 }); // DragonStableINTDO - Dragons Dragon Stable
        Viking v = new Viking {
            Id = Guid.NewGuid().ToString(),
            Name = data.ChildName,
            User = user,
            Inventory = inv,
            AchievementPoints = new List<AchievementPoints>(),
            Rooms = new List<Room>()
        };
        
        missionService.SetUpMissions(v);
        
        ctx.Vikings.Add(v);
        ctx.SaveChanges();

        roomService.CreateRoom(v, "MyRoomINT");

        return Ok(new RegistrationResult {
            UserID = v.Id,
            Status = MembershipUserStatus.Success
        });
    }
}
