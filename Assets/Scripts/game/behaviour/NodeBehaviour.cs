using backend;
using backend.level_serialization;
using backend.threat_modelling;
using DefaultNamespace.node;
using UnityEngine;

/*
 * Classes implementing this interface are where the Attack() method is defined for each node type.
 *
 * It is also where modifiable parameters are kept.
 */


namespace DefaultNamespace {
    public class NodeBehaviour {

        protected Node node;
        protected NodeField[] fields;
        protected NodeField selectedStartingThreats;
        
        public NodeBehaviour(Node node) {
            this.node = node;
            InitialiseFields();
        }

        protected virtual void InitialiseFields() {
            
        }
        
        public void InitialiseStartingThreatSet() {
            Control_Dropdown_Option_Set optionSet = node.nodeObject.GetNodeDefinition().GetStartingThreatsOptionSet();
            selectedStartingThreats = new NodeField("Starting Threats", optionSet);
        }
        
        public virtual ThreatStatus Attack(Threat threat) {
            Node[] connectedNodes = node.GetConnectedNodes();
            
            // Check if any of the logical nodes connected are effective against the threat
            foreach (Node n in connectedNodes) {
                if (n.nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Logical) {
                    ThreatStatus status = n.GetThreatEffect(threat);
                    if (status == ThreatStatus.Failure) {
                        return status;
                    }
                }
            }

            return node.GetThreatEffect(threat);
        }

        public NodeField[] GetFields() {
            return fields;
        }
        
        public void SetFields(NodeSave nodeSave) {
            fields = nodeSave.fields;
            selectedStartingThreats = nodeSave.selectedStartingThreats;
        }

        public virtual int GetTotalHealth() {
            Debug.Log("GetTotalHealth method not implemented");
            return 0;
        }

        public NodeField GetSelectedStartingThreats() {
            return selectedStartingThreats;
        }

    }
}