using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDataBase : MonoBehaviour
{
    public ItemData[] allResources;

    public static ResourcesDataBase instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of ResourcesDataBase in this scene");
            return;
        }
        instance = this;
    }
}
