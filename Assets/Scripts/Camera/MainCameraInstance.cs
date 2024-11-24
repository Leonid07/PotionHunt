using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraInstance : MonoBehaviour
{
    public static MainCameraInstance InstanceMainCamera { get; private set; }

    private void Awake()
    {
        if (InstanceMainCamera != null && InstanceMainCamera != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceMainCamera = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}