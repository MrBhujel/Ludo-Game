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
    public int totalSix = 0;

    public AudioSource ads;

    void Awake()
    {
        gm = this;
        ads = GetComponent<AudioSource>();
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
                GameManager.gm.SelfRoll();
            }
        }
    }

    public void SelfRoll()
    {
        if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2])
        {
            //Invoke function is used to delay, passing function as string and then the time for delay is passed
            Invoke("Rolled", 0.6f);
        }
    }

    void Rolled()
    {
        GameManager.gm.manageRollingDice[2].MouseRoll();
    }

    void ShiftDice()
    {
        int nextDice;
        if (GameManager.gm.totalPlayerCanPlay == 1)
        {
            if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);

                PassOut(0);
                GameManager.gm.manageRollingDice[2].MouseRoll();
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);

                PassOut(2);
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 2)
        {
            if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);

                PassOut(0);
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);

                PassOut(2);
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 2) { nextDice = 0; } else { nextDice = i + 1; }
                i = PassOut(i);
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
                i = PassOut(i);
                if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
    }

    int PassOut(int i)
    {
        if (i == 0)
        {
            if (GameManager.gm.blueCompletePlayers == 4)
            {
                return i + 1;
            }
        }
        else if (i == 1)
        {
            if (GameManager.gm.blueCompletePlayers == 4)
            {
                return i + 1;
            }
        }
        else if (i == 2)
        {
            if (GameManager.gm.blueCompletePlayers == 4)
            {
                return i + 1;
            }
        }
        else if (i == 3)
        {
            if (GameManager.gm.blueCompletePlayers == 4)
            {
                return i + 1;
            }
        }
        return i;
    }
}
