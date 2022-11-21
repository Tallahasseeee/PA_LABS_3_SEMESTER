using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AVLTree
{
    public long _rootPosition = -1;

    public int _contentSize = 50;

    public int _nodeByteSize = sizeof(long) * 4 + sizeof(int) * 2 + 50;

    public int _passedNodesAmount = 0;

    private Node _nodeToDelete;
    public string Add(Node node)
    {
        string message;
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.Open)))
        {
            node._position = binaryWriter.BaseStream.Length;
        }
        SetNode(node._position, node);
  
        if (_rootPosition == -1)
        {
            _rootPosition = node._position;
            RootChanged();
            message = "Node added as root";
        }
        else
        {
            _rootPosition = InsertNode(GetNode(_rootPosition), node)._position;
            message = "NodeAdded";
        }
        return message;
    }

    private Node InsertNode(Node current, Node node)
    {
        if(node._key  < current._key)
        {
            if(current._leftChild == -1)
            {
                current._leftChild = node._position;
                node._parent = current._position;
                node._height = -1;
                current = IncrementParentsHeight(current);
                SetNode(current._position, current);
                SetNode(node._position, node);
                return GetNode(current._position);
            }
            current._leftChild = InsertNode(GetNode(current._leftChild), node)._position;
            current = GetNode(current._position);
            current = BalanceTree(current);
        }
        else if(node._key > current._key)
        {
            if (current._rightChild == -1)
            {
                current._rightChild = node._position;
                node._parent = current._position;
                node._height = -1;
                current = IncrementParentsHeight(current);
                SetNode(current._position, current);
                SetNode(node._position, node);
                return GetNode(current._position);
            }
            current._rightChild = InsertNode(GetNode(current._rightChild), node)._position;
            current = GetNode(current._position);
            current = BalanceTree(current);
            //Debug.Log(debug._key.ToString() + "  " + debug._height.ToString());
        }
        SetNode(current._position, current);
        return GetNode(current._position);
    }
    public void Delete(int target)
    {
        _rootPosition = Delete(GetNode(_rootPosition), target)._position;
        DeleteFromMemory(_nodeToDelete);
    }
    private Node Delete(Node current, int target)
    {
        Node parent;
        if (current == null)
        { return null; }
        else
        {
            //left subtree
            if (target < current._key)
            {
                long oldChild = current._leftChild;
                current._leftChild = Delete(GetNode(current._leftChild), target)._position;
                if (oldChild == current._leftChild)
                    current = GetNode(current._position);
                else
                    SetNode(current._position, current);
                if (current._leftChild == -1)
                {
                    DecrementParentsHeight(current, 0);
                }
                if (BalanceFactor(current) == -2)//here
                {
                    if (BalanceFactor(GetNode(current._rightChild)) <= 0)
                    {
                        current = RotateRR(current, true);
                        SetNode(current._position, current);
                    }
                    else
                    {
                        current = RotateRL(current);
                        SetNode(current._position, current);
                    }
                }
            }
            //right subtree
            else if (target > current._key)
            {
                long oldChild = current._rightChild;
                current._rightChild = Delete(GetNode(current._rightChild), target)._position;
                if (oldChild == current._rightChild)
                    current = GetNode(current._position);
                else
                    SetNode(current._position, current);
                if (current._rightChild == -1)
                {
                    DecrementParentsHeight(current, 0);
                }
                if (BalanceFactor(current) == 2)
                {
                    if (BalanceFactor(GetNode(current._leftChild)) >= 0)
                    {
                        current = RotateLL(current, true);
                        SetNode(current._position, current);
                    }
                    else
                    {
                        current = RotateLR(current);
                        SetNode(current._position, current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current._rightChild != -1)
                {
                    parent = GetNode(current._rightChild);
                    while (parent._leftChild != -1)
                    {
                        parent = GetNode(parent._leftChild);
                    }
                    current._key = parent._key;
                    SetNode(current._position, current);
                    long oldChild = current._rightChild;
                    current._rightChild = Delete(GetNode(current._rightChild), parent._key)._position;
                    if (oldChild == current._rightChild)
                        current = GetNode(current._position);
                    else
                        SetNode(current._position, current);
                    if (current._rightChild == -1)
                    {
                        DecrementParentsHeight(current, 0);
                    }
                    if (BalanceFactor(current) == 2)//rebalancing
                    {
                        if (BalanceFactor(GetNode(current._leftChild)) >= 0)
                        {
                            current = RotateLL(current, true);
                            SetNode(current._position, current);
                        }
                        else 
                        { 
                            current = RotateLR(current);
                            SetNode(current._position, current);
                        }
                    }
                }
                else
                {   
                    if (current._leftChild != -1)
                    {
                        _nodeToDelete = current;
                        return GetNode(current._leftChild);
                    }
                    else
                    {
                        _nodeToDelete = current;
                        Node n = new Node();
                        n._position = -1;
                        return n;
                    }
                        
                }
            }
        }
        return GetNode(current._position);
    }
    public string Find(int key)
    {
        Node node = Find(key, GetNode(_rootPosition));
        if (node == null)
        {
            Debug.Log(_passedNodesAmount);
            _passedNodesAmount = 0;
            return "Key is not found";
        }
        else
        {
            Debug.Log(_passedNodesAmount);
            _passedNodesAmount = 0;
            return node._content;
        }
    }

    private Node Find(int target, Node current)
    {
        _passedNodesAmount++;
        if (target < current._key)
        {
            if(target == current._key)
            {
                return current;
            }
            else if(current._leftChild == -1)
            {
                return null;
            }
            else
            {
                return Find(target, GetNode(current._leftChild));
            }
        }
        else
        {
            if (target == current._key)
            {
                return current;
            }
            else if (current._rightChild == -1)
            {
                return null;
            }
            else
            {
                return Find(target, GetNode(current._rightChild));
            }
        }
    }
    private Node BalanceTree(Node current)
    {
        current = GetNode(current._position);
        int bFactor = BalanceFactor(current);
        if (bFactor > 1)
        {
            if (BalanceFactor(GetNode(current._leftChild)) > 0) 
            {
                current = RotateLL(current, true);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if(bFactor < -1)
        {
            if(BalanceFactor(GetNode(current._rightChild)) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current, true);
                //Debug.Log(current._key.ToString() + "  " + current._height.ToString());
            }
        }

        SetNode(current._position, current);
        return GetNode(current._position);
    }

    private Node RotateLL(Node parent, bool changeHeight)
    {
        Node pivot = GetNode(parent._leftChild);
        parent._leftChild = pivot._rightChild;
        pivot._rightChild = parent._position;
        pivot._parent = parent._parent;
        parent._parent = pivot._position;
        if (pivot._parent == -1)
        {
            _rootPosition = pivot._position;
            RootChanged();
        }
        else
        {
            Node grandparent = GetNode(pivot._parent);
            if (pivot._key > grandparent._key)
                grandparent._rightChild = pivot._position;
            else
            {
                grandparent._leftChild = pivot._position;
            }
            SetNode(grandparent._position, grandparent);
        }
        pivot._height++;
        if (!changeHeight)
            parent._height--;
        SetNode(parent._position, parent);
        SetNode(pivot._position, pivot);
        if (changeHeight)
        {
            parent._height--;
            SetNode(parent._position, parent);
            DecrementParentsHeight(parent, 0);
        }
        return GetNode(pivot._position);
    }

    private Node RotateLR(Node parent)
    {
        Node pivot = GetNode(parent._leftChild);
        parent._leftChild = RotateRR(pivot,false)._position;
        return RotateLL(parent, true);
    }

    private Node RotateRL(Node parent)
    {
        Node pivot = GetNode(parent._rightChild);
        parent._rightChild = RotateLL(pivot, false)._position;
        return RotateRR(parent, true);
    }

    private Node RotateRR(Node parent, bool changeHeight)
    {
        Node pivot = GetNode(parent._rightChild);
        parent._rightChild = pivot._leftChild;
        pivot._leftChild = parent._position;
        pivot._parent = parent._parent;
        parent._parent = pivot._position;
        if(pivot._parent == -1)
        {
            _rootPosition = pivot._position;
            RootChanged();
        }
        else
        {
            Node grandparent = GetNode(pivot._parent);
            if(pivot._key > grandparent._key)
                grandparent._rightChild = pivot._position;
            else
            {
                grandparent._leftChild = pivot._position;
            }
            SetNode(grandparent._position, grandparent);
        }
        pivot._height++;
        SetNode(pivot._position, pivot);
        if (!changeHeight)
            parent._height--;
        SetNode(parent._position, parent);
        SetNode(pivot._position, pivot);
        if (changeHeight)
        {
            parent._height--;
            SetNode(parent._position, parent);
            DecrementParentsHeight(parent, 0);
        }
        //Debug.Log(pivot._key.ToString() + "  " + pivot._height.ToString());
        return GetNode(pivot._position);
    }

    private Node DecrementParentsHeight(Node current, int lastBalanceFactor)
    {
        Node node = new Node();
        int newLastBalanceFactor = 0;
        if (current._parent != -1)
        {
            node = GetNode(current._parent);
            newLastBalanceFactor = Mathf.Abs(BalanceFactor(node));
        }
        current._height--;
        SetNode(current._position, current);
        //if(current._key == 2)
            //Debug.Log(current._key.ToString() + "  " + current._height.ToString());
        if (current._parent == -1)
        {
            if (Mathf.Abs(BalanceFactor(GetNode(current._position))) == 1 && lastBalanceFactor != 2)
                current._height++;
            SetNode(current._position, current);
            //Debug.Log(current._key.ToString() + "  " + current._height.ToString());
            return GetNode(current._position);
        }
        else if (Mathf.Abs(BalanceFactor(GetNode(current._position))) == 1 && lastBalanceFactor != 2)
        {
            current._height++;
            SetNode(current._position, current);
            Debug.Log(current._key.ToString() + "  " + current._height.ToString());
            return GetNode(current._position);
        }
        lastBalanceFactor = newLastBalanceFactor;
        //Debug.Log(node._key.ToString() + "  " + node._height.ToString());
        DecrementParentsHeight(node, lastBalanceFactor);
        SetNode(current._position, current);
        Debug.Log(current._key.ToString() + "  " + current._height.ToString());
        return GetNode(current._position);
    }



    private Node IncrementParentsHeight(Node current)
    {
        current._height++;
        SetNode(current._position, current);
        if (current._parent == -1)
        {
            if (BalanceFactor(GetNode(current._position)) == 0)
                current._height--;
            SetNode(current._position, current);
            //Debug.Log(current._key.ToString() + "  " + current._height.ToString());
            return GetNode(current._position);
        }
        else if(BalanceFactor(GetNode(current._parent)) == 0)
        {
            SetNode(current._position, current);
            Debug.Log(current._key.ToString() + "  " + current._height.ToString());
            return GetNode(current._position);
        }
        Node node = new Node();
        if (current._parent != -1)
        {
            node = GetNode(current._parent);
        }
        IncrementParentsHeight(node);
        SetNode(current._position, current);
        //Debug.Log(current._key.ToString() + "  " + current._height.ToString());
        return GetNode(current._position);
    }

    private int BalanceFactor(Node current)
    {
        int l;
        int r;
        if (current._leftChild != -1)
            l = GetNode(current._leftChild)._height;
        else
            l = -2;
        if (current._rightChild != -1)
            r = GetNode(current._rightChild)._height;
        else
            r = -2;
        return l - r;
    }

    public Node GetNode(long position)    
    {
        Node node = new Node();
        using (BinaryReader binaryReader = new BinaryReader(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.Open)))
        {
            binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
            byte[] buffer = binaryReader.ReadBytes(_nodeByteSize);
            node.Deserialize(buffer);
        }
        return node;
    }

    public void SetNode(long position, Node node)
    {
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.Open)))
        {
            binaryWriter.BaseStream.Seek(position, SeekOrigin.Begin);
            node._position = position;
            byte[] buffer = node.Serialize();
            binaryWriter.Write(buffer);
        }
    }

    private void RootChanged()
    {
        using(BinaryWriter binaryWriter = new BinaryWriter(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Root.dat", FileMode.Open)))
        {
            binaryWriter.Write(_rootPosition);
        }
    }

    public void DeleteFromMemory(Node node)
    {
        long fileLength;
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.Open)))
        {
            fileLength = binaryWriter.BaseStream.Length;
        }
        Node lastNode = GetNode(fileLength - _nodeByteSize);
        if(lastNode._position == _rootPosition)
        {
            _rootPosition = node._position;
        }
        lastNode._position = node._position;
        if (lastNode._parent != -1)
        {
            Node parent = GetNode(lastNode._parent);
            if (lastNode._key > parent._key)
            {
                parent._rightChild = lastNode._position;
            }
            else
            {
                parent._leftChild = lastNode._position;
            }
            SetNode(parent._position, parent);
        }
        SetNode(lastNode._position, lastNode);
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open("C:\\Users\\STRIX\\UnityProjects\\PA_LAB_3\\Assets\\Files\\Tree.dat", FileMode.Open)))
        {
            binaryWriter.BaseStream.SetLength(fileLength - _nodeByteSize);
        }
    }
}
