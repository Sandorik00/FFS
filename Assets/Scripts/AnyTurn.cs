using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyTurn : MonoBehaviour
{

    //public Transform seeker, target;
    Pathfinding pathfinding;

    

    public SpriteRenderer sprite;

    void Awake()
    {
        TurnBase.yetTurn = false;
        pathfinding = GetComponent<Pathfinding>();

        //sprite.transform = new Vector3(1, 1, 1);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if(TurnBase.enemyTurn == true && TurnBase.yetTurn == false)
        {
            TurnBase.yetTurn = true;
            pathfinding.enemyPath();
        }
    }
}
