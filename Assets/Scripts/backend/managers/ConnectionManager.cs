using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class ConnectionManager : MonoBehaviour {
        public GameObject connectionPrefab;
        private List<Connection> connections;

        void Awake() {
            if (connections == null) {
                connections = new List<Connection>();
            }
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

            Node[] nodes = node.GetConnectedNodes();
            
            foreach (Node neighbour in nodes) {
                CreateAndAddConnection(node, neighbour);
            }
        }
        
        //If the connection already exists, return, otherwise create a new one and add it to connections.
        public Connection CreateAndAddConnection(Node start, Node end) {
            Connection connection = GetConnection(start, end);

            if (connection) {
                return connection;
            }

            if (start != null && end != null) {
                start.AddConnection(end.nodeObject);
                end.AddConnection(start.nodeObject);
            }

            connection = CreateConnection(start, end);
            connections.Add(connection);
            return connection;
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

        public void RemoveConnection(Connection connection) {
            if (connection == null) {
                return;
            }

            if(connection.end != null && connection.start != null) {
                connection.end.connectedNodes.Remove(connection.start);
                connection.start.connectedNodes.Remove(connection.end);
            }
            
            connections.Remove(connection);
            Destroy(connection.gameObject);

            Debug.Log("Connection removed: " + connection);
        }
        
        // Returns all the connections to a particular Node
        public Connection[] GetConnectionsToNode(Node node) {
            List<Connection> nodeConnections = new List<Connection>();
            
            foreach (Node neighbour in node.connectedNodes) {
                nodeConnections.Add(GetConnection(node, neighbour));
            }

            return nodeConnections.ToArray();
        }

        public void CreateConnectionsFromIDArray(Node startNode, int[] connectedNodes) {
            foreach (int i in connectedNodes) {
                Node connectedNode = GameManager.levelScene.nodeManager.GetNodeByID(i);
                CreateAndAddConnection(startNode, connectedNode);
            }
        }


        public Connection[] GetConnections() {
            if (connections == null) {
                connections = new List<Connection>();
            }
            
            return connections.ToArray();
        }
    }
}