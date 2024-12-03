using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Ссылка на CanvasGroup
    public float fadeDuration = 1f; // Длительность плавного появления/исчезновения

    private void Start()
    {
        // Начальное состояние CanvasGroup (можно изменить по необходимости)
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Запускает корутину для плавного появления CanvasGroup.
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(0, 1));
    }

    /// <summary>
    /// Запускает корутину для плавного исчезновения CanvasGroup.
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(1, 0));
    }

    /// <summary>
    /// Корутину для управления плавным изменением прозрачности CanvasGroup.
    /// </summary>
    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        // Обновляем alpha CanvasGroup на протяжении fadeDuration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        // Устанавливаем точное конечное значение alpha
        canvasGroup.alpha = endAlpha;

        // Настройка взаимодействия с CanvasGroup
        canvasGroup.interactable = endAlpha > 0.9f;
        canvasGroup.blocksRaycasts = endAlpha > 0.9f;
    }
}
