using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;


public class UserManager : MonoBehaviour {


    public enum AlbaType
    {
        None,
        Kid,
        Jang,
        Quick,
        Repair,
        Apart
    }

    public enum PlantType
    {
        None,
        Kangbuk,
        Kangnam,
        Cheonho
    }

  //  public bool first = false;
    public UISprite rewardIcon;
    public UILabel rewardAmount;
    public UILabel rewardGold;
    public UILabel nextRaceLabel;
    public GameObject ingLabel;

    public int raceReward = 0;
    // 아르바이트 랜덤

    AlbaType albaType = AlbaType.None;
    PlantType plantType = PlantType.None;

    static UserManager _instance = null;

    public DateTime nowDate;
    public int myGold;
    public int stamina;
    public new string name;

    csWork.Work nowWork;

    public UILabel nowYearLabel;
    public UILabel nowMonthLabel;
    public UILabel nowDayLabel;

    public UILabel creatureLabel;
    public UILabel inputLabel;
    public UILabel invenLabel;
    
    public UILabel goldLabel;

    public UILabel nowLabel;
    public UILabel maxLabel;

    public UISprite yesterDayBackground;
    public UISprite yesterDaySprite;
    public UISprite tomorrowDayBackground;
    public UISprite tomorrowDaySprite;
    
    public CalenderManager c_manager;
    public csInventory inven;

    public csStamina s_manager;
    public UISlider workSlider;

    public UISprite WorkSprite;
    public GameObject TouchSreen;
    public GameObject WorkRateObj;
    public GameObject possibleProgress;
    public GameObject WorkEnd;
    public GameObject raceNotice;
    public GameObject creature;
    public GameObject GameOver;

    // 나중에 읽어와서 초기화
    csWork.Work yesterDayWork = csWork.Work.Nowork;

    int progressCount = 0;
    bool isRise = false;
    float riseTime = 0;
    float startValue = 0;
    float endValue = 0;


    // 각 작업의 소요일수
    int KangNamDay = 1;
    int KangBukDay = 1;
    int AlbaDay = 1;
    int CheonHoDay = 1;
    int LaborDay = 2;
    int PlantDay = 1;
    int RepairDay = 1;
    int RestDay = 1;

    //현재 하고있는 작업의 총 소요일수
    int nowWorkDay = 0;

    // 현재 하고 있는 작업의 소요일수
    int WorkCount = 0;

    public static UserManager Instance()
    {
        return _instance;
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        string isLoginOnce = PlayerPrefs.GetString("_isLoginOnce");

        int year = 0;
        int month = 0;
        int day = 0;

          if (isLoginOnce != "True")
          {
              PlayerPrefs.SetString("_isLoginOnce", "True");

              PlayerPrefs.SetString("Name","이름을 정해 주세요");
              PlayerPrefs.SetInt("Stamina", 100);
              PlayerPrefs.SetInt("MyGold", 0);
              PlayerPrefs.SetInt("Year", 2040);
              PlayerPrefs.SetInt("Month", 1);
              PlayerPrefs.SetInt("Day", 1);
              PlayerPrefs.SetInt("ProgressCount",0);

             // 리더보드 관련 변수
              PlayerPrefs.SetInt("MixCount", 0);

              name = PlayerPrefs.GetString("Name");
              stamina = PlayerPrefs.GetInt("Stamina");
              myGold = PlayerPrefs.GetInt("MyGold");
              year = PlayerPrefs.GetInt("Year");
              month = PlayerPrefs.GetInt("Month");
              day = PlayerPrefs.GetInt("Day");
              progressCount =  PlayerPrefs.GetInt("ProgressCount");
              nowDate = new DateTime(year, month, day);
          }

          if (isLoginOnce == "True")
          {
              name = PlayerPrefs.GetString("Name");
              stamina = PlayerPrefs.GetInt("Stamina");
              myGold = PlayerPrefs.GetInt("MyGold");
              year = PlayerPrefs.GetInt("Year");
              month = PlayerPrefs.GetInt("Month");
              day = PlayerPrefs.GetInt("Day");
              nowDate = new DateTime(year, month, day);
              progressCount = PlayerPrefs.GetInt("ProgressCount");
          }
     
        creatureLabel.text = name;
        invenLabel.text = name;
        inputLabel.text = name;

      //  prefsInit();

        DateLabelUpdate();
        StartCoroutine(Init());
    }

    public void userInfoSave()
    {      
        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.SetInt("Stamina", stamina);
        PlayerPrefs.SetInt("MyGold", myGold);
        PlayerPrefs.SetInt("Year", nowDate.Year);
        PlayerPrefs.SetInt("Month", nowDate.Month);
        PlayerPrefs.SetInt("Day", nowDate.Day);

        // 리더보드 최대 골드 갱신
        Money_LeaderBoard();
    }


