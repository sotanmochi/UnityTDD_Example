using System;
using System.Diagnostics;

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

    private Stopwatch _stopwatch = new Stopwatch();

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
        _isPressed = true;
        _stopwatch.Start();
    }

    /// <summary>
    /// ボタンを離す
    /// </summary>
    public void SetButtonUp()
    {
        if (_isPressed && _isEntered)
        {
            _isPressed = false;

            _stopwatch.Stop();
            var elapsedSeconds = _stopwatch.ElapsedMilliseconds / 1000f;

            if (elapsedSeconds < _longPressDuration)
            {
                OnTapped?.Invoke();
            }
            else
            {
                OnLongPressed?.Invoke();
            }
        }
    }

    /// <summary>
    /// ボタンの領域に入る
    /// </summary>
    public void SetButtonEnter()
    {
        _isEntered = true;
    }

    /// <summary>
    /// ボタンの領域から出る
    /// </summary>
    public void SetButtonExit()
    {
        _isEntered = false;
    }
}