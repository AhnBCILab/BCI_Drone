using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    Vector3 End_pos;
    Vector3 Start_pos;
    public float fraction_of_way_there;
    float speed;
    int move_unit;
    public float current_time = 0.0f;
    public static int outpNum;
    public static bool isFirst = true;
    // Start is called before the first frame update
    void Start()
    {
        if (isFirst)
        {
            Start_pos = transform.position;
            End_pos = transform.position;
            isFirst = false;
            Debug.Log("First");
        }
        else
        {
            Start_pos = GetVector3("pos");
            End_pos = GetVector3("pos");
            transform.position = Start_pos;
        }
        
        move_unit = 10;
        speed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;
        if (current_time > 5.0f)
        {
            current_time = 0.0f;
            outpNum = 0;
            SetVector3("pos", transform.position);
            Debug.Log(transform.position.x);
            Debug.Log(transform.position.y);
            Debug.Log(transform.position.z);
            SceneManager.LoadScene("Main_Blink");
        }

        if (outpNum == 2) // Forward
        {
            Set(0, 0, move_unit);
            outpNum = 0;
        }
        else if (outpNum == 5) // Backward
        {
            Set(0, 0, -move_unit);
            outpNum = 0;
        }
        else if (outpNum == 4) // Down
        {
            Set(0, -move_unit, 0);
            outpNum = 0;
        }
        else if (outpNum == 3) // Right
        {
            Set(move_unit, 0, 0);
            outpNum = 0;
        }
        else if (outpNum == 1) // Up
        {
            Set(0, move_unit, 0);
            outpNum = 0;
        }
        else if (outpNum == 6) // Left
        {
            Set(-move_unit, 0, 0);
            outpNum = 0;
        }
        fraction_of_way_there += (Time.deltaTime * speed) / Vector3.Distance(Start_pos, End_pos);
        transform.position = Vector3.Lerp(Start_pos, End_pos, fraction_of_way_there);
    }

    public void Set(int x, int y, int z)
    {
        End_pos = transform.position + new Vector3(x, y, z);
        fraction_of_way_there = 0.0f;
        Start_pos = transform.position;
    }

    public static void SetVector3(string key, Vector3 value)
    {
        PlayerPrefs.SetFloat(key + "X", value.x);
        PlayerPrefs.SetFloat(key + "Y", value.y);
        PlayerPrefs.SetFloat(key + "Z", value.z);

    }

    public static Vector3 GetVector3(string key)
    {
        Vector3 v3 = Vector3.zero;
        v3.x = PlayerPrefs.GetFloat(key + "X");
        v3.y = PlayerPrefs.GetFloat(key + "Y");
        v3.z = PlayerPrefs.GetFloat(key + "Z");
        return v3;
    }

}
