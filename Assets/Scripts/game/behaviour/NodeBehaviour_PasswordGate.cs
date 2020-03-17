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

                t.SetAccessLevel((int)fields[1].GetValue());
            }

            return threats;
        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Description", "Protects against unauthorised access.");
            fields[1] = new NodeField("Access Level", 0, true, ControlDropdownOptionSets.Access, false);
            //fields[2] = new NodeField("Additions", ControlDropdownOptionSets.Password_Addons, false);       
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

            /*if (threat.threatType == ThreatType.Authorised_Access_Block) {
                if (threat.GetAccessLevel() >= (int) fields[1].GetValue()) {
                    return ThreatStatus.Failure;
                }
            }*/

            if (threat.threatType == ThreatType.Unauthorised_Access) {
                threat.SetAccessLevel((int)fields[1].GetValue());
            }
            

            return base.Attack(threat);
        }
    }
}