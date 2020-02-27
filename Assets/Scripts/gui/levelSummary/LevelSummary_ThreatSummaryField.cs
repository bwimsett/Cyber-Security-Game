using System.Collections;
using System.Collections.Generic;
using backend;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSummary_ThreatSummaryField : MonoBehaviour {

    private Threat threat;
    public TextMeshProUGUI summaryField;

    public void SetThreat(Threat threat) {
        this.threat = threat;
        summaryField.text = GameManager.levelScene.threatManager.threatSummariser.SummariseThreat(threat);
    }

}
