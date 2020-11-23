﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Blink : MonoBehaviour
{
    public Text txt;
    public int random = 0;
    public string path = "";

    public StimulusSender theSender = null;
    public StimulusSender theListener = null;
    ColorBlock cb;

    // NorthA = Up, SouthA = Left, Asia = Right, Africa = Backward, Oceania = Down, Europe = Forward
    public Button Up; //image to toggle
    public Button Left; //image to toggle
    public Button Right; //image to toggle
    public Button Backward; //image to toggle  
    public Button Down; //image to toggle 
    public Button Forward; //image to toggle 

    public int num_of_blink_arrow = 2;
    public float current_time = 0.0f;

    public float interval = 0.1f;
    public float startDelay = 0.1f;
    public float timebetweenarrows = 0.1f;

    public int blinkcnt = 0;
    public int BlinkCount = 120;

    bool isBlinking = false;
    bool Button0 = true, Button1 = true, Button2 = true, Button3 = true, Button4 = true, Button5 = true;
    public int up = 0, left = 0, right = 0, bward = 0, down = 0, fward = 0;
    int count = 0;
    public byte buttonIndexNum = 0;
    int rndnum = 0;

    public byte finish = 7;
    public byte start = 8;

    public int[] ranArr = { 0, 1, 2, 3, 4, 5 };
    public string outp = "";
    public string output = "";
    //string ipUIVAServer = "localhost";
    //public UIVA_Client theClient = null;

    bool blinkstate = true;
    Button pubimg;

    RectTransform noArect;

    void Start()
    {
        random = UnityEngine.Random.Range(1, 7);
        switch (random)
        {
            case 1:
                path = "Up";
                break;
            case 2:
                path = "Forward";
                break;
            case 3:
                path = "Right";
                break;
            case 4:
                path = "Down";
                break;
            case 5:
                path = "Backward";
                break;
            case 6:
                path = "Left";
                break;
            default:
                break;
        }
        txt.text = "Look at " + path;
        
        theSender = new StimulusSender();
        theSender.open("localhost", 12140);
        theListener = new StimulusSender();
        theListener.open("localhost", 12240);
        //theClient = new UIVA_Client(ipUIVAServer);
        //theClient.Press_O(start);
        cb.normalColor = Color.gray;
        cb.colorMultiplier = 1.5f;
        Up.colors = cb;
        Left.colors = cb;
        Right.colors = cb;
        Backward.colors = cb;
        Forward.colors = cb;
        Down.colors = cb;
        noArect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Menu");
        }
        current_time += Time.deltaTime;
        //Restart blinking
        if (current_time > 5.0f && blinkstate == true)
        {
            blinkstate = false;
            Button0 = true;
            Button1 = true;
            Button2 = true;
            Button3 = true;
            Button4 = true;
            Button5 = true;
            blinkcnt = 0;
            BlinkButton();
        }
    }
    public void BlinkButton()
    {
        if (blinkcnt < BlinkCount)
        {
            rndnum = UnityEngine.Random.Range(0, 6);
            if (rndnum == 0 && Button0 == true)
            {
                Button0 = false;
                buttonIndexNum = 1;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Up;

                if (isBlinking)
                    return;
                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 0 && Button0 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 1 && Button1 == true)
            {
                Button1 = false;
                buttonIndexNum = 2;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Forward;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 1 && Button1 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 2 && Button2 == true)
            {
                Button2 = false;
                buttonIndexNum = 3;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Right;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 2 && Button2 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 3 && Button3 == true)
            {
                Button3 = false;
                buttonIndexNum = 4;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Down;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 3 && Button3 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 4 && Button4 == true)
            {
                Button4 = false;
                buttonIndexNum = 5;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Backward;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 4 && Button4 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 5 && Button5 == true)
            {
                Button5 = false;
                buttonIndexNum = 6;
                //theClient.Press_O(buttonIndexNum);
                theSender.send(buttonIndexNum);
                pubimg = Left;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 5 && Button5 == false)
            {
                BlinkButton();
            }

            if (Button0 == false && Button1 == false && Button2 == false && Button3 == false && Button4 == false && Button5 == false)
            {
                Button0 = true;
                Button1 = true;
                Button2 = true;
                Button3 = true;
                Button4 = true;
                Button5 = true;
            }
        }
        else
        {
            System.Threading.Thread.Sleep(1000);
            theSender.send(finish);
            output = theListener.receive();
            outp = output.Substring(0, 28);
            theSender.send(start);
            theSender.close();
            theListener.close();
            System.Threading.Thread.Sleep(1000);
            FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/" + InputName.patient_id + ".txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
            writer.WriteLine("Order: " + random + " / Result: " + outp[27]);
            writer.Close();
            switch (outp)
            {
                case "OVTK_StimulationId_Number_01":
                    Move_with_ControlPanel.outpNum = 1;
                    break;
                case "OVTK_StimulationId_Number_02":
                    Move_with_ControlPanel.outpNum = 2;
                    break;
                case "OVTK_StimulationId_Number_03":
                    Move_with_ControlPanel.outpNum = 3;
                    break;
                case "OVTK_StimulationId_Number_04":
                    Move_with_ControlPanel.outpNum = 4;
                    break;
                case "OVTK_StimulationId_Number_05":
                    Move_with_ControlPanel.outpNum = 5;
                    break;
                case "OVTK_StimulationId_Number_06":
                    Move_with_ControlPanel.outpNum = 6;
                    break;
                default:
                    Move_with_ControlPanel.outpNum = 0;
                    break;
            }
            SceneManager.LoadScene("Map");
        }
    }
    public void ToggleState()
    {
        if (cb.normalColor == Color.gray)
        {
            cb.normalColor = Color.yellow;
            pubimg.colors = cb;
        }
        else
        {
            cb.normalColor = Color.gray;
            pubimg.colors = cb;
        }
        count++;
        if (count == num_of_blink_arrow)
        {
            CancelInvoke();
            blinkcnt++;

            if (rndnum == 0) up++;
            else if (rndnum == 1) fward++;
            else if (rndnum == 2) right++;
            else if (rndnum == 3) down++;
            else if (rndnum == 4) bward++;
            else left++;

            count = 0;
            isBlinking = false;
            Invoke("BlinkButton", timebetweenarrows);
        }
    }

}