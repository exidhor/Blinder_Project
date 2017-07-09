using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

/// <summary>
/// A node in the patrolPatern. Le location is the objective of the node and the commande is the action of the object on when it reach the point.
/// </summary>
public class PatrolNode : MonoBehaviour
{
    public Location location;
    public PatrolCommande commande;


    public Location GetLocation()
    {
        return location;
    }

    public PatrolCommande GetCommande()
    {
        return commande;
    }

}
