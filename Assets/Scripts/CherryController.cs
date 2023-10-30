using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    GameObject currentCherry;
    Vector2 endPos;
    
    void Start()
    {
        InvokeRepeating("SpawnCherry", 0, 10.0f);
    }

    void SpawnCherry()
    {
        // try

        // top bottom left right
        int loc = Random.Range(1, 5);
        Vector3 spawnLoc = Vector3.zero;

        if (loc == 1) // top
        {
            int posX = Random.Range(3, 24);
            spawnLoc = new Vector3(-350 + posX * 30, 475, 0);
            endPos = new Vector2(-350 + posX * 30, -475);
        } else if (loc == 2) // right
        {
            int posY = Random.Range(3, 24);
            spawnLoc = new Vector3(800, 450 - posY * 30, 0);
            endPos = new Vector2(-800, 450 - posY * 30);
        }
        else if (loc == 3) // bottom
        {
            int posX = Random.Range(3, 24);
            spawnLoc = new Vector3(-350 + posX * 30, -475, 0);
            endPos = new Vector2(-350 + posX * 30, 475);
        } else if (loc == 4) // left
        {
            int posY = Random.Range(3, 24);
            spawnLoc = new Vector3(-800, 450 - posY * 30, 0);
            endPos = new Vector2(800, 450 - posY * 30);
        }

        currentCherry = Instantiate(cherryPrefab, spawnLoc, transform.rotation);
        
        StartCoroutine("LerpToLocation");
    }

    IEnumerator LerpToLocation()
    {
        float startTime = Time.time;
        float duration = 7.0f;
        Vector2 startPos = currentCherry.transform.position;

        while (Vector2.Distance(currentCherry.transform.position, endPos) > 0 && currentCherry != null)
        {
            float timeFrac = (Time.time - startTime) / duration;
            currentCherry.transform.position = Vector2.Lerp(startPos, endPos, timeFrac);

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(currentCherry);
    }
}
