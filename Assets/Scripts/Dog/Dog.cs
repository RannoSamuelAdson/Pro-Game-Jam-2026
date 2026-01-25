using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject _followTarget;
    public float _distance;
    public float _speed;
    private Vector3 offset;
    private Animator animator;

    public void Init(float distance, float speed, GameObject followTarget)
    {
        _distance = distance;
        _speed = Random.Range(0.02f, _speed);
        _followTarget = followTarget;
        animator = GetComponent<Animator>();
        animator.speed = 0f;
        Debug.Log("Dog spawn");
    }

    void Start()
    {
        offset = new Vector3(Random.Range(0, _distance), Random.Range(0, _distance), Random.Range(0, _distance + 0.5f));
        transform.position = _followTarget.transform.position + offset;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _followTarget.transform.position) > _distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _followTarget.transform.position + offset, _speed);
            animator.speed = 1f;
            animator.SetFloat("Horizontal", transform.position.x * 10f);
            animator.SetFloat("Vertical", transform.position.y * 10f);
        }
        else
        {
            animator.speed = 0f;
        }

    }


}
