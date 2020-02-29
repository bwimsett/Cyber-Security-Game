using System.Collections.Generic;
using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_PasswordGate : NodeBehaviour {
        
        public NodeBehaviour_PasswordGate(Node node) : base(node) {
            
        }

        public override Threat[] GenerateThreats() {
            Threat[] threats = base.GenerateThreats();

            foreach (Threat t in threats) {
                if (t.threatType != ThreatType.Authorised_Access_Block) {
                    continue;
                }

                // Loop through all the connected nodes
                foreach (Node n in node.connectedNodes) {
                        
                    // Get the connection to the node
                    Connection c = GameManager.levelScene.connectionManager.GetConnection(node, n);
                        
                    // If the other node is the end of the connection, set access level to match
                    if (c.end != node) {
                        NodeField accessSet =
                            c.end.GetBehaviour().GetFieldWithSet(ControlDropdownOptionSets.Access);
                            
                        if (accessSet != null) {
                            t.SetAccessLevel((int)accessSet.GetValue());    
                        }
                    }
                }
            }

            return threats;
        }

        protected override void InitialiseFields() {
            fields = new NodeField[3];
            
            fields[0] = new NodeField("Description", "Protects against unauthorised access.");
            fields[1] = new NodeField("Additions", ControlDropdownOptionSets.Password_Addons);
            fields[2] = new NodeField("Access Level", 0, true, ControlDropdownOptionSets.Access);
            
            base.InitialiseFields();
        }

        public override void InitialiseStartingThreatSet() {
            base.InitialiseStartingThreatSet();
            selectedStartingThreats.SetValue(new char[]{'1'});
        }

        public override ThreatStatus Attack(Threat threat) {

            if (threat.threatType == ThreatType.Password_Crack) {

                int remainingHealth = GetTotalHealth() - threat.GetStrength();

                // If password gate was too weak to stop the access attempt
                if (remainingHealth > 0) {
                    return ThreatStatus.Failure;
                }
            }

            if (threat.threatType == ThreatType.Privilege_Escalation) {
                if (threat.GetAccessLevel() < (int) fields[2].GetValue()) {
                    return ThreatStatus.Failure;
                }
            }

            if (threat.threatType == ThreatType.Authorised_Access_Block) {
                if (threat.GetAccessLevel() > (int) fields[2].GetValue()) {
                    return ThreatStatus.Failure;
                }
            }
            

            return base.Attack(threat);
        }
    }
}