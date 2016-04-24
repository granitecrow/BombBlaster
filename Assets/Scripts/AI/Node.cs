using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
    public List<Node> adjacent = new List<Node>();
    public Node previous = null;        // look at the previous node we looked at to determine path
    public string label = string.Empty;

    public void Clear()
    {
        previous = null;
    }

}
