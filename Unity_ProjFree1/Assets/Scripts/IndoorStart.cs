using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorStart : MonoBehaviour{
	public float viewRotateSpeed = 2f;
	public float returnSpeed = 5f;
	public float viewRotateTime = 2f;

	public Vector3 rotateTarget = new Vector3(-15, 145, 0);
	public Vector3 posTarget = new Vector3(0, 5, 11.7f);

	private Vector3 startCamRot;
	private Vector3 startCamPos;

	private FloorLayers[] fls;
	private Rigidbody[] rbs;

	private void Awake() {
		fls = GameObject.FindObjectsOfType<FloorLayers>();
		rbs = GameObject.FindObjectsOfType<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		if(other.transform.root.tag == "Player") {
			StartCoroutine(CameraHint(other.transform.root));
		}
	}

	IEnumerator CameraHint(Transform player) {
		Time.timeScale = 0f;
		bool changeview = true;
		bool changeBack = true;
		Transform cam = player.Find("Main Camera");
		startCamRot = cam.localEulerAngles;
		startCamPos = cam.localPosition;


		for(int j = 0; j < fls.Length; j++) {
			fls[j].setTransparent = false;
		}

		
		while(changeview) {
			cam.localEulerAngles = Vector3.Slerp(cam.localEulerAngles, rotateTarget, viewRotateSpeed * Time.unscaledDeltaTime);
			yield return new WaitForSecondsRealtime(0);

			if(Vector3.Distance(cam.localEulerAngles, rotateTarget) <= 1f) {
				//if (cam.localEulerAngles == rotateTarget) {
				cam.localEulerAngles = rotateTarget;
				yield return new WaitForSecondsRealtime(viewRotateTime);
				changeview = false;

			}
		}

		yield return new WaitForSecondsRealtime(0);
		

		/*
		float elapsedTime = 0;

		while(elapsedTime < viewRotateSpeed) {
			cam.localEulerAngles = Vector3.Slerp(cam.localEulerAngles, rotateTarget, (elapsedTime / viewRotateSpeed));
			elapsedTime += Time.unscaledDeltaTime;
			yield return new WaitForSecondsRealtime(0);

			if (elapsedTime >= viewRotateSpeed) {
				yield return new WaitForSecondsRealtime(viewRotateTime);
				changeview = false;
			}
		}
		*/


		/*
		while(changeBack) {
			cam.localEulerAngles = Vector3.Slerp(cam.localEulerAngles, startCamRot, returnSpeed * Time.unscaledDeltaTime);
			yield return new WaitForSecondsRealtime(0);

			if(cam.localEulerAngles == rotateTarget) {
				yield return new WaitForSecondsRealtime(viewRotateTime);
				changeBack = false;
			}
		}
		*/

		yield return new WaitForSecondsRealtime(0);


		print("done");

		for(int j = 0; j < rbs.Length; j++) {
			fls[j].setTransparent = true;
		}
		Time.timeScale = 1f;
	}
}
