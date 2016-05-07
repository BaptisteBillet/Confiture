using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    private	Player m_Player;

	//Access to the main class
	//private Mower m_Mower;
	//Access to the Rigidbody Component
	private Rigidbody m_Rigidbody;

	//Stock the player ID, for Multiplayer
	//private int m_PlayerId;

	// Displacement
	public float m_MaxSpeed;
	public float m_Speed;
	public Vector3 m_DisplacementDirection;

	public float m_CurrentSpeed = 0.0f;

    private int m_DistanceCase=1;

    public enum Direction
    {
        Stop,
        Up,
        Down,
        Left,
        Right
    }


    //Can go
    public LayerMask m_LayerMaskObstacle;
    public bool m_CanGoUp = true;
    public bool m_CanGoDown = true;
    public bool m_CanGoLeft = true;
    public bool m_CanGoRight = true;
    

    //Imput
    public bool m_IsStarted;

    public Direction m_Direction;
    public Direction m_MovingDirection;

    //Destination Vector
    public Vector3 m_Aim;
    public bool m_IsMoving=false;

	//Animator
	Animator m_Animator;

    //Slide Lerp
    private Vector3 m_StartMarker = new Vector3();
    private Vector3 m_EndMarker = new Vector3();
    private float m_SpeedLerp = 5;
    private float m_StartTime;
    private float m_JourneyLength;



    // Use this for initialization
    void Start () 
	{
        m_Player = GetComponent<Player>();

		m_Animator = GetComponent<Animator>();

		m_Rigidbody = GetComponent<Rigidbody>();

		InitializeInput();

        m_Aim = transform.position;
        transform.position = Vector3.zero;

    }

	void InitializeInput()
	{
		m_IsStarted = false;
        m_Direction = Direction.Stop;
        m_MovingDirection = Direction.Stop;
        transform.position = Vector3.zero;
    }

	void InputDetection()
	{
        #region old

        if ((Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) < 0) && m_CanGoLeft)
        {
            m_Direction = Direction.Left;
            StartLerpPlayer();
            
        }
        if ((Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) > 0) && m_CanGoRight)
        {
            m_Direction = Direction.Right;
            StartLerpPlayer();
          
        }

        if ((Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) < 0) && m_CanGoUp)
        {
            m_Direction = Direction.Up;
            StartLerpPlayer();
        }

        if ((Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) > 0) && m_CanGoDown)
        {
            m_Direction = Direction.Down;
            StartLerpPlayer();
        }

        
        #endregion
    }

    #region ChangePosition
    public void StartLerpPlayer()
    {
        m_IsMoving = true;

        m_StartTime = Time.time;

        m_StartMarker = transform.position;

        switch(m_Direction)
        {
            case Direction.Up:
                m_EndMarker = new Vector3(m_StartMarker.x, m_StartMarker.y, m_StartMarker.z+m_DistanceCase);
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;

            case Direction.Down:
                m_EndMarker = new Vector3(m_StartMarker.x, m_StartMarker.y, m_StartMarker.z - m_DistanceCase);
                transform.eulerAngles = new Vector3(0, 180, 0);
                break;

            case Direction.Left:
                m_EndMarker = new Vector3(m_StartMarker.x - m_DistanceCase, m_StartMarker.y, m_StartMarker.z);
                transform.eulerAngles = new Vector3(0, -90, 0);
                break;

            case Direction.Right:
                m_EndMarker = new Vector3(m_StartMarker.x + m_DistanceCase, m_StartMarker.y, m_StartMarker.z);
                transform.eulerAngles = new Vector3(0, 90, 0);
                break;
        }

        

        m_JourneyLength = Vector3.Distance(m_StartMarker, m_EndMarker);

        StartCoroutine(LerpPlayer());
    }

    IEnumerator LerpPlayer()
    {
        while (transform.position != m_EndMarker)
        {
            float distCovered = (Time.time - m_StartTime) * m_SpeedLerp;
            float fracJourney = distCovered / m_JourneyLength;
            transform.position = Vector3.Lerp(m_StartMarker, m_EndMarker, fracJourney);

            yield return new WaitForEndOfFrame();
        }
        m_Direction = Direction.Stop;
        m_IsMoving = false;
    }
    #endregion


    void Update()
	{
        /*
        //CheckForObstacle();
        Displacement();
        m_MovingDirection = Direction.Stop;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z+1), Color.red);
        */

        if(m_IsMoving==false)
        {
            CheckForObstacle();
            InputDetection();
        }

    }

    void CheckForObstacle()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1, m_LayerMaskObstacle))
        {
            m_CanGoUp = false;   

            /*
            if (m_MovingDirection == Direction.Up)
            {
                m_MovingDirection = Direction.Stop;
            }*/
        }
        else
        {
            m_CanGoUp = true;
        }

        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1, m_LayerMaskObstacle))
        {
            /*
            if (m_MovingDirection == Direction.Down)
            {
                m_MovingDirection = Direction.Stop;
            }*/
            m_CanGoDown = false;
        }
        else
        {
            m_CanGoDown = true;
        }

        if (Physics.Raycast(transform.position, Vector3.left, out hit, 1, m_LayerMaskObstacle))
        {
            /*
            if (m_MovingDirection == Direction.Left)
            {
                m_MovingDirection = Direction.Stop;
            }*/
            m_CanGoLeft = false;
        }
        else
        {
            m_CanGoLeft = true;
        }

        if (Physics.Raycast(transform.position, Vector3.right, out hit, 1, m_LayerMaskObstacle))
        {
            /*
            if (m_MovingDirection == Direction.Right)
            {
                m_MovingDirection = Direction.Stop;
            }
            */
            m_CanGoRight = false;
        }
        else
        {
            m_CanGoRight = true;
        }

    }


   
}

