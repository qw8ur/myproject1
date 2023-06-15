using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Object enemyObject;
    private GameObject[] _enemies;
    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemies(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateEnemies(int num)
    {

        _enemies = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            GameObject go = GameObject.Instantiate(enemyObject) as GameObject;
            Vector3 vdir = new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-10f,10f));
            if (vdir.magnitude < 0.001f)
            {
                vdir.x = 1.0f;
            }
            vdir.Normalize();
            go.transform.position = vdir * Random.Range(-100f, 300f);
            _enemies[i] = go;
        }
    }
}
