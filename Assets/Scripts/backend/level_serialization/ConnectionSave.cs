namespace backend.level_serialization {
    [System.Serializable]
    public class ConnectionSave {

        public bool duplex;
        public int startNode; // Node ID
        public int endNode;

        public ConnectionSave(bool duplex, int startNode, int endNode) {
            this.duplex = duplex;
            this.startNode = startNode;
            this.endNode = endNode;
        }

    }
}