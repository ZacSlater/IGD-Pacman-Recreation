using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject preMadeLevel;
    public GameObject[] sprites;
    int rowMax;
    int colMax;
    int[] walls = { 1, 2, 3, 4, 7 };
    private GameObject[,] level;
    public GameObject lvlParent;
    int[,] levelMap =
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
 };
    /* 
     0- empty
     1- outside corner
     2- outside wall
     3- inside corner
     4- inside wall
     5- standard pellet
     6- power pellet
     7- t junction
     */

    void Start()
    {
        Destroy(preMadeLevel);
        placeSprites();
        checkRotation();
        dupLevel();
    }

    public void placeSprites()
    {
        rowMax = levelMap.GetLength(0) - 1;
        colMax = levelMap.GetLength(1) - 1;
        level = new GameObject[rowMax + 1, colMax + 1];
        for (int row = 0; row <= rowMax; row++)
        {
            for (int col = 0; col <= colMax; col++)
            {
                int spriteNum = levelMap[row, col];
                level[row, col] = Instantiate(sprites[spriteNum], new Vector3(col * 30 - 30 * colMax, rowMax * 30 - row * 30, 0), transform.rotation, lvlParent.transform);

                // rotation

                if (spriteNum == 2 || spriteNum == 4) // wall piece
                {
                    rotateWall(level[row, col], row, col);
                } else if (spriteNum == 1 || spriteNum == 3) // corner piece
                {
                    //rotateCorner(level[row, col], row, col);
                } else if (spriteNum == 7) // t-junction
                {
                    //rotateTJunction(level[row, col], row, col);
                }
            }
        }
    }

    public void rotateWall(GameObject sprite, int row, int col)
    {
        // if wall has a piece (not 5,6) next to it, horizontal
        // if wall has a piece (not 5,6) above and below it, vertical
        int horVal = 0;
        int vertVal = 0;

        if (!(col == 0 || col == colMax || row == 0 || row == rowMax))
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                horVal++;
            }

            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
            {
                horVal++;
            }

            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0)) // is not a piece horizontally is a pellet
            {
                vertVal++;
            }

            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
            {
                vertVal++;
            }

        } else if (col == 0 && !(row == 0 || row == rowMax)) // if on the left edge with an adjacent piece
        {
            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
            {
                horVal++;
            }

            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0)) // is not a piece horizontally is a pellet
            {
                vertVal++;
            }

            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
            {
                vertVal++;
            }

            /*Debug.Log("------");
            Debug.Log("above val: " + levelMap[row - 1, col]);
            Debug.Log(!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0));
            Debug.Log("horVal: " + horVal);
            Debug.Log("vertVal: " + vertVal);
            Debug.Log("------");*/
        } else if (col == colMax && !(row == 0 || row == rowMax)) // if on the right edge with an adjacent piece
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                horVal++;
            }

            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0)) // is not a piece horizontally is a pellet
            {
                vertVal++;
            }

            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
            {
                vertVal++;
            }
        } else if (row == 0 && !(col == 0 || col == colMax)) // if on the right edge with an adjacent piece
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                horVal++;
            }

            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
            {
                horVal++;
            }

            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
            {
                vertVal++;
            }
        } else if (row == rowMax && !(col == 0 || col == colMax)) // if on the right edge with an adjacent piece
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                horVal++;
            }

            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
            {
                horVal++;
            }

            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0)) // is not a piece horizontally is a pellet
            {
                vertVal++;
            }
        }

        if (horVal > vertVal)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (horVal == vertVal)
        {
            if (col == 0 || col == colMax)
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        if (row == 0 && col == colMax)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (row == rowMax && col == colMax)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

    }

    /*public void rotateCorner(GameObject sprite, int row, int col)
    {
        int hits = 0;
        if (!(row == 0 || col == 0 || row == rowMax || col == colMax))
        {
            // check NE
            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0 || levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                hitS = S + 2;
            }

            // check SE
            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0 || levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0)) // is not a piece horizontally is a pellet
            {
                //sprite.transform.rotation = new Quaternion(0, 0, 0, 90);
                hitS = S + 2;
            }

            // check SW
            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0 || levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 270);
                hitS = S + 2;
            }

            // check NW
            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0 || levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 180);
                hitS = S + 2;
            }
        }

        if (hits > 1)
        {
            // do a priority check
        }

        // seperate check for first row
        if (row == 0)
        {

        }
        // seperate check for first col
        // seperate check for last row
        // seperate check for last col

    }

    public void rotateTJunction(GameObject sprite, int row, int col)
    {
        return;
    }*/

    public void checkRotation()
    {
        for (int row = 0; row <= rowMax; row++)
        {
            for (int col = 0; col <= colMax; col++)
            {
                // check that its a corner or t-junction piece because all of the wall pieces are fine

                // corner first cause easier
                // check which surronding pieces are not (pellets or air)

                if (levelMap[row, col] == 1 || levelMap[row, col] == 3) // corner piece
                {
                    
                    try
                    {
                        int N = 0;
                        int S = 0;
                        int E = 0;
                        int W = 0;
                        
                        if (!(row == 0 || col == 0 || row == rowMax || col == colMax))
                        {
                            // check N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // check S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }
                        }
                        else if (col == 0 && !(row == 0 || row == rowMax))
                        {
                            W++;
                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }
                        }
                        else if (row == 0 && !(col == 0 || col == colMax))
                        {
                            N++;

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        }
                        else if (col == colMax && !(row == 0 || row == rowMax))
                        {
                            E++;

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        }
                        else if (row == rowMax && !(col == 0 || col == colMax))
                        {
                            S++;

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        }

                        if (row == 0 && col == 0)
                        {
                            S = S + 2;
                            E = E + 2;
                        } 
                        else if (row == 0 && col == colMax)
                        {
                            E++;
                            // W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                        } 
                        else if (row == rowMax && col == 0)
                        {
                            S++;
                            // E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }
                        } 
                        else if (row == rowMax && col == colMax)
                        {
                            S++;
                            E++;
                            // W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }
                        }

                        if (N > S)
                        {
                            if (E > W) // if NE
                            {
                                level[row, col].transform.rotation = Quaternion.Euler(0, 0, 90);
                            }
                            else // if NW
                            {
                                level[row, col].transform.rotation = Quaternion.Euler(0, 0, 180);
                            }
                        }
                        else
                        {
                            if (E > W) // if SE
                            {
                                level[row, col].transform.rotation = Quaternion.Euler(0, 0, 0);
                            }
                            else // if SW
                            {
                                level[row, col].transform.rotation = Quaternion.Euler(0, 0, 270);
                            }
                        }

                    }
                    catch
                    {
                        Debug.Log("Row: " + row + " Col: " + col);
                    }
                    

                } 
                else if (levelMap[row, col] == 7) // t-junction
                {
                    //try
                    //{
                        int N = 0;
                        int S = 0;
                        int E = 0;
                        int W = 0;

                        if (!(row == 0 || col == 0 || row == rowMax || col == colMax))
                        {
                            // check N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // check S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }
                        }
                        else if (col == 0 && !(row == 0 || row == rowMax))
                        {
                            W++;
                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }
                        }
                        else if (row == 0 && !(col == 0 || col == colMax))
                        {
                            N++;

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S -2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        Debug.Log(N + "" + S + "" + E + "" + W);
                        }
                        else if (col == colMax && !(row == 0 || row == rowMax))
                        {
                            E++;

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // S
                            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                S = S + 2;
                                if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        S = S + 2;
                                    }
                                    else
                                    {
                                        S = S - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        }
                        else if (row == rowMax && !(col == 0 || col == colMax))
                        {
                            S++;

                            // N
                            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                                N = N + 2;
                                if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                                {
                                    // if vertical
                                    if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                    {
                                        N = N + 2;
                                    }
                                    else
                                    {
                                        N = N - 2;
                                    }
                                }
                            }

                            // check E
                            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                E = E + 2;
                                if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        E = E + 2;
                                    }
                                    else
                                    {
                                        E = E - 2;
                                    }
                                }
                            }

                            // check W
                            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                            {
                                //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                                W = W + 2;
                                if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                                {
                                    // if horizontal
                                    if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                    {
                                        W = W + 2;
                                    }
                                    else
                                    {
                                        W = W - 2;
                                    }
                                }
                            }
                        }

                    if (row == 0 && col == 0)
                    {
                        S = S + 2;
                        E = E + 2;
                    }
                    else if (row == 0 && col == colMax)
                    {
                        E++;
                        // W
                        if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                        {
                            W = W + 2;
                            if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                            {
                                // if horizontal
                                if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                {
                                    W = W + 2;
                                }
                                else
                                {
                                    W = W - 2;
                                }
                            }
                        }

                        // S
                        if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0))
                        {
                            //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                            S = S + 2;
                            if (levelMap[row + 1, col] == 2 || levelMap[row + 1, col] == 4)
                            {
                                // if vertical
                                if (level[row + 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                {
                                    S = S + 2;
                                }
                                else
                                {
                                    S = S - 2;
                                }
                            }
                        }

                    }
                    else if (row == rowMax && col == 0)
                    {
                        S++;
                        // E
                        if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
                        {
                            E = E + 2;
                            if (levelMap[row, col + 1] == 2 || levelMap[row, col + 1] == 4)
                            {
                                // if horizontal
                                if (level[row, col + 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                {
                                    E = E + 2;
                                }
                                else
                                {
                                    E = E - 2;
                                }
                            }
                        }

                        // N
                        if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                        {
                            //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                            N = N + 2;
                            if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                            {
                                // if vertical
                                if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                {
                                    N = N + 2;
                                }
                                else
                                {
                                    N = N - 2;
                                }
                            }
                        }
                    }
                    else if (row == rowMax && col == colMax)
                    {
                        S++;
                        E++;
                        // W
                        if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
                        {
                            W = W + 2;
                            if (levelMap[row, col - 1] == 2 || levelMap[row, col - 1] == 4)
                            {
                                // if horizontal
                                if (level[row, col - 1].transform.rotation == Quaternion.Euler(0, 0, 90))
                                {
                                    W = W + 2;
                                }
                                else
                                {
                                    W = W - 2;
                                }
                            }
                        }

                        // N
                        if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0))
                        {
                            //sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                            N = N + 2;
                            if (levelMap[row - 1, col] == 2 || levelMap[row - 1, col] == 4)
                            {
                                // if vertical
                                if (level[row - 1, col].transform.rotation == Quaternion.Euler(0, 0, 0))
                                {
                                    N = N + 2;
                                }
                                else
                                {
                                    N = N - 2;
                                }
                            }
                        }
                    }

                    if (N < S && N < E && N < W) // North is least
                    {
                        level[row, col].transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (S < E && S < W) // South is least
                    {
                        level[row, col].transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else if (E < W) // East is least
                    {
                        level[row, col].transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                    else // West is least
                    {
                        level[row, col].transform.rotation = Quaternion.Euler(0, 0, 90);
                    }


                    //}
                    /*catch
                    {
                        Debug.Log("Row: " + row + " Col: " + col);
                    }*/
                }
            }
        }
    }

    public void dupLevel()
    {
        Instantiate(lvlParent, new Vector3(30, 0, 0), Quaternion.Euler(0, 180, 0));
        /*GameObject removedLastRow = lvlParent;
        foreach (Transform sprite in lvlParent.transform)
        {
            if (sprite.position.y == 0)
            {
                Destroy(sprite.gameObject);
            }
        }
        Instantiate(removedLastRow, new Vector3(30, 0, 0), Quaternion.Euler(0, 0, 180));
        Instantiate(removedLastRow, new Vector3(0, 0, 0), Quaternion.Euler(180, 0, 0));*/

        GameObject q4 = Instantiate(lvlParent, new Vector3(30, 0, 0), Quaternion.Euler(0, 0, 180));
        foreach (Transform sprite in q4.transform)
        {
            //Debug.Log(sprite.name + sprite.position.y);
            if (sprite.position.y > -15)
            {
                
                Destroy(sprite.gameObject);
            }
        }
        GameObject q3 = Instantiate(q4, new Vector3(0, 0, 0), Quaternion.Euler(180, 0, 0));
        foreach (Transform sprite in q3.transform)
        {
            //Debug.Log(sprite.name + sprite.position.y);
            if (sprite.position.y > -15)
            {

                Destroy(sprite.gameObject);
            }
        }
    }
}
