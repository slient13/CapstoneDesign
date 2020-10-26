using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject Cube;

    public int Width_x = 0;
    public int Width_z = 0;
    public int Width_y = 0;

    public float pos_x = 0;
    public float pos_z = 0;
    public float pos_y = 0;
    



    // Start is called before the first frame update
    void Start()
    {
        

        for (int x = 0; x < Width_x; x++)
        {
            for (int z = 0; z < Width_z; z++)
            {
                for (int y = 0; y < Width_y; y++)
                {
                    Instantiate(Cube, new Vector3(pos_x + x, pos_y + y,pos_z + z),Quaternion.identity);
                    
                }
            }
        }
      

                
                   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
