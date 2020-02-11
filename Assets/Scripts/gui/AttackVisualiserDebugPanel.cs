
using backend;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackVisualiserDebugPanel : MonoBehaviour {

    public Button next;
    public Button previous;
    public TextMeshProUGUI text;

    public GameObject attackOptionPrefab;
    private AttackOption[] attackOptions;
    public Transform attackOptionsContainer;
    
    private Threat[] succesfulAttacks;
    private int currentAttack;

    void Start() {
        attackOptions = new AttackOption[0];
    }
    
    public void OnEnable() {
        Refresh();
    }
    
    public void SetAttacks(Threat[] succesfulAttacks) {  
        this.succesfulAttacks = succesfulAttacks;
        currentAttack = 0;
        GenerateAttackOptions();
        Refresh();
    }

    private void GenerateAttackOptions() {
        ClearAttackOptions();
        attackOptions = new AttackOption[succesfulAttacks.Length];

        for (int i = 0; i < attackOptions.Length; i++) {
            attackOptions[i] = Instantiate(attackOptionPrefab, attackOptionsContainer).GetComponent<AttackOption>();
            attackOptions[i].SetThreat(succesfulAttacks[i], i);
        }
    }

    private void ClearAttackOptions() {
        if (attackOptions.Length == 0) {
            return;
        }
        
        foreach (AttackOption a in attackOptions) {
            Destroy(a.gameObject);
        }

        attackOptions = null;
    }

    public void Next() {
        currentAttack++;
        Refresh();
    }

    public void Prev() {
        currentAttack--;
        Refresh();
    }

    public void Refresh() {
        if (succesfulAttacks == null) {
            next.enabled = false;
            previous.enabled = false;
            text.text = "No attacks";
            return;
        }
        
        if (currentAttack == succesfulAttacks.Length-1) {
            next.enabled = false;
        }
        else {
            next.enabled = true;
        }

        
        if (currentAttack == 0) {
            previous.enabled = false;
        }
        else {
            previous.enabled = true;
        }

        if (succesfulAttacks.Length > 0 && gameObject.activeSelf) {
            GameManager.levelScene.attackVisualiser.VisualiseAttack(succesfulAttacks[currentAttack]);
        }
        else {
            GameManager.levelScene.attackVisualiser.ClearVisualisation();
        }

        text.text = "Attack " + (currentAttack + 1) + "/" + succesfulAttacks.Length;
    }

}
