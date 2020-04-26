using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour {

    public int charMov;
    public Transform chara;
    Grid grid;
    public bool gainmove, waitMove, waitMoveCheck;
    

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    
    void Start () {
        gainmove = false;
        waitMove = true;
        waitMoveCheck = true;
        TurnBase.enemyTurn = false;
	}
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0) && waitMove == true)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FindZone(chara.position, charMov);
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && gainmove == true && waitMoveCheck == true)
        {
            waitMoveCheck = false;
            waitMove = false;
            Vector3 targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(targetPoint);
            transition(targetPoint);
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
        //Node currentNode = finish;

        while (finish != start)
        {
            path.Add(finish);
            finish = finish.parent;
        }
        path.Reverse();

        grid.walkP = path;
    }


    void transition(Vector3 targetPos)
    {
        
        Node startNode = grid.NodeFromWorldPoint(chara.position);
        Node finishNode = grid.NodeFromWorldPoint(targetPos);
        if (finishNode.parent == null)
        {
            Debug.Log("Не туда воюешь!");
            waitMoveCheck = true;
            return;
        }
        leadPath(startNode, finishNode);
        chara.position = finishNode.worldPosition;
        foreach(Node node in grid.moveField)
        {
            if(node.parent != null) node.parent = null;
        }
        grid.moveField.Clear();
        grid.walkP.Clear();
        waitMoveCheck = true;
        gainmove = false;
        waitMove = true;
    }

    
}
