using System.Collections;
using System.Collections.Generic;
using backend;
using DefaultNamespace;
using gui.levelSummary;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSummary_ThreatSummaryField : MonoBehaviour {

    private Threat threat;
    public TextMeshProUGUI summaryField;
    public LevelSummaryReplayButton summaryButton;

    public void SetThreat(Threat threat, int threatIndex) {
        this.threat = threat;
        summaryField.text = GameManager.levelScene.threatManager.threatSummariser.SummariseThreat(threat);
        summaryButton.SetThreatindex(threatIndex);
        
    }

}
