using UnityEngine;

public class InputSimulator
{
    public static KeyCode simulatedKeyDown;

    public static void SimulateKeyDown(KeyCode key)
    {
        simulatedKeyDown = key;
    }

    public static bool GetKeyDown(KeyCode key)
    {
        if (simulatedKeyDown == key)
        {
            simulatedKeyDown = KeyCode.None;  // Reset the simulated key
            return true;
        }
        return false;
    }
}

