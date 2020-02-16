using System;
using DefaultNamespace;
using UnityEngine;

namespace backend.threat_modelling {
    [CreateAssetMenu (fileName = "Option Set", menuName = "Control Dropdown Options")]
    public class Control_Dropdown_Option_Set : ScriptableObject {

        public ControlDropdownOptionSets setName = ControlDropdownOptionSets.None;
        public Control_Dropdown_Option[] options;

    }
}