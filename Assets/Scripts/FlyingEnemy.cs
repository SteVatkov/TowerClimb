using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;

    private CircleCollider2D sight;

    // Use this for initialization
    void Start () {
        sight = GetComponentInChildren<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (sight.IsTouchingLayers(LayerMask.GetMask("Player"))){
            float moveTowards = moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, moveTowards);
            Debug.Log("Touching");
        }
    }
}
