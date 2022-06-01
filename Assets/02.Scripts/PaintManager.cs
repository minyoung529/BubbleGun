using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : MonoBehaviour
{
    // 강의를 보며 따라 쓴 코드입니다.

    public Shader texturePaint;
    public Shader extendIslands;

    // 변수의 키값들
    private int prepareUVID = Shader.PropertyToID("_PrepareUV");
    private int positionID = Shader.PropertyToID("_PainterPosition");
    private int hardnessID = Shader.PropertyToID("_Hardness");
    private int strengthID = Shader.PropertyToID("_Strength");
    private int radiusID = Shader.PropertyToID("_Radius");
    private int colorID = Shader.PropertyToID("_PainterColor");
    private int textureID = Shader.PropertyToID("_MainTex");
    
    private Material paintMaterial;
    private Material extendMaterial;
    
    // 그래픽 명령을 실행할 버퍼...??
    private CommandBuffer command;

    public void Awake()
    {
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);

        command = new CommandBuffer();
    }

    public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null)
    {
        // Render Texture => 런타임 중에 생성, 갱신되는 텍스쳐!
        RenderTexture mask = paintable.getMask();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetColor(colorID, color ?? Color.red);
        paintMaterial.SetTexture(textureID, support);

        // paintMaterial에 mask를,
        // rend paintMaterial에 렌더한다
        command.SetRenderTarget(mask);
        command.DrawRenderer(paintable.getRenderer(), paintMaterial, 0);

        // 원래 있던 support를 mask에 그림??
        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        // 위에 명령들 싹 다 함
        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }
}