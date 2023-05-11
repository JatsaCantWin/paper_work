using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RoomGenerator: MonoBehaviour
{
    public GameObject[] colorizableObjects;
    public GameObject[] variableObjects;
    
    public void Start()
    {
        foreach (var colorizableObject in colorizableObjects)
        {
            var colorPalette = colorizableObject.GetComponent<ColorPalette>();

            if (colorPalette == null)
            {
                Debug.LogError("Object '" + colorizableObject.name + "' does not have a ColorPalette component.");
            }
            
            ColorizeObject(colorizableObject, colorPalette.GetRandomColor());
        }
        
        foreach (var variableObject in variableObjects)
        {
            var spritePalette = variableObject.GetComponent<SpritePalette>();

            if (spritePalette == null)
            {
                Debug.LogError("Object '" + variableObject.name + "' does not have a SpritePalette component.");
            }
            
            VarySprite(variableObject, spritePalette.GetRandomSprite());
        }
    }
    
    protected static void ColorizeObject(GameObject obj, Color color)
    {
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Object '" + obj.name + "' does not have a SpriteRenderer component.");
            return;
        }
        
        spriteRenderer.color = color;
    }

    protected static void VarySprite(GameObject obj, Sprite sprite)
    {
        var spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Object '" + obj.name + "' does not have a SpriteRenderer component.");
            return;
        }
        
        spriteRenderer.sprite = sprite;
    }
}
