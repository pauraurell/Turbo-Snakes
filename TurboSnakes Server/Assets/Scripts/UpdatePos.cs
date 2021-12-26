using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePos : MonoBehaviour
{
    private Vector3 previous_pos;
    public GameObject serverObj;
    // Start is called before the first frame update
    void Start()
    {
        previous_pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (previous_pos != transform.localPosition)
        {
            SetGameObjectsPosition();
            previous_pos = transform.localPosition;
        }
    }

    private void SetGameObjectsPosition() 
    {
        for (int i = 0; i < serverObj.GetComponent<ServerUDP>().WorldObjects.Count; i++)
        {
            if (serverObj.GetComponent<ServerUDP>().WorldObjects[i].getGameObject() == gameObject)
            {
                serverObj.GetComponent<ServerUDP>().WorldObjects[i].position = transform.localPosition;
            }
        }
    }
}
