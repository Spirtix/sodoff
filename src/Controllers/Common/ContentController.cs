using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class ContentController : Controller {

    private readonly DBContext ctx;
    private KeyValueService keyValueService;
    public ContentController(DBContext ctx, KeyValueService keyValueService) {
        this.ctx = ctx;
        this.keyValueService = keyValueService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetDefaultNameSuggestion")]
    public IActionResult GetDefaultNameSuggestion() {
        // TODO:  generate random names, and ensure they aren't already taken
        string[] suggestions = new string[] { "dragon1", "dragon2", "dragon3" };
        return Ok(new DisplayNameUniqueResponse {
            Suggestions = new SuggestionResult {
                Suggestion = suggestions
            }
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/ValidateName")]
    public IActionResult ValidateName([FromForm] string apiToken,[FromForm] string nameValidationRequest) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null) {
            // TODO: better error handling than just replying not unique
            return Ok(new NameValidationResponse { Result = NameValidationResult.NotUnique });
        }

        // Check if name populated
        NameValidationRequest request = XmlUtil.DeserializeXml<NameValidationRequest>(nameValidationRequest);

        if (request.Category == NameCategory.Default) {
            // This is an avatar we are checking
            // Check if viking exists
            bool exists = ctx.Vikings.Count(e => e.Name == request.Name) > 0;
            NameValidationResult result = exists ? NameValidationResult.NotUnique : NameValidationResult.Ok;
            return Ok(new NameValidationResponse { Result = result});

        } else {
            // TODO: pets, groups, default
            return Ok();
        }
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetKeyValuePair")]
    public Schema.PairData? GetKeyValuePair([FromForm] string apiToken, [FromForm] int pairId) {
        // Find session by apiToken
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null)
            return null;

        // Get the pair
        Model.PairData? pair = keyValueService.GetPairData(session.UserId, session.VikingId, pairId);
        
        return keyValueService.ModelToSchema(pair);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetKeyValuePair")]
    public IActionResult SetKeyValuePair([FromForm] string apiToken, [FromForm] int pairId, [FromForm] string contentXML) {
        Schema.PairData schemaData = XmlUtil.DeserializeXml<Schema.PairData>(contentXML);

        // Get the session
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null)
            return Ok(false);
        
        bool result = keyValueService.SetPairData(session.UserId, session.VikingId, pairId, schemaData);

        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetKeyValuePairByUserID")]
    public Schema.PairData? GetKeyValuePairByUserID([FromForm] string apiToken, [FromForm] int pairId, [FromForm] string userId) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null)
            return null;

        Model.PairData? pair = keyValueService.GetPairData(userId, null, pairId);

        return keyValueService.ModelToSchema(pair);
    }
    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetKeyValuePairByUserID")]
    public IActionResult SetKeyValuePairByUserID([FromForm] string apiToken, [FromForm] int pairId, [FromForm] string userId, [FromForm] string contentXML) {
        Schema.PairData schemaData = XmlUtil.DeserializeXml<Schema.PairData>(contentXML);

        // Get the session
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null || string.IsNullOrEmpty(userId))
            return Ok(false);

        bool result = keyValueService.SetPairData(userId, null, pairId, schemaData);

        return Ok(result);
    }

}
