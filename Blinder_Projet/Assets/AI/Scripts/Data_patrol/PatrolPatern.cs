using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main data of the patrol behaviour. It create a path with nodes (see PatrolNode). The patern can be completly configurable (repeat, repeat in backtrack...).
/// </summary>
public class PatrolPatern : MonoBehaviour
{
    public bool repeating;
    public bool repeatInBacktrack;
    private bool InReverse;

    public PatrolNode[] nodes;
    public int currentNodeIndex;

    void Start()
    {
        InReverse = false;
    }

    /// <summary>
    /// This method allow the patern to switch to the next node
    /// </summary>
    private void NextNode()
    {
        if (InReverse)
            --currentNodeIndex;
        else
            ++currentNodeIndex;

        if (repeating && currentNodeIndex >= nodes.GetLength(0))
        {
            if (repeatInBacktrack)
            {
                InReverse = true;
                currentNodeIndex -= 2;
            }
            else
                currentNodeIndex = 0;
        }

        if (!repeating || !repeatInBacktrack || currentNodeIndex >= 0) return;

        InReverse = false;
        currentNodeIndex += 2;
    }

    /// <summary>
    /// This method allow the patern to switch to the previous node
    /// </summary>
    private void PreviousNode()
    {
        if (InReverse)
            ++currentNodeIndex;
        else
            --currentNodeIndex;

        if (repeating 
            && currentNodeIndex < 0)
        {
            if (repeatInBacktrack)
            {
                InReverse = true;
                currentNodeIndex += 2;
            }
            else
                currentNodeIndex = nodes.GetLength(0) - 1;
        }

        if (!repeating || !repeatInBacktrack || currentNodeIndex < nodes.GetLength(0)) return;

        InReverse = false;
        currentNodeIndex -= 2;
    }

    /// <summary>
    /// This method return the current node
    /// </summary>
    public PatrolNode GetCurrentNode()
    {
        return nodes[currentNodeIndex];
    }
}
