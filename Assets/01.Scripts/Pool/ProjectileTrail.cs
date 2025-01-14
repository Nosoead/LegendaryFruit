using System.Collections;
using UnityEngine;

public class ProjectileTrail : MonoBehaviour
{
    [SerializeField] private PooledProjectile projectile;
    [SerializeField] private TrailRenderer trailRenderer;
    private float trailSpeed;   
    private Vector3 target;

    private void OnEnable()
    {
        projectile.OnStartMovingEnvet += EnableTrail;
        projectile.OnEndMovingEnvet += DisableTrail;
    }

    private void OnDisable()
    {
        projectile.OnStartMovingEnvet -= EnableTrail;
        projectile.OnEndMovingEnvet -= DisableTrail;
    }

    private void EnableTrail(AttributeType type, Material material)
    {
        TrailSetting(type,material);
        //trailRenderer.Clear();
        trailRenderer.emitting = true;
    }

    private void DisableTrail()
    {
        trailRenderer.Clear();
        trailRenderer.emitting = false;
    }

    private void TrailSetting(AttributeType type, Material material)
    {
        switch(type)
        {
            case AttributeType.Normal:
                trailRenderer.material = material;
                trailRenderer.material.color = Color.white;
                break;
            case AttributeType.Burn:
                trailRenderer.material = material;
                trailRenderer.material.color = Color.red;
                break;
            case AttributeType.SlowDown:
                trailRenderer.material = material;
                trailRenderer.startWidth = 0.2f;
                trailRenderer.material.color = Color.blue;
                break;
            case AttributeType.Knockback:
                trailRenderer.material = material;
                trailRenderer.startWidth = 0.2f;
                trailRenderer.material.color = Color.yellow;
                break;
        }
    }
}
