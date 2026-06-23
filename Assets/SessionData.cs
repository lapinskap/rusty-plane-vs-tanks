using UnityEngine;

[CreateAssetMenu(fileName = "GameSessionData", menuName = "Game/Session Data")]
public class SessionData : ScriptableObject
{
    [Tooltip("Is God Mode currently enabled for the player across levels?")]
    public bool isGodMode;
}
