using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Services;
using sodoff.Util;
using System;

namespace sodoff.Controllers.Common;
public class ContentController : Controller {

    private readonly DBContext ctx;
    private KeyValueService keyValueService;
    private ItemService itemService;
    public ContentController(DBContext ctx, KeyValueService keyValueService, ItemService itemService) {
        this.ctx = ctx;
        this.keyValueService = keyValueService;
        this.itemService = itemService;
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

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetCommonInventory")]
    public IActionResult GetCommonInventory([FromForm] string apiToken) {
        // TODO, this is a placeholder
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (user is null && viking is null) {
            return Ok();
        }

        return Ok(new CommonInventoryData {
            UserID = Guid.Parse(user is not null ? user.Id : viking.Id)
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetCommonInventory")]
    public IActionResult GetCommonInventoryV2([FromForm] string apiToken) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null || viking.Inventory is null) return Ok();

        List<InventoryItem> items = viking.Inventory.InventoryItems.ToList();
        List<UserItemData> userItemData = new();
        foreach (InventoryItem item in items) {
            ItemData itemData = itemService.GetItem(item.ItemId);
            UserItemData uid = new UserItemData {
                UserInventoryID = viking.Inventory.Id,
                ItemID = itemData.ItemID,
                Quantity = item.Quantity,
                Uses = itemData.Uses,
                ModifiedDate = DateTime.Now,
                Item = itemData
            };
            userItemData.Add(uid);
        }

        CommonInventoryData invData = new CommonInventoryData {
            UserID = Guid.Parse(viking.UserId),
            Item = userItemData.ToArray()
        };
        return Ok(invData);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetCommonInventory")]
    public IActionResult SetCommonInventory([FromForm] string apiToken, [FromForm] string commonInventoryRequestXml) {
        CommonInventoryRequest[] request = XmlUtil.DeserializeXml<CommonInventoryRequest[]>(commonInventoryRequestXml);
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null || viking.Inventory is null) return Ok();

        // Set inventory items
        List<CommonInventoryResponseItem> responseItems = new();
        foreach (var req in request) {
            InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID);
            if (item is null) item = new InventoryItem { ItemId = (int)req.ItemID, Quantity = 0 };
            int origQuantity = item.Quantity;
            item.Quantity = request[0].Quantity;
            responseItems.Add(new CommonInventoryResponseItem {
                CommonInventoryID = viking.InventoryId,
                ItemID = item.ItemId,
                Quantity = origQuantity
            });
            viking.Inventory.InventoryItems.Add(item);
        }

        CommonInventoryResponse response = new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = responseItems.ToArray()
        };

        ctx.SaveChanges();
        return Ok(response);
    }


    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetAuthoritativeTime")]
    public IActionResult GetAuthoritativeTime() {
        return Ok(new DateTime(DateTime.Now.Ticks));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ItemStoreWebService.asmx/GetItem")] // NOTE: Should be in a separate controler, but it's inventory related, so I'll leave it here for now
    public IActionResult GetItem([FromForm] int itemId) {
        return Ok(itemService.GetItem(itemId));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetAvatar")]
    public IActionResult SetAvatar([FromForm] string apiToken, [FromForm] string contentXML) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return Ok(new SetAvatarResult {
                Success = false,
                StatusCode = AvatarValidationResult.Error
            });
        }

        viking.AvatarSerialized = contentXML;
        ctx.SaveChanges();

        return Ok(new SetAvatarResult {
			Success = true,
			DisplayName = viking.Name,
			StatusCode = AvatarValidationResult.Valid
		});
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/CreatePet")]
    public IActionResult CreatePet([FromForm] string apiToken, [FromForm] string request) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            // TODO: result for invalid session
            return Ok();
        }

        RaisedPetRequest raisedPetRequest = XmlUtil.DeserializeXml<RaisedPetRequest>(request);
        // TODO: Investigate SetAsSelectedPet and UnSelectOtherPets - they don't seem to do anything

        // Update the RaisedPetData with the info
        String dragonId = Guid.NewGuid().ToString();
        raisedPetRequest.RaisedPetData.IsPetCreated = true;
        raisedPetRequest.RaisedPetData.RaisedPetID = 0; // Initially make zero, so the db auto-fills
        raisedPetRequest.RaisedPetData.EntityID = Guid.Parse(dragonId);
        raisedPetRequest.RaisedPetData.Name = string.Concat("Dragon-", dragonId.AsSpan(0, 8)); // Start off with a random name
        raisedPetRequest.RaisedPetData.IsSelected = false; // The api returns false, not sure why
        raisedPetRequest.RaisedPetData.CreateDate = new DateTime(DateTime.Now.Ticks);
        raisedPetRequest.RaisedPetData.UpdateDate = new DateTime(DateTime.Now.Ticks);

