using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class csRanking : MonoBehaviour
{
    csGameFINISH GameFIN;

    public Text P1_Button_Text;

	public GameObject Player1;
	public GameObject Player2;
	public GameObject Player3;
	public GameObject Player4;

    csCarState P1_Car_State;
	csCarState P2_Car_State;
	csCarState P3_Car_State;
	csCarState P4_Car_State;

    WaypointProgressTracker Player1_traker;
    WaypointProgressTracker Player2_traker;
    WaypointProgressTracker Player3_traker;
    WaypointProgressTracker Player4_traker;

    public Text Player1_Rank;
    public Text Player2_Rank;
    public Text Player3_Rank;
    public Text Player4_Rank;

    csCollisionCheck Player1_CollCheck;
    csCollisionCheck Player2_CollCheck;
    csCollisionCheck Player3_CollCheck;
    csCollisionCheck Player4_CollCheck;

	CarController P1_Carcontroller;
	CarController P2_Carcontroller;
	CarController P3_Carcontroller;
	CarController P4_Carcontroller;

    public Text P1Rep;
    public Text P2Rep;
    public Text P3Rep;
    public Text P4Rep;

    public Text One_Blank;
    public Text Two_Blank;
    public Text Three_Blank;
    public Text Four_Blank;

    public Text Winner_Text;

    public Text Over_Count;

    bool Over_Counter_Fix = true;

    float P1_Speed1_Ex;
	float P1_Speed1_0x;
	float P1_Speed1_1x;
	float P1_Speed1_2x;

    float P2_Speed1_Ex;
    float P2_Speed1_0x;
    float P2_Speed1_1x;
    float P2_Speed1_2x;

    float P3_Speed1_Ex;
    float P3_Speed1_0x;
    float P3_Speed1_1x;
    float P3_Speed1_2x;

    float P4_Speed1_Ex;
    float P4_Speed1_0x;
    float P4_Speed1_1x;
    float P4_Speed1_2x;

    public bool Player1_Win = false;
    public bool Player2_Win = false;
    public bool Player3_Win = false;
    public bool Player4_Win = false;

    /// 이펙트부분

    public GameObject F_Particle;
    public GameObject T_Particle;

    public GameObject Four_Effect;
    public GameObject Three_Effect;

    ///

    void Start()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();

        P1_Button_Text.text = UserManager.Instance().name;

        GameFIN = GameObject.Find("Directional Light").GetComponent<csGameFINISH>();

        Player1_traker = GameObject.Find("MyPlayer").GetComponent<WaypointProgressTracker>();
        Player2_traker = GameObject.Find("Computer1").GetComponent<WaypointProgressTracker>();
        Player3_traker = GameObject.Find("Computer2").GetComponent<WaypointProgressTracker>();
        Player4_traker = GameObject.Find("Computer3").GetComponent<WaypointProgressTracker>();

        Player1_CollCheck = GameObject.Find("MyPlayer").GetComponent<csCollisionCheck>();
        Player2_CollCheck = GameObject.Find("Computer1").GetComponent<csCollisionCheck>();
        Player3_CollCheck = GameObject.Find("Computer2").GetComponent<csCollisionCheck>();
        Player4_CollCheck = GameObject.Find("Computer3").GetComponent<csCollisionCheck>();

		P1_Car_State = GameObject.Find("MyPlayer").GetComponent<csCarState>();
		P2_Car_State = GameObject.Find("Computer1").GetComponent<csCarState>();
		P3_Car_State = GameObject.Find("Computer2").GetComponent<csCarState>();
		P4_Car_State = GameObject.Find("Computer3").GetComponent<csCarState>();

		P1_Carcontroller = GameObject.Find("MyPlayer").GetComponent<CarController>();
		P2_Carcontroller = GameObject.Find("Computer1").GetComponent<CarController>();
		P3_Carcontroller = GameObject.Find("Computer2").GetComponent<CarController>();
		P4_Carcontroller = GameObject.Find("Computer3").GetComponent<CarController>();

        P1_Speed1_Ex = P1_Car_State.maxSpeed * 0.9f;
        P1_Speed1_0x = P1_Car_State.maxSpeed;
        P1_Speed1_1x = P1_Car_State.maxSpeed * 1.5f;
        P1_Speed1_2x = P1_Car_State.maxSpeed * 2.0f;

        P2_Speed1_Ex = P1_Car_State.maxSpeed * 0.9f;
        P2_Speed1_0x = P2_Car_State.maxSpeed;
        P2_Speed1_1x = P2_Car_State.maxSpeed * 1.5f;
        P2_Speed1_2x = P2_Car_State.maxSpeed * 2.0f;

        P3_Speed1_Ex = P1_Car_State.maxSpeed * 0.9f;
        P3_Speed1_0x = P3_Car_State.maxSpeed;
        P3_Speed1_1x = P3_Car_State.maxSpeed * 1.5f;
        P3_Speed1_2x = P3_Car_State.maxSpeed * 2.0f;

        P4_Speed1_Ex = P1_Car_State.maxSpeed * 0.9f;
        P4_Speed1_0x = P4_Car_State.maxSpeed;
        P4_Speed1_1x = P4_Car_State.maxSpeed * 1.5f;
        P4_Speed1_2x = P4_Car_State.maxSpeed * 2.0f;
    }

    void Update()
	{
        Debug.Log("P1 = " + P1_Car_State.carState);
        Debug.Log("P2 = " + P2_Car_State.carState);
        Debug.Log("P3 = " + P3_Car_State.carState);
        Debug.Log("P4 = " + P4_Car_State.carState);


        P1Rep.text = Player1_CollCheck.Checking_Rep.ToString();
        P2Rep.text = Player2_CollCheck.Checking_Rep.ToString();
        P3Rep.text = Player3_CollCheck.Checking_Rep.ToString();
        P4Rep.text = Player4_CollCheck.Checking_Rep.ToString();

        if (Player1_Win == true)
        {
            Player2_Win = false;
            Player3_Win = false;
            Player4_Win = false;
        }

        if (Player2_Win == true)
        {
            Player1_Win = false;
            Player3_Win = false;
            Player4_Win = false;
        }

        if (Player3_Win == true)
        {
            Player1_Win = false;
            Player2_Win = false;
            Player4_Win = false;
        }

        if (Player4_Win == true)
        {
            Player1_Win = false;
            Player2_Win = false;
            Player3_Win = false;
        }

        ///////////////////////////////////1Player///////////////////////////////////////////
        /// 1Player 가 1등일 경우
        if (Player1_traker.progressDistance > Player2_traker.progressDistance && 
            Player1_traker.progressDistance > Player3_traker.progressDistance && 
            Player1_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player1_Rank.text = "1 th";
            One_Blank.text = UserManager.Instance().name;

            Destroy(F_Particle);
            Destroy(T_Particle);

            P1_Car_State.maxSpeed = P1_Speed1_Ex;

            P1_Car_State.isRanking_3th = false;
            P1_Car_State.isRanking_4th = false;
        }

        /// 1Player 가 2 등일경우
        else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance > Player3_traker.progressDistance &&
                 Player1_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player1_Rank.text = "2 th";
            Two_Blank.text = UserManager.Instance().name;

            Destroy(F_Particle);
            Destroy(T_Particle);

            P1_Car_State.maxSpeed = P1_Speed1_0x;

            P1_Car_State.isRanking_3th = false;
            P1_Car_State.isRanking_4th = false;
        }

        else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                 Player1_traker.progressDistance < Player3_traker.progressDistance &&
                 Player1_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player1_Rank.text = "2 th";
            Two_Blank.text = UserManager.Instance().name;

            Destroy(F_Particle);
            Destroy(T_Particle);

            P1_Car_State.maxSpeed = P1_Speed1_0x;

            P1_Car_State.isRanking_3th = false;
            P1_Car_State.isRanking_4th = false;
        }

        else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                 Player1_traker.progressDistance > Player3_traker.progressDistance &&
                 Player1_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player1_Rank.text = "2 th";
            Two_Blank.text = UserManager.Instance().name;

            Destroy(F_Particle);
            Destroy(T_Particle);

            P1_Car_State.maxSpeed = P1_Speed1_0x;

            P1_Car_State.isRanking_3th = false;
            P1_Car_State.isRanking_4th = false;
        }

        /// 1Player 가 3 등일경우
        else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance < Player3_traker.progressDistance &&
                 Player1_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player1_Rank.text = "3 th";
			Three_Blank.text = UserManager.Instance().name;

            P1_Car_State.maxSpeed = P1_Speed1_1x;

            P1_Car_State.isRanking_3th = true;
            P1_Car_State.isRanking_4th = false;
        }

        else if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
                 Player1_traker.progressDistance < Player3_traker.progressDistance &&
                 Player1_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player1_Rank.text = "3 th";
            Three_Blank.text = UserManager.Instance().name;

            P1_Car_State.maxSpeed = P1_Speed1_1x;

            P1_Car_State.isRanking_3th = true;
            P1_Car_State.isRanking_4th = false;
        }

        else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance > Player3_traker.progressDistance &&
                 Player1_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player1_Rank.text = "3 th";
            Three_Blank.text = UserManager.Instance().name;

            P1_Car_State.maxSpeed = P1_Speed1_1x;

            P1_Car_State.isRanking_3th = true;
            P1_Car_State.isRanking_4th = false;
        }


        /// 1Player 가 4 등일 경우

        else if (Player1_traker.progressDistance < Player2_traker.progressDistance &&
                 Player1_traker.progressDistance < Player3_traker.progressDistance &&
                 Player1_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player1_Rank.text = "4 th";
            Four_Blank.text = UserManager.Instance().name;

            P1_Car_State.maxSpeed = P1_Speed1_2x;

            P1_Car_State.isRanking_3th = false;
            P1_Car_State.isRanking_4th = true;
        }
        ////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////2Player///////////////////////////////////////////
        /// 2Player 가 1등일 경우
        if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
            Player2_traker.progressDistance > Player3_traker.progressDistance &&
            Player2_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player2_Rank.text = "1 th";
            One_Blank.text = "말타지";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P2_Car_State.maxSpeed = P2_Speed1_Ex;

            P2_Car_State.isRanking_3th = false;
            P2_Car_State.isRanking_4th = false;
        }

        /// 2Player 가 2 등일경우
        else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                 Player2_traker.progressDistance > Player3_traker.progressDistance &&
                 Player2_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player2_Rank.text = "2 th";
            Two_Blank.text = "말타지";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P2_Car_State.maxSpeed = P2_Speed1_0x;

            P2_Car_State.isRanking_3th = false;
            P2_Car_State.isRanking_4th = false;
        }

        else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                 Player2_traker.progressDistance < Player3_traker.progressDistance &&
                 Player2_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player2_Rank.text = "2 th";
            Two_Blank.text = "말타지";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P2_Car_State.maxSpeed = P2_Speed1_0x;

            P2_Car_State.isRanking_3th = false;
            P2_Car_State.isRanking_4th = false;
        }

        else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                 Player2_traker.progressDistance > Player3_traker.progressDistance &&
                 Player2_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player2_Rank.text = "2 th";
            Two_Blank.text = "말타지";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P2_Car_State.maxSpeed = P2_Speed1_0x;

            P2_Car_State.isRanking_3th = false;
            P2_Car_State.isRanking_4th = false;
        }

        /// 2Player 가 3 등일경우
        else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                 Player2_traker.progressDistance < Player3_traker.progressDistance &&
                 Player2_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player2_Rank.text = "3 sT";
            Three_Blank.text = "말타지";

            P2_Car_State.maxSpeed = P2_Speed1_1x;

            P2_Car_State.isRanking_3th = true;
            P2_Car_State.isRanking_4th = false;
        }

        else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                 Player2_traker.progressDistance < Player3_traker.progressDistance &&
                 Player2_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player2_Rank.text = "3 th";
            Three_Blank.text = "말타지";

            P2_Car_State.maxSpeed = P2_Speed1_1x;

            P2_Car_State.isRanking_3th = true;
            P2_Car_State.isRanking_4th = false;
        }

        else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                 Player2_traker.progressDistance > Player3_traker.progressDistance &&
                 Player2_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player2_Rank.text = "3 sT";
            Three_Blank.text = "말타지";

            P2_Car_State.maxSpeed = P2_Speed1_1x;

            P2_Car_State.isRanking_3th = true;
            P2_Car_State.isRanking_4th = false;
        }


        /// 2Player 가 4 등일 경우

        else if (Player2_traker.progressDistance < Player1_traker.progressDistance &&
                 Player2_traker.progressDistance < Player3_traker.progressDistance &&
                 Player2_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player2_Rank.text = "4 th";
            Four_Blank.text = "말타지";

            P2_Car_State.maxSpeed = P2_Speed1_2x;

            P2_Car_State.isRanking_3th = false;
            P2_Car_State.isRanking_4th = true;
        }
        ////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////3Player///////////////////////////////////////////
        /// 3Player 가 1등일 경우
        if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
            Player3_traker.progressDistance > Player1_traker.progressDistance &&
            Player3_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player3_Rank.text = "1 th";
            One_Blank.text = "타랑";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P3_Car_State.maxSpeed = P3_Speed1_Ex;

            P3_Car_State.isRanking_3th = false;
            P3_Car_State.isRanking_4th = false;
        }

        /// 3Player 가 2 등일경우
        else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                 Player3_traker.progressDistance > Player1_traker.progressDistance &&
                 Player3_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player3_Rank.text = "2 th";
            Two_Blank.text = "타랑";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P3_Car_State.maxSpeed = P3_Speed1_0x;

            P3_Car_State.isRanking_3th = false;
            P3_Car_State.isRanking_4th = false;
        }

        else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                 Player3_traker.progressDistance < Player1_traker.progressDistance &&
                 Player3_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player3_Rank.text = "2 th";
            Two_Blank.text = "타랑";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P3_Car_State.maxSpeed = P3_Speed1_0x;

            P3_Car_State.isRanking_3th = false;
            P3_Car_State.isRanking_4th = false;
        }

        else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                 Player3_traker.progressDistance > Player1_traker.progressDistance &&
                 Player3_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player3_Rank.text = "2 th";
            Two_Blank.text = "타랑";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P3_Car_State.maxSpeed = P3_Speed1_0x;

            P3_Car_State.isRanking_3th = false;
            P3_Car_State.isRanking_4th = false;
        }

        /// 3Player 가 3 등일경우
        else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                 Player3_traker.progressDistance < Player1_traker.progressDistance &&
                 Player3_traker.progressDistance > Player4_traker.progressDistance)
        {
            Player3_Rank.text = "3 th";
            Three_Blank.text = "타랑";

            P3_Car_State.maxSpeed = P3_Speed1_1x;

            P3_Car_State.isRanking_3th = true;
            P3_Car_State.isRanking_4th = false;
        }

        else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                 Player3_traker.progressDistance < Player1_traker.progressDistance &&
                 Player3_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player3_Rank.text = "3 th";
            Three_Blank.text = "타랑";

            P3_Car_State.maxSpeed = P3_Speed1_1x;

            P3_Car_State.isRanking_3th = true;
            P3_Car_State.isRanking_4th = false;
        }

        else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                 Player3_traker.progressDistance > Player1_traker.progressDistance &&
                 Player3_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player3_Rank.text = "3 th";
            Three_Blank.text = "타랑";

            P3_Car_State.maxSpeed = P3_Speed1_1x;

            P3_Car_State.isRanking_3th = true;
            P3_Car_State.isRanking_4th = false;
        }


        /// 3Player 가 4 등일 경우

        else if (Player3_traker.progressDistance < Player2_traker.progressDistance &&
                 Player3_traker.progressDistance < Player1_traker.progressDistance &&
                 Player3_traker.progressDistance < Player4_traker.progressDistance)
        {
            Player3_Rank.text = "4 th";
            Four_Blank.text = "타랑";

            P3_Car_State.maxSpeed = P3_Speed1_2x;

            P3_Car_State.isRanking_3th = false;
            P3_Car_State.isRanking_4th = true;
        }
        ////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////4Player///////////////////////////////////////////
        /// 4Player 가 1등일 경우
        if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
            Player4_traker.progressDistance > Player3_traker.progressDistance &&
            Player4_traker.progressDistance > Player1_traker.progressDistance)
        {
            Player4_Rank.text = "1 th";
            One_Blank.text = "더헛";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P4_Car_State.maxSpeed = P4_Speed1_Ex;

            P4_Car_State.isRanking_3th = false;
            P4_Car_State.isRanking_4th = false;
        }

        /// 4Player 가 2 등일경우
        else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                 Player4_traker.progressDistance > Player3_traker.progressDistance &&
                 Player4_traker.progressDistance > Player1_traker.progressDistance)
        {
            Player4_Rank.text = "2 th";
            Two_Blank.text = "더헛";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P4_Car_State.maxSpeed = P4_Speed1_0x;

            P4_Car_State.isRanking_3th = false;
            P4_Car_State.isRanking_4th = false;
        }

        else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                 Player4_traker.progressDistance < Player3_traker.progressDistance &&
                 Player4_traker.progressDistance > Player1_traker.progressDistance)
        {
            Player4_Rank.text = "2 th";
            Two_Blank.text = "더헛";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P4_Car_State.maxSpeed = P4_Speed1_0x;

            P4_Car_State.isRanking_3th = false;
            P4_Car_State.isRanking_4th = false;
        }

        else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                 Player4_traker.progressDistance > Player3_traker.progressDistance &&
                 Player4_traker.progressDistance < Player1_traker.progressDistance)
        {
            Player4_Rank.text = "2 th";
            Two_Blank.text = "더헛";

            Destroy(F_Particle);
            Destroy(T_Particle);

            P4_Car_State.maxSpeed = P4_Speed1_0x;

            P4_Car_State.isRanking_3th = false;
            P4_Car_State.isRanking_4th = false;
        }

        /// 4Player 가 3 등일경우
        else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                 Player4_traker.progressDistance < Player3_traker.progressDistance &&
                 Player4_traker.progressDistance > Player1_traker.progressDistance)
        {
            Player4_Rank.text = "3 th";
            Three_Blank.text = "더헛";

            P4_Car_State.maxSpeed = P4_Speed1_1x;

            P4_Car_State.isRanking_3th = true;
            P4_Car_State.isRanking_4th = false;
        }

        else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                 Player4_traker.progressDistance < Player3_traker.progressDistance &&
                 Player4_traker.progressDistance < Player1_traker.progressDistance)
        {
            Player4_Rank.text = "3 th";
            Three_Blank.text = "더헛";

            P4_Car_State.maxSpeed = P4_Speed1_1x;

            P4_Car_State.isRanking_3th = true;
            P4_Car_State.isRanking_4th = false;
        }

        else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                 Player4_traker.progressDistance > Player3_traker.progressDistance &&
                 Player4_traker.progressDistance < Player1_traker.progressDistance)
        {
            Player4_Rank.text = "3 th";
            Three_Blank.text = "더헛";

            P4_Car_State.maxSpeed = P4_Speed1_1x;

            P4_Car_State.isRanking_3th = true;
            P4_Car_State.isRanking_4th = false;
        }


        /// 4Player 가 4 등일 경우

        else if (Player4_traker.progressDistance < Player2_traker.progressDistance &&
                 Player4_traker.progressDistance < Player3_traker.progressDistance &&
                 Player4_traker.progressDistance < Player1_traker.progressDistance)
        {
            Player4_Rank.text = "4 th";
            Four_Blank.text = "더헛";

            P4_Car_State.maxSpeed = P4_Speed1_2x;

            P4_Car_State.isRanking_3th = false;
            P4_Car_State.isRanking_4th = true;
        }
        ////////////////////////////////////////////////////////////////////////////////////

    }

    public void Stage2_Over()
    {

        if (Player1_traker.progressDistance > Player2_traker.progressDistance &&
            Player1_traker.progressDistance > Player3_traker.progressDistance &&
            Player1_traker.progressDistance > Player4_traker.progressDistance)
        {
            Winner_Text.text = "Winner Player 1";
            Player1_Win = true;
            Player2_Win = false;
            Player3_Win = false;
            Player4_Win = false;

            Player1_traker.progressDistance = Player1_traker.progressDistance + 10000.0f;

            StartCoroutine(Stage_Over_Stop());
        }

        else if (Player2_traker.progressDistance > Player1_traker.progressDistance &&
                 Player2_traker.progressDistance > Player3_traker.progressDistance &&
                 Player2_traker.progressDistance > Player4_traker.progressDistance)
        {
            Winner_Text.text = "Winner Player 2";
            Player1_Win = false;
            Player2_Win = true;
            Player3_Win = false;
            Player4_Win = false;

            Player2_traker.progressDistance = Player2_traker.progressDistance + 10000.0f;

            StartCoroutine(Stage_Over_Stop());
        }

        else if (Player3_traker.progressDistance > Player2_traker.progressDistance &&
                 Player3_traker.progressDistance > Player1_traker.progressDistance &&
                 Player3_traker.progressDistance > Player4_traker.progressDistance)
        {
            Winner_Text.text = "Winner Player 3";
            Player1_Win = false;
            Player2_Win = false;
            Player3_Win = true;
            Player4_Win = false;

            Player3_traker.progressDistance = Player3_traker.progressDistance + 10000.0f;

            StartCoroutine(Stage_Over_Stop());
        }
        else if (Player4_traker.progressDistance > Player2_traker.progressDistance &&
                 Player4_traker.progressDistance > Player3_traker.progressDistance &&
                 Player4_traker.progressDistance > Player1_traker.progressDistance)
        {
            Winner_Text.text = "Winner Player 4";
            Player1_Win = false;
            Player2_Win = false;
            Player3_Win = false;
            Player4_Win = true;

            Player4_traker.progressDistance = Player4_traker.progressDistance + 10000.0f;

            StartCoroutine(Stage_Over_Stop());
        }
    }

    IEnumerator Stage_Over_Stop()
    {
        if (Over_Counter_Fix == true)
        {
            Over_Counter_Fix = false;

            Over_Count.text = "10";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "9";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "8";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "7";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "6";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "5";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "4";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "3";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "2";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "1";

            yield return new WaitForSeconds(1.0f);

            Over_Count.text = "";

            Time.timeScale = 0.0f;
            GameFIN.GameFin = true;
        }  
    }

}
