using System;
using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Antivirus : NodeBehaviour {
        
        public NodeBehaviour_Antivirus(Node node) : base(node) {
            
        }
        
        protected override void InitialiseFields() {
            fields = new NodeField[4];
            
            fields[0] = new NodeField("Node Definition", "Scans for, and attempts to eliminate malware from the attached node");
            fields[1] = new NodeField("Scanned File Types", 0, false,ControlDropdownOptionSets.Antivirus_FileTypes);
            fields[2] = new NodeField("Scan Frequency", 0, false, ControlDropdownOptionSets.Antivirus_ScanFrequency);
            fields[3] = new NodeField("", ControlDropdownOptionSets.Antivirus_BlockMedia);
            
            base.InitialiseFields();
        }

        public override ThreatStatus Attack(Threat threat) {
            bool isVirus = threat.threatType == ThreatType.Virus_Worm || threat.threatType == ThreatType.Virus_Trojan ||
                           threat.threatType == ThreatType.Virus_Ransomware;

            if (!isVirus) {
                return base.Attack(threat);
            }

            int health = GetTotalHealth();

            int remainingHealth = health - threat.GetStrength();

            if (remainingHealth >= 0) {
                return ThreatStatus.Failure;
            }
            
            threat.SetNodeHealth(remainingHealth);
            
            return base.Attack(threat);
        }


       
    }
}