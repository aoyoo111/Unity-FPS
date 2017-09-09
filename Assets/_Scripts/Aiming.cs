using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Aiming : MonoBehaviour
{
    //武器鼠标右键瞄准控制类
    //定义切换瞄准时间
    public float time = 2f;
    //定义武器Aim GameObject对象Position瞄准的位置
    private Vector3 point = Vector3.zero;
    //定义武器Aim GameObject对象Rotation瞄准的位置
    private Quaternion rotation = Quaternion.Euler(Vector3.zero);
    //定义武器准心alpha阿尔法透明度的值
    public float alpha = 1f;
    //定制武器瞄准状态
    public bool aiming;

    //定义实例化变量
    public static Aiming instance;

    void Awake()
    {
        //实例化自身
        instance = this;
    }

    void Update()
    {
        //触发鼠标右键抬起还原武器原角度
        if (Input.GetMouseButtonUp(1))
        {
            point = Vector3.zero;
            rotation = Quaternion.Euler(Vector3.zero);
            //设置取消瞄准状态
            aiming = false;
        }
        //武器当前localPosition和瞄准后的point进行差值运算
        transform.localPosition = Vector3.Lerp(transform.localPosition, point, Time.deltaTime * time);
        //武器当前localRotation和瞄准后的rotation进行差值运算
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * time);

        if (point != Vector3.zero && alpha > 0f)
        {
            //瞄准时alpha值设置渐变隐藏
            alpha -= 1f * Time.deltaTime * time;
        }
        else if (point == Vector3.zero && alpha < 1f)
        {
            //非瞄准时alpha值设置渐变隐藏
            alpha += 1f * Time.deltaTime * time;
        }
        //遍历准心十字Image组件
        foreach (Transform _cross in UIManager.instance.crosshairs)
        {
            //设置准心十字Image组件的颜色
            _cross.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
        }
    }

    //存储武器Aim GameObject对象瞄准的位置
    public void Aim(Vector3[] aimPoint)
    {
        //设置Position
        point = aimPoint[0];
        //设置Rotation
        rotation = Quaternion.Euler(transform.localEulerAngles + aimPoint[1]);
        //设置瞄准状态
        aiming = true;
    }
}
