using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Basic")]
    public GameObject[] enemies;
    public Transform bossGraphics;
    


    [Header("Stats")]
    public int faceFacing = 0;
    public float rotationDuration = 0.3f;
    public int bossLife = 10;
    public float timeInvulneravility = 10.0f;
    public float alphaDuration = 0.5f;


    private Coroutine rotateCoroutine;
    private int lastFaceFacing = 0;

    private bool invulnerability = false;

    private EnemyType[] bossTypes = { EnemyType.Earth, EnemyType.Fire, EnemyType.Wind, EnemyType.Ice };


    // Start is called before the first frame update
    void Start()
    {
        MessageManager.Instance.RegisterEnemy(this.gameObject);
        lastFaceFacing = faceFacing;
    }

    // Update is called once per frame
    void Update()
    {
        BossFacing();
    }


    void BossFacing()
    {
        this.transform.LookAt(Camera.main.transform.position);
        if (faceFacing != lastFaceFacing)
        {
            if (rotateCoroutine != null)
                StopCoroutine(rotateCoroutine);

            rotateCoroutine = StartCoroutine(RotateOverTime(faceFacing));
            lastFaceFacing = faceFacing;
        }
    }

    IEnumerator RotateOverTime(int faceFacing)
    {
        Quaternion startRotation = bossGraphics.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90f * faceFacing, 0);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            bossGraphics.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossGraphics.localRotation = targetRotation;
    }

    IEnumerator AlphaMaterial(float targetAlpha)
    {
        Renderer[] renderers = bossGraphics.GetComponentsInChildren<Renderer>();

        List<Material> materials = new List<Material>();
        List<float> startAlphas = new List<float>();

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetFloat("_Mode", 2);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                materials.Add(mat);
                startAlphas.Add(mat.color.a);
            }
        }

        float elapsed = 0f;

        while (elapsed < alphaDuration)
        {
            float t = elapsed / alphaDuration;
            for (int i = 0; i < materials.Count; i++)
            {
                Color col = materials[i].color;
                col.a = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                materials[i].color = col;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        
        for (int i = 0; i < materials.Count; i++)
        {
            Color col = materials[i].color;
            col.a = targetAlpha;
            materials[i].color = col;
        }
    }

    IEnumerator Invulnerability()
    {
        invulnerability = true;
        StartCoroutine(AlphaMaterial(0.3f));
        yield return new WaitForSeconds(timeInvulneravility);
        StartCoroutine(AlphaMaterial(1f));
        invulnerability = false;
    }



    public EnemyType GetBossType()
    {
        if (faceFacing >= 0 && faceFacing < bossTypes.Length)
        {
            return bossTypes[faceFacing];
        }
        return EnemyType.Earth;
    }

    public void TakeDamage()
    {
        if (invulnerability) return;

        bossLife--;

        int randomFaceFacing = Random.Range(0, 4);
        while (randomFaceFacing == faceFacing) { randomFaceFacing = Random.Range(0, 4); }
        faceFacing = randomFaceFacing;

        StartCoroutine(Invulnerability());
    }

}
