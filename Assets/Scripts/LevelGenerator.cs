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
    }

    public void placeSprites()
    {
        rowMax = levelMap.GetLength(0) - 1;
        colMax = levelMap.GetLength(1) - 1;
        for (int row = 0; row <= rowMax; row++)
        {
            for (int col = 0; col <= colMax; col++)
            {
                int spriteNum = levelMap[row, col];
                GameObject sprite = Instantiate(sprites[spriteNum], new Vector3(transform.position.x + col*30, transform.position.y-row*30, transform.position.z), transform.rotation);
                
                // rotation

                if (spriteNum == 2 || spriteNum == 4) // wall piece
                {
                    rotateWall(sprite, row, col);
                } else if (spriteNum == 1 || spriteNum == 3) // corner piece
                {
                    rotateCorner(sprite, row, col);
                } else if (spriteNum == 7) // t-junction
                {
                    rotateTJunction(sprite, row, col);
                }
            }
        }
    }

    public void rotateWall(GameObject sprite, int row, int col)
    {
        // if wall has a piece (not 5,6) next to it, horizontal
        // if wall has a piece (not 5,6) above and below it, vertical

        if (!(col == 0 || col == colMax))
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0 || levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                return;
            }
        }

        if (col == 0) // if on the left edge with an adjacent piece
        { 
            if (!(levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0))
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                return;
            }
        }

        if (col == colMax) // if on the right edge with an adjacent piece
        {
            if (!(levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0))
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                return;
            }
        }


    }

    public void rotateCorner(GameObject sprite, int row, int col)
    {
        int hits = 0;
        if (!(row == 0 || col == 0 || row == rowMax || col == colMax))
        {
            // check NE
            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0 || levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
                hits++;
            }

            // check SE
            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0 || levelMap[row, col + 1] == 5 || levelMap[row, col + 1] == 6 || levelMap[row, col + 1] == 0)) // is not a piece horizontally is a pellet
            {
                //sprite.transform.rotation = new Quaternion(0, 0, 0, 90);
                hits++;
            }

            // check SW
            if (!(levelMap[row + 1, col] == 5 || levelMap[row + 1, col] == 6 || levelMap[row + 1, col] == 0 || levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 270);
                hits++;
            }

            // check NW
            if (!(levelMap[row - 1, col] == 5 || levelMap[row - 1, col] == 6 || levelMap[row - 1, col] == 0 || levelMap[row, col - 1] == 5 || levelMap[row, col - 1] == 6 || levelMap[row, col - 1] == 0)) // is not a piece horizontally is a pellet
            {
                sprite.transform.rotation = Quaternion.Euler(0, 0, 180);
                hits++;
            }
        }

        if (hits > 1)
        {
            // do a priority check
        }

        // seperate check for first row
        // seperate check for first col
        // seperate check for last row
        // seperate check for last col

    }

    public void rotateTJunction(GameObject sprite, int row, int col)
    {
        return;
    }

}
