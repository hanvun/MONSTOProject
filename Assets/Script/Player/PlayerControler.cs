using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

	const string TOP_WALL = "TopWall";
	const string BOTTOM_WALL = "BottomWall";
	const string LEFT_WALL = "LeftWall";
	const string RIGHT_WALL = "RightWall";

	bool isHolded;	//タッチし続けている状態か
	bool standby = true;
	[SerializeField] Vector3 force; //移動値

	[SerializeField] float friction = 0.005f;	//摩擦係数
	[SerializeField] float minforce = 0.2f;	//停止移動値
	[SerializeField] float power = 4f;

	[SerializeField] SpriteRenderer arrow; //矢印画像
	[SerializeField] float arrowScale = 100; //矢印の表示倍率

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		force *= friction;
		standby = false;
		if (force.magnitude < minforce) {
			//force = default(Vector3);
			standby = true;
		}
		//このスクリプトがアタッチされているGameObject
		transform.position += force * Time.deltaTime;

		if (Input.GetMouseButtonDown (0) && standby) {
			var hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit != null)Debug.Log ("Touch Player");

			if (hit.collider != null) {
				arrow.gameObject.SetActive (true);
				isHolded = true;
			}
		}

		if (Input.GetMouseButton (0) && standby) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0;
			float scale = (transform.position - pos)
				.magnitude / Screen.width * arrowScale;
			arrow.transform.localScale = new Vector3 (scale, scale, 1);

			float angle = Vector3.Angle (transform.position - pos, Vector3.up);

			Vector3 cross = Vector3.Cross (transform.position - pos, Vector3.up);
			if (cross.z > 0)angle *= -1;
			arrow.transform.rotation
			= Quaternion.Euler (new Vector3 (0, 0, angle + 90));
		}

		if (Input.GetMouseButtonUp (0) && isHolded) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log (pos);
			pos.z = transform.position.z;
			force = transform.position - pos;
			force *= power;
			isHolded = false;
			arrow.gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		Debug.Log ("Onhit");
		//壁に当たったら反対側に
		if(coll.CompareTag(TOP_WALL) || coll.CompareTag(BOTTOM_WALL)){
			force.y *= -1;
		}
		else if(coll.CompareTag(LEFT_WALL) || coll.CompareTag(RIGHT_WALL)) {
			force.x *= -1;
		}
		else if(coll.CompareTag("Enemy")){
			Vector3 vectoEnemy = coll.gameObject.transform.position - this.transform.position;
			force.z = 0;
			vectoEnemy.z = 0;


			float deg = Vector2.Angle (force, vectoEnemy);

			Vector3 cross = Vector3.Cross (force, vectoEnemy);
			if (cross.z > 0) deg *= -1;

			force = Quaternion.AngleAxis (deg * 2, Vector3.back) * force * -1;
			coll.GetComponent<EnemyController> ().Damage (10);
			
		}
	}
}
