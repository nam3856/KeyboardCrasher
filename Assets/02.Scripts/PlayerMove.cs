using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        int rand = Random.Range(0, 6);
        An.SetInteger("rand", rand);
        An.SetTrigger("Attack");
        rand = Random.Range(0, 2);

        EffectAn[count].SetInteger("rand", rand);
        EffectAn[count].SetTrigger("Attack");

        float crit = Random.Range(16f, 50f);
        StartCoroutine(CamZoomCoroutine(crit));

        if (crit <= 25f)
        {
            UI_Game.Instance.Critical();
            CameraShake.Shake();
        }
        UI_Game.Instance.Add();
        count = (count + 1) % (EffectAn.Length-1); 
    }

    private IEnumerator CamZoomCoroutine(float rnd)
    {
        yield return new WaitForSeconds(0.1f);
        CameraZoomFeedback.DoZoomEffect(rnd, Random.Range(0.04f, 0.14f));
    }
}
