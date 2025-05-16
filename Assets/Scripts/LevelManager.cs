using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;
    [SerializeField] private EscapePortal portal;
    [SerializeField] private int levelIndex = 1;

    void Start()
    {
        if (player != null)
        {
            player.OnPlayerKilled += ShowDefeatMessage;
        }

        if (portal != null)
        {
            portal.OnCharacterArrivedToPortal += ShowVictoyMessage;
        }
    }

    private void ShowDefeatMessage()
    {
        // TODO: agreagar defeat
    }

    private void ShowVictoyMessage()
    {
        // TODO: agreagar defeat
    }

}
