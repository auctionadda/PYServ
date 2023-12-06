using System.Collections.Generic;
using UnityEngine;
using Vuforia;
 
public class ModelTargetManager : MonoBehaviour
{
    private ModelTargetBehaviour[] modelTargets;
 
    private void Start()
    {
        // Find all Model Target instances in the scene
        modelTargets = FindObjectsOfType<ModelTargetBehaviour>();
 
        // Register the event handler for target tracking
        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.TargetFound += OnTargetFound;
            modelTarget.TargetLost += OnTargetLost;
        }
    }
 
    private void OnTargetFound(object sender, TargetStatusEventArgs args)
    {
        // Handle target found event
        Debug.Log("Target found: " + args.StatusInfo);
        // You can implement your own logic here, such as showing augmented reality content.
    }
 
    private void OnTargetLost(object sender, TargetStatusEventArgs args)
    {
        // Handle target lost event
        Debug.Log("Target lost: " + args.StatusInfo);
        // You can implement your own logic here, such as hiding augmented reality content.
    }
}