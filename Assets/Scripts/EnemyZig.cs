using UnityEngine;
using System.Collections;

public class EnemyZig : enemyAction {

	// Use this for initialization
	public override void Move(){
		Vector3 tempPos = pos;
		tempPos.x = Mathf.Sin (Time.time * Mathf.PI ) * 4;
		//print (Time.time * Mathf.PI * 2);
		pos = tempPos;
		base.Move ();
	}
}
