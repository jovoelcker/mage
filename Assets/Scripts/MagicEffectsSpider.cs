using UnityEngine;
using System.Collections;

public class MagicEffectsSpider : MagicEffects {

	public AudioClip rootsAppear;
	public AudioClip rootsDisappear;
	public GameObject Spider;
	public GameObject SpiderRootEffect;
	
	public override void Attack() {
		if (attackable) {
			attackable = false;

			Instantiate(SpiderRootEffect, Spider.transform.position, Spider.transform.rotation);
			GameObject roots = GameObject.Find("Roots");
			roots.GetComponent<Animation>().Play("begin");
			roots.GetComponent<Animation>()["idle"].wrapMode = WrapMode.Loop;
			roots.GetComponent<Animation>().PlayQueued("idle");
			Spider.GetComponent<SpiderAnimation>().Rooted = true;
			Invoke("endRootEffectSpider", 10);

			gameLogic.playSoundEffect(rootsAppear);
		}
	}

	private void endRootEffectSpider()
	{
		GameObject _roots = GameObject.Find("Roots");
		
		if( _roots )
		{
			GameObject.Find("Roots").GetComponent<Animation>().Play("end");
			Spider.GetComponent<SpiderAnimation>().Rooted = false;
			
			if (!Spider.GetComponent<SpiderAnimation>().Dead)
				gameLogic.playSoundEffect(rootsDisappear, 0.4f);
			
			Invoke("ReleaseAttack", 0.5f);
		}
	}
	
	public override void Defend() { }

}
