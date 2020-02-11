using UnityEngine;

namespace backend {
    public class AttackVisualiser : MonoBehaviour{

        public Color attackColor;

        private Threat[] currentTrace;
        
        public void VisualiseAttack(Threat threat) {
            ClearVisualisation();
            
            currentTrace = threat.GetTrace();

            foreach (Threat t in currentTrace) {
                t.GetNode().nodeObject.SetTempColor(attackColor);
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
            }
        }
        
        
        
    }
}