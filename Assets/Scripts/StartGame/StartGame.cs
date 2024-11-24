using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button thisButton;

    public GameObject playerCamera;
    public GameObject mainCanvasCamera;

    private void Start()
    {
        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(()=> { LoadGame(); });
    }

    public void LoadGame()
    {
        playerCamera.SetActive(false);
        mainCanvasCamera.SetActive(true);
    }
}
