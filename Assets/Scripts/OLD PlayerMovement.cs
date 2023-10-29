using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    [SerializeField]
    float timeBetweenPoints;
    float startTime;
    int quadrant; // as in mathematically defined quadrants
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        quadrant = 2;
        anim.SetInteger("Direction", 1);
        startPos = gameObject.transform.position;
        endPos = new Vector3(startPos.x + 150, startPos.y, startPos.z);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, endPos) > 1)
        {
            // tween
            float t = (Time.time - startTime) / timeBetweenPoints;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, t * t * t);
        }  
        else
        {
            gameObject.transform.position = endPos;
            startPos = endPos;
            startTime = Time.time;
            if (quadrant == 2)
            {
                // ended in top right
                quadrant = 1;
                endPos = new Vector3(startPos.x, startPos.y - 120, startPos.z);
                anim.SetInteger("Direction", 2);
            } else if (quadrant == 1)
            {
                // ended in bottom right
                quadrant = 4;
                endPos = new Vector3(startPos.x - 150, startPos.y, startPos.z);
                anim.SetInteger("Direction", 0);
            } else if (quadrant == 4)
            {
                quadrant = 3;
                endPos = new Vector3(startPos.x, startPos.y + 120, startPos.z);
                anim.SetInteger("Direction", 3);
            } else if (quadrant == 3)
            {
                quadrant = 2;
                endPos = new Vector3(startPos.x + 150, startPos.y, startPos.z);
                anim.SetInteger("Direction", 1);
            }
            
        }
    }
}
