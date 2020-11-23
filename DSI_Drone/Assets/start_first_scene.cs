using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class start_first_scene : MonoBehaviour
{
    public TextMeshProUGUI inputField;
    public GameObject first_scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start()
    {
        string user_id = inputField.text;
        print(user_id);
        first_scene.SetActive(false);
    }
}
