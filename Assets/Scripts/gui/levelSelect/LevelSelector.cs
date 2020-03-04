using System.Collections;
using System.Collections.Generic;
using backend.level;
using DefaultNamespace;
using gui.levelSelect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;

public class LevelSelector : MonoBehaviour {

    public TextMeshProUGUI tokenTotal;
    public TextMeshProUGUI percentageCompletion;
    
    public LevelSet[] levelSets;

    public GameObject levelSetComponentPrefab;
    public Transform levelGroupTransform;
    private LevelSetComponent[] levelSetComponents;

    public float levelGroupMoveSpeed;
    private int currentGroup = 0; // The group currently shown most clearly on screen
    private Vector3 targetPosition;
    private float levelGroupWidth = 1300;
    
    public Button rightButton;
    public Button leftButton;

    [Range(0,255)]
    public int opacity;

    void Update() {
        foreach (LevelSetComponent c in levelSetComponents) {
            c.UpdateConnectionOpacity(opacity);
        }
        UpdatePosition();
    }
    
    void Start() {
        RefreshTargetPosition();
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

    public void RefreshTargetPosition() {
        float targetx = -levelGroupWidth * currentGroup;
        targetPosition = new Vector3(targetx, 0, 0);
    }

    public void UpdatePosition() {
        Vector3 position = levelGroupTransform.localPosition;
        float distanceFromTarget = Vector3.Distance(position, targetPosition);

        if (distanceFromTarget < 1f) {
            levelGroupTransform.localPosition = targetPosition;
            return;
        }

        levelGroupTransform.localPosition = Vector3.Lerp(position, targetPosition, levelGroupMoveSpeed*Time.deltaTime);
    }
    
    public void Refresh() {
        GenerateSets();
        RefreshTotals();
    }

    public void NextGroup() {
        currentGroup++;
        RefreshTargetPosition();

        if (currentGroup >= levelSets.Length - 1) {
            rightButton.gameObject.SetActive(false);
        }

        if (currentGroup > 0) {
            leftButton.gameObject.SetActive(true);
        }
    }

    public void PrevGroup() {
        currentGroup--;
        RefreshTargetPosition();
        
        if (currentGroup == 0) {
            leftButton.gameObject.SetActive(false);
        }

        if (currentGroup < levelSets.Length - 1) {
            rightButton.gameObject.SetActive(true);
        }
    }
    
    

}
