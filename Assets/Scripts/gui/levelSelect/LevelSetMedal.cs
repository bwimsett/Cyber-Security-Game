using backend.level;
using backend.level_serialization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace gui.levelSelect {
    public class LevelSetMedal : MonoBehaviour {

        private LevelDescription levelDescription;
        public Image medalImage;

        public Image connector;

        private int levelId;
        
        public Sprite locked;
        public Sprite noMedal;
        public Sprite bronze;
        public Sprite silver;
        public Sprite gold;

        public Color noMedalColor;
        public Color bronzeColor;
        public Color silverColor;
        public Color goldColor;
        
        public void SetLevel(LevelDescription levelDescription, int levelId) {
            this.levelDescription = levelDescription;
            this.levelId = levelId;
            
            RefreshMedal();
        }

        public void OnClick() {
            Level level = new Level();
            
            LevelSerializer ls = new LevelSerializer();

            LevelSave levelSave = ls.GetLevelSave(levelDescription.levelFile);

            if (levelSave != null) {
                GameManager.selectedLevelSave = levelSave;
            }
            
            // Load level progress
            GameManager.currentLevelScore = GameManager.currentSaveGame.GetLevelScore(levelId);
            
            SceneManager.LoadScene(1);
        }

        public void RefreshMedal() {
            Medal medal = GameManager.currentSaveGame.GetLevelMedal(levelId);
            
            switch (medal) {
                case Medal.Locked: medalImage.sprite = locked; break;
                case Medal.None: medalImage.sprite = noMedal; break;
                case Medal.Bronze: medalImage.sprite = bronze; break;
                case Medal.Silver: medalImage.sprite = silver; break;
                case Medal.Gold: medalImage.sprite = gold; break;
            }
        }

        public Color GetMedalColor() {
            Medal medal = GameManager.currentSaveGame.GetLevelMedal(levelId);
            
            switch (medal) {
                case Medal.None: return noMedalColor;
                case Medal.Locked: return noMedalColor;
                case Medal.Bronze: return bronzeColor;
                case Medal.Silver: return silverColor;
                case Medal.Gold: return goldColor;
            }

            return Color.black;
        }
        
    }
}