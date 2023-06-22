using sodoff.Schema;
using sodoff.Util;
using System.Runtime.Serialization.Formatters.Binary;

namespace sodoff.Services;
public class MissionStoreSingleton {

    private Dictionary<int, Mission> missions = new();

    public MissionStoreSingleton() {
        ServerMissionArray missionArray = XmlUtil.DeserializeXml<ServerMissionArray>(XmlUtil.ReadResourceXmlString("missions"));
        foreach (var mission in missionArray.MissionDataArray) {
            SetUpRecursive(mission);
        }
    }

    public Mission GetMission(int missionID) {
        return DeepCopy(missions[missionID]);
    }

    private void SetUpRecursive(Mission mission) {
        missions.Add(mission.MissionID, mission);
        foreach (var innerMission in mission.Missions) {
            SetUpRecursive(innerMission);
        }
    }

    // FIXME: Don't use BinaryFormatter for deep copying
    // FIXME: Remove <EnableUnsafeBinaryFormatterSerialization> flag from the project file once we have a different way of deep copying
    public static Mission DeepCopy(Mission original) {
        using (MemoryStream memoryStream = new MemoryStream()) {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(memoryStream, original);

            memoryStream.Position = 0;

            Mission clone = (Mission)formatter.Deserialize(memoryStream);

            return clone;
        }
    }

}
