using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public RollingDice rollingDice;

    public int numberOfStepsToMove;
    public bool canPlayerMove = true;

    List<PathPoint> playerOnPathPointList = new List<PathPoint>();

    public bool canRollDice = true;
    public bool transferDice = false;
    public bool selfDice = false;

    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;

    public int blueCompletePlayers;
    public int redCompletePlayers;
    public int greenCompletePlayers;
    public int yellowCompletePlayers;

    public RollingDice[] manageRollingDice;

    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;
    public PlayerPiece[] yellowPlayerPiece;

    public int totalPlayerCanPlay;

    void Awake()
    {
        gm = this;
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed!!!");
        }
    }

    public void RollingDiceManager()
    {
        if (GameManager.gm.transferDice)
        {
            if (GameManager.gm.numberOfStepsToMove != 6)
            {
                ShiftDice();
            }
            GameManager.gm.canRollDice = true;
        }
        else
        {
            if (GameManager.gm.selfDice)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.canRollDice = true;
            }
        }

        
    }

    void ShiftDice()
    {
        int nextDice;
        if (GameManager.gm.totalPlayerCanPlay == 1)
        {

        }
        else if (GameManager.gm.totalPlayerCanPlay == 2)
        {
            if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 2) { nextDice = 0; } else { nextDice = i + 1; }
                if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3) { nextDice = 0; } else { nextDice = i + 1; }
                if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
    }
}
