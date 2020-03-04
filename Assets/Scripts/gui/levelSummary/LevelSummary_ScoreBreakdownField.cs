using TMPro;
using UnityEngine;

namespace gui.levelSummary {
    public class LevelSummary_ScoreBreakdownField : MonoBehaviour{
        public TextMeshProUGUI title;
        public TextMeshProUGUI value;
        public Color negativeColor;
        public Color positiveColor;
        public int score;

        public void setScore(string title, int score, bool displayZeroScore) {
            this.title.text = title;
            this.score = score;

            if (score < 0) {
                value.color = negativeColor;
                value.text = "" + score;
            }
            else {
                value.color = positiveColor;
                value.text = "+" + score;
            }
            
            if (score == 0 && !displayZeroScore) {
                gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(true);
            }
            
        }
    }
}