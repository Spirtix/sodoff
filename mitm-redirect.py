from mitmproxy import ctx
import mitmproxy.http


def routable(path):
  methods = ['GetRules', 'LoginParent', 'RegisterParent', 'GetSubscriptionInfo', 'GetUserInfoByApiToken', 'IsValidApiToken_V2']
  for method in methods:
    if method in path:
      return True
  return False


class LocalRedirect:
  def __init__(self):
    print('Loaded redirect addon')

  def request(self, flow: mitmproxy.http.HTTPFlow):
    if 'common.api.jumpstart.com' in flow.request.pretty_host and routable(flow.request.path):
      flow.request.host = "localhost"
      flow.request.scheme = 'http'
      flow.request.port = 5000

addons = [
  LocalRedirect()
]
