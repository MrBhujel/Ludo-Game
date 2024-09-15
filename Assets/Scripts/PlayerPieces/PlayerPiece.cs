using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool moveNow;
    public bool isReady = false;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMove;

    public PathPointParent pathParent;

    Coroutine comeOutside;
    Coroutine moveSteps;

    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;

    // Awake functions run before Start()
    public void Awake()
    {
        pathParent = FindObjectOfType<PathPointParent>();
    }

    // Making Player come out of home
    public void MakePlayerReadyToMove(PathPoint[] pathPointsToMoveOn)
    {
        if (comeOutside == null)
        {
            comeOutside = StartCoroutine(ComeOutside(pathPointsToMoveOn));
            numberOfStepsAlreadyMove = 1;

            previousPathPoint = pathPointsToMoveOn[0];
            currentPathPoint = pathPointsToMoveOn[0];
            currentPathPoint.AddPlayerPiece(this);
            GameManager.gm.AddPathPoint(currentPathPoint);

            GameManager.gm.canRollDice = true;
            GameManager.gm.selfDice = true;
            GameManager.gm.transferDice = false;
        }
    }

    IEnumerator ComeOutside(PathPoint[] pathPointsToMoveOn)
    {
        isReady = true;
        yield return new WaitForSeconds(0.2f);
        transform.position = pathPointsToMoveOn[0].transform.position;

        comeOutside = null;
    }

    // This part moves the player pieces, we'll call it into respective player scripts
    public void MoveSteps(PathPoint[] pathPointsToMoveOn)
    {
        if (moveSteps == null)
        {
            moveSteps = StartCoroutine(MoveStep_Enum(pathPointsToMoveOn));
        }
    }

    IEnumerator MoveStep_Enum(PathPoint[] pathPointsToMoveOn)
    {
        numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;

        for (int i = numberOfStepsAlreadyMove; i < (numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
            if (IsPathPointAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveOn))
            {
                transform.position = pathPointsToMoveOn[i].transform.position;

                yield return new WaitForSeconds(0.35f);
            }
        }

        if (IsPathPointAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveOn))
        {
            numberOfStepsAlreadyMove += numberOfStepsToMove;
            GameManager.gm.numberOfStepsToMove = 0;

            GameManager.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);

            currentPathPoint = pathPointsToMoveOn[numberOfStepsAlreadyMove - 1];
            currentPathPoint.AddPlayerPiece(this);

            GameManager.gm.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (GameManager.gm.numberOfStepsToMove != 6)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.transferDice = true;
            }
            else
            {
                GameManager.gm.selfDice = true;
                GameManager.gm.transferDice = false;
            }

            GameManager.gm.numberOfStepsToMove = 0;
        }

        GameManager.gm.canPlayerMove = true;
        GameManager.gm.RollingDiceManager();

        moveSteps = null;
    }

    bool IsPathPointAvailableToMove(int numOfStepsToMove_, int numOfStepsAlreadyMove, PathPoint[] pathPointsToMove)
    {
        if (numOfStepsToMove_ == 0)
        {
            return false;
        }
        int leftNumberOfPath = pathPointsToMove.Length - numOfStepsAlreadyMove;
        if (leftNumberOfPath >= numOfStepsToMove_)
        {
            return true;
        }
        else return false;
    }
}
