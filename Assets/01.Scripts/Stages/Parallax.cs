using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    private void FixedUpdate()
    {
        SetBackground();
    }

    public void InitParallax()
    {
        cam = Camera.main.gameObject;
        SetPositionAndLength();
    }

    private void SetPositionAndLength()
    {
        startpos = transform.position.x;
        length = (GetComponent<SpriteRenderer>().bounds.size.x);
    }

    private void SetBackground()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + length / 2)
        {
            startpos += length;
        }
        else if (temp < startpos - length / 2)
        {
            startpos -= length;
        }
    }
}
