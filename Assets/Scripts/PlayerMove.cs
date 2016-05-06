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
	public float m_BaseSpeed;
	public float m_MaxSpeed;
	public float m_MinSpeed;
	public float m_Accel;
	public float m_Deccel;
	public Vector3 m_DisplacementDirection;

	public float m_CurrentSpeed = 0.0f;
	public float m_CurrentRotation = 0.0f;

	//Rotating speed
	public float m_RotateSpeed;

	//Imput
	public bool m_IsStarted;
	private bool m_isInputDetected = true;
	private bool m_UpImput;
	private bool m_DownImput;
	private bool m_LeftImput;
	private bool m_RightImput;

	//Movement State
	public bool m_IsRotatingLeft;
	public bool m_IsRotatingRight;

	//Animator
	Animator m_Animator;

	// Use this for initialization
	void Start () 
	{
        m_Player = GetComponent<Player>();

		m_Animator = GetComponent<Animator>();

		m_Rigidbody = GetComponent<Rigidbody>();

		InitializeInput();
	
	}

	void InitializeInput()
	{
		m_IsStarted = false;
		m_UpImput = false;
		m_DownImput = false;
		m_LeftImput = false;
		m_RightImput = false;
	}

	void InputDetection()
	{

		if (m_isInputDetected)
		{
			#region Gamepad
			if (Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) < 0)
			{
				m_LeftImput = true;
			}
			else
			{
				m_LeftImput = false;

			}
			if (Input.GetAxis("L_XAxis_" + m_Player.m_PlayerNumber.ToString()) > 0)
			{
				m_RightImput = true;
			}
			else
			{
				m_RightImput = false;
			}
			if (Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) < 0)
			{
				m_UpImput = true;
			}
			else
			{
				m_UpImput = false;
			}
			if (Input.GetAxis("L_YAxis_" + m_Player.m_PlayerNumber.ToString()) > 0)
			{
				m_DownImput = true;
			}
			else
			{
				m_DownImput = false;
			}
			#endregion

			#region Keyboard
			if (Input.GetKey("up"))
			{
				m_UpImput = true;
			}
			if (Input.GetKeyUp("up"))
			{
				m_UpImput = false;
			}

			if (Input.GetKey("down"))
			{
				m_DownImput = true;
			}
			if (Input.GetKeyUp("down"))
			{
				m_DownImput = false;
			}
			if (Input.GetKey("left"))
			{
				m_LeftImput = true;
			}
			if (Input.GetKeyUp("left"))
			{
				m_LeftImput = false;
			}

			if (Input.GetKey("right"))
			{
				m_RightImput = true;
			}
			if (Input.GetKeyUp("right"))
			{
				m_RightImput = false;
			}
			#endregion
		}

	}

	void RotateTheMower()
	{
		m_Rigidbody.angularVelocity = Vector3.zero;
		if (m_LeftImput ^ m_RightImput)
		{
				if (m_LeftImput && !m_RightImput)
				{
					//Rotate Left
					transform.Rotate(Vector3.down, m_RotateSpeed * Time.deltaTime);

					m_IsRotatingLeft = true;
					m_IsRotatingRight = false;

				}

				if (!m_LeftImput && m_RightImput)
				{
					//Rotate Right
					transform.Rotate(Vector3.up, m_RotateSpeed * Time.deltaTime);

					m_IsRotatingLeft = false;
					m_IsRotatingRight = true;
				}

		}
		else
		{
			//Is not rotating
			m_IsRotatingLeft = false;
			m_IsRotatingRight = false;
		}
	}

	void Update()
	{

		//QuickFix de la position en Y
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		//////Code QTE demarrage Tondeuse

		//Get the Input
		InputDetection();

		//Set the rotation
		RotateTheMower();

		//Move the mower
		m_CurrentSpeed += m_Accel * Time.deltaTime;
		
		m_Rigidbody.velocity = transform.forward * m_CurrentSpeed;
		
		m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, m_MinSpeed, m_MaxSpeed);
		
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
