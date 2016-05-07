using UnityEngine;
using System.Collections;

public class PlayerTilesCheck : MonoBehaviour {

    public TilesManager m_TilesManager;
    PlayerMove m_PlayerMove;

    public bool m_IsLightButtonOnTile;
    public bool m_IsCoinOnTile;


    // Use this for initialization
    void Start ()
    {
        m_PlayerMove = GetComponent<PlayerMove>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_PlayerMove.m_IsMoving)
        {
            switch (m_TilesManager.m_ListTiles[(int)transform.position.x][(int)transform.position.z].m_State)
            {
                case TilesBehavior.State.Coin:
                    m_IsCoinOnTile = true;
                    break;
                case TilesBehavior.State.Red:
                    break;
                case TilesBehavior.State.Blue:
                    break;
                case TilesBehavior.State.Green:
                    break;
                case TilesBehavior.State.Yellow:
                    break;
                default:
                    break;
            }

        }
	}
}
