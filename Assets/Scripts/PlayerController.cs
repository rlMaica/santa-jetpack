using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;
	public Rigidbody2D rb;

	public Transform pisoCheckTransform;
	public bool piso;
	public LayerMask pisoCheckLayerMask;
	Animator animator;

	public ParticleSystem jetpack;
	private bool dead = false;
	private uint moedas = 0;

	public Texture2D moedaIconTexture;

	void OnGUI() {
		MostrarContadorDeMoedas ();
		MostrarBotaoDeRestart ();
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void FixedUpdate(){
//		rb.AddForce(new Vector2 (0, jetpackForce));
//	}

	void FixedUpdate () {
		bool jetpackAtivo = Input.GetButton("Fire1");

		jetpackAtivo = jetpackAtivo && !dead;

		if (jetpackAtivo){
			rb.AddForce(new Vector2 (0, jetpackForce));
		}

		if (!dead) {
			Vector2 newVelocity = rb.velocity;
			newVelocity.x = forwardMovementSpeed;
			rb.velocity = newVelocity;
		}

		AtualizarPisoStatus ();
		AjusteJetpack (jetpackAtivo);
	}

	void AtualizarPisoStatus() {
		piso = Physics2D.OverlapCircle (pisoCheckTransform.position, 0.1f, pisoCheckLayerMask);
		animator.SetBool ("andando", piso);
	}

	void AjusteJetpack (bool jetpackAtivo){
		jetpack.enableEmission = !piso;
		jetpack.emissionRate = jetpackAtivo ? 300.0f : 75.0f; 
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag("Moedas"))
			ColetandoMoedas(collider);
		else
			HitByLaser(collider);
	}

	void HitByLaser(Collider2D laserCollider){
		dead = true;
		animator.SetBool ("andando", true);
		animator.SetBool ("morto", true);
	}

	void ColetandoMoedas(Collider2D coinCollider){
		moedas++;

		Destroy(coinCollider.gameObject);
	}

	void MostrarContadorDeMoedas(){
		Rect coinIconRect = new Rect(10, 10, 32, 32);
		GUI.DrawTexture(coinIconRect, moedaIconTexture);                         

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;

		Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
		GUI.Label(labelRect, moedas.ToString(), style);
	}

	void MostrarBotaoDeRestart(){
		if (dead && piso)
		{
			Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
			if (GUI.Button(buttonRect, "Toque para reiniciar!"))
			{
				Application.LoadLevel (Application.loadedLevelName);
			};
		}
	}
}
