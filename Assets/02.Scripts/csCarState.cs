using UnityEngine;
using System.Collections;

public class csCarState : MonoBehaviour
{
    //가지고 있는 능력
    public bool isJumper = false;
    public bool isClimber = false;
    public bool isSmart = false;

    public bool isRanking_3th = false;
    public bool isRanking_4th = false;

    /// 이펙트부분
    public GameObject Jumping_Effect;
    public GameObject Big_Jumping_Ready_Effect1;
    public GameObject Big_Jumping_Ready_Effect2;
    public GameObject Big_Jumping_Effect;
    public GameObject HP_Zero_Effect;
    public GameObject RankingEffect_3th;
    public GameObject RankingEffect_4th;

    public GameObject Three_Particle;
    public GameObject Four_Particle;

    /// 

    public int MaxHp = 1;
    public int HP = 1;
    public int Power = 0;
    public int Sec_Heal = 0;
    public float HP_Timer = 0;

    csCollisionCheck CollCheck;
    csJumperAnim csJumpAnim;
    CarController car;
    WaypointProgressTracker tracker;
    CarAIControl ai;

    public Transform respawn;
    public Transform Big_Jump_Target;
    int AddForceCount = 0;

    float BackForce_Fix = 0;
    float Climbing_Count = 0;

    GameObject childBody;
    GameObject car_body;

    public PhysicMaterial Bounce;

    public float maxSpeed = 0;
    public float reverseTime = 0;
    public float BigJump_Count = 0;
    int RecoveryLimit = 0;
    bool Recovery_Fix = true;

    bool isBackForcing = false;
    bool isStunning = false;
    bool isClimbingRotate = false;
    public bool boolRecovery = false;
    public bool isbigJumpReady = false;
    bool isBigJump = false;

    public bool CanClimbing = false;




    public enum CARSTATE
    {
        REVERSE = -1,
        READY = 0,
        RUN,
        WATER,
        WATER_Sixty,
        FREEZE,
        PARALYZE,
        HOMEIN,
        CLIMBING,
        RECOVERY,
        STUN,
        DETOUR,
        BACKFORCE,
        BIG_JUMP_READY,
        BIG_JUMP,
        JUMP,
		SMALL_JUMP
    }

    public CARSTATE carState = CARSTATE.READY;

    void Start()
    {
        ai = GetComponent<CarAIControl>();
        CollCheck = GetComponent<csCollisionCheck>();
        car = GetComponent<CarController>();
        childBody = gameObject.transform.FindChild("Image").gameObject;
        car_body = gameObject.transform.FindChild("Body").gameObject;
        Big_Jump_Target = GameObject.Find("Big_Jump_Area").transform.FindChild("Big_Jump_Point");
        tracker = GetComponent<WaypointProgressTracker>();
        StartCoroutine(JumperTrigger());
    }

    IEnumerator JumperTrigger()
    {
        yield return new WaitForSeconds(0.1f);

      /*  if (isJumper == true)
        {
            //csJumpAnim = gameObject.transform.FindChild("Image").transform.FindChild("SD_QUERY_01").GetComponent<csJumperAnim>();
        }*/
    }


