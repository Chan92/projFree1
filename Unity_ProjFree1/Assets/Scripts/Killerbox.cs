using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killerbox : MonoBehaviour{
	public float startDelay = 2;
	public float destroyDelay = 0.2f;
	public float respawnDelay = 2f;
	public bool respawn = true;
	private Coroutine respawnroutine;
	private Vector3 startPos;
	private MeshRenderer mr;

	//get the start height after a short wait
	private IEnumerator Start() {
		yield return new WaitForSeconds(startDelay);
		startPos = transform.position;
		mr = transform.GetComponent<MeshRenderer>();
	}

	//destroy the box when it hit something after it fell
	private void OnCollisionEnter(Collision collision) {
		if (transform.position.y < startPos.y) {
			if(respawn && respawnroutine == null) {
				respawnroutine = StartCoroutine(Respawn());
			} else {
				Destroy(gameObject, destroyDelay);
			}
		}
	}

	IEnumerator Respawn() {
		mr.enabled = false;
		yield return new WaitForSeconds(respawnDelay);
		transform.position = startPos;
		mr.enabled = true;	
		respawnroutine = null;
	}
}
