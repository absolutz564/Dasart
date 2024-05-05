using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreetViewTransition : MonoBehaviour
{
    public float transitionDuration = 3.0f;
    public List<Vector3> positionsList = new List<Vector3>(); // Lista de posições
    public List<GameObject> Objects;
    public List<GameObject> CasasObjects;
    public GameObject CanvasFullScreen;
    public Image ImageFullScreen;

    public float zoomSpeed = 0.5f; // Velocidade do zoom
    public float minZoom = 1f; // Zoom mínimo
    public float maxZoom = 5f; // Zoom máximo

    public RectTransform imageTransform;
    private Vector2 touchStartPos;
    private bool isPinching = false;
    public List<GameObject> Markers;
    public VideoPlayer videoPlayer;
    public List<VideoClip> Clips;
    public int clipIndex = 0;
    public Image ArrowRight;
    public Image ArrowLeft;

    public Sprite ArrowEnabled;
    public Sprite ArrowDisabled;
    void Update()
    {
        if (CanvasFullScreen.activeSelf)
        {
            float zoomValues = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            ZoomImageByValue(zoomValues);
            // Tela touch para zoom
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    touchStartPos = touchOne.position - touchZero.position;
                    isPinching = true;
                }
                else if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
                {
                    float currentPinchDistance = (touchOne.position - touchZero.position).magnitude;
                    float previousPinchDistance = touchStartPos.magnitude;
                    float pinchDifference = currentPinchDistance - previousPinchDistance;

                    float zoomValue = pinchDifference * Time.deltaTime * zoomSpeed;
                    ZoomImageByValue(zoomValue);
                }
            }
            else
            {
                isPinching = false;
            }
        }
    }

    public void SetClipIndex(bool isRight)
    {
        videoPlayer.Stop();
        if (clipIndex == 0 && isRight)
        {
            clipIndex = 1;
            ArrowRight.sprite = ArrowDisabled;
            ArrowLeft.sprite = ArrowEnabled;
        }
        else if(clipIndex == 1 && !isRight)
        {
            clipIndex = 0;
            ArrowRight.sprite = ArrowEnabled;
            ArrowLeft.sprite = ArrowDisabled;
        }
        videoPlayer.clip = Clips[clipIndex];
        videoPlayer.Play();
    }

    public void SetMarkerStatus(int index)
    {
        if (Markers[index].activeSelf)
        {
            Markers[index].SetActive(false);
        } else
        {
            Markers[index].SetActive(true);
        }
    }

    void ZoomImageByValue(float zoomValue)
    {
        Vector3 newScale = imageTransform.localScale + new Vector3(zoomValue, zoomValue, 0f);
        newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
        newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
        imageTransform.localScale = newScale;
    }

    IEnumerator TransitionCamera(Vector3 newPosition)
    {
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / transitionDuration;

            // Interpola a posição e rotação da câmera
            transform.position = Vector3.Lerp(initialPosition, newPosition, t);
            // Mantém a rotação constante durante a transição
            transform.rotation = initialRotation;

            yield return null;
        }
    }

    public void SetCasaFotos (int index)
    {

    }
    IEnumerator WaitToPlayVideo()
    {
        yield return new WaitForSeconds(1.5f);
        if (currIndex == 2)
        {
            videoPlayer.Play();

        }
    }

    public void ChangeVideoState()
    {
        if(videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        } else if (videoPlayer.isPaused)
        {
            videoPlayer.Play();
        }
    }

    int currIndex;
    public void StartTransition(int index)
    {
        currIndex = index;
        if (index >= 0 && index < positionsList.Count)
        {
            Vector3 newCameraPosition = positionsList[index];
            StartCoroutine(TransitionCamera(newCameraPosition));
            if (index == 2)
            {
                StartCoroutine(WaitToPlayVideo());
            } else
            {
                videoPlayer.Stop();
            }
        }
        else
        {
            Debug.LogWarning("Índice fora dos limites da lista de posições.");
        }
    }

    public RectTransform gridRectTransform;
    public RectTransform gridRectLocTransform;

    public void MoveGridLoc(bool isNext)
    {
        int childCount = CountRectTransformChildren(gridRectLocTransform) - 6;

        float targetX = isNext ? gridRectLocTransform.anchoredPosition.x - 385f : gridRectLocTransform.anchoredPosition.x + 385f;

        if (!isNext && gridRectLocTransform.anchoredPosition.x < 0)
        {
            Vector2 newPosition = new Vector2(targetX, gridRectLocTransform.anchoredPosition.y);
            gridRectLocTransform.anchoredPosition = newPosition;
        }
        else if (isNext && gridRectLocTransform.anchoredPosition.x > (childCount * -385f))
        {
            Vector2 newPosition = new Vector2(targetX, gridRectLocTransform.anchoredPosition.y);
            gridRectLocTransform.anchoredPosition = newPosition;
        }
    }

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

    public void SetInvisible()
    {
        foreach (GameObject obj in Objects)
        {
            obj.SetActive(false);
        }
    }

    public void SetCasasInvisible()
    {
        foreach (GameObject obj in CasasObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ViewFullScreen(Casa casa)
    {
        imageTransform.localScale = Vector3.one;
        imageTransform.GetComponent<Image>().preserveAspect = false;
        if (casa.canResize)
        {
            imageTransform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            imageTransform.GetComponent<Image>().preserveAspect = true;
        }
        CanvasFullScreen.SetActive(true);

        ImageFullScreen.sprite = casa.isPhoto ? casa.BaseImage.sprite : casa.BaseImagePlants.sprite;
    }
}