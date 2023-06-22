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

    public RegistrationController(DBContext ctx, ItemService itemService) {
        this.ctx = ctx;
        this.itemService = itemService;
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
            Inventory = inv
        };
        ctx.Vikings.Add(v);
        ctx.SaveChanges();

        return Ok(new RegistrationResult {
            UserID = v.Id,
            Status = MembershipUserStatus.Success
        });
    }
}
