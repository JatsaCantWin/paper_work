using UnityEngine;

[System.Serializable]
public class PrefabPalette : MonoBehaviour
{
    public GameObject[] prefabArray;

    public GameObject GetRandomPrefab()
    {
        if (prefabArray.Length == 0)
        {
            Debug.LogError("The prefab array is empty!");
            return null;
        }

        var randomPrefab = prefabArray[Random.Range(0, prefabArray.Length)];
        return randomPrefab;
    }
}
