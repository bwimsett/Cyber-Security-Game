using System.Collections;
using System.Collections.Generic;
using backend.level;
using DefaultNamespace;
using gui.levelSelect;
using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public TextMeshProUGUI tokenTotal;
    public TextMeshProUGUI percentageCompletion;
    
    public LevelSet[] levelSets;

    public GameObject levelSetComponentPrefab;
    public Transform levelGroupTransform;
    private LevelSetComponent[] levelSetComponents;

    [Range(0,255)]
    public int opacity;

    void Update() {
        foreach (LevelSetComponent c in levelSetComponents) {
            c.UpdateConnectionOpacity(opacity);
        }
    }
    
    void Start() {
        Initialise();
    }

    private void Initialise() {
        Refresh();
    }

    private void Clear() {
        if (levelSetComponents == null) {
            return;
        }
        
        foreach (LevelSetComponent c in levelSetComponents) {
            Destroy(c.gameObject);
        }
    }

    private void GenerateSets() {
        Clear();

        levelSetComponents = new LevelSetComponent[levelSets.Length];
        
        for (int i = 0; i < levelSets.Length; i++) {
            levelSetComponents[i] = Instantiate(levelSetComponentPrefab, levelGroupTransform).GetComponent<LevelSetComponent>();
            levelSetComponents[i].SetLevels(levelSets[i], i);
        }
    }

    private void RefreshTotals() {
        tokenTotal.text = GameManager.currentSaveGame.GetTokens()+" Tokens";
        percentageCompletion.text = GameManager.currentSaveGame.GetPercentageCompletion() + "% Complete";
    }

    public void Refresh() {
        GenerateSets();
        RefreshTotals();
    }

}
