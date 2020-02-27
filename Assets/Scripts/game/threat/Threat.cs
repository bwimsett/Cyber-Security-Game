using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DefaultNamespace;

namespace backend {
    public class Threat {
        public ThreatType threatType;
        private Threat parentThreat;
        private Node node;
        private ThreatStatus status;
        private int strength;
        private int nodeHealth;
        
        public Threat(ThreatType threatType, Threat parentThreat, Node node, int strength) {
            this.threatType = threatType;
            this.parentThreat = parentThreat;
            this.node = node;
            this.strength = strength;

            //GameManager.levelScene.threatManager.ThreatDebugLog(threatType+" appeared at "+node);
           
            Run();
        }

        private void Run() {
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
            Node[] nodes = node.GetConnectedNodes();

            foreach (Node n in nodes) {

                Connection c = GameManager.levelScene.connectionManager.GetConnection(node, n);
                bool flowValid = false;

                // Check the threat can flow in this direction down this connection
                if (c) {
                    flowValid = c.FlowDirectionValid(node, n);
                }
                
                if (!IsInParentChain(n) && flowValid) {
                    GameManager.levelScene.threatManager.CreateThreat(threatType, this, n);
                }
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
        
        private bool IsInParentChain(Node node) {
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

        public ThreatStatus GetStatus() {
            return status;
        }
    }
}