using System.Collections;
using UnityEngine;

public class HiddenFeature : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey = KeyCode.E;
    public GameObject stonePrefab;
    public GameObject treePrefab;
    public GameObject currentDisguise;
    public GameObject player;
    public bool isCooldown;
    public float cooldownTimer = 10f;

    public void Start()
    {
        // Get the player object when the game starts
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        // Check if player is in range and on cooldown on every frame update, and handle key input
        if (isInRange && !isCooldown)
        {
            if (Input.GetKeyDown(interactKey))
            {
                ToggleHidden();
            }
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                cooldownTimer = 10f;
            }
        }
    }

    // Toggle player's hidden state
    public void ToggleHidden()
    {
        if (currentDisguise != null)
        {
            Destroy(currentDisguise);
        }
        else
        {
            currentDisguise = Instantiate(Random.value > 0.5f ? stonePrefab : treePrefab, player.transform.position, Quaternion.identity, player.transform);
            StartCoroutine(HiddenTimer());
        }
    }

    // Handling hidden state duration and flicker effects
    IEnumerator HiddenTimer()
    {
        yield return new WaitForSeconds(17f); 

        if (currentDisguise == null) yield break;  
        float blinkInterval = 0.5f;
        for (int i = 0; i < 6; i++)
        {
            if (currentDisguise == null) yield break;  
            currentDisguise.SetActive(!currentDisguise.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }

        if (currentDisguise != null)  
            Destroy(currentDisguise);
        
        isCooldown = true;
    }

    // When the player enters the trigger area, set isInRange to true
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player now in range");
        }
    }

    // When the player leaves the trigger area, set isInRange to false
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player now out of range");
        }
    }
}
