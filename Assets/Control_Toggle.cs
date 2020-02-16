using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control_Toggle : MonoBehaviour {
    public Toggle toggle;
    public TextMeshProUGUI titleText;
    private int pos;
    private ControlSettings_Tickbox parent;
    
    
    public void Initialise(int pos, string title, ControlSettings_Tickbox parent) {
        titleText.text = title;
        this.pos = pos;
        this.parent = parent;
    }

    public void SetValue(bool value) {
        toggle.isOn = value;
    }
    
    public void OnValueChanged() {
        parent.OnTickboxValueChanged(pos, toggle.isOn);
    }
}
