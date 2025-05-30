﻿#version 330 core

uniform vec2 u_screenSize;
uniform sampler2D u_texture;
uniform float u_verticalAngle;
uniform float u_multEffect;

out vec4 FragColor;

void main()
{
       vec2 fragCoord = gl_FragCoord.xy;

       float halfHeight = u_screenSize.y / 2;
       if(u_verticalAngle > 0)
            halfHeight -= u_verticalAngle * 70;
       
       float rowDistance = abs((u_screenSize.y / (fragCoord.y - halfHeight * (1 + u_verticalAngle)))); 

       vec4 texColor = texture(u_texture, gl_FragCoord.xy / u_screenSize);

       float distanceFactor = 255 / (1 + pow(rowDistance, 1.5) * u_multEffect);
       distanceFactor = clamp(distanceFactor, 0.0, 1.0);
       texColor.rgb *= distanceFactor;   

       FragColor = texColor;  
}