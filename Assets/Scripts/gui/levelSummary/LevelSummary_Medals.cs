using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using gui.levelSummary;
using UnityEngine;

public class LevelSummary_Medals : MonoBehaviour {

    public LevelSummary_Medal[] medals;
    public int medalSpacing;
    public float medalMoveSpeed;
    public float medalSizeFalloff;
    public float medalSnapTolerance;
    public float minMedalScale;

    private Medal medal;
    private int selectedMedal;
    
    public void SetMedal(Medal medal) {
        Reset();
        
        this.medal = medal;
        
        switch (medal) {
            case Medal.None: selectedMedal = 0;
                break;
            case Medal.Bronze: selectedMedal = 1;
                break;
            case Medal.Silver: selectedMedal = 2;
                break;
            case Medal.Gold: selectedMedal = 3;
                break;
        }

        Refresh();
    }

    private void Refresh() {

        Level currentLevel = GameManager.currentLevel;

        Medal[] medalTypes = new Medal[] {Medal.None, Medal.Bronze, Medal.Silver, Medal.Gold};
        
        for (int i = 0; i < medals.Length; i++) {
            int distanceFromMedal = i - selectedMedal;
            
            Vector3 targetPos = new Vector3(distanceFromMedal * medalSpacing, 0, 0);
            
            medals[i].SetTargets(targetPos, medalSizeFalloff, medalMoveSpeed, medalSnapTolerance, minMedalScale);
            medals[i].transform.SetSiblingIndex(3-Mathf.Abs(distanceFromMedal));
    
            medals[i].SetText(currentLevel.GetMedalBoundary(medalTypes[i])+"+");
        }
    }

    public void Reset() {
        foreach (LevelSummary_Medal medal in medals) {
            medal.transform.localPosition = new Vector2(900, 0); 
        }
    }


}
