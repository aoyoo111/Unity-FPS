using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("武器动画组件")]
    //定义Animation组件
    public Animation am;
    //定义武器隐藏瞄准动画剪辑
    public AnimationClip undrawA;
    //定义武器开火动画剪辑
    public AnimationClip fireA;
    //定义武器换弹夹动画剪辑
    public AnimationClip reloadA;

    [Header("武器属性")]
    //子弹总数
    public int totalAmmo = 120;
    //弹夹容量
    public int clipSize = 30;
    //每次消耗的弹药数量
    private int ammo;

    //上一发子弹发射到下一发子弹冷却间隔时间
    public int cooldown = 11;
    //当前冷却时间
    private int _cooldown = 0;

    //定义开火模式枚举变量,显示到inspector面板中
    public Weapon.fireMode mode;
    //定义开火模式枚举,自动/半自动
    public enum fireMode
    {
        AUTOMATIC,
        SEMI_AUTOMATIC
    }

    [Header("武器射线")]
    //定义相机变量
    public Camera cam;
    //定义射击时产生的枪口火光粒子系统
    public ParticleSystem muzzleFlash;
    //定义射击时产生的效果
    public GameObject hit;

    [Header("武器十字准心")]
    //十字准心大小
    public float crossHairSize;

    [Header("武器瞄准")]
    //定义武器Aim GameObject对象Position和Rotation瞄准的位置
    public Vector3[] aimPoints;

    //定义实例化自身的变量
    public static Weapon instance;

    void OnEnable()
    {
        //实例化自身
        instance = this;

        //实例化UIManager,更新当前武器弹药数量和总弹药数量
        UIManager.instance.UpdateAmmo(clipSize);
        UIManager.instance.UpdateTotalAmmo(totalAmmo);

    }

    void Start()
    {
        //初始化弹药
        ammo = clipSize;
        //重复执行UpdateCooldown方法,0.01f秒后执行,重复执行间隔0.01f
        InvokeRepeating("UpdateCooldown", 0.01f, 0.01f);
    }

    //更新冷却
    void UpdateCooldown()
    {
        if (_cooldown > 0)
            _cooldown--;
    }

    //换弹夹方法
    void reload()
    {
        //当弹药等于弹夹容量时返回false停止执行
        if (ammo == clipSize)
            return;

        //播放换弹夹动画,动画淡入切换
        am.CrossFade(reloadA.name);
        //当弹药总数>=弹夹容量-当前弹药数量
        if (totalAmmo >= (clipSize - ammo))
        {
            //更新弹药总数
            totalAmmo -= (clipSize - ammo);
            //更新当前弹药数量
            ammo += (clipSize - ammo);
        }
        else
        {
            ammo += totalAmmo;
            totalAmmo = 0;
        }

        //实例化UIManager,更新当前武器弹药数量和总弹药数量
        UIManager.instance.UpdateAmmo(ammo);
        UIManager.instance.UpdateTotalAmmo(totalAmmo);
    }

    //开火方法
    void fire()
    {
        //当弹药<=0时返回false停止执行
        if (ammo <= 0 || Input.GetKey(KeyCode.LeftShift))
            return;

        //停止正在播放的动画
        am.Stop();
        //播放武器开火动画
        am.Play(fireA.name);

        //生成子弹效果
        RaycastHit _hit;
        //获取角色摄像机屏幕中心点,屏幕中心点+ Random.Range(-UIManager.instance.calculateCrossHair()/1.4f, UIManager.instance.calculateCrossHair() / 1.4f)表示在十字准心内随机弹道
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2 + Random.Range(-UIManager.instance.calculateCrossHair()/1.4f, UIManager.instance.calculateCrossHair() / 1.4f), Screen.height / 2 + Random.Range(-UIManager.instance.calculateCrossHair() / 1.4f, UIManager.instance.calculateCrossHair() / 1.4f), 0f));
        if (Physics.Raycast(ray, out _hit, 5000))
        {
            //5秒后销毁射击时创建的射击到物体效果
            Destroy(GameObject.Instantiate(hit, _hit.point, Quaternion.Euler(_hit.normal)), 5f);
            //播放枪口火光效果
            muzzleFlash.Play();
        }

        //当前冷却时间等于冷却时间
        _cooldown = cooldown;

        //弹药减少
        ammo -= 1;

        //实例化UIManager,更新当前弹药数量
        UIManager.instance.UpdateAmmo(ammo);
    }

    void Update()
    {
        //触发鼠标右键按下还显示武器瞄准原角度
        if (Input.GetMouseButtonDown(1))

            Aiming.instance.Aim(aimPoints);

        //Debug.Log(ammo + " " + clipSize + " " + totalAmmo);
        //当处于自动模式和鼠标右键点击时和冷却时间<=0时执行开火
        if (mode == Weapon.fireMode.AUTOMATIC && Input.GetMouseButton(0) && _cooldown <= 0)
        {
            fire();
        }
        //当处于半自动模式和鼠标右键按下时和冷却时间<=0时执行开火
        else if (mode == Weapon.fireMode.SEMI_AUTOMATIC && Input.GetMouseButtonDown(0) && _cooldown <= 0)
        {
            fire();
        }
        //判断当前没有播放开火,取消瞄准,换弹夹动画时执行换弹夹方法
        if (!am.IsPlaying(fireA.name) && ((undrawA != null && !am.IsPlaying(undrawA.name)) || undrawA == null) && !am.IsPlaying(reloadA.name) && Input.GetKey(KeyCode.R))
        {
            reload();
        }
    }
}
