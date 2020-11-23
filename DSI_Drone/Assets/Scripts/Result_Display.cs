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
public class Result_Display : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI guideText;
    private string[] r_texts = {"Congratulations!","You have finished your journey!", "Mr. Montague and Ms. Capulet sent their gratitudes for trying your best!","Let's take a look at your results!", };
    private string end_text = "Returning home in ";
    public TextMeshProUGUI[] resultText = new TextMeshProUGUI[4];
    public Image[] resultImage = new Image[4];
    public Sprite[] resultSprite = new Sprite[4];
    //public bool[] stageResult = { true, false, true, true };
    void Start()
    {
        for(int i =0;i< Pandora_ControlPanelBlink_NoSender.stageResult.Length; i++)
        {
            if (Pandora_ControlPanelBlink_NoSender.stageResult[i])
            {
                resultText[i].text = "Stage " + (i + 1) + " Clear!";
                resultImage[i].sprite = resultSprite[i];
            }
            else
            {
                resultText[i].text = "Stage " + (i + 1) + " Fail..";
            }
        }
        StartCoroutine(startDisplay());
    }
    IEnumerator startDisplay()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < r_texts.Length; i++)
        {
            for (int j = 0; j <= r_texts[i].Length; j++)
            {
                guideText.text = r_texts[i].Substring(0, j);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < 4; i++)
        {
            StartCoroutine(FadeInText(1.0f ,resultText[i]));
            StartCoroutine(FadeInImage(resultImage[i]));
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(5.0f);
        for (int j = 0; j <= end_text.Length; j++)
        {
            guideText.text = end_text.Substring(0, j);
            yield return new WaitForSeconds(0.1f);
        }
        for (int j = 5; j > 0; j--)
        {
            guideText.text = end_text+"\n"+j;
            yield return new WaitForSeconds(1.0f);
        }
        SceneManager.LoadScene("Menu");
    }
    IEnumerator FadeInImage(Image image)
    {
        float targetAlpha = 1.0f;
        float FadeRate = 0.5f;
        Color curColor = image.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            Debug.Log(image.material.color.a);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
            image.color = curColor;
            yield return null;
        }
    }
    IEnumerator FadeInText(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
