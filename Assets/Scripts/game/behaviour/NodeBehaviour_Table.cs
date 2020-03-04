using System;
using backend;
using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Table : NodeBehaviour {
        protected override void InitialiseFields() {
            fields = new NodeField[3];
            fields[0] = new NodeField("Users", true, "This table mostly contains password and login information for the network");
            fields[1] = new NodeField("Sensitivity", true, 75, 0, 100);
            fields[2] = new NodeField("Minimum access", 0, true, ControlDropdownOptionSets.Access);
            //fields[3] = new NodeField("Minimum Access", 1, ControlDropdownOptionSets.Connection_Encryption_Types);
        }
        
        public NodeBehaviour_Table(Node node) : base(node) {
            
        }
        


    }
}