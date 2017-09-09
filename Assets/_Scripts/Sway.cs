using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour
{
    //武器摇摆
    public float amount = 0.055f;
    public float maxAmount = 0.09f;
    //光滑值
    private float smooth = 3f;
    //
    private float _smooth;

    private Vector3 def;
    //private Vector2 defAth;
    //private Vector3 euler;

    void Start()
    {
        def = transform.localPosition;
        //euler = transform.localEulerAngles;
    }

    void Update()
    {
        _smooth = smooth;

        float factorX = -Input.GetAxis("Mouse X") * amount;
        float factorY = -Input.GetAxis("Mouse Y") * amount;

        if (factorX > maxAmount)
            factorX = maxAmount;
        if (factorX < -maxAmount)
            factorX = -maxAmount;
        if (factorY > maxAmount)
            factorY = maxAmount;
        if (factorY < -maxAmount)
            factorY = -maxAmount;

        Vector3 final = new Vector3(def.x + factorX, def.y + factorY, def.z);
        //如果为瞄准状态
        if (Aiming.instance.aiming)
        {
            final /= 20f;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, Time.deltaTime * _smooth);
    }
}
