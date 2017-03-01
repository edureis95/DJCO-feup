﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
    public HealthBarScript healthBar;    

    public float playerSpeed;
    private float playerCurrentSpeed;
    public float jumpSpeed;

    private Rigidbody2D playerRgBody;

    public LayerMask firstFloor;
    public LayerMask secondFloor;

    private bool onSecondFloor;
    private bool isGrounded;

    private Collider2D playerColider;

    private float startX = 0f;
    private float meters;
    public Text scoreText;
    public Text metersText;

    public static float maxHealth = 100;
    public float Health;
        
    private float timerLeftSlowDown;

    private bool haveColideWithOtherStudent;
    // Use this for initialization
    void Start () {
        metersText.text = "meters: 0m";
        scoreText.text = "score: 0";
        playerRgBody = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<Collider2D>();
        startX = transform.position.x;

        healthBar.updateBar(Health / PlayerControler.maxHealth);

        timerLeftSlowDown = 0.0f;

        playerCurrentSpeed = playerSpeed;

        haveColideWithOtherStudent = false;
    }

    public void stepInBeer() {
        if (Health + 10 >= 0)
        {
            Health -= 10;
            healthBar.updateBar(Health / PlayerControler.maxHealth);
        }
    }

    public void stepInOtherStudent(float slowDownTime) {
        timerLeftSlowDown = slowDownTime;
        playerCurrentSpeed = playerSpeed / 3.0f;
        haveColideWithOtherStudent = true;
    }

    public void setOnSecondFloorTo(bool value) {
        this.onSecondFloor = value;
    }

    public bool isOnSecondFloor()
    {
        return onSecondFloor;
    }

    // Update is called once per 
    void Update () {
        if (haveColideWithOtherStudent) {
            timerLeftSlowDown -= Time.deltaTime;
            if (timerLeftSlowDown < 0) {
                haveColideWithOtherStudent = false;
                playerCurrentSpeed = playerSpeed;
            }
        }

        // secondFloor colider is importante to make the double jump to the secound floor only
        isGrounded = Physics2D.IsTouchingLayers(playerColider, firstFloor) || Physics2D.IsTouchingLayers(playerColider, secondFloor);

        playerRgBody.velocity = new Vector2(playerCurrentSpeed, playerRgBody.velocity.y);
        if (isGrounded && !onSecondFloor && Input.GetMouseButtonDown(0)) {
            playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
        }        

        // gui update
        float meters = ((transform.position.x - startX) / 2f);
        metersText.text = "meters: " + meters.ToString("0.00") + "m"; 
	}
}