        // Save the dragon in the db
        Dragon dragon = new Dragon {
            EntityId = Guid.NewGuid().ToString(),
            Viking = viking,
            RaisedPetData = XmlUtil.SerializeXml(raisedPetRequest.RaisedPetData),
        };

        if (raisedPetRequest.SetAsSelectedPet == true) {
            viking.SelectedDragon = dragon;
            ctx.Update(viking);
        }
        ctx.Dragons.Add(dragon);
        ctx.SaveChanges();

        return Ok(new CreatePetResponse {
            RaisedPetData = GetRaisedPetDataFromDragon(dragon)
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/ContentWebService.asmx/SetRaisedPet")]
    public IActionResult SetRaisedPet([FromForm] string apiToken, [FromForm] string request) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            // TODO: result for invalid session
            return Ok();
        }

        RaisedPetRequest raisedPetRequest = XmlUtil.DeserializeXml<RaisedPetRequest>(request);

        // Find the dragon
        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.Id == raisedPetRequest.RaisedPetData.RaisedPetID);
        if (dragon is null) {
            return Ok(new SetRaisedPetResponse {
                RaisedPetSetResult = RaisedPetSetResult.Invalid
            });
        }

        dragon.RaisedPetData = XmlUtil.SerializeXml(raisedPetRequest.RaisedPetData);
        ctx.Update(dragon);
        ctx.SaveChanges();

        return Ok(new SetRaisedPetResponse {
            RaisedPetSetResult = RaisedPetSetResult.Success
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetSelectedPet")]
    public IActionResult SetSelectedPet([FromForm] string apiToken, [FromForm] int raisedPetID) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            // TODO: result for invalid session
            return Ok();
        }

        // Find the dragon
        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.Id == raisedPetID);
        if (dragon is null) {
            return Ok(new SetRaisedPetResponse {
                RaisedPetSetResult = RaisedPetSetResult.Invalid
            });
        }

        // Set the dragon as selected
        viking.SelectedDragon = dragon;
        ctx.Update(viking);
        ctx.SaveChanges();

        return Ok(new SetRaisedPetResponse {
            RaisedPetSetResult = RaisedPetSetResult.Success
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetAllActivePetsByuserId")]
    public RaisedPetData[]? GetAllActivePetsByuserId([FromForm] string apiToken, [FromForm] string userId, [FromForm] bool active) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return null;
        }

        RaisedPetData[] dragons = viking.Dragons
            .Where(d => d.RaisedPetData is not null)
            .Select(GetRaisedPetDataFromDragon)
            .ToArray();

        if (dragons.Length == 0) {
            return null;
        }
        return dragons;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetSelectedRaisedPet")]
    public RaisedPetData[]? GetSelectedRaisedPet([FromForm] string apiToken, [FromForm] string userId, [FromForm] bool isActive) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null) {
            return null;
        }

        Dragon? dragon = viking.SelectedDragon;
        if (dragon is null) {
            return null;
        }

