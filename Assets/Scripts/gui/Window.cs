using System;
using TMPro;
using UnityEngine;

namespace gui {
    public class Window : MonoBehaviour{
        public TextMeshProUGUI title;

        public void SetTitle(String title) {
            this.title.text = title;
        }
    }
}