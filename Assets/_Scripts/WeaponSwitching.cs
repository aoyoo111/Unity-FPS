using UnityEngine;
using System.Collections;

public class WeaponSwitching : MonoBehaviour
{
    //武器切换类
    //定义Animation组件
    public Animation am;
    //定义武器隐藏武器动画剪辑
    public AnimationClip undraw;
    //定义武器对象数组
    public GameObject[] weapons;
    //定义当前武器索引
    private int index = 0;

    void Start()
    {
        //默认显示数组第一个武器
        StartCoroutine(switchW(0f, 0));
    }

    void Update()
    {
        //鼠标滚轮切换
        //if (Input.GetAxis("Mouse ScrollWheel") != 0 && !am.IsPlaying(undraw.name))
        if (Input.GetKeyDown(KeyCode.Q) && !am.IsPlaying(undraw.name))
        {
            //切换下一个武器
            switchWeapon(index + 1);
        }
    }

    //切换武器方法
    void switchWeapon(int _index)
    {
        //当传参值大于武器数组-1的数量
        if (_index > weapons.Length - 1)
        {
            _index = 0;
        }

        //获取当前武器的Weapon组件
        Weapon _w = weapons[index].GetComponent<Weapon>();
        //定义获取到的武器动画片段
        AnimationClip _undraw = _w.undrawA;
        //当武器动画片段等于空
        if (_undraw == null)
        {
            //播放当前类定义的动画
            am.CrossFade(undraw.name);
            //赋值动画
            _undraw = undraw;
        }
        else
        {
            //播放Weapon组件中定义的动画
            _w.am.CrossFade(_undraw.name);
        }

        //武器索引等于传入的索引参数
        index = _index;

        //开启协程执行switchW方法
        StartCoroutine(switchW(_undraw.length, index));

    }

    //延迟切换显示武器协程
    IEnumerator switchW(float second, int _index)
    {
        //等待几秒后执行
        yield return new WaitForSeconds(second);

        //遍历所有武器对象,并且设置禁用不显示
        foreach (GameObject obj in weapons)
        {
            obj.SetActive(false);
        }
        //设置显示当前切换的武器
        weapons[index].SetActive(true);
    }
}
