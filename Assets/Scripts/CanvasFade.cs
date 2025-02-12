using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float alpha = 0.0f;
    public GameObject UiToHide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn(float fadeDuration)
    {
  
        canvasGroup.alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(FadeIn(fadeDuration));
    }

    public void StartFadeOut(float fadeDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        float elapsedTime = 0.0f;
        while ( alpha <= 1f)
        {
            alpha = (elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UiToHide.SetActive(false);
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float elapsedTime = 0.0f;
        UiToHide.SetActive(true);
        while (alpha >= 0.0f)
        {
            alpha = (1 - (elapsedTime / fadeDuration));
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
