using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathPointToMoveOn;
    public PathPointParent pathPointParent;
    public List<PlayerPiece> playerPieceList = new List<PlayerPiece>();

    void Start()
    {
        pathPointParent = GetComponentInParent<PathPointParent>();
    }

    public bool AddPlayerPiece(PlayerPiece playerPiece)
    {
        if (this.name == "Center Path") { Completed(playerPiece); }
        if (this.name != "PathPoint" && this.name != "PathPoint (8)" && this.name != "PathPoint (13)" && this.name != "PathPoint (21)" && this.name != "PathPoint (26)" && this.name != "PathPoint (34)" && this.name != "PathPoint (39)" && this.name != "PathPoint (47)" && this.name != "Center Path")
        {
            if (playerPieceList.Count == 1)
            {
                string prePlayerPieceName = playerPieceList[0].name;
                string CurrPlayerPieceName = playerPiece.name;
                CurrPlayerPieceName = CurrPlayerPieceName.Substring(0, CurrPlayerPieceName.Length - 4);

                if (!prePlayerPieceName.Contains(CurrPlayerPieceName))
                {
                    playerPieceList[0].isReady = false;

                    StartCoroutine(RevertOnStart(playerPieceList[0]));

                    playerPieceList[0].numberOfStepsAlreadyMove = 0;
                    RemovePlayerPiece(playerPieceList[0]);
                    playerPieceList.Add(playerPiece);

                    return false;
                }
            }
        }
        AddPlayer(playerPiece);
        return true;
    }

    IEnumerator RevertOnStart(PlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManager.gm.blueOutPlayers -= 1;
            pathPointToMoveOn = pathPointParent.bluePlayerPathPoints;
        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManager.gm.redOutPlayers -= 1;
            pathPointToMoveOn = pathPointParent.redPlayerPathPoints;
        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManager.gm.greenOutPlayers -= 1;
            pathPointToMoveOn = pathPointParent.greenPlayerPathPoints;
        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManager.gm.yellowOutPlayers -= 1;
            pathPointToMoveOn = pathPointParent.yellowPlayerPathPoints;
        }

        for (int i = playerPiece.numberOfStepsAlreadyMove - 1; i >= 0; i--)
        {
            playerPiece.transform.position = pathPointToMoveOn[i].transform.position;
            yield return new WaitForSeconds(0.02f);
        }

        playerPiece.transform.position = pathPointParent.basePoints[BasePointPosition(playerPiece.name)].transform.position;
    }

    int BasePointPosition(string name)
    {
        for (int i = 0; i < pathPointParent.basePoints.Length; i++)
        {
            if (pathPointParent.basePoints[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void AddPlayer(PlayerPiece playerPiece)
    {
        playerPieceList.Add(playerPiece);
        RescaleAndReositionAllPlayerPiece();
    }

    public void RemovePlayerPiece(PlayerPiece playerPiece)
    {
        if (playerPieceList.Contains(playerPiece))
        {
            playerPieceList.Remove(playerPiece);
            RescaleAndReositionAllPlayerPiece();
        }
    }

    private void Completed(PlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue"))
        {
            GameManager.gm.blueCompletePlayers += 1;
            GameManager.gm.blueOutPlayers -= 1;
            
            if (GameManager.gm.blueCompletePlayers == 4)
            {
                ShowCelebration();
            }
        }
        else if (playerPiece.name.Contains("Red"))
        {
            GameManager.gm.redCompletePlayers += 1;
            GameManager.gm.redOutPlayers -= 1;

            if (GameManager.gm.redCompletePlayers == 4)
            {
                ShowCelebration();
            }
        }
        else if (playerPiece.name.Contains("Green"))
        {
            GameManager.gm.greenCompletePlayers += 1;
            GameManager.gm.greenOutPlayers -= 1;
            
            if (GameManager.gm.greenCompletePlayers == 4)
            {
                ShowCelebration();
            }
        }
        else if (playerPiece.name.Contains("Yellow"))
        {
            GameManager.gm.yellowCompletePlayers += 1;
            GameManager.gm.yellowOutPlayers -= 1;
            
            if (GameManager.gm.yellowCompletePlayers == 4)
            {
                ShowCelebration();
            }
        }
    }

    void ShowCelebration()
    {

    }

    public void RescaleAndReositionAllPlayerPiece()
    {
        int plsCount = playerPieceList.Count;
        bool isOdd = (plsCount % 2) == 0 ? false : true;

        int extent = plsCount / 2;
        int counter = 0;
        int spriteLayer = 0;

        if (isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                playerPieceList[counter].transform.localScale = new Vector3(pathPointParent.scales[plsCount - 1], pathPointParent.scales[plsCount - 1], 1f);

                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathPointParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        else
        {
            for (int i = -extent; i < extent; i++)
            {
                playerPieceList[counter].transform.localScale = new Vector3(pathPointParent.scales[plsCount - 1], pathPointParent.scales[plsCount - 1], 1f);

                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathPointParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < playerPieceList.Count; i++)
        {
            playerPieceList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
}
