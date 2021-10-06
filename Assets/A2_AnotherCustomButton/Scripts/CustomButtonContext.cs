using System;

public class CustomButtonContext
{
    // イベント
    public event Action OnTapped;
    public event Action OnLongPressed;
    public event Action OnPressed;
    public event Action OnReleased;

    // 状態
    private bool _isPressed;
    private bool _isEntered;
    private float _longPressDuration = 1.0f;

    /// <summary>
    /// 長押しの時間を設定する
    /// </summary>
    /// <param name="durationSeconds"></param>
    public void SetLongPressDuration(float durationSeconds)
    {
        _longPressDuration = durationSeconds;
    }

    /// <summary>
    /// ボタンを押す
    /// </summary>
    public void SetButtonDown()
    {

    }

    /// <summary>
    /// ボタンを離す
    /// </summary>
    public void SetButtonUp()
    {

    }

    /// <summary>
    /// ボタンの領域に入る
    /// </summary>
    public void SetButtonEnter()
    {

    }

    /// <summary>
    /// ボタンの領域から出る
    /// </summary>
    public void SetButtonExit()
    {

    }
}