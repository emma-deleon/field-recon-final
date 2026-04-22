using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Splines;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class TMPSplineText : MonoBehaviour
{
    public SplineContainer splineContainer;

    [Range(0f, 1f)]
    public float startOffset = 0f;
    public float spacingScale = 1f;
    public float rollOffset = 0f;

    private TMP_Text _tmp;

    void OnEnable() => _tmp = GetComponent<TMP_Text>();

    void LateUpdate()
    {
        if (splineContainer == null) return;
        var spline = splineContainer.Spline;
        if (spline == null || spline.Count < 2) return;

        PlaceCharacters(spline);
    }

    void PlaceCharacters(Spline spline)
    {
        _tmp.ForceMeshUpdate();
        var textInfo = _tmp.textInfo;
        if (textInfo.characterCount == 0) return;

        float boundsMinX = _tmp.bounds.min.x;
        float textWidth = _tmp.bounds.max.x - boundsMinX;
        if (textWidth < 0.0001f) return;

        float splineLength = spline.GetLength();
        float startDist = startOffset * splineLength;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vi = charInfo.vertexIndex;
            int mi = charInfo.materialReferenceIndex;
            var verts = textInfo.meshInfo[mi].vertices;

            // calc rotation point
            float3 center = new float3(
                (verts[vi].x + verts[vi + 2].x) * 0.5f,
                charInfo.baseLine,
                0f
            );

            // convert world position to spline position
            float normX = (center.x - boundsMinX) / textWidth;
            float dist = math.clamp(startDist + normX * textWidth * spacingScale, 0f, splineLength);
			// Calculate the distance and use % (Modulo) to wrap it around the total length
			float wrappedDist = (startDist + normX * textWidth * spacingScale) % splineLength;

			// If the number goes negative (sliding backwards), wrap it to the end
			if (wrappedDist < 0) wrappedDist += splineLength;

			float t = SplineUtility.ConvertIndexUnit(spline, wrappedDist, PathIndexUnit.Distance, PathIndexUnit.Normalized);

            splineContainer.Evaluate(t, out float3 pos, out float3 tan, out float3 up);

            // create the rotation for each character from the tan and up info
            quaternion rot = quaternion.LookRotationSafe(tan, up);
            if (math.abs(rollOffset) > 0.001f)
                rot = math.mul(rot, quaternion.AxisAngle(math.forward(), math.radians(rollOffset)));

            // update the verts with the calculated values
            for (int j = 0; j < 4; j++)
            {
                float3 local = (float3)verts[vi + j] - center;
                float3 offset = new float3(0f, local.y, local.x);
                verts[vi + j] = _tmp.transform.InverseTransformPoint(pos + math.rotate(rot, offset));
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            _tmp.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}