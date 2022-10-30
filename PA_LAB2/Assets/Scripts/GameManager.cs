using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LabirynthGenerator LabirynthGenerator;
    public bool ChoosingStartNode = false;
    public bool ChoosingGoalNode = false;
    private void Start()
    {
        instance = this;
    }

    public void ChooseStartNode()
    {
        ChoosingStartNode = true;
        ChoosingGoalNode = false;
    }

    public void ChooseGoalNode()
    {
        ChoosingStartNode = false;
        ChoosingGoalNode = true;
    }

    public void ChooseWalls()
    {
        ChoosingStartNode = false;
        ChoosingGoalNode = false;
    }


}
