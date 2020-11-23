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

public class inputNameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/" + InputName.patient_id + ".txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
        print(InputName.patient_id);
        writer.WriteLine("Result: " + InputName.patient_id);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
