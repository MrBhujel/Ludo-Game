using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public PathPointParent pathPointParent;
    public List<PlayerPiece> playerPieceList = new List<PlayerPiece>();

    void Start()
    {
        pathPointParent = GetComponentInParent<PathPointParent>();
    }

    public void AddPlayerPiece(PlayerPiece playerPiece)
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
