using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disguiseDisappear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startEffect());
    }
    IEnumerator startEffect()
    {
        for (int j = 3; j > 0; j--)
        {
            print("Exploding in " + j);
            yield return new WaitForSeconds(1.0f);
        }
        disguiseGone("disguise4");
    }
    public void disguiseGone(string tag)
    {
        bool first = true;
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
    }
    IEnumerator objFadeout(GameObject obj)
    {
        yield return new WaitForSeconds(5.0f);
        for (int i = 20; i > 0; i--)
        {
            if (i%2 == 0)
            {
                obj.GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(Random.Range(1.0f, 20.0f) / 100f);
            }
            else
            {
                obj.GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(Random.Range(1.0f, 20.0f)/100f);
            }
        }
        obj.SetActive(false);
    }
}
