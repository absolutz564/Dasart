using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Casa : MonoBehaviour
{
    public bool isPhoto = true;
    public Image BaseImage;
    public Image BaseImagePlants;
    public Button ButtonSetImage;
    public Sprite PhotoBtImg;
    public Sprite PlantBtImg;

    public GameObject AllPictures;
    public GameObject AllPlants;

    public List<Sprite> NextImages;
    public List<Sprite> NextPlants;
    public Image ArrowRight;
    public Image ArrowLeft;

    public Sprite ArrowEnabled;
    public Sprite ArrowDisabled;
    public int index = 0;
    int indexPlant = 0;
    public bool canResize = false;

    public void VerifyAndChange(bool isNext)
    {
        if (isPhoto)
        {
            SetNextImage(isNext);
        }
        else
        {
            SetNextImagePlant(isNext);
        }
    }
    public void SetNextImage(bool isNext)
    {
        if (isNext && index < NextImages.Count -1)
        {
            index++;
            if (NextImages[index])
            {
                ArrowLeft.sprite = ArrowEnabled;
                BaseImage.sprite = NextImages[index];
                if (index == NextImages.Count - 1)
                {
                    ArrowRight.sprite = ArrowDisabled;
                }
                else
                {
                    ArrowRight.sprite = ArrowEnabled;
                }
            }
        }
        else if (!isNext && index > 0)
        {
            index--;
            if (NextImages[index])
            {
                BaseImage.sprite = NextImages[index];
                ArrowRight.sprite = ArrowEnabled;
                if (index == 0)
                {
                    ArrowLeft.sprite = ArrowDisabled;
                }
                else
                {
                    ArrowLeft.sprite = ArrowEnabled;
                }
            }
        }
    }
    public void SetNextImagePlant(bool isNext)
    {
        if (isNext && index < NextPlants.Count - 1)
        {
            index++;
            if (NextPlants[index])
            {
                ArrowLeft.sprite = ArrowEnabled;
                BaseImagePlants.sprite = NextPlants[index];
                if (index == NextPlants.Count - 1)
                {
                    ArrowRight.sprite = ArrowDisabled;
                }
                else
                {
                    ArrowRight.sprite = ArrowEnabled;
                }
            }
        }
        else if (!isNext && index > 0)
        {
            index--;
            if (NextPlants[index])
            {
                BaseImagePlants.sprite = NextPlants[index];
                ArrowRight.sprite = ArrowEnabled;
                if (index == 0)
                {
                    ArrowLeft.sprite = ArrowDisabled;
                }
                else
                {
                    ArrowLeft.sprite = ArrowEnabled;
                }
            }
        }
    }
    public void SetImageType()
    {
        index = 0;
        ArrowLeft.sprite = ArrowDisabled;
        ArrowRight.sprite = ArrowEnabled;
        BaseImage.sprite = NextImages[index];
        BaseImagePlants.sprite = NextPlants[index];
        if (isPhoto)
        {
            isPhoto = false;
            AllPictures.SetActive(false);
            AllPlants.SetActive(true);
            ButtonSetImage.image.sprite = PlantBtImg;
        } 
        else
        {
            isPhoto = true;
            AllPictures.SetActive(true);
            AllPlants.SetActive(false);
            ButtonSetImage.image.sprite = PhotoBtImg;
        }
    }

    public void SetImage(Image img)
    {
        BaseImage.sprite = img.sprite;
    }
    public void SetImagePlant(Image img)
    {
        BaseImagePlants.sprite = img.sprite;
    }

    public RectTransform gridRectTransform;

    public void MoveGrid(bool isNext)
    {
        int childCount = CountRectTransformChildren(gridRectTransform) - 5;

        float targetX = isNext ? gridRectTransform.anchoredPosition.x - 206.5f : gridRectTransform.anchoredPosition.x + 206.5f;

        if (!isNext && gridRectTransform.anchoredPosition.x < 0)
        {
            Vector2 newPosition = new Vector2(targetX, gridRectTransform.anchoredPosition.y);
            gridRectTransform.anchoredPosition = newPosition;
        }
        else if (isNext && gridRectTransform.anchoredPosition.x > (childCount * -206.5f))
        {
            Vector2 newPosition = new Vector2(targetX, gridRectTransform.anchoredPosition.y);
            gridRectTransform.anchoredPosition = newPosition;
        }
    }

    int CountRectTransformChildren(RectTransform rectTransform)
    {
        int count = rectTransform.childCount;
        int totalChildren = 0;

        for (int i = 0; i < count; i++)
        {
            RectTransform childRectTransform = rectTransform.GetChild(i).GetComponent<RectTransform>();
            if (childRectTransform != null)
            {
                totalChildren++;
            }
        }

        return totalChildren;
    }

}
