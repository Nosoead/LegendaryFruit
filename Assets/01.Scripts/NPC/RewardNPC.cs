using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RewardNPC : NPC
{
    //스폰포인트
    [SerializeField] protected Transform spawnPositionsRoot;
    protected List<Transform> spawnPositions = new List<Transform>();
    protected int randomNum;

    //F키 누르세요
    [SerializeField]
    protected Canvas pressFCanvas;
    protected int playerLayer;
    protected int invincibleLayer;

    protected IObjectPool<PooledReward> reward;
    protected PooledReward pooledReward;

    public override void InitNPC()
    {
        gameObject.layer = LayerMask.NameToLayer("NPC");
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        if (pressFCanvas == null)
        {
            pressFCanvas = GetComponentInChildren<Canvas>();
        }
        pressFCanvas.gameObject.SetActive(false);
        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
        reward = PoolManager.Instance.GetObjectFromPool<PooledReward>(PoolType.PooledReward);
    }

    #region /TogglePrompt
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer) && StageManager.Instance.GetStageClear())
        {
            TogglePrompt(isOpen: true);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer) && StageManager.Instance.GetStageClear())
        {
            TogglePrompt(isOpen: false);
        }
    }

    protected void TogglePrompt(bool isOpen)
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

    public override void ReleaseReward()
    {
        base.ReleaseReward();
        if (pooledReward != null && pooledReward.gameObject.activeSelf)
        {
            pooledReward.PoolRelease();
        }
    }
}
