using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsWall = false;
    public bool IsStartNode = false;
    public bool IsGoalNode = false;
    public void OnMouseDown()
    {
        if (!GameManager.instance.ChoosingStartNode && !GameManager.instance.ChoosingGoalNode)
        {
            IsWall = !IsWall;
            RefreshColor();
        }
        else if (GameManager.instance.ChoosingStartNode)
        {
            GameManager.instance.LabirynthGenerator.SetStartCell(this);
            IsStartNode = true;
            IsGoalNode = false;
            IsWall = false;
            RefreshColor();
        }
        else if (GameManager.instance.ChoosingGoalNode)
        {
            GameManager.instance.LabirynthGenerator.SetGoalCell(this);
            IsGoalNode = true;
            IsStartNode = false;
            IsWall = false;
            RefreshColor();
        }
    }

    public void RefreshColor()
    {
        if (IsWall)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        if (IsStartNode)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        else if (IsGoalNode)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
    }
}
