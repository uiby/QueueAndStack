using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour {
    [SerializeField] bool isPlayer;
    [SerializeField] HandArea handArea;
    [SerializeField] List<Block> handBlocks; //手札
    DataStruct dataStruct;
    Queue<Block> queue = new Queue<Block>();
    Stack<Block> stack = new Stack<Block>();
    int remainSubmitCount = 1;
    int remainRecallCount = 1;

    public void Initialize() {
        while (0 < queue.Count) {
            handBlocks.Add(queue.Dequeue());
        }
        while (0 < stack.Count) {
            handBlocks.Add(stack.Pop());
        }

        handArea.Initialize(handBlocks);
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
    public bool CanRecall() {
        var submitedCardCount = 0;
        if (IsQueue()) {
            submitedCardCount = queue.Count;
        }
        else {
            submitedCardCount = stack.Count;
        }

        return submitedCardCount != 0;
    }

    //場に出す
    protected void OnSubmit(Block n) {
        remainSubmitCount--;
        if (IsQueue()) {
            queue.Enqueue(n);
        }
        else {
            stack.Push(n);
        }
        handBlocks.Remove(n);
    }

    //場から回収
    protected void OnRecall() {
        remainRecallCount--;
        if (IsQueue()) {
            handBlocks.Add(queue.Dequeue());
        } 
        else {
            handBlocks.Add(stack.Pop());
        }
    }

    bool IsQueue() {
        return dataStruct == DataStruct.QUEUE;
    }

}
