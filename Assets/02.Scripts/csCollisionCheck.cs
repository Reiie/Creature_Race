using UnityEngine;
using System.Collections;

public class csCollisionCheck : MonoBehaviour
{
    CarController car;
    csCarState car_state;
    CarAIControl ai;
    WaypointProgressTracker tracker;
    csDestroyTile Tile_Destroy;
    csDestroy_Original Original_Destroy;
    csRanking Ranking;
    csGameTimer GameTimer;

    bool isEndWall = false;
    bool Jumping = false;
    int Jumping_Fix = 0;
    int TurnelFix = 0;
    int RoadFix = 0;

    public int Checking_Rep = 1;
	float JUMPER_POWER = 10000;    /// 10000

    public float Y;
    public float Z;

    public float Lap_Time = 0;

    public bool GAMEGAME_ENDEND = false;

    /// 이펙트부분

    public GameObject BN_Particle;
    public GameObject FN_Particle;

    public GameObject Water_Particle;
    public GameObject Collision_Effect;
    public GameObject FKnock_Back_Effect;
    public GameObject BKnock_Back_Effect;
    ///
    public AudioClip hitClip;
    public AudioClip springClip;
    public AudioClip bigReadyClip;

    public bool Game_End = false;

    bool Checking_Rep_Fixer = true;
    public bool CODEFIXER = true;
    public bool TargetFix = false;
    public bool TargetFix2 = false;

    void Start()
    { 
        car = GetComponent<CarController>();
        car_state = GetComponent<csCarState>();
        ai = GetComponent<CarAIControl>();
        tracker = GetComponent<WaypointProgressTracker>();
        Original_Destroy = GameObject.Find("Original_Target_Setting").GetComponent<csDestroy_Original>();
        Ranking = GameObject.Find("Directional Light").GetComponent<csRanking>();
        GameTimer = GameObject.Find("Directional Light").GetComponent<csGameTimer>();
    }

