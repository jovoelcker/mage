using UnityEngine;
using System.Collections;

public class MagicEffectsFence : MagicEffects {

	public override void Attack() {
		if (attackable) {
			attackable = false;
		}
	}
	
	public override void Defend() { }

}
