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
using TMPro;

public class Preparation_Narration : MonoBehaviour
{
    public TextMeshProUGUI guideText;
    private string[] r_texts = { 
        "Welcome!", "Before we begin our journey,", 
        "Let's take a moment to ease your mind",
        "Take a deep breath and try to clear your mind for 30 seconds",
        "Now, close your eyes and breath..", 
        "We will begin our journey in " };
    private string end_text = "We will begin our journey in ";
    private string end_text2 = "Good Luck!";
    // Start is called before the first frame update
    void Start()
    {
        guideText.text = "";
        StartCoroutine(resting());
    }
    IEnumerator resting()
    {
        for(int i = 0; i < r_texts.Length; i++)
        {
            for(int j = 0; j <= r_texts[i].Length; j++)
            {
                guideText.text = r_texts[i].Substring(0, j);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1.5f);
        }
        for (int j = 30; j > 0; j--)
        {
            guideText.text = end_text + "\n" + j;
            yield return new WaitForSeconds(1.0f);
        }
        for (int j = 0; j <= end_text2.Length; j++)
        {
            guideText.text = end_text2.Substring(0, j);
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("PANDORA");
    }
}
