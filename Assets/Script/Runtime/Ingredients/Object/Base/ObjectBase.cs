using UnityEngine;
using UnityEngine.Serialization;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] protected ObjectTypeEnum _objectTypeEnum;
    public ObjectTypeEnum ObjectType => _objectTypeEnum;
}
