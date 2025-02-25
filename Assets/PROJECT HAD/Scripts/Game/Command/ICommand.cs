using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public static class Awaitable
    {
        public static async Task WaitForSecondsAsync(float seconds)
        {
            var tcs = new TaskCompletionSource<bool>();
            MonoBehaviour dummy = new GameObject("Awaitable").AddComponent<DummyMonoBehaviour>();
            dummy.StartCoroutine(Wait(seconds, tcs, dummy));
            await tcs.Task;
        }

        private static IEnumerator Wait(float seconds, TaskCompletionSource<bool> tcs, MonoBehaviour dummy)
        {
            yield return new WaitForSeconds(seconds);
            tcs.SetResult(true);
            Object.Destroy(dummy.gameObject);
        }

        private class DummyMonoBehaviour : MonoBehaviour { }
    }

    public interface ICommand
    {
        Task Execute();
    }
}
