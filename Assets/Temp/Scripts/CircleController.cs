using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleController : MonoBehaviour
{
    public float jumpForce = 5;
    public float jumpInterval = 0.5f;
    private float timer;
    private bool isGameOver = false;

    private Rigidbody2D rb;

    public ScoreManager scoreManager;
    public GameObject gameOverPanel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timer <= 0)
        {
            Jump();
            timer = jumpInterval;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            // Game over
            GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (isGameOver) return; // Add this line

        //Debug.Log("Score: " + ScoreManager.score);
        scoreManager.ScoreIncrease();
    }

    private void GameOver()
    {
        isGameOver = true; // Add this line


        Debug.LogError("Game Over !@!!");
        // Freeze the Snake's motion
        rb.velocity = Vector2.zero;

        if (gameOverPanel != null)
        {
            //展示游戏结束页面
            gameOverPanel.SetActive(true);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 保持X轴速度，仅修改Y轴
    }
}
