using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile = null;
    [SerializeField]
    private float projectileSpeed = 5.0f;
    [SerializeField]
    private GameObject bulletSource = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(projectile,bulletSource.transform.position,bulletSource.transform.rotation).GetComponent<Rigidbody>().AddForce(transform.up*projectileSpeed);
            
        }
    }
}
