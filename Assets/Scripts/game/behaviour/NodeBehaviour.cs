using System;
using backend;
using DefaultNamespace.node;

/*
 * Classes implementing this interface are where the Attack() method is defined for each node type.
 *
 * It is also where modifiable parameters are kept.
 */


namespace DefaultNamespace {
    public class NodeBehaviour {

        private Node node;
        protected NodeField[] fields;
        
        public NodeBehaviour(Node node) {
            this.node = node;
            InitialiseFields();
        }

        protected virtual void InitialiseFields() {
            
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

        public virtual NodeField[] GetFields() {
            return fields;
        }

        public virtual void SetModifiableFields(NodeField[] values) {

        }

        public void SetFields(NodeField[] fields) {
            this.fields = fields;
        }

    }
}