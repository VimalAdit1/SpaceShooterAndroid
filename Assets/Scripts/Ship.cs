using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Vector2 direction;

    public float speed = 5f;
    public float offset = 0f;
    public GameObject bulletPrefab;
    public GameObject gunPoint;
    public GameManager gameManager;
    public Gun[] guns;
    public Gun selected;
    float timeBetweenShots;
    float next;
    public ParticleSystem left;
    public ParticleSystem right;
    public Joystick joystick;
    public int price;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(0f,0f);
        next = Time.time;
        SelectGun(0);
        transform.position = new Vector3(Random.Range(0, 100), Random.Range(0, 100), 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
       */
        direction.x = joystick.Horizontal;
        direction.y = joystick.Vertical;
        
    }
    private void FixedUpdate()
    {
        
        if (direction != Vector2.zero)
        {
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.AddForce(direction*speed, ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0f, 0f, rotation+offset);
            shoot();
            left.startLifetime = Mathf.Clamp(0.5f * rb.velocity.magnitude,0,1.2f);
            right.startLifetime = Mathf.Clamp(0.5f * rb.velocity.magnitude, 0,1.2f);
        }
        else
        {
            left.startLifetime = 0;
            right.startLifetime = 0;
        }
    }
    void updateTimeBetweenShots()
    {
        timeBetweenShots = selected.timeBetweenShots;
    }
    void shoot()
    {
        if(Time.time>next)
        {
            selected.shoot();
            next = Time.time + timeBetweenShots;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("PowerUp"))
        {
            int gunNo = collision.collider.GetComponent<PowerUp>().gunNo;
            SelectGun(gunNo);
            gameManager.spawnPowerup = true;
            gameManager.nextPowerup = Time.time + 5f;
        }
    }

    private void SelectGun(int gunNo)
    {
        this.selected = guns[gunNo];
        gameManager.selectedGun = gunNo;
        updateTimeBetweenShots();
        selected.gunPoint = this.gunPoint;
    }
}
