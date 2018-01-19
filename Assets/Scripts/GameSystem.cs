using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    public BasePlayer player;
    public BasePlayer com;

	// Use this for initialization
	void Start () {
		player.Initialize();
        com.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
