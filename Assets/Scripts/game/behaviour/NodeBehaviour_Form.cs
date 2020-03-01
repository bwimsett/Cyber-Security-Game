using DefaultNamespace.node;

namespace DefaultNamespace {
    public class NodeBehaviour_Form : NodeBehaviour {
        public NodeBehaviour_Form(Node node) : base(node) {
        }

        protected override void InitialiseFields() {
            fields = new NodeField[2];
            
            fields[0] = new NodeField("Description", "[BLANK]");
            fields[1] = new NodeField("Access Level", 0, true, ControlDropdownOptionSets.Access);
            
            
            base.InitialiseFields();
        }
    }
}