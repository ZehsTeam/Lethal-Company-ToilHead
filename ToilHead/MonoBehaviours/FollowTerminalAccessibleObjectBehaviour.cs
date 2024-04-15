using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace com.github.zehsteam.ToilHead.MonoBehaviours;

public class FollowTerminalAccessibleObjectBehaviour : NetworkBehaviour
{
    public string objectCode;
    public float codeAccessCooldownTimer;
    private float currentCooldownTimer;
    private bool inCooldown;

    public InteractEvent terminalCodeEvent;
    public InteractEvent terminalCodeCooldownEvent;

    [Space(3f)]
    public MeshRenderer[] codeMaterials;
    public int rows;
    public int columns;

    private TextMeshProUGUI mapRadarText;
    private Image mapRadarBox;
    private bool initializedValues;

    // Custom Variables
    private RectTransform rectTransform;

    private void Start()
    {
        InitializeValues();
    }

    public void InitializeValues()
    {
        if (initializedValues) return;
        initializedValues = true;
        
        GameObject gameObject = Object.Instantiate(StartOfRound.Instance.objectCodePrefab, StartOfRound.Instance.mapScreen.mapCameraStationaryUI, worldPositionStays: false);
        rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.position = transform.position + Vector3.up * 4.35f;
        rectTransform.position += rectTransform.up * 1.2f - rectTransform.right * 1.2f;

        mapRadarText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        mapRadarText.text = objectCode;
        mapRadarBox = gameObject.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (rectTransform == null) return;

        rectTransform.position = transform.position + Vector3.up * 4.35f;
        rectTransform.position += rectTransform.up * 1.2f - rectTransform.right * 1.2f;
    }

    public void CallFunctionFromTerminal()
    {
        if (!inCooldown)
        {
            terminalCodeEvent.Invoke(GameNetworkManager.Instance.localPlayerController);

            if (codeAccessCooldownTimer > 0f)
            {
                currentCooldownTimer = codeAccessCooldownTimer;
                StartCoroutine(countCodeAccessCooldown());
            }

            Debug.Log("calling terminal function for code : " + objectCode + "; object name: " + gameObject.name);
        }
    }

    public void TerminalCodeCooldownReached()
    {
        terminalCodeCooldownEvent.Invoke(null);
        Debug.Log("cooldown reached for object with code : " + objectCode + "; object name: " + gameObject.name);
    }

    private IEnumerator countCodeAccessCooldown()
    {
        inCooldown = true;
        if (!initializedValues)
        {
            InitializeValues();
        }
        Image cooldownBar = mapRadarBox;
        Image[] componentsInChildren = mapRadarText.gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            if (componentsInChildren[i].type == Image.Type.Filled)
            {
                cooldownBar = componentsInChildren[i];
            }
        }
        cooldownBar.enabled = true;
        mapRadarText.color = Color.red;
        mapRadarBox.color = Color.red;
        while (currentCooldownTimer > 0f)
        {
            yield return null;
            currentCooldownTimer -= Time.deltaTime;
            cooldownBar.fillAmount = currentCooldownTimer / codeAccessCooldownTimer;
        }
        TerminalCodeCooldownReached();
        mapRadarText.color = Color.green;
        mapRadarBox.color = Color.green;
        currentCooldownTimer = 1.5f;
        int frameNum = 0;
        while (currentCooldownTimer > 0f)
        {
            yield return null;
            currentCooldownTimer -= Time.deltaTime;
            cooldownBar.fillAmount = Mathf.Abs(currentCooldownTimer / 1.5f - 1f);
            frameNum++;
            if (frameNum % 7 == 0)
            {
                mapRadarText.enabled = !mapRadarText.enabled;
            }
        }
        mapRadarText.enabled = true;
        cooldownBar.enabled = false;
        inCooldown = false;
    }

    public void SetCodeTo(int codeIndex)
    {
        if (codeIndex > RoundManager.Instance.possibleCodesForBigDoors.Length)
        {
            Debug.LogError("Attempted setting code to an index higher than the amount of possible codes in TerminalAccessibleObject");
            return;
        }
        objectCode = RoundManager.Instance.possibleCodesForBigDoors[codeIndex];
        SetMaterialUV(codeIndex);
        if (mapRadarText == null)
        {
            InitializeValues();
        }
        mapRadarText.text = objectCode;
    }

    public override void OnDestroy()
    {
        if (mapRadarText != null && mapRadarText.gameObject != null)
        {
            Destroy(mapRadarText.gameObject);
        }

        base.OnDestroy();
    }

    private void SetMaterialUV(int codeIndex)
    {
        float num = 0f;
        float num2 = 0f;
        for (int i = 0; i < codeIndex; i++)
        {
            num += 1f / (float)columns;
            if (num >= 1f)
            {
                num = 0f;
                num2 += 1f / (float)rows;
                if (num2 >= 1f)
                {
                    num2 = 0f;
                }
            }
        }
        if (codeMaterials != null && codeMaterials.Length != 0)
        {
            Material material = codeMaterials[0].material;
            material.SetTextureOffset("_BaseColorMap", new Vector2(num, num2));
            for (int j = 0; j < codeMaterials.Length; j++)
            {
                codeMaterials[j].sharedMaterial = material;
            }
        }
    }
}