    void Update()
    {
        BackForce_Fixer();
        Climbing();

        HP_Timer = HP_Timer + Time.deltaTime;
        //
        if (isRanking_3th == true)
        {
            gameObject.GetComponent<Transform>().FindChild("3sT_Effect").gameObject.SetActive(true);
        }
        else if (isRanking_3th == false)
        {
            gameObject.GetComponent<Transform>().FindChild("3sT_Effect").gameObject.SetActive(false);
        }

        if (isRanking_4th == true)
        {
            gameObject.GetComponent<Transform>().FindChild("4sT_Effect").gameObject.SetActive(true);
        }
        else if (isRanking_4th == false)
        {
            gameObject.GetComponent<Transform>().FindChild("4sT_Effect").gameObject.SetActive(false);
        }
        //
        if (tracker.speed == 0 && carState == CARSTATE.BACKFORCE)
        {
            isBackForcing = false;

            if (carState != CARSTATE.BIG_JUMP)
            {
                if (carState != CARSTATE.READY)
                {
                    StartCoroutine(BlinkEffect());
                }
                else if (carState == CARSTATE.READY)
                {
                    carState = CARSTATE.READY;
                }      
            }
            else if (carState != CARSTATE.BIG_JUMP_READY)
            {
                if (carState != CARSTATE.READY)
                {
                    StartCoroutine(BlinkEffect());
                }
                else if (carState == CARSTATE.READY)
                {
                    carState = CARSTATE.READY;
                }
            }
        }

        if (car.CurrentSpeed == 0 && carState == CARSTATE.BACKFORCE)
        {
            isBackForcing = false;

            if (carState != CARSTATE.READY)
            {
                StartCoroutine(ToBlink());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }

        if (HP_Timer > 1.0)
        {
            HP += Sec_Heal;
            HP_Timer = 0;
        }

        if (HP >= MaxHp)
        {
            HP = MaxHp;
        }

        if (HP == 0)
        {
            Reverse();            

            if (carState != CARSTATE.READY)
            {
                StartCoroutine(ToBlink());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }

        if (isBackForcing == true)
        {
            BackForce_Fix = BackForce_Fix + Time.deltaTime;
        }

        if (isbigJumpReady)
        {
            BigJump_Count = BigJump_Count + Time.deltaTime;
        }

        if (BigJump_Count > 3.0f)
        {
            isbigJumpReady = false;
            BigJump_Count = 0;
            isBigJump = true;
            carState = CARSTATE.BIG_JUMP;
        }

        if (isBigJump)
        {
            carState = CARSTATE.BIG_JUMP;
        }

        if (carState == CARSTATE.CLIMBING)
        {
            Climbing_Count = Climbing_Count + Time.deltaTime;
        }

        if (Climbing_Count > 7)
        {
            Climbing_Count = 0;
        }

        if ((tracker.speed < 0.5 && carState == CARSTATE.RUN) ||
            (tracker.speed < 0.5 && carState == CARSTATE.DETOUR) ||
            (tracker.speed < 0.5 && carState == CARSTATE.WATER) ||
            (tracker.speed < 0.5 && carState == CARSTATE.BIG_JUMP) ||
            (tracker.speed < 0.5 && carState == CARSTATE.WATER_Sixty) ||
            (tracker.speed < 0.5 && carState != CARSTATE.BACKFORCE) ||
            (tracker.speed < 0.5 && carState != CARSTATE.BIG_JUMP_READY) ||
            (tracker.speed < 0.5 && carState != CARSTATE.BIG_JUMP))
        {
            if (carState != CARSTATE.BIG_JUMP && carState != CARSTATE.BIG_JUMP_READY)
            {
                if (carState != CARSTATE.BIG_JUMP)
                {
                    if (carState != CARSTATE.READY)
                    {
                        carState = CARSTATE.RECOVERY;
                    }
                    else if (carState == CARSTATE.READY)
                    {
                        carState = CARSTATE.READY;
                    }
                }
                else if (carState != CARSTATE.BIG_JUMP_READY)
                {
                    if (carState != CARSTATE.READY)
                    {
                        carState = CARSTATE.RECOVERY;
                    }
                    else if (carState == CARSTATE.READY)
                    {
                        carState = CARSTATE.READY;
                    }
                }
            }
        }

            if ((car.CurrentSpeed <= 0.1 && carState == CARSTATE.RUN) || 
            (car.CurrentSpeed <= 0.1 && carState == CARSTATE.DETOUR) || 
            (car.CurrentSpeed <= 0.1 && carState == CARSTATE.WATER)  || 
            (car.CurrentSpeed <= 0.1 && carState != CARSTATE.BIG_JUMP) ||
            (car.CurrentSpeed <= 0.1 && carState != CARSTATE.BIG_JUMP_READY) ||
            (car.CurrentSpeed <= 0.1 && carState == CARSTATE.WATER_Sixty))
        {
            if (carState != CARSTATE.RECOVERY)
            {
                isBackForcing = true;

                if (BackForce_Fix >= 3.0 && carState != CARSTATE.RECOVERY)
                {
                    if (carState != CARSTATE.RECOVERY)
                    {
                        if (carState != CARSTATE.BIG_JUMP || carState != CARSTATE.BIG_JUMP_READY)
                        {
                            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                            carState = CARSTATE.BACKFORCE;
                        }
                    }
                }
            }
        }

        if (carState == CARSTATE.RECOVERY)
        {
            reverseTime = reverseTime + Time.deltaTime;
        }

        if (reverseTime >= 3.0f)
        {
            boolRecovery = true;
        }


        switch (carState)
        {
            case CARSTATE.REVERSE:
                break;

            case CARSTATE.READY:

                car.m_Topspeed = 0;

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                break;

            case CARSTATE.RUN:

                if (carState != CARSTATE.RECOVERY)
                {
                    boolRecovery = false;
                    isClimbingRotate = false;
                    car.m_Topspeed = maxSpeed;
                    reverseTime = 0;
                    Climbing_Count = 0;
                }

                break;

            case CARSTATE.WATER:
                car.m_Topspeed = maxSpeed / 2;
                break;

            case CARSTATE.WATER_Sixty:

                car.m_Topspeed = maxSpeed * 0.4f;
                break;

            case CARSTATE.CLIMBING:

                CanClimbing = true;

                GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 0.5f, 1.0f) * 10000);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

                if (Climbing_Count > 7)
                {
                    carState = CARSTATE.RECOVERY;
                    Climbing_Count = 0;
                }

                break;

            case CARSTATE.RECOVERY:

                if (CollCheck.Game_End == false)
                {
                    if (carState != CARSTATE.READY)
                    {
                        tracker.target = tracker.orignal_target;
                        ai.m_Target = tracker.orignal_target;

                        if (carState != CARSTATE.BIG_JUMP || carState != CARSTATE.BIG_JUMP_READY)
                        {
                            if (boolRecovery)
                            {
                                boolRecovery = false;

                                if (carState != CARSTATE.BIG_JUMP)
                                {
                                    if (carState != CARSTATE.READY)
                                    {
                                        StartCoroutine(BlinkEffect());
                                    }
                                    else if (carState == CARSTATE.READY)
                                    {
                                        carState = CARSTATE.READY;
                                    }                                    
                                }
                                else if (carState != CARSTATE.BIG_JUMP_READY)
                                {
                                    if (carState != CARSTATE.READY)
                                    {
                                        StartCoroutine(BlinkEffect());
                                    }
                                    else if (carState == CARSTATE.READY)
                                    {
                                        carState = CARSTATE.READY;
                                    }
                                }
                            }
                        }
                        else if (carState == CARSTATE.BIG_JUMP)
                        {
                            carState = CARSTATE.BIG_JUMP;
                        }
                        else if (carState == CARSTATE.BIG_JUMP_READY)
                        {
                            carState = CARSTATE.BIG_JUMP_READY;
                        }
                    }
                }
                else if (CollCheck.Game_End == true)
                {
                    carState = CARSTATE.READY;
                }

                break;

            case CARSTATE.STUN:
                if (!isStunning)
                {
                    StartCoroutine(Stunning());
                }
                break;

            case CARSTATE.DETOUR:

                break;

            case CARSTATE.BACKFORCE:

                if (carState != CARSTATE.BIG_JUMP)
                {
                    if (carState != CARSTATE.READY)
                    {
                        StartCoroutine(BlinkEffect());
                    }
                    else if (carState == CARSTATE.READY)
                    {
                        carState = CARSTATE.READY;
                    }
                }
                else if (carState != CARSTATE.BIG_JUMP_READY)
                {
                    if (carState != CARSTATE.READY)
                    {
                        StartCoroutine(BlinkEffect());
                    }
                    else if (carState == CARSTATE.READY)
                    {
                        carState = CARSTATE.READY;
                    }
                }

                car.AccelControl = true;
                break;

            case CARSTATE.BIG_JUMP_READY:

                reverseTime = 0;

                isbigJumpReady = true;
                car.GetComponent<Transform>().localRotation = Big_Jump_Target.GetComponent<Transform>().rotation;

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                GameObject BJR_Particle1 = Instantiate(Big_Jumping_Ready_Effect1) as GameObject;
                BJR_Particle1.transform.position = gameObject.transform.position;
                Destroy(BJR_Particle1, 0.5f);

                GameObject BJR_Particle2 = Instantiate(Big_Jumping_Ready_Effect2) as GameObject;
                BJR_Particle2.transform.position = gameObject.transform.position;
                Destroy(BJR_Particle2, 0.5f);

                break;
            case CARSTATE.BIG_JUMP:

                //reverseTime = 0;

                GameObject BJ_Particle = Instantiate(Big_Jumping_Effect) as GameObject;

                BJ_Particle.transform.position = gameObject.transform.position;
                BJ_Particle.transform.parent = gameObject.transform;

                Destroy(BJ_Particle, 1.0f);

                car.m_Topspeed = maxSpeed;

                if (isBigJump)
                {
                    BIG_Jump();
                    AddForceCount++;
                    if(AddForceCount>20)
                    {
                        isBigJump = false;
                        AddForceCount = 0;

                        //reverseTime = 0;
                    }
                }

                break;

		case CARSTATE.SMALL_JUMP:

                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                carState = CARSTATE.RUN;

			break;

            case CARSTATE.JUMP:

                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                GameObject J_Particle = Instantiate(Jumping_Effect) as GameObject;
                J_Particle.transform.position = gameObject.transform.position;
                Destroy(J_Particle, 0.5f);

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                //GetComponent<Rigidbody>().constraints = ;
                StartCoroutine(Jump_To_Run());

                break;
        }
    }

