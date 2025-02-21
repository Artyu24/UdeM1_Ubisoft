using UnityEngine;

public interface IGrabbable
{
    public void OnGrab(Transform catcher);
    public void OnRelease();
}
