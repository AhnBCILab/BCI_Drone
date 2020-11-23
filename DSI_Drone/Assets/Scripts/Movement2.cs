using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class Movement2 : MonoBehaviour
{
    Vector3 End_pos;
    Vector3 Start_pos;
    public float fraction_of_way_there;
    float speed;
    int move_unit;
    public static int Map_num; // 1 : forest, 2: desert, 3: maze, 4: park
    public GameObject Goal;
    double Goal_distance = 10.0;
    System.Random rand = new System.Random();
    // Start is called before the first frame update
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
        move_unit = 10;
        speed = 15.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))//Forward
        {
            Set(0, 0, move_unit);
        }
        else if (Input.GetKey(KeyCode.W))//UP
        {
            Set(0, move_unit, 0);
        }
        else if (Input.GetKey(KeyCode.L))//Right turn
        {
           
        }
        else if (Input.GetKey(KeyCode.D))//Right
        {
            Set(move_unit, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))//Down
        {
            Set(0, -move_unit, 0);
        }
        else if (Input.GetKey(KeyCode.A))//Left
        {
            Set(-move_unit, 0, 0);
        }
        else if (Input.GetKey(KeyCode.K))//Left turn
        {
            
        }
        fraction_of_way_there += (Time.deltaTime * speed) / Vector3.Distance(Start_pos, End_pos);
        transform.position = Vector3.Lerp(Start_pos, End_pos, fraction_of_way_there);
        if(check()){
            Debug.Log("Success!\n");
            SceneManager.LoadScene("Success");
        }
    }
    public void Set(int x, int y, int z)
    {
        End_pos = transform.position + new Vector3(x, y, z);
        fraction_of_way_there = 0.0f;
        Start_pos = transform.position;
    }

    public bool check(){
        if(Math.Abs(transform.position.x - Goal.transform.position.x) < Goal_distance && Math.Abs(transform.position.y - Goal.transform.position.y) < Goal_distance && Math.Abs(transform.position.z - Goal.transform.position.z) < Goal_distance){
            return true;
        }else{
             return false;
        }
    }

}
