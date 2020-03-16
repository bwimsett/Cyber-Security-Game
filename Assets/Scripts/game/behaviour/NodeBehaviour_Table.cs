using System;
using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Table : NodeBehaviour {
        protected override void InitialiseFields() {
            fields = new NodeField[3];
            fields[0] = new NodeField("Users", true, "This table mostly contains password and login information for the network");
            fields[1] = new NodeField("Sensitivity", true, 75, 0, 100);
            fields[2] = new NodeField("Minimum access", 0, true, ControlDropdownOptionSets.Access, true);
        }
        
        public override ThreatStatus Attack(Threat threat) {
            bool unauthorisedAccess = threat.threatType == ThreatType.Unauthorised_Access;
            bool authorisedAccess = threat.threatType == ThreatType.Authorised_Access_Block;
            int threatAccessLevel = threat.GetAccessLevel();

            if (!unauthorisedAccess && !authorisedAccess) {
                return base.Attack(threat);
            }
            
            // If the threat came from a password gate of access level too low
            if (threatAccessLevel < (int)GetFieldWithSet(ControlDropdownOptionSets.Access).GetValue() && unauthorisedAccess) {
                return ThreatStatus.Evolve;
            }

            if (threatAccessLevel > (int)GetFieldWithSet(ControlDropdownOptionSets.Access).GetValue() && authorisedAccess) {
                return ThreatStatus.Success;
            }    
            
            return ThreatStatus.Failure;
        }


        public NodeBehaviour_Table(Node node) : base(node) {
        }
    }
}