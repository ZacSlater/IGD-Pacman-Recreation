using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    State state;
    private enum State
    {
        Alive,
        Dead
    }
    private KeyCode lastInput;
    private KeyCode currentInput;
    private int playerX, playerY;
    private bool isMoving;
    public Animator anim;
    int speed;
    Vector2 startPos;
    public AudioSource step;
    public AudioSource eat;
    public AudioSource bump;
    public ParticleSystem wallBump;
    public ParticleSystem trail;
    public ParticleSystem deathEffect;
    bool newBump;
    int teleport = 0;
    ScoreManager scoreManager;
    GhostManager ghostManager;
    int lives = 3;
    public GameObject[] livesUI;
    /*int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };*/

    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0,0,0,0,4,0,0,0,5,0,0,0,0,0,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,4,4,4,4,3,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1}
    };

    private void Start()
    {
        playerX = 1;
        playerY = 1;
        isMoving = false;
        state = State.Alive;
        scoreManager = GameObject.FindAnyObjectByType<ScoreManager>();
        ghostManager = GameObject.FindAnyObjectByType<GhostManager>();
    }

    private void Update()
    {
        if (GameObject.FindAnyObjectByType<GameManager>().state == GameManager.State.Ingame)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                lastInput = KeyCode.W;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                lastInput = KeyCode.A;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                lastInput = KeyCode.S;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                lastInput = KeyCode.D;
            }

            if (!isMoving && state == State.Alive)
            {

                if (IsWalkable(lastInput))
                {
                    isMoving = true;
                    currentInput = lastInput;
                    StartCoroutine("LerpToLocation");
                }
                else
                {
                    if (IsWalkable(currentInput))
                    {
                        isMoving = true;
                        StartCoroutine("LerpToLocation");
                    }
                }

            }

            if (speed == 0 && lastInput != KeyCode.None)
            {
                anim.SetInteger("moveX", 0);
                anim.SetInteger("moveY", 0);

                if (newBump)
                {
                    newBump = false;
                    bump.Play();
                    wallBump.Play();
                }

            }
            else
            {
                newBump = true;
            }
        }

    }

    IEnumerator LerpToLocation()
    {
        float startTime = Time.time;
        float duration = 0.05f;
        startPos = gameObject.transform.position;
        Vector2 endPos = new Vector2();

        if (currentInput == KeyCode.W)
        {
            endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 30);
            playerY--;
            anim.SetInteger("moveX", 0);
            anim.SetInteger("moveY", 1);
        } else if (currentInput == KeyCode.A)
        {
            endPos = new Vector2(gameObject.transform.position.x - 30, gameObject.transform.position.y);
            playerX--;
            anim.SetInteger("moveX", -1);
            anim.SetInteger("moveY", 0);
        } else if (currentInput == KeyCode.S)
        {
            endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 30);
            playerY++;
            anim.SetInteger("moveX", 0);
            anim.SetInteger("moveY", -1);
        } else if (currentInput == KeyCode.D)
        {
            endPos = new Vector2(gameObject.transform.position.x + 30, gameObject.transform.position.y);
            playerX++;
            anim.SetInteger("moveX", 1);
            anim.SetInteger("moveY", 0);
        }
        bool playedSound = false;
        while (Vector2.Distance(gameObject.transform.position, endPos) > 0)
        {
            float timeFrac = (Time.time - startTime) / duration;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, timeFrac);
            speed = 1;

            if (timeFrac > 0.5f && !playedSound)
            {
                playedSound = true;
                trail.Play();
                if (levelMap[playerY, playerX] == 0)
                {
                    step.Play();
                }
                else if (levelMap[playerY, playerX] == 5 || levelMap[playerY, playerX] == 6)
                {
                    eat.Play();
                    levelMap[playerY, playerX] = 0;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        //gameObject.transform.position = endPos;

        if (teleport == 1)
        {
            gameObject.transform.position = new Vector3(360, 0, 0);
            playerX = 25;
            playerY = 14;
        } else if (teleport == 2)
        {
            gameObject.transform.position = new Vector3(-360, 0, 0);
            playerX = 1;
            playerY = 14;
        }
        teleport = 0;
        speed = 0;
        isMoving = false;
    }

    private int getLevelItem(KeyCode direction)
    {
        try
        {
            if (direction == KeyCode.W)
            {
                return levelMap[playerY - 1, playerX];
            }

            if (direction == KeyCode.A)
            {
                return levelMap[playerY, playerX - 1];
            }

            if (direction == KeyCode.S)
            {
                return levelMap[playerY + 1, playerX];
            }

            if (direction == KeyCode.D)
            {
                return levelMap[playerY, playerX + 1];
            }
        }
        catch
        {
            
        }
        return -1;
    }

    private bool IsWalkable(KeyCode direction)
    {
        int val = -1;
        try
        {
            if (direction == KeyCode.W)
            {
                val = levelMap[playerY - 1, playerX];
            }

            if (direction == KeyCode.A)
            {
                val = levelMap[playerY, playerX - 1];
            }

            if (direction == KeyCode.S)
            {
                val = levelMap[playerY + 1, playerX];
            }

            if (direction == KeyCode.D)
            {
                val = levelMap[playerY, playerX + 1];
            }
        } catch // out of bounds error only occurs when the player is going through the teleporter
        {
            // teleport player
        }

        //Debug.Log("playerX: " + playerX + " playerY: " + playerY + " val: " + val);

        return val == 0 || val == 5 || val == 6;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameObject.FindAnyObjectByType<GameManager>().state == GameManager.State.Ingame)
        {

            if (collision.gameObject.tag == "Pellet")
            {
                Destroy(collision.gameObject);
                scoreManager.increaseScore(10);
                StartCoroutine("CheckWin");
            }

            if (collision.gameObject.tag == "PowerPellet")
            {
                Destroy(collision.gameObject);
                scoreManager.increaseScore(10);
                ghostManager.powerPelletEaten();
                StartCoroutine("CheckWin");
            }

            if (collision.gameObject.tag == "Cherry")
            {

                Destroy(collision.gameObject);
                scoreManager.increaseScore(100);
            }
            GhostController ghostCon = null;
            try
            {
                ghostCon = collision.gameObject.GetComponent<GhostController>();
            }
            catch
            {

            }

            if (ghostCon != null)
            {
                // checkstate
                if (ghostCon.state == GhostController.State.Walking)
                {
                    PlayerDeath();
                }
                else if (ghostCon.state == GhostController.State.Scared || ghostCon.state == GhostController.State.Recovering)
                {
                    ghostCon.KillGhost();
                }
            }

            if (collision.gameObject.name == "LeftTeleporter")
            {
                teleport = 1;
            }

            if (collision.gameObject.name == "RightTeleporter")
            {
                teleport = 2;
            }
        }
    }

    public void PlayerDeath()
    {
        lives--;
        if (lives >= 0)
        {
            for (int i = 0; i <= 3 - lives - 1; i++)
            {
                livesUI[i].SetActive(false);
            }
            state = State.Dead;
            deathEffect.Play();
            anim.Play("playerDeath");
            StartCoroutine("SpawnLocation");
        } else
        {
            GameObject.FindAnyObjectByType<GameManager>().GameOver();
        }
        
    }

    public IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject pel = null;
        GameObject powPel = null;
        try
        {
            pel = GameObject.FindGameObjectWithTag("Pellet");
            powPel = GameObject.FindGameObjectWithTag("PowerPellet");
        }
        finally
        {
            if (pel == null && powPel == null)
            {
                GameObject.FindAnyObjectByType<GameManager>().GameOver();
            }
        }
    }

    public IEnumerator SpawnLocation()
    {
        yield return new WaitForSeconds(0.75f);
        transform.position = new Vector3(-360, 390, 0);
        anim.Play("playerIdle");
        state = State.Alive;
        playerX = 1;
        playerY = 1;
    }

}
