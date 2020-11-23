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

using TelloLib;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

public class Move_with_ControlPanel_Seven_AR : SingletonMonoBehaviour<Move_with_ControlPanel_Seven_AR>
{
    public float fraction_of_way_there;
    public float current_time = 0.0f;
    public static int outpNum = 0;
    public static int ControlPanel_On_Off = 0;
    public GameObject ControlPanel;
    public static float Total_time = 0.0f;
    private static bool isLoaded = false;
    private TelloVideoTexture telloVideoTexture;
    public float move_unit = 1.0f;
    float lx = 0f;
    float ly = 0f;
    float rx = 0f;
    float ry = 0f;
    public bool finished = false;
    public int Max_mmove = 15;
    // FlipType is used for the various flips supported by the Tello.
    public enum FlipType
    {

        // FlipFront flips forward.
        FlipFront = 0,

        // FlipLeft flips left.
        FlipLeft = 1,

        // FlipBack flips backwards.
        FlipBack = 2,

        // FlipRight flips to the right.
        FlipRight = 3,

        // FlipForwardLeft flips forwards and to the left.
        FlipForwardLeft = 4,

        // FlipBackLeft flips backwards and to the left.
        FlipBackLeft = 5,

        // FlipBackRight flips backwards and to the right.
        FlipBackRight = 6,

        // FlipForwardRight flips forewards and to the right.
        FlipForwardRight = 7,
    };

    // VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
    public enum VideoBitRate
    {
        // VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
        VideoBitRateAuto = 0,

        // VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
        VideoBitRate1M = 1,

        // VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
        VideoBitRate15M = 2,

        // VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
        VideoBitRate2M = 3,

        // VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
        VideoBitRate3M = 4,

        // VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
        VideoBitRate4M = 5,

    };
    override protected void Awake()
    {
        if (!isLoaded)
        {
            DontDestroyOnLoad(this.gameObject);
            isLoaded = true;
        }
        base.Awake();

        Tello.onConnection += Tello_onConnection;
        Tello.onUpdate += Tello_onUpdate;
        Tello.onVideoData += Tello_onVideoData;

        if (telloVideoTexture == null)
            telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

    }
    private void OnEnable()
    {
        if (telloVideoTexture == null)
            telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
    }
    void Start()
    {
        if (telloVideoTexture == null)
            telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

        Tello.startConnecting();
        
    }
    void OnApplicationQuit()
    {
        Tello.land();
        Tello.stopConnecting();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Tello.takeOff();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Tello.land();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            finished = true;
            Tello.land();
            SceneManager.LoadScene("Menu");
        }else if (Input.GetKeyDown(KeyCode.H))//H for hijack
        {
            finished = true;
        }
        if (!finished)
        {
            Total_time += Time.deltaTime;
            if (ControlPanel_On_Off == 0)
            {
                current_time += Time.deltaTime;
            }
            if (current_time > 7.0f && ControlPanel_On_Off == 0) //Movement ends
            {
                current_time = 0.0f;
                outpNum = 0;
                if (ControlPanel_Blink_Seven_AR_NoSender.moveCount >= Max_mmove)
                {
                    finished = true;
                    Tello.land();
                    Tello.stopConnecting();
                    SceneManager.LoadScene("Menu");
                }
                else
                {
                    ControlPanel_On_Off = 1;
                    ControlPanel.SetActive(true);
                }
            }
            if (current_time > 1.0)
            {
                lx = 0f;
                ly = 0f;
                rx = 0f;
                ry = 0f;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            if (outpNum == 1)//Forward
            {
                ry = move_unit;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 2)//UP
            {
                ly = move_unit;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 3)//Right turn
            {
                lx = 0.5f;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 4)//Right
            {
                rx = move_unit;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 5)//Down
            {
                ly = -move_unit;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 6)//Left
            {
                rx = -move_unit;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
            else if (outpNum == 7)//Left turn
            {
                lx = -0.5f;
                outpNum = 0;
                Tello.controllerState.setAxis(lx, ly, rx, ry);
            }
        }
        else
        {
            lx = 0f;
            ly = 0f;
            rx = 0f;
            ry = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                ry = 1;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                ry = -1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rx = 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rx = -1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                ly = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                ly = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                lx = 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                lx = -1;
            }
            Tello.controllerState.setAxis(lx, ly, rx, ry);
        }


    }
    private void Tello_onUpdate(int cmdId)
    {
        //throw new System.NotImplementedException();
        //Debug.Log("Tello_onUpdate : " + Tello.state);
    }

    private void Tello_onConnection(Tello.ConnectionState newState)
    {
        //throw new System.NotImplementedException();
        //Debug.Log("Tello_onConnection : " + newState);
        if (newState == Tello.ConnectionState.Connected)
        {
            Tello.queryAttAngle();
            Tello.setMaxHeight(50);

            Tello.setPicVidMode(1); // 0: picture, 1: video
            Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
            //Tello.setEV(0);
            Tello.requestIframe();
            Tello.takeOff();
        }
    }

    private void Tello_onVideoData(byte[] data)
    {
        //Debug.Log("Tello_onVideoData: " + data.Length);
        if (telloVideoTexture != null)
            telloVideoTexture.PutVideoData(data);
    }
}
