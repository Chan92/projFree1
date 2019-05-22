using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRanged : EnemyControl{

	void Start(){
        
    }

    void Update(){
		Movement();
		Attack();
	}

	protected override void Attack() {
		//print("RangeRobo_:Test?");
		base.Attack();
		//print("RangeRobo_: Attack player");
	}
}
