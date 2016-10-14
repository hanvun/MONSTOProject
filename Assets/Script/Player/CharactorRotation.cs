using UnityEngine;
using System.Collections;

public class CharactorRotation : MonoBehaviour {

	[SerializeField] SpriteRenderer sprite;
	[SerializeField] float range = 0.1f;
	[SerializeField] float speed = 2f;
	float elapsed;
	float rand;

	// Use this for initialization
	void Start () {
		rand = Random.value * Mathf.PI * 2;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.unscaledDeltaTime * speed;

		sprite.transform.localPosition =
			new Vector3(0, Mathf.Sin(elapsed + rand) * range, 0);
	}
}
