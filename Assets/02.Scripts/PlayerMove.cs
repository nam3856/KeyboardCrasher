using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class PlayerMove : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public CameraZoomFeedback CameraZoomFeedback;
    public CameraShake CameraShake;
    public Rigidbody2D Rb;
    public Animator An;
    public Animator [] EffectAn;
    private float x;
    public float MoveSpeed = 5f;
    public int count;
    float lastInputTime;
    float crit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void Start()
    {
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }
    private void Update()
    {
        float inputInterval = Time.time - lastInputTime;
        if (inputInterval > 0.6f)
        {
            An.speed = 1f;
        }
    }
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        int rand = Random.Range(0, 6);
        
        An.SetInteger("rand", rand);
        An.SetTrigger("Attack");
        rand = Random.Range(0, 2);
        float inputInterval = Time.time - lastInputTime;
        float normalizedSpeed = Mathf.Clamp(0.5f / inputInterval, 1f, 5f);
        Debug.Log(normalizedSpeed);
        lastInputTime = Time.time;
        An.speed = normalizedSpeed;

        crit = Random.Range(30f, 50f);
        StartCoroutine(CamZoomCoroutine(crit));

        if (crit <= 35f)
        {
            UI_Game.Instance.ShowCriticalText();
            //CameraShake.Shake();
        }
        UI_Game.Instance.Add();

    }
    public void OnAnimationCompleted(int rand)
    {
        EffectAn[count].SetInteger("rand", rand);
        EffectAn[count].SetTrigger("Attack");
        count = (count + 1) % (EffectAn.Length - 1);
    }
    public void StartCameraZoom(float val)
    {
        if (val == 0)
        {
            StartCoroutine(CamZoomCoroutine(crit));
        }
        else
        {
            StartCoroutine(CamZoomCoroutine(val));
        }
    }

    private IEnumerator CamZoomCoroutine(float rnd)
    {
        yield return new WaitForSeconds(0.1f);
        CameraZoomFeedback.DoZoomEffect(rnd, Random.Range(0.04f, 0.14f));
    }
}
