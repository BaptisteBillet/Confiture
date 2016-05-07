using UnityEngine;
using System.Collections;



public class Player : MonoBehaviour {

	public int m_PlayerNumber;

	// Use this for initialization
	void Start ()
    {
        switch(m_PlayerNumber)
        {
            case 1:
                name = "Player Red";
                break;

            case 2:
                name = "Player Blue";
                break;

            case 3:
                name = "Player Green";
                break;

            case 4:
                name = "Player Yellow";
                break;

        }
	    
	}

}
