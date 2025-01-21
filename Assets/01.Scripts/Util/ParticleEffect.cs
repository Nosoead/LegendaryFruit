using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void SetDirection(float lookDirection)
    {
        var renderer = particle.GetComponent<ParticleSystemRenderer>();

        if (0 < lookDirection)
        {
            renderer.flip = new Vector2(0, 0);
        }
        else
        {
            renderer.flip = new Vector2(1, 0);
        }
    }

    public void PlayEffect()
    {
        particle.Play();
    }
}
