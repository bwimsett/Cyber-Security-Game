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
    
    protected override void Initialise() {
        fieldTitle.text = nodeField.GetFieldTitle();
        dropdown.options = GenerateOptionData();
        dropdown.value = (int) nodeField.GetValue();
    }

    public override void Refresh() {
        
    }

    public override void OnValueChanged() {
        nodeField.SetValue(dropdown.value);
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
