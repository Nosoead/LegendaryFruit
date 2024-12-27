using System.Collections;
using System.Collections.Generic;
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

    //포탈 자동배치할 경우
    //private void Awake()
    //{
    //    playerLayer = LayerMask.NameToLayer("Player");
    //    if (pressFCanvas == null)
    //    {
    //        pressFCanvas = GetComponentInChildren<Canvas>();
    //    }
    //    if (stage == null)
    //    {
    //        stage = GetComponentInParent<Stage>();
    //    }
    //}
    //private void Start()
    //{
    //    pressFCanvas.gameObject.SetActive(false);
    //    stageType = (StageType)stage.stageData.StageID;
    //}

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
        currentstageType = (StageType)stage.stageData.stageID;
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
            ChagedStage();
        }
    }

    private void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
        {
            currentstageType = GetNextStage(currentstageType);
            StageManager.Instance.ChangeStage(currentstageType);
        }
    }

    private StageType GetNextStage(StageType stageType)
    {
        //TODO 어떤 보상이 나올지 랜덤으로 뽑아주기 5자리쨰 숫자 몫값으로
        //보스 이후 nextLevel확인할 수 있도록 ID 수정하기
        //맵다양성 높아지면 stageVariation도 랜덤으로 뽑아서 처리
        StageType nextStageType = (StageType)(stageType + nextStage);
        Debug.Log($"last : {stageType}, next : {nextStageType}");
        return nextStageType;
    }
    #endregion
}
