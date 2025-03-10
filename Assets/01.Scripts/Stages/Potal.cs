using UnityEngine;
using UnityEngine.Events;

public class Potal : MonoBehaviour, IInteractable
{
    //안내문구
    [SerializeField] private Canvas pressFCanvas;
    private int playerLayer;
    private int invincibleLayer;
    [SerializeField] private SpriteRenderer barricadeImage;

    //스테이지 셋팅
    [SerializeField] private Stage stage;
    private int weaponReward = 10000; //나눴을 때 몫, 1:무기, 2:재화
    private int currencyReward = 20000;
    //private int nextLevel = 1000; //나중에 다음 레벨있으면 써볼까 생각
    private int nextStage = 10;
    //private int stageVariation = 1; //바리에이션 늘어나면 randomRange써서 ㄱㄱ
    private StageType currentstageType;
    //하드코딩 ->  다음번에는 스테이지도 좀 더 캐릭터 다루듯이 분할하는 연습할 것
    public bool isCurrencyPotal;

    public void InitPotal()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        if (pressFCanvas == null)
        {
            pressFCanvas = GetComponentInChildren<Canvas>();
        }
        if (stage == null)
        {
            stage = GetComponentInParent<Stage>();
        }
        if (barricadeImage != null)
        {
            if (GameManager.Instance.GetGameClear())
            {
                barricadeImage.enabled = false;
            }
            else
            {
                barricadeImage.enabled = true;
            }
        }
        pressFCanvas.gameObject.SetActive(false);
        currentstageType = (StageType)stage.GetStageID();
        GameManager.Instance.OnGameClear += OnOpenPotal;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameClear -= OnOpenPotal;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameClear -= OnOpenPotal;
    }
    private void OnOpenPotal()
    {
        if (barricadeImage != null)
        {
            barricadeImage.enabled = false;
        }
    }

    #region /TogglePrompt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            if (GameManager.Instance.GetGameClear())
            {
                TogglePrompt(isOpen: true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            if (GameManager.Instance.GetGameClear())
            {
                TogglePrompt(isOpen: false);
            }
        }
    }

    private void TogglePrompt(bool isOpen)
    {
        if (isOpen)
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
        else
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
    }
    #endregion

    #region /Interact: stage
    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            ChagedStage();
        }
    }

    private void ChagedStage()
    {
        if (GameManager.Instance.GetGameClear() == true)
        {
            if (!stage.GetBossData())
            {
                currentstageType = GetNextStage(currentstageType);
                StageManager.Instance.ChangeStage(currentstageType);
                GameManager.Instance.Save();
            }
            else
            {
                GameManager.Instance.GameEnd(true);
            }
        }
    }

    private StageType GetNextStage(StageType stageType)
    {
        //TODO 어떤 보상이 나올지 랜덤으로 뽑아주기 5자리쨰 숫자 몫값으로
        //보스 이후 nextLevel확인할 수 있도록 ID 수정하기
        //맵다양성 높아지면 stageVariation도 랜덤으로 뽑아서 처리
        int stageNum = (int)stageType;
        if (isCurrencyPotal)
        {
            stageNum = stageNum % 10000 + currencyReward + nextStage;
            return (StageType)stageNum;
        }
        else
        {
            stageNum = stageNum % 10000 + weaponReward + nextStage;
            return (StageType)stageNum;
        }
    }
    #endregion
}
