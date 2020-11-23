﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainingBlink : MonoBehaviour

{
    public StimulusSender theSender = null;
    ColorBlock cb;
    public Button Button0; //image to toggle
    public Button Button1; //image to toggle
    public Button Button2; //image to toggle
    public Button Button3; //image to toggle  
    public Button Button4; //image to toggle 
    public Button Button5; //image to toggle 
    public Button Button6; //image to toggle

    public int num_of_blink_arrow = 2;
    public float current_time = 0.0f;

    public float interval = 0.1f;
    public float startDelay = 0.0f;
    public float timebetweenarrows = 0.1f;

    public int blinkcnt = 0;
    public int BlinkCount = 210;

    bool isBlinking = false;
    bool but0 = true, but1 = true, but2 = true, but3 = true, but4 = true, but5 = true, but6 = true;
    public int cnt_but0 = 0, cnt_but1 = 0, cnt_but2 = 0, cnt_but3 = 0, cnt_but4 = 0, cnt_but5 = 0, cnt_but6 = 0;
    int count = 0;
    public byte buttonIndexNum = 0;
    int targetChange = 0;
    int rndnum = 0;

    public byte finish = 9;

    public int[] ranArr = { 0, 1, 2, 3, 4, 5, 6 };

    bool blinkstate = true;
    Button pubimg;
    public Text text;
    void Start()
    {
        theSender = new StimulusSender();
        theSender.open("localhost", 12140);
        cb.normalColor = Color.gray;
        cb.colorMultiplier = 1.5f;
        Button0.colors = cb;
        Button1.colors = cb;
        Button2.colors = cb;
        Button3.colors = cb;
        Button5.colors = cb;
        Button4.colors = cb;
        Button6.colors = cb;
        text.text = "Wait...";
        blinkstate = true;
        print("blinkstate:" + blinkstate);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Menu");
        }
        current_time += Time.deltaTime;
        if (targetChange == 7 && current_time < 10.0f)
        {
            text.text = "Finish";
            blinkstate = false;
            if (current_time > 5.0f) {
                theSender.send(finish);
                theSender.close();
            }
            if (current_time > 5.0f) SceneManager.LoadScene("Menu");
        }
        if (current_time > 3.0f && blinkstate == true)
        {
            int targetNumber = targetChange + 1;
            
            text.text = "Look at "+ targetNumber + " button";
        }
        //Restart blinking
        if (current_time > 5.0f && blinkstate == true)
        {
            blinkstate = false;
            but0 = true;
            but1 = true;
            but2 = true;
            but3 = true;
            but4 = true;
            but5 = true;
            but6 = true;
            blinkcnt = 0;
            BlinkButton();
        }
        /*else if (current_time > 41.0f)
        {
            current_time = 0.0f;
            blinkstate = true;
        }*/
    }
    public void BlinkButton()
    {
        if (blinkcnt < BlinkCount)
        {
            rndnum = UnityEngine.Random.Range(0, 7);
            if (rndnum == 0 && but0 == true)
            {
                but0 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 1;
                theSender.send(buttonIndexNum);

                pubimg = Button0;

                if (isBlinking)
                    return;
                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 0 && but0 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 1 && but1 == true)
            {
                but1 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 2;
                theSender.send(buttonIndexNum);

                pubimg = Button1;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 1 && but1 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 2 && but2 == true)
            {
                but2 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 3;
                theSender.send(buttonIndexNum);

                pubimg = Button2;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }
            }
            else if (rndnum == 2 && but2 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 3 && but3 == true)
            {
                but3 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 4;
                theSender.send(buttonIndexNum);

                pubimg = Button3;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 3 && but3 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 4 && but4 == true)
            {
                but4 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 5;
                theSender.send(buttonIndexNum);

                pubimg = Button4;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 4 && but4 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 5 && but5 == true)
            {
                but5 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 6;
                theSender.send(buttonIndexNum);

                pubimg = Button5;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 5 && but5 == false)
            {
                BlinkButton();
            }
            else if (rndnum == 6 && but6 == true)
            {
                but6 = false;

                if (ranArr[targetChange] == rndnum)
                    buttonIndexNum = 1;
                else
                    buttonIndexNum = 0;
                //buttonIndexNum = 6;
                theSender.send(buttonIndexNum);

                pubimg = Button6;

                if (isBlinking)
                    return;

                if (pubimg != null)
                {
                    isBlinking = true;
                    InvokeRepeating("ToggleState", startDelay, interval);
                }

            }
            else if (rndnum == 6 && but6 == false)
            {
                BlinkButton();
            }

            if (but0 == false && but1 == false && but2 == false && but3 == false && but4 == false && but5 == false && but6 == false)
            {
                but0 = true;
                but1 = true;
                but2 = true;
                but3 = true;
                but4 = true;
                but5 = true;
                but6 = true;
            }
        }
        else
        {
            targetChange++;
            current_time = 0.0f;
            blinkstate = true;
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

            if (rndnum == 0) cnt_but0++;
            else if (rndnum == 1) cnt_but1++;
            else if (rndnum == 2) cnt_but2++;
            else if (rndnum == 3) cnt_but3++;
            else if (rndnum == 4) cnt_but4++;
            else if (rndnum == 5) cnt_but5++;
            else cnt_but6++;

            count = 0;
            isBlinking = false;
            Invoke("BlinkButton", timebetweenarrows);
        }
    }

}