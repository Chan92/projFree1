using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour{
    [Header("Movement")]
    public Transform movePointList;
    public float moveSpeed = 10;
    private List<Transform> movePoints = new List<Transform>();
    private Transform curPoint;
    private int pid = 0;

    [Header("PlayerDetection")]
    public float detectDistance = 5;
    public float detectAngle = 45;
    private RaycastHit hit;
	private Transform player;

    void Awake(){
        MovePoints();
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

    void Update(){
        if (movePoints != null){
            Movement();
        }
    }

    //moves the enemy forwards to given location point
    void Movement(){
        print(DetectionCheck());

		//if player is detected, go after the player else return to path
		if(DetectionCheck()) {

		} else {
			if(Vector3.Distance(transform.position, curPoint.position) <= 2) {
				PatrolDirection(PointId());
			}
		}

        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }
    
    //get all patrol points and put them in a list for easy access
    void MovePoints(){
        foreach (Transform point in movePointList){
            movePoints.Add(point);
        }
        PatrolDirection(0);
    }

    //get a direction point the enemy has to move to
    void PatrolDirection(int id){
        curPoint = movePoints[id];
        transform.LookAt(curPoint);
    }

    //update the point id
    int PointId() {
        pid++;
        
        if (pid >= movePoints.Count){
            pid = 0;
        }
        
        return pid;
    }

	//detect if the player is sight
    bool DetectionCheck(){
		Vector3 rayDir = player.position - transform.position;

		if (Physics.Raycast(transform.position, rayDir.normalized, out hit, detectDistance)) {
			Debug.DrawRay(transform.position, rayDir.normalized * detectDistance, Color.red);

			if(hit.transform.tag == "Player") {
				float angle = Mathf.Abs(Vector3.Angle(transform.forward, player.position - transform.position));				
				if (angle < detectAngle) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		} else {
			Debug.DrawRay(transform.position, rayDir.normalized * detectDistance, Color.white);
			return false;
		}

    }
}
