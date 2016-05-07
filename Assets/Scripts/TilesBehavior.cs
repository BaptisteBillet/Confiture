using UnityEngine;
using System.Collections;

public class TilesBehavior : MonoBehaviour {

    public enum State
    {
        Coin,
        Empty,
        Red,
        Blue,
        Green,
        Yellow
    }

    public State m_State;

    public GameObject m_Coin;
    float m_TimeCoinMin = 5;
    float m_TimeCoinMax = 10;


    // Use this for initialization
    void Start ()
    {
        if (m_State == State.Coin)
        {
            m_Coin.GetComponent<MeshRenderer>().enabled = true;
        }
	}
	
    
    IEnumerator WaitForCoinActivation()
    {
        m_Coin.SetActive(false);
        yield return new WaitForSeconds(Random.Range(m_TimeCoinMin, m_TimeCoinMax));
        m_Coin.SetActive(true);
    }
}
