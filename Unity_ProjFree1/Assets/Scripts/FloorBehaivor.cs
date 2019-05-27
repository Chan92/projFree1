using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaivor : MonoBehaviour{
	public bool setTrans = false;
	public Material normalMat;
	public Material transMat;

	private Transform player;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

    void Update(){
		if (setTrans) {
			MaterialSwap();
		} else {
			EnableSwap();
		}
	}

	//makes the object slightly transparent when its above the player level
	void MaterialSwap() {
		if (player.position.y < transform.position.y) {
			if (transMat != null)
				transform.GetComponent<Renderer>().material = transMat;
		} else {
			if (normalMat != null)
				transform.GetComponent<Renderer>().material = normalMat;
		}
	}

	//makes the object invisible when its above the player level
	void EnableSwap() {
		if (player.position.y < transform.position.y) {
			transform.GetComponent<MeshRenderer>().enabled = false;
		} else {
			transform.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
