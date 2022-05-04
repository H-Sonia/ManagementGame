using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject dorm, Boxing, Kitchen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRoomState(GameObject gin)
    {
        gin.SetActive(!gin.active);
    }
}
