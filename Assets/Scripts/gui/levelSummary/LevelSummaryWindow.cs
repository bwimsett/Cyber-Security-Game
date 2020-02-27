﻿using System.Collections;
using System.Collections.Generic;
using backend;
using backend.level;
using DefaultNamespace;
using gui;
using gui.levelSummary;
using TMPro;
using UnityEngine;

public class LevelSummaryWindow : Window {

    public TextMeshProUGUI scoreText;
    public LevelSummary_ScoreBreakdownField[] scoreBreakdown_Fields;
    public LevelSummary_Medals medals;
    public GameObject threatSummaryFieldPrefab;
    public Transform threatSummaryContainer;
    private LevelSummary_ThreatSummaryField[] threatSummaryFields;

    
    public void Refresh() {
        gameObject.SetActive(true);       
        title.text = "Level Complete!";
        scoreText.text = "Score: " + GameManager.currentLevelScore.GetTotalScore();
        GenerateScoreBreakdown();
        GenerateThreatSummaries();
        medals.SetMedal(GameManager.currentLevelScore.medal);
    }

    private void GenerateScoreBreakdown() {
        LevelScore levelScore = GameManager.currentLevelScore;
        
        scoreBreakdown_Fields[0].setScore("Health: ", levelScore.score_healthpoints);
        scoreBreakdown_Fields[1].setScore("Budget: ", levelScore.score_budgetremaining);
        scoreBreakdown_Fields[2].setScore("Controls: ",  levelScore.score_controltypes);
        scoreBreakdown_Fields[3].setScore("Threats: ", levelScore.score_threatsdefended);
        scoreBreakdown_Fields[4].setScore("First Attempt: ", levelScore.score_firstattempt);
        scoreBreakdown_Fields[5].setScore("Total: ", levelScore.GetTotalScore());
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
            threatSummaryFields[i].SetThreat(threats[i]);
        }
    }

}
