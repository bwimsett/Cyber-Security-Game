using DefaultNamespace;
using DefaultNamespace.node;
using UnityEngine;

namespace backend {
    public class AttackVisualiser : MonoBehaviour{

        public Color attackColor;
        private Threat[] currentTrace;
        private int tracePosition;
        private bool playingThreat;

        void Update() {
            UpdateAttackAnimation();
        }
        
        public void VisualiseAttack(Threat threat) {
            ClearVisualisation();
            
            currentTrace = threat.GetTrace();

            Debug.Log(threat.GetStringTrace());
            
            foreach (Threat t in currentTrace) {
                NodeObject nodeObject = t.GetNode().nodeObject;
                nodeObject.SetTempColor(GameManager.levelScene.threatManager.threatColor);
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

            playingThreat = false;
            
            foreach (Threat t in currentTrace) {
                Node n = t.GetNode();
                if (n.nodeObject == null) {
                    continue;
                }
                t.GetNode().nodeObject.ResetThreatSimulation();
                t.GetNode().nodeObject.healthBar.SetVisible(false);
            }
            
            GameManager.levelScene.connectionManager.ResetAttackSimulations();
        }

        public void AnimateAttack(Threat threat) {
            ClearVisualisation();
            currentTrace = threat.GetTrace();
            tracePosition = 0;
            playingThreat = true;
        }

        public void UpdateAttackAnimation() {
            if (!playingThreat) {
                return;
            }
            
            NodeObject currentNode = currentTrace[tracePosition].GetNode().nodeObject;
            Animator currentNodeAnimator = currentNode.nodeAttackMonitor.attackAnimator;

            // Check if the current node has already been attacked
            if (currentNodeAnimator.GetBool("attacked")) {
                // Check if the current node has finished playing attack animation
                NodeAttackAnimationMonitor animationMonitor = currentNode.nodeAttackMonitor;
                
                if (!animationMonitor.finishedAttacking) {
                    return;
                }
                
                // Check if connection has finished playing attack animation
                if (tracePosition >= currentTrace.Length - 1) {
                    // Exit if the final node has been attacked
                    playingThreat = false;
                    return;
                }

                Node nextNode = currentTrace[tracePosition + 1].GetNode();
                Connection connection =
                    GameManager.levelScene.connectionManager.GetConnection(currentNode.GetNode(), nextNode);

                if (connection == null) {
                    tracePosition++;
                    UpdateAttackAnimation();
                    return;
                }

                if (connection.threatSimulationComplete) {
                    // Increase trace position and return
                    tracePosition++;
                    UpdateAttackAnimation();
                    return;
                }
                
                // Trigger connection animation if it has not completed (will be ignored if animation already started).
                connection.TriggerThreatSimulation(currentNode.GetNode());
                
            }
            else {
                // Play attack animation for node
                currentNodeAnimator.SetBool("attacked", true);
                currentNode.SimulateAttack();      
            }
        }
        
        
        
    }
}