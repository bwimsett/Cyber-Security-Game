using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_MailScanner : NodeBehaviour{
        public NodeBehaviour_MailScanner(Node node) : base(node) {
        }

        protected override void InitialiseFields() {
            fields = new NodeField[1];
            
            fields[0] = new NodeField("Description", "Scans all mail passing through this node.");
        }

        public override ThreatStatus Attack(Threat threat) {
            if (threat.threatType == ThreatType.Mail_Malware || threat.threatType == ThreatType.Spam_Email) {
                return ThreatStatus.Failure;
            }

            return base.Attack(threat);
        }
    }
}