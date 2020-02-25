using System.Collections;
using System.Collections.Generic;
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
    
    private LevelScore levelScore;


    public void SetScore(LevelScore levelScore) {
        this.levelScore = levelScore;
        medals.SetMedal(levelScore.medal);
        
        Refresh();
    }

    public void Refresh() {
        gameObject.SetActive(true);
        
        title.text = "Level Complete!";
        scoreText.text = "Score: " + levelScore.GetTotalScore();
        GenerateScoreBreakdown();
    }

    private void GenerateScoreBreakdown() {
        scoreBreakdown_Fields[0].setScore("Health: ", levelScore.score_healthpoints);
        scoreBreakdown_Fields[1].setScore("Budget: ", levelScore.score_budgetremaining);
        scoreBreakdown_Fields[2].setScore("Controls: ",  levelScore.score_controltypes);
        scoreBreakdown_Fields[3].setScore("Threats: ", levelScore.score_threatsdefended);
        scoreBreakdown_Fields[4].setScore("First Attempt: ", levelScore.score_firstattempt);
        scoreBreakdown_Fields[5].setScore("Total: ", levelScore.GetTotalScore());
    }

}
