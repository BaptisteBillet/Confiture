using UnityEngine;
using System.Collections;

public class PlayerPickCoin : MonoBehaviour {

    PlayerTilesCheck m_PlayerTilesCheck;

	// Use this for initialization
	void Start ()
    {
        m_PlayerTilesCheck = GetComponent<PlayerTilesCheck>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Coin":
                col.GetComponent<MeshRenderer>().enabled = false;
                col.GetComponent<Collider>().enabled = false;
                break;

            default:
                break;
        }
    }
}
