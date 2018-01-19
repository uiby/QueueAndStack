﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitArea : MonoBehaviour {
    [SerializeField, Range(5, 6)] int maxBlockCount = 6;
    int nowBlockCount = 0;
    Queue<Block> queue = new Queue<Block>();
    Stack<Block> stack = new Stack<Block>();

    public void Reset() {
        queue = new Queue<Block>();
        stack = new Stack<Block>();
        nowBlockCount = 0;
    }

    public void Submit(Block block, DataStruct dataStruct) {
        var x = block.width;
        var y = block.height;
        var pos = new Vector2(0, -100f + y * nowBlockCount);
        block.ChangeState(BlockState.SUBMIT);
        block.TransParent(transform);
        block.MovePosition(pos);

        nowBlockCount++;
        if (IsQueue(dataStruct)) {
            queue.Enqueue(block);
        } 
        else {
            stack.Push(block);
        }
    }

    //場から回収
    public Block Recall(DataStruct dataStruct) {
        nowBlockCount--;
        if (IsQueue(dataStruct)) {
            return queue.Dequeue();
        }

        return stack.Pop();
    }

    public void Refrash(DataStruct dataStruct) {
        if (nowBlockCount == 0) return;
        var queueList = queue.ToArray();
        var x = queueList[0].width;
        var y = queueList[0].height;
        var pos = new Vector2(0, -100f);
        for (int n = 0; n < queueList.Length; n++) {
            if (queueList[n].blockState != BlockState.SUBMIT) continue;
            queueList[n].TransParent(transform);
            queueList[n].MovePosition(pos);
            pos.y += y;
        }
    }

    public bool CanSubmit() {
        return nowBlockCount < maxBlockCount;
    }

    public bool CanRecall() {
        return nowBlockCount > 0;
    }

    bool IsQueue(DataStruct data) {
        return data == DataStruct.QUEUE;
    }

    public List<Block> ReleaseAll() {
        var blocks = new List<Block>();
        while (0 < queue.Count) {
            blocks.Add(queue.Dequeue());
        }
        while (0 < stack.Count) {
            blocks.Add(stack.Pop());
        }
        return blocks;
    }
}