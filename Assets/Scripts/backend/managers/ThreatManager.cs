using System.Collections.Generic;
using backend;
using UnityEngine;

namespace DefaultNamespace {
    public class ThreatManager : MonoBehaviour{

        private List<Threat> successfulThreats;
        private List<Threat> failedThreats;
        private List<Threat> propagatedThreats;
        private List<Threat> evolvedThreats;
        private int activeThreatCount = 0;

        public void SimulateThreats() {
            
            successfulThreats = new List<Threat>();
            failedThreats = new List<Threat>();
            propagatedThreats = new List<Threat>();
            evolvedThreats = new List<Threat>();
            activeThreatCount = 0;
            
            Node[] nodes = GameManager.currentLevel.nodes.ToArray();

            foreach (Node n in nodes) {
                NodeDefinition nodeDef = n.nodeObject.GetNodeDefinition();

                foreach (ThreatType t in nodeDef.startingThreats) {
                    CreateThreat(t, null, n);
                }
            }
        }

        public Threat CreateThreat(ThreatType t, Threat parent, Node n) {
            activeThreatCount++;
            Threat threat = new Threat(t, parent, n);
            return threat;
        }

        public void SetThreatStatus(Threat threat, ThreatStatus status) {
            threat.SetStatus(status);

            switch (status) {
                case ThreatStatus.Evolve:
                    evolvedThreats.Add(threat);
                    break;
                case ThreatStatus.Failure:
                    failedThreats.Add(threat);
                    break;
                case ThreatStatus.Success:
                    successfulThreats.Add(threat);
                    break;
                case ThreatStatus.Propagate:
                    propagatedThreats.Add(threat);
                    break;
            }

            CheckForSimulationCompletion();
        }

        private bool CheckForSimulationCompletion() {
            int failedCount = failedThreats.Count;
            int successfulCount = successfulThreats.Count;
            int propCount = propagatedThreats.Count;
            int evolvedCount = evolvedThreats.Count;
            
            int total = failedCount + successfulCount + propCount + evolvedCount;
          
            //Debug.Log("Completed Threats: "+total+"/"+activeThreatCount);
            
            if (total == activeThreatCount) {
                PrintThreats(successfulThreats);
                return true;
            }

            return false;
        }

        private void PrintThreats(List<Threat> threats) {
            foreach (Threat t in threats) {
                Debug.Log(t.GetTrace());
            }
        }
        
        public void ThreatDebugLog(string output) {
            Debug.Log(output);
        }


    }
}