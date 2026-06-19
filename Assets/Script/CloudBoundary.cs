using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBoundary : MonoBehaviour
{
    public float minX = -2.5f;
    public float maxX = 2.5f;




    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
