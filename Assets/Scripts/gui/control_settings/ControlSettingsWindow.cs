using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.node;
using gui;
using gui.control_settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsWindow : Window {
    private Node node;

    public RectTransform settingContainerTransform;
    public ControlSettings_Slider slider_prefab;
    public ControlSettings_Dropdown dropdown_prefab;
    public ControlSettings_Text text_prefab;

    private ControlSettings_Field[] fieldObjects;
    
    public void SetNode(Node node) {
        this.node = node;
        title.text = node.nodeObject.GetNodeDefinition().nodeName;
        GenerateFields();
    }

    // Generates a field on the interface for each field in the selected node's behaviour
    private void GenerateFields() {
        ClearFields();
        
        NodeBehaviour behaviour = node.GetBehaviour();
        NodeField[] fields  = behaviour.GetFields();

        if (fields == null) {
            return;
        }
        
        fieldObjects = new ControlSettings_Field[fields.Length];

        for (int i = 0; i < fields.Length; i++) {
            NodeField f = fields[i];
            fieldObjects[i] = GetFieldFromFieldType(f);
        }
        
        settingContainerTransform.ForceUpdateRectTransforms();
        LayoutRebuilder.ForceRebuildLayoutImmediate(settingContainerTransform);

    }

    // Instantiates a field object and attaches it to the interface, based on the given fieldType
    private ControlSettings_Field GetFieldFromFieldType(NodeField nodeField) {
        GameObject prefab = null;
        
        switch (nodeField.GetFieldType()) {
            case NodeFieldType.integer_range: prefab = slider_prefab.gameObject;
                break;
            case NodeFieldType.enumerable: prefab = dropdown_prefab.gameObject;
                break;
            case NodeFieldType.text: prefab = text_prefab.gameObject;
                break;
        }

        ControlSettings_Field field = Instantiate(prefab, settingContainerTransform).GetComponent<ControlSettings_Field>();
        field.SetNodeField(nodeField);

        return field;
    }

    // Clears the fields
    private void ClearFields() {
        if (fieldObjects == null) {
            return;
        }
        
        foreach (ControlSettings_Field f in fieldObjects) {
            Destroy(f.gameObject);
        }

        fieldObjects = null;
    }

}
