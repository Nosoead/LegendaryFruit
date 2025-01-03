using UnityEngine;


public class TitleScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManagerExtension.Instance.LoadScene(SceneType.OneCycleScene);
        }
    }
}
