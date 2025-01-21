using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerExtension : Singleton<SceneManagerExtension>
{
    private BaseScene[] scenes;

    protected override void Awake()
    {
        base.Awake();
        RegisterScenes();
        //PoolManager.Instance.Init();
    }

    private void RegisterScenes()
    {
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
        UIManager.Instance.ResetUI();
        GatherInputManager.Instance.ResetStates();
    }
}
