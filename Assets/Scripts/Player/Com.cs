using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Com : BasePlayer {
    [SerializeField] TurnText turnText; //ターン数を見るために使用
    [SerializeField] GameSystem gameSystem;
    [SerializeField] Player player; //プレイヤーの情報
    Block submitBlock;
    bool canSubmit;

    List<int> myHandList;
    List<int> mySubmitList;
    List<int> opHandList;
    List<int> opSubmitList;

    public IEnumerator TurnAction() {
        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
        yield return StartCoroutine(Think());
        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));

        if (!canSubmit) {
            ForceFinish();
            yield break;
        }
        Action();
    }

    public IEnumerator SelectDataStruct() {
        yield return new WaitForSeconds(2f);
        gameSystem.SelectSubmitMethod((DataStruct)Random.Range(0, 1));
    }

    IEnumerator Think() {
        canSubmit = true;
        //自分の手札を把握
        var myHandBlocks = GetMyHandBlocks();
        //敵の手札を把握
        var opHandBlocks = player.GetMyHandBlocks();
        yield return null;
        if (myHandBlocks.Count == 0 && recallCount >= 2) { //手札がないand取り出しできない場合、強制終了
            canSubmit = false;
            yield break;
        } else if (myHandBlocks.Count == 0 && recallCount < 2) {
            Recall();
            myHandBlocks = GetMyHandBlocks();
        }

        var index = 0;

        //自分の手札と場を把握
        UpdateMyInfo();
        //敵の手札と場を把握
        opHandList = GetValueByBlocksList(opHandBlocks);
        opSubmitList = GetValueByBlocksList(player.GetSubmitBlocks());

        if (dataStruct == DataStruct.STACK) {
            var result = CaptureQueue(mySubmitList, opSubmitList, myHandList, opHandList);
            index = myHandBlocks.FindIndex(item => item.GetValue() == result);
        } else {
            var result = CaptureStack(mySubmitList, opSubmitList, myHandList, opHandList);
            index = myHandBlocks.FindIndex(item => item.GetValue() == result);
        }

        submitBlock = myHandBlocks[index];
    }

    void Action() {
        Submit(submitBlock);
    }

    //敵がキューだった場合の攻略方法 (自分はスタック)
    //必須条件　自分の手札がある
    //必要な情報　現在の提出状況(お互い)
    //お互いの手札
    //お互いの残り取り出し回数
    //
    int CaptureQueue(List<int> _mySubmitList, List<int> _opSubmitList, List<int> _myHandList, List<int> _opHandList) {
        var result = 0;
        var nowTurnCount = turnText.turnCount;
        //場の積み上げ数を比較する
        var compSubmitedBlocks = _mySubmitList.Count - _opSubmitList.Count; //積み上げ数 = 自分 - 相手

        if (compSubmitedBlocks > 0) { //自分の方が積みあがってる場合: 常に相手の手札の最大コストに勝てる自分の最小コストをだす
            var opHandMax = _opHandList.Max();
            _myHandList.Sort(); //昇順(後半ほど大きい)にソート
            if (_myHandList.Exists(value => value > opHandMax)) {
                result = _myHandList.Find(value => value > opHandMax);
            } else {
                result = _myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        } else if (compSubmitedBlocks < 0) { //敵の方が積みあがってる場合: 
            var nextSubmitNumber = _mySubmitList.Count;
            var opValue = _opSubmitList[nextSubmitNumber];
            _myHandList.Sort(); //昇順(後半ほど大きい)にソート
            if (_myHandList.Exists(value => value > opValue)) { //場に勝てるカードがある場合
                result = _myHandList.Find(value => value > opValue);
            } else { //勝てない場合
                result = _myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        } else {
            var nowWinCount = GetWinCount(_mySubmitList, _opSubmitList); //場の状況をみる
            if (nowWinCount > 0) {
                result = _myHandList[Random.Range(0, _myHandList.Count - 1)];
            } else if (nowTurnCount >= 5 && CanRecall() && recallCount < 2) {
                Recall();
                UpdateMyInfo();
                result = CaptureQueue(mySubmitList, _opSubmitList, myHandList, _opHandList);
            } else {
                result = _myHandList[Random.Range(0, _myHandList.Count - 1)];
            }

        }

        return result;
    }

    //敵がスタックだった場合の攻略方法 (自分はキュー)
    //必須条件　自分の手札がある
    int CaptureStack(List<int> _mySubmitList, List<int> _opSubmitList, List<int> _myHandList, List<int> _opHandList) {
        var result = 0;
        var nowTurnCount = turnText.turnCount;
        //場の積み上げ数を比較する
        var submitedBlocksMany = _mySubmitList.Count >= _opSubmitList.Count ? Owner.COM : Owner.PLAYER; //積み上げ数が自分の方が多いならCOM

        if (submitedBlocksMany == Owner.COM) { //自分の方が積みあがってる場合: 場の状況を見て相手の方が強い場合は取り出す
            var opHandMax = _opHandList.Max(); //相手の手札の最大と算出
            _myHandList.Sort(); //昇順(後半ほど大きい)にソート
            var nowWinCount = GetWinCount(_mySubmitList, _opSubmitList); //場の状況をみる
            if (nowWinCount > 0) {
                if (_myHandList.Exists(value => value > opHandMax)) {
                    result = _myHandList.Find(value => value > opHandMax);
                } else {
                    result = _myHandList.Min(); //敵に勝てない場合、最小値を積む
                }
            } else if (nowTurnCount >= 5 && CanRecall() && recallCount < 2) {
                Recall();
                UpdateMyInfo();
                result = CaptureStack(mySubmitList, _opSubmitList, myHandList, _opHandList);
            } else {
                result = _myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        } else { //敵の方が積みあがってる場合:
            var nextSubmitNumber = _mySubmitList.Count;
            var opValue = _opSubmitList[nextSubmitNumber];
            _myHandList.Sort(); //昇順(後半ほど大きい)にソート
            if (_myHandList.Exists(value => value > opValue)) { //場に勝てるカードがある場合
                result = _myHandList.Find(value => value > opValue);
            } else { //勝てない場合
                result = _myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        }

        return result;
    }

    int GetWinCount(List<int> mySubmitList, List<int> opSubmitList) {
        var myWinCount = 0;
        var opWinCount = 0;
        var battleCount = 6;
        for (int n = 0; n < battleCount; n++) {
            var myValue = n < mySubmitList.Count ? mySubmitList[n] : 0;
            var opValue = n < opSubmitList.Count ? opSubmitList[n] : 0;

            if ((myValue == 1 && opValue == 6) || (myValue > opValue)) { //myの勝利
                myWinCount++;
            } else if ((opValue == 1 && myValue == 6) || (opValue > myValue)) { //opの勝利
                opWinCount++;
            }
        }

        return myWinCount - opWinCount;
    }

    //自分の手札と場の状況を更新する
    void UpdateMyInfo() {
        myHandList = GetValueByBlocksList(GetMyHandBlocks());
        mySubmitList = GetValueByBlocksList(GetSubmitBlocks());
    }
}
