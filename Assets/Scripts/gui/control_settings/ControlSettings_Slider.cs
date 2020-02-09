using System.Net.Mime;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace gui.control_settings {
    public class ControlSettings_Slider : ControlSettings_Field {
        public Slider slider;
        public Image sliderPointer;
        public Gradient pointerGradient;

        protected override void Initialise() {
            fieldTitle.text = nodeField.GetFieldTitle();
            slider.minValue = nodeField.GetMinValue();
            slider.maxValue = nodeField.GetMaxValue();
            slider.value = (int) nodeField.GetValue();
            slider.interactable = !nodeField.isReadOnly() || GameManager.currentLevel.IsEditMode();
        }

        public override void Refresh() {
            sliderPointer.color = pointerGradient.Evaluate(slider.normalizedValue);
        }

        public override void OnValueChanged() {
            Refresh();
            nodeField.SetValue(Mathf.RoundToInt(slider.value));
        }
    }
}