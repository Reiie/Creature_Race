using UnityEngine;
using System.Collections;

public class csScene : MonoBehaviour {

    // Use this for initialization

    public GameObject creature;
    public CalenderManager manager;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnLevelWasLoaded()
    {
        Debug.Log("씬전환");
        if (Application.loadedLevelName == "3. UiScene")
        {
            
            StartCoroutine(delay());
        }
    }

    IEnumerator delay()
    {
      
        yield return new WaitForSeconds(0.5f);
        //UserManager.Instance().WorkUpdate();


        UserManager.Instance().continueWork();


        if (UserManager.Instance().first == true)
        {
            UserManager.Instance().CreatureFalse();
        }
    }
}
