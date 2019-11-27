using System.Diagnostics;
using DefaultNamespace;

namespace backend {
    public class Threat {
        public ThreatType threatType;
        private Threat parentThreat;
        private Node node;
        private ThreatStatus status;
        
        public Threat(ThreatType threatType, Threat parentThreat, Node node) {
            this.threatType = threatType;
            this.parentThreat = parentThreat;
            this.node = node;

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
            }
            
            GameManager.levelScene.threatManager.SetThreatStatus(this, status);
        }

        private void Propagate(ThreatStatus status) {
            Node[] nodes = node.GetConnectedNodes();

            foreach (Node n in nodes) {
                if (!IsInParentChain(n)) {
                    GameManager.levelScene.threatManager.CreateThreat(threatType, this, n);
                }
            }
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

        public string GetTrace() {
            return GetTrace("");
        }
        
        private string GetTrace(string output) {
            output = " -> ("+threatType+", "+status+" at "+node+")" + output;

            if (parentThreat != null) {
                return parentThreat.GetTrace(output);
            }

            return output;
        }
    }
}