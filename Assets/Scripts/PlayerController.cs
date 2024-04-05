using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public PowerUpMaGuns currentPowerUp = PowerUpMaGuns.None;
    public GameObject gunsPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;
    public GameObject powerupIndicator;

    public bool onPowerUp;
    public float distanceMultiplier = 15.0f;
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        StartCoroutine(StartOnCountdown());
    }
    IEnumerator StartOnCountdown()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.gameObject.SetActive(false);
        onPowerUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        if (currentPowerUp == PowerUpMaGuns.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

    }
    void MoveForward()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpMaGuns;
            powerupIndicator.gameObject.SetActive(true);
            onPowerUp = true;
            Destroy(other.gameObject);
            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());


        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        onPowerUp = false;
        currentPowerUp = PowerUpMaGuns.None;
        powerupIndicator.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") &&  currentPowerUp == PowerUpMaGuns.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 enemyDistance = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(enemyDistance * distanceMultiplier, ForceMode.Impulse);
            Debug.Log("Collided with " + collision.gameObject.name + "Power up situation is " + onPowerUp);
            Debug.Log("Player collided with: " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());

        }

    }
    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(gunsPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<Guns>().Fire(enemy.transform);
        }
    }

}
