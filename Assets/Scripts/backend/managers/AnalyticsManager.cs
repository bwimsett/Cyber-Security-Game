using System.Collections;
using System.Collections.Generic;
using backend.level;
using DefaultNamespace;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Setup;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour {

    private static float startTime = 0;
    private static float totalTime = 0;
    private static bool pausedTimer;
    
    public static void StartLevel() {
        string levelName = GameManager.selectedLevelDescription.levelName;
  
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, GameManager.selectedLevelDescription.levelName, "Score");
        startTime = Time.time;
        totalTime = 0;
    }

    public static void EndLevel(int score, Medal medal) {
        string levelName = GameManager.selectedLevelDescription.levelName;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName, "Score", score);
        
        ResumeTimer();
        
        totalTime += Time.time - startTime;
        Debug.Log("Time to complete level: " + totalTime+"s");
        
        // Create event for time
        GameAnalytics.NewDesignEvent("Time:"+levelName, totalTime);
        
        // Create event for medal
        GameAnalytics.NewDesignEvent("Medal:"+levelName, LevelScore.MedalToInt(medal));
         
    }

    public static void PauseTimer() {
        string levelName = GameManager.selectedLevelDescription.levelName;

        // Calculate total time up until now
        totalTime += Time.time - startTime;
        Debug.Log("Timer paused at: "+totalTime+"s");
        
        pausedTimer = true;

    }

    public static void ResumeTimer() {
        if (!pausedTimer) {
            return;
        }
        
        // Set start time to now
        startTime = Time.time;
    }


}
