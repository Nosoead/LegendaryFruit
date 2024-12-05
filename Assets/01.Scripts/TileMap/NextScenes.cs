using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // SceneAsset 사용을 위한 네임스페이스
#endif

public class NextScene : MonoBehaviour
{
    [SerializeField] private SceneAsset nextSceneAsset; // 에디터에서 씬을 직접 드래그하여 설정
    [SerializeField] private KeyCode interactionKey = KeyCode.F; // 상호작용 키
    private bool isPlayerNearObject = false; // 플레이어가 근처에 있는지 여부

    private void Update()
    {
        // 플레이어가 근처에 있고 설정된 키를 눌렀을 때 씬 이동
        if (isPlayerNearObject && Input.GetKeyDown(interactionKey))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // 씬 에셋이 설정되었는지 확인 후 씬 로드
        if (nextSceneAsset != null)
        {
            string scenePath = AssetDatabase.GetAssetPath(nextSceneAsset);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.LogError("Next scene asset is not set!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearObject = false;
        }
    }
}