using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSubmitMethod : MonoBehaviour {
    [SerializeField] GameSystem gameSystem;
    Image background;
    RectTransform[] children;
	// Use this for initialization
	void Awake () {
        background = GetComponent<Image>();
		children = GetComponentsInChildren<RectTransform>();
	}

    public void Hide() {
        for (int n = 0; n < children.Length; n++)
            children[n].gameObject.SetActive(false);
        background.enabled = false;
    }

    public void Show() {
        for (int n = 0; n < children.Length; n++)
            children[n].gameObject.SetActive(true);
        background.enabled = true;
    }

    //dataStruct 0:QUEUE, 1:STACK
    public void OnSelectSubmitMethod(int dataStruct) {
        gameSystem.SelectSubmitMethod((DataStruct)dataStruct);
    }
}
