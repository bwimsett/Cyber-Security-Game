using DefaultNamespace.node;
using TMPro;
using UnityEngine;

namespace gui.control_settings {
    public class ControlSettings_Field : MonoBehaviour{
        public TextMeshProUGUI fieldTitle;
        protected NodeField nodeField;
        
        public void SetNodeField(NodeField nodeField) {
            this.nodeField = nodeField;
            Initialise();
        }

        protected virtual void Initialise() {
            
        }
        
        public virtual void Refresh() {
            
        }

        public virtual void OnValueChanged() {
            
        }
     
        
    }
}