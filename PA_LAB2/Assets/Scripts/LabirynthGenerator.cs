using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;
public class LabirynthGenerator : MonoBehaviour
{
    private IDS_Alrorithm IDS = new IDS_Alrorithm();
    private AStarAlgorithm AStar = new AStarAlgorithm();
    [SerializeField] Transform _grid;
    [SerializeField] private GameObject _nodePrefab; 
    private List<Transform> _allNodesTransforms = new List<Transform>();
    [SerializeField] private Transform _startCell;
    [SerializeField] private Transform _goalCell;
    private Node _startNode;
    private Node _goalNode;

    [SerializeField] TextMeshProUGUI _time;
    private Stopwatch sw = new Stopwatch();

    [SerializeField] TextMeshProUGUI _iterations;
    [SerializeField] TextMeshProUGUI _statesAmount;
    [SerializeField] TextMeshProUGUI _deadEnds;
    [SerializeField] TextMeshProUGUI _statesInMemory;



    public void GenerateBlankGrid()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                var node = Instantiate(_nodePrefab, new Vector3(j * 1.1f, i * 1.1f, 0), Quaternion.identity, _grid);
                _allNodesTransforms.Add(node.transform);
            }
        }
    }

    public void SetNodesCells(Algorithm algo)
    {
        algo.AllNodes = new List<Node>();
        foreach (var cell in _allNodesTransforms)
        {
            algo.AllNodes.Add(new Node(cell));
        }
    }


    private void Start()
    {
        GenerateBlankGrid();
        SetNodesCells(IDS);
        SetNodesCells(AStar);
        IDS.SetNodesChildren();
        AStar.SetNodesChildren();

    }

    public void FindPathByIDS()
    {
        IDS.SetWalls();
        sw.Restart();
        IDS.SetNodesChildren();
        _startNode = IDS.FindNodeByCell(_startCell);
        _goalNode = IDS.FindNodeByCell(_goalCell);
        IDS.IDS(_startNode, _goalNode, 20);
        _time.text = sw.ElapsedMilliseconds.ToString();
        _iterations.text = IDS.Iterations.ToString();
        _statesAmount.text = IDS.StatesAmount.ToString();
        _statesInMemory.text = IDS.StatesInMemory.ToString();
        _deadEnds.text = IDS.DeadEnds.ToString();
        ShowPath(IDS);

    }

    public void FindPathByAStar()
    {
        AStar.SetWalls();
        AStar.SetNodesChildren();
        _startNode = AStar.FindNodeByCell(_startCell);
        _goalNode = AStar.FindNodeByCell(_goalCell);
        AStar.SetManhattanDistances(_goalNode);
        AStar.PathFinding(_startNode, _goalNode, sw);
        _time.text = AStar.Time.ToString();
        _iterations.text = AStar.Iterations.ToString();
        _statesAmount.text = AStar.StatesAmount.ToString();
        _statesInMemory.text = AStar.StatesInMemory.ToString();
        _deadEnds.text = AStar.DeadEnds.ToString();
        ShowPath(AStar);
    }

    public void ShowPath(Algorithm algo)
    {
        foreach(var node in algo.Path)
        {
            node.Cell.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }

        UnityEngine.Debug.Log("Start cell position: " + (_startCell.position.x/1.1f).ToString() + "  " + (_startCell.position.y/1.1f).ToString());
        UnityEngine.Debug.Log("GOAL cell position: " + (_goalCell.position.x/1.1f).ToString() + "  " + (_goalCell.position.y / 1.1f).ToString());
    }

    public void SetStartCell(Cell cell)
    {
        foreach(var node in AStar.AllNodes)
        {
            if (node.Cell == cell)
                continue;
            node.Cell.GetComponent<Cell>().IsStartNode = false;
            node.Cell.GetComponent<Cell>().RefreshColor();
        }
        _startCell = cell.transform;
    }

    public void SetGoalCell(Cell cell)
    {
        foreach (var node in AStar.AllNodes)
        {
            if (node.Cell.GetComponent<Cell>() == cell)
                continue;
            node.Cell.GetComponent<Cell>().IsGoalNode = false;
            node.Cell.GetComponent<Cell>().RefreshColor();
        }
        _goalCell = cell.transform;
    }

}
