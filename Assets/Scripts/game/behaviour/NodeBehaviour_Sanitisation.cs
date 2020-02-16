using backend;
using backend.threat_modelling;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Sanitisation : NodeBehaviour {
        public NodeBehaviour_Sanitisation(Node node) : base(node) {
        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];

            fields[0] = new NodeField("Description", "Strips any escaped data from input");
            fields[1] = new NodeField("Sanitise against", ControlDropdownOptionSets.Sanitisation_Options);
            
            base.InitialiseFields();
        }

        public override ThreatStatus Attack(Threat threat) {
            int totalHealth = GetTotalHealth();

            bool cookiesSelected = ((char[])fields[1].GetValue())[0] == '1';
            bool formSelected = ((char[])fields[1].GetValue())[1] == '1';
            bool urlSelected = ((char[])fields[1].GetValue())[2] == '1';
            
            switch (threat.threatType) {
                case ThreatType.SQL_Injection: totalHealth -= threat.GetStrength();
                    break;
                case ThreatType.SQL_Injection_Form:
                    if (formSelected) { return ThreatStatus.Failure; }
                    break;
                case ThreatType.SQL_Injection_Cookie:
                    if(cookiesSelected){ return ThreatStatus.Failure; }
                    break;
                case ThreatType.SQL_Injection_URL:
                    if (urlSelected) { return ThreatStatus.Failure; }
                    break; 
            }

            if (totalHealth >=  0 && threat.threatType == ThreatType.SQL_Injection) {
                return ThreatStatus.Failure;
            }

            
            return base.Attack(threat);
        }

        public override int GetTotalHealth() {
            int total = 0;
            char[] bitmask = (char[])fields[1].GetValue();

            Control_Dropdown_Option_Set optionSet =
                GameManager.levelScene.nodeManager.GetControlOptionSet(ControlDropdownOptionSets.Sanitisation_Options);
            
            for(int i = 0; i < bitmask.Length; i++) {
                char c = bitmask[i];
                
                if (c != '1') {
                    continue;
                }

                total += optionSet.options[i].health;
            }

            return total;
        }
        
    }
}