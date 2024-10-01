using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LAB2DOP
{
    public static class Vector2Extensions
    {
        public static bool IsPointOverUIObject()
        {
            bool uiIsTouch = false;
            /*for (int i = 0; i < Input.touchCount;)
            {
                if (EventSystem.current.IsPointerOverGameObject(i))
                {
                    uiIsTouch = true;
                    return uiIsTouch;
                }
            }*/

            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    uiIsTouch = true;
                    Debug.Log("UI is touch");
                    return uiIsTouch;
                }
            }
            Debug.Log("Dont UI is touch");
            return uiIsTouch;

            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //return false;
            //}

            //PointerEventData eventPosition = new PointerEventData(EventSystem.current);
            //eventPosition.position = new Vector2(pos.x, pos.y);

            //List<RaycastResult> results = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(eventPosition, results);

            //return results.Count > 0;
        }
    }

}
