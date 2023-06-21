using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RoomGenerator: MonoBehaviour
{
    public GameObject[] colorizableObjects;
    public GameObject[] variableObjects;
    public GameObject[] prefabObjects;
    
    public void Awake()
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

        foreach (var prefabObject in prefabObjects)
        {
            var prefabPalette = prefabObject.GetComponent<PrefabPalette>();

            if (prefabPalette == null)
            {
                Debug.LogError("Object '" + prefabObject.name + "' does not have a PrefabPalette component.");
            }

            VaryPrefab(prefabObject, prefabPalette.GetRandomPrefab());
        }
    }
    
    protected static void ColorizeObject(GameObject obj, Color color)
    {
        var renderer = obj.GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("Object '" + obj.name + "' does not have a Renderer component.");
            return;
        }

		renderer.material.color = color;
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

    void VaryPrefab(GameObject prefabObject, GameObject randomPrefab)
    {
        if (prefabObject == null || randomPrefab == null)
        {
            Debug.LogError("Prefab object or random prefab is null.");
            return;
        }

        GameObject instantiatedPrefab = Instantiate(randomPrefab);

        instantiatedPrefab.transform.position = prefabObject.transform.position;
        instantiatedPrefab.transform.rotation = prefabObject.transform.rotation;
        instantiatedPrefab.transform.localScale = prefabObject.transform.localScale;

        Destroy(prefabObject);
    }
}
