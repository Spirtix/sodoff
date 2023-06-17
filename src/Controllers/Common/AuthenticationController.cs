using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

[ApiController]
public class AuthenticationController : Controller {

    private readonly DBContext ctx;

    public AuthenticationController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/GetRules")]
    [EncryptResponse]
    public IActionResult GetRules() {
        GetProductRulesResponse response = new GetProductRulesResponse {
            GlobalSecretKey = "11A0CC5A-C4DF-4A0E-931C-09A44C9966AE"
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/AuthenticationWebService.asmx/LoginParent")]
    [DecryptRequest("parentLoginData")]
    [EncryptResponse]
    public IActionResult LoginParent() {
        ParentLoginData data = XmlUtil.DeserializeXml<ParentLoginData>(Request.Form["parentLoginData"]);

        // Authenticate the user
        User? user = ctx.Users.FirstOrDefault(e => e.Username == data.UserName);
        if (user is null || new PasswordHasher<object>().VerifyHashedPassword(null, user.Password, data.Password) != PasswordVerificationResult.Success) {
            return Ok(new ParentLoginInfo { Status = MembershipUserStatus.InvalidPassword });
        }

        // Create session
        Session session = new Session {
            User = user,
            ApiToken = Guid.NewGuid().ToString()
        };

        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        var response = new ParentLoginInfo {
            UserName = user.Username,
            Email = user.Email,
            ApiToken = session.ApiToken,
            UserID = user.Id,
            Status = MembershipUserStatus.Success,
            SendActivationReminder = false,
            UnAuthorized = false
        };

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/GetUserInfoByApiToken")]
    public IActionResult GetUserInfoByApiToken([FromForm] string apiToken) {
        // First check if this is a user session
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is not null) {
            return Ok(new UserInfo {
                UserID = user.Id,
                Username = user.Username,
                MultiplayerEnabled = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Then check if this is a viking session
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is not null)
        {
            return Ok(new UserInfo {
                UserID = viking.Id,
                Username = viking.Name,
                MultiplayerEnabled = true,
                Age = 24,
                OpenChatEnabled = true
            });
        }

        // Otherwise, this is a bad session, return empty UserInfo
        return Ok(new UserInfo {});
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/IsValidApiToken_V2")]
    public IActionResult IsValidApiToken([FromForm] string apiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null)
            return Ok(ApiTokenStatus.TokenNotFound);
        return Ok(ApiTokenStatus.TokenValid);
    }

    // This is more of a "create session for viking", rather than "login child"
    [HttpPost]
    [Produces("application/xml")]
    [Route("AuthenticationWebService.asmx/LoginChild")]
    [DecryptRequest("childUserId")]
    [EncryptResponse]
    public IActionResult LoginChild([FromForm] string parentApiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
        if (user is null) {
            // Return empty response
            return Ok();
        }

        // Find the viking
        string? childUserId = Request.Form["parentLoginData"];
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == childUserId);

        // Create session
        Session session = new Session {
            Viking = viking,
            ApiToken = Guid.NewGuid().ToString()
        };
        ctx.Sessions.Add(session);
        ctx.SaveChanges();

        // Return back the api token
        return Ok(session.ApiToken);
    }
}
