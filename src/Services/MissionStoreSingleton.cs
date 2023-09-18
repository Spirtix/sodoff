using sodoff.Schema;
using sodoff.Util;
using System.Runtime.Serialization.Formatters.Binary;

namespace sodoff.Services;
public class MissionStoreSingleton {

    private Dictionary<int, Mission> missions = new();
    private int[] activeMissions;
    private int[] upcomingMissions;
    private int[] activeMissionsV1;
    private int[] upcomingMissionsV1;

    public MissionStoreSingleton() {
        ServerMissionArray missionArray = XmlUtil.DeserializeXml<ServerMissionArray>(XmlUtil.ReadResourceXmlString("missions"));
        DefaultMissions defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("defaultmissionlist"));
        foreach (var mission in missionArray.MissionDataArray) {
            SetUpRecursive(mission);
        }
        activeMissions = defaultMissions.Active;
        upcomingMissions = defaultMissions.Upcoming;
        
        defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("defaultmissionlistv1"));
        activeMissionsV1 = defaultMissions.Active;
        upcomingMissionsV1 = defaultMissions.Upcoming;
    }

    public Mission GetMission(int missionID) {
        return DeepCopy(missions[missionID]);
    }

    public int[] GetActiveMissions(string apiKey) {
        if (apiKey == "a1a13a0a-7c6e-4e9b-b0f7-22034d799013") {
            return activeMissionsV1;
        }
        return activeMissions;
    }

    public int[] GetUpcomingMissions(string apiKey) {
        if (apiKey == "a1a13a0a-7c6e-4e9b-b0f7-22034d799013") {
            return upcomingMissionsV1;
        }
        return upcomingMissions;
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
