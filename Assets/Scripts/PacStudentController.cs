using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private KeyCode lastInput;
    private KeyCode currentInput;
    private int playerX, playerY;
    private bool isMoving;
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
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
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
        Debug.Log(levelMap[1, 1]);
        Debug.Log(levelMap[3, 1]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = KeyCode.W;
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = KeyCode.A;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = KeyCode.S;
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = KeyCode.D;
        }

        if (!isMoving)
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
                    else
                    {
                        // stop animations
                    }
                }
        }
    }




    IEnumerator LerpToLocation()
    {
        float startTime = Time.time;
        float duration = 0.5f;
        Vector2 startPos = gameObject.transform.position;
        Vector2 endPos = new Vector2();

        if (currentInput == KeyCode.W)
        {
            endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 30);
            playerY--;
            // animation for direction
        } else if (currentInput == KeyCode.A)
        {
            endPos = new Vector2(gameObject.transform.position.x - 30, gameObject.transform.position.y);
            playerX--;
            // animation for direction
        } else if (currentInput == KeyCode.S)
        {
            endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 30);
            playerY++;
            // animation for direciton
        } else if (currentInput == KeyCode.D)
        {
            endPos = new Vector2(gameObject.transform.position.x + 30, gameObject.transform.position.y);
            playerX++;
            // animation for direction
        }

        while (Vector2.Distance(gameObject.transform.position, endPos) > 0)
        {
            float timeFrac = (Time.time - startTime) / duration;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, timeFrac);
            //Debug.Log("in while loop");
            yield return new WaitForSeconds(0.01f);
        }
        
        gameObject.transform.position = endPos;

        // playsound

        // play dust particle effect



        isMoving = false;
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

        Debug.Log("playerX: " + playerX + " playerY: " + playerY + " val: " + val);

        return val == 0 || val == 5 || val == 6;
    }
}
