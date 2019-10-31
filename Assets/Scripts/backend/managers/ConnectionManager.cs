using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class ConnectionManager : MonoBehaviour {
        public GameObject connectionPrefab;
        private List<Connection> connections;

        void Awake() {
            connections = new List<Connection>();
        }
        
        // Instantiates a new connection, and returns it.
        private Connection CreateConnection(Node start, Node end) {
            Connection connection = Instantiate(connectionPrefab, GameManager.levelScene.transform)
                .GetComponent<Connection>();
            
            connection.SetEnds(start, end);

            connection.gameObject.name = "Connection " + connection;

            return connection;
        }

        // Create all the necessary connections for a node
        public void CreateConnectionsForNode(Node node) {
            foreach (Node neighbour in node.connectedNodes) {
                CreateAndAddConnection(node, neighbour);
            }
        }
        
        //If the connection already exists, return, otherwise create a new one and add it to connections.
        public void CreateAndAddConnection(Node start, Node end) {
            Connection connection = GetConnection(start, end);
            
            if (connection) {
                return;
            }
            
            connection = CreateConnection(start, end);
            connections.Add(connection);
        }
        
        // Performs a linear search for a connection with both nodes
        public Connection GetConnection(Node end1, Node end2) {
            foreach (Connection connection in connections) {

                bool start_end1 = connection.start == end1;
                bool start_end2 = connection.start == end2;
                bool end_end1 = connection.end == end1;
                bool end_end2 = connection.end == end2;
                
                if ((start_end1 && end_end2) || (start_end2 && end_end1)) {
                    return connection;
                }
            }

            return null;
        }
        
        // Returns all the connections to a particular Node
        public Connection[] GetConnectionsToNode(Node node) {
            List<Connection> nodeConnections = new List<Connection>();
            
            foreach (Node neighbour in node.connectedNodes) {
                nodeConnections.Add(GetConnection(node, neighbour));
            }

            return nodeConnections.ToArray();
        }
        
    }
}