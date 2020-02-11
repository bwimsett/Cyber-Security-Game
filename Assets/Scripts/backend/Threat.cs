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
            
            switch (status) {
                case ThreatStatus.Success:
                case ThreatStatus.Propagate: Propagate(status);
                    break;
                case ThreatStatus.Evolve: node.EvolveThreat(this);
                    break;
                case ThreatStatus.Success_On_Propagation: status = SucceedOnPropagation();
                    break;
            }
            
            GameManager.levelScene.threatManager.SetThreatStatus(this, status);
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
            
            do {
                currentThreat = parentThreat;

                if (currentThreat == null) {
                    break;
                }
                
                // If the evolution step has been found
                if (currentThreat.threatType == threatType && currentThreat.status == ThreatStatus.Evolve) {
                    // If the evolution occurred at the same node
                    if (currentThreat.node == node) {
                        return ThreatStatus.Propagate;
                    }
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

        public Node GetNode() {
            return node;
        }
    }
}