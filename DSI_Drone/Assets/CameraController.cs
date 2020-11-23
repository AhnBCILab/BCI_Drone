using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    //public GameObject player;
    public Transform target;
    public float dist = 5.0f;
    public float height = 10.0f;
    public float smoothRotate = 5.0f;

    private Transform tr;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        //offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

        tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);

        tr.LookAt(target);

    }
}
