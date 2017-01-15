using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKohnanGenerator : MonoBehaviour
{
	public GameObject itemKohnan;
	public GameObject itemVodafon;
	public float generateInterval  = 10.0f;
	public int everyGenerateVodafon = 5;
	public int maxItemKohnan  = 100;
	public int maxItemVodafon  = 20;
	private float count	= 0.0f;
	private int generateCount	= 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update ()
	{
		count -= Time.deltaTime;
		if( count < 0.0f )
		{
			count = generateInterval;

			// 指定した数毎にボーダフォン出現.
			if( generateCount % everyGenerateVodafon == 0 )
			{
				// ボーダフォンアイテム数が上限に達していなければ生成.
				GameObject[] tagObjs = GameObject.FindGameObjectsWithTag( "ItemVodafon" );
				if( tagObjs.Length < maxItemVodafon )
				{
					Instantiate( itemVodafon, transform.position, Quaternion.identity );
					generateCount++;
				}
			}
			else
			{
				// コーナンアイテム数が上限に達していなければ生成.
				GameObject[] tagObjs = GameObject.FindGameObjectsWithTag( "ItemKohnan" );
				if( tagObjs.Length < maxItemKohnan )
				{
					Instantiate( itemKohnan, transform.position, Quaternion.identity );
					generateCount++;
				}
			}
		}
	}
}