/* BUTTON
if (Input.GetButtonDown("A_1"))
{
	Debug.Log("A");
}
if (Input.GetButtonDown("B_1"))
{
	Debug.Log("B");
}
if (Input.GetButtonDown("X_1"))
{
	Debug.Log("X");
}
if (Input.GetButtonDown("Y_1"))
{
	Debug.Log("Y");
}
if (Input.GetButtonDown("Start_1"))
{
	Debug.Log("Start");
}
if (Input.GetButtonDown("Back_1"))
{
	Debug.Log("Select");
}
if (Input.GetButtonDown("LB_1"))
{
	Debug.Log("LB");
}
if (Input.GetButtonDown("RB_1"))
{
	Debug.Log("RB");
}
if (Input.GetAxis("DPad_XAxis_1")>0)
{
	Debug.Log("Pad droit");
}
if (Input.GetAxis("DPad_XAxis_1") < 0)
{
	Debug.Log("Pad gauche");
}
if (Input.GetAxis("L_XAxis_1") < 0)
{
	Debug.Log("Stick Gauche Gauche");
}
if (Input.GetAxis("L_XAxis_1") > 0)
{
	Debug.Log("Stick Gauche Droit");
}
if (Input.GetAxis("L_YAxis_1") < 0)
{
	Debug.Log("Stick Gauche Haut");
}
if (Input.GetAxis("L_YAxis_1") > 0)
{
	Debug.Log("Stick Gauche Bas");
}
if (Input.GetAxis("R_XAxis_1") < 0)
{
	Debug.Log("Stick Droit Gauche");
}
if (Input.GetAxis("R_XAxis_1") > 0)
{
	Debug.Log("Stick Droit Droit");
}
if (Input.GetAxis("R_YAxis_1") < 0)
{
	Debug.Log("Stick Droit Haut");
}
if (Input.GetAxis("R_YAxis_1") > 0)
{
	Debug.Log("Stick Droit Bas");
}
 
Debug.Log("Gachette Droite "+Input.GetAxis("TriggersR_1"));
Debug.Log("Gachette Gauche " + Input.GetAxis("TriggersL_1"));
*/
