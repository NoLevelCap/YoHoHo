using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeClickManager : MonoBehaviour, IPointerClickHandler {

    public Node node;
    public MapRenderer map;

    private Image Area;

    public bool OnVisit;

    // Use this for initialization
    void Start () {
        Area = GetComponentsInChildren<Image>()[0];
	}

    private void Update()
    {
        CalculateNodeView();
    }

    private void CalculateNodeView()
    {
        if (node.visited || OnVisit)
        {
            OnVisit = node.visited = true;
            Area.sprite = map.NodeIcons[1];
            return;
        }

        if (GameManager.SelectedNode == node)
        {
            Area.sprite = map.NodeIcons[2];
            return;
        }

        Area.sprite = map.NodeIcons[0];

        return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Line connection in GameManager.LastVisitedNode.fConnections)
        {
            if (connection.NB == node)
            {
                GameManager.SelectedNode = node;
                break;
            }
        }
    }
}
