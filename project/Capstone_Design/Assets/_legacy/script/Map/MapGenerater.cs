using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject Gress;

    public int Width_x = 0;
    public int Width_z = 0;
    public int Width_y = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < Width_x; x++)
        {
            for (int z = 0; z < Width_z; z++)
            {
                for (int y = 0; y < Width_y; y++)
                {
                    Instantiate(Gress, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
