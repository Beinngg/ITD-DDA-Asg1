using System;
using Firebase;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var result = task.Result;
            if (result == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready!");
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + result);
            }
        });
    }
}
