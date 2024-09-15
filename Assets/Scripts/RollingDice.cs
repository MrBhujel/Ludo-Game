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


    public void OnMouseDown()
    {
        generateRandomDiceRoll = StartCoroutine(DiceRolling());
    }

    IEnumerator DiceRolling()
    {
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

            if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[0]) { outPiece = GameManager.gm.blueOutPlayers; }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[1]) { outPiece = GameManager.gm.redOutPlayers; }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[2]) { outPiece = GameManager.gm.greenOutPlayers; }
            else if (GameManager.gm.rollingDice == GameManager.gm.manageRollingDice[3]) { outPiece = GameManager.gm.yellowOutPlayers; }

            if (GameManager.gm.numberOfStepsToMove != 6 && outPiece == 0)
            {
                GameManager.gm.canRollDice = true;
                GameManager.gm.selfDice = false;
                GameManager.gm.transferDice = true;
                GameManager.gm.RollingDiceManager();
            }


            if (generateRandomDiceRoll != null)
            {
                StopCoroutine(generateRandomDiceRoll);
            }
        }
    }
}
