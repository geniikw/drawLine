using UnityEngine;
using System.Collections.Generic;
using System;

public class BehaviourWithTree : MonoBehaviour, ISerializationCallbackReceiver
{
    //node class that is used at runtime
    public class Node
    {
        public string interestingValue = "value";
        public List<Node> children = new List<Node>();
    }

    //node class that we will use for serialization
    [Serializable]
    public struct SerializableNode
    {
        public string interestingValue;
        public int childCount;
        public int indexOfFirstChild;
    }

    //the root of what we use at runtime. not serialized.
    Node root = new Node();

    //the field we give unity to serialize.
    public List<SerializableNode> serializedNodes;

    public void OnBeforeSerialize()
    {
        //unity is about to read the serializedNodes field's contents. lets make sure
        //we write out the correct data into that field "just in time".
        serializedNodes.Clear();
        AddNodeToSerializedNodes(root);
    }

    void AddNodeToSerializedNodes(Node n)
    {
        var serializedNode = new SerializableNode()
        {
            interestingValue = n.interestingValue,
            childCount = n.children.Count,
            indexOfFirstChild = serializedNodes.Count + 1
        };
        serializedNodes.Add(serializedNode);
        foreach (var child in n.children)
            AddNodeToSerializedNodes(child);
    }

    public void OnAfterDeserialize()
    {
        //Unity has just written new data into the serializedNodes field.
        //let's populate our actual runtime data with those new values.

        if (serializedNodes.Count > 0)
            root = ReadNodeFromSerializedNodes(0);
        else
            root = new Node();
    }

    Node ReadNodeFromSerializedNodes(int index)
    {
        var serializedNode = serializedNodes[index];
        var children = new List<Node>();
        for (int i = 0; i != serializedNode.childCount; i++)
            children.Add(ReadNodeFromSerializedNodes(serializedNode.indexOfFirstChild + i));

        return new Node()
        {
            interestingValue = serializedNode.interestingValue,
            children = children
        };
    }

    void OnGUI()
    {
        Display(root);
    }

    void Display(Node node)
    {
        GUILayout.Label("Value: ");
        node.interestingValue = GUILayout.TextField(node.interestingValue, GUILayout.Width(200));

        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        foreach (var child in node.children)
            Display(child);
        if (GUILayout.Button("Add child"))
            node.children.Add(new Node());

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}