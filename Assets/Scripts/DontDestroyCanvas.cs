using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{
    private void Awake()
    {
        // Если уже существует экземпляр этого канваса, уничтожаем новый
        if (FindObjectsOfType<DontDestroyCanvas>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}