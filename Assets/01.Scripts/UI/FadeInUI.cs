using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInUI : UIBase, IPersistentUI
{
    private Image image;
    private Coroutine fadeCoroutine;
  
    private void Awake()
    {
        image = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        StageManager.Instance.OnPlayFadeIn += StartFade;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnPlayFadeIn -= StartFade;
    }

    private void StartFade(StageType type)
    {
        StartCoroutine(FadeCoroutine(type));
    }
    private IEnumerator FadeCoroutine(StageType type)
    {
        image.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.4f);
        yield return FadeOut(type);
    }


    private IEnumerator FadeOut(StageType type)
    {
        float Duration = 1f;
        float elapsedTime = 0f;
        Color startColor = new Color(0, 0, 0, 1);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsedTime / Duration);
            yield return null;
        }
        image.color = endColor;
    }
}
