using backend;
using backend.threat_modelling;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_ConnectionEncryption : NodeBehaviour{
        
        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Description", "Encrypts data sent along data connections");
            fields[1] = new NodeField("Encryption Type", 0, false, ControlDropdownOptionSets.Connection_Encryption_Types, false);
        }

        public override ThreatStatus Attack(Threat threat) {
            ThreatType threatType = threat.threatType;

            Control_Dropdown_Option_Set encryptionTypes = fields[1].GetOptionSet();
            Control_Dropdown_Option chosenEncryption = encryptionTypes.options[(int)fields[1].GetValue()];

            if (threatType == ThreatType.Interception_Data_Theft) {
                int remainingHealth = GetTotalHealth() - threat.GetStrength();
                
                threat.SetNodeHealth(remainingHealth);

                if (remainingHealth <= 0) {
                    return ThreatStatus.Propagate;
                }

                return ThreatStatus.Failure;
            }

            return base.Attack(threat);
        }

        public NodeBehaviour_ConnectionEncryption(Node node) : base(node) {
            
        }
        
        
    }
}