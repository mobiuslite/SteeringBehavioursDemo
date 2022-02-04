using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float smoothAmount = 5f;

    Vector3 smoothPos = Vector3.zero;

    float startingY;

    private void Start()
    {
        startingY = this.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetFixed = target.position;
        targetFixed.y = startingY;

        Vector3 dir = targetFixed - this.transform.position;

        float dis = dir.magnitude;

        smoothPos += dir.normalized * smoothAmount * Time.deltaTime * dis;
        smoothPos.y = startingY;

        transform.position = smoothPos;
    }
}
