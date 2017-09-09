using UnityEngine;
using System.Collections;

//如果没有添加Animation组件,系统将自动添加
[RequireComponent(typeof(Animation))]
public class PlayerAnimations : MonoBehaviour
{
    //角色行动动画控制类
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

    public Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {
        if (rb.velocity.magnitude >= 0.1)
        {
            playAnim(walk.name);
        }
        else
        {
            playAnim(idle.name);
        }
    }

    //播放动画函数
    public void playAnim(string animName)
    {
        am.CrossFade(animName);
    }

    //停止动画函数
    public void stopAnim()
    {
        am.Stop();
    }
}
