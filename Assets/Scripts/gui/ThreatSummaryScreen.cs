using System.Collections;
using System.Collections.Generic;
using backend;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreatSummaryScreen : MonoBehaviour {

    public TextMeshProUGUI threatName;
    public TextMeshProUGUI threatPos;
    private Threat[] threats;
    private int currentThreatIndex;
    public Button summaryButton;

    public TextMeshProUGUI nextButtonText;
    public Button nextButton;
    public Button prevButton;

    public void SetThreats(Threat[] threats) {
        if (threats.Length == 0) {
            GameManager.levelScene.guiManager.DisplayLevelSummary();
            return;
        }

        summaryButton.enabled = true;
        
        currentThreatIndex = 0;
        this.threats = threats;
        RefreshButtons();
        ViewThreat();
    }

    private void ViewThreat() {
        if (currentThreatIndex >= threats.Length - 1) {
            currentThreatIndex = threats.Length - 1;
        }

        if (currentThreatIndex < 0) {
            currentThreatIndex = 0;
        }

        threatPos.text = currentThreatIndex + 1 + "/" + threats.Length;
        threatName.text =
            GameManager.levelScene.threatManager.threatSummariser.GetThreatName(threats[currentThreatIndex]);
        
        GameManager.levelScene.attackVisualiser.AnimateAttack(threats[currentThreatIndex]);
    }

    public void NextThreat() {
        currentThreatIndex++;

        if (currentThreatIndex >= threats.Length) {
            GameManager.levelScene.guiManager.DisplayLevelSummary();
            GameManager.levelScene.guiManager.controlsMenu.Hide();
            GameManager.levelScene.guiManager.controlsMenu.SetInteractable(false);
            summaryButton.enabled = false;
        }
        else {
            ViewThreat();
        }
        
        RefreshButtons();
    }

    public void SetThreatIndex(int index) {
        if (index < 0 || index >= threats.Length) {
            return;
        }

        currentThreatIndex = index;
        ViewThreat();
    }

    public void PrevThreat() {
        currentThreatIndex--;
        ViewThreat();
        RefreshButtons();
    }
    
    private void RefreshButtons() {
        if (currentThreatIndex == 0) {
            prevButton.enabled = false;
        }

        if (currentThreatIndex > 0) {
            prevButton.enabled = true;
        }

        if (currentThreatIndex >= threats.Length - 1) {
            nextButtonText.text = "summary >";
        }

        if (currentThreatIndex < threats.Length - 1) {
            nextButtonText.text = "next >";
        }
    }
    
    
}
