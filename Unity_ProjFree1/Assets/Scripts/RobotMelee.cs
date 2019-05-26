using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMelee : EnemyControl{
    void Update(){
		Movement();
		Offense();
	}

	//attacks the player
	protected override IEnumerator Attack() {
		yield return new WaitForSeconds(0);

		if (soundObj && attackSound)
			soundObj.PlayOneShot(attackSound);
		if (anim)
			anim.SetTrigger("Attack");

		player.GetComponent<Health>().GetDmg(hitDamage);

		print("attack");
		yield return new WaitForSeconds(hitDelay);
		offenseCoroutine = null;
	}
}
