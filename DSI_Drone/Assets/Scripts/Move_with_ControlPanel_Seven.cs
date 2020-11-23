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

public class Move_with_ControlPanel_Seven : MonoBehaviour
{
    Vector3 End_pos;
    Vector3 Start_pos;
    public float fraction_of_way_there;
    public float current_time = 0.0f;
    float speed;
    int move_unit;
    public static int Map_num = 1; // 1 : forest, 2: desert, 3: maze, 4: park
    public static int Mode = 0;
    public static int outpNum = 0;
    public static int ControlPanel_On_Off = 0;
    public GameObject ControlPanel;
    double Goal_distance = 24.0;
    System.Random rand = new System.Random();
    public static int Dir = 0;
    public static float Total_time = 0.0f;
    void Start()
    {
        switch (Map_num)
        {
            case 1:
                transform.position = new Vector3(-240, 105, 250);
                break;
            case 2:
                transform.position = new Vector3(130, 85, 570);
                break;
            case 3:
                transform.position = new Vector3(240, 65, 240);
                break;
            case 4:
                transform.position = new Vector3(-150, 95, 570);
                break;
            default:
                Debug.Log("map sellection fail\n");
                break;
        }
        Start_pos = transform.position;
        End_pos = transform.position;
        move_unit = 25;
        speed = 15.0f;

    }

    // Update is called once per frame
    void Update()
    {
        Total_time += Time.deltaTime;
        if (ControlPanel_On_Off == 0)
        {
            current_time += Time.deltaTime;
        }
        if (current_time > 3.0f && ControlPanel_On_Off == 0) //Movement ends
        {
            current_time = 0.0f;
            outpNum = 0;
            ControlPanel_On_Off = 1;
            ControlPanel.SetActive(true);
        }
        if (outpNum == 1)//Forward
        {
            Set(0, 0, move_unit);

        }
        else if (outpNum == 2)//UP
        {
            Set(0, move_unit, 0);
        }
        else if (outpNum == 3)//Right turn
        {
            StartCoroutine(RotateMe(Vector3.up * 90, 2.0f));
            Dir = (Dir + 1) % 4;
        }
        else if (outpNum == 4)//Right
        {
            Set(move_unit, 0, 0);
        }
        else if (outpNum == 5)//Down
        {
            Set(0, -move_unit, 0);
        }
        else if (outpNum == 6)//Left
        {
            Set(-move_unit, 0, 0);
        }
        else if (outpNum == 7)//Left turn
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
        outpNum = 0;
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
        outpNum = 0;
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
