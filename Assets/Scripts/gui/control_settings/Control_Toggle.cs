using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control_Toggle : MonoBehaviour {
    public Toggle toggle;
    public TextMeshProUGUI titleText;
    private int pos;
    private ControlSettings_Tickbox parent;

    public TMP_InputField threatStrength;
    
    
    public void Initialise(int pos, string title, ControlSettings_Tickbox parent) {
        titleText.text = title;
        this.pos = pos;
        this.parent = parent;
    }

    public void SetValue(bool value) {
        toggle.isOn = value;
    }

    public void SetThreatStrength(int value) {
        if (!threatStrength) {
            return;
        }
        
        threatStrength.text = "" + value;
    }
    
    public void OnValueChanged() {
        parent.OnTickboxValueChanged(pos, toggle.isOn);
    }

    public void OnValueChanged_ThreatStrength() {
        int value = Int32.Parse(threatStrength.text);
        parent.OnTextFieldValueChanged(pos, value); 
    }
}
