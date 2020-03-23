using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour {

    public int charMov;
    public Transform chara;
    Grid grid;
    public bool gainmove;
    public bool waitMove;
    

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    
    void Start () {
        gainmove = false;
        waitMove = true;
        TurnBase.enemyTurn = false;
	}
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0) && waitMove == true)
        {
            Vector3 trt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(trt);
            FindZone(chara.position, charMov);
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && gainmove == true)
        {
            waitMove = false;
            gainmove = false;
            Vector3 rrr = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transition(rrr);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Ход передан");
            TurnBase.enemyTurn = true;
        }
            
    }


    void FindZone(Vector3 pos, int mov)
    {
        Node startNode = grid.NodeFromWorldPoint(pos);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node node = openSet[0];

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = GetDistance(node);

                if (newCostToNeighbour <= mov)
                {
                    neighbour.parent = node;


                    openSet.Add(neighbour);
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);
        }
       

            grid.moveField = closedSet;
        
        gainmove = true;
    }


    int GetDistance(Node node)
    {
        int n;
        for(n = 1; node.parent != null; n++)
        {
            node = node.parent;
        }
        return n;
    }


    void leadPath(Node start, Node finish)
    {
        List<Node> path = new List<Node>();
        Node currentNode = finish;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.walkP = path;
    }


    void transition(Vector3 trr)
    {
        
        Node startNode = grid.NodeFromWorldPoint(chara.position);
        Node finishNode = grid.NodeFromWorldPoint(trr);
        leadPath(startNode, finishNode);
        chara.position = finishNode.worldPosition;
        grid.moveField.Clear();
        grid.walkP.Clear();
        waitMove = true;
    }

    
}
