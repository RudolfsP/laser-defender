﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //configuration params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.1f;

    [Header("Projectile")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.5f;

    Coroutine firingCoroutine;
    GameObject powerUp;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start() {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update() {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        DropActivator dropActivator = other.gameObject.GetComponent<DropActivator>();
        if (damageDealer) { ProcessHit(damageDealer); }
        if (dropActivator) { ProcessItemCatch(dropActivator); } 
    }

    private void ProcessItemCatch(DropActivator dropActivator) {
        ProcessExtraLaser();
        FindObjectOfType<GameSession>().AddToScore(dropActivator.GetScoreBonus());
        dropActivator.Caught();
    }

    private void ProcessExtraLaser() {
        powerUp = GameObject.FindGameObjectWithTag("ExtraLaser");
        if (powerUp != null) {
            if(projectileFiringPeriod > 0.05f) {
                projectileFiringPeriod -= 0.01f;
            }   
        }
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth() {
        return health;
    }

    private void Fire() {
        if(Input.GetButtonDown("Fire1")) {
            firingCoroutine = StartCoroutine(FireContinuosly());
        }

        if(Input.GetButtonUp("Fire1")) {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuosly() {
        //Quaternion.identity == use the rotation we have currently
        while(true) {
            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move() {
        //deltaX and deltaY is user movement in this case
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries() {
        Camera gameCamera = Camera.main;
        //in Vector3 the y and z coordinates don't matter because we are saying .x
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
