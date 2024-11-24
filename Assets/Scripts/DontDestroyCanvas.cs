using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{
    private void Awake()
    {
        // ���� ��� ���������� ��������� ����� �������, ���������� �����
        if (FindObjectsOfType<DontDestroyCanvas>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}