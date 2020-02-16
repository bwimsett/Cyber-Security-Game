using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Captcha : NodeBehaviour {
        public NodeBehaviour_Captcha(Node node) : base(node) {

        }

        protected override void InitialiseFields() {
            /*fields = new NodeField[2];
            
            fields[0] = new NodeField("Node Description", "Verifies whether a user is human");
            fields[1] = new NodeField("CAPTCHA Type", 0, ControlDropdownOptionSets.Captcha_Types);*/
            base.InitialiseFields();
        }

        public override ThreatStatus Attack(Threat threat) {


            return base.Attack(threat);
        }
    }
}