using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapNModeSelect : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject op1_forest;
    public GameObject op2_desert;
    public GameObject op3_maze;
    public GameObject op4_park;

    public GameObject ModeTextBox;
    public GameObject Mode_op1_correction;
    public GameObject Mode_op2_performance;
    public int choiceMade_Map;
    public int choiceMade_Mode;
    public void choiceOption1(){
        choiceMade_Map = 1;
    }
    public void choiceOption2(){
        choiceMade_Map = 2;
    }
    public void choiceOption3(){
        choiceMade_Map = 3;
    }
    public void choiceOption4(){
        choiceMade_Map = 4;
    }
    public void choiceOption1_Mode()
    {
        choiceMade_Mode = 1;
    }
    public void choiceOption2_Mode()
    {
        choiceMade_Mode = 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(choiceMade_Map >= 1){
            Move_with_ControlPanel_Seven.Map_num = choiceMade_Map;
            op1_forest.SetActive(false);
            op2_desert.SetActive(false);
            op3_maze.SetActive(false);
            op4_park.SetActive(false);
            TextBox.SetActive(false);

            Mode_op1_correction.SetActive(true);
            Mode_op2_performance.SetActive(true);
            ModeTextBox.SetActive(true);
            choiceMade_Map = 0;
        }
        if (choiceMade_Mode >= 1)
        {
            Move_with_ControlPanel_Seven.Mode = choiceMade_Mode;
            SceneManager.LoadScene("Map");
        }

    }
}
