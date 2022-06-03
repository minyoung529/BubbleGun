using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : MonoBehaviour
{
    // ���Ǹ� ���� ���� �� �ڵ��Դϴ�.

    public Shader texturePaint;
    public Shader extendIslands;

    // ������ Ű����
    private int prepareUVID = Shader.PropertyToID("_PrepareUV");
    private int positionID = Shader.PropertyToID("_PainterPosition");
    private int hardnessID = Shader.PropertyToID("_Hardness");
    private int strengthID = Shader.PropertyToID("_Strength");
    private int radiusID = Shader.PropertyToID("_Radius");
    private int colorID = Shader.PropertyToID("_PainterColor");
    private int textureID = Shader.PropertyToID("_MainTex");
    private int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    private int uvIslandsID = Shader.PropertyToID("_UVIslands");

    private Material paintMaterial;
    private Material extendMaterial;
    
    // �׷��� ����� ������ ����...??
    private CommandBuffer command;

    public void Awake()
    {
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);

        command = new CommandBuffer();
    }

    public void InitTextures(Paintable paintable)
    {
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }


    public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null)
    {
        Debug.Log("Painting");
        // Render Texture => ��Ÿ�� �߿� ����, ���ŵǴ� �ؽ���!
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetColor(colorID, color ?? Color.red);
        paintMaterial.SetTexture(textureID, support);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        // paintMaterial�� mask��,
        // rend paintMaterial�� �����Ѵ�
        command.SetRenderTarget(mask);
        command.DrawRenderer(paintable.getRenderer(), paintMaterial, 0);

        // ���� �ִ� support�� mask�� �׸�??
        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        // ���� �߰��� ��ɵ� �� �� �����ϰ� �ʱ�ȭ
        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }
}