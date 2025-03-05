using UnityEngine;

public class GrabObject : ObjectBase, IGrabbable
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;

    public ObjectBase GetObjectBase()
    {
        return this;
    }

    public void OnGrab(Transform catcher)
    {
        transform.SetParent(catcher);
        
        if(_rb != null)
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        if(_col != null)
            _col.isTrigger = true;
    }

    public void OnRelease()
    {
        transform.SetParent(null);
        
        if(_rb != null)
            _rb.constraints = RigidbodyConstraints.None;
        if(_col != null)
            _col.isTrigger = false;
    }
}
