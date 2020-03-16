using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Captcha : NodeBehaviour {
        public NodeBehaviour_Captcha(Node node) : base(node) {

        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Node Description", "Verifies whether a user is human");
            fields[1] = new NodeField("CAPTCHA Type", 0, true, ControlDropdownOptionSets.Captcha_Types, false);
        }

        public override ThreatStatus Attack(Threat threat) {
            int health = GetTotalHealth();

            if (threat.threatType == ThreatType.Spam_Email || threat.threatType == ThreatType.Spam_Accounts) {
                health -= threat.GetStrength();

                threat.SetNodeHealth(health);
                
                if (health > 0) {
                    return ThreatStatus.Failure;
                }
            }

            return base.Attack(threat);
        }
    }
}