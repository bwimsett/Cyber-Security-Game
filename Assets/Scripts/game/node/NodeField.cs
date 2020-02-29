using System;
using backend.threat_modelling;

namespace DefaultNamespace.node {
    [Serializable]
    public class NodeField {

        private NodeFieldType fieldType;
        private string fieldTitle;
        private object value;
        private bool readOnly;

        private int minValue;
        private int maxValue;

        private ControlDropdownOptionSets optionSetName = ControlDropdownOptionSets.None;
        [NonSerialized]
        private Control_Dropdown_Option_Set optionSet;

        private bool colourOptions;

        public NodeField(string fieldTitle, bool readOnly, object value) {
            this.fieldTitle = fieldTitle;
            this.value = value;
            this.readOnly = readOnly;
            
            EstablishFieldType();
        }
        
        public NodeField(string fieldTitle, string value) {
            this.fieldTitle = fieldTitle;
            this.value = value;
            readOnly = true;
            fieldType = NodeFieldType.text;
        }
        
        public NodeField(string fieldTitle, bool readOnly, int value, int minValue, int maxValue) {
            this.fieldTitle = fieldTitle;
            this.value = value;
            this.readOnly = readOnly;
            this.minValue = minValue;
            this.maxValue = maxValue;

            fieldType = NodeFieldType.integer_range;
        }

        public NodeField(string fieldTitle, int value, bool colourOptions, ControlDropdownOptionSets optionSetName) {
            this.fieldTitle = fieldTitle;
            this.value = value;
            readOnly = false;
            this.optionSetName = optionSetName;
            this.colourOptions = colourOptions;
            fieldType = NodeFieldType.enumerable_single;
        }
        
        public NodeField(string fieldTitle, ControlDropdownOptionSets optionSetName) {
            this.fieldTitle = fieldTitle;
            value = new char[GameManager.levelScene.nodeManager.GetControlOptionSet(optionSetName).options.Length];
            readOnly = false;
            this.optionSetName = optionSetName;
            fieldType = NodeFieldType.enumerable_many;
        }
        
        public NodeField(string fieldTitle, Control_Dropdown_Option_Set optionSet) {
            this.fieldTitle = fieldTitle;
            value = new char[optionSet.options.Length];
            readOnly = false;
            this.optionSet = optionSet;
            fieldType = NodeFieldType.enumerable_many;
        }
        
        private void EstablishFieldType() {            
            if (value is int) {
                fieldType = NodeFieldType.integer;
            }

            if (value is string) {
                fieldType = NodeFieldType.text;
            }
        }

        public NodeFieldType GetFieldType() {
            return fieldType;
        }

        public string GetFieldTitle() {
            return fieldTitle;
        }

        public bool isReadOnly() {
            return readOnly;
        }

        public bool IsColourOptions() {
            return colourOptions;
        }

        public int GetMinValue() {
            return minValue;
        }

        public int GetMaxValue() {
            return maxValue;
        }

        public object GetValue() {
            return value;
        }

        public void SetValue(object value) {
            this.value = value;
        }

        public void SetOptionSet(Control_Dropdown_Option_Set optionSet) {
            this.optionSet = optionSet;
        }

        public ControlDropdownOptionSets GetOptionSetName() {
            return optionSetName;
        }
        
        public Control_Dropdown_Option_Set GetOptionSet() {
            if (optionSetName != ControlDropdownOptionSets.None) {
                return GameManager.levelScene.nodeManager.GetControlOptionSet(optionSetName);
            }
   
            return optionSet;
        }

    }
}