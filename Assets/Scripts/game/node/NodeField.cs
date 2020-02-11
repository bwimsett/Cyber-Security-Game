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

        private ControlDropdownOptionSets optionSet = ControlDropdownOptionSets.None;

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

        public NodeField(string fieldTitle, int value, ControlDropdownOptionSets optionSet) {
            this.fieldTitle = fieldTitle;
            this.value = value;
            readOnly = false;
            this.optionSet = optionSet;

            fieldType = NodeFieldType.enumerable;
        }
        
        private void EstablishFieldType() {            
            if (value is int) {
                fieldType = NodeFieldType.integer;
            }

            if (value is int && optionSet != ControlDropdownOptionSets.None) {
                fieldType = NodeFieldType.enumerable;
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

        public Control_Dropdown_Option_Set GetOptionSet() {
            return GameManager.levelScene.nodeManager.GetControlOptionSet(optionSet);
        }

    }
}