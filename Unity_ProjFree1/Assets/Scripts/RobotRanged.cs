using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRanged : EnemyControl{
    void Update(){
		if(!death && !Invisibility()) {
			Movement();
			Offense();
		}
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

	//check if a box is thrown on the enemy
	private void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Box") {
			Death();
		}
	}
}
