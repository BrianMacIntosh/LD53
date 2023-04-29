using UnityEngine;

public class OrdersCamera : MonoBehaviour
{
	private Camera m_camera;

	[SerializeField]
	private Material m_screenMaterial;

	public RenderTexture RenderTexture
	{
		get; private set;
	}

	public Material ScreenMaterial
	{
		get { return m_screenMaterial; }
	}

	private int m_textureHeight = 720;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();

		RenderTexture = new RenderTexture(Mathf.FloorToInt(m_camera.aspect * m_textureHeight), m_textureHeight, 0);

		m_camera.targetTexture = RenderTexture;
		m_screenMaterial.mainTexture = RenderTexture;
	}
}
