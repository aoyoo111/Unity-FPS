using UnityEngine;
using System.Collections;

//如果没有添加Animation组件,系统将自动添加
[RequireComponent(typeof(Animation))]
public class WeaponAnimations : MonoBehaviour
{
    //角色操作武器动画控制类
    //定义Animation组件
    public Animation am;
    //定义武器Idle时动画剪辑
    public AnimationClip idle;
    //定义武器行走时动画剪辑
    public AnimationClip walk;
    //定义武器奔跑时动画剪辑
    public AnimationClip run;
    //定义武器隐藏时动画剪辑
    public AnimationClip undraw;

    void Update()
    {
        if (am.IsPlaying(undraw.name))
            return;

        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //播放奔跑动画
                am.CrossFade(run.name);
            }
            else
            {
                //播放行走动画
                am.CrossFade(walk.name);
            }
        }
        else
        {
            //播放Idle动画
            am.CrossFade(idle.name);
        }
    }
}
