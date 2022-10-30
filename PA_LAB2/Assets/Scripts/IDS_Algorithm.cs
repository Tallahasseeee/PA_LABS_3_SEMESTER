using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDS_Alrorithm : Algorithm
{

    public bool IDS(Node root, Node goal, int maxDepth)
    {
        for (int i = 0; i < maxDepth; i++)
        {
            if(DFS(root, goal, i))
            {
                return true;
            }
        }

        return false;
    }

    public bool DFS(Node root, Node goal, int depth)
    {
        if (root == goal)
        {
            StatesInMemory++;
            return true;
        }


        if (depth == 0)
        {
            DeadEnds++;
            return false;
        }

        foreach(var child in root.Children)
        {
            StatesAmount++;
            Iterations++;
            if (child == root)
                continue;
            if(DFS(child, goal, depth - 1))
            {
                Path.Add(child);
                StatesInMemory++;
                return true;
                
            }
        }

        DeadEnds++;
        return false;
    }

    
}
