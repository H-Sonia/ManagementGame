using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject dorm, boxing, kitchen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRoomState(int num)
    {
        GameObject g = null;
        switch(num)
        {
            case (1):
                g = dorm;
                break;
            case (2):
                g = boxing;
                break;
            case (3):
                g = kitchen;
                break;
        }
        g.SetActive(!g.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
