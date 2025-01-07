using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerExtension : Singleton<SceneManagerExtension>
{
    private BaseScene[] scenes;

    protected override void Awake()
    {
        base.Awake();
        RegisterScenes();
    }

    private void RegisterScenes()
    {
        //TODO SceneName을 Enum으로 해서 BaseScene배열과 SceneName위치 통일시키기
        scenes = new BaseScene[2];
        scenes[0] = new TitleScene();
        scenes[1] = new OneCycleScene();
    }

    public void LoadScene(SceneType sceneType)
    {
        ResetWork();
        SceneManager.LoadScene(sceneType.ToString());
        SceneManager.sceneLoaded += HandleSceneLoaded;
        
        void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == sceneType.ToString())
            {
                scenes[(int)sceneType].Init();
                SceneManager.sceneLoaded -= HandleSceneLoaded;
            }
        }
    }

    private void ResetWork()
    {
        StageManager.Instance.ResetStageManager();
        PoolManager.Instance.ResetAllObjectPool();
        UIManager.Instance.ClosePersistentUI();
    }

}
