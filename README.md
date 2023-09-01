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
- many missions
- hideouts
- farms

### Methods

#### Fully implemented
- GetKeyValuePairByUserID
- GetKeyValuePair
- SetKeyValuePairByUserID
- SetKeyValuePair
- GetAuthoritativeTime
- LoginParent
- GetUserInfoByApiToken
- IsValidApiToken_V2
- LoginChild
- SetAvatar
- RegisterParent
- RegisterChild
- CreatePet
- SetRaisedPet
- SetSelectedPet
- GetAllActivePetsByuserId
- GetSelectedRaisedPet
- SetImage
- GetImage
- GetImageByUserId
- GetItem
- GetStore
- PurchaseItems (V1)
- PurchaseItems (V2)
- AcceptMission
- GetUserMissionState
- GetUserActiveMissionState
- GetUserUpcomingMissionState
- GetUserCompletedMissionState
- GetChildList
- GetUnselectedPetByTypes
- UseInventory
- DeleteProfile
- DeleteAccountNotification
- SetAchievementAndGetReward
- GetAchievementsByUserID
- GetPetAchievementsByUserID
- RerollUserItem
- FuseItems
- AddBattleItems

#### Implemented enough (probably)
- GetRules (doesn't return any rules, probably doesn't need to)
- GetQuestions (doesn't return all questions, probably doesn't need to)
- GetSubscriptionInfo (always returns member, with end date 10 years from now)
- SetTaskState (only the TaskCanBeDone status is supported; might contain a serious problem - see the MissionService class)
- GetCommonInventory
- GetUserRoomItemPositions
- SetUserRoomItemPositions
- SetUserAchievementAndGetReward (works like SetAchievementAndGetReward)

#### Partially implemented
- GetUserProfileByUserID (a lot is still placeholder)
- GetUserProfile (a lot is still placeholder)
- GetDetailedChildList (a lot is still placeholder)
- ValidateName (needs to do pets, groups, default)
- GetDefaultNameSuggestion (needs to return unused names)
- SetCommonInventory (some properties are not retained, doesn't support delete)
- GetUserRoomList (room categories are not implemented)
- SetNextItemState (may require more work, we don't know yet)
- SetUserRoom
- SetUserAchievementTask (returns a real reward but still use task placeholder)
- ProcessRewardedItems (need add cash reward)
- SellItems (need add cash reward)
- ApplyRewards

#### Currently static or stubbed
- GetAllRanks (needs to be populated with what ranks the user has)
- GetAchievementTaskInfo (returns a static XML)
- GetAllRewardTypeMultiplier (returns a static XML)
- GetBuddyList (returns an emtpy array)
- GetRankAttributeData (returns a static XML)
- GetUserMessageQueue (returns an emtpy array)
- SendMessage (doesn't do anything and returns false)
- SaveMessage (doesn't do anything and returns false)
- GetMMOServerInfoWithZone (returns an empty array)
- GetActiveChallenges (returns an empty array)
- GetAnnouncementsByUser (returns no announcements, but that might be sufficient)
- GetAverageRatingForRoom (return max rating)
- GetUserActivityByUserID (returns an empty array)
- GetUserGameCurrency (return 65536 gems and 65536 coins)
- SetAchievementByEntityIDs (returns a static achievement)
