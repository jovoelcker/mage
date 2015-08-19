using UnityEngine;
using System.Collections;

public abstract class MagicEffects : MonoBehaviour {

	public GameLogic gameLogic;

	// Flags which allow to decline actions
	protected bool attackable = true;
	protected bool defendable = true;

	// Every object effected by magic has to implement these methods
	abstract public void Attack();
	abstract public void Defend();
	
	protected void ReleaseAttack() {
		attackable = true;
	}
	
	protected void ReleaseDefend() {
		defendable = true;
	}

}
