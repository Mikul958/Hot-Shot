using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    private static GameConfig _instance;
    public static GameConfig instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GameConfig>("GameConfig");
            return _instance;
        }
    }

    // Level management constants
    [Header("Level Settings")]
    public int allowedOverPar = 3;  // Strokes allowed over par, level is failed if this is exceeded
    public const short NO_STAR_MASK = 0x0;
    public const short FIRST_STAR_MASK = 0x1;
    public const short SECOND_STAR_MASK = 0x2;
    public const short THIRD_STAR_MASK = 0x4;

    // Ball movement constants
    [Space(10)]
    [Header("Ball Input Settings")]
    public float maxSafeSpeed;      // If the ball is moving above this speed, inputs are disabled
    public float minHitSpeed;       // Speed applied to the ball at min drag distance
    public float maxHitSpeed;       // Speed applied to the ball at max drag distance
    public float minMouseDiff;      // Minimum drag distance required for a hit
    public float maxMouseDiff;      // Maximum effective drag distance, further drag will not affect force
    public float trailStart;        // Distance from the center of the ball the input trail starts
    public float maxTrailEnd;       // End of the input trail at a drag distance of maxMouseDiff

    // Ball physics constants
    [Space(10)]
    [Header("Ball Physics Settings")]
    public float boostSpeed;        // Speed boost panels attempt to apply in their direction
    public float rampBoostSpeed;    // Speed ramps attempt to apply in their direction
    public float addedRampScale;    // Additional visual scale applied to ball by ramp
    public float rampHangtime;      // Amount of time ball stays at its peak, rise/fall phases take the same amount of time
    public float boostCooldown;     // Minimum time allowed between two boost panel collisions (not ramps)
    public float outOfBoundsWait;   // Time between touching OoB collision and being visibly counted out of bounds
    public float respawnWait;       // Time it takes for the player to respawn after OoB is fully triggered
    public float greenDrag;         // Drag applied when ball is on the default terrain
    public float roughDrag;         // Drag applied when ball is on rough terrain
    public float veryRoughDrag;     // Drag applied when ball is on very rough terrain (e.g. sand trap)
    public float iceDrag;           // Drag applied when ball is on ice
    public float noTerrainDrag;     // Default drag applied when the ball is not touching solid terrain
}
