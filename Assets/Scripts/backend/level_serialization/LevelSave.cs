using DefaultNamespace;

namespace backend.level_serialization {
    [System.Serializable]
    public class LevelSave {

        public int budget;
        public NodeSave[] nodes;
        public ConnectionSave[] connections;
        public int currentNodeID;
        public int attempts;

        public int bronzeScore;
        public int silverScore;
        public int goldScore;

        public LevelSave(Level level) {
            budget = level.GetBudget();
            nodes = SerializeNodes(GameManager.currentLevel.nodes.ToArray());
            connections = SerializeConnections(GameManager.levelScene.connectionManager.GetConnections());
            currentNodeID = GameManager.currentLevel.GetNewNodeID() - 1;
            attempts = level.GetAttempts();
            bronzeScore = level.GetMedalBoundary(Medal.Bronze);
            silverScore = level.GetMedalBoundary(Medal.Silver);
            goldScore = level.GetMedalBoundary(Medal.Gold);
        }

        private NodeSave[] SerializeNodes(Node[] inputNodes) {
           NodeSave[] result = new NodeSave[inputNodes.Length];

            for (int i = 0; i < inputNodes.Length; i++) {
                result[i] = new NodeSave(inputNodes[i].nodeObject);
            }

            return result;
        }

        private ConnectionSave[] SerializeConnections(Connection[] inputConnections) {

            ConnectionSave[] outputConnections = new ConnectionSave[inputConnections.Length];

            for (int i = 0; i < outputConnections.Length; i++) {
                int start = inputConnections[i].start.GetNodeID();
                int end = inputConnections[i].end.GetNodeID();
                bool duplex = inputConnections[i].IsDuplex();
                
                outputConnections[i] = new ConnectionSave(duplex, start, end);
            }

            return outputConnections;

        }

    }
}