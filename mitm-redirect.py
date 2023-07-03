from mitmproxy import ctx
import mitmproxy.http

methods = [
  'GetRules',
  'LoginParent',
  'AuthenticateUser',
  'RegisterParent',
  'GetSubscriptionInfo',
  'GetUserInfoByApiToken',
  'IsValidApiToken_V2',
  'ValidateName',
  'GetDefaultNameSuggestion',
  'RegisterChild',
  'GetProfileByUserId',
  'LoginChild',
  'GetUserProfileByUserID',
  'GetKeyValuePair',
  'SetKeyValuePair',
  'GetKeyValuePairByUserID',
  'SetKeyValuePairByUserID',
  'GetUserProfile',
  'GetQuestions',
  'GetCommonInventory',
  'SetCommonInventory',
  'GetItem',
  'GetAuthoritativeTime',
  'SetAvatar',
  'GetPetAchievementsByUserID',
  'GetDetailedChildList',
  'GetStore',
  'GetAllRanks',
  'GetUserUpcomingMissionState',
  'GetUserActiveMissionState',
  'GetUserCompletedMissionState',
  'SetTaskState',
  'CreatePet',
  'SetRaisedPet',
  'SetSelectedPet',
  'GetAllActivePetsByuserId',
  'GetSelectedRaisedPet',
  'SetImage',
  'GetImage',
  'GetImageByUserId',
  'GetAchievementTaskInfo',
  'GetAllRewardTypeMultiplier',
  'GetBuddyList',
  'GetRankAttributeData',
  'GetUserMessageQueue',
  'SendMessage',
  'SaveMessage',
  'GetMMOServerInfoWithZone',
  'GetActiveChallenges',
  'GetAchievementsByUserID',
  'PurchaseItems'
]

def routable(path):
  for method in methods:
    if method in path:
      return True
  return False


class LocalRedirect:
  def __init__(self):
    print('Loaded redirect addon')

  def request(self, flow: mitmproxy.http.HTTPFlow):
    if 'api.sodoff.spirtix.com' in flow.request.pretty_host and routable(flow.request.path):
      flow.request.host = "localhost"
      flow.request.scheme = 'http'
      flow.request.port = 5000

addons = [
  LocalRedirect()
]
