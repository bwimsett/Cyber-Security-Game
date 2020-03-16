using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {

    public Animator animator;
    public GameObject optionPrefab;
    public Transform optionsContainer;
    private ControlsMenu_Option[] options;
    public CanvasGroup canvasGroup;
    
    void Start() {
        RefreshOptions();
    }
    
    public void RefreshOptions() {
        if (options != null) {
            ClearOptions();
        }

        NodeDefinition[] nodeDefinitions = GameManager.levelScene.nodeManager.nodeDefinitions;
        options = new ControlsMenu_Option[nodeDefinitions.Length];

        bool editMode = GameManager.currentLevel.IsEditMode();
        
        for (int i = 0; i < nodeDefinitions.Length; i++) {

            NodeDefinition n = nodeDefinitions[i];

            bool isControl = n.nodeFamily != NodeFamily.Base && n.nodeFamily != NodeFamily.Zone;

            
            
            options[i] = Instantiate(optionPrefab, optionsContainer).GetComponent<ControlsMenu_Option>();
            options[i].SetNode(nodeDefinitions[i]);
            
            if (!editMode && !isControl) {
                options[i].gameObject.SetActive(false);
            }
        }
        
        SortOptions();
    }

    public void SortOptions() {
        bool sorted = true;

        do {
            
            sorted = true;
            
            for (int i = 0; i < options.Length - 1; i++) {
                // Swap this option with the next if it is more expensive
                if (options[i].GetNode().nodeUnlockTokens > options[i + 1].GetNode().nodeUnlockTokens) {
                    ControlsMenu_Option thisOption = options[i];
                    options[i] = options[i + 1];
                    options[i + 1] = thisOption;
                    sorted = false;
                }
            }
            
        } while (!sorted);
        
        // Now sort them in the hierarchy
        for (int i = 0; i < options.Length; i++) {
            options[i].transform.SetSiblingIndex(i);
        }
    }

    public void ClearOptions() {
        foreach (ControlsMenu_Option option in options) {
            if (option) {
                Destroy(option.gameObject);
            }
        }
    }

    // Hides or shows the menu    
    public void Toggle() {
        animator.SetTrigger("Trigger");
        animator.SetBool("Hidden", !animator.GetBool("Hidden"));
    }

    public void Hide() {
        if (!animator.GetBool("Hidden")) {
            animator.SetTrigger("Trigger");
            animator.SetBool("Hidden", true);
        }
    }

    public void SetInteractable(bool interactable) {
        canvasGroup.interactable = interactable;
    }
    
}
