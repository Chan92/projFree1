using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRanged : EnemyControl{
    void Update(){
		Movement();
		Offense();
	}

	//attacks the player
	protected override IEnumerator Attack() {
		yield return new WaitForSeconds(0);
		base.Attack();
		//yield return new WaitForSeconds(hitDelay);
		//offenseCoroutine = null;
	}
}
