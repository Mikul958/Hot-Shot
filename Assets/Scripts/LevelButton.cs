using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    private Color highlightColor;
    private MenuManager menuManager;
    private GameObject[] starRefs;

    void Start()
    {
        menuManager = FindFirstObjectByType<MenuManager>();
        ColorUtility.TryParseHtmlString(GameConfig.STAR_HIGHLIGHT_HEX, out highlightColor);

        starRefs = new GameObject[3];
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("star-1"))
                starRefs[0] = child.gameObject;
            else if (child.CompareTag("star-2"))
                starRefs[1] = child.gameObject;
            else if (child.CompareTag("star-3"))
                starRefs[2] = child.gameObject;
        }

        LevelData.Level thisLevel = LevelData.instance.getLevelDataAt(levelNumber);
        if ((thisLevel.starMask & GameConfig.FIRST_STAR_MASK) != 0)
            highlightStar(0);
        if ((thisLevel.starMask & GameConfig.SECOND_STAR_MASK) != 0)
            highlightStar(1);
        if ((thisLevel.starMask & GameConfig.THIRD_STAR_MASK) != 0)
            highlightStar(2);
    }

    private void highlightStar(int starIndex)
    {
        starRefs[starIndex].GetComponent<Image>().color = highlightColor;
    }

    public void clickLevelButton()
    {
        menuManager.goToLevel(levelNumber);
    }
}
