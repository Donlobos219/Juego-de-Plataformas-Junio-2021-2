using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControllerPlatform : MonoBehaviour
{
    public float gravityScale = 8.0f;
    public static float globalGravity = -9.81f;

    public static Vector3 playerPos;

    public float speed;
    public float jumpForce;

    [Space(15)]
    public float checkDistance;
    public Transform GroundCheck;
    public LayerMask GroundMask;

    [Space(15)]
    public Transform PlayerMesh;

    [Space(15)]
    public bool canJump;
    public bool canMove;

    public bool normalJump;
    public bool canUseChargeJump;
    public bool CanDash;
    public bool canDoDash;
    public bool canDoShoot;
    public bool canNormalShoot;
    public bool canUseSuperShoot;

    public float shootDelay = 2;
    public float delay = 5;
    public float DashSpeed = 10000f;

    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;

    private Rigidbody rbody;

    public Transform spawnPoint;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public Rigidbody bullet;
    public Rigidbody SuperBullet;
    public float impulse = 20f;
    public float superImpulse = 70f;
    
    
    public GameObject emptyObject;

    public GameObject dashIcon;
    public GameObject superJumpIcon;
    public GameObject superShotIcon;
    
    // Start is called before the first frame update
    void Start()
    {
        superShotIcon.SetActive(false);
        superJumpIcon.SetActive(false);
        dashIcon.SetActive(false);
        canNormalShoot = true;
        canDoShoot = true;
        jumpPressure = 0f;
        minJump = 40f;
        maxJumpPressure = 100f;
        rbody = GetComponent<Rigidbody>();
        normalJump = true;
        canUseChargeJump = false;
        canUseSuperShoot = false;
        CanDash = false;
        StartCoroutine(TrackPlayer());
    }

    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rbody.AddForce(gravity, ForceMode.Acceleration);

        //Cursor.lockState = CursorLockMode.Locked;

        Rigidbody rb = GetComponent<Rigidbody>();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;    
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 MoveDirection = forward * verticalInput + right * horizontalInput;

        rb.velocity = new Vector3(MoveDirection.x * speed, rb.velocity.y, MoveDirection.z * speed);

        if(MoveDirection != new Vector3(0,0,0))
        {
            PlayerMesh.rotation = Quaternion.LookRotation(MoveDirection);
        }
            
    }

    void Update()
    {
        if(canDoShoot == true && canNormalShoot == true)
        {
            if(Input.GetMouseButton(0))
            {
                Rigidbody shoot = (Rigidbody)Instantiate(bullet, emptyObject.transform.position + transform.forward, transform.rotation);
                shoot.AddForce(transform.forward * impulse, ForceMode.Impulse);
                StartCoroutine(CanShoot(shootDelay));
                canDoShoot = false;                       
            }
        }

        if (canUseSuperShoot == true && canNormalShoot == false)
            if (Input.GetMouseButton(0))
            {
                Rigidbody shoot = (Rigidbody)Instantiate(SuperBullet, emptyObject.transform.position + transform.forward, transform.rotation);
                shoot.AddForce(transform.forward * superImpulse, ForceMode.Impulse);
                StartCoroutine(CanShoot(shootDelay));
                canDoShoot = false;
            }

                canJump = Physics.CheckSphere(GroundCheck.position, checkDistance, GroundMask);
        
        if(CanDash == true && canDoDash == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
             {
                

                Rigidbody rb = GetComponent<Rigidbody>();
                StartCoroutine(CanDoDash(delay));
                
                //Rigidbody rb = GetComponent<Rigidbody>();
                //rb.velocity = Vector3.right * DashSpeed;
                this.GetComponent<Rigidbody>().AddForce(transform.forward * DashSpeed);
                canDoDash = false;
            }               
        }

        if (canJump && normalJump == true && Input.GetButtonDown("Jump"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.up * jumpForce;
        }

        if(canJump && normalJump == false && canUseChargeJump == true)
        {
            if(Input.GetButton("Jump"))
            {
                if(jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime*10f;
                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
            }
            else
            {
                if(jumpPressure > 0f)
                {
                    jumpPressure = jumpPressure + minJump;
                    rbody.velocity = new Vector3(jumpPressure/10, jumpPressure,0f);
                    jumpPressure = 0f;
                    //canUseChargeJump = false;
                }
            }
            
        }
    }

    private void OnDrawGizmoSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.transform.position, checkDistance);
    }

    IEnumerator CanShoot(float shootDelay)
    {
        yield return new WaitForSeconds(shootDelay);
        canDoShoot = true;

    }
    IEnumerator CanDoDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDoDash = true;
    }

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "TrampaNivel1.1")
        {
            this.gameObject.transform.position = spawnPoint.position;
        }

        if(collision.gameObject.tag == "TrampaNivel1.2")
        {
            this.gameObject.transform.position = spawnPoint2.position;
        }

        if (collision.gameObject.tag == "TrampaNivel1.3")
        {
            this.gameObject.transform.position = spawnPoint3.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Final")
        {
            SceneManager.LoadScene("Final");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
