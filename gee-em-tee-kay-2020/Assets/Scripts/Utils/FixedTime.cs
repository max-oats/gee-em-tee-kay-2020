using UnityEngine;

public class FixedTime : MonoBehaviour
{
    public static int fixedFrameCount = 0;

    private void FixedUpdate()
    {
        fixedFrameCount++;
    }

    public static bool ShouldRun(int offset, int every)
    {
        return (fixedFrameCount + offset) % every == 0;
    }
}