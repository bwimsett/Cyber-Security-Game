using System;
using System.Collections;
using System.Collections.Generic;
using backend.threat_modelling;
using gui.control_settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettings_Tickbox : ControlSettings_Field {

    public GameObject tickbox_prefab;
    public Transform tickboxContainer;
    private Control_Toggle[] tickboxes;
    private char[] bitmask;

    protected override void Initialise() {
        tickboxes = new Control_Toggle[0];
        fieldTitle.text = nodeField.GetFieldTitle();
        GenerateOptions();
        bitmask = (char[])nodeField.GetValue();
        SetValue();
    }

    private void ClearOptions() {
        if (tickboxes.Length == 0) {
            return;
        }

        foreach (Control_Toggle t in tickboxes) {
            Destroy(t.gameObject);
        }
        
        tickboxes = new Control_Toggle[0];
    }

    public void OnTickboxValueChanged(int pos, bool value) {
        char maskValue = '0';

        if (value) {
            maskValue = '1';
        }

        bitmask[pos] = maskValue;
        
        nodeField.SetValue(bitmask);

        Debug.Log("Bitmask updated: " + GetBitmaskString());
    }

    private void SetValue() {
        for(int i = 0; i < tickboxes.Length; i++) {
            bool value = bitmask[i] == '1';
            tickboxes[i].SetValue(value);
        }
    }
    
    private void GenerateOptions() {
        ClearOptions();
        Control_Dropdown_Option_Set optionSet = nodeField.GetOptionSet();
        Control_Dropdown_Option[] options = optionSet.options;

        tickboxes = new Control_Toggle[options.Length];
        bitmask = new char[options.Length];

        for(int i = 0; i < tickboxes.Length; i++) {
            tickboxes[i] = Instantiate(tickbox_prefab, tickboxContainer).GetComponent<Control_Toggle>();
            tickboxes[i].Initialise(i, options[i].name+" ("+options[i].cost+")", this);
            bitmask[i] = '0';
        }
    }

    private string GetBitmaskString() {
        string output = "";
        
        foreach (char c in bitmask) {
            output += c;
        }

        return output;
    }
}
