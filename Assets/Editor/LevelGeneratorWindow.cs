using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LevelGeneratorWindow : EditorWindow
{
    int m_ColumnsNumber;
    int m_RowsNumber;

    bool m_ShowGenerate = true;
    bool m_ShowAdd = false;
    bool m_ShowRemove = false;
    bool m_ShowGrid = true;
    bool m_TiledGrid = true;

    GameObject m_TilePrefab;
    GameObject m_LevelHolder;
    GameObject m_Grid;
    GameObject m_WallHolder;

    Material m_EmptyTile;
    Material m_EmptyTile2;


    public List<List<GameObject>> m_Map = new List<List<GameObject>>();
    List<GameObject> m_TempListRow = new List<GameObject>();



    [MenuItem("Window/Grid Generation Window")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelGeneratorWindow));
    }

    void OnGUI()
    {
        m_EmptyTile = (Material)Resources.Load("M_EmptyTile");
        m_EmptyTile2 = (Material)Resources.Load("M_EmptyTile2");
        m_TilePrefab = Resources.Load("SM_Tile") as GameObject;
        m_LevelHolder = GameObject.Find("LevelHolder");
        m_Grid = GameObject.Find("Grid");

        //m_ShowGenerate = EditorGUILayout.Foldout(m_ShowGenerate, "Generate grid");


        #region Generate new Grid

        // Demande à l'utilisateur combien de colonnes et lignes il désire pour sa nouvelle Grid.
        m_ColumnsNumber = EditorGUILayout.IntField("Columns", m_ColumnsNumber);
        m_RowsNumber = EditorGUILayout.IntField("Rows", m_RowsNumber);

        if (GUILayout.Button("Generate Grid"))
        {
            if (m_ColumnsNumber > 0 && m_RowsNumber > 0)
            {
                // Si une grid est déjà présente sur la scène, la détruit pour la remplacer plus tard.
                if (m_LevelHolder != null)
                {
                    DestroyImmediate(m_LevelHolder);
                }


                m_LevelHolder = new GameObject("LevelHolder");
                m_Grid = new GameObject("Grid");
                m_Grid.transform.parent = m_LevelHolder.transform;
                m_WallHolder = new GameObject("WallHolder");
                m_WallHolder.transform.parent = m_LevelHolder.transform;

                for (int x = 0; x < m_ColumnsNumber; ++x)
                {
                    List<GameObject> _column = new List<GameObject>();
                    GameObject _columnParent = new GameObject("Column");

                    for (int y = 0; y < m_RowsNumber; ++y)
                    {
                        GameObject t = (GameObject)Instantiate(
                                    m_TilePrefab,
                                    new Vector3(x, 0, y),
                                    new Quaternion());
                        t.transform.parent = _columnParent.transform;
                        _column.Add(t);
                    }
                    _columnParent.transform.parent = m_Grid.transform;
                    m_Map.Add(_column);
                }
                FillList();
                //GridTiling();
                //GameObject.Find("Game").GetComponent<GameManager>().m_OffsetX = 0;
                //GameObject.Find("Game").GetComponent<GameManager>().m_OffsetY = 0;
            }
            else
            {
                Debug.LogError("DEV ERROR: L'un des deux paramètres est égal ou inférieur à 0 !");
            }
        }
        /*
        if (m_ShowGenerate)
        {
            
        }
        */
        #endregion

        #region ShowGrid
        m_ShowGrid = EditorGUILayout.Toggle("Show Grid", m_ShowGrid);

        if (m_ShowGrid)
        {
            FillList();
            for (int i = 0; i < m_Map.Count; i++)
            {
                for (int j = 0; j < m_Map[0].Count; j++)
                {
                    m_Map[i][j].GetComponent<Renderer>().enabled = true;
                }
            }
        }
        else
        {
            FillList();
            for (int i = 0; i < m_Map.Count; i++)
            {
                for (int j = 0; j < m_Map[0].Count; j++)
                {
                    m_Map[i][j].GetComponent<Renderer>().enabled = false;
                }
            }
        }
        #endregion

        #region Validate Grid
        /*if (GUILayout.Button("Validate Grid"))
        {
            FillList();
            for (int i = 0; i < m_Map.Count; i++)
            {
                for (int j = 0; j < m_Map[0].Count; j++)
                {
                    m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile;
                }
            }
        }*/

        #endregion

        #region Tile Grid
        m_TiledGrid = EditorGUILayout.Toggle("Tiled Grid", m_TiledGrid);

        if(!m_TiledGrid)
        {
            FillList();
            for (int i = 0; i < m_Map.Count; i++)
            {
                for (int j = 0; j < m_Map[0].Count; j++)
                {
                    m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile;

                }
            }
        }
        else
        {
            FillList();
            for (int i = 0; i < m_Map.Count; i++)
            {
                for (int j = 0; j < m_Map[0].Count; j++)
                {
                    if ((m_Map[i][j].transform.position.x % 2 == 0 && m_Map[i][j].transform.position.z % 2 == 0) || (m_Map[i][j].transform.position.x % 2 != 0 && m_Map[i][j].transform.position.z % 2 != 0))
                    {
                        m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile;
                    }
                    else
                    {
                        m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile2;
                    }
                }
            }
        }
        #endregion

        #region Add Buttons
        
        GUILayout.BeginHorizontal();
        if(m_LevelHolder != null)
        {
            #region AddLeft
            if (GUILayout.Button("Add Left"))
            {
                FillList();

                //Crée le GameObject servant de parent aux tuiles formant la nouvelle colonne.
                GameObject _columnParent = new GameObject("Column");

                // List temporaire permettant de stocker toutes les tiles créées pour agir dessus par la suite.
                List<GameObject> _column = new List<GameObject>();

                // Je mets le gameObject de la colonne en enfant de la Grid.
                _columnParent.transform.parent = m_Grid.transform;

                // Je mets ce gameObject comme premier enfant de la Grid.
                _columnParent.transform.SetAsFirstSibling();

                // Je crée une nouvelle colonne
                // Chaque élément sera au même x (à gauche de la colonne précédente)
                // Le z de référence est celui du premier élément de la dernière colonne créée à gauche
                // Le nombre de tuiles à créer correspond au nombre d'enfants de la dernière colonne créée à gauche.
                _column = CreateColumn(m_Map[0][0].transform.position.x - 1,
                                        m_Map[0][0].transform.position.z,
                                        m_Map[0][0].transform.parent.transform.childCount);

                for (int i = 0; i < _column.Count; i++)
                {
                    // J'attribue à chaque tuile la colonne comme parent.
                    _column[i].transform.parent = _columnParent.transform;
                }

                // J'ajoute la colonne à la liste des colonnes à l'index 0 (car elle est devenue la première colonne).
                m_Map.Insert(0, _column);

                //TileOrganization();
                //GameObject.Find("Game").GetComponent<GameManager>().m_OffsetX++;

            }
            #endregion

            #region AddRight
            if (GUILayout.Button("Add Right"))
            {
                FillList();

                GameObject _columnParent = new GameObject("Column");
                List<GameObject> _column = new List<GameObject>();

                _columnParent.transform.parent = m_Grid.transform;

                // Je crée la nouvelle colonne
                // Chaque élément sera au même x (à droite de la colonne)
                // Le z de référence est celui du premier élément de la première colonne (le plus en bas à gauche)
                // Le nombre de tuiles à créer dépend du nombre de tuiles dans la colonne la plus à gauche.
                _column = CreateColumn(m_Map[m_Map.Count - 1][0].transform.position.x + 1,
                                        m_Map[0][0].transform.position.z,
                                        m_Map[0][0].transform.parent.childCount);

                for (int i = 0; i < _column.Count; i++)
                {
                    _column[i].transform.parent = _columnParent.transform;
                }

                // J'ajoute la colonne à la fin de la liste des colonnes (car elle est devenue la dernière colonne).
                m_Map.Add(_column);

                //TileOrganization();
            }
            #endregion

            #region AddTop
            if (GUILayout.Button("Add Top"))
            {
                FillList();

                // Récupère la dernière tuile de la première colonne (donc la plus en haut à gauche).
                GameObject _lastChild = m_Map[0][0].transform.parent.GetChild(m_Map[0][0].transform.parent.childCount - 1).gameObject;

                // Crée une liste temporaire pour stocker les éléments de la nouvelle ligne pour agir dessus par la suite.
                List<GameObject> _row = new List<GameObject>();

                // Je crée la nouvelle ligne
                // Le x de référence pour chaque nouvel élément est celui de la dernière tuile de la première colonne
                // Chaque élément sera placé sur le même z (au-dessus de la dernière ligne créée).
                // Le nombre d'éléments à créer dépend du nombre de colonnes
                _row = CreateRow(_lastChild.transform.position.x,
                                    _lastChild.transform.position.z + 1,
                                    m_Map.Count);


                for(int i = 0; i < _row.Count; i++)
                {
                    // J'ajoute un nouvel élément à chaque colonne.
                    m_Map[i].Add(_row[i]);

                    // J'indique que le parent du nouvel élément est le même que la première tuile de la colonne correspondante.
                    _row[i].transform.parent = m_Map[i][0].transform.parent;
                }

                //TileOrganization();
            }
            #endregion

            #region AddBottom
            if (GUILayout.Button("Add Bottom"))
            {
                FillList();
                GameObject _firstChild = m_Map[0][0].transform.parent.GetChild(0).gameObject;
                List<GameObject> _row = new List<GameObject>();
                _row = CreateRow(_firstChild.transform.position.x, _firstChild.transform.position.z - 1, m_Map.Count);

                for (int i = 0; i < _row.Count; i++)
                {
                    m_Map[i].Insert(0, _row[i]);
                    _row[i].transform.parent = m_Map[i][1].transform.parent;
                    _row[i].transform.SetAsFirstSibling();
                }

                //TileOrganization();
                //GameObject.Find("Game").GetComponent<GameManager>().m_OffsetY++;
            }
            #endregion
        }

        GUILayout.EndHorizontal();

        #endregion
    }

    // Actualise la liste contenant chaque élément
    void FillList()
    {
        if (m_Grid != null)
        {
            m_Map.Clear();
            for (int i = 0; i < m_Grid.transform.childCount; i++)
            {
                List<GameObject> _columnList = new List<GameObject>();
                for (int j = 0; j < m_Grid.transform.GetChild(i).childCount; j++)
                {
                    _columnList.Add(m_Grid.transform.GetChild(i).transform.GetChild(j).gameObject);
                }
                m_Map.Add(_columnList);
            }
        }
    }

    List<GameObject> CreateColumn(float _x, float _z, int _columnHeight)
    {
        List<GameObject> column = new List<GameObject>();

        for (int z = 0; z < _columnHeight; ++z)
        {
            GameObject t = (GameObject)Instantiate(
                        m_TilePrefab,
                        new Vector3(_x, 0, _z + z),
                        new Quaternion());
            column.Add(t);
        }
        return column;
    }

    List<GameObject> CreateRow(float _x, float _z, int _rowHeight)
    {
        List<GameObject> _row = new List<GameObject>();
        for (int x = 0; x < _rowHeight; x++)
        {
            GameObject t = (GameObject)Instantiate(
                        m_TilePrefab,
                        new Vector3(_x + x, 0, _z),
                        new Quaternion());
            _row.Add(t);
        }
        return _row;
    }

    /*void TileOrganization()
    {
        for (int i = 0; i < m_Map.Count; i++)
        {
            for (int j = 0; j < m_Map[0].Count; j++)
            {
                m_Map[i][j].gridPosition = new Vector2(i, j);
            }
        }
        GridTiling();
    }*/

    /*void GridTiling()
    {
        for (int i = 0; i < m_Map.Count; i++)
        {
            for (int j = 0; j < m_Map[0].Count; j++)
            {
                if (m_Map[i][j].GetComponent<Renderer>().sharedMaterial.name.Contains("m_EmptyTile") && (m_Map[i][j].gridPosition.x + m_Map[i][j].gridPosition.y) % 2 == 1)
                {
                    m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile;
                }
                else if (m_Map[i][j].GetComponent<Renderer>().sharedMaterial.name.Contains("m_EmptyTile"))
                {
                    m_Map[i][j].GetComponent<Renderer>().material = m_EmptyTile2;
                }
            }
        }

    }*/

}
