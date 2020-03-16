using System.Collections;
using System.Collections.Generic;
using backend;
using backend.level;
using DefaultNamespace;
using gui;
using gui.levelSummary;
using GameAnalyticsSDK.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSummaryWindow : Window {

    public TextMeshProUGUI scoreText;
    public LevelSummary_ScoreBreakdownField[] scoreBreakdown_Fields;
    public LevelSummary_Medals medals;
    public GameObject threatSummaryFieldPrefab;
    public Transform threatSummaryContainer;
    private LevelSummary_ThreatSummaryField[] threatSummaryFields;
    public Animator animator;

    public Button nextLevelButton;
    public TextMeshProUGUI nextLevelButtonText;

    public void SetVisible(bool visible) {
        if (visible) {
            animator.SetTrigger("swipein");
        }
    }
    
    public void Refresh() {
        gameObject.SetActive(true);       
        title.text = "Level Complete!";
        scoreText.text = "Score: " + GameManager.currentLevelScore.GetTotalScore();
        GenerateScoreBreakdown();
        GenerateThreatSummaries();
        medals.SetMedal(GameManager.currentLevelScore.CalculateMedalFromCurrentScore());

        nextLevelButton.interactable = true;

        int tokensRequired = GameManager.selectedLevelDescription.nextLevel.unlockTokens -
                             GameManager.currentSaveGame.GetTokens();
        nextLevelButtonText.text = "Next";
        
        // Update next button
        if (tokensRequired > 0) {
            nextLevelButton.interactable = false;
            string buttonText = tokensRequired + " Token";
            if (tokensRequired > 1) {
                buttonText += "s";
            }
            nextLevelButtonText.text = buttonText;
        }
    }

    private void GenerateScoreBreakdown() {
        LevelScore levelScore = GameManager.currentLevelScore;
        
        scoreBreakdown_Fields[0].setScore("Health: ", levelScore.score_healthpoints, false);
        scoreBreakdown_Fields[1].setScore("Budget: ", levelScore.score_budgetremaining, false);
        scoreBreakdown_Fields[2].setScore("Controls: ",  levelScore.score_controltypes, false);
        scoreBreakdown_Fields[3].setScore("Threats: ", levelScore.score_threatsdefended, false);
        scoreBreakdown_Fields[4].setScore("Threats Failed: ", levelScore.score_threatsfailed, false);
        scoreBreakdown_Fields[5].setScore("Total: ", levelScore.GetTotalScore(), true);
    }

    public void GenerateThreatSummaries() {
        if (threatSummaryFields == null) {
            threatSummaryFields = new LevelSummary_ThreatSummaryField[0];
        }
        
        if (threatSummaryFields.Length > 0) {
            foreach (LevelSummary_ThreatSummaryField field in threatSummaryFields) {
                Destroy(field.gameObject);
            }
        }

        Threat[] threats = GameManager.levelScene.threatManager.GetThreats(ThreatStatus.Success);

        threatSummaryFields = new LevelSummary_ThreatSummaryField[threats.Length];

        for (int i = 0; i < threats.Length; i++) {
            threatSummaryFields[i] = Instantiate(threatSummaryFieldPrefab, threatSummaryContainer)
                .GetComponent<LevelSummary_ThreatSummaryField>();
            threatSummaryFields[i].SetThreat(threats[i], i);
        }
    }

    public void NextLevel() {
        AnalyticsManager.EndLevel(GameManager.currentLevelScore.GetTotalScore(), GameManager.currentLevelScore.medal);
        AnalyticsManager.StartLevel();
        
        GameManager.currentLevel.LoadLevel(GameManager.selectedLevelDescription.nextLevel);
            
        animator.SetTrigger("swipeout");
        GameManager.levelScene.guiManager.DisplayLevelScene(false);
        GameManager.levelScene.guiManager.levelNameText.textField.enabled = true;
        
        
    }

    public void Retry() {
        GameManager.levelScene.guiManager.DisplayLevelScene(true);
    }

}
