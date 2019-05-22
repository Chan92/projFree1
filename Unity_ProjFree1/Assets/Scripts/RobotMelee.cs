using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMelee : EnemyControl{

    void Start(){
        
    }

    void Update(){
		Movement();
		//Attack();
	}

	protected override void Attack() {
		if(!DetectionCheck()) {
			print("MeleeRobo_: No player detected");
			return;
		}
		print("MeleeRobo_: Player found");
	}
}
