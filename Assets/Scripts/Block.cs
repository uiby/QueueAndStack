using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : GuiParts {
    [SerializeField, Range(1, 6)] int value = 1;
    public int GetValue() {
        return value;
    }
    public Owner owner{get; private set;}
    public BlockState blockState{get; private set;}
    Text valueText;

    protected override void Awake() {
        base.Awake();        
        valueText = GetComponentInChildren<Text>();
    }

    public void Initialize(Owner _owner) {
        owner = _owner;
        valueText.text = value.ToString();
        if (value == 6)
            valueText.text = "Joker";
        GetComponent<Image>().color = initColor;
    }

    public void ChangeState(BlockState state) {
        blockState = state;
    }

    public void Lose() {
        GetComponent<Image>().color = new Color (1, 1, 1, 1);
        valueText.text = "";
    }
}
