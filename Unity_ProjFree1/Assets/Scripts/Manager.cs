using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour{
	static public bool firstLoad = true;

    void Awake(){
		DontDestroyOnLoad(gameObject);
		if (firstLoad) {
			StartCoroutine(FirstLoad());
		}
    }

	IEnumerator FirstLoad() {
		yield return new WaitForSeconds(0);
		firstLoad = false;
	}
}