    void Update()
    {
        CheckTile();

        JumperForce();

        if (car.m_WheelColliders[0].isGrounded == false && car.m_WheelColliders[1].isGrounded == false && car.m_WheelColliders[2].isGrounded == false && car.m_WheelColliders[3].isGrounded == false)
        {
            car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if (car.m_WheelColliders[0].isGrounded == true || car.m_WheelColliders[1].isGrounded == true || car.m_WheelColliders[2].isGrounded == true || car.m_WheelColliders[3].isGrounded == true)
        {
            car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if (Game_End == true)
        {
            car_state.reverseTime = 0;
            car_state.carState = csCarState.CARSTATE.READY;
            car.GetComponent<Rigidbody>().velocity = Vector3.zero;
            car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // car.m_Topspeed = 0;
            GAMEGAME_ENDEND = true;
        }

        if (Jumping == true)
        {
            Jumping_Fix++;
			GetComponent<Rigidbody> ().velocity = new Vector3(0, 0, 0);
        }

        if (Jumping_Fix > 31)///31
        {
            Jumping_Fix = 0;
            Jumping = false;
        }

        if (car_state.carState == csCarState.CARSTATE.BIG_JUMP && car.m_WheelColliders[0].isGrounded)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            car_state.carState = csCarState.CARSTATE.RUN;
        }
    }

    void JumperForce()
    {
        if (Jumping == true && Jumping_Fix < 31)
        {
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, Y, Z) * JUMPER_POWER * Time.fixedDeltaTime);

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            car_state.carState = csCarState.CARSTATE.JUMP;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (car_state.carState == csCarState.CARSTATE.BIG_JUMP_READY)
            {
                car_state.BigJump_Count = 0;
                car_state.reverseTime = 0;
            }

            GameObject C_Particle = Instantiate(Collision_Effect) as GameObject;
            C_Particle.transform.position = collision.transform.position + new Vector3 (-1, 1, 0);
            Destroy(C_Particle, 0.5f);

            car_state.HP -= collision.gameObject.GetComponent<csCarState>().Power;
            GetComponent<AudioSource>().clip = hitClip;

            GetComponent<AudioSource>().Play();
            if (car_state.HP <= 0)
            {
                car_state.HP = 0;
            }  
        }
    }

    IEnumerator Checking_Rep_Fix()
    {
        yield return new WaitForSeconds(0.3f);
        Checking_Rep_Fixer = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Round_Check")
        {
            if (Checking_Rep_Fixer == true)
            {
                Checking_Rep_Fixer = false;
                Checking_Rep++;
                StartCoroutine(Checking_Rep_Fix());
            }
        }

        if (other.tag == "Force_Fixer1")
        {
            JUMPER_POWER = 14000;

            Z = 1000;
        }
        if (other.tag == "Force_Fixer2")
        {
            JUMPER_POWER = 22000;

            Z = 1000;
        }
        if (other.tag == "Force_Fixer3")
        {
            JUMPER_POWER = 18000;
        }

        if (other.tag == "Stage2_Over")
        {
            Checking_Rep = 1;

            if (Checking_Rep == 1)
            {
                GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 0.0f, 5.0f) * 10.0f);

                switch (UserManager.Instance().distanceCount)

                {
                    case 0:
                        tracker.progressDistance += tracker.progressDistance + 100000;
                        UserManager.Instance().distanceCount++;
                        break;
                    case 1:
                        tracker.progressDistance += tracker.progressDistance + 50000;
                        UserManager.Instance().distanceCount++;
                        break;
                    case 2:
                        tracker.progressDistance += tracker.progressDistance + 10000;
                        UserManager.Instance().distanceCount++;
                        break;
                    default:
                        break;
                }

                Ranking.Stage2_Over();
                Lap_Time = GameTimer.Play_Time;
                Game_End = true;
            }
        }

        if (other.tag == "Stage1_Over")
        {
            if (Checking_Rep == 3)
            {
                Checking_Rep = 2;
                Ranking.Stage2_Over();
                Lap_Time = GameTimer.Play_Time;
                Game_End = true;
            }
        }

        if (other.tag == "Moving_Hurdle")
        {
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0, 0.5f, -1) * 1000000);
        }

        if (other.tag == "Knock_Back")
        {
            GetComponent<AudioSource>().clip = springClip;
            GetComponent<AudioSource>().Play();

            csKnockBack_Tile Tile;

            Tile = other.GetComponent<csKnockBack_Tile>();
            other.GetComponent<csDestroyTile>().KnockBack.GetComponent<csKnockBack_Tile>().Tile_Timer = 0;
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0, 1, -2) * 500000 * 2);

            Destroy(other.gameObject);

            Destroy(BN_Particle);
            Destroy(FN_Particle);

            BN_Particle = Instantiate(BKnock_Back_Effect) as GameObject;
            BN_Particle.transform.position = gameObject.transform.position + new Vector3(-1, 1, 0);
            BN_Particle.transform.parent = gameObject.transform;
            Destroy(BN_Particle, 3.0f);

        }
        else if (other.tag == "!Knock_Back")
        {
            GetComponent<AudioSource>().clip = springClip;
            GetComponent<AudioSource>().Play();

            csKnockBack_Tile Tile;

            Tile = other.GetComponent<csKnockBack_Tile>();
            other.GetComponent<csDestroyTile>().KnockBack.GetComponent<csKnockBack_Tile>().Tile_Timer = 0;
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0, 1, 2) * 500000 * 2);

            Destroy(other.gameObject);

            Destroy(BN_Particle);
            Destroy(FN_Particle);

            FN_Particle = Instantiate(FKnock_Back_Effect) as GameObject;
            FN_Particle.transform.position = gameObject.transform.position + new Vector3(-1, 1, 0);
            FN_Particle.transform.parent = gameObject.transform;
            Destroy(FN_Particle, 3.0f);
        }

        if (other.tag == "Jump_Area")
        {

            Transform JumpingPoint;

            car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

            JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();
            transform.LookAt(JumpingPoint);

            car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 0.0f, 5.0f) * 100000);


        }

		if (other.tag == "Stage1_Jump_Intersection2_Fix")
		{

			if (car_state.isJumper == true) 
			{

				Transform JumpingPoint;

                car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();
				transform.LookAt(JumpingPoint);

                car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 0.0f, 20.0f) * 100000);
			}

		}

        if (other.tag == "Last_Point")
        {
            car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
        }

        if (other.tag == "Small_Jump_Area")
        {
			car_state.carState = csCarState.CARSTATE.SMALL_JUMP;
            car_state.respawn = tracker.orignal_target;
            JUMPER_POWER = 10000;
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 5.0f, 2.5f) * 1.0f);
            
        }

        if (other.tag == "Bomb_Area")
        {

        }

        if (other.tag == "Untagged")
        {

        }

        if (other.tag == "Jumper_MovingTo_Intersection")
        {
            if (car_state.isJumper == true && CODEFIXER == true)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;

                CODEFIXER = false;
                StartCoroutine(CODEFIXER_TRIGGER());
            }
        }

        if (other.tag == "Clime_Jump_Intersection")
        {

            if (car_state.isJumper == true && CODEFIXER == true)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;

                CODEFIXER = false;
                StartCoroutine(CODEFIXER_TRIGGER());
            }

            if (car_state.isClimber == true && CODEFIXER == true)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
                CODEFIXER = false;
                StartCoroutine(CODEFIXER_TRIGGER());
            }
        }

        if (other.tag == "Jump_Intersection")
        {
            if (car_state.isJumper == true)
            {
                if (CODEFIXER == true)
                {       
                    Transform JumpingPoint;
                    JumpingPoint = GameObject.Find("LookPoint1").transform;

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    Y = 500.0f;///1000
                    Z = 1000.0f;///4600

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;          
                    //car_state.respawn = tracker.orignal_target;
                    CODEFIXER = false;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Progress_Distance_2-1")
        {
            tracker.progressDistance = 55;
        }

        if (other.tag == "Progress_Distance_2-2")
        {
            tracker.progressDistance = 120;
        }

        if (other.tag == "Progress_Distance_2-3")
        {
            tracker.progressDistance = 740;
        }

        if (other.tag == "Progress_Distance_2-4")
        {
            tracker.progressDistance = 270;
        }

        if (other.tag == "Progress_Distance_2-5")
        {
            tracker.progressDistance = 630;
        }

        if (other.tag == "Target_Release_008")///360
        {
            if (Checking_Rep == 1)
            {
                tracker.progressDistance = 160;
            }
            else if (Checking_Rep == 2)
            {
                tracker.progressDistance = 520;
            }
            else if (Checking_Rep == 3)
            {
                tracker.progressDistance = 880;
            }
        }

        if (other.tag == "Target_Release_017")
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            if (Checking_Rep == 1)
            {
                tracker.progressDistance = 290;
            }
            else if (Checking_Rep == 2)
            {
                tracker.progressDistance = 650;
            }
            else if (Checking_Rep == 3)
            {
                tracker.progressDistance = 1010;
            }
        }

        if (other.tag == "Jump_Intersection2")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 800.0f;///1000
                    Z = 400.0f;///4600

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Stage2_JumpIntersection1")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 200.0f;///200
                    Z = 1200.0f;///1200

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();

                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Stage2_JumpIntersection11")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 800.0f;///200
                    Z = 1200.0f;///1200

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();

                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Stage2_JumpIntersection2")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    JUMPER_POWER = 2000;

                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                    transform.LookAt(JumpingPoint);

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 15000.0f;///1000
                    Z = 200.0f;///4600

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Stage2_JumpIntersection3")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 400.0f;///1000
                    Z = 1200.0f;///4600

                    car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Jump_Intersection3")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;

                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 200.0f;///1000
                    Z = 800.0f;///4600

                    car_state.respawn = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();

                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "High_Jump_Intersection")
        {
            if (car_state.isJumper == true)
            {
                CODEFIXER = true;

                if (CODEFIXER == true)
                {
                    Transform JumpingPoint;
                    JumpingPoint = other.gameObject.transform.FindChild("LookPoint").GetComponent<Transform>();

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

                    transform.LookAt(JumpingPoint);

                    car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Jumping = true;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    car_state.carState = csCarState.CARSTATE.JUMP;

                    CODEFIXER = false;

                    Y = 200.0f;///1000
                    Z = 1200.0f;///4600

                    ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                    StartCoroutine(CODEFIXER_TRIGGER());
                }
            }
        }

        if (other.tag == "Stage1_Turnel_Intersection")
        {
            int Turnel_Road_Choice = Random.Range(0, 2);

            TurnelFix++;

            if (Turnel_Road_Choice == 0 && TurnelFix == 1)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
            }
            else if (Turnel_Road_Choice == 1 && TurnelFix == 1)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point2").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point2").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
            }
        }

        if (other.tag == "Straight")
        {
            RoadFix++;

            if (RoadFix == 1)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
                StartCoroutine(Road_Fix_Reset());
            }
        }

        if (other.tag == "Original_Target_Setting")
        {
            tracker.orignal_target = tracker.target;
            //car_state.respawn = tracker.target;
            //car_state.respawn = tracker.orignal_target;
            StartCoroutine(Original_Target_Setting_Destroy());
        }

        if (other.tag == "Climber_MovingTo_Intersection")
        {
            if (car_state.isClimber == true)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
            }   
        }

        if (other.tag == "Base_WayPoint")
        {
            tracker.target = tracker.orignal_target;
            ai.m_Target = tracker.target;
            tracker.isHurdle = false;
            TurnelFix = 0;

            RoadFix = 0;
            //car_state.respawn = ai.m_Target;
            //car_state.respawn = tracker.orignal_target;
            car_state.carState = csCarState.CARSTATE.RUN;
        }

        if (other.tag == "Jump_Fail")
        {
            if(car_state.isJumper == true)
            {
                //tracker.target = tracker.orignal_target;
                ai.m_Target = tracker.target;
                tracker.isHurdle = false;
                TurnelFix = 0;

                RoadFix = 0;

                car_state.carState = csCarState.CARSTATE.RUN;
            }
        }

        if (other.tag == "Stage1_Intersection1")
        {
            RoadFix++;
            int Road_Choice = Random.Range(0, 2);

            if (Road_Choice == 0 && RoadFix == 1)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point1").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;

                StartCoroutine(Road_Fix_Reset());
            }
            else if (Road_Choice == 1 && RoadFix == 1)
            {
                ai.m_Target = other.gameObject.transform.FindChild("Point2").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Point2").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;

                StartCoroutine(Road_Fix_Reset());
            }
        }

        if (other.tag == "Wall")
        {
            if (car_state.isClimber)
            {
                car.m_Topspeed = 0;
                car_state.carState = csCarState.CARSTATE.CLIMBING;
                gameObject.transform.FindChild("Image").GetComponent<Transform>().transform.localRotation = Quaternion.Euler(-90.0f, 0, 0);

                gameObject.GetComponent<Transform>().localRotation = other.GetComponent<Transform>().localRotation;

                tracker.isHurdle = true;
                ai.m_Target = other.gameObject.transform.FindChild("Wall_Target").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Wall_Target").GetComponent<Transform>();
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
            }
        }

        if (other.tag == "End_Wall")
        {
            if (car_state.isClimber)
            {
                StartCoroutine(endWall());
            }
        }

        if (other.tag == "LeftHurdle" || other.tag == "RightHurdle")
        {
            if (car_state.isSmart)
            {
                if (car_state.carState != csCarState.CARSTATE.DETOUR)
                {
                    ai.m_Target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                    tracker.target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                    tracker.isHurdle = true;
                    car_state.carState = csCarState.CARSTATE.DETOUR;
                    //car_state.respawn = ai.m_Target;
                    //car_state.respawn = tracker.orignal_target;
                }
            }
            
        }



        if (other.tag == "Wall_Line")
        {
            ai.m_Target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
            tracker.target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
            //car_state.respawn = ai.m_Target;
            //car_state.respawn = tracker.orignal_target;
            tracker.isHurdle = true;
        }

        if(other.tag == "LeftHurdle(Water)" || other.tag == "RightHurdle(Water)")
        {
            if (car_state.carState != csCarState.CARSTATE.DETOUR && TargetFix == false)
            {                
                TargetFix = true;

                ai.m_Target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                tracker.target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                tracker.isHurdle = true;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
                car_state.carState = csCarState.CARSTATE.DETOUR;
            }                
        }

        if (other.tag == "CenterHurdle")
        {
            if (car_state.isSmart)
            {
                if (car_state.carState != csCarState.CARSTATE.DETOUR)
                {
                    int choice = Random.Range(0, 2);

                    if (choice == 0)
                    {
                        ai.m_Target = other.gameObject.transform.FindChild("Target1").GetComponent<Transform>();
                        tracker.target = other.gameObject.transform.FindChild("Target1").GetComponent<Transform>();
                        //car_state.respawn = ai.m_Target;
                        //car_state.respawn = tracker.orignal_target;
                    }
                    else if (choice == 1)
                    {
                        ai.m_Target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                        tracker.target = other.gameObject.transform.FindChild("Target2").GetComponent<Transform>();
                        //car_state.respawn = ai.m_Target;
                        //car_state.respawn = tracker.orignal_target;
                    }
                    tracker.isHurdle = true;
                    car_state.carState = csCarState.CARSTATE.DETOUR;
                }
            }
        }
        if(other.tag == "HurdleEnd")
        {
            if (car_state.isSmart)
            {
                //tracker.target = tracker.orignal_target;
                ai.m_Target = tracker.target;
                tracker.isHurdle = false;
                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;
                car_state.carState = csCarState.CARSTATE.RUN;
            }
        }

        if (other.tag == "HurdleEnd(Water)")
        {
            if (car_state.carState != csCarState.CARSTATE.DETOUR)
            {
                TargetFix = false;

                tracker.target = tracker.orignal_target;
                ai.m_Target = tracker.target;
                tracker.isHurdle = false;

                //car_state.respawn = ai.m_Target;
                //car_state.respawn = tracker.orignal_target;

                car_state.carState = csCarState.CARSTATE.RUN;
            }
        }

        if (other.tag == "Big_Jump_Area")
        {
            if (car_state.isJumper == true)
            {
                GetComponent<AudioSource>().clip = bigReadyClip;
                GetComponent<AudioSource>().Play();
                car_state.BIG_JUMP_POWER = 400000;
                car_state.Big_Jump_Target = other.GetComponent<Transform>().FindChild("Big_Jump_Point");
                car_state.carState = csCarState.CARSTATE.BIG_JUMP_READY;
            }
        }

        if (other.tag == "Big_Jump_Area1")
        {
            if (car_state.isJumper == true)
            {
                GetComponent<AudioSource>().clip = bigReadyClip;
                GetComponent<AudioSource>().Play();
                car_state.BIG_JUMP_POWER = 480000;
                car_state.Big_Jump_Target = other.GetComponent<Transform>().FindChild("Big_Jump_Point1");
                car_state.carState = csCarState.CARSTATE.BIG_JUMP_READY;
            }
        }

        if (other.tag == "Big_Jump_Area2")
        {
            if (car_state.isJumper == true)
            {
                GetComponent<AudioSource>().clip = bigReadyClip;
                GetComponent<AudioSource>().Play();
                car_state.BIG_JUMP_POWER = 520000;
                car_state.Big_Jump_Target = other.GetComponent<Transform>().FindChild("Big_Jump_Point2");
                car_state.carState = csCarState.CARSTATE.BIG_JUMP_READY;
            }
        }
    }

    void CheckTile()
    {
       RaycastHit hit;


        if (car_state.carState != csCarState.CARSTATE.CLIMBING)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, -0.04f, 0), -transform.up, out hit, 1.0f))
            {

                if (hit.collider.gameObject.tag == "Water")
                {

                    if (car_state.carState != csCarState.CARSTATE.RECOVERY)
                    {
						GameObject W_Particle = Instantiate (Water_Particle) as GameObject;
						W_Particle.transform.position = gameObject.transform.position;
						Destroy (W_Particle, 0.5f);
                        car_state.carState = csCarState.CARSTATE.WATER;
                    }
                }

                if (hit.collider.gameObject.tag == "Stage2_Water")
                {

                    if (car_state.carState != csCarState.CARSTATE.RECOVERY)
                    {
						GameObject W_Particle = Instantiate (Water_Particle) as GameObject;
						W_Particle.transform.position = gameObject.transform.position;
						Destroy (W_Particle, 0.5f);
                        car_state.carState = csCarState.CARSTATE.WATER_Sixty;
                    }
                }

                else if (hit.collider.gameObject.tag == "Floor")
                {

                    if (car_state.carState != csCarState.CARSTATE.DETOUR && car_state.carState != csCarState.CARSTATE.RECOVERY
                        && car_state.carState != csCarState.CARSTATE.BIG_JUMP_READY && car_state.carState != csCarState.CARSTATE.BIG_JUMP)
                    {
                        car_state.carState = csCarState.CARSTATE.RUN;
                    }

                    
                }
                else if (hit.collider.gameObject.tag == "Bomb_Area")
                {
                    hit.collider.gameObject.SetActive(false);
                    car_state.carState = csCarState.CARSTATE.STUN;
                }
                else if (hit.collider.tag == "Recovery_Area")
                {
                    StartCoroutine(car_state.BlinkEffect());
                    car_state.carState = csCarState.CARSTATE.RECOVERY;
                } 
            }

        }

     Debug.DrawRay((transform.position) + new Vector3(0, -0.04f, 0) , -transform.up * 1, Color.red);

    }

    IEnumerator endWall()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        car_state.carState = csCarState.CARSTATE.RUN;
        gameObject.transform.FindChild("Image").GetComponent<Transform>().transform.localRotation = Quaternion.Euler(0.0f, 0, 0);
        GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 0.0f, 5.0f) * 50000);

        car_state.CanClimbing = false;

        TargetFix = false;
    }

    IEnumerator Road_Fix_Reset()
    {
        yield return new WaitForSeconds(0.3f);

        RoadFix = 0;
    }

    IEnumerator CODEFIXER_TRIGGER()
    {
        yield return new WaitForSeconds(1.0f);

        CODEFIXER = true;
    }

    IEnumerator Original_Target_Setting_Destroy()
    {
        yield return new WaitForSeconds(5.0f);

        Original_Destroy.Destroy_Object = true;
    }
}
