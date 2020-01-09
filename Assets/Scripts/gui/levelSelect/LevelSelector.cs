using System.Collections;
using System.Collections.Generic;
using backend.level;
using gui.levelSelect;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public LevelSet[] levelSets;

    public GameObject levelSetComponentPrefab;
    public Transform levelGroupTransform;
    private LevelSetComponent[] levelSetComponents;
    
    void Start() {
        Initialise();
    }

    private void Initialise() {
        GenerateSets();
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

    public void Refresh() {
        GenerateSets();
    }

}
