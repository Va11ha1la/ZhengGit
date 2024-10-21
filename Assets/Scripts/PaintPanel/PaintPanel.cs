using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
public class PaintPanel : MonoBehaviour
{
    public Texture2D brush;//画板笔刷
    private SpriteRenderer spriteRenderer;//画板的~
    private int index = 0;//当前第几幅画
    private float _time = 0;//存档定时
    public float brushSize = 1;
    private void Start()
    {
        //克隆新的纹理来画，原纹理原封不动作为每次的模版
        spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D newTexture2D = CopyTexture(spriteRenderer.sprite.texture);
        Sprite newSprite = Sprite.Create(newTexture2D, new Rect(0, 0, newTexture2D.width, newTexture2D.height), new Vector2(0.5f, 0.5f));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSprite;
    }
    void Update()
     {
         _time += Time.deltaTime;
         if (_time > 5)
         {
             _time = 0;
             SaveTexture(Application.persistentDataPath+$"/WritingBackup/writing{index}.png",spriteRenderer.sprite.texture);
         }
         if (Input.GetMouseButton(0))
         {
             PaintTrack();
         }
         else if(Input.GetMouseButtonUp(0))
         {
             _isNewLine = true;
         }
     }
    /// <summary>
    /// 克隆新的纹理图
    /// </summary>
    /// <param name="originalTexture"></param>
    /// <returns></returns>
    private Texture2D CopyTexture(Texture2D originalTexture)
    {
        // 创建新的纹理
        Texture2D clonedTexture = new Texture2D(originalTexture.width, originalTexture.height);
        // 复制像素数据
        Color[] pixels = originalTexture.GetPixels();
        clonedTexture.SetPixels(pixels);
        clonedTexture.Apply(); // 应用更改

        return clonedTexture;
    }
    /// <summary>
    /// 存纹理图到硬盘
    /// </summary>
    /// <param name="path"></param>
    /// <param name="textureToSave"></param>
    public void SaveTexture(string path,Texture2D textureToSave)
    {
        // 确保纹理已应用
        textureToSave.Apply();

        // 将Texture2D转换为PNG格式的字节数组
        byte[] textureData = textureToSave.EncodeToPNG();

        // 获取目录路径
        string directory = Path.GetDirectoryName(path);

        // 确保目录存在
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        // 保存文件
        File.WriteAllBytes(path, textureData);
        Debug.Log($"Texture saved to: {path}");
    }

    private bool _isNewLine = true;
    private Vector3 _lastPosition;
    void PaintTrack()
    {
        //获取目标点
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hit,20f))
        {
            if (hit.transform == transform)
            {
                if (_isNewLine)
                {
                    _lastPosition=hit.point;
                    _isNewLine = false;
                }
                DrawLine(hit.point,_lastPosition);
            }
            _lastPosition = hit.point;
        }
    }
    void DrawLine(Vector3 start, Vector3 end)
    {
        int steps = 40; // 可以根据需要调整此值
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector3 point = Vector3.Lerp(start, end, t);
            PaintOnSprite(point);
        }
    }
    void PaintOnSprite(Vector3 hitPoint)
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        // 获取Sprite的尺寸
        float spriteWidth = sprite.bounds.size.x * spriteRenderer.transform.localScale.x;
        float spriteHeight = sprite.bounds.size.y * spriteRenderer.transform.localScale.y;

        // 计算局部坐标
        Vector2 localPoint = (Vector2)hitPoint - (Vector2)spriteRenderer.transform.position;
        
        int brushWidth=(int)(brush.width*brushSize);
        int brushHeight = (int)(brushSize * brush.height);
        
        // 计算在Sprite纹理中的对应像素位置
        int pixelX = (int)((localPoint.x / spriteWidth + 0.5f) * sprite.texture.width);
        int pixelY = (int)((localPoint.y / spriteHeight + 0.5f) * sprite.texture.height);
        // 确保像素位置在范围内
        if (pixelX >= 0 && pixelX < texture.width && pixelY >= 0 && pixelY < texture.height)
        {
            // 获取要替换的颜色
            Color[] overlayPixels = brush.GetPixels();
            // 在指定位置替换像素
            for (int x = 0; x < brushWidth; x++)
            {
                for (int y = 0; y < brushHeight; y++) 
                {
                    int targetX =pixelX - brushWidth / 2+x;
                    int targetY =pixelY - brushHeight / 2+y;

                    if (targetX >= 0 && targetX < texture.width && targetY >= 0 && targetY < texture.height)
                    {
                        texture.SetPixel(targetX, targetY, overlayPixels[x + y * brushWidth]);
                    }
                }
            }
        }
        texture.Apply();
    }
}

