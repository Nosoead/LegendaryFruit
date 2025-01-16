using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Pool;

public class PooledProjectile : MonoBehaviour, ISetPooledObject<PooledProjectile>
{
    protected IObjectPool<PooledProjectile> objectPool;
    private IDamageable attackObject;
    public IObjectPool<PooledProjectile> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public event UnityAction<AttributeType, Material> OnStartMovingEnvet;
    public event UnityAction OnEndMovingEnvet;

    [Header("ProjectTile_Data")]
    private ProjectileType currentProjectTileType;
    [SerializeField] private SpriteRenderer projectTileSprtie;
    private float projectTileDamage;
    private float projectTileSpeed;
    private float projectTileMaxDistance;
    private float projectTileRateTime;

    [Header("ProjectTile_AttributeTypeData")]
    private AttributeType currentAttributeType;
    private Material currentTrailMaterial;
    private float currentAttirbuteValue;
    private float currentAttributeRateTime;
    private float currentAttributeStack;

    private float lookDir;

    private void Awake()
    {
        if(projectTileSprtie == null)
        {
            projectTileSprtie = GetComponent<SpriteRenderer>();
        }
    }

    public void SetPooledObject(IObjectPool<PooledProjectile> pool)
    {
        ObjectPool = pool;
    }

    public void SetData(RangedAttackData data, float damage)
    {
        currentProjectTileType = data.projectileType;
        projectTileSprtie.sprite = data.projectTileSprite;
        projectTileDamage = damage;
        projectTileSpeed = data.rangedAttackSpeed;
        projectTileMaxDistance = data.maxDistance;
        projectTileRateTime = data.rangedAttackRate;
    }

    public void SetAttirbuteData(RangedAttackData data)
    {
        for(int i = 0; i < data.attributeDatas.Count; i++)
        {
            currentAttributeType = data.attributeDatas[i].attributeType;
            if (data.attributeDatas[i].attributeType == AttributeType.Normal) break; 
            currentAttirbuteValue = data.attributeDatas[i].attributeValue;
            currentAttributeStack = data.attributeDatas[i].attributeStack;
            currentAttributeRateTime = data.attributeDatas[i].attributeLateTime;
            currentTrailMaterial = data.attributeDatas[i].trailMaterial;
        }
    }

    public void ProjectTileShoot(Vector3 dir)
    {
        if(dir.x <= 1)
        {
            lookDir = dir.x;
            projectTileSprtie.flipX = true;
        }
        if(dir.x >= 1)
        {
            lookDir = dir.x;
            projectTileSprtie.flipX = false;
        }
        StartCoroutine(ProjectTileMove(dir));
    }

    private IEnumerator ProjectTileMove(Vector3 dir)
    { 
        Vector2 startPositon = this.transform.position;
        while (Vector2.Distance(startPositon, transform.position) < projectTileMaxDistance)
        {
            OnStartMovingEnvet?.Invoke(currentAttributeType, currentTrailMaterial);
            transform.position += dir * projectTileSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, 0.5f, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                break;
            }
            yield return null;
        }

        OnEndMovingEnvet?.Invoke();
        ResetState();
        ProjectTileRelease();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && currentProjectTileType == ProjectileType.Monster)
        {
            attackObject = collision.gameObject.GetComponent<IDamageable>();
            ApplyDamageByAttribute(attackObject, projectTileDamage, currentAttirbuteValue, currentAttributeRateTime, currentAttributeStack);
            ResetState();
            ProjectTileRelease();
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster") && currentProjectTileType == ProjectileType.Player)
        {
            attackObject = collision.gameObject.GetComponent<IDamageable>();
            ApplyDamageByAttribute(attackObject, projectTileDamage, currentAttirbuteValue, currentAttributeRateTime, currentAttributeStack);
            ResetState();
            ProjectTileRelease();
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ResetState();
            ProjectTileRelease();
        }
    }

    private void ApplyDamageByAttribute( IDamageable obj, float damage, float attirbuteValue, float attributeRateTime, float attributeStack)
    {
        switch (currentAttributeType)
        {
            case AttributeType.Normal:
                obj.TakeDamage(damage);
                break;
            case AttributeType.Burn:
                obj.BurnDamage(damage, attirbuteValue, attributeRateTime, attributeStack);
                break;
            case AttributeType.SlowDown:
                obj.SlowDown(damage, attirbuteValue, attributeRateTime);
                break;
            case AttributeType.Knockback:
                obj.Knockback(damage, attirbuteValue, attributeRateTime,lookDir);
                break;
        }
    }

    private void ProjectTileRelease()
    {
        StopAllCoroutines();
        objectPool.Release(this);
    }

    private void ResetState()
    {
        projectTileDamage = 0f;
        currentAttributeType = AttributeType.Normal;
        currentAttirbuteValue = 0f;
        currentAttributeRateTime = 0f;
        currentAttributeStack = 0f;
    }
}
