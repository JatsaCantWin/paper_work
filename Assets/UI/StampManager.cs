using UnityEngine;
using TMPro;

public class StampManager : MonoBehaviour
{
    public GameObject documents;
    public GameObject documents_1stamp;
    public GameObject documents_2stamp;
    public GameObject documents_3stamp;
    public TextMeshProUGUI endGameText;

    private int currentStamp = 0;

    private void Start()
    {
        documents.SetActive(true);
        documents_1stamp.SetActive(false);
        documents_2stamp.SetActive(false);
        documents_3stamp.SetActive(false);
        endGameText.gameObject.SetActive(false);
    }

     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            getStamp();
        }
    }

    public void getStamp()
    {
        currentStamp++;

        switch (currentStamp)
        {
            case 1:
                documents.SetActive(false);
                documents_1stamp.SetActive(true);
                break;
            case 2:
                documents_1stamp.SetActive(false);
                documents_2stamp.SetActive(true);
                break;
            case 3:
                documents_2stamp.SetActive(false);
                documents_3stamp.SetActive(true);
                Invoke("EndGame", 1f); 
                break;
        }
    }

    private void EndGame()
    {
        documents_3stamp.SetActive(false);
        endGameText.gameObject.SetActive(true);
    }
}
