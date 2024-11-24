using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TMP_Text timerText;        // Текстовое поле для отображения таймера
    public GameObject panel;      // Панель, которая активируется по окончании таймера
    public TMP_Text textWin;
    public string winText = "ALL RIGHT!"; 
    private float timeRemaining = 60f;  // Время в секундах (2 минуты)

    private Coroutine timerCoroutine;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        timerCoroutine = StartCoroutine(StartTimer());  // Запускаем таймер-корутину
    }

    IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);  // Ждём 1 секунду
            timeRemaining--;
        }

        // Когда таймер равен 00:00
        timerText.text = "00:00";
        panel.SetActive(true);  // Активируем панель
        textWin.text = winText;
        StopCoroutine(timerCoroutine);  // Останавливаем корутину
    }
}