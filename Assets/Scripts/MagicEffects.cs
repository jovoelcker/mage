using UnityEngine;
using System.Collections;

public abstract class MagicEffects : MonoBehaviour {

	// Every object effected by magic has to implement these methods
	abstract public void Attack();
	abstract public void Defend();

}
