using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [HideInInspector] public int fruits;
    [HideInInspector] public Transform respawnPoint;
    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public int chosenSkinId;

    [SerializeField] private int[] numberFruit;
    public InGame_UI inGameUI;

    [Header("Player info")]
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject deathfx;

    [Header("Camera Shake FX")]
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private Vector3 shakeDirection;
    [SerializeField] private float forceMultiplier;
    [SerializeField] CameraFollow cameraFollow;
    public void ScreenShake(int facingDir)
    {
        impulse.m_DefaultVelocity = new Vector3(shakeDirection.x * facingDir, shakeDirection.y) * forceMultiplier;
        impulse.GenerateImpulse();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RespawnPlayer();
    }

    private bool HaveEnoughFruits()
    {
        if (fruits > 0)
        {
            fruits--;

            if (fruits < 0)
                fruits = 0;

            DropFruit();
            return true;
        }
        return false;
    }

    private void DropFruit()
    {
        int fruitIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(FruitType)).Length);
        GameObject newFruit = Instantiate(fruitPrefab, currentPlayer.transform.position, transform.rotation);
        newFruit.GetComponent<Fruit_DroppedByPlayer>().FruitSetup(fruitIndex);
        Destroy(newFruit, 20);
    }
    public void OnTakingDamage()
    {
        if (!HaveEnoughFruits())
        {
            KillPlayer();


            if (GameManager.instance.difficulty < 3)
                Invoke("RespawnPlayer", 1);
            else
                inGameUI.OnDeath();
        }
    }


    public void OnFalling()
    {
        KillPlayer();

        int difficulty = GameManager.instance.difficulty;

        if (difficulty < 3)
        {
            Invoke("RespawnPlayer", 1);

            if (difficulty > 1)
                HaveEnoughFruits();
        }
        //else
        //{
        //    inGameUI.OnDeath();
        //    Debug.Log("OnDead");
        //}

    }
    public void RespawnPlayer()
    {
        if (currentPlayer == null)
        {
            AudioManager.instance.PlaySFX(1);
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
            currentPlayer.GetComponent<Player>().enabled = true;
            cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<CameraFollow>();
        }
    }

    public void KillPlayer()
    {
        AudioManager.instance.PlaySFX(1);
        cameraFollow.target = null;
        if (deathfx != null)
        {
            GameObject newDeathfx = Instantiate(deathfx, currentPlayer.transform.position, currentPlayer.transform.rotation);
            Destroy(newDeathfx, 0.5f);

       }
        StartCoroutine(OnDestroyPlayer());
    }

    public IEnumerator OnDestroyPlayer()
    {
        currentPlayer.GetComponent<Collider2D>().enabled = false;
        currentPlayer.GetComponent<Player>().enabled = false;
        yield return new WaitForSeconds(2f);
        inGameUI.OnDeath();
        Destroy(currentPlayer);
    }
}
