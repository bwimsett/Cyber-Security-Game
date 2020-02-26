using System.Collections.Generic;
using System.Diagnostics;
using backend.level_serialization;
using DefaultNamespace;

namespace backend.level {
    [System.Serializable]
    public class LevelScore {

        private const int SCORE_PER_HEALTHPOINT = 1;
        private const int SCORE_PER_BUDGETPOINT = 250;
        private const int SCORE_PER_CONTROL_TYPE = 350;
        private const int SCORE_PER_THREAT_DEFENDED = 500;
        private const int SCORE_FIRST_ATTEMPT = 750;

        public int score_healthpoints;
        public int score_budgetremaining;
        public int score_controltypes;
        public int score_threatsdefended;
        public int score_firstattempt;

        public Medal medal;

        public LevelScore() {
            medal = Medal.None;
        }

        public LevelScore(Threat[] failedThreats) {
            CalculateScore(failedThreats);
        }

        public void CalculateScore(Threat[] failedThreats) {
            score_budgetremaining = CalculateScore_Budget();
            score_controltypes = CalculateScore_ControlType();
            score_threatsdefended = CalculateScore_ThreatsDefended(failedThreats);
            score_firstattempt = CalculateScore_FirstAttempt();
            medal = ClassifyMedal(GetTotalScore());
        }

        private int CalculateScore_Budget() {
            int budgetRemaining = GameManager.currentLevel.GetBudget();
            return budgetRemaining * SCORE_PER_BUDGETPOINT;
        }

        private int CalculateScore_ControlType() {
            List<NodeDefinition> controlTypes = new List<NodeDefinition>();

            foreach (Node n in GameManager.currentLevel.nodes) {
                NodeDefinition def = n.nodeObject.GetNodeDefinition();

                bool isControl = def.nodeFamily != NodeFamily.Base;
                
                if (!controlTypes.Contains(def) && isControl) {
                    controlTypes.Add(def);
                }
            }

            return controlTypes.Count * SCORE_PER_CONTROL_TYPE;
        }

        private int CalculateScore_ThreatsDefended(Threat[] failedThreats) {
            return failedThreats.Length * SCORE_PER_THREAT_DEFENDED;
        }

        private int CalculateScore_FirstAttempt() {
            if (GameManager.currentLevel.GetAttempts() == 1) {
                return SCORE_FIRST_ATTEMPT;
            }

            return 0;
        }

        private Medal ClassifyMedal(int score) {
            Level currentLevel = GameManager.currentLevel;

            if (score >= currentLevel.GetMedalBoundary(Medal.Gold)) {
                return Medal.Gold;
            }

            if (score >= currentLevel.GetMedalBoundary(Medal.Silver)) {
                return Medal.Silver;
            }

            if (score >= currentLevel.GetMedalBoundary(Medal.Bronze)) {
                return Medal.Bronze;
            }

            return Medal.None;
        }

        public int GetTotalScore() {
            return score_budgetremaining + score_controltypes + score_threatsdefended + score_firstattempt;
        }
        
        public override string ToString() {
            string output = "";

            output += "Healthpoint score: " + score_healthpoints + "\n" +
                      "Budget remaining score: " + score_budgetremaining + "\n" +
                      "Control variation score: " + score_controltypes + "\n" +
                      "Threats defended score: " + score_threatsdefended + "\n" +
                      "First attempt score: " + score_firstattempt + "\n" +
                      "Total: " + GetTotalScore() + "\n" +
                      "Medal: " + medal;

            return output;
        }
    }
}