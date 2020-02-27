using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_PasswordGate : NodeBehaviour {
        public NodeBehaviour_PasswordGate(Node node) : base(node) {
            
        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Description", "Protects against unauthorised access.");
            fields[1] = new NodeField("Additions", ControlDropdownOptionSets.Password_Addons);
            
            base.InitialiseFields();
        }

        public override ThreatStatus Attack(Threat threat) {
            if (threat.threatType == ThreatType.Unauthorised_Access) {
                int health = GetTotalHealth();

                health -= threat.GetStrength();
                
                threat.SetNodeHealth(health);

                if (health >= 0) {
                    return ThreatStatus.Failure;
                }
            }

            return base.Attack(threat);
        }
    }
}