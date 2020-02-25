using TMPro;
using UnityEngine;

namespace gui.levelSummary {
    public class LevelSummary_ScoreBreakdownField : MonoBehaviour{
        public TextMeshProUGUI title;
        public TextMeshProUGUI value;
        public int score;

        public void setScore(string title, int score) {
            this.title.text = title;
            this.score = score;
            this.value.text = "+" + score;

            if (score == 0) {
                gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(true);
            }
            
        }
    }
}