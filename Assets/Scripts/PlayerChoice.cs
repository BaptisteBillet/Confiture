using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerChoice : MonoBehaviour {

    public bool m_PlayerRedReady=false;
    public bool m_PlayerBlueReady = false;
    public bool m_PlayerGreenReady = false;
    public bool m_PlayerYellowReady = false;

    private int m_PlayerNumber = 0;

    private bool m_CanSelect = true;

    //UI


    //Animator
    public Animator m_AnimatorRed;
    public Animator m_AnimatorBlue;
    public Animator m_AnimatorGreen;
    public Animator m_AnimatorYellow;

    // Use this for initialization
    void Start ()
    {
        m_PlayerRedReady = false;
        m_PlayerBlueReady = false;
        m_PlayerGreenReady = false;
        m_PlayerYellowReady = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_CanSelect == true)
        {
            InputDetection();
        }
	}

    void StartTheGame()
    {

    }

    void InputDetection()
    {
        //Player Red
        if (Input.GetButtonDown("A_1"))
        {
            if (m_PlayerRedReady)
            {
                if (m_PlayerNumber > 1)
                {
                    m_CanSelect = false;
                    StartTheGame();
                }
            }
            else
            {
                m_AnimatorRed.SetTrigger("Open");
                m_PlayerRedReady = true;
                m_PlayerNumber++;
            }
        }
        if (Input.GetButtonDown("B_1"))
        {
            if (m_PlayerRedReady)
            {
                m_PlayerRedReady = false;
                m_PlayerNumber--;
                m_AnimatorRed.SetTrigger("Close");
            }
        }

        //Player Blue
        if (Input.GetButtonDown("A_2"))
        {
            if (m_PlayerBlueReady)
            {
                if (m_PlayerNumber > 1)
                {
                    m_CanSelect = false;
                    StartTheGame();
                }
            }
            else
            {
                m_AnimatorBlue.SetTrigger("Open");
                m_PlayerBlueReady = true;
                m_PlayerNumber++;
            }
        }
        if (Input.GetButtonDown("B_2"))
        {
            if (m_PlayerBlueReady)
            {
                m_PlayerBlueReady = false;
                m_PlayerNumber--;
                m_AnimatorBlue.SetTrigger("Close");
            }
        }

        //Player Green
        if (Input.GetButtonDown("A_3"))
        {
            if (m_PlayerGreenReady)
            {
                if (m_PlayerNumber > 1)
                {
                    m_CanSelect = false;
                    StartTheGame();
                }
            }
            else
            {
                m_AnimatorGreen.SetTrigger("Open");
                m_PlayerGreenReady = true;
                m_PlayerNumber++;
            }
        }
        if (Input.GetButtonDown("B_3"))
        {
            if (m_PlayerGreenReady)
            {
                m_PlayerGreenReady = false;
                m_PlayerNumber--;
                m_AnimatorGreen.SetTrigger("Close");
            }
        }

        //Player Yellow
        if (Input.GetButtonDown("A_4"))
        {
            if (m_PlayerYellowReady)
            {
                if (m_PlayerNumber > 1)
                {
                    m_CanSelect = false;
                    StartTheGame();
                }
            }
            else
            {
                m_AnimatorYellow.SetTrigger("Open");
                m_PlayerYellowReady = true;
                m_PlayerNumber++;
            }
        }
        if (Input.GetButtonDown("B_4"))
        {
            if (m_PlayerYellowReady)
            {
                m_PlayerYellowReady = false;
                m_PlayerNumber--;
                m_AnimatorYellow.SetTrigger("Close");
            }
        }
    }
}
