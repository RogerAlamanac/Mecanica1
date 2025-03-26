using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;

    private float time;
    public float stepTime;
    public float totalTime = 10f;

    public float mass = 1f;
    private float gravityConstant = 9.8f;

    public struct Wall
    {
        public Vector3 normalVector;
        public float displacement;
        public float epsilonFactor;

        public Wall(Vector3 normal, float disp, float epsilon)
        {
            this.normalVector = normal;
            this.displacement = disp;
            this.epsilonFactor = epsilon; 
        }
    }

    public float epsilon = 1;
    private Wall[] cube;


    void Start()
    {
        cube = new Wall[]
        {
            new Wall(new Vector3(0,0,1), 0, epsilon),
            new Wall(new Vector3(0,1,0), 0, epsilon),
            new Wall(new Vector3(1,0,0), 0, epsilon),
            new Wall(new Vector3(-1,0,0), 5, epsilon),
            new Wall(new Vector3(0,-1,0), 5, epsilon),
            new Wall(new Vector3(0,0,-1), 5, epsilon),
        };

        position = new Vector3(1, 1, 1);
        velocity = new Vector3(5, 5, 5);
        acceleration = new Vector3(0, 0, -gravityConstant);

        time = 0;

        transform.position = position;
    }


    void Update()
    {
        if (time < totalTime)
        {
            Vector3 newPosition, newVelocity;
            (newPosition, newVelocity, time) = EulerMethod(position, velocity, time);
            (position, velocity) = checkCollision(cube, newPosition, position, newVelocity, velocity);

            transform.position = position;
        }
        else return;
    }

    (Vector3, Vector3, float) EulerMethod(Vector3 position, Vector3 velocity, float time)
    {
        Vector3 newPosition = position + velocity * stepTime;
        Vector3 newVelocity = velocity +acceleration * stepTime;

        time += stepTime;

        return(newPosition, newVelocity, time);
    }

    (Vector3, Vector3) checkWallCollision(Wall wall, Vector3 newPos, Vector3 oldPos, Vector3 newVel, Vector3 oldvel)
    {
        float oldDot = Vector3.Dot(oldPos, wall.normalVector) + wall.displacement;
        float newDot = Vector3.Dot(newPos, wall.normalVector) + wall.displacement;

        if(oldDot * newDot < 0)
        {
            //Update velocity
            float velocityDot = Vector3.Dot(wall.normalVector, newVel);
            Vector3 reflectionVelocity = newVel - (1 + wall.epsilonFactor) * velocityDot*wall.normalVector;

            //Update position
            float penetration = -newDot;
            Vector3 correctedPosition = newPos + (1 + wall.epsilonFactor) * penetration * wall.normalVector + 0.01f * wall.normalVector;

            return (correctedPosition, reflectionVelocity);
        }
        else
        {
            return (newPos, newVel);
        }
    }
    (Vector3, Vector3) checkCollision(Wall[] cube, Vector3 newPos, Vector3 oldPos, Vector3 newVel, Vector3 oldVel)
    {
        int i = 0;
        Vector3 finalPosition = newPos;
        Vector3 finalvelocity = newVel;

        while( i > cube.Length)
        {
            Vector3 tempPos, tempVel;
            (tempPos, tempVel) = checkWallCollision(cube[i], finalPosition, oldPos, finalvelocity, oldVel);

            if (tempPos != finalPosition) i = 0;
            else i++;

            finalPosition = tempPos;
            finalvelocity = tempVel;
        }
        return (finalPosition, finalvelocity);  
    }
}
