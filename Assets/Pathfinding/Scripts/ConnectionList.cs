using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using UnityEngine;

namespace Pathfinding
{
    public class ConnectionList
    {
        private Node _sourceNode;
        private List<Connection> _connections;

        public ConnectionList(Node sourceNode)
        {
            _sourceNode = sourceNode;

            _connections = new List<Connection>(8);

            for (int i = 0; i < 8; i++)
            {
                _connections.Add(null);
            }
        }

        public bool Exists(int index)
        {
            return _connections[index] != null;
        }

        public Connection Get(int index)
        {
            return _connections[index];
        }

        public void Set(int index, Connection connection)
        {
            _connections[index] = connection;
        }

        public List<Node> GetConnectedNodes()
        {
            List<Node> connectedNodes = new List<Node>();

            for (int i = 0; i < _connections.Count; i++)
            {
                if (_connections[i] != null)
                {
                    connectedNodes.Add(_connections[i].GetOtherNode(_sourceNode));
                }
            }

            return connectedNodes;
        }

        public List<Connection> GetConnections()
        {
            List<Connection> connections = new List<Connection>();

            for (int i = 0; i < _connections.Count; i++)
            {
                if (_connections[i] != null)
                {
                    connections.Add(_connections[i]);
                }
            }

            return connections;
        }

        public void ComputeLeftConnection(Node currentNode, NodeGrid grid)
        {
            Vector2i leftCoord = new Vector2i(currentNode.Coord.x - 1, currentNode.Coord.y);
            Connection leftConnection = CheckSideConnection(currentNode, leftCoord, grid);

            EtablishConnection((int) ConnectionIndex.Left, leftConnection, leftCoord, grid);
        }

        public void ComputeRightConnection(Node currentNode, NodeGrid grid)
        {
            Vector2i rightCoord = new Vector2i(currentNode.Coord.x + 1, currentNode.Coord.y);
            Connection rightConnection = CheckSideConnection(currentNode, rightCoord, grid);

            EtablishConnection((int) ConnectionIndex.Right, rightConnection, rightCoord, grid);
        }

        public void ComputeTopConnection(Node currentNode, NodeGrid grid)
        {
            Vector2i topCoord = new Vector2i(currentNode.Coord.x, currentNode.Coord.y + 1);
            Connection topConnection = CheckSideConnection(currentNode, topCoord, grid);

            EtablishConnection((int) ConnectionIndex.Top, topConnection, topCoord, grid);
        }

        public void ComputeBotConnection(Node currentNode, NodeGrid grid)
        {
            Vector2i botCoord = new Vector2i(currentNode.Coord.x, currentNode.Coord.y - 1);
            Connection botConnection = CheckSideConnection(currentNode, botCoord, grid);

            EtablishConnection((int) ConnectionIndex.Bottom, botConnection, botCoord, grid);
        }

        public void ComputeTopLeftDiagonalFromSide(Node currentNode, NodeGrid grid)
        {
            Connection newConnection = null;
            Vector2i topLeft = new Vector2i(currentNode.Coord.x - 1, currentNode.Coord.y + 1);

            if (Exists((int) ConnectionIndex.Left) && Exists((int) ConnectionIndex.Top))
            {
                if (grid.IsInside(topLeft))
                {
                    Node otherNode = grid.Get(topLeft);

                    if (!otherNode.IsBlocking)
                    {
                        newConnection = new Connection(currentNode, otherNode, Heuristic.DiagonalCost);
                    }
                }
            }

            EtablishConnection((int) ConnectionIndex.TopLeft, newConnection, topLeft, grid);
        }

        public void ComputeTopRightDiagonalFromSide(Node currentNode, NodeGrid grid)
        {
            Connection newConnection = null;
            Vector2i topRight = new Vector2i(currentNode.Coord.x + 1, currentNode.Coord.y + 1);

            if (Exists((int) ConnectionIndex.Right) && Exists((int) ConnectionIndex.Top))
            {
                if (grid.IsInside(topRight) && !grid.Get(topRight).IsBlocking)
                {
                    Node otherNode = grid.Get(topRight);

                    if (!otherNode.IsBlocking)
                    {
                        newConnection = new Connection(currentNode, otherNode, Heuristic.DiagonalCost);
                    }
                }
            }

            EtablishConnection((int) ConnectionIndex.TopRight, newConnection, topRight, grid);
        }

        private void EtablishConnection(int index, Connection connection, Vector2i otherCoord, NodeGrid grid)
        {
            Set(index, connection);

            int oppositeIndex = Connection.GetOppositeIndex(index);

            if (grid.IsInside(otherCoord))
                grid.Get(otherCoord).Connections.Set(oppositeIndex, connection);
        }

        private Connection CheckSideConnection(Node currentNode, Vector2i destinationCoord, NodeGrid grid)
        {
            if (grid.IsInside(destinationCoord))
            {
                Node destinationNode = grid.Get(destinationCoord);
                if (destinationNode.IsBlocking)
                    return null;

                return new Connection(currentNode, destinationNode, Heuristic.SideCost);
            }

            return null;
        }
    }
}