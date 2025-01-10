using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class PooledProjectTile : MonoBehaviour, ISetPooledObject<PooledProjectTile>
{
    protected IObjectPool<PooledProjectTile> objectPool;
    private IDamageable attackObject;
    public IObjectPool<PooledProjectTile> ObjectPool
    { get => objectPool; set => objectPool = value; }


    [Header("ProjectTile_Data")]
    private ProjectileType currentProjectTileType;
    [SerializeField] private SpriteRenderer projectTileSprtie;
    private float projectTileDamage;
    private float projectTileSpeed;
    private float projectTileMaxDistance;

    [Header("ProjectTile_AttributeTypeData")]
    private AttributeType currentAttributeType;
    private float currentAttirbuteValue;
    private float currentAttributeRateTime;
    private float currentAttributeStack;

    private void Awake()
    {
        if(projectTileSprtie == null)
        {
            projectTileSprtie = GetComponent<SpriteRenderer>();
        }
    }

    public void SetPooledObject(IObjectPool<PooledProjectTile> pool)
    {
        ObjectPool = pool;
    }

    public void SetData(RangedAttackData data)
    {
        currentProjectTileType = data.projectileType;
        projectTileSprtie.sprite = data.projectTileSprite;
        projectTileDamage = data.rangedAttackPower;
        projectTileSpeed = data.rangedAttackSpeed;
        projectTileMaxDistance = data.maxDistance;    
    }

    public void SetAttirbuteData(RangedAttackData data)
    {
        currentAttributeType = data.attributeType;
        if (currentAttributeType == AttributeType.Normal) return;
        //currentAttirbuteValue = data.;
        //currentAttributeRateTime = attributeRateTime;
        //currentAttributeStack = attributeStack;
    }

    public void ProjectTileShoot(Vector3 dir)
    {
        StartCoroutine(ProjectTileMove(dir));
    }

    private IEnumerator ProjectTileMove(Vector3 dir)
    {
        Vector2 startPositon = transform.position;
        while (Vector2.Distance(startPositon, transform.position) > projectTileMaxDistance)
        {
            transform.position += dir * projectTileSpeed * Time.deltaTime;
            yield return null;
        }

        ResetState();
        ProjectTileRelease();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && currentProjectTileType == ProjectileType.Player)
        {
            attackObject = collision.gameObject.GetComponent<IDamageable>();
            ApplyDamageByAttribute(attackObject, projectTileDamage, currentAttirbuteValue, currentAttributeRateTime, currentAttributeStack);
            ResetState();
            ProjectTileRelease();
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster") && currentProjectTileType == ProjectileType.Monster)
        {
            attackObject = collision.gameObject.GetComponent<IDamageable>();
            ApplyDamageByAttribute(attackObject, projectTileDamage, currentAttirbuteValue, currentAttributeRateTime, currentAttributeStack);
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
