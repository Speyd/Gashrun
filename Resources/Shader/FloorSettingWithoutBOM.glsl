#version 130

uniform sampler2D renderTexture;
uniform vec2 resolution;
uniform float angle;
uniform float verticalAngle;
uniform vec2 originPosition;
uniform float FOV;
uniform float scale;

uniform float upFactor;
uniform float downFactor;
uniform float downLogScale;

uniform bool invertEffect;
uniform vec4 effectColor;
uniform float effectStrength;
uniform float effectRange;
uniform float effectStart;
uniform float effectEnd;

uniform float offsetY;


out vec4 FragColor;

float smoothFactor(float edge0, float edge1, float x, float softness) {
    float t = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
    float s = pow(t, softness) / (pow(t, softness) + pow(1.0 - t, softness));
    return s;
}

void main() {
    vec2 fragCoord = gl_FragCoord.xy;
     float halfHeight = resolution.y * 0.5;
    

    float verticalOffset = 0;
    if(verticalAngle <= 0.0)
    {
        verticalOffset = verticalAngle * upFactor;
    }
    else
    {
        verticalOffset = log(verticalAngle * downLogScale + 1.0) * downFactor;
    }

    vec2 realUV = vec2(
        (fragCoord.x - 0.5 * resolution.x) / resolution.y,
        ((fragCoord.y - 0.5 * resolution.y) / resolution.y) - verticalOffset
    );

    vec2 uv = realUV;
    uv.y *= offsetY;
    if (uv.y >= 0.0)
    {
        discard;
    }

    float ang = uv.x * FOV + angle;

    float dist = 1.0 / ((abs(uv.y) + 0.001) * cos(uv.x * FOV));
    vec2 floorUV = vec2(cos(ang), sin(ang)) * dist + originPosition / scale;


    floorUV = floorUV * scale;
    vec2 texUV = floorUV - floor(floorUV);

    vec4 color = texture(renderTexture, texUV);

    float scaledDist = dist * effectStrength;
    float factor = smoothFactor(effectEnd, effectStart, scaledDist, effectRange);

    float effectFactor = invertEffect ? (1.0 - factor) : factor;

    color.rgb = mix(effectColor.rgb, color.rgb, effectFactor);
    color.a = mix(effectColor.a, color.a, effectFactor);

    FragColor = color;
}