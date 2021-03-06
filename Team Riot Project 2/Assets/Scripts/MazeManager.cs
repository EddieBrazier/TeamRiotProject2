﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    //list of maze end locations
    public List<Transform> endLocations;
    
    //maze end prefab
    public GameObject endPrefab;

    bool tutorial;
    public GameObject mazebox;
    private GameObject activeBox;
    GameObject gameUI;
    // Start is called before the first frame update
    void Start()
    {
        //get a random number and spawn the endpoint of the maze
        int mazeNum = Random.Range(0, 3);
        GameObject mazeEnd = Instantiate(endPrefab);
        mazeEnd.transform.SetParent(transform);
        mazeEnd.transform.position = endLocations[mazeNum].position;
        
        gameUI = GameObject.FindGameObjectWithTag("interface");

        if (mazebox == null)
            mazebox = Resources.Load("mazeBox") as GameObject;

        if (PlayerManager.instance != null)
        {
            tutorial = PlayerManager.instance.tutorial;
        }

        Vector3 textpos = new Vector3(151, 275, 0);
        if (tutorial == false)
        {
            activeBox = Instantiate(mazebox, textpos, Quaternion.identity);
            //runetut.GetComponentInChildren<Text>().text = "TESTING";
            activeBox.transform.parent = gameUI.transform;
            activeBox.AddComponent<TutorialBox>();
            activeBox.GetComponent<TutorialBox>().PushText("Maze Fly Tutorial:\n\n\n" + " \nHere you must guide the creature " +
                "through the maze by clicking and dragging.");
            

            //Debug.Log(gameUI);
            //Debug.Log(runetut);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if(activeBox != null)
        {
            Destroy(activeBox);
        }
    }
}
