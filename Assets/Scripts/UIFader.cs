using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration;

    void Start() => fadeGroup = GetComponent<CanvasGroup>();

    public void FadeIn() => StartCoroutine(FadeCoroutine(0));
    public void FadeOut() => StartCoroutine(FadeCoroutine(1));

    private IEnumerator FadeCoroutine(float targetAlpha)
    {
        float timePassed = 0;
        float startAlpha = fadeGroup.alpha;

        while (timePassed < fadeDuration)
        {
            timePassed += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timePassed / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
    }

    public float GetFadeDuration() => fadeDuration;
    public bool SetInputBlocking(bool value) => fadeGroup.blocksRaycasts = value; 
}
