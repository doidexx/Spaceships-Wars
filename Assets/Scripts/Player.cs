using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player padding and movement speed
    [SerializeField] float movementSpeed;
    [SerializeField] float xPadding;
    [SerializeField] float botPadding;
    [SerializeField] float topPadding;

    //Screen points
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    [SerializeField] GameObject laserPrefab;

    [SerializeField] float laserPadding;
    [SerializeField] float laserSpeed; //laser movement speed
    [Range(0.0f, 1.0f)][SerializeField] float fireRate;

    bool canShoot;

    void Start()
    {
        canShoot = true;
        MovementBoundaries();
    }

    void Update()
    {
        Movement();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position + Vector3.up * laserPadding, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = Vector2.up * laserSpeed;
            StartCoroutine(ShootTimer());
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
}
