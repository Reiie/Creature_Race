using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csTextAnim : MonoBehaviour
{
    public TextMesh Loading_Text;

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

        Loading_Text.text = "Loading";

        yield return new WaitForSeconds(0.2f);

        Loading_Text.text = "Loading.";

        yield return new WaitForSeconds(0.2f);

        Loading_Text.text = "Loading..";

        yield return new WaitForSeconds(0.2f);

        Loading_Text.text = "Loading...";
        GOGO();

    }

}
