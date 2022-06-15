using UnityEngine;

public class Paintable : MonoBehaviour
{
    private const int TEXTURE_SIZE = 1024;

    public float extendsIslandOffset = 1;

    private RenderTexture extendIslandsRenderTexture;
    private RenderTexture uvIslandsRenderTexture;
    private RenderTexture maskRenderTexture;
    private RenderTexture supportTexture;

    private Renderer rend;

    private int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture getMask() => maskRenderTexture;
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;
    public RenderTexture getExtend() => extendIslandsRenderTexture;
    public RenderTexture getSupport() => supportTexture;
    public Renderer getRenderer() => rend;

    void Start()
    {
        maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        supportTexture.filterMode = FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        if (gameObject.layer == LayerMask.GetMask("PLATFORM"))
        {
            Debug.Log("fsd");
            rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);
        }
        else
        {
            rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);
        }

        GameManager.Instance.PaintManager.InitTextures(this);
    }

    void OnDisable()
    {
        maskRenderTexture?.Release();
        uvIslandsRenderTexture?.Release();
        extendIslandsRenderTexture?.Release();
        supportTexture?.Release();
    }
}