using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class RankService {

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();

        public RankService() {
            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("allranks"));
            
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }
        }
        
        public int getRankFromXP(int? xpPoints, AchievementPointTypes type) {
            return ranks[type].Count(r => r.Value <= xpPoints);
        }
    }
}
