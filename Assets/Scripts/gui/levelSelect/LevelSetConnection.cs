using UnityEngine;

namespace gui.levelSelect {
    public class LevelSetConnection : MonoBehaviour{

        public LineRenderer lineRenderer;

        private LevelSetMedal start;
        private LevelSetMedal end;

        public void SetMedals(LevelSetMedal start, LevelSetMedal end) {
            this.start = start;
            this.end = end;
            Refresh();
        }

        public void Refresh() {
            Color startColor = start.GetMedalColor();
            Color endColor = end.GetMedalColor();

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }

    }
}