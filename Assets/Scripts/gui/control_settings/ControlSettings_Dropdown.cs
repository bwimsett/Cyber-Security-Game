using System;
using System.Collections;
using System.Collections.Generic;
using backend.threat_modelling;
using DefaultNamespace;
using DefaultNamespace.node;
using gui.control_settings;
using TMPro;
using UnityEngine;

public class ControlSettings_Dropdown : ControlSettings_Field {

    public TMP_Dropdown dropdown;
    public Gradient optionColours;
    private bool colourOptions;
    public TextMeshProUGUI selectedItem;

    void Update() {
        Refresh();

        // Disable dropdown for access

    }
    
    protected override void Initialise() {
        fieldTitle.text = nodeField.GetFieldTitle();
        dropdown.options = GenerateOptionData();
        dropdown.value = (int) nodeField.GetValue();
        ColourOptions(nodeField.IsColourOptions());
        if (nodeField.isReadOnly()) {
            if (GameManager.currentLevel.IsEditMode()) {
                dropdown.interactable = true;
            }
            else {
                dropdown.interactable = false;
            }
        }
    }

    public override void Refresh() {
        if (!colourOptions) {
            return;
        }

        Transform listTransform = dropdown.transform.Find("Dropdown List");
        selectedItem.color = optionColours.Evaluate((float) dropdown.value / dropdown.options.Count);

        if (!listTransform) {
            return;
        }

        NodeField_Dropdown_Item[] options = listTransform.GetComponentsInChildren<NodeField_Dropdown_Item>();

        for (int i = 0; i < options.Length; i++) {
            NodeField_Dropdown_Item item = options[i];

            float colourPos = (float)i /options.Length;

            item.text.color = optionColours.Evaluate(colourPos);
        }   
    }

    public override void OnValueChanged() {
        nodeField.SetValue(dropdown.value);
        GameManager.currentLevel.RecalculateBudget();
    }

    public void ColourOptions(bool colourOptions) {
        this.colourOptions = colourOptions;
        Refresh();
    }
    
    private List<TMP_Dropdown.OptionData> GenerateOptionData() {
        Control_Dropdown_Option_Set options = nodeField.GetOptionSet();
        
        if (options == null) {
            Debug.Log("No option set chosen for dropdown: " + nodeField.GetFieldTitle());
            return null;
        }

        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();

        foreach (Control_Dropdown_Option o in options.options) {
            string optionText = o.name + " (" + o.cost + ")";
            data.Add(new TMP_Dropdown.OptionData(optionText));
        }

        return data;
    }
}
