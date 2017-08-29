using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour {

    public Sprite Diamond, Circle;

    private RectTransform Ship;
    private Transform NodeContainer;

    private int[] points;

    private void Start()
    {
        
    }

    private void Awake()
    {
        Ship = transform.Find("SmallShip").GetComponent<RectTransform>();
        NodeContainer = transform.Find("ProgressBarDetail").Find("Nodes");

        switch (GameManager.SelectedNode.type)
        {
            case Node.LevelType.Peaceful:
                LoadProgressBar(1);
                break;
            case Node.LevelType.Difficult:
                LoadProgressBar(5);
                break;
            case Node.LevelType.Normal:
                LoadProgressBar(3);
                break;
            default:
                break;
        }

        FindObjectOfType<SailManager>().activepoints = points;
    }

    public void LoadProgressBar(int EventAmounts)
    {
        foreach (Transform child in NodeContainer)
        {
            child.GetComponent<Image>().sprite = Circle;
        }

            points = new int[EventAmounts];
        for (int i = 0; i < EventAmounts; i++)
        {
            int r;
            do
            {
                r = UnityEngine.Random.Range(1, NodeContainer.childCount);
            } while (points.Contains(r) && points.Length >= NodeContainer.childCount);
            points[i] = r;
            NodeContainer.GetChild(points[i]).GetComponent<Image>().sprite = Diamond;
        }
        Array.Sort(points);

        
    }





}
