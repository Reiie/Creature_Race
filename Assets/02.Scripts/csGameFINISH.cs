using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class csGameFINISH : MonoBehaviour
{
    public bool GameFin = false;

    public int Player1_Rank = 0;
    public int Player2_Rank = 0;
    public int Player3_Rank = 0;
    public int Player4_Rank = 0;

    public GameObject FINISH_Camera;
    public GameObject FINISH_Object;
    public GameObject FINISH_Canvas1;
    public GameObject FINISH_Canvas2;

    public GameObject Basic_Canvas;
    public GameObject Player1_Canvas;
    public GameObject Player2_Canvas;
    public GameObject Player3_Canvas;
    public GameObject Player4_Canvas;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject Camera4;

    WaypointProgressTracker Player1_traker;
    WaypointProgressTracker Player2_traker;
    WaypointProgressTracker Player3_traker;
    WaypointProgressTracker Player4_traker;

    CarController P1_Controller;
    CarController P2_Controller;
    CarController P3_Controller;
    CarController P4_Controller;

    csRanking Ranking;

    csCollisionCheck P1_CollCheck;
    csCollisionCheck P2_CollCheck;
    csCollisionCheck P3_CollCheck;
    csCollisionCheck P4_CollCheck;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    public GameObject Winner1_Position;
    public GameObject Winner2_Position;
    public GameObject Winner3_Position;
    public GameObject Retire1_Position;
    public GameObject Retire2_Position;
    public GameObject Retire3_Position;
    public GameObject Retire4_Position;

    public Text Ranking1_Name;
    public Text Ranking2_Name;
    public Text Ranking3_Name;
    public Text Ranking4_Name;

    public Text Ranking1_LapTime;
    public Text Ranking2_LapTime;
    public Text Ranking3_LapTime;
    public Text Ranking4_LapTime;

    public Text Ranking1_GainGOLD;
    public Text Ranking2_GainGOLD;
    public Text Ranking3_GainGOLD;
    public Text Ranking4_GainGOLD;

    void Start()
    {
        Ranking = GameObject.Find("Directional Light").GetComponent<csRanking>();

        Player1_traker = GameObject.Find("MyPlayer").GetComponent<WaypointProgressTracker>();
        Player2_traker = GameObject.Find("Computer1").GetComponent<WaypointProgressTracker>();
        Player3_traker = GameObject.Find("Computer2").GetComponent<WaypointProgressTracker>();
        Player4_traker = GameObject.Find("Computer3").GetComponent<WaypointProgressTracker>();

        P1_CollCheck = GameObject.Find("MyPlayer").GetComponent<csCollisionCheck>();
        P2_CollCheck = GameObject.Find("Computer1").GetComponent<csCollisionCheck>();
        P3_CollCheck = GameObject.Find("Computer2").GetComponent<csCollisionCheck>();
        P4_CollCheck = GameObject.Find("Computer3").GetComponent<csCollisionCheck>();

        P1_Controller = GameObject.Find("MyPlayer").GetComponent<CarController>();
        P2_Controller = GameObject.Find("Computer1").GetComponent<CarController>();
        P3_Controller = GameObject.Find("Computer2").GetComponent<CarController>();
        P4_Controller = GameObject.Find("Computer3").GetComponent<CarController>();
    }

    void Update()
    {
        Rank_and_LapTime();

        if (GameFin == true)
        {
            GameFin = false;

            P1_Controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
            P2_Controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
            P3_Controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
            P4_Controller.GetComponent<Rigidbody>().velocity = Vector3.zero;

            P1_Controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            P2_Controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            P3_Controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            P4_Controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            Basic_Canvas.SetActive(false);
            Player1_Canvas.SetActive(false);
            Player2_Canvas.SetActive(false);
            Player3_Canvas.SetActive(false);
            Player4_Canvas.SetActive(false);
            Camera1.SetActive(false);
            Camera2.SetActive(false);
            Camera3.SetActive(false);
            Camera4.SetActive(false);

            Time.timeScale = 1.0f;

            FINISH_Camera.SetActive(true);
            FINISH_Object.SetActive(true);
            FINISH_Canvas1.SetActive(true);

            if (Ranking.Player1_Win == true && Ranking.Player2_Win == false && Ranking.Player3_Win == false && Ranking.Player4_Win == false)
            {
                Player1.transform.position = Winner1_Position.transform.position;
                Player1_Rank = 1;
            }
            else if (Ranking.Player1_Win == false && Ranking.Player2_Win == true && Ranking.Player3_Win == false && Ranking.Player4_Win == false)
            {
                Player2.transform.position = Winner1_Position.transform.position;
                Player2_Rank = 1;
            }
            else if (Ranking.Player1_Win == false && Ranking.Player2_Win == false && Ranking.Player3_Win == true && Ranking.Player4_Win == false)
            {
                Player3.transform.position = Winner1_Position.transform.position;
                Player3_Rank = 1;
            }
            else if (Ranking.Player1_Win == false && Ranking.Player2_Win == false && Ranking.Player3_Win == false && Ranking.Player4_Win == true)
            {
                Player4.transform.position = Winner1_Position.transform.position;
                Player4_Rank = 1;
            }

            if (P1_CollCheck.Game_End == true)
            {
                /// 1P 2등 완주
                if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance > Player3_traker.progressDistance &&
                 Player1_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 1000.0f;

                    Player1.transform.position = Winner2_Position.transform.position;
                    Player1_Rank = 2;
                }

                else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                         Player1_traker.progressDistance < Player3_traker.progressDistance &&
                         Player1_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 1000.0f;

                    Player1.transform.position = Winner2_Position.transform.position;
                    Player1_Rank = 2;
                }

                else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                         Player1_traker.progressDistance > Player3_traker.progressDistance &&
                         Player1_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 1000.0f;

                    Player1.transform.position = Winner2_Position.transform.position;
                    Player1_Rank = 2;
                }
                /// 1P 3등 완주
                else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance < Player3_traker.progressDistance &&
                 Player1_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 100.0f;

                    Player1.transform.position = Winner3_Position.transform.position;
                    Player1_Rank = 3;
                }

                else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                         Player1_traker.progressDistance < Player3_traker.progressDistance &&
                         Player1_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 100.0f;

                    Player1.transform.position = Winner3_Position.transform.position;
                    Player1_Rank = 3;
                }

                else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                         Player1_traker.progressDistance > Player3_traker.progressDistance &&
                         Player1_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player1_traker.progressDistance = Player1_traker.progressDistance + 100.0f;

                    Player1.transform.position = Winner3_Position.transform.position;
                    Player1_Rank = 3;
                }
            }

            if (P2_CollCheck.Game_End == true)
            {
                /// 2P 2등 완주
                if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                 Player2_traker.progressDistance > Player3_traker.progressDistance &&
                 Player2_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player2_traker.progressDistance = Player2_traker.progressDistance + 1000.0f;

                    Player2.transform.position = Winner2_Position.transform.position;
                    Player2_Rank = 2;
                }

                else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                         Player2_traker.progressDistance < Player3_traker.progressDistance &&
                         Player2_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player2_traker.progressDistance = Player2_traker.progressDistance + 1000.0f;

                    Player2.transform.position = Winner2_Position.transform.position;
                    Player2_Rank = 2;
                }

                else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                         Player2_traker.progressDistance > Player3_traker.progressDistance &&
                         Player2_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player2_traker.progressDistance = Player2_traker.progressDistance + 1000.0f;

                    Player2.transform.position = Winner2_Position.transform.position;
                    Player2_Rank = 2;
                }

                /// 2P 3 등 완주
                else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                         Player2_traker.progressDistance < Player3_traker.progressDistance &&
                         Player2_traker.progressDistance > Player4_traker.progressDistance)
                {

                    Player2_traker.progressDistance = Player2_traker.progressDistance + 100.0f;

                    Player2.transform.position = Winner3_Position.transform.position;
                    Player2_Rank = 3;
                }

                else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                         Player2_traker.progressDistance < Player3_traker.progressDistance &&
                         Player2_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player2_traker.progressDistance = Player2_traker.progressDistance + 100.0f;

                    Player2.transform.position = Winner3_Position.transform.position;
                    Player2_Rank = 3;
                }

                else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                         Player2_traker.progressDistance > Player3_traker.progressDistance &&
                         Player2_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player2_traker.progressDistance = Player2_traker.progressDistance + 100.0f;

                    Player2.transform.position = Winner3_Position.transform.position;
                    Player2_Rank = 3;
                }
            }

            if (P3_CollCheck.Game_End == true)
            {
                /// 3P 2등 완주
                if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                 Player3_traker.progressDistance > Player1_traker.progressDistance &&
                 Player3_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 1000.0f;

                    Player3.transform.position = Winner2_Position.transform.position;
                    Player3_Rank = 2;
                }

                else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                         Player3_traker.progressDistance < Player1_traker.progressDistance &&
                         Player3_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 1000.0f;

                    Player3.transform.position = Winner2_Position.transform.position;
                    Player3_Rank = 2;
                }

                else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                         Player3_traker.progressDistance > Player1_traker.progressDistance &&
                         Player3_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 1000.0f;

                    Player3.transform.position = Winner2_Position.transform.position;
                    Player3_Rank = 2;
                }

                /// 3P 3 등 완주
                else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                         Player3_traker.progressDistance < Player1_traker.progressDistance &&
                         Player3_traker.progressDistance > Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 100.0f;

                    Player3.transform.position = Winner3_Position.transform.position;
                    Player3_Rank = 3;
                }

                else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                         Player3_traker.progressDistance < Player1_traker.progressDistance &&
                         Player3_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 100.0f;

                    Player3.transform.position = Winner3_Position.transform.position;
                    Player3_Rank = 3;
                }

                else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                         Player3_traker.progressDistance > Player1_traker.progressDistance &&
                         Player3_traker.progressDistance < Player4_traker.progressDistance)
                {
                    Player3_traker.progressDistance = Player3_traker.progressDistance + 100.0f;

                    Player3.transform.position = Winner3_Position.transform.position;
                    Player3_Rank = 3;
                }
            }

            if (P4_CollCheck.Game_End == true)
            {
                /// 4P 2등 완주
                if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                 Player4_traker.progressDistance > Player3_traker.progressDistance &&
                 Player4_traker.progressDistance > Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 1000.0f;

                    Player4.transform.position = Winner2_Position.transform.position;
                    Player4_Rank = 2;
                }

                else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                         Player4_traker.progressDistance < Player3_traker.progressDistance &&
                         Player4_traker.progressDistance > Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 1000.0f;

                    Player4.transform.position = Winner2_Position.transform.position;
                    Player4_Rank = 2;
                }

                else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                         Player4_traker.progressDistance > Player3_traker.progressDistance &&
                         Player4_traker.progressDistance < Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 1000.0f;

                    Player4.transform.position = Winner2_Position.transform.position;
                    Player4_Rank = 2;
                }

                /// 4P 3등 완주
                else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                         Player4_traker.progressDistance < Player3_traker.progressDistance &&
                         Player4_traker.progressDistance > Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 100.0f;

                    Player4.transform.position = Winner3_Position.transform.position;
                    Player4_Rank = 3;
                }

                else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                         Player4_traker.progressDistance < Player3_traker.progressDistance &&
                         Player4_traker.progressDistance < Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 100.0f;

                    Player4.transform.position = Winner3_Position.transform.position;
                    Player4_Rank = 3;
                }

                else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                         Player4_traker.progressDistance > Player3_traker.progressDistance &&
                         Player4_traker.progressDistance < Player1_traker.progressDistance)
                {
                    Player4_traker.progressDistance = Player4_traker.progressDistance + 100.0f;

                    Player4.transform.position = Winner3_Position.transform.position;
                    Player4_Rank = 3;
                }
            }
            if (P1_CollCheck.Game_End == false)     /// 1P 완주못함
            {
                Player1.transform.position = Retire1_Position.transform.position;
                Player1_Rank = -1;
            }
            if (P2_CollCheck.Game_End == false)     /// 2P 완주못함
            {
                Player2.transform.position = Retire2_Position.transform.position;
                Player2_Rank = -1;
            }
            if (P3_CollCheck.Game_End == false)     /// 3P 완주못함
            {
                Player3.transform.position = Retire3_Position.transform.position;
                Player3_Rank = -1;
            }
            if (P4_CollCheck.Game_End == false)     /// 4P 완주못함
            {
                Player4.transform.position = Retire4_Position.transform.position;
                Player4_Rank = -1;
            }

            StartCoroutine(SceneTrans());
        }

    }
    
    void Rank_and_LapTime()
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (Player1_Rank == 1 && Player2_Rank == 2 && Player3_Rank == 3 && Player4_Rank == -1)
        { 
            ///
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == 3 && Player3_Rank == 2 && Player4_Rank == -1)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 3 && Player2_Rank == 1 && Player3_Rank == 2 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;

            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == 1 && Player3_Rank == 3 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == 3 && Player3_Rank == 1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == 2 && Player3_Rank == -1 && Player4_Rank == 3)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == 3 && Player3_Rank == -1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == 1 && Player3_Rank == -1 && Player4_Rank == 3)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 3 && Player2_Rank == 1 && Player3_Rank == -1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == 3 && Player3_Rank == -1 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 3 && Player2_Rank == 2 && Player3_Rank == -1 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;

            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == -1 && Player3_Rank == 2 && Player4_Rank == 3)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == -1 && Player3_Rank == 3 && Player4_Rank == 2)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == -1 && Player3_Rank == 1 && Player4_Rank == 3)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 3 && Player2_Rank == -1 && Player3_Rank == 1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == -1 && Player3_Rank == 3 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 3 && Player2_Rank == -1 && Player3_Rank == 2 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;

            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 1 && Player3_Rank == 2 && Player4_Rank == 3)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 1 && Player3_Rank == 3 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 2 && Player3_Rank == 1 && Player4_Rank == 3)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 2 && Player3_Rank == 1 && Player4_Rank == 3)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 3 && Player3_Rank == 1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 2 && Player3_Rank == 3 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 3 && Player3_Rank == 2 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;

            Ranking4_Name.text = UserManager.Instance().name;
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == 2 && Player3_Rank == -1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = "타랑";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == 1 && Player3_Rank == -1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "타랑";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == -1 && Player3_Rank == 2 && Player4_Rank == -1)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == -1 && Player3_Rank == 1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == -1 && Player3_Rank == -1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 2 && Player2_Rank == -1 && Player3_Rank == -1 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;

            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 1 && Player3_Rank == 2 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 1 && Player3_Rank == -1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 2 && Player3_Rank == -1 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 2 && Player3_Rank == 1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == -1 && Player3_Rank == 1 && Player4_Rank == 2)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == -1 && Player3_Rank == 2 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;

            Ranking3_Name.text = UserManager.Instance().name;
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "말타지";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == 1 && Player2_Rank == -1 && Player3_Rank == -1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;

            Ranking2_Name.text = "말타지";
            Ranking2_LapTime.text = "Retire";
            Ranking3_Name.text = "타랑";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == 1 && Player3_Rank == -1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            Ranking2_LapTime.text = "Retire";
            Ranking3_Name.text = "타랑";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == -1 && Player3_Rank == 1 && Player4_Rank == -1)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            Ranking2_LapTime.text = "Retire";
            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "더헛";
            Ranking4_LapTime.text = "Retire";
        }
        else if (Player1_Rank == -1 && Player2_Rank == -1 && Player3_Rank == -1 && Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;

            Ranking2_Name.text = UserManager.Instance().name;
            Ranking2_LapTime.text = "Retire";
            Ranking3_Name.text = "말타지";
            Ranking3_LapTime.text = "Retire";
            Ranking4_Name.text = "타랑";
            Ranking4_LapTime.text = "Retire";
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
    }


    IEnumerator SceneTrans()
    {
        if (Player1_Rank == 1)
        {
            UserManager.Instance().raceReward = 3000;
        }
        else if (Player1_Rank == 2)
        {
            UserManager.Instance().raceReward = 2000;
        }
        else if (Player1_Rank == 3)
        {
            UserManager.Instance().raceReward = 1000;
        }
        else
        {
            UserManager.Instance().raceReward = 0;
        }

        UserManager.Instance().raceUpdate();
      //  UserManager.Instance().first = true;
        //UserManager.Instance().nowDate = UserManager.Instance().nowDate.AddDays(1);
       // int tmp = PlayerPrefs.GetInt("ProgressCount");
        //PlayerPrefs.SetInt("ProgressCount", 0);
        //UserManager.Instance().userInfoSave();


        yield return new WaitForSeconds(5.0f);

        FINISH_Canvas2.SetActive(true);
        FINISH_Canvas1.SetActive(false);

        if (Player1_Rank == 0)
        {
            Ranking4_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking4_LapTime.text = P1_Lab_Time;
        }
        else if (Player2_Rank == 0)
        {
            Ranking4_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking4_LapTime.text = P2_Lab_Time;
        }
        else if (Player3_Rank == 0)
        {
            Ranking4_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking4_LapTime.text = P3_Lab_Time;
        }
        else if (Player4_Rank == 0)
        {
            Ranking4_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking4_LapTime.text = P4_Lab_Time;
        }

        if (Player1_Rank == 1)
        {
            Ranking1_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P1_Lab_Time;
        }
        else if (Player2_Rank == 1)
        {
            Ranking1_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P2_Lab_Time;
        }
        else if (Player3_Rank == 1)
        {
            Ranking1_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P3_Lab_Time;
        }
        else if (Player4_Rank == 1)
        {
            Ranking1_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking1_LapTime.text = P4_Lab_Time;
        }

        if (Player1_Rank == 2)
        {
            Ranking2_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P1_Lab_Time;
        }
        else if (Player2_Rank == 2)
        {
            Ranking2_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P2_Lab_Time;
        }
        else if (Player3_Rank == 2)
        {
            Ranking2_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P3_Lab_Time;
        }
        else if (Player4_Rank == 2)
        {
            Ranking2_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking2_LapTime.text = P4_Lab_Time;
        }

        if (Player1_Rank == 3)
        {
            Ranking3_Name.text = UserManager.Instance().name;
            string P1_Lab_Time = P1_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P1_Lab_Time;
        }
        else if (Player2_Rank == 3)
        {
            Ranking3_Name.text = "말타지";
            string P2_Lab_Time = P2_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P2_Lab_Time;
        }
        else if (Player3_Rank == 3)
        {
            Ranking3_Name.text = "타랑";
            string P3_Lab_Time = P3_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P3_Lab_Time;
        }
        else if (Player4_Rank == 3)
        {
            Ranking3_Name.text = "더헛";
            string P4_Lab_Time = P4_CollCheck.Lap_Time.ToString("N2");
            Ranking3_LapTime.text = P4_Lab_Time;
        }




        StartCoroutine(NEXT());
    }

    IEnumerator NEXT()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene("2. Intro_To_Ui_Loading");
    }
}
