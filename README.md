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

#### Implemented enough (probably)
- GetRules (doesn't return any rules, probably doesn't need to)
- GetQuestions (doesn't return all questions, probably doesn't need to)
- GetSubscriptionInfo (always returns member, with end date 10 years from now)

#### Partially implemented
- GetUserProfileByUserID (a lot is still placeholder)
- GetUserProfile (a lot is still placeholder)
- GetDetailedChildList (a lot is still placeholder)
- ValidateName (needs to do pets, groups, default)
- GetDefaultNameSuggestion (needs to return unused names)

#### Currently static or stubbed
- GetStore (needs to filter out stores from request)
- GetAllRanks (needs to be populated with what ranks the user has)
- GetCommonInventory
- GetAllActivePetsByuserId (always returns null)
- GetPetAchievementsByUserId (always returns null)
