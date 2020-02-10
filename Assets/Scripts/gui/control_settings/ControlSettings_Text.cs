using System.Diagnostics;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace gui.control_settings {
    public class ControlSettings_Text : ControlSettings_Field {

        public TextMeshProUGUI textField;
        public TMP_InputField inputField;

        protected override void Initialise() {
            fieldTitle.text = nodeField.GetFieldTitle();
            textField.autoSizeTextContainer = true;
            textField.text = (string)nodeField.GetValue();
            textField.ForceMeshUpdate(true);
            //textField.rectTransform.sizeDelta = new Vector2(textField.rectTransform.rect.width, textField.maxHeight);
        }

        public override void Refresh() {
            base.Refresh();
        }

        public override void OnValueChanged() {
            textField.text = inputField.text;
            nodeField.SetValue(textField.text);
            inputField.gameObject.SetActive(false);
            textField.gameObject.SetActive(true);
        }

        public void OnClick() {
            if (nodeField.isReadOnly() && !GameManager.currentLevel.IsEditMode()) {
                return;
            }
            
            textField.gameObject.SetActive(false);
            inputField.gameObject.SetActive(true);
            inputField.text = textField.text;
        }
    }
}