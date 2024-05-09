using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform respawnPoint;
    public GameObject currentPlayer;

    public int chosenSkinId;

    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        instance = this;
        PlayerRespawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            PlayerRespawn();
    }

    public void PlayerRespawn()
    {
        if (currentPlayer == null)
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
    }

}
