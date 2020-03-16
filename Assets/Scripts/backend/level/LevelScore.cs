using System.Collections.Generic;
using System.Diagnostics;
using backend.level_serialization;
using DefaultNamespace;

namespace backend.level {
    [System.Serializable]
    public class LevelScore {

        private const int TOKENS_GOLD = 3;
        private const int TOKENS_SILVER = 2;
        private const int TOKENS_BRONZE = 1;

        private const int SCORE_PER_THREAT_FAILED = -350;
        private const int SCORE_PER_HEALTHPOINT = 1;
        private const int SCORE_PER_BUDGETPOINT = 250;
        private const int SCORE_PER_CONTROL_TYPE = 350;
        private const int SCORE_PER_THREAT_DEFENDED = 500;
        private const int SCORE_FIRST_ATTEMPT = 750;

        public int score_healthpoints;
        public int score_budgetremaining;
        public int score_controltypes;
        public int score_threatsdefended;
        public int score_threatsfailed;
        public int score_firstattempt;

        public Medal medal;

        public LevelScore() {
            medal = Medal.None;
        }

        public LevelScore(Threat[] successfulThreats, Threat[] failedThreats) {
            CalculateScore(successfulThreats, failedThreats);
        }

        public void CalculateScore(Threat[] successfulThreats, Threat[] failedThreats) {
            score_budgetremaining = CalculateScore_Budget(successfulThreats);
            score_controltypes = CalculateScore_ControlType();
            score_threatsdefended = CalculateScore_ThreatsDefended(failedThreats);
            score_threatsfailed = CalculateScore_FailedThreats(successfulThreats);
            score_firstattempt = CalculateScore_FirstAttempt();

            Medal newMedal = ClassifyMedal(GetTotalScore());

            // Only overwrite medal if it is better than previous attempts
            if (MedalToInt(newMedal) > MedalToInt(medal)) {
                medal = newMedal;
            }
        }

        private int CalculateScore_FailedThreats(Threat[] successfulThreats) {
            return successfulThreats.Length * SCORE_PER_THREAT_FAILED;
        }
        
        private int CalculateScore_Budget(Threat[] successfulThreats) {
            int budgetRemaining = GameManager.currentLevel.GetRemainingBudget();

            int score = budgetRemaining * SCORE_PER_BUDGETPOINT;

            if (successfulThreats.Length > 0) {
                score = 0;
            }
            
            return score;
        }

        private int CalculateScore_ControlType() {
            List<NodeDefinition> controlTypes = new List<NodeDefinition>();

            foreach (Node n in GameManager.currentLevel.nodes) {
                NodeDefinition def = n.nodeObject.GetNodeDefinition();

                bool isControl = def.nodeFamily != NodeFamily.Base && def.nodeFamily != NodeFamily.Zone;
                
                if (!controlTypes.Contains(def) && isControl) {
                    controlTypes.Add(def);
                }
            }

            return controlTypes.Count * SCORE_PER_CONTROL_TYPE;
        }

        public Medal CalculateMedalFromCurrentScore() {
            return ClassifyMedal(GetTotalScore());
        }
        
        private int CalculateScore_ThreatsDefended(Threat[] failedThreats) {

            // Ignore authorised access block as a scoring threat
            int validThreats = 0;

            foreach (Threat t in failedThreats) {
                if (t.threatType == ThreatType.Authorised_Access_Block) {
                    continue;
                }

                validThreats++;
            }
            
            return validThreats * SCORE_PER_THREAT_DEFENDED;
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
            return score_budgetremaining + score_controltypes + score_threatsdefended + score_threatsfailed +score_firstattempt;
        }

        public int GetTokens() {
            switch (medal) {
                case Medal.Bronze: return TOKENS_BRONZE;
                case Medal.Silver: return TOKENS_SILVER;
                case Medal.Gold: return TOKENS_GOLD;
            }

            return 0;
        }

        public int GetMaxTokens() {
            return TOKENS_GOLD;
        }
        
        public static int MedalToInt(Medal medal) {
            switch (medal) {
                case Medal.Gold: return 3;
                case Medal.Silver: return 2;
                case Medal.Bronze: return 1;
                case Medal.None: return 0;
            }

            return 0;
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