#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class DEV_SCREENSHOOT_QUEUE : MonoBehaviour
{
	public List<Transform> screenShoots;

	public Transform screenShoot;
	public DEV_RENDER_TEXTURE_EXPORTER renderTextureExporter;

	private int index = 0;

	private EditorCoroutine _coroutine;

	[Button]
	public void DEV_SCREEN_SHOOT_ALL()
	{
		_coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(ExportWithDelay());
	}
	
	private IEnumerator ExportWithDelay()
	{
		for (int i = 0; i < screenShoots.Count; i++)
		{
			screenShoot.SetParent(screenShoots[i]);
			screenShoot.localPosition = new Vector3(0, screenShoot.position.y, screenShoot.position.z);
			
			yield return new EditorWaitForSeconds(.25f);
			
			renderTextureExporter.ExportRenderTexture();
			
			yield return new EditorWaitForSeconds(.15f);
		}
	}

	[Button]
	public void DEV_FORCE_STOP()
	{
		EditorCoroutineUtility.StopCoroutine(_coroutine);
	}
}

#endif
