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
    private MissionService missionService;
    private RoomService roomService;
    public ContentController(DBContext ctx, KeyValueService keyValueService, ItemService itemService, MissionService missionService, RoomService roomService) {
        this.ctx = ctx;
        this.keyValueService = keyValueService;
        this.itemService = itemService;
        this.missionService = missionService;
        this.roomService = roomService;
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
            if (item.Quantity == 0) continue; // Don't include an item that the viking doesn't have
            ItemData itemData = itemService.GetItem(item.ItemId);
            UserItemData uid = new UserItemData {
                UserInventoryID = item.Id,
                ItemID = itemData.ItemID,
                Quantity = item.Quantity,
                Uses = itemData.Uses,
                ModifiedDate = new DateTime(DateTime.Now.Ticks),
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

        // SetCommonInventory can remove any number of items from the inventory, this checks if it's possible
        foreach (var req in request) {
            if (req.Quantity >= 0) continue;
            int inventorySum = viking.Inventory.InventoryItems.Sum(e => {if (e.ItemId == req.ItemID) return e.Quantity; return 0;});
            if (inventorySum < -req.Quantity)
                return Ok(new CommonInventoryResponse { Success = false });
        }

        // Now that we know the request is valid, update the inventory
        foreach (var req in request) {
            if (req.ItemID == 0) continue; // Do not save a null item
            
            if (isFarmExpansion((int)req.ItemID)) {
                // if req.Quantity < 0 remove unique items
                for (int i=req.Quantity; i<0; ++i) {
                     InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID && e.Quantity>0);
                     item.Quantity--;
                }
                // if req.Quantity > 0 add unique items
                for (int i=0; i<req.Quantity; ++i) {
                    InventoryItem item = new InventoryItem { ItemId = (int)req.ItemID, Quantity = 1 };
                    viking.Inventory.InventoryItems.Add(item);
                    responseItems.Add(new CommonInventoryResponseItem {
                        CommonInventoryID = item.Id,
                        ItemID = item.ItemId,
                        Quantity = 0
                    });
                }
            } else {
                InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID);
                if (item is null) {
                    item = new InventoryItem { ItemId = (int)req.ItemID, Quantity = 0 };
                    viking.Inventory.InventoryItems.Add(item);
                }
                int updateQuantity = 0; // The game expects 0 if quantity got updated by just 1
                if (req.Quantity > 1)
                    updateQuantity = req.Quantity; // Otherwise it expects the quantity from the request
                item.Quantity += req.Quantity;
                ctx.SaveChanges(); // We need to get the ID of the newly created item
                if (req.Quantity > 0)
                    responseItems.Add(new CommonInventoryResponseItem {
                        CommonInventoryID = item.Id,
                        ItemID = item.ItemId,
                        Quantity = updateQuantity
                    });
            }
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
        if (itemId == 0) // For a null item, return an empty item
            return Ok(new ItemData());
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
        int imageSlot = (viking.Images.Select(i => i.ImageSlot).DefaultIfEmpty(-1).Max() + 1);
        raisedPetRequest.RaisedPetData.ImagePosition = imageSlot;
        // NOTE: Placing an egg into a hatchery slot calls CreatePet, but doesn't SetImage.
        // NOTE: We need to force create an image slot because hatching multiple eggs at once would create dragons with the same slot
        Image image = new Image {
            ImageType = "EggColor", // NOTE: The game doesn't seem to use anything other than EggColor.
            ImageSlot = imageSlot,
            Viking = viking,
        };
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
        ctx.Images.Add(image);
        ctx.SaveChanges();

        // TODO: handle CommonInventoryRequests here too

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

        dragon.RaisedPetData = XmlUtil.SerializeXml(UpdateDragon(dragon, raisedPetRequest.RaisedPetData));
        ctx.Update(dragon);
        ctx.SaveChanges();

        // TODO: handle CommonInventoryRequests here too

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

        return Ok(true); // RaisedPetSetResult.Success doesn't work, this does
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
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");
        
        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>() };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Upcoming))
            result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id));

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserActiveMissionState")]
    public IActionResult GetUserActiveMissionState([FromForm] string apiToken, [FromForm] string userId) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");
        
        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Active)) {
            Mission updatedMission = missionService.GetMissionWithProgress(mission.MissionId, viking.Id);
            if (mission.UserAccepted != null)
                updatedMission.Accepted = (bool)mission.UserAccepted;
            result.Missions.Add(updatedMission);
        }
        
        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserCompletedMissionState")]
    public IActionResult GetUserCompletedMissionState([FromForm] string apiToken, [FromForm] string userId) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");

        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Completed))
            result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id));

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/AcceptMission")]
    public IActionResult AcceptMission([FromForm] string userId, [FromForm] int missionId) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok(false);

        MissionState? missionState = viking.MissionStates.FirstOrDefault(x => x.MissionId == missionId);
        if (missionState is null || missionState.MissionStatus != MissionStatus.Upcoming)
            return Ok(false);

        missionState.MissionStatus = MissionStatus.Active;
        missionState.UserAccepted = true;
        ctx.SaveChanges();
        return Ok(true);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserMissionState")]
    public IActionResult GetUserMissionState([FromForm] string userId, [FromForm] string filter) {
        MissionRequestFilterV2 filterV2 = XmlUtil.DeserializeXml<MissionRequestFilterV2>(filter);
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");

        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        foreach (var m in filterV2.MissionPair)
            if (m.MissionID != null)
            result.Missions.Add(missionService.GetMissionWithProgress((int)m.MissionID, viking.Id));

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetTaskState")]
    public IActionResult SetTaskState([FromForm] string apiToken, [FromForm] string userId, [FromForm] int missionId, [FromForm] int taskId, [FromForm] bool completed, [FromForm] string xmlPayload) {
        Session? session = ctx.Sessions.FirstOrDefault(s => s.ApiToken == apiToken);
        if (session is null || session.VikingId != userId)
            return Ok(new SetTaskStateResult { Success = false, Status = SetTaskStateStatus.Unknown });

        List<MissionCompletedResult> results = missionService.UpdateTaskProgress(missionId, taskId, userId, completed, xmlPayload);

        SetTaskStateResult taskResult = new SetTaskStateResult {
            Success = true,
            Status = SetTaskStateStatus.TaskCanBeDone,
        };

        if (results.Count > 0)
            taskResult.MissionsCompleted = results.ToArray();

        return Ok(taskResult);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetBuddyList")]
    public IActionResult GetBuddyList() {
        // TODO: this is a placeholder
        return Ok(new BuddyList { Buddy = new Buddy[0] });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/PurchaseItems")]
    public IActionResult PurchaseItems([FromForm] string apiToken, [FromForm] string purchaseItemRequest) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null)
            return Ok();

        PurchaseStoreItemRequest request = XmlUtil.DeserializeXml<PurchaseStoreItemRequest>(purchaseItemRequest);
        CommonInventoryResponseItem[] items = new CommonInventoryResponseItem[request.Items.Length];
        for (int i = 0; i < request.Items.Length; i++) {
            InventoryItem? item = null;
            if (!isFarmExpansion(request.Items[i])) 
                item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == request.Items[i]);
            if (item is null) {
                item = new InventoryItem { ItemId = request.Items[i], Quantity = 0 };
                viking.Inventory.InventoryItems.Add(item);
            }
            item.Quantity++;
            ctx.SaveChanges();
            items[i] = new CommonInventoryResponseItem {
                CommonInventoryID = item.Id,
                ItemID = request.Items[i],
                Quantity = 0 // The quantity of purchased items is always 0 and the items are instead duplicated in both the request and the response
            };
        }

        CommonInventoryResponse response = new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = items,
            UserGameCurrency = new UserGameCurrency {
                UserID = Guid.Parse(viking.Id),
                UserGameCurrencyID = 1, // TODO: user's wallet ID?
                CashCurrency = 1000,
                GameCurrency = 1000,
            }
        };
        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/PurchaseItems")]
    public IActionResult PurchaseItemsV1([FromForm] string apiToken, [FromForm] string itemIDArrayXml) {
        Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
        if (viking is null)
            return Ok();

        int[] itemIdArr = XmlUtil.DeserializeXml<int[]>(itemIDArrayXml);
        CommonInventoryResponseItem[] items = new CommonInventoryResponseItem[itemIdArr.Length];
        for (int i = 0; i < itemIdArr.Length; i++) {
            InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == itemIdArr[i]);
            if (item is null) {
                item = new InventoryItem { ItemId = itemIdArr[i], Quantity = 0 };
                viking.Inventory.InventoryItems.Add(item);
            }
            item.Quantity++;
            ctx.SaveChanges();
            items[i] = new CommonInventoryResponseItem {
                CommonInventoryID = item.Id,
                ItemID = itemIdArr[i],
                Quantity = 0 // The quantity of purchased items is always 0 and the items are instead duplicated in both the request and the response
            };
        }

        CommonInventoryResponse response = new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = items,
            UserGameCurrency = new UserGameCurrency {
                UserID = Guid.Parse(viking.Id),
                UserGameCurrencyID = 1, // TODO: user's wallet ID?
                CashCurrency = 1000,
                GameCurrency = 1000,
            }
        };
        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserRoomItemPositions")]
    public IActionResult GetUserRoomItemPositions([FromForm] string apiToken, [FromForm] string roomID) {
        if (roomID is null)
            roomID = "";
        Room? room = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking?.Rooms.FirstOrDefault(x => x.RoomId == roomID);
        if (room is null)
            return Ok(new UserItemPositionList { UserItemPosition = new UserItemPosition[0] });
        return Ok(roomService.GetUserItemPositionList(room));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetUserRoomItemPositions")]
    public IActionResult SetUserRoomItemPositions([FromForm] string apiToken, [FromForm] string createXml, [FromForm] string updateXml, [FromForm] string removeXml, [FromForm] string roomID) {
        if (roomID is null)
            roomID = "";
        Room? room = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking?.Rooms.FirstOrDefault(x => x.RoomId == roomID);
        if (room is null) {
            room = new Room {
                RoomId = roomID,
                Items = new List<RoomItem>()
            };
            ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking?.Rooms.Add(room);
            ctx.SaveChanges();
        }

        UserItemPositionSetRequest[] createItems = XmlUtil.DeserializeXml<UserItemPositionSetRequest[]>(createXml);
        UserItemPositionSetRequest[] updateItems = XmlUtil.DeserializeXml<UserItemPositionSetRequest[]>(updateXml);
        int[] deleteItems = XmlUtil.DeserializeXml<int[]>(removeXml);

        Tuple<int[], UserItemState[]> createData = roomService.CreateItems(createItems, room);
        UserItemState[] state = roomService.UpdateItems(updateItems, room);
        roomService.DeleteItems(deleteItems, room);

        UserItemPositionSetResponse response = new UserItemPositionSetResponse {
            Success = true,
            CreatedUserItemPositionIDs = createData.Item1,
            UserItemStates = createData.Item2,
            Result = ItemPositionValidationResult.Valid
        };

        if (state.Length > 0)
            response.UserItemStates = state;

        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserRoomList")]
    public IActionResult GetUserRoomList([FromForm] string request) {
        // TODO: Categories are not supported
        UserRoomGetRequest userRoomRequest = XmlUtil.DeserializeXml<UserRoomGetRequest>(request);
        ICollection<Room>? rooms = ctx.Vikings.FirstOrDefault(x => x.Id == userRoomRequest.UserID.ToString())?.Rooms;
        UserRoomResponse response = new UserRoomResponse { UserRoomList = new List<UserRoom>() };
        if (rooms is null)
            return Ok(response);
        foreach (var room in rooms) {
            if (room.RoomId == "MyRoomINT" || room.RoomId == "StaticFarmItems") continue;

            int itemID = 0;
            if (room.RoomId != "") {
                // farm expansion room: RoomId is Id for expansion item
                if (Int32.TryParse(room.RoomId, out int inventoryItemId)) {
                    InventoryItem? item = room.Viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == inventoryItemId);
                    if (item != null) {
                        itemID = item.ItemId;
                    }
                }
            }

            UserRoom ur = new UserRoom {
                RoomID = room.RoomId,
                CategoryID = 541, // Placeholder
                CreativePoints = 0, // Placeholder
                ItemID = itemID,
                Name = room.Name
            };
            response.UserRoomList.Add(ur);
        }
        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetUserRoom")]
    public IActionResult SetUserRoom([FromForm] string apiToken, [FromForm] string request) {
        UserRoom roomRequest = XmlUtil.DeserializeXml<UserRoom>(request);
        Room? room = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking?.Rooms.FirstOrDefault(x => x.RoomId == roomRequest.RoomID);
        if (room is null) {
            // setting farm room name can be done before call SetUserRoomItemPositions
            room = new Room {
                RoomId = roomRequest.RoomID,
                Name = roomRequest.Name
            };
            ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking?.Rooms.Add(room);
        } else {
            room.Name = roomRequest.Name;
        }
        ctx.SaveChanges();
        return Ok(new UserRoomSetResponse {
            Success = true,
            StatusCode = UserRoomValidationResult.Valid
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserActivityByUserID")]
    public IActionResult GetUserActivityByUserID() {
        return Ok(new ArrayOfUserActivity { UserActivity = new UserActivity[0] });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetNextItemState")]
    public IActionResult SetNextItemState([FromForm] string setNextItemStateRequest) {
        SetNextItemStateRequest request = XmlUtil.DeserializeXml<SetNextItemStateRequest>(setNextItemStateRequest);
        RoomItem? item = ctx.RoomItems.FirstOrDefault(x => x.Id == request.UserItemPositionID);
        if (item is null)
            return Ok();

        // NOTE: The game sets OverrideStateCriteria only if a speedup is used
        return Ok(roomService.NextItemState(item, request.OverrideStateCriteria));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserGameCurrency")]
    public IActionResult GetUserGameCurrency([FromForm] string userId) {
        // TODO: This is a placeholder
        return Ok(new UserGameCurrency {
            CashCurrency = 1000,
            GameCurrency = 1000,
            UserGameCurrencyID = 0,
            UserID = Guid.Parse(userId)
        });
    }

    private RaisedPetData GetRaisedPetDataFromDragon (Dragon dragon) {
        RaisedPetData data = XmlUtil.DeserializeXml<RaisedPetData>(dragon.RaisedPetData);
        data.RaisedPetID = dragon.Id;
        data.EntityID = Guid.Parse(dragon.EntityId);
        data.IsSelected = dragon.SelectedViking is not null;
        return data;
    }

    // Needs to merge newDragonData into dragonData
    private RaisedPetData UpdateDragon (Dragon dragon, RaisedPetData newDragonData) {
        RaisedPetData dragonData = XmlUtil.DeserializeXml<RaisedPetData>(dragon.RaisedPetData);

        // The simple attributes
        dragonData.IsPetCreated = newDragonData.IsPetCreated;
        if (newDragonData.ValidationMessage is not null) dragonData.ValidationMessage = newDragonData.ValidationMessage;
        if (newDragonData.EntityID is not null) dragonData.EntityID = newDragonData.EntityID;
        if (newDragonData.Name is not null) dragonData.Name = newDragonData.Name;
        dragonData.PetTypeID = newDragonData.PetTypeID;
        if (newDragonData.GrowthState is not null) dragonData.GrowthState = newDragonData.GrowthState;
        if (newDragonData.ImagePosition is not null) dragonData.ImagePosition = newDragonData.ImagePosition;
        if (newDragonData.Geometry is not null) dragonData.Geometry = newDragonData.Geometry;
        if (newDragonData.Texture is not null) dragonData.Texture = newDragonData.Texture;
        dragonData.Gender = newDragonData.Gender;
        if (newDragonData.Accessories is not null) dragonData.Accessories = newDragonData.Accessories;
        if (newDragonData.Colors is not null) dragonData.Colors = newDragonData.Colors;
        if (newDragonData.Skills is not null) dragonData.Skills = newDragonData.Skills;
        if (newDragonData.States is not null) dragonData.States = newDragonData.States;

        dragonData.IsSelected = newDragonData.IsSelected;
        dragonData.IsReleased = newDragonData.IsReleased;
        dragonData.UpdateDate = newDragonData.UpdateDate;

        // Attributes is special - the entire list isn't re-sent, so we need to manually update each
        if (dragonData.Attributes is null) dragonData.Attributes = new RaisedPetAttribute[] { };
        List<RaisedPetAttribute> attribs = dragonData.Attributes.ToList();
        if (newDragonData.Attributes is not null) {
            foreach (RaisedPetAttribute newAttribute in newDragonData.Attributes) {
                RaisedPetAttribute? attribute = attribs.Find(a => a.Key == newAttribute.Key);
                if (attribute is null) {
                    attribs.Add(newAttribute);
                }
                else {
                    attribute.Value = newAttribute.Value;
                    attribute.Type = newAttribute.Type;
                }
            }
            dragonData.Attributes = attribs.ToArray();
        }

        return dragonData;
    }

    private ImageData? GetImageData (Viking viking, String ImageType, int ImageSlot) {
        Image? image = viking.Images.FirstOrDefault(e => e.ImageType == ImageType && e.ImageSlot == ImageSlot);
        if (image is null) {
            return null;
        }

        string imageUrl = string.Format("{0}://{1}/RawImage/{2}/{3}/{4}.jpg", HttpContext.Request.Scheme, HttpContext.Request.Host, viking.Id, ImageType, ImageSlot);

        return new ImageData {
            ImageURL = imageUrl,
            TemplateName = image.TemplateName,
        };
    }

    private bool isFarmExpansion(int itemId) {
        ItemData? itemData = itemService.GetItem(itemId);
        if (itemData != null && itemData.Category != null) {
            foreach (ItemDataCategory itemCategory in itemData.Category) {
                if (itemCategory.CategoryId == 541) { // if item is farm expansion
                    return true;
                }
            }
        }
        return false;
    }
}
