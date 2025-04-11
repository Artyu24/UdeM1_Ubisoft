using UnityEngine;

public interface IGrabbable
{
    public ObjectBase GetObjectBase();
    public bool OnGrab(Transform catcher);
    public void OnRelease();
}
