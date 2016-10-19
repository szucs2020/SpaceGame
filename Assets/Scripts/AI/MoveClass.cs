using UnityEngine;
using System.Collections;

public class MoveClass {

	public void Move(AIPlayer AI, Transform me, Vector3 target) {

		if (target.x < me.position.x) {
			AI.setMovementAxis (new Vector2 (-1, 1));
		} else {
			AI.setMovementAxis (new Vector2 (1, 1));
		}
	}
}
