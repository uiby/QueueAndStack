using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubmitArea : MonoBehaviour {
    [SerializeField, Range(5, 6)] int maxBlockCount = 6;
    [SerializeField] List<Image> crownList;
    [SerializeField] Text label;
    int nowBlockCount = 0;
    Queue<Block> queue = new Queue<Block>();
    Stack<Block> stack = new Stack<Block>();

    //セット単位のリセット
    public void Reset() {
        queue = new Queue<Block>();
        stack = new Stack<Block>();
        nowBlockCount = 0;
        label.text = "";
        for (int n = 0; n < crownList.Count; n++)
            crownList[n].enabled = false;
    }

    public void SetLabel(DataStruct dataStruct) {
        if (dataStruct == DataStruct.STACK)
            label.text = "Stack";
        else label.text = "Queue";
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

    public List<Block> GetList(DataStruct dataStruct) {
        if (dataStruct == DataStruct.QUEUE) {
            return queue.ToList();
        }
        var stackList = stack.ToList();
        stackList.Reverse();
        return stackList;
    }

    public void ShowCrown(int n) {
        crownList[n].enabled = true;
    }
}
