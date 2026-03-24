using UnityEngine;

public class Interact_Waypoint_Indicator : Waypoint_Indicator
{
    public GameObject GameObjectIndicator => gameObjectIndicator;

    public bool enableSameDisable;
    public void Start()
    {
        InstantiateWaypointGameObject();
    }
    protected override void IndicatorInFrame()
    {

        //GAME OBJECT INDICATOR ON-SCREEN -------------------------------------------------------------
        if (parentOnScreen) //ICON ON-SCREEN
        {
            //Object Prefab

            //Position
            gameObjectIndicator.transform.localPosition = onScreenGameObjectOffset;

            //Rotation
            gameObjectIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, onScreenGameObjectRotation);

            //Size
            if (onScreenGameObjectHide) //HIDE
            {
                gameObjectIndicator.transform.localScale = new Vector3(0, 0, Prefab_Z_Scale);
            }
            else //Show
            {
                //FADE WITH RANGE
                if (onScreenGameObjectFadeWithRange)
                {
                    FadeGameObjectWithRange();
                }
                else
                {
                    //gameObjectIndicatorCanvasGroup.alpha = 1f;
                    //User defined alpha
                    gameObjectIndicatorCanvasGroup.alpha = onScreenGameObjectColor.a;
                }

                //SCALE WITH RANGE
                if (onScreenGameObjectScaleWithRange)
                {
                    ScaleGameObjectWithRange(onScreenGameObjectSize, reverseOnScreenGameObjectScaleWithRange);
                }
                else //Use user Scale
                {
                    gameObjectIndicator.transform.localScale = new Vector3(onScreenGameObjectSize,
                        onScreenGameObjectSize, Prefab_Z_Scale);
                }
            }
        }

        //GAME OBJECT INDICATOR OFF-SCREEN -------------------------------------------------------------
        if (!parentOnScreen) //ICON OFF-SCREEN
        {
            //Position
            gameObjectIndicator.transform.localPosition = offScreenGameObjectOffset;

            //Rotation
            //gameObjectIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, offScreenGameObjectRotation);
            //Rotation
            if (offScreenObjectRotates)
            {
                //gameObjectIndicator.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + offScreenGameObjectRotation);
                gameObjectIndicator.transform.localRotation =
                    Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + offScreenGameObjectRotation);
            }
            else
            {
                //gameObjectIndicator.transform.eulerAngles = new Vector3(0f, 0f, offScreenGameObjectRotation);
                gameObjectIndicator.transform.localRotation = Quaternion.Euler(0, 0, offScreenGameObjectRotation);
            }

            //Size
            if (offScreenGameObjectHide) //HIDE
            {
                gameObjectIndicator.transform.localScale = new Vector3(0, 0, Prefab_Z_Scale);
            }
            else //Show
            {
                //FADE WITH RANGE
                if (offScreenGameObjectFadeWithRange)
                {
                    FadeGameObjectWithRange();
                }
                else
                {
                    // gameObjectIndicatorCanvasGroup.alpha = 1f;
                    //User defined alpha
                    gameObjectIndicatorCanvasGroup.alpha = offScreenGameObjectColor.a;
                }

                //SCALE WITH RANGE
                if (offScreenGameObjectScaleWithRange)
                {
                    ScaleGameObjectWithRange(offScreenGameObjectSize, reverseOffScreenGameObjectScaleWithRange);
                }
                else //Use user Scale
                {
                    gameObjectIndicator.transform.localScale = new Vector3(offScreenGameObjectSize,
                        offScreenGameObjectSize, Prefab_Z_Scale);
                }
            }
        }
    }

    int h = 0;
    protected override void InstantiateWaypointGameObject()
    {
        Debug.Log("initting " + gameObject.name, transform);
        if (onScreenGameObject != null && offScreenGameObject != null)
        {
            //Onscreen
            if (gameObjectIndicatorOnOffScreenStatus == 0)
            {
                Destroy(gameObjectIndicator);
                gameObjectIndicator =
                    Instantiate(onScreenGameObject, wpParentPos,
                        Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg)) as GameObject;
                gameObjectIndicatorOnOffScreenStatus = 1;
            }

            if (gameObjectIndicatorOnOffScreenStatus == 1)
            {
                Destroy(gameObjectIndicator);
                gameObjectIndicator =
                    Instantiate(offScreenGameObject, wpParentPos,
                        Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg)) as GameObject;
                gameObjectIndicatorOnOffScreenStatus = 0;
            }

            gameObjectIndicator.layer = 2;
            gameObjectIndicator.transform.position = wpParentRectTransform.position;
            gameObjectIndicator.transform.SetParent(wpParentGameObject.transform);
            gameObjectIndicator.name = wpParentRectTransform.name + "-Prefab";
            gameObjectIndicator.transform.localRotation = Quaternion.Euler(0, 0, 0);

            h++;
            Debug.Log(h);
            gameObjectIndicatorCanvasGroup = gameObjectIndicator.AddComponent<CanvasGroup>();
            gameObjectIndicatorCanvasGroup.alpha = 1f;
            gameObjectIndicatorCanvasGroup.blocksRaycasts = false;
            gameObjectIndicatorCanvasGroup.interactable = false;

            gameObjectIndicator.transform.SetSiblingIndex(gameObjectDepth);

            gameObjectIndicatorCreated = true;
        }
        iGameObjectIndicator++;
        var v = GameObjectIndicator.GetComponentsInChildren<ButtonClickIndicator>(true);

        IndicatorController.Instance._buttonClickIndicator = v[0];
        for (int i = 1; i < v.Length; i++) IndicatorController.Instance.m_subButtonClickIndicators.Add(v[i]);
    }
}
