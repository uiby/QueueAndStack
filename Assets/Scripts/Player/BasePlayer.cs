using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasePlayer : MonoBehaviour {
    [SerializeField] Owner onwer;
    [SerializeField] HandArea handArea;
    [SerializeField] SubmitArea submitArea;
    [SerializeField] protected List<Block> handBlocks; //手札
    public DataStruct dataStruct{get; private set;}
    int remainSubmitCount = 0;
    int remainRecallCount = 0;
    public int recallCount{get; private set;}
    public int winCount{get; private set;} //勝利数

    //ゲーム単位のリセット
    public void Initialize() {
        Reset();
        winCount = 0;
    }

    //セット単位のリセット
    public void Reset() {
        for (int n = 0; n < handBlocks.Count; n++) {
            handBlocks[n].Initialize(onwer);
            handBlocks[n].ChangeState(BlockState.HAND);
        }
        handArea.Initialize(handBlocks);
        submitArea.Reset();
        remainSubmitCount = 0;
        remainRecallCount = 0;
        recallCount = 0;
    }

    //ターン単位のリセット
    public void InitState() {
        remainSubmitCount = 1;
        remainRecallCount = 1;
    }

    public bool FinishTurn() {
        return remainSubmitCount == 0;
    }

    public void Win() {
        winCount++;
    }

    public List<Block> GetSubmitBlocks() {
        return submitArea.GetList(dataStruct);
    }

    public List<Block> GetMyHandBlocks() {
        return handBlocks.Where(item => item.blockState == BlockState.HAND).ToList();
    }

    public List<int> GetValueByBlocksList(List<Block> blockList) {
        return blockList.Select(item => item.GetValue()).ToList();
    }

    public void WinBattle(int n) {
        submitArea.ShowCrown(n);
    }

    //---プレイヤーの行動---//
    public void DecideDataStruct(DataStruct _dataStruct) {
        dataStruct = _dataStruct;
    }

    public IEnumerator TurnLoop() {
        yield return null;
        while (!FinishTurn()) {
            yield return null;
        }
    }

    //場に出す
    protected void Submit(Block block) {
        if (!submitArea.CanSubmit()) return;
        if (remainSubmitCount == 0) return;
        Debug.Log(onwer+" : SUBMIT "+block.GetValue());
        remainSubmitCount--;
        submitArea.Submit(block, dataStruct);

        ChangeState(BlockState.SUBMIT, block.GetValue()); //

        handArea.Refresh(handBlocks);
    }

    //場から回収
    protected void Recall() {
        if (!submitArea.CanRecall()) return;
        if (remainRecallCount == 0 || remainSubmitCount == 0) return;
        recallCount++;
        remainRecallCount--;

        var block = submitArea.Recall(dataStruct);
        Debug.Log(onwer+" : RECALL "+ block.GetValue());

        ChangeState(BlockState.HAND, block.GetValue());
        handArea.AddToHand(handBlocks);
        if (dataStruct == DataStruct.QUEUE)
            submitArea.Refrash(dataStruct);
    }

    protected bool CanRecall() {
        return submitArea.CanRecall() && (remainRecallCount > 0 && remainSubmitCount > 0);
    }

    protected void ForceFinish() {
        remainSubmitCount--;
    }

    void ChangeState(BlockState changeState, int value) {
        for (int n = 0; n < handBlocks.Count; n++) {
            if (handBlocks[n].GetValue() == value) {
                handBlocks[n].ChangeState(changeState);
                return;
            }
        }
    }

}
