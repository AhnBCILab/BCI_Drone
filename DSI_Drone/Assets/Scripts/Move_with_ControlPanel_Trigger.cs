using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_with_ControlPanel_Trigger : MonoBehaviour
{
    Vector3 End_pos;
    Vector3 Start_pos;
    public float fraction_of_way_there;
    public float current_time = 0.0f;
    float speed;
    int move_unit;
    public static int Map_num; // 1 : forest, 2: desert, 3: maze, 4: park
    public static int outpNum = 0;
    public static int ControlPanel_On_Off = 0;
    public GameObject Goal, ControlPanel;
    double Goal_distance = 10.0;
    System.Random rand = new System.Random();
    void Start()
    {
        switch (Map_num)
        {
            case 1:
                Goal = GameObject.Find("Goal_forest");
                transform.position = new Vector3(-240, 80, 250);
                break;
            case 2:
                Goal = GameObject.Find("Goal_desert");
                transform.position = new Vector3(130, 60, 570);
                break;
            case 3:
                Goal = GameObject.Find("Goal_maze");
                transform.position = new Vector3(240, 40, 240);
                break;
            case 4:
                Goal = GameObject.Find("Goal_park");
                transform.position = new Vector3(-150, 70, 570);
                break;
            default:
                Debug.Log("map sellection fail\n");
                break;
        }
        Start_pos = transform.position;
        End_pos = transform.position;
        ControlPanel.transform.position = new Vector3(0, 5, -1);
        //Debug.Log(ControlPanel.transform.position);
        move_unit = 25;
        speed = 15.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (ControlPanel_On_Off == 0)
        {
            current_time += Time.deltaTime;
        }
        if (current_time > 5.0f && ControlPanel_On_Off == 0) //Movement ends
        {
            current_time = 0.0f;
            outpNum = 0;
            //ControlPanel_On_Off = 1;
            //ControlPanel.SetActive(true); //ViveInput Trigger will change these values 
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
}
