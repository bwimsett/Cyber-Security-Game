using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace gui {
    public class LevelNameText : MonoBehaviour {
        private string text;
        
        public TextMeshProUGUI textField;
        public int lettersUnscrambledPerIteration;
        [Range(0,1)]
        public float scramblePercentage;
        [Range(0,100)]
        public int randomCharacterPercentage;

        public Animator animator;

        private string scrambleCharacters = "?!><&$#@~+\\/";

        private string[] scrambledStrings;

        void Update() {
            int numberOfScrambleStrings = scrambledStrings.Length-1;
            int percentagePos = Mathf.FloorToInt(scramblePercentage * numberOfScrambleStrings);

            textField.text = scrambledStrings[percentagePos];
        }

        public void SetText(string newText) {
            text = newText;
            GenerateScrambledLevelTitleStrings();
            animator.SetTrigger("scramble");
        }
        
        void Awake() {
            text = GameManager.levelName;
            GenerateScrambledLevelTitleStrings();
        }

        public void GenerateScrambledLevelTitleStrings() {
            List<string> scrambledStrings = new List<string>();
            
            // Create a list of all the letters that are scrambled
            List<int> scrambledLetters = new List<int>();
            List<int> unscrambledLetters = new List<int>();
            
            string currentScramble = Scramble(text);
            scrambledStrings.Add(currentScramble);

            for (int i = 0; i < currentScramble.Length; i++) {
                scrambledLetters.Add(i);
            }

            // While there are still letters to unscramble
            while (scrambledLetters.Count > 0) {
                
                // Choose x to unscramble
                for (int i = 0; i < lettersUnscrambledPerIteration; i++) {
                    // Choose a position in scrambled letters
                    int chosenPos = Random.Range(0, scrambledLetters.Count);
                    // Add that position to the list of unscrambled letters
                    unscrambledLetters.Add(scrambledLetters[chosenPos]);
                    // Remove that position from the list of scrambled letters
                    scrambledLetters.RemoveAt(chosenPos);
                    
                    // Check if scrambledLetters.count == 0
                    if (scrambledLetters.Count == 0) {
                        break;
                    }
                }

                // Scramble everything
                char[] startingPoint = Scramble(currentScramble).ToCharArray();
                
                // Change all unscrambled letters to their original character
                for (int i = 0; i < unscrambledLetters.Count; i++) {
                    startingPoint[unscrambledLetters[i]] = text[unscrambledLetters[i]];
                }
                
                // Add string to list
                scrambledStrings.Add(startingPoint.ArrayToString());
            }

            this.scrambledStrings = scrambledStrings.ToArray();
        }

        public string Scramble(string input) {
            if (input == null) {
                return "";
            }
            
            List<char> lettersInName = new List<char>();
            
            foreach (char c in input) {
                lettersInName.Add(c);
            }

            string output = "";

            while (lettersInName.Count > 0) {
                int position = Random.Range(0, lettersInName.Count);
                char letter = lettersInName[position];
                bool changeCharacter = Random.Range(0, 100) < randomCharacterPercentage;
                if (changeCharacter) {
                    letter = scrambleCharacters[Random.Range(0, scrambleCharacters.Length)];
                }
                output += letter;
                lettersInName.RemoveAt(position);
            }

            return output;
        }

        
        

    }
}