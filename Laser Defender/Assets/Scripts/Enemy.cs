using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] int scoreValue = 150;
    [SerializeField] float health = 100f;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float explosionLength = 1f;

    [Header("Enemy prefabs")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject explosionPrefab;

    [Header("Death")]
    [SerializeField] GameObject dropPrefab;
    [SerializeField] int minCoins = 0;
    [SerializeField] int maxCoins = 1;
    [SerializeField] float minDropSpeed = 2f;
    [SerializeField] float maxDropSpeed = 10f;

    [Header("Enemy sounds")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.1f;
    // Start is called before the first frame update
    void Start() {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update() {
        CountDownAndShoot();
    }

    private void CountDownAndShoot() {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f) {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire() {
        GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity);
        Destroy(explosion, explosionLength);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        SpawnDrops();
    }

    private void SpawnDrops() {
        int coinsToDrop = Random.Range(minCoins, ++maxCoins);
        for(int i = 0; i < coinsToDrop; i++) {
            GameObject drop = Instantiate(
            dropPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
            drop.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Random.Range(minDropSpeed, maxDropSpeed));
        }

        //AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

}
