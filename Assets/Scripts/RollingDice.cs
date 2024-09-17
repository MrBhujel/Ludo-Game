using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer diceRollAnimation;
    [SerializeField] int numberRolled;

    Coroutine generateRandomDiceRoll;

    public int outPiece;
    public PathPointParent pathParent;
    PlayerPiece[] currentPlayerPiece;
    PathPoint[] pathPointToMoveOn;
    Coroutine moveSteps;
    PlayerPiece outPlayerPiece;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathPointParent>();
    }

    public void OnMouseDown()
    {
        generateRandomDiceRoll = StartCoroutine(DiceRolling());
    }

    public void MouseRoll()
    {
        generateRandomDiceRoll = StartCoroutine(DiceRolling());
    }


    IEnumerator DiceRolling()
    {
        GameManager.gm.transferDice = false;
        yield return new WaitForEndOfFrame();

        if (GameManager.gm.canRollDice)
        {
            GameManager.gm.canRollDice = false;

            numberSpriteHolder.gameObject.SetActive(false);
            diceRollAnimation.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            numberRolled = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberRolled];
            numberRolled += 1;

            GameManager.gm.numberOfStepsToMove = numberRolled;
            GameManager.gm.rollingDice = this;

            numberSpriteHolder.gameObject.SetActive(true);
            diceRollAnimation.gameObject.SetActive(false);

            yield return new WaitForEndOfFrame();

            int numberGot = GameManager.gm.numberOfStepsToMove;

            if (PlayerCanNotMove())
            {
                yield return new WaitForSeconds(0.5f);

                if (numberGot != 6)
                {
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    GameManager.gm.selfDice = true;
                }
            }
            else
            {
                if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]) { outPiece = GameManager.gm.blueOutPlayers; }
                else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]) { outPiece = GameManager.gm.redOutPlayers; }
                else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]) { outPiece = GameManager.gm.greenOutPlayers; }
                else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]) { outPiece = GameManager.gm.yellowOutPlayers; }

                if (outPiece == 0 && numberGot != 6)
                {
                    yield return new WaitForSeconds(0.5f);
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    if (outPiece == 0 && numberGot == 6)
                    {
                        // Add a delay of 0.5 seconds for pieces to come out automatically
                        yield return new WaitForSeconds(0.5f);
                        MakePlayerReadyToMove(0);
                    }
                    else if (outPiece == 1 && numberGot != 6 && GameManager.gm.canPlayerMove)
                    {
                        int playerPiecePosition = CheckOutPlayer();
                        if (playerPiecePosition >= 0)
                        {
                            GameManager.gm.canPlayerMove = false;
                            moveSteps = StartCoroutine(MoveStep_Enum(playerPiecePosition));
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);

                            if (numberGot != 6)
                            {
                                GameManager.gm.transferDice = true;
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;
                            }
                        }
                    }
                    else if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
                    {
                        if (numberGot == 6 && outPiece < 4)
                        {
                            MakePlayerReadyToMove(OutPlayerToMove());
                        }
                        else
                        {
                            int playerPiecePosition = CheckOutPlayer();
                            if (playerPiecePosition >= 0)
                            {
                                GameManager.gm.canPlayerMove = false;
                                moveSteps = StartCoroutine(MoveStep_Enum(playerPiecePosition));
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.5f);

                                if (numberGot != 6)
                                {
                                    GameManager.gm.transferDice = true;
                                }
                                else
                                {
                                    GameManager.gm.selfDice = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CheckOutPlayer() < 0)
                        {
                            yield return new WaitForSeconds(0.5f);

                            if (numberGot != 6)
                            {
                                GameManager.gm.transferDice = true;
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;
                            }
                        }
                    }
                }

            }

            GameManager.gm.RollingDiceManager();

            if (generateRandomDiceRoll != null)
            {
                StopCoroutine(generateRandomDiceRoll);
            }
        }
    }

    int OutPlayerToMove()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!GameManager.gm.greenPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }

    int CheckOutPlayer()
    {
        if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
        {
            currentPlayerPiece = GameManager.gm.bluePlayerPiece;
            pathPointToMoveOn = pathParent.bluePlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1])
        {
            currentPlayerPiece = GameManager.gm.redPlayerPiece;
            pathPointToMoveOn = pathParent.redPlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
        {
            currentPlayerPiece = GameManager.gm.greenPlayerPiece;
            pathPointToMoveOn = pathParent.greenPlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3])
        {
            currentPlayerPiece = GameManager.gm.yellowPlayerPiece;
            pathPointToMoveOn = pathParent.yellowPlayerPathPoints;
        }

        for (int i = 0; i < currentPlayerPiece.Length; i++)
        {
            if (currentPlayerPiece[i].isReady && IsPathPointAvailableToMove(GameManager.gm.numberOfStepsToMove, currentPlayerPiece[i].numberOfStepsAlreadyMove, pathPointToMoveOn))
            {
                return i;
            }
        }
        return -1;
    }

    public bool PlayerCanNotMove()
    {
        if (outPiece > 0)
        {
            bool canNotMove = false;
            if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                currentPlayerPiece = GameManager.gm.bluePlayerPiece;
                pathPointToMoveOn = pathParent.bluePlayerPathPoints;
            }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1])
            {
                currentPlayerPiece = GameManager.gm.redPlayerPiece;
                pathPointToMoveOn = pathParent.redPlayerPathPoints;
            }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
            {
                currentPlayerPiece = GameManager.gm.greenPlayerPiece;
                pathPointToMoveOn = pathParent.greenPlayerPathPoints;
            }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3])
            {
                currentPlayerPiece = GameManager.gm.yellowPlayerPiece;
                pathPointToMoveOn = pathParent.yellowPlayerPathPoints;
            }


            for (int i = 0; i < currentPlayerPiece.Length; i++)
            {
                if (currentPlayerPiece[i].isReady)
                {
                    if (IsPathPointAvailableToMove(GameManager.gm.numberOfStepsToMove, currentPlayerPiece[i].numberOfStepsAlreadyMove, pathPointToMoveOn))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!canNotMove)
                    {
                        canNotMove = false;
                    }
                }
            }

            if (canNotMove)
            {
                return true;
            }
        }

        return false;
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

    public void MakePlayerReadyToMove(int outPlayer)
    {
        if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[outPlayer];
            pathPointToMoveOn = pathParent.bluePlayerPathPoints;
            GameManager.gm.blueOutPlayers += 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.redPlayerPiece[outPlayer];
            pathPointToMoveOn = pathParent.redPlayerPathPoints;
            GameManager.gm.redOutPlayers += 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[outPlayer];
            pathPointToMoveOn = pathParent.greenPlayerPathPoints;
            GameManager.gm.greenOutPlayers += 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.yellowPlayerPiece[outPlayer];
            pathPointToMoveOn = pathParent.yellowPlayerPathPoints;
            GameManager.gm.yellowOutPlayers += 1;
        }


        outPlayerPiece.isReady = true;
        outPlayerPiece.transform.position = pathPointToMoveOn[0].transform.position;
        outPlayerPiece.numberOfStepsAlreadyMove = 1;

        outPlayerPiece.previousPathPoint = pathPointToMoveOn[0];
        outPlayerPiece.currentPathPoint = pathPointToMoveOn[0];
        outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece);
        GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);

        GameManager.gm.canRollDice = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
        GameManager.gm.numberOfStepsToMove = 0;
        GameManager.gm.SelfRoll();

    }

    IEnumerator MoveStep_Enum(int movePlayer)
    {
        if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[movePlayer];
            pathPointToMoveOn = pathParent.bluePlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.redPlayerPiece[movePlayer];
            pathPointToMoveOn = pathParent.redPlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[movePlayer];
            pathPointToMoveOn = pathParent.greenPlayerPathPoints;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.yellowPlayerPiece[movePlayer];
            pathPointToMoveOn = pathParent.yellowPlayerPathPoints;
        }


        GameManager.gm.transferDice = false;
        yield return new WaitForSeconds(0.25f);
        int numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;

        outPlayerPiece.currentPathPoint.RescaleAndReositionAllPlayerPiece();

        for (int i = outPlayerPiece.numberOfStepsAlreadyMove; i < (outPlayerPiece.numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
        {
            if (IsPathPointAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointToMoveOn))
            {
                outPlayerPiece.transform.position = pathPointToMoveOn[i].transform.position;

                yield return new WaitForSeconds(0.35f);
            }
        }

        if (IsPathPointAvailableToMove(numberOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMove, pathPointToMoveOn))
        {
            outPlayerPiece.numberOfStepsAlreadyMove += numberOfStepsToMove;

            GameManager.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);

            outPlayerPiece.currentPathPoint = pathPointToMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];

            if (outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece))
            {
                if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
                {
                    GameManager.gm.selfDice = true;
                }
                else
                {
                    if (GameManager.gm.numberOfStepsToMove != 6)
                    {
                        GameManager.gm.transferDice = true;
                    }
                    else
                    {
                        GameManager.gm.selfDice = true;
                    }
                }
            }
            else
            {
                GameManager.gm.selfDice = true;
            }

            GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.currentPathPoint;



            GameManager.gm.numberOfStepsToMove = 0;
        }

        GameManager.gm.canPlayerMove = true;
        GameManager.gm.RollingDiceManager();

        moveSteps = null;
    }
}
