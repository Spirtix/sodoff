﻿using Microsoft.AspNetCore.Mvc;
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
    private AchievementService achievementService;
    private InventoryService inventoryService;
    private Random random = new Random();
    public ContentController(DBContext ctx, KeyValueService keyValueService, ItemService itemService, MissionService missionService, RoomService roomService, AchievementService achievementService, InventoryService inventoryService) {
        this.ctx = ctx;
        this.keyValueService = keyValueService;
        this.itemService = itemService;
        this.missionService = missionService;
        this.roomService = roomService;
        this.achievementService = achievementService;
        this.inventoryService = inventoryService;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetDefaultNameSuggestion")]
    [VikingSession(Mode=VikingSession.Modes.VIKING_OR_USER, UseLock=false)]
    public IActionResult GetDefaultNameSuggestion(User? user, Viking? viking) {
        string[] adjs = { //Adjectives used to generate suggested names
            "Adventurous", "Active", "Alert", "Attentive",
            "Beautiful", "Berkian", "Berserker", "Bold", "Brave",
            "Caring", "Cautious", "Creative", "Curious",
            "Dangerous", "Daring", "Defender",
            "Elder", "Exceptional", "Exquisite", 
            "Fearless", "Fighter", "Friendly",
            "Gentle", "Grateful", "Great",
            "Happy", "Honorable", "Hunter",
            "Insightful", "Intelligent",
            "Jolly", "Joyful", "Joyous",
            "Kind", "Kindly",
            "Legendary", "Lovable", "Lovely",
            "Marvelous", "Magnificent",
            "Noble", "Nifty", "Neat",
            "Outcast", "Outgoing", "Organized",
            "Planner", "Playful", "Pretty",
            "Quick", "Quiet",
            "Racer", "Random", "Resilient",
            "Scientist", "Seafarer", "Smart", "Sweet",
            "Thinker", "Thoughtful",
            "Unafraid", "Unique",
            "Valiant", "Valorous", "Victor", "Victorious", "Viking",
            "Winner", "Warrior", "Wise",
            "Young", "Youthful",
            "Zealous", "Zealot"
        };

        if (user is null)
            user = viking.User;
        string uname = user.Username;

        Random choice = new Random(); //Randomizer for selecting random adjectives
        
        List<string> suggestions = new();
        AddSuggestion(choice, uname, suggestions);

        for (int i = 0; i < 5; i++)
            AddSuggestion(choice, GetNameSuggestion(choice, uname, adjs), suggestions);

        return Ok(new DisplayNameUniqueResponse {
            Suggestions = new SuggestionResult {
                Suggestion = suggestions.ToArray()
            }
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/ValidateName")]
    public IActionResult ValidateName([FromForm] string nameValidationRequest) {
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
    [Route("ContentWebService.asmx/GetKeyValuePairByUserID")]
    [VikingSession(Mode=VikingSession.Modes.VIKING_OR_USER, UseLock=false)]
    public Schema.PairData? GetKeyValuePairByUserID(User? user, Viking? viking, [FromForm] int pairId, [FromForm] string? userId) {
        Model.PairData? pair = keyValueService.GetPairData(user, viking, userId, pairId);

        return keyValueService.ModelToSchema(pair);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetKeyValuePair")]
    [Route("ContentWebService.asmx/SetKeyValuePairByUserID")]
    [VikingSession(Mode=VikingSession.Modes.VIKING_OR_USER)]
    public IActionResult SetKeyValuePairByUserID(User? user, Viking? viking, [FromForm] int pairId, [FromForm] string contentXML, [FromForm] string? userId) {
        Schema.PairData schemaData = XmlUtil.DeserializeXml<Schema.PairData>(contentXML);

        bool result = keyValueService.SetPairData(user, viking, userId, pairId, schemaData);

        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetCommonInventory")]
    [VikingSession(Mode=VikingSession.Modes.VIKING_OR_USER, UseLock=false)]
    public IActionResult GetCommonInventory(User? user, Viking? viking) {
        if (viking != null) {
            return Ok( inventoryService.GetCommonInventoryData(viking) );
        } else {
            // TODO: placeholder - return 8 viking slot items
            return Ok(new CommonInventoryData {
                UserID = Guid.Parse(user.Id),
                Item = new UserItemData[] {
                    new UserItemData {
                        UserInventoryID = 0,
                        ItemID = 7971,
                        Quantity = 8,
                        Uses = -1,
                        ModifiedDate = new DateTime(DateTime.Now.Ticks),
                        Item = itemService.GetItem(7971)
                    }
                }
            });
        }
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetCommonInventory")]
    [VikingSession(UseLock=false)]
    public IActionResult GetCommonInventoryV2(Viking viking) {
        return Ok(inventoryService.GetCommonInventoryData(viking));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetCommonInventory")]
    [VikingSession]
    public IActionResult SetCommonInventory(Viking viking, [FromForm] string commonInventoryRequestXml) {
        CommonInventoryRequest[] request = XmlUtil.DeserializeXml<CommonInventoryRequest[]>(commonInventoryRequestXml);
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

            if (inventoryService.ItemNeedUniqueInventorySlot((int)req.ItemID)) {
                // if req.Quantity < 0 remove unique items
                for (int i=req.Quantity; i<0; ++i) {
                     InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID && e.Quantity>0);
                     item.Quantity--;
                }
                // if req.Quantity > 0 add unique items
                for (int i=0; i<req.Quantity; ++i) {
                    responseItems.Add(
                        inventoryService.AddItemToInventoryAndGetResponse(viking, (int)req.ItemID!, 1)
                    );
                }
            } else {
                var responseItem = inventoryService.AddItemToInventoryAndGetResponse(viking, (int)req.ItemID!, req.Quantity);
                if (req.Quantity > 0) {
                    responseItems.Add(responseItem);
                }
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
    [Route("ContentWebService.asmx/UseInventory")]
    [VikingSession]
    public IActionResult UseInventory(Viking viking, [FromForm] int userInventoryId, [FromForm] int numberOfUses) {
        InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == userInventoryId);
        if (item is null)
            return Ok(false);
        if (item.Quantity < numberOfUses)
            return Ok(false);
        
        item.Quantity -= numberOfUses;
        ctx.SaveChanges();
        return Ok(true);
    }
    

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetAuthoritativeTime")]
    public IActionResult GetAuthoritativeTime() {
        return Ok(new DateTime(DateTime.Now.Ticks));
    }

    private int GetAvatarVersion(AvatarData avatarData) {
        foreach (AvatarDataPart part in avatarData.Part) {
            if (part.PartType == "Version") {
                return (int)part.Offsets[0].X * 100 + (int)part.Offsets[0].Y * 10 + (int)part.Offsets[0].Z;
            }
        }
        return 0;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetAvatar")]
    [VikingSession]
    public IActionResult SetAvatar(Viking viking, [FromForm] string contentXML) {
        if (viking.AvatarSerialized != null) {
            AvatarData dbAvatarData = XmlUtil.DeserializeXml<AvatarData>(viking.AvatarSerialized);
            AvatarData reqAvatarData = XmlUtil.DeserializeXml<AvatarData>(contentXML);

            int dbAvatarVersion = GetAvatarVersion(dbAvatarData);
            int reqAvatarVersion = GetAvatarVersion(reqAvatarData);

            if (dbAvatarVersion > reqAvatarVersion) {
                // do not allow override newer version avatar data by older version
                return Ok(new SetAvatarResult {
                    Success = false,
                });
            }
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
    [VikingSession]
    public IActionResult CreatePet(Viking viking, [FromForm] string request) {
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

        if (raisedPetRequest.CommonInventoryRequests is not null) {
            foreach (var req in raisedPetRequest.CommonInventoryRequests) {
                InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID);
                
                //Does the item exist in the user's inventory?
                if (item is null) continue; //If not, skip it.
                
                if (item.Quantity + req.Quantity >= 0 ) { //If so, can we update it appropriately?
                    //We can.  Do so.
                    item.Quantity += req.Quantity; //Note that we use += here because req.Quantity is negative.
                    ctx.SaveChanges();
                }
            }
        }

        return Ok(new CreatePetResponse {
            RaisedPetData = GetRaisedPetDataFromDragon(dragon)
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetRaisedPet")] // used by Magic & Mythies
    [VikingSession]
    public IActionResult SetRaisedPetv2(Viking viking, [FromForm] string raisedPetData) {
        RaisedPetData petData = XmlUtil.DeserializeXml<RaisedPetData>(raisedPetData);

        // Find the dragon
        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.Id == petData.RaisedPetID);
        if (dragon is null) {
            return Ok(new SetRaisedPetResponse {
                RaisedPetSetResult = RaisedPetSetResult.Invalid
            });
        }

        dragon.RaisedPetData = XmlUtil.SerializeXml(UpdateDragon(dragon, petData));
        ctx.Update(dragon);
        ctx.SaveChanges();

        return Ok(new SetRaisedPetResponse {
            RaisedPetSetResult = RaisedPetSetResult.Success
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/ContentWebService.asmx/SetRaisedPet")]
    [VikingSession]
    public IActionResult SetRaisedPet(Viking viking, [FromForm] string request, [FromForm] bool? import) {
         RaisedPetRequest raisedPetRequest = XmlUtil.DeserializeXml<RaisedPetRequest>(request);

        // Find the dragon
        Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.Id == raisedPetRequest.RaisedPetData.RaisedPetID);
        if (dragon is null) {
            return Ok(new SetRaisedPetResponse {
                RaisedPetSetResult = RaisedPetSetResult.Invalid
            });
        }

        dragon.RaisedPetData = XmlUtil.SerializeXml(UpdateDragon(dragon, raisedPetRequest.RaisedPetData, import ?? false));
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
    [VikingSession]
    public IActionResult SetSelectedPet(Viking viking, [FromForm] int raisedPetID) {
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
    public RaisedPetData[]? GetAllActivePetsByuserId([FromForm] string userId, [FromForm] bool active) {
        // NOTE: this is public info (for mmo) - no session check
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        if (viking is null)
            return null;

        RaisedPetData[] dragons = ctx.Dragons
            .Where(d => d.VikingId == userId && d.RaisedPetData != null)
            .Select(d => GetRaisedPetDataFromDragon(d, viking.SelectedDragonId))
            .ToArray();

        if (dragons.Length == 0) {
            return null;
        }
        return dragons;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUnselectedPetByTypes")] // used by old SoD (e.g. 1.13)
    [VikingSession(UseLock=false)]
    public RaisedPetData[]? GetUnselectedPetByTypes(Viking viking, [FromForm] string petTypeIDs, [FromForm] bool active) {
        RaisedPetData[] dragons = viking.Dragons
            .Where(d => d.RaisedPetData is not null)
            .Select(d => GetRaisedPetDataFromDragon(d, viking.SelectedDragonId))
            .ToArray();

        if (dragons.Length == 0) {
            return null;
        }

        List<RaisedPetData> filteredDragons = new List<RaisedPetData>();
        int[] petTypeIDsInt = Array.ConvertAll(petTypeIDs.Split(','), s => int.Parse(s));
        foreach (RaisedPetData dragon in dragons) {
            if (petTypeIDsInt.Contains(dragon.PetTypeID)) {
                filteredDragons.Add(dragon);
            }
        }

        if (filteredDragons.Count == 0) {
            return null;
        }

        return filteredDragons.ToArray();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetSelectedRaisedPet")]
    [VikingSession(UseLock=false)]
    public RaisedPetData[]? GetSelectedRaisedPet(Viking viking, [FromForm] string userId, [FromForm] bool isActive) {
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
    [VikingSession]
    public bool SetImage(Viking viking, [FromForm] string ImageType, [FromForm] int ImageSlot, [FromForm] string contentXML, [FromForm] string imageFile) {
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
    [VikingSession(UseLock=false)]
    public ImageData? GetImage(Viking viking, [FromForm] string ImageType, [FromForm] int ImageSlot) {
        return GetImageData(viking, ImageType, ImageSlot);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetImageByUserId")]
    public ImageData? GetImageByUserId([FromForm] string userId, [FromForm] string ImageType, [FromForm] int ImageSlot) {
        // NOTE: this is public info (for mmo) - no session check
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);
        if (viking is null || viking.Images is null) {
            return null;
        }

        return GetImageData(viking, ImageType, ImageSlot);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserUpcomingMissionState")]
    public IActionResult GetUserUpcomingMissionState([FromForm] string apiToken, [FromForm] string userId, [FromForm] string apiKey) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");
        
        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>() };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Upcoming))
            result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id, apiKey));

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserActiveMissionState")]
    public IActionResult GetUserActiveMissionState([FromForm] string apiToken, [FromForm] string userId, [FromForm] string apiKey) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");
        
        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Active)) {
            Mission updatedMission = missionService.GetMissionWithProgress(mission.MissionId, viking.Id, apiKey);
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
    public IActionResult GetUserCompletedMissionState([FromForm] string apiToken, [FromForm] string userId, [FromForm] string apiKey) {
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");

        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Completed))
            result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id, apiKey));

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/AcceptMission")]
    [VikingSession]
    public IActionResult AcceptMission(Viking viking, [FromForm] string userId, [FromForm] int missionId) {
        if (viking.Id != userId)
            return Unauthorized("Can't accept not owned mission");

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
    [Route("ContentWebService.asmx/GetUserMissionState")] // used by Magic & Mythies
    public IActionResult GetUserMissionStatev1([FromForm] string userId, [FromForm] string filter) {
        // TODO: This is a placeholder
        return Ok(new UserMissionStateResult { Missions = new List<Mission>()  });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetUserMissionState")]
    //[VikingSession(UseLock=false)]
    public IActionResult GetUserMissionState([FromForm] string userId, [FromForm] string filter, [FromForm] string apiKey) {
        MissionRequestFilterV2 filterV2 = XmlUtil.DeserializeXml<MissionRequestFilterV2>(filter);
        Viking? viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId);
        if (viking is null)
            return Ok("error");

        UserMissionStateResult result = new UserMissionStateResult { Missions = new List<Mission>()  };
        if (filterV2.MissionPair.Count > 0) {
            foreach (var m in filterV2.MissionPair)
                if (m.MissionID != null)
                    result.Missions.Add(missionService.GetMissionWithProgress((int)m.MissionID, viking.Id, apiKey));
        // TODO: probably should also check for msiion based on filterV2.ProductGroupID vs mission.GroupID
        } else {
            if (filterV2.GetCompletedMission ?? false) {
                foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus == MissionStatus.Completed))
                    result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id, apiKey));
            } else {
                foreach (var mission in viking.MissionStates.Where(x => x.MissionStatus != MissionStatus.Completed))
                    result.Missions.Add(missionService.GetMissionWithProgress(mission.MissionId, viking.Id, apiKey));
            }
        }

        result.UserID = Guid.Parse(viking.Id);
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SetTaskState")]
    [VikingSession]
    public IActionResult SetTaskState(Viking viking, [FromForm] string userId, [FromForm] int missionId, [FromForm] int taskId, [FromForm] bool completed, [FromForm] string xmlPayload, [FromForm] string apiKey) {
        if (viking.Id != userId)
            return Unauthorized("Can't set not owned task");

        List<MissionCompletedResult> results = missionService.UpdateTaskProgress(missionId, taskId, userId, completed, xmlPayload, apiKey);

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
    [Route("/ContentWebService.asmx/RedeemMysteryBoxItems")]
    [VikingSession]
    public IActionResult RedeemMysteryBoxItems(Viking viking, [FromForm] string request) {
        var req = XmlUtil.DeserializeXml<RedeemRequest>(request);

        // get and reduce quantity of box item
        InventoryItem? invItem = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == req.ItemID);
        if (invItem is null || invItem.Quantity < 1) {
            return Ok(new CommonInventoryResponse{ Success = false });
        }
        --invItem.Quantity;

        // get real item id (from box) add it to inventory
        itemService.OpenBox(req.ItemID, out int newItemId, out int quantity);
        ItemData newItem = itemService.GetItem(newItemId);
        CommonInventoryResponseItem newInvItem = inventoryService.AddItemToInventoryAndGetResponse(viking, newItem.ItemID, quantity);

        // prepare list of possible rewards for response
        List<ItemData> prizeItems = new List<ItemData>();
        prizeItems.Add(newItem);
        foreach (var reward in itemService.GetItem(req.ItemID).Relationship.Where(e => e.Type == "Prize")) {
            if (prizeItems.Count >= req.RedeemItemFetchCount)
                break;
            prizeItems.Add(itemService.GetItem(reward.ItemId));
        }

        return Ok(new CommonInventoryResponse{
            Success = true,
            CommonInventoryIDs = new CommonInventoryResponseItem[]{ newInvItem },
            PrizeItems = new List<PrizeItemResponse>{ new PrizeItemResponse{
                ItemID = req.ItemID,
                PrizeItemID = newItem.ItemID,
                MysteryPrizeItems = prizeItems,
            }}
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/PurchaseItems")]
    [VikingSession]
    public IActionResult PurchaseItems(Viking viking, [FromForm] string purchaseItemRequest) {
        PurchaseStoreItemRequest request = XmlUtil.DeserializeXml<PurchaseStoreItemRequest>(purchaseItemRequest);
        List<CommonInventoryResponseItem> items = new List<CommonInventoryResponseItem>();
        for (int i = 0; i < request.Items.Length; i++) {
            int itemId = request.Items[i];
            if (request.AddMysteryBoxToInventory) {
                // force add boxes as item (without "opening")
                items.Add(inventoryService.AddItemToInventoryAndGetResponse(viking, itemId, 1));
            } else if (itemService.IsBundleItem(itemId)) {
                // open and add bundle
                ItemData bundleItem = itemService.GetItem(itemId);
                foreach (var reward in bundleItem.Relationship.Where(e => e.Type == "Bundle")) {
                    int quantity = itemService.GetItemQuantity(reward);
                    for (int j=0; j<quantity; ++j)
                        items.Add(inventoryService.AddItemToInventoryAndGetResponse(viking, reward.ItemId, 1));
                }
            } else {
                // check for mystery box ... open if need
                itemService.CheckAndOpenBox(itemId, out itemId, out int quantity);
                for (int j=0; j<quantity; ++j) {
                    items.Add(inventoryService.AddItemToInventoryAndGetResponse(viking, itemId, 1));
                }
            }
            // NOTE: The quantity of purchased items is always 0 and the items are instead duplicated in both the request and the response.
            //       Call AddItemToInventoryAndGetResponse with Quantity == 1 we get response with quantity == 0.
        }

        CommonInventoryResponse response = new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = items.ToArray(),
            UserGameCurrency = achievementService.GetUserCurrency(viking)
        };
        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/PurchaseItems")]
    [VikingSession]
    public IActionResult PurchaseItemsV1(Viking viking, [FromForm] string itemIDArrayXml) {
        int[] itemIdArr = XmlUtil.DeserializeXml<int[]>(itemIDArrayXml);
        List<CommonInventoryResponseItem> items = new List<CommonInventoryResponseItem>();
        for (int i = 0; i < itemIdArr.Length; i++) {
            itemService.CheckAndOpenBox(itemIdArr[i], out int itemId, out int quantity);
            for (int j=0; j<quantity; ++j) {
                items.Add(inventoryService.AddItemToInventoryAndGetResponse(viking, itemId, 1));
                // NOTE: The quantity of purchased items is always 0 and the items are instead duplicated in both the request and the response.
                //       Call AddItemToInventoryAndGetResponse with Quantity == 1 we get response with quantity == 0.
            }
        }

        CommonInventoryResponse response = new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = items.ToArray(),
            UserGameCurrency = achievementService.GetUserCurrency(viking)
        };
        return Ok(response);
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserRoomItemPositions")]
    public IActionResult GetUserRoomItemPositions([FromForm] string userId, [FromForm] string roomID) {
        // NOTE: this is public info (for mmo) - no session check
        Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Id == userId);

        if (roomID is null)
            roomID = "";
        Room? room = viking?.Rooms.FirstOrDefault(x => x.RoomId == roomID);
        if (room is null)
            return Ok(new UserItemPositionList { UserItemPosition = new UserItemPosition[0] });

        return Ok(roomService.GetUserItemPositionList(room));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetUserRoomItemPositions")]
    [VikingSession]
    public IActionResult SetUserRoomItemPositions(Viking viking, [FromForm] string createXml, [FromForm] string updateXml, [FromForm] string removeXml, [FromForm] string roomID) {
        if (roomID is null)
            roomID = "";
        Room? room = viking.Rooms.FirstOrDefault(x => x.RoomId == roomID);
        if (room is null) {
            room = new Room {
                RoomId = roomID,
                Items = new List<RoomItem>()
            };
            viking.Rooms.Add(room);
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
        // NOTE: this is public info (for mmo) - no session check
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
    [VikingSession]
    public IActionResult SetUserRoom(Viking viking, [FromForm] string request) {
        UserRoom roomRequest = XmlUtil.DeserializeXml<UserRoom>(request);
        Room? room = viking.Rooms.FirstOrDefault(x => x.RoomId == roomRequest.RoomID);
        if (room is null) {
            // setting farm room name can be done before call SetUserRoomItemPositions
            room = new Room {
                RoomId = roomRequest.RoomID,
                Name = roomRequest.Name
            };
            viking.Rooms.Add(room);
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
        // TODO: This is a placeholder
        return Ok(new ArrayOfUserActivity { UserActivity = new UserActivity[0] });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/SetNextItemState")]
    [VikingSession]
    public IActionResult SetNextItemState(Viking viking, [FromForm] string setNextItemStateRequest) {
        SetNextItemStateRequest request = XmlUtil.DeserializeXml<SetNextItemStateRequest>(setNextItemStateRequest);
        RoomItem? item = ctx.RoomItems.FirstOrDefault(x => x.Id == request.UserItemPositionID);
        if (item is null)
            return Ok();

        if (item.Room.Viking != viking)
            return Unauthorized("Can't set state not owned item");

        // NOTE: The game sets OverrideStateCriteria only if a speedup is used
        return Ok(roomService.NextItemState(item, request.OverrideStateCriteria));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/GetGameData")]
    public IActionResult GetGameData() {
        // TODO: This is a placeholder
        return Ok(new GetGameDataResponse());
    }
    
    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/GetUserGameCurrency")]
    [VikingSession]
    public IActionResult GetUserGameCurrency(Viking viking) {
        // TODO: This is a placeholder
        return Ok(achievementService.GetUserCurrency(viking));
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/RerollUserItem")]
    [VikingSession]
    public IActionResult RerollUserItem(Viking viking, [FromForm] string request) {
        RollUserItemRequest req = XmlUtil.DeserializeXml<RollUserItemRequest>(request);

        // get item
        InventoryItem? invItem = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == req.UserInventoryID);
        if (invItem is null)
            return Ok(new RollUserItemResponse { Status = Status.ItemNotFound });
        
        // get item data and stats
        ItemData itemData = itemService.GetItem(invItem.ItemId);
        ItemStatsMap itemStatsMap;
        if (invItem.StatsSerialized != null) {
            itemStatsMap = XmlUtil.DeserializeXml<ItemStatsMap>(invItem.StatsSerialized);
        } else {
            itemStatsMap = itemData.ItemStatsMap;
        }

        List<ItemStat> newStats;
        Status status = Status.Failure;
        
        // update stats
        if (req.ItemStatNames != null) {
            // reroll only one stat (from req.ItemStatNames)
            newStats = new List<ItemStat>();
            foreach (string name in req.ItemStatNames) {
                ItemStat itemStat = itemStatsMap.ItemStats.FirstOrDefault(e => e.Name == name);

                // draw new stats
                StatRangeMap rangeMap = itemData.PossibleStatsMap.Stats.FirstOrDefault(e => e.ItemStatsID == itemStat.ItemStatID).ItemStatsRangeMaps.FirstOrDefault(e => e.ItemTierID == (int)(itemStatsMap.ItemTier));
                int newVal = random.Next(rangeMap.StartRange, rangeMap.EndRange+1);

                // check draw results
                Int32.TryParse(itemStat.Value, out int oldVal);
                if (newVal > oldVal) {
                    itemStat.Value = newVal.ToString();
                    newStats.Add(itemStat);
                    status = Status.Success;
                }
            }
            // get shards
            inventoryService.AddItemToInventory(viking, InventoryService.Shards, -((int)(itemData.ItemRarity) + (int)(itemStatsMap.ItemTier) - 1));
        } else {
            // reroll full item
            newStats = itemService.CreateItemStats(itemData.PossibleStatsMap, (int)itemData.ItemRarity, (int)itemStatsMap.ItemTier);
            itemStatsMap.ItemStats = newStats.ToArray();
            status = Status.Success;
            // get shards
            int price = 0;
            switch (itemData.ItemRarity) {
                case ItemRarity.Common:
                    price = 5;
                    break;
                case ItemRarity.Rare:
                    price = 7;
                    break;
                case ItemRarity.Epic:
                    price = 10;
                    break;
                case ItemRarity.Legendary:
                    price = 20;
                    break;
            }
            switch (itemStatsMap.ItemTier) {
                case ItemTier.Tier2:
                    price = (int)Math.Floor(price * 1.5);
                    break;
                case ItemTier.Tier3:
                case ItemTier.Tier4:
                    price = price * 2;
                    break;
            }
            inventoryService.AddItemToInventory(viking, InventoryService.Shards, -price);
        }
 
        // save
        invItem.StatsSerialized = XmlUtil.SerializeXml(itemStatsMap);
        ctx.SaveChanges();

        // return results
        return Ok(new RollUserItemResponse {
            Status = status,
            ItemStats = newStats.ToArray() // we need return only updated stats, so can't `= itemStatsMap.ItemStats`
        });
    }
    
    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/FuseItems")]
    [VikingSession]
    public IActionResult FuseItems(Viking viking, [FromForm] string fuseItemsRequest) {
        FuseItemsRequest req = XmlUtil.DeserializeXml<FuseItemsRequest>(fuseItemsRequest);

        ItemData blueprintItem;
        try {
            if (req.BluePrintInventoryID != null) {
                blueprintItem = itemService.GetItem(
                    viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == req.BluePrintInventoryID).ItemId
                );
            } else {
                blueprintItem = itemService.GetItem(req.BluePrintItemID ?? -1);
            }
        } catch(System.Collections.Generic.KeyNotFoundException) {
            return Ok(new FuseItemsResponse { Status = Status.BluePrintItemNotFound });
        }

        // TODO: check for blueprintItem.BluePrint.Deductibles and blueprintItem.BluePrint.Ingredients
        
        // remove items from DeductibleItemInventoryMaps and BluePrintFuseItemMaps
        foreach (var item in req.DeductibleItemInventoryMaps) {
            InventoryItem? invItem = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == item.UserInventoryID);
            invItem.Quantity -= item.Quantity;
        }
        foreach (var item in req.BluePrintFuseItemMaps) {
            InventoryItem? invItem = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == item.UserInventoryID);
            viking.Inventory.InventoryItems.Remove(invItem);
        }
        
        var resItemList = new List<InventoryItemStatsMap>();
        foreach (BluePrintSpecification output in blueprintItem.BluePrint.Outputs) {
            if (output.ItemID is null)
                continue;

            itemService.CheckAndOpenBox((int)(output.ItemID), out int newItemId, out int quantity);
            for (int i=0; i<quantity; ++i) {
                resItemList.Add(
                    inventoryService.AddBattleItemToInventory(viking, newItemId, (int)output.Tier)
                );
            }
        }
        
        // NOTE: saved inside AddBattleItemToInventory
         
        // return response with new item info
        return Ok(new FuseItemsResponse {
            Status = Status.Success,
            InventoryItemStatsMaps = resItemList
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/SellItems")]
    [VikingSession]
    public IActionResult SellItems(Viking viking, [FromForm] string sellItemsRequest) {
        int shard = 0;
        int gold = 0;
        SellItemsRequest req = XmlUtil.DeserializeXml<SellItemsRequest>(sellItemsRequest);
        foreach (var invItemID in req.UserInventoryCommonIDs) {
            inventoryService.SellInventoryItem(viking, invItemID, ref gold, ref shard);
        }

        if (gold == 0 && shard == 0) { // NOTE: client sometimes call SellItems with invalid UserInventoryCommonIDs for unknown reasons
            return Ok(new CommonInventoryResponse { Success = false });
        }

        // apply shards reward
        CommonInventoryResponseItem resShardsItem = inventoryService.AddItemToInventoryAndGetResponse(viking, InventoryService.Shards, shard);
        
        // apply cash (gold) reward from sell items
        achievementService.AddAchievementPoints(viking, AchievementPointTypes.GameCurrency, gold);
        
        // save
        ctx.SaveChanges();

        // return success with shards reward
        return Ok(new CommonInventoryResponse {
            Success = true,
            CommonInventoryIDs = new CommonInventoryResponseItem[] {
                resShardsItem
            },
            UserGameCurrency = achievementService.GetUserCurrency(viking)
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/AddBattleItems")]
    [VikingSession]
    public IActionResult AddBattleItems(Viking viking, [FromForm] string request) {
        AddBattleItemsRequest req = XmlUtil.DeserializeXml<AddBattleItemsRequest>(request);
        
        var resItemList = new List<InventoryItemStatsMap>();
        foreach (BattleItemTierMap battleItemTierMap in req.BattleItemTierMaps) {
            for (int i=0; i<battleItemTierMap.Quantity; ++i) {
                resItemList.Add(
                    inventoryService.AddBattleItemToInventory(viking, battleItemTierMap.ItemID, (int)battleItemTierMap.Tier, battleItemTierMap.ItemStats)
                    // NOTE: battleItemTierMap.ItemStats is extension for importer
                );
            }
        }
        
        // NOTE: saved inside AddBattleItemToInventory
        
        return Ok(new AddBattleItemsResponse{
            Status = Status.Success,
            InventoryItemStatsMaps = resItemList
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/ProcessRewardedItems")]
    [VikingSession]
    public IActionResult ProcessRewardedItems(Viking viking, [FromForm] string request) {
        ProcessRewardedItemsRequest req = XmlUtil.DeserializeXml<ProcessRewardedItemsRequest>(request);
        
        if (req is null || req.ItemsActionMap is null)
            return Ok(new ProcessRewardedItemsResponse());
        
        int shard = 0;
        int gold = 0;
        bool soldInventoryItems = false;
        bool soldRewardBinItems = false;
        var itemsAddedToInventory = new List<CommonInventoryResponseRewardBinItem>();
        foreach (ItemActionTypeMap actionMap in req.ItemsActionMap) {
            switch (actionMap.Action) {
                case ActionType.MoveToInventory:
                    // item is in inventory in result of ApplyRewards ... only add to itemsAddedToInventory
                    itemsAddedToInventory.Add (new CommonInventoryResponseRewardBinItem {
                        ItemID = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == actionMap.ID).ItemId,
                        CommonInventoryID = actionMap.ID,
                        Quantity = 0,
                        UserItemStatsMapID = actionMap.ID
                    });
                    break;
                case ActionType.SellInventoryItem:
                    soldInventoryItems = true;
                    inventoryService.SellInventoryItem(viking, actionMap.ID, ref gold, ref shard);
                    break;
                case ActionType.SellRewardBinItem:
                    soldRewardBinItems = true;
                    inventoryService.SellInventoryItem(viking, actionMap.ID, ref gold, ref shard);
                    break;
            }
        }
        
        // apply shards reward from sell items
        InventoryItem item = inventoryService.AddItemToInventory(viking, InventoryService.Shards, shard);
        
        // NOTE: client expects multiple items each with quantity = 0
        var inventoryResponse = new CommonInventoryResponseItem[shard];
        for (int i=0; i<shard; ++i) {
            inventoryResponse[i] = new CommonInventoryResponseItem {
                CommonInventoryID = item.Id,
                ItemID = item.ItemId,
                Quantity = 0
            };
        }
        
        // apply cash (gold) reward from sell items
        achievementService.AddAchievementPoints(viking, AchievementPointTypes.GameCurrency, gold);
        
        // save
        ctx.SaveChanges();
        
        return Ok(new ProcessRewardedItemsResponse {
            SoldInventoryItems = soldInventoryItems,
            SoldRewardBinItems = soldRewardBinItems,
            MovedRewardBinItems = itemsAddedToInventory.ToArray(),
            CommonInventoryResponse = new CommonInventoryResponse {
                Success = false,
                CommonInventoryIDs = inventoryResponse,
                UserGameCurrency = achievementService.GetUserCurrency(viking)
            }
        });
    }
    
    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/ContentWebService.asmx/ApplyRewards")]
    [VikingSession]
    public IActionResult ApplyRewards(Viking viking, [FromForm] string request) {
        ApplyRewardsRequest req = XmlUtil.DeserializeXml<ApplyRewardsRequest>(request);
        
        List<AchievementReward> achievementRewards = new List<AchievementReward>();
        UserItemStatsMap? rewardedBattleItem = null;
        CommonInventoryResponse? rewardedStandardItem = null;
        
        int rewardMultipler = 0;
        if (req.LevelRewardType == LevelRewardType.LevelFailure) {
            rewardMultipler = 1;
        } else if (req.LevelRewardType == LevelRewardType.LevelCompletion) {
            rewardMultipler = 2 * req.LevelDifficultyID;
        }
        
        if (rewardMultipler > 0) {
            // TODO: XP values and method of calculation is not grounded in anything ...

            // dragons XP
            if (req.RaisedPetEntityMaps != null) {
                int dragonXp = 40 * rewardMultipler;
                foreach (RaisedPetEntityMap petInfo in req.RaisedPetEntityMaps) {
                    Dragon? dragon = viking.Dragons.FirstOrDefault(e => e.Id == petInfo.RaisedPetID);
                    dragon.PetXP = (dragon.PetXP ?? 0) + dragonXp;
                    achievementRewards.Add(new AchievementReward{
                        EntityID = petInfo.EntityID,
                        PointTypeID = AchievementPointTypes.DragonXP,
                        Amount = dragonXp
                    });
                }
            }

            // player XP and gems
            achievementRewards.Add(
                achievementService.AddAchievementPoints(viking, AchievementPointTypes.PlayerXP, 60 * rewardMultipler)
            );
            achievementRewards.Add(
                achievementService.AddAchievementPoints(viking, AchievementPointTypes.CashCurrency, 2 * rewardMultipler)
            );
        }

        //  - battle backpack items, blueprints and other items
        if (req.LevelRewardType != LevelRewardType.LevelFailure) {
            Gender gender = XmlUtil.DeserializeXml<AvatarData>(viking.AvatarSerialized).GenderType;
            ItemData rewardItem = itemService.GetDTReward(gender);
            if (itemService.ItemHasCategory(rewardItem, 651) || rewardItem.PossibleStatsMap is null) {
                // blueprint or no battle item (including box)
                List<CommonInventoryResponseItem> standardItems = new List<CommonInventoryResponseItem>();
                itemService.CheckAndOpenBox(rewardItem.ItemID, out int itemId, out int quantity);
                for (int i=0; i<quantity; ++i) {
                    standardItems.Add(inventoryService.AddItemToInventoryAndGetResponse(viking, itemId, 1));
                    // NOTE: client require single quantity items
                }
                rewardedStandardItem = new CommonInventoryResponse {
                    Success = true,
                    CommonInventoryIDs = standardItems.ToArray()
                };
            } else {
                // DT item
                InventoryItemStatsMap item = inventoryService.AddBattleItemToInventory(viking, rewardItem.ItemID, random.Next(1, 4));
                rewardedBattleItem = new UserItemStatsMap {
                    Item = item.Item,
                    ItemStats = item.ItemStatsMap.ItemStats,
                    ItemTier = item.ItemStatsMap.ItemTier,
                    UserItemStatsMapID = item.CommonInventoryID,
                    CreatedDate = new DateTime(DateTime.Now.Ticks)
                };
            }
        }

        // save
        ctx.SaveChanges();
        
        return Ok(new ApplyRewardsResponse {
            Status = Status.Success,
            AchievementRewards = achievementRewards.ToArray(),
            RewardedItemStatsMap = rewardedBattleItem,
            CommonInventoryResponse = rewardedStandardItem,
        });
    }

    private static RaisedPetData GetRaisedPetDataFromDragon (Dragon dragon, int? selectedDragonId = null) {
        if (selectedDragonId is null)
            selectedDragonId = dragon.Viking.SelectedDragonId;
        RaisedPetData data = XmlUtil.DeserializeXml<RaisedPetData>(dragon.RaisedPetData);
        data.RaisedPetID = dragon.Id;
        data.EntityID = Guid.Parse(dragon.EntityId);
        data.IsSelected = (selectedDragonId == dragon.Id);
        return data;
    }

    // Needs to merge newDragonData into dragonData
    private RaisedPetData UpdateDragon (Dragon dragon, RaisedPetData newDragonData, bool import = false) {
        RaisedPetData dragonData = XmlUtil.DeserializeXml<RaisedPetData>(dragon.RaisedPetData);

        // The simple attributes
        dragonData.IsPetCreated = newDragonData.IsPetCreated;
        if (newDragonData.ValidationMessage is not null) dragonData.ValidationMessage = newDragonData.ValidationMessage;
        if (newDragonData.EntityID is not null) dragonData.EntityID = newDragonData.EntityID;
        if (newDragonData.Name is not null) dragonData.Name = newDragonData.Name;
        dragonData.PetTypeID = newDragonData.PetTypeID;
        if (newDragonData.GrowthState is not null) {
            achievementService.DragonLevelUpOnAgeUp(dragon, dragonData.GrowthState, newDragonData.GrowthState);
            dragonData.GrowthState = newDragonData.GrowthState;
        }
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

        if (import) dragonData.CreateDate = newDragonData.CreateDate;

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

    private void AddSuggestion(Random rand, string name, List<string> suggestions) {
        if (ctx.Vikings.Any(x => x.Name == name) || suggestions.Contains(name)) {
            name += rand.Next(1, 5000);
            if (ctx.Vikings.Any(x => x.Name == name) || suggestions.Contains(name)) return;
        }
        suggestions.Add(name);
    }

    private string GetNameSuggestion(Random rand, string username, string[] adjectives) {
        string name = username;
        if (rand.NextDouble() >= 0.5)
            name = username + "The" + adjectives[rand.Next(adjectives.Length)];
        if (name == username || rand.NextDouble() >= 0.5)
            return adjectives[rand.Next(adjectives.Length)] + name;
        return name;
    }
}
