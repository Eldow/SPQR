using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerPower : MonoBehaviour {
    public const string PowerProp = "power";
    public const int MaxPower = 100;
}

public static class PlayerPowerExtensions {
    public static void SetPower(this PhotonPlayer player, int newPower) {
        newPower = Mathf.Clamp(newPower, 0, PlayerPower.MaxPower);

        Hashtable power = new Hashtable();
        power[PlayerPower.PowerProp] = newPower;

        player.SetCustomProperties(power);
    }

    public static void DecreasePower(this PhotonPlayer player, int amount) {
        PlayerPowerExtensions.SetPower(player, player.GetPower() - amount);

        // TODO : CALL STATE MACHINE
    }

    public static int GetPower(this PhotonPlayer player) {
        object power;

        if (player.CustomProperties.TryGetValue(PlayerPower.PowerProp, 
            out power)) {
            return (int)power;
        }

        return 0;
    }
}