using UnityEngine;

public interface IGrabbable
{
    public ObjectBase GetObjectBase();
    public void OnGrab(Transform catcher);
    public void OnRelease();
}
