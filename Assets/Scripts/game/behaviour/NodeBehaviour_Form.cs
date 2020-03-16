using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Form : NodeBehaviour {
        public NodeBehaviour_Form(Node node) : base(node) {
        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Description", "[BLANK]");
            fields[1] = new NodeField("Access Level", 0, true, ControlDropdownOptionSets.Access, true);
        }

        public override ThreatStatus Attack(Threat threat) {
            bool unauthorisedAccess = threat.threatType == ThreatType.Unauthorised_Access;
            bool authorisedAccess = threat.threatType == ThreatType.Authorised_Access_Block;
            int threatAccessLevel = threat.GetAccessLevel();

            if (!unauthorisedAccess && !authorisedAccess) {
                return base.Attack(threat);
            }
            
            // If the threat came from a password gate of access level too low
            if (threatAccessLevel < (int)fields[1].GetValue() && unauthorisedAccess) {
                return ThreatStatus.Success;
            }

            if (threatAccessLevel > (int) fields[1].GetValue() && authorisedAccess) {
                return ThreatStatus.Success;
            }    
            
            return ThreatStatus.Failure;
        }
    }
}