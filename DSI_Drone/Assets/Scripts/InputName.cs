﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class InputName : MonoBehaviour
{
    public GameObject[] questionGroupArr;
    public QAClass[] qaArr;
    public GameObject SecondScene;
    //public GameObject Background_world;
    public GameObject Play;
    public GameObject Training;
    public GameObject AR_mode;
    public GameObject Pandora;
    //public GameObject DataAcquire;
    public GameObject Quit;
    public GameObject FitstScene;

    public static string patient_id = "";
    public static string Real_id = "";
    public static int number = 1;
    private static InputName s_Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        qaArr = new QAClass[questionGroupArr.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static InputName instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new GameObject("InputName").AddComponent<InputName>();
            }
            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }
    public void StartGame()
    {
        if(patient_id == "")
        {
            FitstScene.SetActive(false);
            SecondScene.SetActive(true);
        }
        else
        {
            FitstScene.SetActive(false);
            //Background_world.SetActive(true);
            Play.SetActive(true);
            Training.SetActive(true);
            //DataAcquire.SetActive(true);
            Quit.SetActive(true);
            AR_mode.SetActive(true);
            Pandora.SetActive(true);
        }
    }
    public void EnterGame()
    {
        ReadQuestionAndAnswer(questionGroupArr[0]);
        Enter();
    }
    void ReadQuestionAndAnswer(GameObject questionGroup)
    {
        QAClass result = new QAClass();
        GameObject a = questionGroup.transform.Find("InputField").gameObject;
        result.Answer = a.transform.Find("Text").GetComponent<Text>().text;
        FileInfo fi = new FileInfo(Application.dataPath + "/StreamingAssets/" + result.Answer + ".txt");
        patient_id = result.Answer;
        Real_id = result.Answer;
        print(InputName.patient_id);
    }
    public void Enter()
    {
        SecondScene.SetActive(false);
        //Background_world.SetActive(true);
        Play.SetActive(true);
        Training.SetActive(true);
        //DataAcquire.SetActive(true);
        Quit.SetActive(true);
        AR_mode.SetActive(true);
        Pandora.SetActive(true);
    }
}

[System.Serializable]
public class QAClass
{
    public string Answer = "";
}