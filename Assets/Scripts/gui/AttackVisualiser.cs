using UnityEngine;

namespace backend {
    public class AttackVisualiser : MonoBehaviour{

        public Color attackColor;

        private Threat[] currentTrace;
        
        public void VisualiseAttack(Threat threat) {
            ClearVisualisation();
            
            currentTrace = threat.GetTrace();

            foreach (Threat t in currentTrace) {
                NodeObject nodeObject = t.GetNode().nodeObject;
                nodeObject.SetTempColor(attackColor);
                nodeObject.healthBar.SetHealth(t.GetNodeHealth(), nodeObject.GetNode().GetBehaviour().GetTotalHealth());
                nodeObject.healthBar.SetVisible(true);
            }    
            
            Debug.Log(threat.GetStringTrace());
        }

        public void ClearVisualisation() {
            if (currentTrace == null) {
                return;
            }
            
            if (currentTrace.Length == 0) {
                return;
            }
            
            foreach (Threat t in currentTrace) {
                t.GetNode().nodeObject.ResetColor();
                t.GetNode().nodeObject.healthBar.SetVisible(false);
            }
        }
        
        
        
    }
}