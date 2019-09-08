using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")] //Player Movement
    [SerializeField] float movementSpeed;
    [SerializeField] float xPadding;
    [SerializeField] float botPadding;
    [SerializeField] float topPadding;
    [SerializeField] float health = 500f;
    [SerializeField] int lifes = 3;

    //Screen points
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserPadding;
    [SerializeField] float laserSpeed; //laser movement speed
    [Range(0.0f, 1.0f)][SerializeField] float fireRate;

    [Header("Sound")]
    [Range(0f, 1f)] [SerializeField] float laserSFXVolume;
    [Range(0f, 1f)] [SerializeField] float deadSFXVolume;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] AudioClip deadSFX;
    AudioSource audioS;

    [SerializeField] GameObject explotion;

    LaserDamageDealer laserDamageDealer;

    GameSession gameSession;
    [SerializeField] GameObject shield;

    [SerializeField] Vector3 startPos;

    bool canShoot;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        audioS = GetComponent<AudioSource>();
        Physics2D.IgnoreLayerCollision(8 , 8);
        canShoot = true;
        MovementBoundaries();
        transform.position = startPos;
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        laserDamageDealer = collision.GetComponent<LaserDamageDealer>();
        if (!laserDamageDealer) { return; }
        Hit(collision);
    }

    private void Hit(Collider2D collision)
    {
        health -= laserDamageDealer.GetDamage();
        laserDamageDealer.Hit();
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
        gameSession.ReduceLifes();
        GameObject newExplotion = Instantiate(explotion,
            transform.position,
            explotion.transform.rotation);
        AudioSource.PlayClipAtPoint(deadSFX,
            Camera.main.transform.position,
            deadSFXVolume);
        Destroy(newExplotion, 1);
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position + Vector3.up * laserPadding, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = Vector2.up * laserSpeed;
            StartCoroutine(ShootTimer());
            audioS.PlayOneShot(laserSFX, laserSFXVolume);
        }
    }
    IEnumerator ShootTimer()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void Movement()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYpos);
    }

    private void MovementBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + botPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - topPadding;
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetLifes()
    {
        return lifes;
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        health = 500;
        StartCoroutine(InvincibleTimer());
    }

    IEnumerator InvincibleTimer()
    {
        Invincible(true);
        canShoot = true;
        yield return new WaitForSeconds(4);
        Invincible(false);
    }

    public void Invincible(bool canTakeDamange)
    {
        shield.SetActive(canTakeDamange);
    }
}