    public void prefsInit()
    {
        //PlayerPrefs.SetString("_isLoginOnce", "True");

        Debug.Log("초기초기화");
        PlayerPrefs.SetString("Name", "이름을 정해 주세요");
        PlayerPrefs.SetInt("Stamina", 100);
        PlayerPrefs.SetInt("MyGold", 0);
        PlayerPrefs.SetInt("Year", 2040);
        PlayerPrefs.SetInt("Month", 1);
        PlayerPrefs.SetInt("Day", 1);
        PlayerPrefs.SetInt("ProgressCount", 0);
    }
    void Start ()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();
    }

    // 리더보드 최대 소지 골드 업데이트
    public void Money_LeaderBoard()
    {
        string leader_board_id = "CgkI9ZbZnpIHEAIQBw";
#if UNITY_ANDROID
        leader_board_id = "CgkI9ZbZnpIHEAIQBw";
#elif UNITY_IPHONE
        leader_board_id = "SOCIALTEST_HIGH_SCORE";
#endif

        Social.ReportScore(myGold, leader_board_id,
            (bool success) =>
            {
#if UNITY_ANDROID
                PlayGamesPlatform.Instance.ShowLeaderboardUI(leader_board_id);
#elif UNITY_IPHONE
                Social.ShowLeaderboardUI();
#endif
            }
        );
    }
    // 리더보드 조합 횟수 업데이트
    public void Mix_LeaderBoard()
    {
        string leader_board_id = "CgkI9ZbZnpIHEAIQCg";
#if UNITY_ANDROID
        leader_board_id = "CgkI9ZbZnpIHEAIQCg";
#elif UNITY_IPHONE
        leader_board_id = "SOCIALTEST_HIGH_SCORE";
#endif

        Social.ReportScore(PlayerPrefs.GetInt("MixCount"), leader_board_id,
            (bool success) =>
            {
#if UNITY_ANDROID
                PlayGamesPlatform.Instance.ShowLeaderboardUI(leader_board_id);
#elif UNITY_IPHONE
                Social.ShowLeaderboardUI();
#endif
            }
        );
    }
    // 랜덤박스 구입 업적
    public void AchievementRandomBox()
    {
        string unLockAchievement_id = "CgkI9ZbZnpIHEAIQAg";

        Social.ReportProgress(unLockAchievement_id, 100.0f, (bool success) =>
        { });
    }
    // 판매 처음 이용 업적
    public void AchievementFirstSell()
    {
        string unLockAchievement_id = "CgkI9ZbZnpIHEAIQAw";

        Social.ReportProgress(unLockAchievement_id, 100.0f, (bool success) =>
        { });
    }

    // 첫 스케줄 시작 업적
    public void AchievementFirstSchedule()
    {
        string unLockAchievement_id = "CgkI9ZbZnpIHEAIQBA";

        Social.ReportProgress(unLockAchievement_id, 100.0f, (bool success) =>
        { });
    }

    // 첫 승리 업적
    public void AchievementFirstVictory()
    {
        string unLockAchievement_id = "CgkI9ZbZnpIHEAIQBQ";

        Social.ReportProgress(unLockAchievement_id, 100.0f, (bool success) =>
        { });
    }

    public void raceUpdate()
    {
         myGold += raceReward;
         Debug.Log("내골드" + raceReward);
         raceReward = 0;
         goldLabel.text = myGold.ToString();

         userInfoSave();
    }

    public void continueWork()
    {
        Debug.Log("코루틴워크 불른다");
        if (PlayerPrefs.GetInt("ProgressCount") != 0)
        {
            CreatureFalse();
            Debug.Log("레이스 끝난후 일정 다시22");
            Debug.Log(nowDate);
           
            Debug.Log(nowDate);
            userInfoSave();
            Progress();
        }
    }

    void Update () {

        //Money_LeaderBoard();

        if (isRise)
        {
            riseTime += Time.deltaTime / 1.5f;
            workSlider.value =  Mathf.Lerp(startValue, endValue, riseTime);
            if (workSlider.value > endValue - 0.01)
            {
                workSlider.value = endValue;
                isRise = false;
                riseTime = 0;

                // 여기에 보상날이면 보상창 뛰우고 터치화면내보내기 추가해야함

                bool isreward = false;
                if (nowDate.Year == 2040)
                {
                    if(c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1].startIndex].isLastWork == 1)
                    {
                        isreward = true;
                        StartCoroutine(reward());
                    }
                    
                }
                //41년
                else
                {
                    if(c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].isLastWork == 1)
                    {
                        isreward = true;
                        StartCoroutine(reward());
                    }
                    
                }
                if (!isreward)
                {
                    StartCoroutine(DayEnd());
                }
            }
        }      
	}

    IEnumerator reward()
    {
        if (nowWork == csWork.Work.Race)
            yield break;

        rewardPay();
        
        yield return new WaitForSeconds(1.0f);
        workSlider.value = 0;
        WorkCount = 0;
        WorkRateObj.GetComponent<TweenScale>().Toggle();

       // WorkRateObj.SetActive(false);
        WorkEnd.SetActive(true);

        WorkEnd.GetComponent<TweenScale>().ResetToBeginning();
        WorkEnd.GetComponent<TweenScale>().PlayForward();

        StartCoroutine(DayEnd());
    }

    void rewardPay()
    {
        switch (nowWork)
        {
            case csWork.Work.Nowork:
                break;
            case csWork.Work.Kangnam:
                int kangnamRandom = UnityEngine.Random.Range(1, 101);

                // 낡은 회로 8개 15%
                if(kangnamRandom <= 15)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 8);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                    rewardAmount.text = 8.ToString();
                }
                // 일반 회로 4개 15%
                else if(15 < kangnamRandom && 30>= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 4);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                    rewardAmount.text = 4.ToString();
                }
                // 고급 회로 2개 3%
                else if (30 < kangnamRandom && 33 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 불순물섞인 금속 7개 15%
                else if (33 < kangnamRandom && 48 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 7);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                    rewardAmount.text = 7.ToString();
                }
                // 보통 금속 4개 15%
                else if (48 < kangnamRandom && 63 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 4);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                    rewardAmount.text = 4.ToString();
                }
                // 정제된 금속 2개 3%
                else if (63 < kangnamRandom && 66 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 녹슨 잡동사니 5개 15%
                else if (66 < kangnamRandom && 81 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 5);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                    rewardAmount.text = 5.ToString();
                }
                // 잡동사니 4개 15%
                else if (81 < kangnamRandom && 96 >= kangnamRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 4);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                    rewardAmount.text = 4.ToString();
                }
                // 고급 잡동사니 2개 3%
                else
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                break;
            case csWork.Work.Kangbuk:
                int kangbukRandom = UnityEngine.Random.Range(1, 101);

                // 낡은 회로 2개 20%
                if (kangbukRandom <= 20)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 일반 회로 3개 10%
                else if (20 < kangbukRandom && 30 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                // 고급 회로 2개 3%
                else if (30 < kangbukRandom && 33 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 불순물섞인 금속 1개 20%
                else if (33 < kangbukRandom && 53 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                // 보통 금속 3개 10%
                else if (53 < kangbukRandom && 63 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                // 정제된 금속 2개 3%
                else if (63 < kangbukRandom && 66 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 녹슨 잡동사니 2개 20%
                else if (66 < kangbukRandom && 86 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 잡동사니 3개 10%
                else if (86 < kangbukRandom && 96 >= kangbukRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                // 고급 잡동사니 3개 3%
                else
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                break;
            case csWork.Work.Alba:
                if(albaType == AlbaType.Kid)
                {
                    myGold += 50;
                    rewardGold.text = 50.ToString();
                }
                else if(albaType == AlbaType.Jang)
                {
                    myGold += 75;
                    rewardGold.text = 75.ToString();
                }
                else if (albaType == AlbaType.Quick)
                {
                    int random = UnityEngine.Random.Range(1, 101);

                    //  100 골드 60%
                    if (random <= 60)
                    {
                        myGold += 100;
                        rewardGold.text = 100.ToString();
                    }
                    // 낡은 회로 1개 15%
                    else if (60 < random && 75 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 낡은 회로 2개 10%
                    else if (75 < random && 85 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 낡은 회로 3개 5%
                    else if (85 < random && 90 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 3);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 3.ToString();
                    }
                    // 일반 회로 1개 7%
                    else if (90 < random && 97 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 일반 회로 2개 3%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                }
                else if (albaType == AlbaType.Repair)
                {
                    int random = UnityEngine.Random.Range(1, 101);

                    //  120 골드 50%
                    if (random <= 50)
                    {
                        myGold += 120;
                        rewardGold.text = 120.ToString();
                    }
                    // 보통 금속 1개 5%
                    else if (50 < random && 55 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 정제된 금속 1개 5%
                    else if (55 < random && 60 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 일반 회로 1개 25%
                    else if (60 < random && 85 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 일반 회로 2개 5%
                    else if (85 < random && 90 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 고급 회로 1개 5%
                    else if (90 < random && 95 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 잡동사니 1개 5%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                }
                else if (albaType == AlbaType.Apart)
                {
                    int random = UnityEngine.Random.Range(1, 101);

                    //  150 골드 60%
                    if (random <= 60)
                    {
                        myGold += 150;
                        rewardGold.text = 150.ToString();
                    }
                    // 잡동사니 1개 13%
                    else if (60 < random && 73 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 잡동사니 2개 5%
                    else if (73 < random && 78 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 보통 금속 1개 10%
                    else if (78 < random && 88 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 보통 금속 2개 5%
                    else if (88 < random && 93 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 정제된 금속 1개 5%
                    else if (93 < random && 98 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 정제된 금속 2개 2%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                }
                break;

            case csWork.Work.CheonHo:
                int cheonhoRandom = UnityEngine.Random.Range(1, 101);

                // 낡은 회로 4개 15%
                if (cheonhoRandom <= 15)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 4);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                    rewardAmount.text = 4.ToString();
                }
                // 일반 회로 2개 15%
                else if (15 < cheonhoRandom && 30 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 2);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                    rewardAmount.text = 2.ToString();
                }
                // 고급 회로 1개 3%
                else if (30 < cheonhoRandom && 33 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                // 불순물섞인 금속 5개 15%
                else if (33 < cheonhoRandom && 48 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 5);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                    rewardAmount.text = 5.ToString();
                }
                // 보통 금속 3개 15%
                else if (48 < cheonhoRandom && 63 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                // 정제된 금속 1개 3%
                else if (63 < cheonhoRandom && 66 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                // 녹슨 잡동사니 5개 15%
                else if (66 < cheonhoRandom && 81 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 5);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                    rewardAmount.text = 5.ToString();
                }
                // 잡동사니 3개 15%
                else if (81 < cheonhoRandom && 96 >= cheonhoRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 3);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                    rewardAmount.text = 3.ToString();
                }
                // 고급 잡동사니 1개 3%
                else
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                break;

            case csWork.Work.Labor:
                myGold += 500;
                rewardGold.text = 500.ToString();
                break;
            case csWork.Work.Plant:
                if (plantType == PlantType.Kangbuk)
                {
                    myGold += 80;
                    rewardGold.text = 80.ToString();
                    int random = UnityEngine.Random.Range(1, 101);

                    // 낡은 회로 2개 20%
                    if (random <= 20)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 일반 회로 1개 10%
                    else if (20 < random && 30 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 회로 1개 3%
                    else if (30 < random && 33 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 불순물섞인 금속 2개 20%
                    else if (33 < random && 53 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 보통 금속 1개 10%
                    else if (53 < random && 63 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 정제된 금속 1개 3%
                    else if (63 < random && 66 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 녹슨 잡동사니 2개 20%
                    else if (66 < random && 86 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 잡동사니 1개 10%
                    else if (86 < random && 96 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 잡동사니 1개 3%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                }
                else if (plantType == PlantType.Cheonho)
                {
                    myGold += 120;
                    rewardGold.text = 120.ToString();
                    int random = UnityEngine.Random.Range(1, 101);
                    // 낡은 회로 2개 15%
                    if (random <= 15)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 일반 회로 1개 15%
                    else if (15 < random && 30 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 회로 1개 3%
                    else if (30 < random && 33 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 불순물섞인 금속 2개 15%
                    else if (33 < random && 48 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 보통 금속 1개 15%
                    else if (53 < random && 63 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 정제된 금속 1개 3%
                    else if (63 < random && 66 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 녹슨 잡동사니 2개 15%
                    else if (66 < random && 81 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 잡동사니 1개 15%
                    else if (81 < random && 96 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 잡동사니 1개 3%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                }
                else if (plantType == PlantType.Kangnam)
                {
                    myGold += 180;
                    rewardGold.text = 180.ToString();
                    // 위 천호랑똑같음
                    int random = UnityEngine.Random.Range(1, 101);
                    // 낡은 회로 2개 15%
                    if (random <= 15)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[3], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 일반 회로 1개 15%
                    else if (15 < random && 30 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[4], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 회로 1개 3%
                    else if (30 < random && 33 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 불순물섞인 금속 2개 15%
                    else if (33 < random && 48 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[0], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 보통 금속 1개 15%
                    else if (53 < random && 63 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[1], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 정제된 금속 1개 3%
                    else if (63 < random && 66 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 녹슨 잡동사니 2개 15%
                    else if (66 < random && 81 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[6], 2);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                        rewardAmount.text = 2.ToString();
                    }
                    // 잡동사니 1개 15%
                    else if (81 < random && 96 >= random)
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[7], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                    // 고급 잡동사니 1개 3%
                    else
                    {
                        inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                        rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                        rewardAmount.text = 1.ToString();
                    }
                }
                break;
            case csWork.Work.Repair:
                myGold += 100;
                rewardGold.text = 100.ToString();
                int repairRandom = UnityEngine.Random.Range(1, 101);

                // 낡은 회로 11%
                if (repairRandom <= 11)
                {
                    int amount = UnityEngine.Random.Range(1, 4);

                    inven.AddItem(csItemDataBase.Instance().MaterialDB[3], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[3].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 일반 회로 11%
                else if (11 < repairRandom && 22 >= repairRandom)
                {
                    int amount = UnityEngine.Random.Range(1, 3);
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[4], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[4].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 고급 회로 11%
                else if (22 < repairRandom && 33 >= repairRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[5], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[5].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                // 불순물섞인 금속 11%
                else if (33 < repairRandom && 44 >= repairRandom)
                {
                    int amount = UnityEngine.Random.Range(1, 4);
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[0], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[0].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 보통 금속  11%
                else if (44 < repairRandom && 55 >= repairRandom)
                {
                    int amount = UnityEngine.Random.Range(1, 3);
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[1], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[1].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 정제된 금속 11%
                else if (55 < repairRandom && 66 >= repairRandom)
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[2], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[2].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                // 녹슨 잡동사니 11%
                else if (66 < repairRandom && 77 >= repairRandom)
                {
                    int amount = UnityEngine.Random.Range(1, 4);
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[6], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[6].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 잡동사니 11%
                else if (77 < repairRandom && 88 >= repairRandom)
                {
                    int amount = UnityEngine.Random.Range(1, 3);
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[7], amount);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[7].itemSpriteName;
                    rewardAmount.text = amount.ToString();
                }
                // 고급 잡동사니 11%
                else
                {
                    inven.AddItem(csItemDataBase.Instance().MaterialDB[8], 1);
                    rewardIcon.spriteName = csItemDataBase.Instance().MaterialDB[8].itemSpriteName;
                    rewardAmount.text = 1.ToString();
                }
                break;
            case csWork.Work.Rest:
                break;
        }
    }

    public void IconActiveTrue()
    {
        if (nowWork == csWork.Work.Race)
        {
            raceNotice.SetActive(true);
            PlayerPrefs.SetInt("ProgressCount", 0);
            //UserManager.Instance().userInfoSave();
            //  raceNotice.GetComponent<TweenScale>().ResetToBeginning();
            //raceNotice.GetComponent<TweenScale>().PlayForward();
        }
        else
        {
            WorkRateObj.SetActive(true);
            WorkRateObj.GetComponent<TweenScale>().ResetToBeginning();
            WorkRateObj.GetComponent<TweenScale>().PlayForward();
            Debug.Log("플레이");
        }
        
       
    }

    public void IconActiveFalse()
    {
        WorkRateObj.SetActive(false);
    }

    // 아무키나 눌러주세요 안보이는 버튼 트루
    IEnumerator DayEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("데이엔드");
        TouchSreen.SetActive(true);        
    }
    public void NextDay()
    {
        // 오늘날짜로 업데이트 - 소지골지도 업데이트
        DateLabelUpdate();
        Debug.Log("넥스트데이");
        WorkUpdate();
        TouchSreen.SetActive(false);
        WorkEnd.SetActive(false);
        ingLabel.SetActive(true);
        rewardGold.text = "0";
        rewardIcon.spriteName = string.Empty;
        rewardAmount.text = string.Empty;
    }

    public void DateLabelUpdate()
    {
        string strDate = nowDate.ToString("yyyy-MM-dd");

        nowYearLabel.text = strDate.Substring(0, 4);
        nowMonthLabel.text = strDate.Substring(5, 2);
        nowDayLabel.text = strDate.Substring(8, 2);

        goldLabel.text = myGold.ToString();
       
        userInfoSave();
        StartCoroutine(nextRaceDay());
    }
     
    IEnumerator nextRaceDay()
    {
        yield return new WaitForSeconds(0.2f);
        csWork.Work tmpWork;
        for (int i = 0; i < 7; i++)
        {
            if (nowDate.Year == 2040)
            {
                if ((nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1].startIndex + i) < 42)
                {
                    tmpWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1].startIndex + i].myWork;
                }
                else
                {
                    tmpWork = c_manager.MyMonths[nowDate.Month].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month].startIndex + i].myWork;
                }
                if (tmpWork == csWork.Work.Race)
                {
                    nextRaceLabel.text  = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1].startIndex + i].dt.Day.ToString();
                    Debug.Log("찾음");
                    break;
                }
            }
            else
            {
                
                if (nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex + i < 42)
                {
                    tmpWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex + i].myWork;
                }
                else
                {
                    tmpWork = c_manager.MyMonths[nowDate.Month + 12].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month + 12].startIndex + i].myWork;
                }

                if (tmpWork == csWork.Work.Race)
                {
                    nextRaceLabel.text = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex + i].dt.Day.ToString();
                    break;
                }
            }
        }
    }
    
    public void WorkUpdate()
    {
        csWork.Work yesterWork;
        csWork.Work tomorrowWork;

        bool isYesterLastWork = false;
        bool isTomorrowLastWork = false;

        if (nowDate.Year == 2040)
        {
            // 2040-1-1 이면
            //어제일
            if (nowDate.Year == 2040 && nowDate.Month == 1 && nowDate.Day == 1)
            {
                yesterWork = csWork.Work.Nowork;
            }
            else
            {
                yesterWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1].startIndex].myWork;
                if(c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1].startIndex].isLastWork == 1)
                {
                    isYesterLastWork = true;
                }
            }
            //내일일

            tomorrowWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day + c_manager.MyMonths[nowDate.Month - 1].startIndex].myWork;

            if(c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day + c_manager.MyMonths[nowDate.Month - 1].startIndex].isLastWork == 1)
            {
                isTomorrowLastWork = true;
            }
        }
        //41년
        else
        {
            yesterWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].myWork;

            if(c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].isLastWork == 1)
            {
                isYesterLastWork = true;
            }

            tomorrowWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].myWork;

            if(c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].isLastWork == 1)
            {
                isTomorrowLastWork = true;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 어제 일
        if (yesterWork == csWork.Work.Kangnam)
        {
            yesterDaySprite.spriteName = "work_kangnam";
        }
        else if (yesterWork == csWork.Work.Kangbuk)
        {
            yesterDaySprite.spriteName = "work_kangbuk";
        }
        else if (yesterWork == csWork.Work.Alba)
        {       
            yesterDaySprite.spriteName = "work_alba";
        }
        else if(yesterWork == csWork.Work.Nowork)
        {
            yesterDaySprite.spriteName = "";
        }
        else if (yesterWork == csWork.Work.CheonHo)
        {
            yesterDaySprite.spriteName = "work_cheonho";
        }
        else if (yesterWork == csWork.Work.Labor)
        {
            yesterDaySprite.spriteName = "work_labor";
        }
        else if (yesterWork == csWork.Work.Plant)
        {
            yesterDaySprite.spriteName = "work_plant";
        }
        else if (yesterWork == csWork.Work.Repair)
        {
            yesterDaySprite.spriteName = "work_repair";
        }
        else if (yesterWork == csWork.Work.Rest)
        {
            yesterDaySprite.spriteName = "work_rest";
        }
        else if (yesterWork == csWork.Work.Race)
        {
            yesterDaySprite.spriteName = "RaceFlag";
        }

        // 내일
        if (tomorrowWork == csWork.Work.Kangnam)
        {
            tomorrowDaySprite.spriteName = "work_kangnam";
        }
        else if (tomorrowWork == csWork.Work.Kangbuk)
        {
            tomorrowDaySprite.spriteName = "work_kangbuk";
        }
        else if (tomorrowWork == csWork.Work.Alba)
        {
            tomorrowDaySprite.spriteName = "work_alba";
        }
        else if (tomorrowWork == csWork.Work.Nowork)
        {
            tomorrowDaySprite.spriteName = string.Empty;
        }
        else if (tomorrowWork == csWork.Work.CheonHo)
        {
            tomorrowDaySprite.spriteName = "work_cheonho";
        }
        else if (tomorrowWork == csWork.Work.Labor)
        {
            tomorrowDaySprite.spriteName = "work_labor";
        }
        else if (tomorrowWork == csWork.Work.Plant)
        {
            tomorrowDaySprite.spriteName = "work_plant";
        }
        else if (tomorrowWork == csWork.Work.Repair)
        {
            tomorrowDaySprite.spriteName = "work_repair";
        }
        else if (tomorrowWork == csWork.Work.Rest)
        {
            tomorrowDaySprite.spriteName = "work_rest";
        }
        else if (tomorrowWork == csWork.Work.Race)
        {
            Debug.Log("낼은레이스");
            tomorrowDaySprite.spriteName = "RaceFlag";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (isYesterLastWork)
        {
            yesterDayBackground.spriteName = "YesterToday_1";
        }
        else
        {
            yesterDayBackground.spriteName = "YesterToday_2";
        }

        if (isTomorrowLastWork)
        {
            tomorrowDayBackground.spriteName = "YesterToday_1";
        }
        else
        {
            tomorrowDayBackground.spriteName = "YesterToday_2";
        }
    }
    
    public void CreatureFalse()
    {
        creature.SetActive(false);
    }
    public void CreatureTrue()
    {
        creature.SetActive(true);
    }

    public void Progress()
    {
        Debug.Log("진행");
        //일주일동안 일정 진행
        Debug.Log("프로그래스카운트 " + PlayerPrefs.GetInt("ProgressCount"));
        if (PlayerPrefs.GetInt("ProgressCount") < 7)
        {
            int tmp = PlayerPrefs.GetInt("ProgressCount");
            PlayerPrefs.SetInt("ProgressCount", tmp + 1);
            nowDate = nowDate.AddDays(1);
            userInfoSave();


            possibleProgress.GetComponent<TweenPosition>().Toggle();
            possibleProgress.GetComponent<TweenAlpha>().Toggle();
            StartCoroutine(co_Progress());     
        }
        //일주일 일정 완료
        else
        {
            CreatureTrue();
            Debug.Log("일정완료");
            PlayerPrefs.SetInt("ProgressCount", 0);
        }
    }

    public IEnumerator co_Progress()
    {
        yield return new WaitForSeconds(1.0f);
        Working();
    }
    

    public void Working()
    {
        if (nowDate.Year == 2040)
        {
            nowWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1].startIndex].myWork;

            Debug.Log("오늘작업 : " + nowWork);
            if (nowDate.Year == 2040 && nowDate.Month == 1 && nowDate.Day == 2)
            {
                yesterDayWork = csWork.Work.Nowork;
            }
            else
            {
                yesterDayWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 3 + c_manager.MyMonths[nowDate.Month - 1].startIndex].myWork;
            }

        }
        //41년
        else
        {
            nowWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].myWork;
            yesterDayWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 3 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].myWork;
        }

        Debug.Log("어제일" + yesterDayWork);
        Debug.Log("오늘일" + nowWork);
        // 어제일과 오늘일이 다르면
        WorkCheck();
        if (yesterDayWork != nowWork)
        {
            WorkCheck();

            startValue = 0;
            endValue = 0;
            workSlider.value = 0;

            //maxLabel.text = nowWorkDay.ToString();
            WorkCount = 0;
        }

        // 오늘할일 실행
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        switch (nowWork)
        {
            case csWork.Work.Nowork:
                Debug.Log("할일없다");
                Debug.Log(stamina);
                WorkCount = 0;
                workSlider.value = 0;

                stamina = stamina + 10;
                if (stamina > 100)
                {
                    stamina = 100;
                }
                break;

            case csWork.Work.Kangnam:
                Debug.Log("강남");
                stamina = stamina - 12;
                nowWorkDay = KangNamDay;
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Kangbuk:
                stamina = stamina - 12;
                nowWorkDay = KangBukDay;
                Debug.Log("강북");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Alba:
                stamina = stamina - 11;
                nowWorkDay = AlbaDay;
                Debug.Log("알바");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.CheonHo:
                stamina = stamina - 12;
                nowWorkDay = CheonHoDay;
                Debug.Log("천호");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Labor:
                stamina = stamina - 12;
                nowWorkDay = LaborDay;
                Debug.Log("막노동");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Plant:
                stamina = stamina - 15;
                nowWorkDay = PlantDay;
                Debug.Log("공장");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Repair:
                stamina = stamina - 8;
                nowWorkDay = RepairDay;
                Debug.Log("공장");
                if (stamina <= 0)
                {
                    stamina = 0;
                }
                break;
            case csWork.Work.Rest:
                stamina = stamina + 25;
                nowWorkDay = RestDay;
                Debug.Log("휴식");
                if (stamina > 100)
                {
                    stamina = 100;
                }
                break;
            case csWork.Work.Race:
                nowWorkDay = RestDay;
                Debug.Log("레이스");
                break;
        }

        WorkCount++;
        nowLabel.text = WorkCount.ToString();
        maxLabel.text = nowWorkDay.ToString();
        // 현재 스태미나 갱신
        s_manager.checkStamina();
    }

    public void NameChange()
    {
        PlayerPrefs.SetString("Name", inputLabel.text);
        name = inputLabel.text;
        creatureLabel.text = name;
        invenLabel.text = name;
    }

   IEnumerator Init()
    {
        yield return new WaitForSeconds(0.2f);
        WorkUpdate();
        PersentageUpdate();
    }

    // 시작하면 현재작업과 진행율 로드
    public void PersentageUpdate()
    {
        Debug.Log("워크프로세스" + PlayerPrefs.GetInt("ProgressCount"));
        if (nowDate.Year == 2040)
        {
            nowWork = c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1].startIndex].myWork;
            Debug.Log("퍼센티지 오늘작업 : " + nowWork);
        }
        else
        {
            nowWork = c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 1 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].myWork;
        }
        
        WorkCheck();  
        int count = 0;
        if (nowWork != csWork.Work.Nowork)
        {
            DateTime tmp = nowDate;
            while(true)
            {
                
                if (tmp.Year == 2040)
                {
                    if(c_manager.MyMonths[tmp.Month - 1].MyDays[tmp.Day - 1 + c_manager.MyMonths[tmp.Month - 1].startIndex].isLastWork == 1)
                    {
                        break;
                    }
                }
                else
                {
                    if(c_manager.MyMonths[tmp.Month - 1 + 12].MyDays[tmp.Day - 1 + c_manager.MyMonths[tmp.Month - 1 + 12].startIndex].isLastWork == 1)
                    {
                        break;
                    }
                }
                tmp = tmp.AddDays(1);
                count++;
            }
        }
        WorkCount = nowWorkDay - count - 1;
        nowLabel.text = WorkCount.ToString();
               
        workSlider.value = ((float)WorkCount / nowWorkDay);
        maxLabel.text = nowWorkDay.ToString();
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void WorkCheck()
    {
        if (nowWork == csWork.Work.Nowork)
        {
            nowWorkDay = 1;
            WorkSprite.spriteName = "No_NoWork";
            
        }
        else if (nowWork == csWork.Work.Kangnam)
        {
            nowWorkDay = KangNamDay;
            WorkSprite.spriteName = "Progress_kangnam";

        }
        else if (nowWork == csWork.Work.Kangbuk)
        {
            nowWorkDay = KangBukDay;
            WorkSprite.spriteName = "Progress_kangbuk";
            Debug.Log("체인지해야대");
            Debug.Log(nowWorkDay);
        }
        else if (nowWork == csWork.Work.Alba)
        {
            nowWorkDay = AlbaDay;

            int albaRandom = UnityEngine.Random.Range(1, 101);

            Debug.Log("심부름");
            // 아이 대신 돌보기 45%
            if (1 <= albaRandom && 45 >= albaRandom)
            {
                albaType = AlbaType.Kid;
                WorkSprite.spriteName = "Progress_kid";
            }
            // 대신 장봐오기 35%
            else if (45 < albaRandom && 80 >= albaRandom)
            {
                albaType = AlbaType.Jang;
                WorkSprite.spriteName = "Progress_jang";
            }
            // 퀵서비스 10%
            else if (80 < albaRandom && 90 >= albaRandom)
            {
                albaType = AlbaType.Quick;
                WorkSprite.spriteName = "Progress_Quick";
            }
            // 긴급 크리쳐 수리 6%
            else if (90 < albaRandom && 96 >= albaRandom)
            {
                albaType = AlbaType.Repair;
                WorkSprite.spriteName = "Progress_repair2";
            }
            // 아파트 분리수거 4%
            else
            {
                albaType = AlbaType.Apart;
                WorkSprite.spriteName = "Progress_apart";
            }
            Debug.Log(albaType);
        }
        else if (nowWork == csWork.Work.CheonHo)
        {
            nowWorkDay = CheonHoDay;
            WorkSprite.spriteName = "Progress_cheonho";
        }
        else if (nowWork == csWork.Work.Labor)
        {
            nowWorkDay = LaborDay;
            WorkSprite.spriteName = "Progress_labor";
        }
        else if (nowWork == csWork.Work.Plant)
        {
            nowWorkDay = PlantDay;
            int plantRandom = UnityEngine.Random.Range(1, 101);
            Debug.Log("공장");
            ingLabel.SetActive(false);
            // 강북 기계 공장 50%
            if (plantRandom <= 50)
            {
                plantType = PlantType.Kangbuk;
                WorkSprite.spriteName = "Progress_kangbukplant";
            }
            // 천호 기계 공장 30%
            else if(50 < plantRandom && 80>= plantRandom)
            {
                plantType = PlantType.Cheonho;
                WorkSprite.spriteName = "Progress_cheonhoplant";
            }
            // 강남 기계 공장 20%
            else
            {
                plantType = PlantType.Kangnam;
                WorkSprite.spriteName = "Progress_kangnamplant";
            }
            
        }
        else if (nowWork == csWork.Work.Repair)
        {
            nowWorkDay = RepairDay;
            WorkSprite.spriteName = "Progress_repair";
        }
        else if (nowWork == csWork.Work.Rest)
        {
            nowWorkDay = RestDay;
            WorkSprite.spriteName = "Progress_rest";
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    
    // 게이지오르기시작
    public void riseStart()
    {     
        startValue = workSlider.value;
        Debug.Log("스타트" + startValue);
        if (nowWorkDay == 0)
        {
        }
        else
        {
            endValue = workSlider.value + (float)(1.0f / (float)nowWorkDay);
        }
        Debug.Log("스타트" + startValue);
        Debug.Log("현재작업 총소요일"+nowWorkDay);
        Debug.Log("엔드" + endValue);
        isRise = true;
    }

    public void RaceStart()
    {
         SceneManager.LoadScene("4. Ui_To_Stage1");
    }

    public void AdsSkip()
    {
        for(int i = 0; i< 6;i++)
        {
            nowDate = nowDate.AddDays(1);
            int tmp = PlayerPrefs.GetInt("ProgressCount");
            PlayerPrefs.SetInt("ProgressCount", tmp + 1);
            userInfoSave();
            Working();

            if (nowDate.Year == 2040)
            {
                if (c_manager.MyMonths[nowDate.Month - 1].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1].startIndex].isLastWork == 1)
                {
                    rewardPay();
                    Debug.Log("보상얻음ㅎㅎ");
                    workSlider.value = 0;
                    WorkCount = 0;
                }
            }
            //41년
            else
            {
                if (c_manager.MyMonths[nowDate.Month - 1 + 12].MyDays[nowDate.Day - 2 + c_manager.MyMonths[nowDate.Month - 1 + 12].startIndex].isLastWork == 1)
                {
                    rewardPay();
                    workSlider.value = 0;
                    WorkCount = 0;
                }
            }
        }
     /*   int tmp2 = PlayerPrefs.GetInt("ProgressCount");
        PlayerPrefs.SetInt("ProgressCount", tmp2 + 1);
        nowDate = nowDate.AddDays(1);
*/

        DateLabelUpdate();
        WorkUpdate();

        StartCoroutine(adsSkipGoRace());
    }

    IEnumerator adsSkipGoRace()
    {
        yield return new WaitForEndOfFrame();
        Progress();
    }
}
