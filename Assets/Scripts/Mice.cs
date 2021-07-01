using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mice : MonoBehaviour
{
    Animator m_Animator;
    public Transform[] moveSpots;
    private int randomSpot;

    public bool playerIsInLOS = false;
    public float fieldOfViewAngle = 160f;
    public float losRadius = 45f;

    private bool aiMemorizesPlayer = false;
    public float memoryStartime = 10f;
    private float increasingMemoryTime;

    Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime = 3f;

    NavMeshAgent nav;

    public float range = 50.0f;

    public float distToPlayer = 5.0f;

    private float randomStrafeStartTime;
    private float waitStrafeTime;
    public float t_minStrafe;
    public float t_maxStrafe;

    private int randomStrafeDir;

    public float chaseRadius = 20f;

    public float facePlayerFactor = 20f;

    //wait time
    private float waitTime;
    public float startWaitTime = 1f;

    //public Transform player;

    //private bool onRange = false;
    

    private Rigidbody rbody;

    private Cheese cheese;

    // Start is called before the first frame update
    private void Awake()
    {
        float rand = Random.Range(3.0f, 4.0f);
        //InvokeRepeating("Shoot", 4, rand);
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }

    void Start()
    {
        cheese = GameObject.FindGameObjectWithTag("Cheese").GetComponent<Cheese>();
        rbody = GetComponent<Rigidbody>();
        m_Animator = gameObject.GetComponent<Animator>();
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //onRange = Vector3.Distance(transform.position, player.position) < range;

        float distance = Vector3.Distance(Cheese.cheesePos, transform.position);

        if (distance <= losRadius)
        {
            CheckLOS();
        }

        if (nav.isActiveAndEnabled)
        {
            if (playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
            {
                Patrol();
                NoiseCheck();

                StopCoroutine(AiMemory());
            }

            if (playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
            {
                Patrol();

            }

            else if (aiHeardPlayer == true && playerIsInLOS == false && aiMemorizesPlayer == false)
            {
                canSpin = true;
                //GoToNoisePosition();
            }

            else if (playerIsInLOS == true)
            {

                aiMemorizesPlayer = true;

                FacePlayer();

                ChasePlayer();


            }

            else if (aiMemorizesPlayer == true && playerIsInLOS == false)
            {
                ChasePlayer();

                StartCoroutine(AiMemory());
            }
        }

    }

    void NoiseCheck()
    {
        float distance = Vector3.Distance(Cheese.cheesePos, transform.position);

        if (distance <= noiseTravelDistance)

        {
            if (Input.GetButton("Fire1"))
            {
                noisePosition = Cheese.cheesePos;

                aiHeardPlayer = true;
            }

            else
            {
                aiHeardPlayer = false;
                canSpin = false;
            }
        }
    }

    void CheckLOS()
    {

        Vector3 direction = Cheese.cheesePos - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, losRadius))
            {
                if (hit.collider.tag == "Cheese")
                {
                    playerIsInLOS = true;

                    aiMemorizesPlayer = true;


                }

                else
                {
                    playerIsInLOS = false;
                }
            }
        }
    }

    void Patrol()
    {
        nav.SetDestination(moveSpots[randomSpot].position);

        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
        {
            
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

                waitTime = startWaitTime;
            }
            else { waitTime -= Time.deltaTime; }
        }
    }

    void ChasePlayer()
    {

        float distance = Vector3.Distance(Cheese.cheesePos, transform.position);

        if (distance <= chaseRadius && distance > distToPlayer)
        {
            
            nav.SetDestination(Cheese.cheesePos);
        }

        else if (nav.isActiveAndEnabled && distance <= distToPlayer)
        {


            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);

            if (waitStrafeTime <= 0)
            {

                if (randomStrafeDir == 0)
                {
                    //nav.SetDestination(strafeLeft.position);

                }

                else
                if (randomStrafeDir == 1)
                {
                    //nav.SetDestination(strafeRight.position);

                }
                waitStrafeTime = randomStrafeStartTime;
            }
            else
            {
                waitStrafeTime -= Time.deltaTime;
            }
        }
    }

    void FacePlayer()
    {

        Vector3 direction = (Cheese.cheesePos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
    }

    IEnumerator AiMemory()
    {
        increasingMemoryTime = 0;

        while (increasingMemoryTime < memoryStartime)
        {
            increasingMemoryTime += Time.deltaTime;
            aiMemorizesPlayer = true;
            yield return null;
        }

        aiHeardPlayer = false;
        aiMemorizesPlayer = false;
    }

    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }


}
