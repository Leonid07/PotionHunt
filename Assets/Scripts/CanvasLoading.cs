using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasLoading : MonoBehaviour
{
    public static CanvasLoading InstanceCanvasLoading { get; private set; }

    private void Awake()
    {
        if (InstanceCanvasLoading != null && InstanceCanvasLoading != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceCanvasLoading = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public GameObject canvasPanel;
    public TMP_Text loadingText;
}
