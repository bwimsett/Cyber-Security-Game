using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {

    public Animator animator;
    public Node[] nodeTypes;
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
        
        options = new ControlsMenu_Option[nodeTypes.Length];

        for (int i = 0; i < nodeTypes.Length; i++) {
            options[i] = Instantiate(optionPrefab, optionsContainer).GetComponent<ControlsMenu_Option>();
            options[i].SetNode(nodeTypes[i]);
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
