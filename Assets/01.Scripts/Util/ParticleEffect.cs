using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void UpdateDirection(bool isLeft)
    {
        var renderer = particle.GetComponent<ParticleSystemRenderer>();

        if (isLeft)
        {
            renderer.flip = new Vector2(1, 0);
        }
        else
        {
            renderer.flip = new Vector2(0, 0);
        }
    }

    public void PlayEffect()
    {
        particle.Play();
    }
}