    void Climbing()
    {
        if (CanClimbing == true)
        {
            isClimbingRotate = true;

            transform.Translate(Vector3.up * (5 * Time.deltaTime));
        }
    }

    IEnumerator Jump_To_Run()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        carState = CARSTATE.RUN;
    }

    void BIG_Jump()
    {
        
      GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        if (Big_Jump_Target.transform.tag == "Big_Hurdle")
        {
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 1.0f, 3.0f) * 800000);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(transform.TransformVector(0.0f, 1.0f, 3.0f) * 400000);
        }
    }

    
    IEnumerator Stunning()
    {
        isStunning = true;

        car.m_Topspeed = 0;

        yield return new WaitForSeconds(3.0f);

        endStunning();

    }

    void endStunning()
    {
        isStunning = false;
        carState = CARSTATE.RUN;
    }

	public void HeadToBlinkEffect()
	{
        if (carState != CARSTATE.BIG_JUMP)
        {
            if (carState != CARSTATE.READY)
            {
                StartCoroutine(BlinkEffect());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }
        else if (carState != CARSTATE.BIG_JUMP_READY)
        {
            if (carState != CARSTATE.READY)
            {
                StartCoroutine(BlinkEffect());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }
    }

    IEnumerator ToBlink()
    {
        tracker.target = tracker.orignal_target;
        ai.m_Target = tracker.orignal_target;

        yield return new WaitForSeconds(1.5f);

        if (carState != CARSTATE.BIG_JUMP)
        {
            if (carState != CARSTATE.READY)
            {
                StartCoroutine(BlinkEffect());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }
        else if (carState != CARSTATE.BIG_JUMP_READY)
        {
            if (carState != CARSTATE.READY)
            {
                StartCoroutine(BlinkEffect());
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }
        }
    }

    public IEnumerator BlinkEffect()
    {
        tracker.target = tracker.orignal_target;
        ai.m_Target = tracker.orignal_target;

        if (carState != CARSTATE.READY && CollCheck.GAMEGAME_ENDEND == false)
        {
            Debug.Log("여기여기");
            if (carState != CARSTATE.BIG_JUMP)
            {
                if (Recovery_Fix == true)
                {
                    Recovery_Fix = false;

                    int count = 0;
                    HP = MaxHp;
                    while (count < 3)
                    {
                        childBody.SetActive(false);
                        yield return new WaitForSeconds(0.15f);

                        childBody.SetActive(true);
                        yield return new WaitForSeconds(0.15f);

                        count++;
                    }
                    respawnCar();
                }
            }
            else if (carState != CARSTATE.BIG_JUMP_READY)
            {
                if (Recovery_Fix == true)
                {
                    Recovery_Fix = false;

                    int count = 0;
                    HP = MaxHp;
                    while (count < 3)
                    {
                        childBody.SetActive(false);
                        yield return new WaitForSeconds(0.15f);

                        childBody.SetActive(true);
                        yield return new WaitForSeconds(0.15f);

                        count++;
                    }
                    respawnCar();
                }
            }
        }
        else if (carState == CARSTATE.READY && CollCheck.GAMEGAME_ENDEND == true)
        {
            carState = CARSTATE.READY;
        }              
    }

    void respawnCar()
    {
        if (carState != CARSTATE.BIG_JUMP_READY)
        {

            transform.position = respawn.position + new Vector3(0.0f, -1.0f, -3.0f);
            transform.rotation = respawn.rotation;

            reverseTime = 0;
            
            car_body.GetComponent<BoxCollider>().material = Bounce;
            Recovery_Fix = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            carState = CARSTATE.RUN;
        }
    }

    void Reverse()
    {
        if (HP == 0)
        {
            HP = -1;

            car_body.GetComponent<BoxCollider>().material = null;

            
            gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Transform>().localRotation = Quaternion.Euler(0.0f, 270.0f, -180.0f);

            GameObject H_Particle = Instantiate(HP_Zero_Effect) as GameObject;
            H_Particle.transform.localPosition = new Vector3(0.5f, -0.5f, 0.5f) ;
            H_Particle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            H_Particle.transform.parent = gameObject.transform;
            Destroy(H_Particle, 6.0f);

            if (carState != CARSTATE.READY)
            {
                carState = CARSTATE.RECOVERY;
            }
            else if (carState == CARSTATE.READY)
            {
                carState = CARSTATE.READY;
            }

            if (isJumper == true)
            {
                Debug.Log("빅점프중리커버리3");
            }

            RigidbodyConstraints_FreezeAll();
        }
    }

    void RigidbodyConstraints_FreezeAll()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void BackForce_Fixer()
    {
        if (carState != CARSTATE.BIG_JUMP_READY)
        {
            if (BackForce_Fix > 6)
            {
                car.AccelControl = false;
                BackForce_Fix = 0;
                isBackForcing = false;
                carState = CARSTATE.RUN;
            }

            if (carState != CARSTATE.BIG_JUMP_READY && car.CurrentSpeed > 5.0f)
            {
                car.AccelControl = false;
                BackForce_Fix = 0;
                isBackForcing = false;
                carState = CARSTATE.RUN;
            }
        }
    }
}
