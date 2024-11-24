using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMover : MonoBehaviour
{
    public RectTransform imageTransform;  // Image, ������� ����� ����������
    public float moveDuration = 2f;       // ������������ ����������� � ���� �������
    public float delayBetweenMoves = 1f;  // �������� ����� ����������

    public bool isActiveFinger = true;

    private Vector2 startPos;             // ��������� ������� Image
    private Vector2 centerPos;            // ������� ������ �������

    private Coroutine moveCoroutine;      // ������ �� ���������� ��������

    void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        startPos = imageTransform.anchoredPosition;   // ���������� ��������� �������
        centerPos = Vector2.zero;  // ����� ������� (��� RectTransform - ��� Vector2.zero)
    }

    public void MoveImageRect()
    {
        moveCoroutine = StartCoroutine(MoveImage());
    }

    public void StopMoving()
    {
        StopCoroutine(moveCoroutine);  // ������������� ��������
    }

    IEnumerator MoveImage()
    {
        while (true)
        {
            // ����������� � ������
            yield return StartCoroutine(MoveToPosition(imageTransform, centerPos, moveDuration));

            // �������� ����� �������� ���������
            yield return new WaitForSeconds(delayBetweenMoves);

            // ����������� ������� � ��������� �������
            yield return StartCoroutine(MoveToPosition(imageTransform, startPos, moveDuration));

            // �������� ����� ��������
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
            yield return null;  // ��� �� ���������� �����
        }

        obj.anchoredPosition = targetPos;  // ������������� ������ ��������� �������
    }
}
