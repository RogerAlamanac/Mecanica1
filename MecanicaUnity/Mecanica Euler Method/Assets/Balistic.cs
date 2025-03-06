using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balistic : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private float time;

    public float totalTime = 10f;
    public float stepTime = 0.01f;
    public float angle1 = 30.0f;

    private float gravity = 9.8f;
    public float speed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(0, 0);
        velocity = new Vector2(speed*Mathf.Cos(angle1*Mathf.PI/180), speed*Mathf.Sin(angle1*Mathf.PI/180));
        acceleration = new Vector2(0, -gravity);
        time = 0;
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if(time < totalTime) {
            (position, velocity, time) = EulerMethod(position, velocity, time);

            if(position.y < 0)
            {
                return;
            }
            else {
                transform.position = position;
            }
        }
        else
        {
            return;
        }

    }

    (Vector2, Vector2, float) EulerMethod(Vector2 position, Vector2 velocity, float time)
    {
        Vector2 newPosition = position + velocity * stepTime;
        Vector2 newVelocity = velocity + acceleration * stepTime;
        float newTime = time + stepTime;

        return (newPosition, newVelocity, newTime);
    }
}
