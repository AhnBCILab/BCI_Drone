using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadMenu : MonoBehaviour
{
    public float current_time = 0.0f;
    public static int result = 0;
    public Text resultText;
    // Start is called before the first frame update
    void Start()
    {
        if(result == 1)
        {
            resultText.text = "Mission Complete";
        }
        else if(result == -1)
        {
            resultText.text = "Mission Fail";
        }
        else
        {
            resultText.text = "Result Error";
        }
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;
        if (current_time > 3.0f){
            SceneManager.LoadScene("Menu");
        }
        
    }
}
