using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    [SerializeField] private PictureShape shape = PictureShape.None;
    [SerializeField] private PictureColor color = PictureColor.White;

    public PictureShape GetShape() { return shape; }
    public void SetShape(PictureShape shape) { this.shape = shape; }
    public PictureColor GetColor() { return color; }
    public void SetColour(PictureColor color) { this.color = color; }
}

public enum PictureShape
{
    None,
    Square,
    Triangle,
    Circle
}

public enum PictureColor
{
    White,
    Red,
    Blue,
    Green
}
