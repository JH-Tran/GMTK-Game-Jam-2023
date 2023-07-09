using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloor : MonoBehaviour
{
    [SerializeField] Animator fireFloorAni;
    private float timer;
    private bool stopTimer = false;
    private bool isOnFire = false;
    private float maxTimer = 7f;
    private float cooldownTimer = 2f;
    public void Awake()
    {
        timer = maxTimer;
        isOnFire = false;
    }

    public void FixedUpdate()
    {
        if (stopTimer == false)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            stopTimer = true;
            StartCoroutine(ResetTimer());
        }
    }
    private IEnumerator ResetTimer()
    {
        fireFloorAni.SetBool("isAnimated", true);
        yield return new WaitForSeconds(cooldownTimer);
        timer = maxTimer;
        isOnFire = !isOnFire;
        stopTimer = false;
        fireFloorAni.SetBool("isAnimated", false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOnFire == true)
        {
            collision.GetComponent<PlayerMovement>().SetMoveSpeedMultiplier(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().SetMoveSpeedMultiplier(false);
        }
    }
}
