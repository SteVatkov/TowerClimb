using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickup = 100;
    [SerializeField] float minX, maxX, verticalKick;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int coinWorth = 10;

    private Rigidbody2D rigidBody;
    private CircleCollider2D myBodyCollider;
    private Score score;
    private bool addedToScore = false;
    private float timeStart;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CircleCollider2D>();
        timeStart = Time.timeSinceLevelLoad;
        SpawnLaunch();
    }

    private void Update()
    {
        if ((Time.timeSinceLevelLoad - timeStart) > 0.5f)
        {
            Collect();
        }
    }

    void SpawnLaunch()
    {
        Vector2 spawnKick = new Vector2(Random.Range(minX, maxX), verticalKick);
        rigidBody.velocity = spawnKick;
    }

    void Collect()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Coin Collector")))
        {
            score = FindObjectOfType<Score>();
            score.AddScore(coinWorth);
            Destroy(gameObject);
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {

        /*
        if (!addedToScore)
        {
            addedToScore = true;
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            Destroy(gameObject);
        }
        }
    */
}
