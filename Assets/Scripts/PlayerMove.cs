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



    public enum Direction
    {
        Stop,
        Up,
        Down,
        Left,
        Right
    }


	//Imput
	public bool m_IsStarted;
	private bool m_isInputDetected = true;

    public Direction m_Direction;
    public Direction m_MovingDirection;

    //Destination Vector
    public Vector3 m_Aim;
    public bool m_IsMoving=false;

	//Animator
	Animator m_Animator;

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

		if (m_isInputDetected)
		{
			#region Gamepad
			if ((Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) < 0)&& (m_MovingDirection == Direction.Left || m_MovingDirection == Direction.Stop))
			{
                
                m_Direction = Direction.Left;
			}
			else
			{
                if ((Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) > 0)&& (m_MovingDirection == Direction.Right || m_MovingDirection == Direction.Stop))
                {
                    
                        m_Direction = Direction.Right;
                }
                else
                {
                    if ((Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) < 0)&& (m_MovingDirection == Direction.Up || m_MovingDirection == Direction.Stop))
                    {
      
                            m_Direction = Direction.Up; 
                    }
                    else
                    {
                        if ((Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) > 0)&& (m_MovingDirection == Direction.Down || m_MovingDirection == Direction.Stop))
                        {
                        
                                m_Direction = Direction.Down;
                        }
                        else
                        {
                            m_Direction = Direction.Stop;
                        }
                    }
                   
                }
               

            }
			
			#endregion

			
		}

	}

	
	void Update()
	{

		//QuickFix de la position en Y
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		//////Code QTE demarrage Tondeuse

		//Get the Input
		InputDetection();

        //Move the mower


        m_Speed = m_MaxSpeed;

        switch (m_Direction)
        {
            case Direction.Stop:
                m_Speed = 0;

                switch(m_MovingDirection)
                {
                    case Direction.Up:
                        if (transform.position.z >= m_Aim.z && m_IsMoving)
                        {
                            transform.position = m_Aim;
                            m_IsMoving = false;
                            m_MovingDirection = Direction.Stop;

                        }
                        break;

                    case Direction.Down:
                        if (transform.position.z <= m_Aim.z && m_IsMoving)
                        {
                            transform.position = m_Aim;
                            m_IsMoving = false;
                            m_MovingDirection = Direction.Stop;

                        }
                        break;

                    case Direction.Left:
                        if (transform.position.x <= m_Aim.x && m_IsMoving)
                        {
                            transform.position = m_Aim;
                            m_IsMoving = false;
                            m_MovingDirection = Direction.Stop;

                        }
                        break;
                    case Direction.Right:
                        if (transform.position.x >= m_Aim.x && m_IsMoving)
                        {
                            transform.position = m_Aim;
                            m_IsMoving = false;
                            m_MovingDirection = Direction.Stop;

                        }
                        break;
                }

               
                break;

            case Direction.Up:
                transform.eulerAngles = new Vector3(0, 0, 0);

                
                if (m_MovingDirection == Direction.Stop)
                {
                    m_Aim = new Vector3(transform.position.x, transform.position.y, transform.position.z+3);
                    //m_Aim += new Vector3(0, 0, 3);
                    m_IsMoving = true;
                    m_MovingDirection = Direction.Up;
                }
                else
                {
                    if (m_MovingDirection == Direction.Up)
                    if (transform.position.z >= m_Aim.z && m_IsMoving)
                    {
                        transform.position = m_Aim;
                        m_Aim = new Vector3(transform.position.x, transform.position.y, transform.position.z+3);
                    }
                }

               
                break;

            case Direction.Down:
                transform.eulerAngles = new Vector3(0, 180, 0);

                if (m_MovingDirection == Direction.Stop)
                {
                    m_Aim = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                    //m_Aim += new Vector3(0, 0, 3);
                    m_IsMoving = true;
                    m_MovingDirection = Direction.Down;
                }
                else
                {
                    if (m_MovingDirection == Direction.Down)
                        if (transform.position.z <= m_Aim.z && m_IsMoving)
                    {
                        transform.position = m_Aim;
                        m_Aim = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                    }
                }
                break;

            case Direction.Left:
                transform.eulerAngles = new Vector3(0, -90, 0);


                if (m_MovingDirection == Direction.Stop)
                {
                    m_Aim = new Vector3(transform.position.x-3, transform.position.y, transform.position.z);
                    //m_Aim += new Vector3(0, 0, 3);
                    m_IsMoving = true;
                    m_MovingDirection = Direction.Left;
                }
                else
                {
                    if (m_MovingDirection == Direction.Left)
                    if (transform.position.x <= m_Aim.x && m_IsMoving)
                    {
                        transform.position = m_Aim;
                        m_Aim = new Vector3(transform.position.x-3, transform.position.y, transform.position.z);
                    }
                }
                break;
                

            case Direction.Right:
                transform.eulerAngles = new Vector3(0, 90, 0);
                if (m_MovingDirection == Direction.Stop)
                {
                    m_Aim = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
                    m_IsMoving = true;
                    m_MovingDirection = Direction.Right;
                }
                else
                {
                    if (m_MovingDirection == Direction.Right)
                        if (transform.position.x >= m_Aim.x && m_IsMoving)
                    {
                        transform.position = m_Aim;
                        m_Aim = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
                    }
                }
                break;
                
        }

        if (m_IsMoving==true)
        {
                m_CurrentSpeed += (m_Speed) * Time.deltaTime;

                m_Rigidbody.velocity = transform.forward * m_CurrentSpeed;

                m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, 0, m_MaxSpeed);
            
        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
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
