using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public Transform target;

    [Range(0, 1)]
    public float speed;

    public Vector3 offset = new Vector3(0, 0, -10);

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position * speed + (target.position + offset) * (1 - speed);
    }
}
