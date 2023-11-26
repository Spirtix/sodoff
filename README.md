# SoD-Off - School of Dragons, Offline

On 7th June, 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.

## Discord
[![Discord Banner](https://discordapp.com/api/guilds/1124405524679643318/widget.png?style=banner2)](https://discord.gg/bqHtMRbhM3)

## Getting started

For the first time setup, run the following command:

```
dotnet restore
```

Then run the server as follows:

```
# run mitmproxy to redirect requests to the app
mitmproxy -s mitm-redirect.py

# run the server
dotnet run --project src/sodoff.csproj
```

Then run School of Dragons.

## Status

### What works
- register/login
- create profile
- list profiles
- tutorial
- roaming in the open world
- inventory
- store
- missions
- hideouts
- farms
- minigames
- MMO (using sodoff-mmo)

### Methods

#### Fully implemented
- AcceptMission
- AddBattleItems
- AuthenticateUser
- CreatePet
- DeleteAccountNotification
- DeleteProfile
- FuseItems
- GetAchievementsByUserID
- GetAllActivePetsByuserId
- GetAuthoritativeTime
- GetChildList
- GetCommonInventory (V2)
- GetDefaultNameSuggestion
- GetDetailedChildList
- GetGameData
- GetImage
- GetImageByUserId
- GetItem
- GetKeyValuePair
- GetKeyValuePairByUserID
- GetMMOServerInfoWithZone (uses resource xml as response)
- GetPetAchievementsByUserID
- GetSelectedRaisedPet
- GetStore
- GetUnselectedPetByTypes
- GetUserActiveMissionState
- GetUserCompletedMissionState
- GetUserInfoByApiToken
- GetUserMissionState
- GetUserProfile
- GetUserProfileByUserID
- GetUserRoomItemPositions
- GetUserUpcomingMissionState
- IsValidApiToken_V2
- LoginChild
- LoginParent
- PurchaseItems (V1)
- PurchaseItems (V2)
- RedeemMysteryBoxItems
- RegisterChild
- RegisterParent
- RerollUserItem
- SetAchievementAndGetReward
- SetAchievementByEntityIDs
- SetAvatar
- SetCommonInventory
- SetDragonXP (used by account import tools)
- SetImage
- SetKeyValuePair
- SetKeyValuePairByUserID
- SetPlayerXP (used by account import tools)
- SetRaisedPet
- SetSelectedPet
- SetUserRoomItemPositions
- UseInventory

#### Implemented enough (probably)
- GetCommonInventory (V1 -  returns the viking's inventory if it is called with a viking; otherwise returns 8 viking slots)
- GetQuestions (doesn't return all questions, probably doesn't need to)
- GetRules (doesn't return any rules, probably doesn't need to)
- GetSubscriptionInfo (always returns member, with end date 10 years from now)
- SendRawGameData
- SetNextItemState
- SetTaskState (only the TaskCanBeDone status is supported; might contain a serious problem - see the MissionService class)
- SetUserAchievementAndGetReward (works like SetAchievementAndGetReward)
- SetUserRoom
- ValidateName

#### Partially implemented
- ApplyPayout (doesn't calculate rewards properly)
- ApplyRewards
- GetGameDataByGame (friend tab displays all players - friend filter is not yet implemented because friend lists are not implemented)
- GetGameDataByGameForDateRange (friend tab displays all players)
- GetTopAchievementPointUsers (ignores type [all, buddy, hall of fame, ...] and mode [overall, monthly, weekly] properties)
- GetUserAchievements (used by Magic & Mythies)
- GetUserRoomList (room categories are not implemented, but it's enough for SoD)
- ProcessRewardedItems (gives gems, but doesn't give gold, gold is not yet implemented)
- SellItems (gives gems, but doesn't give gold, gold is not yet implemented)
- SetUserAchievementTask (returns a real reward but still use task placeholder)

#### Currently static or stubbed
- GetAchievementTaskInfo (returns a static XML)
- GetActiveChallenges (returns an empty array)
- GetAllRanks (needs to be populated with what ranks the user has)
- GetAllRewardTypeMultiplier (returns a static XML)
- GetAllRewardTypeMultiplier (returns a static XML)
- GetAnnouncementsByUser (returns no announcements, but that might be sufficient)
- GetAverageRatingForRoom (return max rating)
- GetBuddyList (returns an emtpy array)
- GetProfileTagAll (returns an empty array - used by Magic & Mythies)
- GetRankAttributeData (returns a static XML)
- GetUserActivityByUserID (returns an empty array)
- GetUserGameCurrency (return 65536 gems and 65536 coins)
- GetUserMessageQueue (returns an emtpy array)
- SaveMessage (doesn't do anything and returns false)
- SendMessage (doesn't do anything and returns false)
