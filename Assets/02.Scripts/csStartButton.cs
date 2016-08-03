using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csStartButton : MonoBehaviour
{
    public GameObject Button_Text;

    void Start()
    {
        GOGO();
    }

    void GOGO()
    {
        StartCoroutine(CORUTIN());
    }

    IEnumerator CORUTIN()
    {
        yield return new WaitForSeconds(0.2f);

        Button_Text.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        Button_Text.SetActive(true);
        GOGO();
    }

}
