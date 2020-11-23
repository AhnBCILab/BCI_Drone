using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyBasedMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 End_pos;
    Vector3 Start_pos;
    public float fraction_of_way_there;
    float speed;
    int move_unit;
    public static int Dir = 0;
    void Start()
    {
        move_unit = 10;
        speed = 15.0f;
        Start_pos = transform.position;
        End_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//Forward
        {
            Set(0, 0, move_unit);

        }
        else if (Input.GetKeyDown(KeyCode.W))//UP
        {
            Set(0, move_unit, 0);
        }
        else if (Input.GetKeyDown(KeyCode.L))//Right turn
        {
            StartCoroutine(RotateMe(Vector3.up * 90, 2.0f));
            Dir = (Dir + 1) % 4;
        }
        else if (Input.GetKeyDown(KeyCode.D))//Right
        {
            Set(move_unit, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))//Down
        {
            Set(0, -move_unit, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))//Left
        {
            Set(-move_unit, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.K))//Left turn
        {
            StartCoroutine(RotateMe(Vector3.up * -90, 2.0f));
            Dir = Dir - 1;
            if (Dir <= -1)
            {
                Dir = 3;
            }
        }
        fraction_of_way_there += (Time.deltaTime * speed) / Vector3.Distance(Start_pos, End_pos);
        transform.position = Vector3.Lerp(Start_pos, End_pos, fraction_of_way_there);
    }
    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }
    public void Set(int x, int y, int z)
    {
        Start_pos = transform.position;
        switch (Dir)
        {
            case 0:
                End_pos = transform.position + new Vector3(x, y, z);
                break;
            case 1:
                End_pos = transform.position + new Vector3(z, y, -x);
                break;
            case 2:
                End_pos = transform.position + new Vector3(-x, y, -z);
                break;
            case 3:
                End_pos = transform.position + new Vector3(-z, y, x);
                break;
        }

        fraction_of_way_there = 0.0f;

    }
}
