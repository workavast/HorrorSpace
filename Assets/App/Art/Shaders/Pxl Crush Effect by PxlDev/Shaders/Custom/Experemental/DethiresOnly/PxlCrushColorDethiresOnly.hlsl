#ifndef PXL_CRUSH_COLOR_V3_INCLUDED
#define PXL_CRUSH_COLOR_V3_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GlobalSamplers.hlsl"

float3 PxlV3LinearToSRGB(float3 color)
{
    float3 low = color * 12.92;
    float3 high = 1.055 * pow(max(color, 0.0), 1.0 / 2.4) - 0.055;
    return float3(color.r <= 0.0031308 ? low.r : high.r,
        color.g <= 0.0031308 ? low.g : high.g,
        color.b <= 0.0031308 ? low.b : high.b);
}

float3 PxlV3RGBToHSV(float3 color)
{
    float4 k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(color.bg, k.wz), float4(color.gb, k.xy), step(color.b, color.g));
    float4 q = lerp(float4(p.xyw, color.r), float4(color.r, p.yzx), step(p.x, color.r));
    float chroma = q.x - min(q.w, q.y);
    return float3(abs(q.z + (q.w - q.y) / max(6.0 * chroma, 1e-10)),
        chroma / max(q.x, 1e-10), q.x);
}

// 64 distinct thresholds, stable in source-texture pixel coordinates.
float PxlV3Bayer8(float2 pixel)
{
    uint2 p = (uint2)floor(pixel) & 7u;
    uint rank = 0u;
    [unroll]
    for (uint bit = 0u; bit < 3u; ++bit)
    {
        uint x = (p.x >> bit) & 1u;
        uint y = (p.y >> bit) & 1u;
        rank |= (((x ^ y) << 1u) | y) << (4u - 2u * bit);
    }
    return (float(rank) + 0.5) / 64.0;
}

// Inverse-CDF sampling. Remap the residual threshold into the selected
// interval so subsequent axis choices have the correct joint probabilities.
bool PxlV3ChooseUpper(float probability, inout float threshold)
{
    probability = saturate(probability);
    if (threshold < probability)
    {
        threshold /= max(probability, 1e-8);
        return true;
    }
    threshold = (threshold - probability) / max(1.0 - probability, 1e-8);
    return false;
}

float PxlV3ChooseAxis(float position, inout float threshold)
{
    float lower = floor(position);
    return lower + (PxlV3ChooseUpper(frac(position), threshold) ? 1.0 : 0.0);
}

void PxlCrushPaletteV3_float(UnityTexture2D Source, float2 UV,
    UnityTexture2D Palette, float Layers, float DitherScale, out float3 Out)
{
    float3 rgb = saturate(SAMPLE_TEXTURE2D_LOD(Source.tex, Source.samplerstate, UV, 0).rgb);
#ifndef UNITY_COLORSPACE_GAMMA
    rgb = PxlV3LinearToSRGB(rgb);
#endif
    float3 hsv = PxlV3RGBToHSV(rgb);
    float2 size = max(float2(1.0, 1.0), floor(Palette.texelSize.zw + 0.5));
    float layers = clamp(floor(Layers + 0.5), 1.0, size.y);
    float rows = max(1.0, floor(size.y / layers));
    float scale = max(1.0, floor(DitherScale + 0.5));
    float2 sourcePixel = floor(saturate(UV) * max(Source.texelSize.zw, 1.0));
    float threshold = PxlV3Bayer8(floor(sourcePixel / scale));

    // Continuous coordinates: no preliminary RGB or value quantization.
    float layer = min(PxlV3ChooseAxis(saturate(hsv.y) * (layers - 1.0), threshold), layers - 1.0);
    float row = min(PxlV3ChooseAxis(saturate(hsv.z) * (rows - 1.0), threshold), rows - 1.0);

    // Hue is undefined on the gray axis. Represent the bottom layer there
    // by a spatial distribution of its existing hue texels, not a new gray.
    // As saturation rises, selection moves continuously to colored layers.
    // Use control flow: a ternary may evaluate both sides in HLSL and the
    // single-layer branch mutates the residual threshold.
    bool distributeHue;
    if (layers > 1.0)
        distributeHue = layer < 0.5;
    else
        distributeHue = !PxlV3ChooseUpper(saturate(hsv.y), threshold);
    float column;
    if (distributeHue)
        column = min(floor(threshold * size.x), size.x - 1.0);
    else
    {
        // Existing palettes store hue-bin centers, not bin boundaries.
        // Wrap before sampling so the last/first bins meet at the red seam.
        float huePosition = frac(hsv.x - 0.5 / size.x) * size.x;
        column = PxlV3ChooseAxis(huePosition, threshold);
        column = column >= size.x ? 0.0 : column;
    }

    float2 paletteUV = (float2(column, layer * rows + row) + 0.5) / size;
    // Exactly one palette texel per fragment, independent of its importer
    // filter/wrap settings. No color lerp, gray synthesis or extra darkening.
    Out = SAMPLE_TEXTURE2D_LOD(Palette.tex, sampler_PointClamp, paletteUV, 0).rgb;
}

#endif
