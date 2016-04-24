using UnityEngine;
using System.Collections;

public class Graph {
    public int rows = 0;
    public int columns = 0;
    public Node[] nodes;

    public Graph(int[,] grid)
    {
        rows = grid.GetLength(0);
        columns = grid.GetLength(1);

        nodes = new Node[grid.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            var node = new Node();
            node.label = i.ToString();
            nodes[i] = node;
        }

        // iterate through each rows in grid
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                var node = nodes[columns * r + c];      // translate grid to array
                if (grid[r,c] == 1)
                {
                    continue;
                }

                // up
                if (r > 0)
                {
                    node.adjacent.Add(nodes[columns * (r - 1) + c]);
                }

                //right
                if (c < columns - 1)
                {
                    node.adjacent.Add(nodes[columns*r + c + 1]);
                }

                // down
                if (r < rows - 1)
                {
                    node.adjacent.Add(nodes[columns * (r + 1) + c]);
                }

                //left
                if (c > 0)
                {
                    node.adjacent.Add(nodes[columns * r + c - 1]);
                }
            }
        }

    }

}
