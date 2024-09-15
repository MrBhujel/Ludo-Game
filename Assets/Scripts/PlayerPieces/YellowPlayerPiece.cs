using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPiece : PlayerPiece
{
    RollingDice yellowHomeRollindDice;

    void Start()
    {
        yellowHomeRollindDice = GetComponentInParent<YellowHome>().rollingDice;
    }
    public void OnMouseDown()
    {
        if (GameManager.gm.rollingDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rollingDice == yellowHomeRollindDice && GameManager.gm.numberOfStepsToMove == 6)
                {
                    GameManager.gm.yellowOutPlayers += 1;
                    MakePlayerReadyToMove(pathParent.yellowPlayerPathPoints);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rollingDice == yellowHomeRollindDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MoveSteps(pathParent.yellowPlayerPathPoints);
            }
        }
    }
}
