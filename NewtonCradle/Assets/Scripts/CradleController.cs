using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Link;
using Ball;

public class CradleController : MonoBehaviour
{
    [SerializeField][Range(0.25f, 5f)] private float mass = 1f;
    [SerializeField][Range(1f, 10f)] private float ballDistance = 2f;
    [SerializeField] private bool debug = true;

    private List<ILink> links = new List<ILink>();
    private List<IBall> balls = new List<IBall>();

    private void Awake() => this.Setup();

    private void Setup()
    {
        this.links = this.GetComponentsInChildren<ILink>().ToList();
        this.balls = this.GetComponentsInChildren<IBall>().ToList();

        if (!this.balls.Any())
            throw new System.NullReferenceException("Balls not found");
        if (!this.links.Any())
            throw new System.NullReferenceException("Links not found");

        this.LinkBalls();

        if (this.balls.Count() != this.balls.Count())
            throw new System.Exception("Diffrent number balls and links");
    }

    private void LinkBalls()
    {
        for (int i = 0; i < this.links.Count(); i++)
        {
            var link = this.links.ElementAt(i);
            var ball = this.balls.ElementAt(i);
            ball.Setup(this.mass);
            link.Setup(ball.Rigidbody, this.ballDistance);
        }
    }

    private void OnDrawGizmos()
    {
        if (this.debug)
        {
            for (int i = 0; i < this.balls.Count(); i++)
            {
                Gizmos.DrawLine(this.links.ElementAt(i).Transform.position, this.balls.ElementAt(i).Transform.position);
            }
        }
        
    }
}

