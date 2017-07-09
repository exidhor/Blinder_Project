using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual class used in the PatrolNode. Allow you to bind an action (sleep, attack, animation) on the reach of a node during a patrol. 
/// </summary>
public class PatrolCommande : MonoBehaviour
{
    /// <summary>
    /// Method to override to create the right behaviour in sublasses
    /// </summary>
    public virtual void Execute()
    {

    }
}
