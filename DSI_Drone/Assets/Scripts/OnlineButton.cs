using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEngine.Experimental;

public class OnlineButton : MonoBehaviour
{

    public GameObject Play;
    public GameObject Training;
    public GameObject Quit;
    public GameObject text_title;
    public GameObject AR_mode;
    public GameObject Pandora;

    public GameObject text_selectMap;
    public GameObject op1_forest;
    public GameObject op2_desert;
    public GameObject op3_maze;
    public GameObject op4_park;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeGameScene()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/StreamingAssets/" + InputName.patient_id + ".txt");
        if (fi.Exists) InputName.patient_id = InputName.Real_id + InputName.number;
        InputName.number = InputName.number + 1;
        Play.SetActive(false);
        Training.SetActive(false);
        Quit.SetActive(false);
        text_title.SetActive(false);
        AR_mode.SetActive(false);
        Pandora.SetActive(false);

        text_selectMap.SetActive(true);
        op1_forest.SetActive(true);
        op2_desert.SetActive(true);
        op3_maze.SetActive(true);
        op4_park.SetActive(true);
    }
}