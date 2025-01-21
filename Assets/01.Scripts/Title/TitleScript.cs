using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TitleScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float blinkDuration = 0.5f; 
    private Tween textBlink;

    void Start()
    {
        textBlink = text.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutQuad);  
    }
    private void Update()
    {
        if (Input.anyKey)
        {
            textBlink.Kill();
            SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene);
        }
    }
}
