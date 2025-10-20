using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    private static LevelData _instance;
    public static LevelData instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<LevelData>("LevelData");
            return _instance;
        }
    }

    [Serializable]
    public class Level
    {
        [SerializeField] public int bestStrokes;
        [SerializeField] public int bestTime;
        [SerializeField] public short starMask;

        public Level()
        {
            bestStrokes = -1;
            bestTime = -1;
            starMask = GameConfig.NO_STAR_MASK;
        }

        public Level(int bestStrokes, int bestTime, short starMask)
        {
            this.bestStrokes = bestStrokes;
            this.bestTime = bestTime;
            this.starMask = starMask;
        }
    }

    private string levelPath;

    public int currentLevel = -1;
    [SerializeField] private Level[] levels = new Level[9];
    
    void OnEnable()
    {
        levelPath = Path.Combine(Application.persistentDataPath, "levels.json");
        loadLevelData();
    }

    public void loadLevelData()
    {
        if (!File.Exists(levelPath))
        {
            saveLevelData();
            return;
        }

        string levelJSON = File.ReadAllText(levelPath);
        JsonUtility.FromJsonOverwrite(levelJSON, this);
    }
    
    public void saveLevelData()
    {
        string levelJSON = JsonUtility.ToJson(this, true);
        File.WriteAllText(levelPath, levelJSON);
    }
}
