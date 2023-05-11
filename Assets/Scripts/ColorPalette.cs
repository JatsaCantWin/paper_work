using UnityEngine;

[System.Serializable]
public class ColorPalette : MonoBehaviour
{
    public Color[] colorArray;

    public Color GetRandomColor()
    {
        if (colorArray.Length == 0)
        {
            Debug.LogError("The color array is empty!");
            return Color.white;
        }
        
        var randomColor = colorArray[Random.Range(0, colorArray.Length)];
        return randomColor;
    }
}