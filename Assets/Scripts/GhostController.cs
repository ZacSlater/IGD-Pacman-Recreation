using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public State state;
    Animator anim;
    public int ghostNum;
    int ghostX;
    int ghostY;
    Position lastPos;

    public class Position
    {
        public int x;
        public int y;

        public Position(int y, int x)
        {
            this.x = x;
            this.y = y;
        }

        public bool equals(int y, int x)
        {
            return this.x == x && this.y == y;
        }

        public Vector3 GetTransform()
        {
            return new Vector3(-390 + x * 30, 420 - y * 30, 0); 
        }

        override public string ToString()
        {
            return "X pos: " + x + " Y pos: " + y + " At Location: " + GetTransform();
        }

    }

    int[,] levelMap =
{
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,2},
        {2,0,3,4,4,3,0,3,4,4,4,3,0,4,4,0,3,4,4,4,3,0,3,4,4,3,0,2},
        {2,0,4,0,0,4,0,4,0,0,0,4,0,4,4,0,4,0,0,0,4,0,4,0,0,4,0,2},
        {2,0,3,4,4,3,0,3,4,4,4,3,0,3,3,0,3,4,4,4,3,0,3,4,4,3,0,2},
        {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
        {2,0,3,4,4,3,0,3,3,0,3,4,4,4,4,4,4,3,0,3,3,0,3,4,4,3,0,2},
        {2,0,3,4,4,3,0,4,4,0,3,4,4,3,3,4,4,3,0,4,4,0,3,4,4,3,0,2},
        {2,0,0,0,0,0,0,4,4,0,0,0,0,4,4,0,0,0,0,4,4,0,0,0,0,0,0,2},
        {1,2,2,2,2,1,0,4,3,4,4,3,0,4,4,0,3,4,4,3,4,0,1,2,2,2,2,1},
        {0,0,0,0,0,2,0,4,3,4,4,3,0,3,3,0,3,4,4,3,4,0,2,0,0,0,0,0},
        {0,0,0,0,0,2,0,4,4,0,0,0,0,0,0,0,0,0,0,4,4,0,2,0,0,0,0,0},
        {0,0,0,0,0,2,0,4,4,0,3,4,4,0,0,4,4,3,0,4,4,0,2,0,0,0,0,0},
        {2,2,2,2,2,1,0,3,3,0,4,0,0,0,0,0,0,4,0,3,3,0,1,2,2,2,2,2},
        {1,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,1},
        {2,2,2,2,2,1,0,3,3,0,4,0,0,0,0,0,0,4,0,3,3,0,1,2,2,2,2,2},
        {0,0,0,0,0,2,0,4,4,0,3,4,4,4,4,4,4,3,0,4,4,0,2,0,0,0,0,0},
        {0,0,0,0,0,2,0,4,4,0,0,0,0,0,0,0,0,0,0,4,4,0,2,0,0,0,0,0},
        {0,0,0,0,0,2,0,4,3,4,4,3,0,3,3,0,3,4,4,3,4,0,2,0,0,0,0,0},
        {1,2,2,2,2,1,0,4,3,4,4,3,0,4,4,0,3,4,4,3,4,0,1,2,2,2,2,1},
        {2,0,0,0,0,0,0,4,4,0,0,0,0,4,4,0,0,0,0,4,4,0,0,0,0,0,0,2},
        {2,0,3,4,4,3,0,4,4,0,3,4,4,3,3,4,4,3,0,4,4,0,3,4,4,3,0,2},
        {2,0,3,4,4,3,0,3,3,0,3,4,4,4,4,4,4,3,0,3,3,0,3,4,4,3,0,2},
        {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
        {2,0,3,4,4,3,0,3,4,4,4,3,0,3,3,0,3,4,4,4,3,0,3,4,4,3,0,2},
        {2,0,4,0,0,4,0,4,0,0,0,4,0,4,4,0,4,0,0,0,4,0,4,0,0,4,0,2},
        {2,0,3,4,4,3,0,3,4,4,4,3,0,4,4,0,3,4,4,4,3,0,3,4,4,3,0,2},
        {2,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,2},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1}
    };

    public enum State
    {
        Walking,
        Scared,
        Recovering,
        Dead
    }

    void Start()
    {
        state = State.Walking;
        anim = GetComponent<Animator>();
        

        if (ghostNum == 1)
        {
            ghostX = 11;
            ghostY = 13;
        } else if (ghostNum == 2)
        {
            ghostX = 11;
            ghostY = 15;
        } else if (ghostNum == 3)
        {
            ghostX = 16;
            ghostY = 13;
        } else if (ghostNum == 4)
        {
            ghostX = 16;
            ghostY = 15;
        }
        lastPos = new Position(ghostY, ghostX);

    }

    public void Move()
    {
        if (state == State.Walking)
        {
            // get a list of valid options
            List<Position> validPositions = new List<Position>();
            if (levelMap[ghostY - 1, ghostX] == 0 && !lastPos.equals(ghostY - 1, ghostX))
            {
                validPositions.Add(new Position(ghostY - 1, ghostX));
            }

            if (levelMap[ghostY, ghostX + 1] == 0 && !lastPos.equals(ghostY, ghostX + 1))
            {
                validPositions.Add(new Position(ghostY, ghostX + 1));
            }

            if (levelMap[ghostY + 1, ghostX] == 0 && !lastPos.equals(ghostY + 1, ghostX))
            {
                validPositions.Add(new Position(ghostY + 1, ghostX));
            }

            if (levelMap[ghostY, ghostX - 1] == 0 && !lastPos.equals(ghostY, ghostX - 1))
            {
                validPositions.Add(new Position(ghostY, ghostX - 1));
            }

            if (validPositions.Count == 0)
            {
                if (levelMap[ghostY - 1, ghostX] == 0)
                {
                    validPositions.Add(new Position(ghostY - 1, ghostX));
                }

                if (levelMap[ghostY, ghostX + 1] == 0)
                {
                    validPositions.Add(new Position(ghostY, ghostX + 1));
                }

                if (levelMap[ghostY + 1, ghostX] == 0)
                {
                    validPositions.Add(new Position(ghostY + 1, ghostX));
                }

                if (levelMap[ghostY, ghostX - 1] == 0)
                {
                    validPositions.Add(new Position(ghostY, ghostX - 1));
                }
            }

            // pick one of the valid options

            if (ghostNum == 1)
            {
                int index = 0;
                Position newPos = validPositions[0];

                for (int i = 1; i < validPositions.Count; i++)
                {
                    if (Vector3.Distance(FindAnyObjectByType<PacStudentController>().transform.position, validPositions[i].GetTransform()) >= Vector3.Distance(FindAnyObjectByType<PacStudentController>().transform.position, newPos.GetTransform()))
                    {
                        index = i;
                        newPos = validPositions[index];
                    }
                }

                StartCoroutine("LerpToPosition", newPos);
            }

            if (ghostNum == 2)
            {
                int index = 0;
                Position newPos = validPositions[0];

                for (int i = 1; i < validPositions.Count; i++)
                {
                    if (Vector3.Distance(FindAnyObjectByType<PacStudentController>().transform.position, validPositions[i].GetTransform()) <= Vector3.Distance(FindAnyObjectByType<PacStudentController>().transform.position, newPos.GetTransform()))
                    {
                        index = i;
                        newPos = validPositions[index];
                    }
                }

                StartCoroutine("LerpToPosition", newPos);
            }

            if (ghostNum == 3)
            {
                int ran = Random.Range(0, validPositions.Count);
                Position newPos = validPositions[ran];
                StartCoroutine("LerpToPosition", newPos);
            }

        }
    }

    IEnumerator LerpToPosition(Position pos)
    {
        float startTime = Time.time;
        float duration = 0.35f;
        Vector3 startPos = gameObject.transform.position;
        Vector3 endPos = pos.GetTransform();
        Debug.Log(lastPos + " || " + pos);

        while (Vector2.Distance(gameObject.transform.position, endPos) > 0)
        {
            float timeFrac = (Time.time - startTime) / duration;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, timeFrac);
            yield return new WaitForSeconds(0.01f);
        }
        lastPos = new Position(ghostY,ghostX);
        ghostX = pos.x;
        ghostY = pos.y;
        
        if (state == State.Walking)
        {
            Move();
        }
        
    }

    public void KillGhost()
    {
        state = State.Dead;
        anim.SetBool("scared", false);
        anim.SetBool("recovering", false);
        anim.SetBool("dead", true);
        GameObject.FindAnyObjectByType<ScoreManager>().increaseScore(300);
        GameObject.FindAnyObjectByType<AudioController>().GhostDeadMusic();
        StartCoroutine("DeadTime");
    }

    public IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindAnyObjectByType<AudioController>().GhostAliveMusic();
        anim.SetBool("dead", false);
        state = State.Walking;
    }    

}
