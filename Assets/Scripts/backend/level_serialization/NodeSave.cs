using System.Numerics;
using DefaultNamespace;
using DefaultNamespace.node;

namespace backend.level_serialization {
    [System.Serializable]
    public class NodeSave {

        public NodeFamily nodeFamily;
        public NodeType nodeType;
        public Vector2Save position;
        public int[] connectedNodes;
        public int id;
        public NodeField[] fields;

        public NodeSave(NodeObject nodeObject) {
            nodeFamily = nodeObject.GetNodeDefinition().nodeFamily;
            nodeType = nodeObject.GetNodeDefinition().nodeType;
            position = new Vector2Save(nodeObject.transform.localPosition);
            connectedNodes = ParseConnectedNodes(nodeObject.GetNode().GetConnectedNodes());
            fields = nodeObject.GetNode().GetBehaviour().GetFields();
            id = nodeObject.GetNode().GetNodeID();
        }

        private int[] ParseConnectedNodes(Node[] nodes) {
            int[] result = new int[nodes.Length];

            for (int i = 0; i < nodes.Length; i++) {
                result[i] = nodes[i].GetNodeID();
            }

            return result;
        }

    }
}