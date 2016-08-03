using UnityEngine;
using System.Collections;

public class csCamera : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject Camera4;

    public GameObject Player1Canvas;
    public GameObject Player2Canvas;
    public GameObject Player3Canvas;
    public GameObject Player4Canvas;

    public void ChangeCamera1()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
        Camera4.SetActive(false);

        Player1Canvas.SetActive(true);
        Player2Canvas.SetActive(false);
        Player3Canvas.SetActive(false);
        Player4Canvas.SetActive(false);

    }

    public void ChangeCamera2()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        Camera3.SetActive(false);
        Camera4.SetActive(false);

        Player1Canvas.SetActive(false);
        Player2Canvas.SetActive(true);
        Player3Canvas.SetActive(false);
        Player4Canvas.SetActive(false);
    }

    public void ChangeCamera3()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(true);
        Camera4.SetActive(false);

        Player1Canvas.SetActive(false);
        Player2Canvas.SetActive(false);
        Player3Canvas.SetActive(true);
        Player4Canvas.SetActive(false);
    }

    public void ChangeCamera4()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
        Camera4.SetActive(true);

        Player1Canvas.SetActive(false);
        Player2Canvas.SetActive(false);
        Player3Canvas.SetActive(false);
        Player4Canvas.SetActive(true);
    }

}
