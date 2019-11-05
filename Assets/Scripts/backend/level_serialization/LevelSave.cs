using DefaultNamespace;

namespace backend.level_serialization {
    [System.Serializable]
    public class LevelSave {

        public int budget;
        public NodeSave[] nodes;
        public int currentNodeID;

        public LevelSave(Level level) {
            budget = level.GetBudget();
            nodes = SerializeNodes(GameManager.currentLevel.nodes.ToArray());
            currentNodeID = GameManager.currentLevel.GetNewNodeID() - 1;
        }

        private NodeSave[] SerializeNodes(Node[] inputNodes) {
           NodeSave[] result = new NodeSave[inputNodes.Length];

            for (int i = 0; i < inputNodes.Length; i++) {
                result[i] = new NodeSave(inputNodes[i].nodeObject);
            }

            return result;
        }

    }
}