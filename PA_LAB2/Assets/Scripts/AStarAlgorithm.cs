using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class AStarAlgorithm : Algorithm
{
    public List<Node> ClosedList = new List<Node>();
    public List<Node> OpenList = new List<Node>();
    public int Time;

    public bool PathFinding(Node startNode, Node endNode, Stopwatch sw)
    {
        sw.Start();
        OpenList.Add(startNode);

        foreach (var node in AllNodes)
        {
            node.Gcost = int.MaxValue;
            node.CalculateFcost();
            node.CameFromNode = null;
        }
        SetManhattanDistances(endNode);

        startNode.Gcost = 0;

        while (OpenList.Count > 0)
        {
            Iterations++;
            Node currentNode = GetLowestFcostNode(OpenList);
            if (currentNode == endNode)
            {
                CalculatePath(endNode);
                Time = (int)sw.ElapsedMilliseconds;
                //UnityEngine.Debug.Log(Time);
                return true;
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            foreach (var node in currentNode.Children)
            {
                StatesAmount++;
                StatesInMemory++;
                if (ClosedList.Contains(node))
                    continue;

                int tentativeGcost = currentNode.Gcost + 1;
                if (tentativeGcost < node.Gcost)
                {
                    node.CameFromNode = currentNode;
                    node.Gcost = tentativeGcost;
                    node.CalculateFcost();

                    if (!OpenList.Contains(node))
                    {
                        OpenList.Add(node);
                    }
                }
            }
        }

        DeadEnds++;
        return false;

    }

    private void CalculatePath(Node endNode)
    {
        Node currentNode = endNode;
        while (currentNode.CameFromNode != null)
        {
            Path.Add(currentNode);
            currentNode = currentNode.CameFromNode;
        }
        Path.Reverse();
    }

    private Node GetLowestFcostNode(List<Node> openList)
    {
        Node lowestFcostNode = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].Fcost < lowestFcostNode.Fcost)
            {
                lowestFcostNode = openList[i];
            }
        }
        return lowestFcostNode;
    }
}

    /*public bool AStar(Node start, Node goal, int maxDepth)
    {
        int depth = 0;
        Node node = start;
        while (depth < maxDepth)
        {
            Explored.Add(node);
            if (node == goal)
            {
                ConvertExploredToPath();
                return true;
            }
            node = FindShortestDistanceChild(node);
            depth++;
        }
        return false;
    }

    public Node FindShortestDistanceChild(Node _node)
    {
        Node node;
        if (_node.Children.Count > 1)
        {
            if (!Explored.Contains(_node.Children[0]))
            {
                node = _node.Children[0];
            }
            else if(!Explored.Contains(_node.Children[1]))
            {
                node = _node.Children[1];
            }
            else if (!Explored.Contains(_node.Children[2]))
            {
                node = _node.Children[2];
            }
            else
            {
                node = _node.Children[3];
            }

            for (int i = 1; i < _node.Children.Count; i++)
            {
                if (Path.Contains(_node.Children[i]))
                    continue;
                if (_node.Children[i].ManhattanDistance < node.ManhattanDistance)
                {
                    node = _node.Children[i];
                }
            }
        }
        else
        {
            node = GoBack();
        }
        return node;
    }

    public Node GoBack()
    {
        for (int i = Explored.Count - 1; i >= 0; i--)
        {
            List<Node> availableNodes = NotExploredChildrenOfNode(Explored[i]);
            if(availableNodes.Count > 0)
            {
                Node node = FindShortestDistanceChild(Explored[i]);
                return node; 
            }
        }
        return null;
    }


    public List<Node> NotExploredChildrenOfNode(Node node)
    {
        List<Node> desiredNodes = new List<Node>();
        for (int i = 0; i < node.Children.Count; i++)
        {
            if (!Explored.Contains(node.Children[i]))
            {
                desiredNodes.Add(node.Children[i]);
            }
        }
        return desiredNodes;
    }

    public void ConvertExploredToPath()
    {
        Node node = Explored[0];
        Path.Add(node);
        bool flag = true;
        for (int i = 0; i < Explored.Count; i++)
        {
            for (int j = Explored.Count - 1; j >= i + 1; j--)
            {
                if (node.Children.Contains(Explored[j])&& flag)
                {
                    node = Explored[j];
                    Path.Add(node);
                    i = j - 1;
                    flag = false;
                }
            }
            flag = true;
        }
        
    }*/

