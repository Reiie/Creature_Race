using UnityEngine;
using System.Collections;

public class csSoundManager : MonoBehaviour 
{
	public AudioClip IntroBG;
    public AudioClip Stage1BG;
    public AudioClip Stage2BG;

	void Start()
	{
		GetComponent<AudioSource> ().Play ();
	}
}
