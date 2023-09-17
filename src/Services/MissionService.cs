﻿using sodoff.Model;
using sodoff.Schema;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace sodoff.Services;
public class MissionService {

    private readonly DBContext ctx;
    private MissionStoreSingleton missionStore;
    private AchievementService achievementService;

    public MissionService(DBContext ctx, MissionStoreSingleton missionStore, AchievementService achievementService) {
        this.ctx = ctx;
        this.missionStore = missionStore;
        this.achievementService = achievementService;
    }

    public Mission GetMissionWithProgress(int missionId, string userId, string apiKey) {
        Mission mission;
        if (missionId == 999 && apiKey == "a3a12a0a-7c6e-4e9b-b0f7-22034d799013") { // TODO This is not a pretty solution with hard-coded values.
            mission = missionStore.GetMission(10999);
            mission.MissionID = 999;
        } else if (missionId == 999 && apiKey == "a2a09a0a-7c6e-4e9b-b0f7-22034d799013") {
            mission = missionStore.GetMission(20999);
            mission.MissionID = 999;
        } else {
            mission = missionStore.GetMission(missionId);
        }
        UpdateMissionRecursive(mission, userId);
        return mission;
    }

    public List<MissionCompletedResult> UpdateTaskProgress(int missionId, int taskId, string userId, bool completed, string xmlPayload, string apiKey) {
        SetTaskProgressDB(missionId, taskId, userId, completed, xmlPayload);

        // NOTE: This won't work if a mission can be completed by completing an inner mission
        // Let's have a mission with ID = 1
        // The mission has an inner inner mission with ID = 2
        // All tasks in mission 1 are completed
        // When the last task of mission 2 is completed the client sends SetTaskState with missionId = 2
        // Mission 2 gets completed which should also complete mission 1 because all tasks and missions are now completed
        // But there's no way of knowing that with the current data structures
        // At the moment, I don't know if a mission can be completed this way
        // I do know that outer missions have inner missions as RuleItems, and if the RuleItem is supposed to be "complete" and it isn't, the quest breaks when the player quits the game and loads the quest again
        List<MissionCompletedResult> result = new();
        if (completed) {
            Mission mission = GetMissionWithProgress(missionId, userId, apiKey);
            if (MissionCompleted(mission)) {
                // Update mission from active to completed
                Viking viking = ctx.Vikings.FirstOrDefault(x => x.Id == userId)!;
                MissionState? missionState = viking.MissionStates.FirstOrDefault(x => x.MissionId == missionId);
                if (missionState != null && missionState.MissionStatus == MissionStatus.Active) {
                    if (mission.Repeatable) {
                        // NOTE: This won't work if repeatable mission use sub-missions, but SoD doesn't have those repeatable mission
                        // NOTE: Repeatable missions needs re-login to work correctly (this looks like og bug)
                        //       probably due to client-side cache of task payload / status
                        var taskStatuses = ctx.TaskStatuses.Where(e => e.VikingId == userId && e.MissionId == missionId);
                        foreach (var task in taskStatuses) {
                            task.Payload = null;
                            task.Completed = false;
                        }
                        missionState.MissionStatus = MissionStatus.Upcoming;
                    } else {
                        missionState.MissionStatus = MissionStatus.Completed;
                    }
                    missionState.UserAccepted = null;
                }
                var rewards = achievementService.ApplyAchievementRewards(viking, mission.Rewards.ToArray());
                ctx.SaveChanges();
                
                // Get mission rewards
                result.Add(new MissionCompletedResult {
                    MissionID = missionId,
                    Rewards = rewards
                });
            }
        }
        return result;
    }

    private void UpdateMissionRecursive(Mission mission, string userId) {
        List<Model.TaskStatus> taskStatuses = ctx.TaskStatuses.Where(e => e.VikingId == userId && e.MissionId == mission.MissionID).ToList();

        // Update mission rules and tasks
        foreach (var task in taskStatuses) {
            // Rules have two types - task and mission
            RuleItem? rule = mission.MissionRule.Criteria.RuleItems.Find(x => x.ID == task.Id && x.Type == RuleItemType.Task);
            if (rule != null && task.Completed) rule.Complete = 1;

            Schema.Task? t = mission.Tasks.Find(x => x.TaskID == task.Id);
            if (t != null) {
                if (task.Completed) t.Completed = 1;
                t.Payload = task.Payload;
            }
        }

        // Update all inner missions
        // Update rules with missions
        foreach (var innerMission in mission.Missions) {
            UpdateMissionRecursive(innerMission, userId);
            if (innerMission.Completed == 1) {
                RuleItem? rule = mission.MissionRule.Criteria.RuleItems.Find(x => x.ID == innerMission.MissionID && x.Type == RuleItemType.Mission);
                if (rule != null) rule.Complete = 1;
            }
        }

        if (MissionCompleted(mission))
            mission.Completed = 1;
    }

    public void SetUpMissions(Viking viking) {
        viking.MissionStates = new List<MissionState>();
        foreach (int m in missionStore.GetActiveMissions()) {
            viking.MissionStates.Add(new MissionState {
                MissionId = m,
                MissionStatus = MissionStatus.Active
            });
        }
        
        foreach (int m in missionStore.GetUpcomingMissions()) {
            viking.MissionStates.Add(new MissionState {
                MissionId = m,
                MissionStatus = MissionStatus.Upcoming
            });
        }
    }

    private void SetTaskProgressDB(int missionId, int taskId, string userId, bool completed, string xmlPayload) {
        Model.TaskStatus? status = ctx.TaskStatuses.FirstOrDefault(task => task.Id == taskId && task.MissionId == missionId && task.VikingId == userId);

        if (status is null) {
            status = new Model.TaskStatus {
                Id = taskId,
                MissionId = missionId,
                VikingId = userId,
                Payload = xmlPayload,
                Completed = completed
            };
            ctx.TaskStatuses.Add(status);
        } else {
            status.Payload = xmlPayload;
            if (!status.Completed) status.Completed = completed; // NOTE: Lab missions update the payload after the task is marked as completed with the completed flag set to false. The official servers ignore this flag when it's already been set to true
        }
        ctx.SaveChanges();
    }

    private bool MissionCompleted(Mission mission) {
        return mission.MissionRule.Criteria.RuleItems.All(x => x.Complete == 1);
    }
}
