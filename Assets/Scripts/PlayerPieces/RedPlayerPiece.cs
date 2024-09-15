using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPiece : PlayerPiece
{
    RollingDice redHomeRollingDice;

    void Start()
    {
        redHomeRollingDice = GetComponentInParent<RedHome>().rollingDice;
    }
    public void OnMouseDown()
    {
        if (GameManager.gm.rollingDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rollingDice == redHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.redOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.redPlayerPathPoints);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rollingDice == redHomeRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MoveSteps(pathParent.redPlayerPathPoints);
            }
        }
    }
}
