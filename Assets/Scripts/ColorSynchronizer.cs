using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSynchronizer : MonoBehaviour
{
    public GameObject[] synchronizedObjects;
    
    private void Start()
    {
        var firstObjectColor = synchronizedObjects[0].GetComponent<Renderer>().material.color;

        for (int i = 1; i < synchronizedObjects.Length; i++)
        {
            synchronizedObjects[i].GetComponent<Renderer>().material.color = firstObjectColor;
        }
    }
}
