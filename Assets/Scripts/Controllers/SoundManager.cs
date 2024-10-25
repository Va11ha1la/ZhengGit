using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;
    private AudioSource loopingAudioSource; // 用于循环播放音效
    private Dictionary<string, AudioClip> dictAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            dictAudio = new Dictionary<string, AudioClip>();
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        dictAudio = new Dictionary<string, AudioClip>();

        // 创建一个新的 AudioSource 用于循环播放
        loopingAudioSource = gameObject.AddComponent<AudioSource>();
        loopingAudioSource.loop = true; // 设置为循环播放
    }

    // 加载音频
    public AudioClip LoadAudio(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError($"音频文件未找到: {path}");
        }
        return clip;
    }

    // 获取并且缓存音频到 dict
    private AudioClip GetAudio(string path)
    {
        if (!dictAudio.ContainsKey(path))
        {
            AudioClip clip = LoadAudio(path);
            if (clip != null)
            {
                dictAudio[path] = clip;
            }
        }

        // 确保路径在字典中存在
        if (dictAudio.ContainsKey(path))
        {
            return dictAudio[path];
        }
        else
        {
            Debug.LogError($"获取音频时出错: {path} 不在字典中。");
            return null;
        }
    }

    // 播放背景音乐
    public void PlayBGM(string name, float volume = 1.0f,bool canLoop = true)
    {
        AudioClip clip = GetAudio(name);
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = canLoop;
            audioSource.Play();
           
        }
    }

    // 停止背景音乐
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // 播放音效
    public void PlaySound(string path, float volume = 1f,bool canLoop = true)
    {
        AudioClip clip = GetAudio(path);
        if (clip != null)
        {
            audioSource.loop = canLoop;
            audioSource.PlayOneShot(clip, volume);
        }
    }
    public void PlayLoopingSound(string path, float volume = 1f)
    {
        loopingAudioSource.clip = GetAudio(path);
        loopingAudioSource.volume = volume;
        loopingAudioSource.Play();
    }

    // 设置背景音乐音量
    public void SetBGMVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // 设置音效音量
    public void SetSFXVolume(float volume)
    {
        // 这可以根据你的需求调整音效的音量
    }
}
