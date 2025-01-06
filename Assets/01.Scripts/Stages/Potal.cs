using UnityEngine;

public class Potal : MonoBehaviour, IInteractable
{
    //안내문구
    [SerializeField] private Canvas pressFCanvas;
    private int playerLayer;
    private int invincibleLayer;

    //스테이지 셋팅
    [SerializeField] private Stage stage;
    private int rewardKind = 10000; //나눴을 때 몫, 1:무기, 2:재화
    private int nextLevel = 1000; //나중에 다음 레벨있으면 써볼까 생각
    private int nextStage = 10;
    private int stageVariation = 1; //바리에이션 늘어나면 randomRange써서 ㄱㄱ
    private StageType currentstageType;

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

        pressFCanvas.gameObject.SetActive(false);
        currentstageType = (StageType)stage.GetStageID();
    }

    #region /TogglePrompt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: false);
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
            Debug.Log("0. " + (int)currentstageType);
            ChagedStage();
        }
    }

    private void ChagedStage()
    {
        Debug.Log("1. "+ (int)currentstageType);
        if (GameManager.Instance.isClear == true)
        {
            Debug.Log("2. " + (int)currentstageType);
            currentstageType = GetNextStage(currentstageType);
            StageManager.Instance.ChangeStage(currentstageType);
            GameManager.Instance.Save();
        }
    }

    private StageType GetNextStage(StageType stageType)
    {
        Debug.Log(stageType + "ddd" + stageType.ToString());
        //TODO 어떤 보상이 나올지 랜덤으로 뽑아주기 5자리쨰 숫자 몫값으로
        //보스 이후 nextLevel확인할 수 있도록 ID 수정하기
        //맵다양성 높아지면 stageVariation도 랜덤으로 뽑아서 처리
        StageType nextStageType = (StageType)(stageType + nextStage);
        Debug.Log($"last : {stageType}, next : {nextStageType}");
        return nextStageType;
    }
    #endregion
}
