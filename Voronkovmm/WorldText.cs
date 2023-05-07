using UnityEngine;

namespace Voronkovmm
{
    public static class WorldText
    {
        public static TextMesh Create(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

            Transform transform = gameObject.transform
                .With(x => x.SetParent(parent, false))
                .With(x => x.localPosition = localPosition);

            TextMesh textMesh = gameObject.GetComponent<TextMesh>()
                .With(x => x.anchor = textAnchor)
                .With(x => x.alignment = textAlignment)
                .With(x => x.text = text)
                .With(x => x.fontSize = fontSize)
                .With(x => x.color = color)
                .With(x => x.GetComponent<MeshRenderer>().sortingOrder = sortingOrder);

            return textMesh;
        }
    } 
}
