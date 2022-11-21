using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    private AVLTree AVLTree = new AVLTree();

    [SerializeField] Transform[] UINodes;

    [SerializeField] private TMP_InputField _inputFindKey;
    [SerializeField] private TextMeshProUGUI _foundContent;

    [SerializeField] private TMP_InputField _inputAddKey;
    [SerializeField] private TMP_InputField _inputAddContent;
    [SerializeField] private TMP_InputField _inputDeleteKey;
    [SerializeField] private TextMeshProUGUI _addingMessage;
    // Start is called before the first frame update
    void Start()
    {
        using (Stream stream = File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.OpenOrCreate))
        {
            
        }

        using(BinaryReader binaryReader = new BinaryReader(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Root.dat", FileMode.OpenOrCreate)))
        {
            if(binaryReader.BaseStream.Length != 0)
            {
                AVLTree._rootPosition = binaryReader.ReadInt64();
            }
        }
        ShowTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add()
    {
        int key;
        if(int.TryParse(_inputAddKey.text, out key))
        {

        }
        else
        {
            throw new Exception("Key didn't parse");
        }
        string content = _inputAddContent.text;
        Node node = new Node(key, content);
        string message = AVLTree.Add(node);
        _addingMessage.text = message;
        Node debug = AVLTree.GetNode(AVLTree._rootPosition);
        //Debug.Log(debug._key.ToString() + "  "  + debug._height.ToString());
        ShowTree();
    }

    public void Delete()
    {
        Int32.TryParse(_inputDeleteKey.text, out int deleteKey);
        AVLTree.Delete(deleteKey);
        ShowTree();
    }
    public void Find()
    {
        Int32.TryParse(_inputFindKey.text, out int targetKey);
        string content = AVLTree.Find(targetKey);
        _foundContent.text = content;
    }

    public void ShowTree()
    {
        Node root = null;
        if (AVLTree._rootPosition != -1)
        {
            root = AVLTree.GetNode(AVLTree._rootPosition);
            UINodes[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = root._key.ToString();
            UINodes[0].GetChild(1).GetComponent<TextMeshProUGUI>().text = root._height.ToString();
        }
        else
        {
            UINodes[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
            UINodes[0].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
        }
        Node first = null, second = null, third = null, fourth = null, fifth = null, sixth = null;
        if (root != null)
        {
            if (root._leftChild != -1)
            {
                first = AVLTree.GetNode(root._leftChild);
            }
            else
            {
                UINodes[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[1].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
            if (root._rightChild != -1)
            {
                second = AVLTree.GetNode(root._rightChild);
            }
            else
            {
                UINodes[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[2].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
        }
        if(first != null)
        {
            UINodes[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = first._key.ToString();
            UINodes[1].GetChild(1).GetComponent<TextMeshProUGUI>().text = first._height.ToString();
            if (first._leftChild != -1)
            {
                third = AVLTree.GetNode(first._leftChild);
                UINodes[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = third._key.ToString();
                UINodes[3].GetChild(1).GetComponent<TextMeshProUGUI>().text = third._height.ToString();
            }
            else
            {
                UINodes[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[3].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
            if(first._rightChild != -1)
            {
                fourth = AVLTree.GetNode(first._rightChild);
                UINodes[4].GetChild(0).GetComponent<TextMeshProUGUI>().text = fourth._key.ToString();
                UINodes[4].GetChild(1).GetComponent<TextMeshProUGUI>().text = fourth._height.ToString();
            }
            else
            {
                UINodes[4].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[4].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
        }
        if(second != null)
        {
            UINodes[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = second._key.ToString();
            UINodes[2].GetChild(1).GetComponent<TextMeshProUGUI>().text = second._height.ToString();
            if (second._leftChild != -1)
            {
                fifth = AVLTree.GetNode(second._leftChild);
                UINodes[5].GetChild(0).GetComponent<TextMeshProUGUI>().text = fifth._key.ToString();
                UINodes[5].GetChild(1).GetComponent<TextMeshProUGUI>().text = fifth._height.ToString();
            }
            else
            {
                UINodes[5].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[5].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
            if (second._rightChild != -1)
            {
                sixth = AVLTree.GetNode(second._rightChild);
                UINodes[6].GetChild(0).GetComponent<TextMeshProUGUI>().text = sixth._key.ToString();
                UINodes[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = sixth._height.ToString();
            }
            else
            {
                UINodes[6].GetChild(0).GetComponent<TextMeshProUGUI>().text = "NULL";
                UINodes[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = "NULL";
            }
        }

        //ShowNode(AVLTree._rootPosition, 0);
    }

    /*public void ShowNode(long currentNodePosition, int i)
    {
        Node current;
        if(currentNodePosition != -1)
        {
            current = AVLTree.GetNode(currentNodePosition);
            UINodes[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = current._key.ToString();
            UINodes[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = current._height.ToString();
        }
        else
        {
            current
        }
        UINodes[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = current._key.ToString();
        UINodes[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = current._height.ToString();
        ShowNode
    }*/
}
