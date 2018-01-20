using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    [SerializeField] SelectSubmitMethod selectSubmitCanvas;
    [SerializeField] ResultCanvas resultCanvas;
    [SerializeField] Player player;
    [SerializeField] Com com;

    Owner setLoser;
    Owner playerTurn;
    bool selected;
    int  turnCount;
    int setCount;

	// Use this for initialization
	void Start () {
        setLoser = Owner.PLAYER; //最初はプレイヤーが選べる
		player.Initialize();
        com.Initialize();
        resultCanvas.Initialize();
        setCount = 1; //1セット目から

        StartCoroutine(GameLoop());
	}

    void Reset() {
        selected = false;
        turnCount = 0;
    }

    public void SelectSubmitMethod(DataStruct dataStruct) {
        var opData = dataStruct == DataStruct.QUEUE ? DataStruct.STACK : DataStruct.QUEUE;
        //データ構造の決定
        if (setLoser == Owner.PLAYER) {
            player.DecideDataStruct(dataStruct);
            com.DecideDataStruct(opData);
        } else {
            com.DecideDataStruct(dataStruct);
            player.DecideDataStruct(opData);
        }
        selected = true;
    }

    void DeceideFirstTurnPlayer() {
        playerTurn = player.dataStruct == DataStruct.QUEUE ? Owner.PLAYER : Owner.COM;
    }

    IEnumerator GameLoop() {
        Reset();
        resultCanvas.HideResultText();
        //各プレイヤーの情報のリセット
        player.Reset();
        com.Reset();
        yield return null;

        selectSubmitCanvas.Show(setLoser, setCount);
        if (setLoser == Owner.COM) StartCoroutine(com.SelectDataStruct()); //COMの場合
        //データ構造の決定
        while (selected == false) {
            yield return null;
        }
        selectSubmitCanvas.Hide();

        //先行後行の決定
        DeceideFirstTurnPlayer();
        yield return null;

        //----ゲーム開始----//
        //8回行う
          //先行ターン
          //後行ターン
        while (turnCount < 16) {
            Debug.Log("turn:"+turnCount+" player:"+ playerTurn);
            if (playerTurn == Owner.PLAYER) {
                player.InitState();
                yield return StartCoroutine(player.TurnLoop());
            } else {
                com.InitState();
                yield return StartCoroutine(com.TurnAction());
                yield return StartCoroutine(com.TurnLoop());                
            }
            turnCount++;
            playerTurn = playerTurn == Owner.PLAYER ? Owner.COM : Owner.PLAYER;
        }

        Debug.Log("finish turn");
        //勝敗
        yield return StartCoroutine(ShowResult(player.GetSubmitBlocks(), com.GetSubmitBlocks()));

        //if 2勝した場合、ゲーム終了
        if (player.winCount == 2) {
            resultCanvas.ShowGameResult("YOU WIN!");
            yield break;
        } else if (com.winCount == 2) {
            resultCanvas.ShowGameResult("YOU LOSE...");
            yield break;
        } else if (setCount == 3) {
            resultCanvas.ShowGameResult("DRAW");
            yield break;
        }

        setCount++;
        yield return new WaitForSeconds(3f);
        StartCoroutine(GameLoop());
    }

    IEnumerator ShowResult(List<Block> playerBlocks, List<Block> comBlocks) {
        var playerWinCount = 0;
        var comWinCount = 0;
        var battleCount = playerBlocks.Count;
        for (int n = 0; n < battleCount; n++) {
            var playerValue = playerBlocks[n].GetValue();
            var comValue = comBlocks[n].GetValue();

            if ((playerValue == 1 && comValue == 6) || (playerValue > comValue)) { //playerの勝利
                playerWinCount++;
                player.WinBattle(n);
                comBlocks[n].Lose();
            } else if ((comValue == 1 && playerValue == 6) || (comValue > playerValue)) { //comの勝利
                comWinCount++;
                com.WinBattle(n);
                playerBlocks[n].Lose();
            }

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("result p:"+playerWinCount+" c:"+comWinCount);
        yield return null;

        if (playerWinCount > comWinCount) {
            resultCanvas.ShowCrown(Owner.PLAYER, player.winCount);
            resultCanvas.ShowResultText("YOU WIN!");
            player.Win();
            setLoser = Owner.COM; //次の選択権
        } else if (comWinCount > playerWinCount) {
            resultCanvas.ShowCrown(Owner.COM, com.winCount);
            resultCanvas.ShowResultText("COM WIN!");
            com.Win();
            setLoser = Owner.PLAYER; //次の選択権
        } else {
            resultCanvas.ShowResultText("DRAW");
            setLoser = (Owner)Random.Range(0, 1); //引き分けの場合、ランダム
        }
    }
}
