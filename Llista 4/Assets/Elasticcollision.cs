using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elasticcollision : MonoBehaviour
{
    public float mass;
    public float position;
    public float velocity;
    public float stepTime;
    public float time;

    public Elasticcollision other;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(position, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (CheckCollision())
        {
            Collision(other);
        }

        transform.position = new Vector2(position, 0.0f);
        other.transform.position = new Vector2(other.position, 0.0f);
        time += stepTime;
    }

    private bool CheckCollision()
    {
        float x1 = position + velocity * stepTime;
        float x2 = other.position + other.velocity * stepTime;

        return Mathf.Abs(x1 - x2) < 0.2f;
    }
    private void Collision(Elasticcollision particle)
    {
        velocity = ((mass - particle.mass) * velocity + 2 * particle.mass * particle.velocity) / mass + particle.mass;
        particle.velocity = ((particle.mass - mass)*particle.velocity + 2 * mass * velocity) / (mass + particle.mass);
    }

    private void Move()
    {
        float newPosition = position + velocity * stepTime;
        position = newPosition;
    }
}
