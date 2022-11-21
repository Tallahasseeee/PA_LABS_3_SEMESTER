using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node
{
    public long _position = -1;
    public long _parent = -1;
    public long _leftChild = -1;
    public long _rightChild = -1;
    public int _key = -1;
    public string _content;
    public int _height = -1;



    public int _contentSize = 50;

    public Node()
    {

    }
    public Node(int key, string content)
    {
        _key = key;
        _content = content;
    }
    public byte[] Serialize()
    {
        byte[] positionBytes = BitConverter.GetBytes(_position);
        byte[] parentBytes = BitConverter.GetBytes(_parent);
        byte[] leftChildBytes = BitConverter.GetBytes(_leftChild);
        byte[] rightChildBytes = BitConverter.GetBytes(_rightChild);
        byte[] keyBytes = BitConverter.GetBytes(_key);
        byte[] contentBytes = new byte[_contentSize];
        byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(_content);

        for (int i = 0; i < contentBytes.Length; i++)
        {
            if(i < stringBytes.Length)
                contentBytes[i] = stringBytes[i];
            else
                contentBytes[i] = System.Text.Encoding.ASCII.GetBytes(" ")[0];
        }

        byte[] heightBytes = BitConverter.GetBytes(_height);

        int bufferSize = positionBytes.Length + parentBytes.Length + leftChildBytes.Length + rightChildBytes.Length + keyBytes.Length + contentBytes.Length + heightBytes.Length;

        byte[] buffer = new byte[bufferSize];

        int index = 0;

        positionBytes.CopyTo(buffer, index);
        index += positionBytes.Length;

        parentBytes.CopyTo(buffer, index);
        index += parentBytes.Length;

        leftChildBytes.CopyTo(buffer, index);
        index += leftChildBytes.Length;

        rightChildBytes.CopyTo(buffer, index);
        index += rightChildBytes.Length;

        keyBytes.CopyTo(buffer, index);
        index += keyBytes.Length;

        contentBytes.CopyTo(buffer, index);
        index += contentBytes.Length;

        heightBytes.CopyTo(buffer, index);
        index += heightBytes.Length;

        return buffer;

    }

    public void Deserialize(byte[] buffer)
    {
        int index = 0;

        _position = BitConverter.ToInt64(buffer[index..(index + sizeof(long))]);
        index += sizeof(long);

        _parent = BitConverter.ToInt64(buffer[index..(index + sizeof(long))]);
        index += sizeof(long);

        _leftChild = BitConverter.ToInt64(buffer[index..(index + sizeof(long))]);
        index += sizeof(long);

        _rightChild = BitConverter.ToInt64(buffer[index..(index + sizeof(long))]);
        index += sizeof(long);

        _key = BitConverter.ToInt32(buffer[index..(index + sizeof(int))]);
        index += sizeof(int);

        _content = System.Text.Encoding.ASCII.GetString(buffer[index..(index + _contentSize)]);
        index += _contentSize;

        _height = BitConverter.ToInt32(buffer[index..(index + sizeof(int))]);
        index += sizeof(int);

    }
}
