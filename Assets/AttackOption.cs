using System.Collections;
using System.Collections.Generic;
using backend;
using TMPro;
using UnityEngine;

public class AttackOption : MonoBehaviour {

    private Threat threat;
    private int position;

    public TextMeshProUGUI textField;

    public void SetThreat(Threat threat, int position) {
        this.threat = threat;

        Threat[] trace = threat.GetTrace();

        string firstNode = trace[0].GetNode().nodeObject.GetNodeDefinition().nodeName;
        string lastNode = threat.GetNode().nodeObject.GetNodeDefinition().nodeName;

        textField.text = "" + (position+1) + " | " + threat.threatType + ", " + firstNode + " -> " + lastNode;
    }
    
   
}
