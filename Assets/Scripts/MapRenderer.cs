using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapRenderer : MonoBehaviour {

    public int NodeWidth, NodeMaxHeight;
    public Sprite[] MapIcons;
    public Sprite[] NodeIcons;
    public GameObject LinePrefab, NodePrefab;
    public int Seed;

    private Random.State PreSeed;
    private Node[,] nodeList;
    private Transform NodeContainer, LineContainer;

    private RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        NodeContainer = transform.Find("Nodes");
        LineContainer = transform.Find("Lines");

        if (GameManager.nl == null)
        {
            PreSeed = Random.state;
            Random.InitState(Seed);

            CreateMap();

            Random.state = PreSeed;

            GameManager.nl = nodeList;
        } else
        {
            nodeList = GameManager.nl;
            LoadMap(GameManager.nl);
            CreateLinks();
        }
	}

    private void CreateMap()
    {
        PopulateNodeList();
        CreateLinks();

        if (GameManager.LastVisitedNode == null) {
            GameManager.LastVisitedNode = nodeList[0, 0];
            GameManager.LastVisitedNode.visited = true;
        }
    }

    private void LoadMap(Node[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != null)
                {
                    LoadNode(map[x, y]);
                }
            }
        }
    }

    private void PopulateNodeList()
    {
        nodeList = new Node[NodeWidth, NodeMaxHeight];

        float Width = rt.sizeDelta.x;
        float Height = rt.sizeDelta.y;

        for (int x = 0; x < nodeList.GetLength(0); x++)
        {
            int max = 1;
            if (x != 0 && x != 1 && x != nodeList.GetLength(0)-1) { max = Random.Range(1, nodeList.GetLength(1) + 1); }
            for (int y = 0; y < max; y++)
            {
                Node cN = CreateNode(
                    new Vector2(Width / nodeList.GetLength(0) * (x + .5f),
                    (Height / max) / 2f + (y * (Height / max)))
                    );
                nodeList[x, y] = cN;

                cN.groupingSize = max;
            }
        }

    }

    private void CreateLinks()
    {
        for (int x = 0; x < nodeList.GetLength(0)-1; x++)
        {
            int SM = nodeList[x, 0].groupingSize;
            int EM = nodeList[x+1, 0].groupingSize;

            if (EM == SM)
            {
                for (int y = 0; y < SM; y++)
                {
                    CreateLineToNode(nodeList[x, y], nodeList[x + 1, y]);
                }
                continue;
            }

            if (EM == 1)
            {
                for (int y = 0; y < SM; y++)
                {
                    CreateLineToNode(nodeList[x, y], nodeList[x + 1, 0]);
                }
                continue;
            }

            if (SM == 1)
            {
                for (int y = 0; y < EM; y++)
                {
                    CreateLineToNode(nodeList[x, 0], nodeList[x + 1, y]);
                }
                continue;
            }

            

            if (SM > EM) {
                for (int y = 0; y < SM; y++)
                {
                    int calc = Mathf.CeilToInt(((float)(y + 1) / (float)SM) * EM) - 1;
                    CreateLineToNode(nodeList[x, y], nodeList[x + 1, calc]);
                }
                continue;
            }

            if (SM < EM)
            {
                for (int y = 0; y < EM; y++)
                {
                    int calc = Mathf.CeilToInt(((float)(y + 1) / (float)EM) * SM) - 1;
                    CreateLineToNode(nodeList[x, calc], nodeList[x + 1, y]);
                }
                continue;
            }

        }
    }

    private Node CreateNode(Vector2 pos)
    {
        Node.LevelType lt = (Node.LevelType)Random.Range(0, 3);
        GameObject Node1 = Instantiate<GameObject>(NodePrefab);
        RectTransform rt = Node1.GetComponent<RectTransform>();
        rt.SetParent(NodeContainer, false);
        rt.anchoredPosition = pos;

        Image Identifier = Node1.GetComponentsInChildren<Image>()[1];
        if ((int)lt < MapIcons.Length)
        {
            Identifier.sprite = MapIcons[(int)lt];
        }
        else
        {
            Identifier.enabled = false;
        }

        Node ChosenNode = new Node(lt, pos, Node1);

        NodeClickManager NCM = Node1.GetComponent<NodeClickManager>();
        NCM.map = this;
        NCM.node = ChosenNode;

        return ChosenNode;
    }

    private Line CreateLineToNode(Node a, Node b)
    {
        GameObject LineO = Instantiate<GameObject>(LinePrefab);
        RectTransform rt = LineO.GetComponent<RectTransform>();
        rt.SetParent(LineContainer, false);
        rt.anchoredPosition = a.lPos;

        LineRenderer LineRenderer = LineO.GetComponent<LineRenderer>();
        LineRenderer.SetPosition(1, new Vector3(b.lPos.x - a.lPos.x, b.lPos.y - a.lPos.y, 0));

        return new Line(a, b);
    }

    private void LoadNode(Node node)
    {
        GameObject NewNodeInstance = Instantiate<GameObject>(NodePrefab);
        RectTransform rt = NewNodeInstance.GetComponent<RectTransform>();
        rt.SetParent(NodeContainer, false);
        rt.anchoredPosition = node.lPos;

        Image Identifier = NewNodeInstance.GetComponentsInChildren<Image>()[1];
        if ((int)node.type < MapIcons.Length)
            Identifier.sprite = MapIcons[(int)node.type];
        else
            Identifier.enabled = false;

        NodeClickManager NCM = NewNodeInstance.GetComponent<NodeClickManager>();
        NCM.map = this;
        NCM.node = node;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Node
{
    public Vector2 lPos;
    public GameObject NodePos;
    public int groupingSize;
    public LevelType type;
    public bool visited;

    public List<Line> fConnections;

    public enum LevelType
    {
        Peaceful, Difficult, Normal
    }

    public Node(LevelType tp, Vector2 lp, GameObject np)
    {
        this.type = tp;
        this.lPos = lp;
        this.NodePos = np;

        fConnections = new List<Line>();
    }
    
}

public class Line
{
    public Node NA, NB;
    public Vector2 APos, BPos;

    public Line(Node A, Node B)
    {
        this.NA = A;
        this.NB = B;

        this.APos = NA.lPos;
        this.BPos = NB.lPos;

        NA.fConnections.Add(this);
    }
    
}
