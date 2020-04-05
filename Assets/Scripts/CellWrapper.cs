using UnityEngine;

public class CellWrapper<T>
{
    // Start is called before the first frame update
    private T data;

    private Transform objTransform;

    public T getValue()
    {
        return data;
    }

    public CellWrapper(T data, Transform trans)
    {
        this.data = data;
        this.objTransform = trans;
    }

    public void setTransform(Transform transform)
    {
        this.objTransform = transform;
    }

    public Transform getTransform()
    {
        return objTransform;
    }
}