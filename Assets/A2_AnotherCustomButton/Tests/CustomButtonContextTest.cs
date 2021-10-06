using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;

public class CustomButtonContextTest
{
    class TapEventTest
    {
        private CustomButtonContext _context;
        private int _eventCount;

        [SetUp]
        public void SetUp()
        {
            _eventCount = 0;
            _context = new CustomButtonContext();
        }

        [Test]
        public void ボタンを押して離したらTapイベントが発行される()
        {
            _context.OnTapped += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(1));
        }

        [Test]
        public void ボタンを押した後ボタンから指をずらした状態で離すとTapイベントは発行されない()
        {
            _context.OnTapped += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonExit();
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(0));
        }

        [Test]
        public void ボタンを押した後ボタンから指をずらして再び戻ってきた状態で離すとTapイベントは発行される()
        {
            _context.OnTapped += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonExit();
            _context.SetButtonEnter();
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator LongPressイベントが発行されたあとに指を離してもTapイベントは発行されない() => UniTask.ToCoroutine(async() => 
        {
            _context.OnTapped += () => _eventCount++;
            _context.SetLongPressDuration(1.0f);

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(0));
        });
    }

    class LongPressEventTest
    {
        private CustomButtonContext _context;
        private int _eventCount;

        [SetUp]
        public void SetUp()
        {
            _eventCount = 0;
            _context = new CustomButtonContext();
        }

        [UnityTest]
        public IEnumerator 任意の秒数押し続けたらLongPressイベントが発行される() => UniTask.ToCoroutine(async() => 
        {
            var durationSeconds = 2.0f;
            _context.SetLongPressDuration(durationSeconds);
            _context.OnLongPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds + 0.1f));
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(1));
        });

        [UnityTest]
        public IEnumerator 任意の秒数押し続けていない場合LongPressイベントは発行されない() => UniTask.ToCoroutine(async() =>
        {
            var durationSeconds = 2.0f;
            _context.SetLongPressDuration(durationSeconds);
            _context.OnLongPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds - 0.1f));
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(0));
        });

        [UnityTest]
        public IEnumerator 長押し中に指をずらした場合LongPressイベントは発行されない() => UniTask.ToCoroutine(async() =>
        {
            var durationSeconds = 2.0f;
            _context.SetLongPressDuration(durationSeconds);
            _context.OnLongPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds / 2.0f));
            _context.SetButtonExit();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds / 2.0f));
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(0));
        });
    }

    class AnimationEventTest
    {
        private CustomButtonContext _context;
        private int _eventCount;

        [SetUp]
        public void SetUp()
        {
            _eventCount = 0;
            _context = new CustomButtonContext();
        }

        [Test]
        public void ボタンを押した瞬間にPressアニメーションが再生される()
        {
            _context.OnPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();

            Assert.That(_eventCount, Is.EqualTo(1));
        }

        [Test]
        public void ボタンを離した瞬間にReleaseアニメーションが再生される()
        {
            _context.OnReleased += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(1));
        }

        [Test]
        public void ボタンを押した後ボタンから指をずらした場合Releaseアニメーションが再生される()
        {
            _context.OnReleased += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonExit();

            Assert.That(_eventCount, Is.EqualTo(1));
        }

        [Test]
        public void ボタンを押した後ボタンから指をずらして再び戻ってきた場合Pressedアニメーションが再生される()
        {
            _context.OnPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonDown();
            _context.SetButtonExit();
            _context.SetButtonEnter();

            Assert.That(_eventCount, Is.EqualTo(2));
        }

        [Test]
        public void ボタン以外を押した状態で指をずらした場合Pressedアニメーションが再生されない()
        {
            _context.OnPressed += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonExit();

            Assert.That(_eventCount, Is.EqualTo(0));
        }

        [Test]
        public void ボタン以外を押した状態で指をずらした場合Releasedアニメーションが再生されない()
        {
            _context.OnReleased += () => _eventCount++;

            _context.SetButtonEnter();
            _context.SetButtonExit();

            Assert.That(_eventCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator 長押しに成功したときReleaseアニメーションが再生される() => UniTask.ToCoroutine(async() =>
        {
            _context.OnReleased += () => _eventCount++;

            var durationSeconds = 2.0f;
            _context.SetLongPressDuration(durationSeconds);

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds + 0.1f));

            _context.SetButtonUp();

            Assert.That(_eventCount, Is.EqualTo(1));
        });

        [UnityTest]
        public IEnumerator 長押しに成功してReleaseアニメーションが再生された場合指を離すまでアニメーションが再生されない()  => UniTask.ToCoroutine(async() =>
        {
            _context.OnReleased += () => _eventCount++;

            var durationSeconds = 2.0f;
            _context.SetLongPressDuration(durationSeconds);

            _context.SetButtonEnter();
            _context.SetButtonDown();
            await UniTask.Delay(TimeSpan.FromSeconds(durationSeconds + 0.1f));

            _context.SetButtonExit();
            _context.SetButtonEnter();

            Assert.That(_eventCount, Is.EqualTo(0));
        });
    }
}
