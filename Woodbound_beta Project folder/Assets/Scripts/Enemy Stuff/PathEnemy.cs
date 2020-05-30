using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Utility.Vector;

[System.Serializable]
public class SplineNode
{
    public SerializableVector2 Point;
    public SerializableVector2 LeftControl;
    public SerializableVector2 RightControl;
}

/// <summary>
/// Variation of MeleeEnemy that follows a path.
/// </summary>
public class PathEnemy : log
{
    [Header("Path Parameters")]
    public SplineNode[] RoughPath = new SplineNode[2];
    [SerializeField]
    private float _distanceUntilNextPoint;
    [SerializeField, Range(0.001f, 1)]
    private float _smoothing = 1;

    [HideInInspector]
    public Vector3[] SmoothPath;

    private Rigidbody2D _targetRigidbody;
    private int _currentSmoothIndex;
    private bool _isFollowingPath;

    private void OnValidate()
    {
        target = GameObject.FindWithTag("Player").transform;
        _targetRigidbody = target.gameObject.GetComponent<Rigidbody2D>();

        if (RoughPath.Length < 2)
        {
            RoughPath = new SplineNode[2];
        }
    }

    public override void CheckDistance()
    {
        float distance = (target.position - transform.position).sqrMagnitude;

        if (distance <= chaseRadius * chaseRadius) // chasable?
        {
            if (distance <= attackRadius * attackRadius) // attackable?
            {
                if (currentState == EnemyState.walk && currentState != EnemyState.stagger)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
                {
                    Vector3 dif = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    // (dif - transform.position) = how far the enemy moved for this frame
                    changeAnim(dif - transform.position);
                    myRigidbody.MovePosition(dif);
                    ChangeState(EnemyState.walk);
                    _targetRigidbody.velocity = Vector2.zero;
                }
            }

            _isFollowingPath = false;
        }
        else
        {
            FollowPath();

            _isFollowingPath = true;
        }
    }

    public IEnumerator Attack()
    {
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
    }

    private void FollowPath()
    {
        if (!_isFollowingPath)
        {
            _currentSmoothIndex = GetClosestRough(transform.position, SmoothPath, RoughPath);
        }

        Vector3 dif = Vector3.MoveTowards(transform.position, SmoothPath[_currentSmoothIndex], moveSpeed * Time.deltaTime);
        // (dif - transform.position) = how far the enemy moved for this frame
        changeAnim(dif - transform.position);
        myRigidbody.MovePosition(dif);
        ChangeState(EnemyState.walk);

        if ((transform.position - SmoothPath[_currentSmoothIndex]).sqrMagnitude < _distanceUntilNextPoint * _distanceUntilNextPoint)
        {
            _currentSmoothIndex += 1;

            if (_currentSmoothIndex > SmoothPath.Length - 1)
            {
                _currentSmoothIndex = 0;
            }
        }
    }

    private static int GetClosestRough(Vector3 position, Vector3[] smooth, SplineNode[] rough)
    {
        float closestDistance = float.PositiveInfinity;
        int closestIndex = 0;

        for (int i = 0; i < rough.Length; i++)
        {
            float distance = (position - rough[i].Point).sqrMagnitude;
            if (distance < closestDistance * closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex * (smooth.Length / rough.Length);
    }

    public void BakeSmooth()
    {
        List<Vector3> result = new List<Vector3>();
        for (float i = 0; i < RoughPath.Length; i += _smoothing)
        {
            int current = (int)i;
            int next = current + 1 > RoughPath.Length - 1 ? 0 : current + 1;

            result.Add(GetPoint(RoughPath[current], RoughPath[next], i - current));
        }

        SmoothPath = result.ToArray();
    }

    private static Vector3 GetPoint(SplineNode left, SplineNode right, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * left.Point +
            3f * oneMinusT * oneMinusT * t * left.RightControl +
            3f * oneMinusT * t * t * right.LeftControl +
            t * t * t * right.Point;
    }
}
