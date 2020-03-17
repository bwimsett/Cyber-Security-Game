using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace;
using GameAnalyticsSDK.Setup;

namespace backend {
    public class Threat {
        public ThreatType threatType;
        private Threat parentThreat;
        private Node node;
        private ThreatStatus status;
        private int strength;
        private int nodeHealth;
        private int accessLevel;
        
        public Threat(ThreatType threatType, Threat parentThreat, Node node, int strength, int accessLevel) {
            this.threatType = threatType;
            this.parentThreat = parentThreat;
            this.node = node;
            this.strength = strength;
            this.accessLevel = accessLevel;
            if (parentThreat != null) {
                accessLevel = parentThreat.GetAccessLevel();
            }
        }

        public void Run() {
            ThreatStatus status = node.Attack(this);
            ThreatStatus newStatus = status;
            
            switch (status) {
                case ThreatStatus.Success:
                case ThreatStatus.Propagate: Propagate(status);
                    break;
                case ThreatStatus.Evolve: node.EvolveThreat(this);
                    break;
                case ThreatStatus.Success_On_Propagation: newStatus = SucceedOnPropagation();
                    break;
            }

            if (newStatus == ThreatStatus.Propagate && status == ThreatStatus.Success_On_Propagation) {
                Propagate(status);
            }
            
            GameManager.levelScene.threatManager.SetThreatStatus(this, newStatus);
        }

        private void Propagate(ThreatStatus status) {
            // Attack connections
            Connection[] connections = GameManager.levelScene.connectionManager.GetConnectionsToNode(node);
            
            foreach (Connection c in connections) {      
                c.Attack(threatType, this, node);
            }
        }

        // Trace the threat history to where this threat was created, and check it has propagated
        private ThreatStatus SucceedOnPropagation() {

            Threat currentThreat = this;
            
            // Go to the origin node
            do {
                if (currentThreat.parentThreat == null) {
                    break;
                }
                
                currentThreat = currentThreat.parentThreat;

                if (currentThreat == null) {
                    break;
                }
                
            } while (currentThreat.parentThreat != null);

            if (currentThreat.node == node) {
                return ThreatStatus.Propagate;
            }

            return ThreatStatus.Success;
        }
        
        public bool IsInParentChain(Node node) {
            if (this.node == node) {
                return true;
            }

            if (parentThreat == null) {
                return false;
            }
            
            return parentThreat.IsInParentChain(node);
        }

        public void SetStatus(ThreatStatus status) {
            this.status = status;
        }

        public string GetStringTrace() {
            return GetStringTrace("");
        }

        // Returns an ordered array containing the path the threat took.
        public Threat[] GetTrace() {
            List<Threat> pastThreats = new List<Threat>();

            Threat currentThreat = this;

            do {
                pastThreats.Add(currentThreat);
                currentThreat = currentThreat.parentThreat;
            } while (currentThreat.parentThreat != null);
            
            pastThreats.Add(currentThreat);

            pastThreats.Reverse();

            return pastThreats.ToArray();
        }
        
        private string GetStringTrace(string output) {
            output = " -> ("+threatType+", "+status+" at "+node+")" + output;

            if (parentThreat != null) {
                return parentThreat.GetStringTrace(output);
            }

            return output;
        }

        public int GetStrength() {
            return strength;
        }

        public void ReduceStrength(int amount) {
            strength -= amount;
        }

        public Node GetNode() {
            return node;
        }

        public void SetNodeHealth(int nodeHealth) {
            this.nodeHealth = nodeHealth;
        }

        public int GetNodeHealth() {
            return nodeHealth;
        }

        // Returns the threat which originated this one
        public Threat GetRoot() {
            if (parentThreat == null) {
                return this;
            }

            return parentThreat.GetRoot();
        }

        public Threat GetParent() {
            return parentThreat;
        }

        public int GetAccessLevel() {
            return accessLevel;
        }

        public void SetAccessLevel(int level) {
            this.accessLevel = level;
        }

        public ThreatStatus GetStatus() {
            return status;
        }

        public override string ToString() {
            return "" + threatType + " " + node + " " + strength;
        }
    }
}