using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int points = 53;

    [Header("Enemy Parameters")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.3f;
    [SerializeField] float maxTimeBetweenShots = 2f;

    [Header("Enemy Laser")]
    [SerializeField] GameObject laser;
    [SerializeField] float laserOffset;
    [SerializeField] float laserSpeed;
    [SerializeField] AudioClip[] laserSFX;
    [Range(0f, 1f)] [SerializeField] float laserSFXVolume;

    [Header("Explotion")]
    [SerializeField] GameObject explotion;
    [SerializeField] AudioClip explotionSFX;
    [Range(0f, 1f)] [SerializeField] float explotionSFXVolume;
    AudioSource audioS;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(9 , 9);
        audioS = GetComponent<AudioSource>();
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject newLaser = Instantiate(laser, 
            transform.position + Vector3.up * -laserOffset, 
            Quaternion.identity);
        newLaser.GetComponent<Rigidbody2D>().velocity = Vector2.down * laserSpeed;
        audioS.PlayOneShot(laserSFX[(int)Random.Range(0, 1)], laserSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LaserDamageDealer laserDamageDealer = other.GetComponent<LaserDamageDealer>();
        HealthUpdater(laserDamageDealer);
    }

    private void HealthUpdater(LaserDamageDealer laserDamageDealer)
    {
        health -= laserDamageDealer.GetDamage();
        laserDamageDealer.Hit();
        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        FindObjectOfType<GameSession>().AddToScore(points);
        Destroy(gameObject);
        GameObject newExplotion = Instantiate(explotion, transform.position, explotion.transform.rotation);
        AudioSource.PlayClipAtPoint(explotionSFX, 
            Camera.main.transform.position,
            explotionSFXVolume);
        Destroy(newExplotion, 1);
    }
}
