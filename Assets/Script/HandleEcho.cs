using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEcho : MonoBehaviour
{
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        StartCoroutine(Diffuse());
    }

    private IEnumerator Diffuse()
    {
        float multiplier = 5f;
        float time = 1 / multiplier;
        while(time > 0f)
        {
            mat.color = new Color(1, 0, 0, time * multiplier);
            yield return null;
            time -= Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
