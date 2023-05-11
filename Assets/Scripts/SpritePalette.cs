using UnityEngine;

[System.Serializable]
public class SpritePalette : MonoBehaviour
{
    public Sprite[] spriteArray;

    public Sprite GetRandomSprite()
    {
        if (spriteArray.Length == 0)
        {
            Debug.LogError("The sprite array is empty!");
            return null;
        }
        
        var randomSprite = spriteArray[Random.Range(0, spriteArray.Length)];
        return randomSprite;
    }
}