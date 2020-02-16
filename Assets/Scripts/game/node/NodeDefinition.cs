using System;
using System.Collections;
using System.Collections.Generic;
using backend;
using backend.threat_modelling;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu (fileName = "New Node", menuName = "Node Type")]
public class NodeDefinition : ScriptableObject
{
    public String nodeName;
    public int nodeCost;
    public NodeFamily nodeFamily;
    public NodeType nodeType;
    public Color nodeColor;
    public Sprite nodeIcon;
    public TextAsset description;
    public ThreatType[] startingThreats;
    public Threat_EffectPair[] threatResponses;
    public Threat_EvolutionPair[] threatEvolutions;

    // Converts starting threat list into a set of options for the control settings menu
    public Control_Dropdown_Option_Set GetStartingThreatsOptionSet() {
        Control_Dropdown_Option[] options = new Control_Dropdown_Option[startingThreats.Length];

        for (int i = 0; i < options.Length; i++) {
            options[i] = new Control_Dropdown_Option();
            options[i].cost = 0;
            options[i].name = startingThreats[i].ToString();
            options[i].health = 0;
        }

        Control_Dropdown_Option_Set optionSet = CreateInstance<Control_Dropdown_Option_Set>();
        optionSet.options = options;

        return optionSet;
    }
}
