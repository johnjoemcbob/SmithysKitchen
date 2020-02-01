using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupScript : MonoBehaviour
{
    public Canvas popupCanvas;
    public Image iconImage;

    public float lerpRate = 0.1f;
    public float maxCanvasScale = 0.01f;
    Vector3 targetCanvasScale;

    public float bounceIntensity = 1.5f;
    public float bounceSpeed = 5.0f;

    RectTransform canvasRectTransform;

    enum POPUP_STATE { UNKNOWN, OPENING, OPEN, CLOSING, CLOSED };
    POPUP_STATE currentState = POPUP_STATE.UNKNOWN;
   
    // Start is called before the first frame update
    void Start()
    {
        if (popupCanvas != null)
        {
            canvasRectTransform = popupCanvas.GetComponent<RectTransform>();
        }

        //SetIconSprite("Sprites/heart_test", Color.red);
        OpenBubble();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentState();
    }

    // --------------------------------------------------------------------
    void UpdateCurrentState()
    {
        if (currentState == POPUP_STATE.UNKNOWN)
            return;

        switch (currentState)
        {
            case (POPUP_STATE.OPENING):
                {
                    canvasRectTransform.localScale = Vector3.Lerp(canvasRectTransform.localScale, targetCanvasScale, lerpRate);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, Mathf.Sin(Time.realtimeSinceStartup * bounceIntensity) * bounceSpeed); //Bounce when opening

                    if (canvasRectTransform.localScale.x >= (targetCanvasScale.x - 0.0001f))
                    {
                        SetState(POPUP_STATE.OPEN);
                    }

                    break;
                }
            case (POPUP_STATE.OPEN):
                {
                    canvasRectTransform.localScale = Vector3.Lerp(canvasRectTransform.localScale, targetCanvasScale, lerpRate);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, Mathf.Sin(Time.realtimeSinceStartup * bounceIntensity) * bounceSpeed); //Bounce when open
                    break;
                }
            case (POPUP_STATE.CLOSING):
                {
                    canvasRectTransform.localScale = Vector3.Lerp(canvasRectTransform.localScale, targetCanvasScale, lerpRate);

                    if (canvasRectTransform.localScale.x <= (targetCanvasScale.x + 0.0001f))
                    {
                        SetState(POPUP_STATE.CLOSED);
                    }

                    break;
                }
            case (POPUP_STATE.CLOSED):
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    // --------------------------------------------------------------------
    void SetState(POPUP_STATE newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case (POPUP_STATE.OPENING):
                {
                    canvasRectTransform.localScale = Vector3.zero;
                    targetCanvasScale = new Vector3(maxCanvasScale + 0.001f, maxCanvasScale + 0.001f, maxCanvasScale + 0.001f);
                    break;
                }
            case (POPUP_STATE.OPEN):
                {
                    targetCanvasScale = new Vector3(maxCanvasScale, maxCanvasScale, maxCanvasScale);
                    break;
                }
            case (POPUP_STATE.CLOSING):
                {
                    canvasRectTransform.localScale = new Vector3(maxCanvasScale, maxCanvasScale, maxCanvasScale);
                    targetCanvasScale = Vector3.zero;
                    break;
                }
            case (POPUP_STATE.CLOSED):
                {
                    canvasRectTransform.localScale = Vector3.zero;
                    break;
                }
            default:
                {
                    break;
                }
        }

        currentState = newState;
    }


    // --------------------------------------------------------------------
    public void OpenBubble()
    {
        SetState(POPUP_STATE.OPENING);
    }

    // --------------------------------------------------------------------
    public void CloseBubble()
    {
        SetState(POPUP_STATE.CLOSING);
    }


    // --------------------------------------------------------------------
    public void SetIconSprite(string resourcePath, Color color)
    {
        if (resourcePath == null)
            return;

        Sprite testSprite = Resources.Load<Sprite>(resourcePath);
        SetIconSprite(testSprite, color);
    }


    // --------------------------------------------------------------------
    public void SetIconSprite(Sprite sprite, Color color)
    {
        if (iconImage == null)
            return;

        if (sprite == null)
            return;

        if (color == null)
            color = Color.white;

        iconImage.sprite = sprite;
        iconImage.color = color;
    }
}
