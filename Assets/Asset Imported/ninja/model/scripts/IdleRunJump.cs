using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]



public class IdleRunJump : MonoBehaviour
{
	
	private Animator anim;						
	private AnimatorStateInfo currentState;		
	private AnimatorStateInfo previousState;
	public bool _random = false;			
	public float _threshold = 0.5f;				
	public float _interval = 2f;			
	 private float _seed = 0.0f;			 
	
	
	
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		currentState = anim.GetCurrentAnimatorStateInfo (0);
		previousState = currentState;
		StartCoroutine ("RandomChange");
	}
	
	// Update is called once per frame
	void  Update ()
	{
		if (Input.GetKeyDown ("up") || Input.GetButton ("Jump")) {
			anim.SetBool ("Next", true);
		}
		
		if (Input.GetKeyDown ("down")) {
			anim.SetBool ("Back", true);
		}
		
		 
	}
	
	
	void OnGUI()
	{
		/*GUI.Box(new Rect(Screen.width - 110 , 10 ,100 ,90), "Change Motion");
		if(GUI.Button(new Rect(Screen.width - 100 , 40 ,80, 20), "Next"))
			anim.SetBool ("Next", true);
		if(GUI.Button(new Rect(Screen.width - 100 , 70 ,80, 20), "Back"))
			anim.SetBool ("Back", true);*/
	}
	
	
	IEnumerator RandomChange ()
	{
		while (true) {
			if (_random) {
				float _seed = Random.Range (-1f, 1f);
				if (_seed <= -_threshold) {
					anim.SetBool ("Back", true);
				} else if (_seed >= _threshold) {
					anim.SetBool ("Next", true);
				}
			}
			yield return new WaitForSeconds (_interval);
		}
		
	}
	
}
