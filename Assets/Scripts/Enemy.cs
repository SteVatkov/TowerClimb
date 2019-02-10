using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject coin;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Die()
    {
        int spawnNum = Random.Range(0, 5);
        for (var i = 0; i < spawnNum; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
            //isAlive = false;
            //animator.SetTrigger("Dying");
            //myRigidBody.velocity = deathKick;
    }
}
