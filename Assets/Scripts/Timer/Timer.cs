using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TMP_Text timerText;        // ��������� ���� ��� ����������� �������
    public GameObject panel;      // ������, ������� ������������ �� ��������� �������
    public TMP_Text textWin;
    public string winText = "ALL RIGHT!"; 
    private float timeRemaining = 60f;  // ����� � �������� (2 ������)

    private Coroutine timerCoroutine;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        timerCoroutine = StartCoroutine(StartTimer());  // ��������� ������-��������
    }

    IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);  // ��� 1 �������
            timeRemaining--;
        }

        // ����� ������ ����� 00:00
        timerText.text = "00:00";
        panel.SetActive(true);  // ���������� ������
        textWin.text = winText;
        StopCoroutine(timerCoroutine);  // ������������� ��������
    }
}