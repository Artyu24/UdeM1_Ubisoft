using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabObject : ObjectBase, IGrabbable
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;
    private bool _isGrabbed;
    public bool IsGrabbed => _isGrabbed;

    public ObjectBase GetObjectBase()
    {
        return this;
    }

    public bool GetIsGrabbed()
    {
        return _isGrabbed;
    }

    public virtual bool OnGrab(Transform catcher)
    {
        if (_isGrabbed)
            return false;
        
        transform.SetParent(catcher);
        
        if(_rb != null)
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        if(_col != null)
            _col.isTrigger = true;

        _isGrabbed = true;
        
        return true;
    }

    public virtual void OnRelease()
    {
        if (!_isGrabbed)
            return;
        
        transform.SetParent(null);
        
        if(_rb != null)
            _rb.constraints = RigidbodyConstraints.None;
        if(_col != null)
            _col.isTrigger = false;
        
        if(gameObject.scene != SceneManager.GetActiveScene())
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        
        _isGrabbed = false;
    }
}
