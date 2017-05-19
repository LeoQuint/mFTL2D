using UnityEditor;
using UnityEngine;

public class TileSpawnerWindow : EditorWindow
{
    string _parentObjectName = "newTiles";

    static string m_EmptyString = "";
    static string m_MissingComponent = "Component missing!";

    //Same
    Object _tileType;
    string _tileMessage = m_EmptyString;
    int _Rows;
    int _Cols;

    float _xSize;
    float _ySize;

    [MenuItem("Tiles/Spawner")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TileSpawnerWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Spawner", EditorStyles.boldLabel);
        _parentObjectName = EditorGUILayout.TextField("Parent name:", _parentObjectName);
        
        GUILayout.Label(_tileMessage, EditorStyles.boldLabel);


        _tileType = EditorGUILayout.ObjectField("Tile:", _tileType, typeof(GameObject), true);
        _Rows = EditorGUILayout.IntField("Rows:", _Rows);
        _Cols = EditorGUILayout.IntField("Cols:", _Cols);

        _xSize = EditorGUILayout.FloatField("Size x:", _xSize);
        _ySize = EditorGUILayout.FloatField("Size y:", _ySize);

        if (GUILayout.Button("Build Object"))
        {
            SpawnSame();
        }

    }

    //Spawns X * Y tiles of the same type.
    void SpawnSame()
    {
        if (_tileType == null)
        {
            _tileMessage = m_MissingComponent;
            return;
        }
        _tileMessage = m_EmptyString;

        GameObject parentObject = new GameObject(_parentObjectName);
        parentObject.transform.position = Vector3.zero;
        parentObject.transform.rotation = Quaternion.identity;


        for (int i = 0; i < _Rows; ++i)
        {
            for (int j = 0; j < _Cols; ++j)
            {
                GameObject g = Instantiate(_tileType, new Vector3(j * _xSize, -i * _ySize, 0f), Quaternion.identity) as GameObject;
                g.transform.SetParent(parentObject.transform);
                g.name = _tileType.name + "(r" + i + "|c" + j + ")";
                if (g.GetComponent<Node>())
                {
                    Node t = g.GetComponent<Node>();
                    t.X = j;
                    t.Y = i;
                }
            }
        }
    }

}