using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csWord_Text : MonoBehaviour
{
    public Text Word_Text;

    void Start()
    {
        int Random_Word = Random.Range(0, 5);

        if (Random_Word == 0)
        {
            Word_Text.text = "『 Team Probe . . . 』";
        }

        else if (Random_Word == 1)
        {
            Word_Text.text = "『 개발자 한명은 오버워치 초 고수일지 모릅니다. 』";
        }

        else if (Random_Word == 2)
        {
            Word_Text.text = "『 레이싱 Ai 는 다시 만들지 않으려구요 . . 』";
        }

        else if (Random_Word == 3)
        {
            Word_Text.text = "『 P.P 용 게임이라 맵 갯수와 디자인이 매우 부족합니다. . 』";
        }

        else if (Random_Word == 4)
        {
            Word_Text.text = "『 아마 . . 더이상의 업데이트는 엄써요 . . 』";
        }

        else if (Random_Word == 5)
        {
            Word_Text.text = "『 겐지장인 김세영 』";
        }
    }
}
