using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {

    public Animator animator;
    public GameObject optionPrefab;
    public Transform optionsContainer;
    private ControlsMenu_Option[] options;
    
    void Start() {
        RefreshOptions();
    }
    
    public void RefreshOptions() {
        if (options != null) {
            ClearOptions();
        }

        NodeDefinition[] nodeDefinitions = GameManager.levelScene.nodeManager.nodeDefinitions;
        options = new ControlsMenu_Option[nodeDefinitions.Length];

        for (int i = 0; i < nodeDefinitions.Length; i++) {
            options[i] = Instantiate(optionPrefab, optionsContainer).GetComponent<ControlsMenu_Option>();
            options[i].SetNode(nodeDefinitions[i]);
        }
    }

    public void ClearOptions() {
        foreach (ControlsMenu_Option option in options) {
            Destroy(option.gameObject);
        }
    }

    // Hides or shows the menu    
    public void Toggle() {
        animator.SetTrigger("Trigger");
        animator.SetBool("Hidden", !animator.GetBool("Hidden"));
    }
    
}
