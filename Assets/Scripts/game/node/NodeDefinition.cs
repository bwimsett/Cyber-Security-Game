using System;
using System.Collections;
using System.Collections.Generic;
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
    
}
