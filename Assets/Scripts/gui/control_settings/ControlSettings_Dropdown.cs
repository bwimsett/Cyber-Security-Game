using System;
using System.Collections;
using System.Collections.Generic;
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
        Type enumType = nodeField.GetOptionsEnum();

        Array options = Enum.GetValues(enumType);

        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();

        foreach (System.Object o in options) {
            data.Add(new TMP_Dropdown.OptionData(o.ToString()));
        }

        return data;
    }
}
