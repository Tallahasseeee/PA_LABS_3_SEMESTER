using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> Children = new List<Node>();
    public Node CameFromNode;
    public Transform Cell;
    public bool IsWall;
    public int Gcost;
    public int Hcost;
    public int Fcost;
    public int ManhattanDistance;

    public Node(Transform cell)
    {
        Cell = cell;
    }

    public void CalculateFcost()
    {
        Fcost = Gcost + Hcost;
    }
}
