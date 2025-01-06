using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : UIBase
{
    private Image image;
    private Coroutine fadeCoroutine;
    /*public override void Open()
    {
      base.Open();
    }*/

    private void Awake()
    {
        Debug.Log("start");
        image = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        StageManager.Instance.onFadeImage += StartFade;
    }

    private void OnDisable()
    {
        StageManager.Instance.onFadeImage -= StartFade;
    }

    private void StartFade(StageType type)
    {
        StartCoroutine(FadeCoroutine(type));
    }
    private IEnumerator FadeCoroutine(StageType type)
    {
        //yield return FadeIn(type);
        image.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.8f);
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
