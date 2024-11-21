using UnityEngine;

public class ForMe : MonoBehaviour
{
    
    public void Me(Transform transform)
    {
        transform.position = this.transform.position;
    }
    public void For(Transform transform)
    {
        this.transform.position = transform.position;
    }
}
