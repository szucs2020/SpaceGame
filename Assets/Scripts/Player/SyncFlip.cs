using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncFlip : NetworkBehaviour {

	[SyncVar(hook="FlipHook")]
	private bool facingRight = false;

	[Command]
	public void CmdSyncFlip(bool direction){

		facingRight = direction;
		Vector3 temp = transform.localScale;
		if (facingRight){
			temp.x = -1;
		} else{
			temp.x = 1;
		}
		transform.localScale = temp;
	}

	void FlipHook(bool direction){
		
		facingRight = direction;
		Vector3 temp = transform.localScale;
		if (facingRight){
			temp.x = -1;
		} else {
			temp.x = 1;
		}
		transform.localScale = temp;
	}
}