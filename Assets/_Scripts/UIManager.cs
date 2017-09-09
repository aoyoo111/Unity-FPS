using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("ATH")]
    //血条组件
    public RectTransform healthBar;
    //血量
    public Text health;
    //弹药总量
    public Text totalAmmo;
    //当前弹量
    public Text ammo;

    [Header("武器十字准心")]
    //十字准心image组件数组
    public RectTransform[] crosshairs;
    //定义行走时准心的大小
    private float walkSize;

    //定义UIManager实例变量
    public static UIManager instance;

    void OnEnable()
    {
        //实例化自身
        instance = this;

        //行走时的准心
        walkSize = crosshairs[0].localPosition.y;
    }

    //更新当前弹药数量
    public void UpdateAmmo(int _ammo)
    {
        ammo.text = _ammo.ToString();
    }

    //更新总弹药数量
    public void UpdateTotalAmmo(int _ammo)
    {
        totalAmmo.text = "/ " + _ammo.ToString();
    }

    //更新血量
    public void UpdateHealth(int _health)
    {
        float healthRatio = _health / 100f;

        healthBar.localScale = new Vector3(healthRatio, 0f, 0f);
        health.text = _health.ToString();
    }
    public void Update()
    {
        //更新准心
        UpdateCrosshair();
    }

    //更新十字准心方法
    public void UpdateCrosshair()
    {
        //计算准心大小
        float crossHairSize = calculateCrossHair();
        //设置准心放大效果,Time.deltaTime * 8f更新速度
        crosshairs[0].localPosition = Vector3.Slerp(crosshairs[0].localPosition, new Vector3(0f, crossHairSize, 0f), Time.deltaTime * 8f);
        crosshairs[1].localPosition = Vector3.Slerp(crosshairs[1].localPosition, new Vector3(crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[2].localPosition = Vector3.Slerp(crosshairs[2].localPosition, new Vector3(-crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[3].localPosition = Vector3.Slerp(crosshairs[3].localPosition, new Vector3(0f, -crossHairSize, 0f), Time.deltaTime * 8f);
    }

    //计算十字准心
    public float calculateCrossHair()
    {
        //行走时的准心大小*武器准心设置的大小
        float size = walkSize * Weapon.instance.crossHairSize;

        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                size *= 2f;
            }
        }
        else
        {
            size /= 2f;
        }

        if (Aiming.instance.aiming)
        {
            size /= 20f;
        }
        return size;
    }
}
