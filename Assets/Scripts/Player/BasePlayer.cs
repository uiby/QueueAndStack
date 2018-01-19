using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour {
    [SerializeField] Owner onwer;
    [SerializeField] HandArea handArea;
    [SerializeField] SubmitArea submitArea;
    [SerializeField] List<Block> handBlocks; //手札
    [SerializeField] DataStruct dataStruct;
    int remainSubmitCount = 1;
    int remainRecallCount = 1;

    public void Initialize() {
        for (int n = 0; n < handBlocks.Count; n++) {
            handBlocks[n].Initialize(onwer);
            handBlocks[n].ChangeState(BlockState.HAND);
        }
        //handBlocks.AddRange(submitArea.ReleaseAll());
        handArea.Initialize(handBlocks);
        submitArea.Reset();
        InitState();
    }

    public void Reset() {
        Initialize();
    }

    public void InitState() {
        remainSubmitCount = 1;
        remainRecallCount = 1;
    }

    public bool FinishTurn() {
        return remainSubmitCount == 0;
    }

    //場に出す
    protected void Submit(Block block) {
        if (!submitArea.CanSubmit()) return;
        if (remainSubmitCount == 0) return;
        remainSubmitCount--;
        submitArea.Submit(block, dataStruct);

        ChangeState(BlockState.SUBMIT, block.GetValue()); //

        handArea.Refresh(handBlocks);
        //handBlocks.Remove(n);
    }

    //場から回収
    protected void Recall() {
        Debug.Log("RECALL");
        if (!submitArea.CanRecall()) return;
        if (remainRecallCount == 0) return;
        remainRecallCount--;

        var block = submitArea.Recall(dataStruct);
        Debug.Log(block.GetValue());

        ChangeState(BlockState.HAND, block.GetValue());
        //handBlocks.Add(block);
        handArea.AddToHand(handBlocks);
        if (dataStruct == DataStruct.QUEUE)
            submitArea.Refrash(dataStruct);
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