        return new RaisedPetData[] {
            GetRaisedPetDataFromDragon(dragon)
        };
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetImage")]
    public bool SetImage([FromForm] string apiToken, [FromForm] string ImageType, [FromForm] int ImageSlot, [FromForm] string contentXML, [FromForm] string imageFile) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null || viking.Dragons is null) {
            return false;
        }

        // TODO: the other properties of contentXML
        ImageData data = XmlUtil.DeserializeXml<ImageData>(contentXML);

        bool newImage = false;
        Image? image = viking.Images.FirstOrDefault(e => e.ImageType == ImageType && e.ImageSlot == ImageSlot);
        if (image is null) {
            image = new Image {
                ImageType = ImageType,
                ImageSlot = ImageSlot,
                Viking = viking,
            };
            newImage = true;
        }

        // Save the image in the db
        image.ImageData = imageFile;
        image.TemplateName = data.TemplateName;

        if (newImage) {
            ctx.Images.Add(image);
        } else {
            ctx.Images.Update(image);
        }
        ctx.SaveChanges();

        return true;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetImage")]
    public ImageData? GetImage([FromForm] string apiToken, [FromForm] string ImageType, [FromForm] int ImageSlot) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null || viking.Images is null) {
            return null;
        }

        return GetImageData(viking, ImageType, ImageSlot);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetImageByUserId")]
    public ImageData? GetImageByUserId([FromForm] string userId, [FromForm] string ImageType, [FromForm] int ImageSlot) {
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        if (viking is null || viking.Images is null) {
            return null;
        }

        // TODO: should we restrict images to only those the caller owns?

        return GetImageData(viking, ImageType, ImageSlot);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserUpcomingMissionState")]
    public IActionResult GetUserUpcomingMissionState([FromForm] string apiToken, [FromForm] string userId) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        UserMissionStateResult result = new UserMissionStateResult();

        if (session is null)
            return Ok(result);

        result.UserID = Guid.Parse(session.VikingId);
        return Ok(result); // TODO: placeholder, returns no upcoming missions
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserActiveMissionState")]
    public IActionResult GetUserActiveMissionState([FromForm] string apiToken, [FromForm] string userId) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        Mission tutorial = XmlUtil.DeserializeXml<Mission>(XmlUtil.ReadResourceXmlString("tutorialmission"));

        // Update the mission with completed tasks
        List<Model.TaskStatus> taskStatuses = ctx.TaskStatuses.Where(e => e.VikingId == userId && e.MissionId == tutorial.MissionID).ToList();
        foreach (var task in taskStatuses) {
            RuleItem? rule = tutorial.MissionRule.Criteria.RuleItems.Find(x => x.ID == task.Id);
            if (rule != null && task.Completed) rule.Complete = 1;

            Schema.Task? t = tutorial.Tasks.Find(x => x.TaskID == task.Id);
            if (t != null) {
                if (task.Completed) t.Completed = 1;
                t.Payload = task.Payload;
            }
        }

        result.Missions.Add(tutorial);

        if (session is null)
            return Ok("error");

        result.UserID = Guid.Parse(session.VikingId);
        return Ok(result); // TODO: placeholder, returns the tutorial
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserCompletedMissionState")]
    public IActionResult GetUserCompletedMissionState([FromForm] string apiToken, [FromForm] string userId) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        UserMissionStateResult result = new UserMissionStateResult();

        if (session is null)
            return Ok(result);

        result.UserID = Guid.Parse(session.VikingId);
        return Ok(result); // TODO: placeholder, returns no completed missions
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetTaskState")]
    public IActionResult SetTaskState([FromForm] string apiToken, [FromForm] string userId, [FromForm] int missionId, [FromForm] int taskId, [FromForm] bool completed, [FromForm] string xmlPayload) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null || session.VikingId != userId)
            return Ok(new SetTaskStateResult { Success = false, Status = SetTaskStateStatus.Unknown });

        Model.TaskStatus? status = ctx.TaskStatuses.FirstOrDefault(task => task.Id == taskId && task.MissionId == missionId && task.VikingId == userId);

        if (status is null) {
            status = new Model.TaskStatus {
                Id = taskId,
                MissionId = missionId,
                VikingId = userId,
                Payload = xmlPayload,
                Completed = completed
            };
            ctx.TaskStatuses.Add(status);
        } else {
            status.Payload = xmlPayload;
            status.Completed = completed;
        }
        ctx.SaveChanges();

        return Ok(new SetTaskStateResult { Success = true, Status = SetTaskStateStatus.TaskCanBeDone });
    }

    private RaisedPetData GetRaisedPetDataFromDragon (Dragon dragon) {
        RaisedPetData data = XmlUtil.DeserializeXml<RaisedPetData>(dragon.RaisedPetData);
        data.RaisedPetID = dragon.Id;
        data.EntityID = Guid.Parse(dragon.EntityId);
        data.IsSelected = dragon.SelectedViking is not null;
        return data;
    }

    private ImageData? GetImageData (Viking viking, String ImageType, int ImageSlot) {
        Image? image = viking.Images.FirstOrDefault(e => e.ImageType == ImageType && e.ImageSlot == ImageSlot);
        if (image is null) {
            return null;
        }

        string imageUrl = string.Format("{0}://{1}/RawImage/{2}/{3}/{4}", HttpContext.Request.Scheme, HttpContext.Request.Host, viking.Id, ImageType, ImageSlot);

        return new ImageData {
            ImageURL = imageUrl,
            TemplateName = image.TemplateName,
        };
    }
}
