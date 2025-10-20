using System;
using System.IO;
using Unity.VisualScripting;
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

    public Level getCurrentLevel()
    {
        if (currentLevel > 0 && currentLevel - 1 < levels.Length)
            return levels[currentLevel - 1];
        throw new Exception("Current level is set to number " + currentLevel + "(index " + (currentLevel - 1) + "); this level number is out of bounds");
    }
    
    public void setCurrentLevel(int levelNumber)
    {
        if (levelNumber > 0 && levelNumber - 1 < levels.Length)
            currentLevel = levelNumber;
        else
            throw new Exception("Level at number " + levelNumber + "(index " + (levelNumber - 1) + ") does not exist");
    }

    public bool incrementCurrentLevelOrExit()
    {
        if (currentLevel >= levels.Length)
            return false;
        currentLevel++;
        return true;
    }

    public void unsetCurrentLevel()
    {
        currentLevel = -1;
    }

    public void updateCurrentLevelData(int strokes, int time, short starMask)
    {
        Level currentLevelData = getCurrentLevel();
        bool dataChanged = false;
        if (currentLevelData.bestStrokes < 0 || strokes < currentLevelData.bestStrokes)
        {
            currentLevelData.bestStrokes = strokes;
            dataChanged = true;
        }
        if (currentLevelData.bestTime < 0 || time < currentLevelData.bestTime)
        {
            currentLevelData.bestTime = time;
            dataChanged = true;
        }
        if ((currentLevelData.starMask & GameConfig.FIRST_STAR_MASK) == 0 && (starMask & GameConfig.FIRST_STAR_MASK) != 0)
        {
            currentLevelData.starMask += GameConfig.FIRST_STAR_MASK;
            dataChanged = true;
        }
        if ((currentLevelData.starMask & GameConfig.SECOND_STAR_MASK) == 0 && (starMask & GameConfig.SECOND_STAR_MASK) != 0)
        {
            currentLevelData.starMask += GameConfig.SECOND_STAR_MASK;
            dataChanged = true;
        }
        if ((currentLevelData.starMask & GameConfig.THIRD_STAR_MASK) == 0 && (starMask & GameConfig.THIRD_STAR_MASK) != 0)
        {
            currentLevelData.starMask += GameConfig.THIRD_STAR_MASK;
            dataChanged = true;
        }

        if (dataChanged)
            saveLevelData();
    }
}
