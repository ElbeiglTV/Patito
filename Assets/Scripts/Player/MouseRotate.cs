using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate
{

  public void RotatePlayer(Transform Subject,int num)
    {
        Vector3 positionOnScreen = UnityEngine.Camera.main.WorldToViewportPoint(Subject.position);
        Vector3 MouseOnScreen = (Vector2)UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 direction = MouseOnScreen - positionOnScreen;
        float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg-90;
        Subject.rotation = Quaternion.Euler(0,-angle,0);
    }

    public Quaternion RotatePlayer(Transform Subject)
    {
        Vector3 positionOnScreen = UnityEngine.Camera.main.WorldToViewportPoint(Subject.position);
        Vector3 MouseOnScreen = (Vector2)UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 direction = MouseOnScreen - positionOnScreen;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
       return Quaternion.Euler(0, -angle, 0);
    }



}
