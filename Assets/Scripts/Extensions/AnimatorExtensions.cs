using System.Linq;
using UnityEngine;

/// <summary>
///     Extensions for the Unity Animator component.
/// </summary>
public static class AnimatorExtensions {
    /// <summary>
    ///     Whether the animator has finished playing.
    /// </summary>
    /// <param name="animator">The animator to check.</param>
    /// <returns>Whether the animator has finished playing.</returns>
    public static bool IsFinished(this Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    public static float GetCurrentTime(this Animator animator, int layer = 0) {
        var animatorState = animator.GetCurrentAnimatorStateInfo(layer);
        return animatorState.normalizedTime % 1;
    }

    public static float GetClipLength(this Animator animator, string clipName) {
        var clips = animator.runtimeAnimatorController.animationClips;
        var clip = clips.FirstOrDefault(clip => clip.name == clipName);
        return clip ? clip.length : -1;
    }
}