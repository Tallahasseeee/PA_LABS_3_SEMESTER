using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm
{
    public List<Node> AllNodes = new List<Node>();

    public List<Node> Path = new List<Node>();

    public long StatesAmount = 0;
    public long Iterations = 0;
    public long DeadEnds = 0;
    public long StatesInMemory = 0;

    public void SetNodesChildren()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                AllNodes[20 * i + j].Children = new List<Node>();
                if (AllNodes[20 * i + j].IsWall)
                    continue;
                if (j + 1 < 20 && !AllNodes[20 * i + j + 1].IsWall)
                    AllNodes[20 * i + j].Children.Add(AllNodes[20 * i + j + 1]);
                if (i + 1 < 20 && !AllNodes[20 * (i + 1) + j].IsWall)
                    AllNodes[20 * i + j].Children.Add(AllNodes[20 * (i + 1) + j]);
                if (j - 1 >= 0 && !AllNodes[20 * i + j - 1].IsWall)
                    AllNodes[20 * i + j].Children.Add(AllNodes[20 * i + j - 1]);
                if (i - 1 >= 0 && !AllNodes[20 * (i - 1) + j].IsWall)
                    AllNodes[20 * i + j].Children.Add(AllNodes[20 * (i - 1) + j]);
            }
        }
    }

    public void SetWalls()
    {
        foreach (var node in AllNodes)
        {
            if (node.Cell.GetComponent<Cell>().IsWall)
            {
                node.IsWall = true;
            }
            else
            {
                node.IsWall = false;
            }
        }
    }
    public Node FindNodeByCell(Transform cell)
    {
        for (int i = 0; i < AllNodes.Count; i++)
        {
            if (AllNodes[i].Cell == cell)
                return AllNodes[i];
        }
        throw new System.Exception("Can't find the node");
    }

    public void SetManhattanDistances(Node goalNode)
    {
        Vector2 goalCoords = new Vector2(AllNodes.IndexOf(goalNode)%20, AllNodes.IndexOf(goalNode) / 20);
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                AllNodes[20 * i + j].Hcost = Mathf.Abs((int)goalCoords.x - j) + Mathf.Abs((int)goalCoords.y - i);
            }
        }
    }

}
