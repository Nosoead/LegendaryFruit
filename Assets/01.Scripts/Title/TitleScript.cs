using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
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
