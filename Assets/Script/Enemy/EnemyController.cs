using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	[SerializeField] int life;
	[SerializeField] int maxLife;
	[SerializeField] Slider hpGauge;

	void Start(){
		life = maxLife;
	}

	// Update is called once per frame
	void Update () {
	
		if (life <= 0) {
			Destroy (gameObject);
		}

		hpGauge.value = (float)life / maxLife;
	}

	public void Damage(int value){
		life = value;
	}
}
