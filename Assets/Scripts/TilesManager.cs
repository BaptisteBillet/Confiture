using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilesManager : MonoBehaviour {

    public List<GameObject> m_ListColumns = new List<GameObject>();
    public List<List<TilesBehavior>> m_ListTiles = new List<List<TilesBehavior>>();
    public List<TilesBehavior> m_ListTilesBehavior = new List<TilesBehavior>();

    public int m_CoinsNumberMax;
    int m_CoinNumber;
    int count = 0;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < m_ListColumns.Count; i++)
        {
            List<TilesBehavior> _listTemp = new List<TilesBehavior>();
            for (int j = 0; j < m_ListColumns[0].transform.childCount; j++)
            {
                //Debug.Log(m_ListColumns[i].transform.GetChild(j).transform.position);
                _listTemp.Add(m_ListColumns[i].transform.GetChild(j).GetComponent<TilesBehavior>());
            }
            m_ListTiles.Add(_listTemp);
        }

        Debug.Log(m_ListTiles[8][5].transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
