using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMover : MonoBehaviour
{
    public RectTransform imageTransform;  // Image, который будем перемещать
    public float moveDuration = 2f;       // Длительность перемещения в одну сторону
    public float delayBetweenMoves = 1f;  // Задержка между движениями

    public bool isActiveFinger = true;

    private Vector2 startPos;             // Начальная позиция Image
    private Vector2 centerPos;            // Позиция центра канваса

    private Coroutine moveCoroutine;      // Ссылка на запущенную корутину

    void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        startPos = imageTransform.anchoredPosition;   // Запоминаем начальную позицию
        centerPos = Vector2.zero;  // Центр канваса (для RectTransform - это Vector2.zero)
    }

    public void MoveImageRect()
    {
        moveCoroutine = StartCoroutine(MoveImage());
    }

    public void StopMoving()
    {
        StopCoroutine(moveCoroutine);  // Останавливаем корутину
    }

    IEnumerator MoveImage()
    {
        while (true)
        {
            // Перемещение к центру
            yield return StartCoroutine(MoveToPosition(imageTransform, centerPos, moveDuration));

            // Задержка перед обратным движением
            yield return new WaitForSeconds(delayBetweenMoves);

            // Перемещение обратно к начальной позиции
            yield return StartCoroutine(MoveToPosition(imageTransform, startPos, moveDuration));

            // Задержка перед повтором
            yield return new WaitForSeconds(delayBetweenMoves);
        }
    }

    IEnumerator MoveToPosition(RectTransform obj, Vector2 targetPos, float duration)
    {
        Vector2 initialPos = obj.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.anchoredPosition = Vector2.Lerp(initialPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  // Ждём до следующего кадра
        }

        obj.anchoredPosition = targetPos;  // Устанавливаем точную финальную позицию
    }
}
