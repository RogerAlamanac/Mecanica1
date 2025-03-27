using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion2D : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;

    private float time;
    public float stepTime;
    public float totalTime = 10f;

    public float mass = 1f;
    public float gravityConstant = 9.8f;

    public struct Wall
    {
        public Vector2 normalVector;
        public float displacement;
        public float epsilonFactor;

        public Wall(Vector2 normal, float disp, float epsilon)
        {
            this.normalVector = normal.normalized;
            this.displacement = disp / normal.magnitude;
            this.epsilonFactor = epsilon;
        }
    }

    public float epsilon = 1;
    private Wall[] triangle;


    void Start()
    {
        triangle = new Wall[]
        {
            new Wall(new Vector2(0,1), 0, epsilon),
            new Wall(new Vector2(-5,-2.5f), 0, epsilon),
            new Wall(new Vector2(-5, -2.5f), 25, epsilon),
        };

        position = new Vector2(1, 1);
        velocity = new Vector2(3, 4);
        acceleration = new Vector2(0, -gravityConstant);

        time = 0;

        transform.position = position;
    }



    void Update()
    {
        if (time < totalTime)
        {
            Vector2 newPosition, newVelocity;
            (newPosition, newVelocity, time) = EulerMethod(position, velocity, time);
            (position, velocity) = CheckCollision(triangle, newPosition, position, newVelocity, velocity);

            transform.position = position;
        }
        else return;
    }

    (Vector2, Vector2, float) EulerMethod(Vector2 position, Vector2 velocity, float time)
    {
        Vector2 newPosition = position + velocity * stepTime;
        Vector2 newVelocity = velocity + acceleration * stepTime;

        time += stepTime;

        return (newPosition, newVelocity, time);
    }

    (Vector2, Vector2) CheckWallCollision(Wall wall, Vector2 newPos, Vector2 oldPos, Vector2 newVel, Vector2 oldVel)
    {
        float oldDot = Vector2.Dot(oldPos, wall.normalVector) + wall.displacement;
        float newDOt = Vector2.Dot(newPos, wall.normalVector) + wall.displacement;

        if (oldDot * newDOt < 0)
        {
            //Update velocity
            float velocityDot = Vector2.Dot(wall.normalVector, newVel);
            Vector2 reflectionVelocity = newVel - (1 + wall.epsilonFactor) * velocityDot * wall.normalVector;

            //Update position
            float penetration = -newDOt;
            Vector2 correctedPosition = newPos + (1 + wall.epsilonFactor) * penetration * wall.normalVector + 0.01f * wall.normalVector;

            return (correctedPosition, reflectionVelocity);
        }
        else
        {
            return (newPos, newVel);
        }
    }

    (Vector2, Vector2) CheckCollision(Wall[] triangle, Vector2 newPos, Vector2 oldPos, Vector2 newVel, Vector2 oldVel)
    {
        int i = 0;
        Vector2 finalPos = newPos;
        Vector2 finalVel = newVel;

        while (i < triangle.Length)
        {
            Vector2 tempPos, tempVel;
            (tempPos, tempVel) = CheckWallCollision(triangle[i], finalPos, oldPos, finalVel, oldVel);

            if (tempPos != finalPos)  i = 0; 
            else i++; 

            finalPos = tempPos;
            finalVel = tempVel;
        }


        return (finalPos, finalVel);
    }
}
