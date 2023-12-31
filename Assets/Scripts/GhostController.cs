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
    List<Position> nodes = new List<Position>();
    List<Position> chokes = new List<Position>();
    int currentNode;
    bool outsideSpawn = false;
    float duration = 0.35f;
    float enhancedDuration = 0.2f;
    Position spawnPos;
    public bool enchancedMode;

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
        {0,0,0,0,0,2,0,4,4,0,3,4,4,-1,-1,4,4,3,0,4,4,0,2,0,0,0,0,0},
        {2,2,2,2,2,1,0,3,3,0,4,0,0,0,0,0,0,4,0,3,3,0,1,2,2,2,2,2},
        {4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4},
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
        spawnPos = new Position(ghostY, ghostX);

        // for ghost 4
        currentNode = 0;
        nodes.Add(new Position(14, 6));
        nodes.Add(new Position(20, 6));
        nodes.Add(new Position(20, 1));
        nodes.Add(new Position(27, 1));
        nodes.Add(new Position(27, 12));
        nodes.Add(new Position(23, 12));
        nodes.Add(new Position(23, 15));
        nodes.Add(new Position(27, 15));
        nodes.Add(new Position(27, 26));
        nodes.Add(new Position(20, 26));
        nodes.Add(new Position(20, 21));
        nodes.Add(new Position(14, 21));
        nodes.Add(new Position(8, 21));
        nodes.Add(new Position(8, 26));
        nodes.Add(new Position(1, 26));
        nodes.Add(new Position(1, 15));
        nodes.Add(new Position(5, 15));
        nodes.Add(new Position(5, 12));
        nodes.Add(new Position(1, 12));
        nodes.Add(new Position(1, 1));
        nodes.Add(new Position(8, 1));
        nodes.Add(new Position(8, 6));

        chokes.Add(new Position(5,6));
        chokes.Add(new Position(5, 21));
        chokes.Add(new Position(23, 6));
        chokes.Add(new Position(23, 21));

    }

    public void Move()
    {
        if (outsideSpawn && !(state==State.Dead))
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


            if (!enchancedMode)
            {
                if (ghostNum == 1 || state == State.Scared || state == State.Recovering)
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
                else if (ghostNum == 2)
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
                else if (ghostNum == 3)
                {
                    int ran = Random.Range(0, validPositions.Count);
                    Position newPos = validPositions[ran];
                    StartCoroutine("LerpToPosition", newPos);
                }
                else if (ghostNum == 4)
                {
                    int index = 0;
                    Position newPos = validPositions[0];

                    if (Vector3.Distance(nodes[currentNode].GetTransform(), gameObject.transform.position) == 0)
                    {
                        if (currentNode < nodes.Count - 1)
                        {
                            currentNode++;
                        }
                        else
                        {
                            currentNode = 0;
                        }
                    }

                    for (int i = 1; i < validPositions.Count; i++)
                    {
                        if (Vector3.Distance(nodes[currentNode].GetTransform(), validPositions[i].GetTransform()) <= Vector3.Distance(nodes[currentNode].GetTransform(), newPos.GetTransform()))
                        {
                            index = i;
                            newPos = validPositions[index];
                        }
                    }

                    StartCoroutine("LerpToPosition", newPos);
                } 
            }
            else if (enchancedMode)
            {
                GameObject player = FindAnyObjectByType<PacStudentController>().gameObject;
                KeyCode playerDir = player.GetComponent<PacStudentController>().getCurrentInput();
                if (state == State.Scared || state == State.Recovering) // scatter
                {
                    int index = 0;
                    Position newPos = validPositions[0];
                    int quadrant = 0;
                    if (ghostNum == 1)
                    {
                        quadrant = 1;
                    } else if (ghostNum == 2)
                    {
                        quadrant = 2;
                    } else if (ghostNum == 3)
                    {
                        quadrant = 3;
                    } else if (ghostNum == 4)
                    {
                        quadrant = 4;
                    }

                    for (int i = 1; i < validPositions.Count; i++)
                    {
                        if (Vector3.Distance(chokes[quadrant - 1].GetTransform(), validPositions[i].GetTransform()) <= Vector3.Distance(chokes[quadrant - 1].GetTransform(), newPos.GetTransform()))
                        {
                            index = i;
                            newPos = validPositions[index];
                        }
                    }
                    StartCoroutine("LerpToPosition", newPos);
                }
                else if (ghostNum == 1) // attacker, straight for the player
                {
                    int index = 0;
                    Position newPos = validPositions[0];

                    for (int i = 1; i < validPositions.Count; i++)
                    {
                        if (Vector3.Distance(player.transform.position, validPositions[i].GetTransform()) <= Vector3.Distance(player.transform.position, newPos.GetTransform()))
                        {
                            index = i;
                            newPos = validPositions[index];
                        }
                    }

                    StartCoroutine("LerpToPosition", newPos);
                }
                else if (ghostNum == 2) // "one (and a half) steps ahead"
                {
                    Vector3 infrontPlayer = new Vector3();
                    if (playerDir == KeyCode.W)
                    {
                        infrontPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 45, player.transform.position.z);
                    }
                    else if (playerDir == KeyCode.A)
                    {
                        infrontPlayer = new Vector3(player.transform.position.x - 45, player.transform.position.y, player.transform.position.z);
                    }
                    else if (playerDir == KeyCode.S)
                    {
                        infrontPlayer = new Vector3(player.transform.position.x, player.transform.position.y - 45, player.transform.position.z);
                    }
                    else if (playerDir == KeyCode.D)
                    {
                        infrontPlayer = new Vector3(player.transform.position.x + 45, player.transform.position.y, player.transform.position.z);
                    }

                    int index = 0;
                    Position newPos = validPositions[0];

                    for (int i = 1; i < validPositions.Count; i++)
                    {
                        if (Vector3.Distance(infrontPlayer, validPositions[i].GetTransform()) <= Vector3.Distance(infrontPlayer, newPos.GetTransform()))
                        {
                            index = i;
                            newPos = validPositions[index];
                        }
                    }

                    StartCoroutine("LerpToPosition", newPos);
                }
                else if (ghostNum == 3) // assist
                {
                    int quadrant = 0;
                    if (player.transform.position.x >= 0 && player.transform.position.y >= 0)
                    {
                        quadrant = 1;
                    }
                    else if(player.transform.position.x <= 0 && player.transform.position.y >= 0)
                    {
                        quadrant = 2;
                    }
                    else if (player.transform.position.x <= 0 && player.transform.position.y <= 0)
                    {
                        quadrant = 3;
                    }
                    else if (player.transform.position.x >= 0 && player.transform.position.y <= 0)
                    {
                        quadrant = 4;
                    }

                    int index = 0;
                    Position newPos = validPositions[0];

                    if (Vector3.Distance(player.transform.position, transform.position) <= 4 * 30) // if close enough attack
                    {
                        for (int i = 1; i < validPositions.Count; i++)
                        {
                            if (Vector3.Distance(player.transform.position, validPositions[i].GetTransform()) <= Vector3.Distance(player.transform.position, newPos.GetTransform()))
                            {
                                index = i;
                                newPos = validPositions[index];
                            }
                        }
                    } else
                    {
                        for (int i = 1; i < validPositions.Count; i++)
                        {
                            if (Vector3.Distance(chokes[quadrant - 1].GetTransform(), validPositions[i].GetTransform()) <= Vector3.Distance(chokes[quadrant - 1].GetTransform(), newPos.GetTransform()))
                            {
                                index = i;
                                newPos = validPositions[index];
                            }
                        }
                    }

                    StartCoroutine("LerpToPosition", newPos);

                }
                else if (ghostNum == 4) // patrol
                {
                    int index = 0;
                    Position newPos = validPositions[0];


                    if (Vector3.Distance(player.transform.position, transform.position) >= 8 * 30) // move toward player
                    {
                        for (int i = 1; i < validPositions.Count; i++)
                        {
                            if (Vector3.Distance(player.transform.position, validPositions[i].GetTransform()) <= Vector3.Distance(player.transform.position, newPos.GetTransform()))
                            {
                                index = i;
                                newPos = validPositions[index];
                            }
                        }
                    }
                    else if (Vector3.Distance(player.transform.position, transform.position) <= 4 * 30) // attack player
                    {
                        for (int i = 1; i < validPositions.Count; i++)
                        {
                            if (Vector3.Distance(player.transform.position, validPositions[i].GetTransform()) >= Vector3.Distance(player.transform.position, newPos.GetTransform()))
                            {
                                index = i;
                                newPos = validPositions[index];
                            }
                        }
                    }
                    else if (Vector3.Distance(player.transform.position, transform.position) <= 6 * 30) // stay a distance away from the ghost
                    {
                        for (int i = 1; i < validPositions.Count; i++)
                        {
                            if (Vector3.Distance(player.transform.position, validPositions[i].GetTransform()) >= Vector3.Distance(player.transform.position, newPos.GetTransform()))
                            {
                                index = i;
                                newPos = validPositions[index];
                            }
                        }
                    }

                    StartCoroutine("LerpToPosition", newPos);
                }
            }


        }
        else if (!outsideSpawn)
        {
            state = FindAnyObjectByType<GhostManager>().globalState;
            List<Position> validPositions = new List<Position>();
            if ((levelMap[ghostY - 1, ghostX] == 0 || levelMap[ghostY - 1, ghostX] == -1) && !lastPos.equals(ghostY - 1, ghostX))
            {
                validPositions.Add(new Position(ghostY - 1, ghostX));
            }

            if ((levelMap[ghostY, ghostX + 1] == 0 || levelMap[ghostY, ghostX + 1] == -1) && !lastPos.equals(ghostY, ghostX + 1))
            {
                validPositions.Add(new Position(ghostY, ghostX + 1));
            }

            if ((levelMap[ghostY + 1, ghostX] == 0 || levelMap[ghostY + 1, ghostX] == -1) && !lastPos.equals(ghostY + 1, ghostX))
            {
                validPositions.Add(new Position(ghostY + 1, ghostX));
            }

            if ((levelMap[ghostY, ghostX - 1] == 0 || levelMap[ghostY, ghostX - 1] == -1) && !lastPos.equals(ghostY, ghostX - 1))
            {
                validPositions.Add(new Position(ghostY, ghostX - 1));
            }

            int index = 0;
            Position newPos = validPositions[0];

            if (Vector3.Distance(new Position(11, 13).GetTransform(), gameObject.transform.position) == 0)
            {
                outsideSpawn = true;
                Move();
                return;
            }

            for (int i = 1; i < validPositions.Count; i++)
            {
                if (Vector3.Distance(new Position(11,13).GetTransform(), validPositions[i].GetTransform()) <= Vector3.Distance(new Position(11, 13).GetTransform(), newPos.GetTransform()))
                {
                    index = i;
                    newPos = validPositions[index];
                }
            }

            StartCoroutine("LerpToPosition", newPos);
        }
        else if (state == State.Dead)
        {
            duration = 5;
            outsideSpawn = false;
            StartCoroutine("LerpToPosition", spawnPos);
        }
    }

    IEnumerator LerpToPosition(Position pos)
    {
        float startTime = Time.time;
        Vector3 startPos = gameObject.transform.position;
        Vector3 endPos = pos.GetTransform();
        

        if (enchancedMode && !(state == State.Dead))
        {
            duration = enhancedDuration;
        }

        while (Vector2.Distance(gameObject.transform.position, endPos) > 0)
        {
            float timeFrac = (Time.time - startTime) / duration;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, timeFrac);
            yield return new WaitForSeconds(0.01f);
        }
        lastPos = new Position(ghostY,ghostX);
        ghostX = pos.x;
        ghostY = pos.y;
        duration = 0.35f;
        Move();

        
    }

    public void KillGhost()
    {
        state = State.Dead;
        anim.SetBool("scared", false);
        anim.SetBool("recovering", false);
        anim.SetBool("dead", true);
        GameObject.FindAnyObjectByType<ScoreManager>().increaseScore(300);
        if (!FindAnyObjectByType<AudioController>().IsGhostMusicOn())
        {
            GameObject.FindAnyObjectByType<AudioController>().GhostDeadMusic();
        }
            

        StartCoroutine("DeadTime");
    }

    public IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(5);
        state = State.Walking;
        anim.SetBool("dead", false);
        if (FindAnyObjectByType<GhostManager>().AllGhostsAlive())
        {
            GameObject.FindAnyObjectByType<AudioController>().GhostAliveMusic();
        }  
    }    

}
