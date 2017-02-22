using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHealth : MonoBehaviour {
    public const string HealthProp = "health";
    public const int MaxHealth = 100;
}

public static class PlayerHealthExtensions {
    public static void SetHealth(this PhotonPlayer player, int newHealth) {
        newHealth = Mathf.Clamp(newHealth, 0, PlayerHealth.MaxHealth);

        Hashtable health = new Hashtable();
        health[PlayerHealth.HealthProp] = newHealth;

        player.SetCustomProperties(health);

    }

    public static void TakeDamage(this PhotonPlayer player, int amount) {
        PlayerHealthExtensions.SetHealth(player, player.GetHealth() - amount);

        // TODO : CALL STATE MACHINE
    }

    public static int GetHealth(this PhotonPlayer player) {
        object health;

        if (player.CustomProperties.TryGetValue(PlayerHealth.HealthProp, 
            out health)) {
            return (int)health;
        }

        return 0;
    }
}