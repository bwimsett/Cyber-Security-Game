using System.Collections;
using System.Collections.Generic;
using backend.threat_modelling;
using DefaultNamespace;
using DefaultNamespace.node;
using gui;
using gui.control_settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsWindow : Window {
    private Node node;

    public RectTransform scrollRectTransform;
    public RectTransform settingsContainer;
    public ControlSettings_Slider slider_prefab;
    public ControlSettings_Dropdown dropdown_prefab;
    public ControlSettings_Text text_prefab;
    public ControlSettings_Tickbox tickbox_prefab;
    public ControlSettings_Tickbox tickbox_threat_prefab;

    public Vector2 positionOffset;
    public RectTransform canvas;
    
    private ControlSettings_Field[] fieldObjects;
    private ControlSettings_Field startingThreatsField;

    void Update() {
        settingsContainer.ForceUpdateRectTransforms();
        scrollRectTransform.ForceUpdateRectTransforms();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRectTransform);
        Canvas.ForceUpdateCanvases();
    }
    
    public void SetNode(Node node) {
        if (node == null) {
            GameManager.levelScene.guiManager.CloseControlSettingsWindow();
            return;
        }
        
        this.node = node;
        title.text = node.nodeObject.GetNodeDefinition().nodeName;

        RefreshPosition();
        
        GenerateFields();
    }

    // Generates a field on the interface for each field in the selected node's behaviour
    private void GenerateFields() {
        ClearFields();
        
        NodeBehaviour behaviour = node.GetBehaviour();
        NodeField[] fields  = behaviour.GetFields();
        List<ControlSettings_Field> fieldList = new List<ControlSettings_Field>();

        if (fields == null) {
            GenerateLevelBuilderFields(fieldList);
            fieldObjects = fieldList.ToArray();
            return;
        }

        foreach (NodeField f in fields) {
            fieldList.Add(GetFieldFromFieldType(f));
        }

        GenerateLevelBuilderFields(fieldList);
        
        fieldObjects = fieldList.ToArray();
    }

    // Instantiates a field object and attaches it to the interface, based on the given fieldType
    private ControlSettings_Field GetFieldFromFieldType(NodeField nodeField) {
        GameObject prefab = null;
        
        switch (nodeField.GetFieldType()) {
            case NodeFieldType.integer_range: prefab = slider_prefab.gameObject;
                break;
            case NodeFieldType.enumerable_single: prefab = dropdown_prefab.gameObject;
                break;
            case NodeFieldType.text: prefab = text_prefab.gameObject;
                break;
            case NodeFieldType.enumerable_many: prefab = tickbox_prefab.gameObject;
                break;
            case NodeFieldType.threat: prefab = tickbox_threat_prefab.gameObject;
                break;
        }

        ControlSettings_Field field = Instantiate(prefab, settingsContainer).GetComponent<ControlSettings_Field>();
        field.SetNodeField(nodeField);
        
        field.GetComponent<RectTransform>().ForceUpdateRectTransforms();

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
        
        Destroy(startingThreatsField.gameObject);

        fieldObjects = null;
    }

    private void GenerateLevelBuilderFields(List<ControlSettings_Field> fieldList) {
        startingThreatsField = GetFieldFromFieldType(node.GetBehaviour().GetSelectedStartingThreats());
        
        if (!GameManager.currentLevel.IsEditMode()) {
            startingThreatsField.gameObject.SetActive(false);
        }
        
        fieldList.Add(startingThreatsField);
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }

    public Node GetNode() {
        return node;
    }

    public void RefreshPosition() {

        RectTransform rect = GetComponent<RectTransform>();

        // Find min and max positions to ensure it stays fully on the screen

        float xMin = 0;
        float xMax = Screen.width - (rect.rect.width);
        float yMin = rect.rect.height;
        float yMax = Screen.height;
              
        Vector2 position = Camera.main.WorldToScreenPoint(node.nodeObject.gameObject.transform.position);

        position.x = Mathf.Max(position.x, xMin);
        position.y = Mathf.Max(position.y, yMin);
        position.x = Mathf.Min(position.x, xMax);
        position.y = Mathf.Min(position.y, yMax);

        rect.anchoredPosition = position;


    }

}
