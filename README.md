# SoD-Off - School of Dragons, Offline

On 7th June, 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.

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
- start tutorial

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
- SetRaisedPet
- GetAllActivePetsByuserId
- SetImage
- GetImage

#### Implemented enough (probably)
- GetRules (doesn't return any rules, probably doesn't need to)
- GetQuestions (doesn't return all questions, probably doesn't need to)
- GetSubscriptionInfo (always returns member, with end date 10 years from now)
- SetTaskState (only the TaskCanBeDone status is supported)
- CreatePet (doesn't handle SetAsSelectedPet/UnSelectOtherPets)

#### Partially implemented
- GetUserProfileByUserID (a lot is still placeholder)
- GetUserProfile (a lot is still placeholder)
- GetDetailedChildList (a lot is still placeholder)
- ValidateName (needs to do pets, groups, default)
- GetDefaultNameSuggestion (needs to return unused names)
- GetUserActiveMissionState (returns the tutorial mission and updates it with progress)

#### Currently static or stubbed
- GetStore (needs to filter out stores from request)
- GetAllRanks (needs to be populated with what ranks the user has)
- GetCommonInventory
- GetPetAchievementsByUserId (always returns null)
- GetUserUpcomingMissionState (returns no missions)
- GetUserCompletedMissionState (returns no missions)
