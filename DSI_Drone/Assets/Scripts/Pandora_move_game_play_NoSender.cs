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

public class Pandora_move_game_play_NoSender : MonoBehaviour
{
    Vector3 End_pos;
    Vector3 Start_pos;
    public GameObject Cam;
    public float fraction_of_way_there;
    public float current_time = 0.0f;
    float speed;
    int move_unit;
    public static int outpNum = 0;
    public static int ControlPanel_On_Off = 0;
    public GameObject ControlPanel;
    public static int Dir = 0;
    public static float Total_time = 0.0f;
    public Vector3[] startPositions = {
        new Vector3(4,20,408),
        new Vector3(220,20,440),
        new Vector3(590,12,360),
        new Vector3(200,20,190)
    };
    public static int Stage = 0;
    public bool pause = false;
    public bool stageCompleteEffect = false;
    public Text GuideText;
    private string m_text = "test for typing effect";
    private string[][] g_texts =
    {
        new string[]{
            "Welcome to Pandora, the Island of Chaos!",
            "You are assigned a mission to find \"Chaotic Spots\" of Pandora!",
            "There are four \"Chaotic Spots\"",
            "Your mission is to find all of them with the Drone infront of you",
            "A Special Control Panel for the Drone will appear soon",
            "Follow the instructions on the Control Panel",
            "If you fail to follow the instructions, the Drone will not move",
            "For the first \"Chaotic Spot\", you need to accomplish TWO instructions",
            "You will be given FOUR chances",
            "Good luck!"
        },
        new string[]{
            "WOW! did you see that??",
            "There was a garden hidden in the Chaotic spot!",
            "The record shows that the garden belongs to Mr. Montague.",
            "Mr. Montague is a resident of Cartoon world..",
            "Strange.. How did a resident of Cartoon world made a garden of REAL world?",
            "I think it is a mystery we have to solve..",
            "For the second \"Chaotic Spot\", you will be given 5 instructions and 10 chances"
        },
        new string[]{
            "Another surprise of the Chaotic spot!",
            "That building was a grocery shop from Italy,",
            "which used to be a famous dating spot for many young lovers..",
            "it is a mystery how that building came all the way from italy to here!",
            "For the third \"Chaotic Spot\", you will be given 7 instructions and 14 chances"
        },
        new string[]
        {
            "The record shows that the garden belongs to Ms. Capulet",
            "There must be an untold story between Mr. Montague and Ms. Capulet..",
            "Let's complete our last mission and discover the truth!",
            "For the last \"Chaotic Spot\", you will be given 10 instructions and 20 chances",
            "You're almost there! Cheer up!"
        }
    };
    private string[] disguise=
    {
        "disguise",
        "disguise2",
        "disguise3",
        "disguise4",
    };
    private bool[] stageCompleteEffect_played = { false, false, false, false };
    void Start()
    {
        
        transform.position = startPositions[Stage];
        transform.rotation = Quaternion.Euler(0, 130, 0);
        Start_pos = transform.position;
        End_pos = transform.position;
        move_unit = 10;
        speed = 15.0f;
        pause = true;
        StartCoroutine(Guide());

    }
    IEnumerator disguiseGone(string tag)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Rigidbody objBody = obj.GetComponent<Rigidbody>();
            // Make sure the rigidbody actually exists!
            if (objBody != null)
            {
                objBody.useGravity = true;
                objBody.isKinematic = false;
            }
            StartCoroutine(objFadeout(obj));
        }
        yield return new WaitForSeconds(7.0f);
        pause = false;
    }
    IEnumerator objFadeout(GameObject obj)
    {
        yield return new WaitForSeconds(5.0f);
        for (int i = 20; i > 0; i--)
        {
            if (i % 2 == 0)
            {
                obj.GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 20.0f) / 100f);
            }
            else
            {
                obj.GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 20.0f) / 100f);
            }
        }
        obj.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        Total_time += Time.deltaTime;
        if (ControlPanel_On_Off == 0 && !pause)
        {
            current_time += Time.deltaTime;
        }
        if (current_time > 3.0f && ControlPanel_On_Off == 0) //Movement ends
        {
            if (CheckStageComplete())
            {//teleport to next stage
                if (stageCompleteEffect && !stageCompleteEffect_played[Stage])
                {
                    pause = true;
                    current_time = 0.0f;
                    StartCoroutine(disguiseGone(disguise[Stage]));
                    stageCompleteEffect_played[Stage] = true;
                }
                else
                {
                    Stage += 1;
                    if (Stage < 4)
                    {
                        pause = true;
                        current_time = 0.0f;
                        print("next Stage");
                        Pandora_ControlPanelBlink_NoSender.StageComplete = false;
                        transform.position = startPositions[Stage];
                        Start_pos = transform.position;
                        End_pos = transform.position;
                        Dir = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0); // initialize everything
                        StartCoroutine(Guide());
                    }
                    else
                    {
                        print("Game clear");
                        SceneManager.LoadScene("Pandora_finish");
                        FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/" + InputName.patient_id + ".txt", FileMode.Append, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
                        writer.WriteLine("CorrectCount: " + Pandora_ControlPanelBlink_NoSender.correctCount + " / TotalCount: " + Pandora_ControlPanelBlink_NoSender.moveCount);
                        writer.Close();
                    }
                }
            }
            else
            {
            current_time = 0.0f;
            outpNum = 0;
            ControlPanel_On_Off = 1;
            ControlPanel.SetActive(true);
            }

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
    public Boolean CheckStageComplete()
    {
        bool result = false;
        result = Pandora_ControlPanelBlink_NoSender.StageComplete;
        //check values in controlPanelBlink
        if (Pandora_ControlPanelBlink_NoSender.stageResult[Stage])
        {
            stageCompleteEffect = true;
        }
        return result;
    }
    IEnumerator Guide()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < g_texts[Stage].Length; i++)
        {
            for (int j = 0; j<=g_texts[Stage][i].Length; j++)
            {
                GuideText.text = g_texts[Stage][i].Substring(0, j);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1.5f);
            GuideText.text = "";
        }
        yield return new WaitForSeconds(1.5f);
        pause = false;
    }
}
