
#ifndef GL3
#define sampleShadow(map,pos) shadow2DProj(map,pos).r
#else
#define sampleShadow(map,pos) textureProj(map,pos)
#endif

#ifdef GL3
#define texture2D texture
#define texture2DProj textureProj
#define texture3D texture
#define textureCube texture
#define texture2DLod textureLod
#define texture2DLodOffset textureLodOffset
#endif

// ==================================== PREPASS ====================================
#if defined(PREPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
attribute vec3 iNormal;
uniform mat4 cModel;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cDepthMode;
uniform vec4 cClipPlane;
varying vec3 vVarying1;
varying float vVarying2;


attribute float iObjectIndex;


// --- GetNormalMatrix ---

mat3 GetNormalMatrix(mat4 modelMatrix)
{
    return mat3(modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz);
}



void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node ifdef(mat4x3)  (type:ifdef(mat4x3), id:8), cost estimation: 2,72225877310823E+39
mat4 var0 = ifdef1;
// based on node vec3*mat3  (type:vec3*mat3, id:9), cost estimation: 3,06254111974676E+39
vVarying1 = (iNormal * GetNormalMatrix(var0));
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 3,06254111974676E+39
vec4 var6 = (vec4(((iPos * var0)).xyz, 1) * cViewProj);
// based on node dot(vec2,vec2)  (type:dot(vec2,vec2), id:13), cost estimation: 3,06254111974676E+39
vVarying2 = dot(var6.zw, cDepthMode.zw);

vec4 ret =  var6;


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

uniform vec4 cMatSpecColor;
varying float vVarying2;
varying vec3 vVarying1;




void PS()
{
// based on node per pixel float  (type:perPixelFloat, id:13), cost estimation: 3,40282346638529E+38
float var1 = vVarying2;
#if !defined(GL3)
// based on node float*float  (type:float*float, id:13), cost estimation: 3,40282346638529E+38
float var2 = (var1 * 255.0);
// based on node floor(float)  (type:floor(float), id:13), cost estimation: 3,40282346638529E+38
float var3 = floor(var2);
// based on node float*float  (type:float*float, id:13), cost estimation: 6,80564693277058E+38
float var4 = ((var2 - var3) * 255.0);
// based on node floor(float)  (type:floor(float), id:13), cost estimation: 6,80564693277058E+38
float var5 = floor(var4);
#endif
#if defined(GL3)
// based on node ifdef(vec3) GL3 (type:ifdef(vec3), id:13)
vec3 ifdef1 = vec3(var1, (0.0), (0.0));
#endif
#if !defined(GL3)
// based on node ifdef(vec3) GL3 (type:ifdef(vec3), id:13)
vec3 ifdef1 = vec3((var3 * 0.0039215686274509803921568627451), (var5 * 0.0039215686274509803921568627451), (var4 - var5));
#endif
gl_FragData[1] = vec4(ifdef1, (0.0));
gl_FragData[0] = vec4(((normalize(vVarying1) * 0.5) + vec3(0.5, 0.5, 0.5)), (cMatSpecColor.w / 255.0));

}
#endif
// ==================================== DEFERREDPASS ====================================
#elif defined(DEFERREDPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
attribute vec3 iNormal;
uniform mat4 cModel;
uniform vec3 cCameraPos;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cDepthMode;
uniform vec4 cClipPlane;
varying vec3 vVarying1;
varying float vVarying3;
varying vec3 vVarying2;


attribute float iObjectIndex;


// --- GetNormalMatrix ---

mat3 GetNormalMatrix(mat4 modelMatrix)
{
    return mat3(modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz);
}



void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node ifdef(mat4x3)  (type:ifdef(mat4x3), id:8), cost estimation: 2,72225877310823E+39
mat4 var1 = ifdef1;
// based on node vec3*mat3  (type:vec3*mat3, id:9), cost estimation: 3,06254111974676E+39
vVarying1 = (iNormal * GetNormalMatrix(var1));
// based on node vec4*mat4x3  (type:vec4*mat4x3, id:8), cost estimation: 3,06254111974676E+39
vec3 var0 = ((iPos * var1)).xyz;
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 3,06254111974676E+39
vec4 var7 = (vec4(var0, 1) * cViewProj);
// based on node dot(vec2,vec2)  (type:dot(vec2,vec2), id:13), cost estimation: 3,06254111974676E+39
vVarying3 = dot(var7.zw, cDepthMode.zw);
// based on node vec3-vec3  (type:vec3-vec3, id:13), cost estimation: 3,06254111974676E+39
vVarying2 = (var0 - cCameraPos);

vec4 ret =  var7;


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

uniform vec4 cMatDiffColor;
uniform vec4 cAmbientColor;
uniform vec4 cMatEmissiveColor;
uniform vec4 cMatEnvMapColor;
uniform vec4 cMatSpecColor;
uniform samplerCube sZoneCubeMap;
varying vec3 vVarying1;
varying float vVarying3;
varying vec3 vVarying2;




void PS()
{
// based on node MatDiffColor  (type:parameter(color), id:2), cost estimation: 0
vec4 var8 = cMatDiffColor;
// based on node vec4*vec4  (type:vec4*vec4, id:13), cost estimation: 1
vec4 var9 = (var8 * vec4(1.000, 0.998, 1.000, 1.000));
// based on node get Varying1  (type:getVarying, id:9), cost estimation: 0
vec3 var10 = vVarying1;
gl_FragData[0] = vec4(vec4(((cAmbientColor.xyz * var9.xyz) + ((cMatEnvMapColor * textureCube(sZoneCubeMap, reflect(vVarying2, var10))) + cMatEmissiveColor).xyz), var9.w).xyz, 1.0);
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 0
float var11 = cMatSpecColor.w;
gl_FragData[1] = vec4(var8.xyz, var11);
gl_FragData[2] = vec4(((normalize(var10) * 0.5) + vec3(0.5, 0.5, 0.5)), (var11 / 255.0));
// based on node per pixel float  (type:perPixelFloat, id:13), cost estimation: 3,40282346638529E+38
float var2 = vVarying3;
#if !defined(GL3)
// based on node float*float  (type:float*float, id:13), cost estimation: 3,40282346638529E+38
float var3 = (var2 * 255.0);
// based on node floor(float)  (type:floor(float), id:13), cost estimation: 3,40282346638529E+38
float var4 = floor(var3);
// based on node float*float  (type:float*float, id:13), cost estimation: 6,80564693277058E+38
float var5 = ((var3 - var4) * 255.0);
// based on node floor(float)  (type:floor(float), id:13), cost estimation: 6,80564693277058E+38
float var6 = floor(var5);
#endif
#if defined(GL3)
// based on node ifdef(vec3) GL3 (type:ifdef(vec3), id:13)
vec3 ifdef1 = vec3(var2, (0.0), (0.0));
#endif
#if !defined(GL3)
// based on node ifdef(vec3) GL3 (type:ifdef(vec3), id:13)
vec3 ifdef1 = vec3((var4 * 0.0039215686274509803921568627451), (var6 * 0.0039215686274509803921568627451), (var5 - var6));
#endif
gl_FragData[3] = vec4(ifdef1, (0.0));

}
#endif
// ==================================== SHADOWPASS ====================================
#elif defined(SHADOWPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
uniform mat4 cModel;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cClipPlane;
varying vec2 vVarying1;


attribute float iObjectIndex;




void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 3,06254111974676E+39
vec4 var0 = (vec4(((iPos * ifdef1)).xyz, 1) * cViewProj);
// based on node break vec4 to vec2, vec2  (type:breakVec4toVec2Vec2, id:0), cost estimation: 3,06254111974676E+39
vVarying1 = var0.zw;

vec4 ret =  var0;


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

varying vec2 vVarying1;




void PS()
{
    #ifdef VSM_SHADOW
        float depth = vVarying1.x / vVarying1.y * 0.5 + 0.5;
        gl_FragColor = vec4(depth, depth * depth, 1.0, 1.0);
    #else
        gl_FragColor = vec4(1.0);
    #endif

}
#endif
// ==================================== LIGHTBASEPASS ====================================
#elif defined(LIGHTBASEPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
attribute vec3 iNormal;
uniform mat4 cModel;
uniform vec3 cCameraPos;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cDepthMode;
uniform vec4 cLightOffsetScale;
uniform mat4 cLightMatrices[4];
uniform vec4 cNormalOffsetScale;
uniform vec4 cLightPos;
uniform vec3 cLightDir;
uniform vec4 cClipPlane;
varying vec3 vVarying2;
varying vec3 vVarying9;
varying float vVarying8;
varying vec4 vVarying7;
varying vec4 vVarying10;
varying vec4 vVarying5;
varying vec3 vVarying1;
varying vec4 vVarying4;
varying vec4 vVarying6;
varying vec3 vVarying3;


attribute float iObjectIndex;


// --- GetNormalMatrix ---

mat3 GetNormalMatrix(mat4 modelMatrix)
{
    return mat3(modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz);
}



void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node ifdef(mat4x3)  (type:ifdef(mat4x3), id:8), cost estimation: 2,72225877310823E+39
mat4 var3 = ifdef1;
// based on node vec4*mat4x3  (type:vec4*mat4x3, id:8), cost estimation: 3,06254111974676E+39
vec3 var0 = ((iPos * var3)).xyz;
// based on node var0  (type:var, id:0), cost estimation: 3,06254111974676E+39
vVarying2 = var0;
// based on node vec4(vec3,float)  (type:makeVec4fromVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec4 var15 = vec4(var0, 1.0);
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) || (!defined(NORMALOFFSET) && !defined(DIRLIGHT)))))
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec3 var22 = var15.xyz;
#endif
// based on node vec3*mat3  (type:vec3*mat3, id:9), cost estimation: 3,06254111974676E+39
vec3 var1 = (iNormal * GetNormalMatrix(var3));
#if ((defined(SPOTLIGHT) && (defined(NORMALOFFSET) && !defined(DIRLIGHT))) || (!defined(SPOTLIGHT) && ((defined(NORMALOFFSET) && !defined(DIRLIGHT)) || (!defined(NORMALOFFSET) && !defined(DIRLIGHT)))))
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 0
vec3 var26 = cLightPos.xyz;
#endif
#if ((defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef5 = cLightDir;
#endif
#if (defined(SPOTLIGHT) && (defined(NORMALOFFSET) && !defined(DIRLIGHT)))
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef5 = normalize((var26 - var22));
#endif
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node saturate(float)  (type:saturate(float), id:13), cost estimation: 6,12508223949352E+39
float var58 = clamp((1.0 - dot(var1, ifdef5)), 0.0, 1.0);
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 3,06254111974676E+39
float var85 = var15.w;
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef4 = vec4((var22 + (var1 * (var58 * cNormalOffsetScale.x))), var85);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef4 = var15;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node ifdef(vec4)  (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vec4 var65 = ifdef4;
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var49 = (var65 * cLightMatrices[0]);
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node vec4(vec3,float)  (type:makeVec4fromVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec4 var25 = vec4((var22 - var26), 1.0);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef6 = var49;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef6 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef3 = var49;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef3 = ifdef6;
#endif
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 1,53127055987338E+40
vVarying9 = ifdef3.xyz;
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 3,06254111974676E+39
vec4 var16 = (vec4(var0, 1) * cViewProj);
// based on node dot(vec2,vec2)  (type:dot(vec2,vec2), id:13), cost estimation: 3,06254111974676E+39
vVarying8 = dot(var16.zw, cDepthMode.zw);
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef8 = vec4((var22 + (var1 * (var58 * cLightOffsetScale.w))), var85);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef8 = var15;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var29 = (ifdef8 * cLightMatrices[3]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef9 = var29;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef9 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef7 = var29;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef7 = ifdef9;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying7 = ifdef7;
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node lightMatrices[int]  (type:lightMatrices[int], id:13), cost estimation: 1
mat4 var18 = cLightMatrices[1];
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var48 = (var65 * var18);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef11 = var48;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef11 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef10 = var48;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef10 = ifdef11;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying10 = ifdef10;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef13 = vec4((var22 + (var1 * (var58 * cLightOffsetScale.y))), var85);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef13 = var15;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var27 = (ifdef13 * var18);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef14 = var27;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef14 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef12 = var27;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef12 = ifdef14;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying5 = ifdef12;
// based on node var1  (type:var, id:0), cost estimation: 3,06254111974676E+39
vVarying1 = var1;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef16 = vec4((var22 + (var1 * (var58 * cLightOffsetScale.x))), var85);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef16 = var15;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var24 = (ifdef16 * cLightMatrices[(0)]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef17 = var24;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef17 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef15 = var24;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef15 = ifdef17;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying4 = ifdef15;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef19 = vec4((var22 + (var1 * (var58 * cLightOffsetScale.z))), var85);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef19 = var15;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var28 = (ifdef19 * cLightMatrices[2]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef20 = var28;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef20 = var25;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef18 = var28;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef18 = ifdef20;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying6 = ifdef18;
// based on node vec3-vec3  (type:vec3-vec3, id:13), cost estimation: 3,06254111974676E+39
vVarying3 = (var0 - cCameraPos);

vec4 ret =  var16;


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

uniform vec3 cLightDirPS;
uniform vec4 cLightPosPS;
uniform vec2 cShadowIntensity;
uniform vec4 cLightColor;
uniform vec4 cMatEnvMapColor;
uniform vec2 cVSMShadowParams;
uniform vec3 cCameraPosPS;
uniform vec2 cShadowMapInvSize;
uniform vec4 cMatSpecColor;
uniform vec4 cMatEmissiveColor;
uniform vec4 cShadowSplits;
uniform vec4 cAmbientColor;
uniform vec4 cShadowDepthFade;
uniform vec4 cShadowCubeAdjust;
uniform vec4 cMatDiffColor;
uniform samplerCube sZoneCubeMap;
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
uniform sampler2D sLightRampMap;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
uniform samplerCube sFaceSelectCubeMap;
uniform samplerCube sIndirectionCubeMap;
#endif
#if (defined(VSM_SHADOW) || (!defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) || (!defined(SIMPLE_SHADOW) && defined(PCF_SHADOW)))) || (!defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) || (!defined(SIMPLE_SHADOW) && defined(PCF_SHADOW)))))))

#ifndef GL_ES
    #ifdef VSM_SHADOW
        uniform sampler2D sShadowMap;
    #else
        uniform sampler2DShadow sShadowMap;
    #endif
#else
    uniform highp sampler2D sShadowMap;
#endif
	
#endif
varying vec3 vVarying1;
varying vec3 vVarying2;
varying vec4 vVarying4;
varying vec3 vVarying3;
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
varying vec3 vVarying9;
#endif
#if ((defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || (!defined(GL_ES) && defined(DIRLIGHT)))) || (!defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || !defined(GL_ES))))
varying float vVarying8;
#endif
#if (!defined(WEBGL) && !defined(GL_ES))
varying vec4 vVarying7;
varying vec4 vVarying6;
varying vec4 vVarying5;
#endif
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
varying vec4 vVarying10;
#endif




void PS()
{
// based on node MatDiffColor  (type:parameter(color), id:2), cost estimation: 0
vec4 var68 = cMatDiffColor;
// based on node vec4*vec4  (type:vec4*vec4, id:13), cost estimation: 1
vec4 var66 = (var68 * vec4(1.000, 0.998, 1.000, 1.000));
// based on node get Varying1  (type:getVarying, id:9), cost estimation: 0
vec3 var8 = vVarying1;
// based on node MatSpecColor  (type:parameter(color), id:3), cost estimation: 0
vec4 var11 = cMatSpecColor;
// based on node get Varying2  (type:getVarying, id:8), cost estimation: 0
vec3 var10 = vVarying2;
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var4 = cLightPosPS;
// based on node vec3*float  (type:vec3*float, id:13), cost estimation: 3,74310581302382E+39
vec3 var5 = ((var4.xyz - var10) * var4.w);
// based on node length(vec3)  (type:length(vec3), id:13), cost estimation: 3,74310581302382E+39
float var6 = length(var5);
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef1 = cLightDirPS;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef1 = (var5 / var6);
#endif
// based on node ifdef(vec3)  (type:ifdef(vec3), id:13), cost estimation: 7,48621162604763E+39
vec3 var7 = ifdef1;
// based on node normalize(vec3)  (type:normalize(vec3), id:13), cost estimation: 3,40282346638529E+38
vec3 var2 = normalize(var8);
// based on node per pixel vec2  (type:perPixelVec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var32 = cShadowIntensity;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 3,40282346638529E+38
float var75 = var32.y;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 3,40282346638529E+38
float var30 = var32.x;
// based on node get Varying4  (type:getVarying, id:13), cost estimation: 0
vec4 var40 = vVarying4;
#if ((defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || (!defined(GL_ES) && defined(DIRLIGHT)))) || (!defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || !defined(GL_ES))))
// based on node per pixel float  (type:perPixelFloat, id:13), cost estimation: 3,40282346638529E+38
float var19 = vVarying8;
#endif
#if (defined(WEBGL) && !defined(GL_ES))
// based on node ifdef(vec4) WEBGL (type:ifdef(vec4), id:13)
vec4 ifdef6 = var40;
#endif
#if (!defined(WEBGL) && !defined(GL_ES))
// based on node ifdef(vec4) WEBGL (type:ifdef(vec4), id:13)
vec4 ifdef6 = (((var19 < cShadowSplits.x))?(var40):((((var19 < cShadowSplits.y))?(vVarying5):((((var19 < cShadowSplits.z))?(vVarying6):(vVarying7))))));
#endif
#if defined(GL_ES)
// based on node ifdef(vec4) GL_ES (type:ifdef(vec4), id:13)
vec4 ifdef5 = var40;
#endif
#if !defined(GL_ES)
// based on node ifdef(vec4) GL_ES (type:ifdef(vec4), id:13)
vec4 ifdef5 = ifdef6;
#endif
#if ((defined(VSM_SHADOW) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))))) || (!defined(VSM_SHADOW) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))))))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var37 = ifdef5;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT))))) || (!defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var31 = var37.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var76 = var37.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var77 = var37.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var78 = var37.w;
#endif
#if ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && defined(PCF_SHADOW)) || (!defined(POINTLIGHT) && defined(PCF_SHADOW))))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && defined(PCF_SHADOW)) || (!defined(POINTLIGHT) && defined(PCF_SHADOW))))))
// based on node per pixel vec2  (type:perPixelVec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var34 = cShadowMapInvSize;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef8 = (var34 * var37.w);
#endif
#if (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef8 = var34;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var33 = ifdef8;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var35 = (var31 + var33.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var36 = (var33.y + var76);
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var39 = (var77 / var78);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var38 = texture2D(sShadowMap, (vec2(var31, var76) / var78)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var59 = var38.x;
#endif
#if (defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW)))))
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 0
float var50 = cVSMShadowParams.x;
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var61 = max((var38.y - (var59 * var59)), var50);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var62 = (var39 - var59);
#endif
#if (defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW)))))
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 0
float var71 = cVSMShadowParams.y;
// based on node float-float  (type:float-float, id:13), cost estimation: 1
float var72 = (1.0 - var71);
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef9 = max(float((var39 <= var59)), clamp((((var61 / (var61 + (var62 * var62))) - var71) / var72), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef9 = 1.0;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef7 = ((sampleShadow(sShadowMap, vec4(var31, var76, var77, var78)) + sampleShadow(sShadowMap, vec4(var35, var76, var77, var78))) + (sampleShadow(sShadowMap, vec4(var31, var36, var77, var78)) + sampleShadow(sShadowMap, vec4(var35, var36, var77, var78))));
#endif
#if (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef7 = ifdef9;
#endif
#if (defined(SIMPLE_SHADOW) && defined(DIRLIGHT))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef4 = sampleShadow(sShadowMap, var37);
#endif
#if (!defined(SIMPLE_SHADOW) && defined(DIRLIGHT))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef4 = ifdef7;
#endif
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var45 = vVarying10;
#endif
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var41 = var45.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var79 = var45.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var80 = var45.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var81 = var45.w;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef13 = (var34 * var45.w);
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef13 = var34;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var42 = ifdef13;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var43 = (var41 + var42.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var44 = (var42.y + var79);
#endif
#if (defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var47 = (var80 / var81);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var46 = texture2D(sShadowMap, (vec2(var41, var79) / var81)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var60 = var46.x;
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var63 = max((var46.y - (var60 * var60)), var50);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var64 = (var47 - var60);
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef14 = max(float((var47 <= var60)), clamp((((var63 / (var63 + (var64 * var64))) - var71) / var72), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef14 = 1.0;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef12 = ((sampleShadow(sShadowMap, vec4(var41, var79, var80, var81)) + sampleShadow(sShadowMap, vec4(var43, var79, var80, var81))) + (sampleShadow(sShadowMap, vec4(var41, var44, var80, var81)) + sampleShadow(sShadowMap, vec4(var43, var44, var80, var81))));
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef12 = ifdef14;
#endif
#if (defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef11 = sampleShadow(sShadowMap, var45);
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef11 = ifdef12;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
// based on node get Varying9  (type:getVarying, id:13), cost estimation: 0
vec3 var17 = vVarying9;
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 1,53127055987338E+40
vec3 var20 = textureCube(sFaceSelectCubeMap, var17).xyz;
// based on node textureCube(samplerCube,vec3)  (type:textureCube(samplerCube,vec3), id:13), cost estimation: 4,59381167962014E+40
vec4 var21 = textureCube(sIndirectionCubeMap, (var17 + ((var20 * var17) * 0.00390625)));
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var23 = cShadowCubeAdjust;
// based on node break vec4 to vec2, vec2  (type:breakVec4toVec2Vec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var74 = var23.zw;
// based on node break vec4 to vec2, vec2  (type:breakVec4toVec2Vec2, id:13), cost estimation: 4,59381167962014E+40
vec2 var73 = var21.zw;
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var55 = vec4(((var21.xy * var23.xy) + vec2((var74.x + (var73.x * 0.5)), (var74.y + var73.y))), vec2((cShadowDepthFade.x + (cShadowDepthFade.y / abs(dot(var20, var17)))), 1.0));
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var51 = var55.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var82 = var55.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var83 = var55.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var84 = var55.w;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef17 = (var34 * var55.w);
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef17 = var34;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var52 = ifdef17;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var53 = (var51 + var52.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var54 = (var52.y + var82);
#endif
#if (defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var57 = (var83 / var84);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var56 = texture2D(sShadowMap, (vec2(var51, var82) / var84)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var67 = var56.x;
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var69 = max((var56.y - (var67 * var67)), var50);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var70 = (var57 - var67);
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef18 = max(float((var57 <= var67)), clamp((((var69 / (var69 + (var70 * var70))) - var71) / var72), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef18 = 1.0;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef16 = ((sampleShadow(sShadowMap, vec4(var51, var82, var83, var84)) + sampleShadow(sShadowMap, vec4(var53, var82, var83, var84))) + (sampleShadow(sShadowMap, vec4(var51, var54, var83, var84)) + sampleShadow(sShadowMap, vec4(var53, var54, var83, var84))));
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef16 = ifdef18;
#endif
#if (!defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef15 = sampleShadow(sShadowMap, var55);
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef15 = ifdef16;
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(float) SPOTLIGHT (type:ifdef(float), id:13)
float ifdef10 = (var75 + (var30 * ifdef11));
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(float) SPOTLIGHT (type:ifdef(float), id:13)
float ifdef10 = (var75 + (var30 * ifdef15));
#endif
#if defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef3 = min(((var75 + (var30 * ifdef4)) + max(((var19 - cShadowDepthFade.z) * cShadowDepthFade.w), (0.0))), 1.0);
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef3 = ifdef10;
#endif
#if defined(SHADOW)
// based on node ifdef(float) SHADOW (type:ifdef(float), id:13)
float ifdef2 = ifdef3;
#endif
#if !defined(SHADOW)
// based on node ifdef(float) SHADOW (type:ifdef(float), id:13)
float ifdef2 = 1.0;
#endif
// based on node dot(vec3,vec3)  (type:dot(vec3,vec3), id:13), cost estimation: 7,82649397268616E+39
float var13 = dot(var2, var7);
#if defined(TRANSLUCENT)
// based on node abs(float)  (type:abs(float), id:13), cost estimation: 7,82649397268616E+39
float var14 = abs(var13);
#endif
#if !defined(TRANSLUCENT)
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 7,82649397268616E+39
float var12 = max(var13, 0.0);
#endif
#if (defined(TRANSLUCENT) && defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef20 = var14;
#endif
#if (!defined(TRANSLUCENT) && defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef20 = var12;
#endif
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,74310581302382E+39
float var9 = texture2D(sLightRampMap, vec2(var6, 0.0)).x;
#endif
#if (defined(TRANSLUCENT) && !defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef21 = (var14 * var9);
#endif
#if (!defined(TRANSLUCENT) && !defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef21 = (var12 * var9);
#endif
#if defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef19 = ifdef20;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef19 = ifdef21;
#endif
gl_FragColor = (vec4(((cAmbientColor.xyz * var66.xyz) + ((cMatEnvMapColor * textureCube(sZoneCubeMap, reflect(vVarying3, var8))) + cMatEmissiveColor).xyz), var66.w) + ((var68 + vec4((var11.xyz * (pow(max(dot(normalize((normalize((cCameraPosPS - var10)) + var7)), var2), (0.0)), var11.w) * cLightColor.w)), (0.0))) * (ifdef2 * ifdef19)));

}
#endif
// ==================================== LIGHTPASS ====================================
#elif defined(LIGHTPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
attribute vec3 iNormal;
uniform mat4 cModel;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cDepthMode;
uniform vec4 cLightOffsetScale;
uniform mat4 cLightMatrices[4];
uniform vec4 cNormalOffsetScale;
uniform vec4 cLightPos;
uniform vec3 cLightDir;
uniform vec4 cClipPlane;
varying vec3 vVarying2;
varying vec4 vVarying9;
varying vec3 vVarying8;
varying float vVarying7;
varying vec4 vVarying3;
varying vec4 vVarying5;
varying vec4 vVarying6;
varying vec3 vVarying1;
varying vec4 vVarying4;


attribute float iObjectIndex;


// --- GetNormalMatrix ---

mat3 GetNormalMatrix(mat4 modelMatrix)
{
    return mat3(modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz);
}



void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node ifdef(mat4x3)  (type:ifdef(mat4x3), id:8), cost estimation: 2,72225877310823E+39
mat4 var3 = ifdef1;
// based on node vec4*mat4x3  (type:vec4*mat4x3, id:8), cost estimation: 3,06254111974676E+39
vec3 var0 = ((iPos * var3)).xyz;
// based on node var0  (type:var, id:0), cost estimation: 3,06254111974676E+39
vVarying2 = var0;
// based on node vec4(vec3,float)  (type:makeVec4fromVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec4 var13 = vec4(var0, 1.0);
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) || (!defined(NORMALOFFSET) && !defined(DIRLIGHT)))))
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec3 var19 = var13.xyz;
#endif
// based on node vec3*mat3  (type:vec3*mat3, id:9), cost estimation: 3,06254111974676E+39
vec3 var1 = (iNormal * GetNormalMatrix(var3));
#if ((defined(SPOTLIGHT) && (defined(NORMALOFFSET) && !defined(DIRLIGHT))) || (!defined(SPOTLIGHT) && ((defined(NORMALOFFSET) && !defined(DIRLIGHT)) || (!defined(NORMALOFFSET) && !defined(DIRLIGHT)))))
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 0
vec3 var23 = cLightPos.xyz;
#endif
#if ((defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef5 = cLightDir;
#endif
#if (defined(SPOTLIGHT) && (defined(NORMALOFFSET) && !defined(DIRLIGHT)))
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef5 = normalize((var23 - var19));
#endif
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node saturate(float)  (type:saturate(float), id:13), cost estimation: 6,12508223949352E+39
float var53 = clamp((1.0 - dot(var1, ifdef5)), 0.0, 1.0);
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 3,06254111974676E+39
float var82 = var13.w;
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef4 = vec4((var19 + (var1 * (var53 * cNormalOffsetScale.x))), var82);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef4 = var13;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node ifdef(vec4)  (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vec4 var64 = ifdef4;
// based on node lightMatrices[int]  (type:lightMatrices[int], id:13), cost estimation: 1
mat4 var15 = cLightMatrices[1];
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var44 = (var64 * var15);
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node vec4(vec3,float)  (type:makeVec4fromVec3Float, id:13), cost estimation: 3,06254111974676E+39
vec4 var22 = vec4((var19 - var23), 1.0);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef6 = var44;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef6 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef3 = var44;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef3 = ifdef6;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying9 = ifdef3;
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var45 = (var64 * cLightMatrices[0]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef8 = var45;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef8 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef7 = var45;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef7 = ifdef8;
#endif
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 1,53127055987338E+40
vVarying8 = ifdef7.xyz;
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 3,06254111974676E+39
vec4 var14 = (vec4(var0, 1) * cViewProj);
// based on node dot(vec2,vec2)  (type:dot(vec2,vec2), id:13), cost estimation: 3,06254111974676E+39
vVarying7 = dot(var14.zw, cDepthMode.zw);
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef10 = vec4((var19 + (var1 * (var53 * cLightOffsetScale.x))), var82);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef10 = var13;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var21 = (ifdef10 * cLightMatrices[(0)]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef11 = var21;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef11 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef9 = var21;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef9 = ifdef11;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying3 = ifdef9;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef13 = vec4((var19 + (var1 * (var53 * cLightOffsetScale.z))), var82);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef13 = var13;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var25 = (ifdef13 * cLightMatrices[2]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef14 = var25;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef14 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef12 = var25;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef12 = ifdef14;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying5 = ifdef12;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef16 = vec4((var19 + (var1 * (var53 * cLightOffsetScale.w))), var82);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef16 = var13;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var26 = (ifdef16 * cLightMatrices[3]);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef17 = var26;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef17 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef15 = var26;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef15 = ifdef17;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying6 = ifdef15;
// based on node var1  (type:var, id:0), cost estimation: 3,06254111974676E+39
vVarying1 = var1;
#if ((defined(SPOTLIGHT) && defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef19 = vec4((var19 + (var1 * (var53 * cLightOffsetScale.y))), var82);
#endif
#if ((defined(SPOTLIGHT) && !defined(NORMALOFFSET)) || (!defined(SPOTLIGHT) && (!defined(NORMALOFFSET) && defined(DIRLIGHT))))
// based on node ifdef(vec4) NORMALOFFSET (type:ifdef(vec4), id:13)
vec4 ifdef19 = var13;
#endif
#if (defined(SPOTLIGHT) || (!defined(SPOTLIGHT) && defined(DIRLIGHT)))
// based on node vec4*mat4  (type:vec4*mat4, id:13), cost estimation: 1,53127055987338E+40
vec4 var24 = (ifdef19 * var15);
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef20 = var24;
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(vec4) SPOTLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef20 = var22;
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef18 = var24;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13)
vec4 ifdef18 = ifdef20;
#endif
// based on node ifdef(vec4) DIRLIGHT (type:ifdef(vec4), id:13), cost estimation: 1,53127055987338E+40
vVarying4 = ifdef18;

vec4 ret =  var14;


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

uniform vec3 cLightDirPS;
uniform vec4 cLightPosPS;
uniform vec4 cMatSpecColor;
uniform vec3 cCameraPosPS;
uniform vec2 cShadowMapInvSize;
uniform vec2 cShadowIntensity;
uniform vec4 cShadowCubeAdjust;
uniform vec2 cVSMShadowParams;
uniform vec4 cShadowSplits;
uniform vec4 cShadowDepthFade;
uniform vec4 cLightColor;
uniform vec4 cMatDiffColor;
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
uniform sampler2D sLightRampMap;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
uniform samplerCube sFaceSelectCubeMap;
uniform samplerCube sIndirectionCubeMap;
#endif
#if (defined(VSM_SHADOW) || (!defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) || (!defined(SIMPLE_SHADOW) && defined(PCF_SHADOW)))) || (!defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) || (!defined(SIMPLE_SHADOW) && defined(PCF_SHADOW)))))))

#ifndef GL_ES
    #ifdef VSM_SHADOW
        uniform sampler2D sShadowMap;
    #else
        uniform sampler2DShadow sShadowMap;
    #endif
#else
    uniform highp sampler2D sShadowMap;
#endif
	
#endif
varying vec3 vVarying1;
varying vec4 vVarying3;
varying vec3 vVarying2;
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
varying vec4 vVarying9;
#endif
#if ((defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || (!defined(GL_ES) && defined(DIRLIGHT)))) || (!defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || !defined(GL_ES))))
varying float vVarying7;
#endif
#if (!defined(WEBGL) && !defined(GL_ES))
varying vec4 vVarying4;
varying vec4 vVarying5;
varying vec4 vVarying6;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
varying vec3 vVarying8;
#endif




void PS()
{
// based on node MatSpecColor  (type:parameter(color), id:3), cost estimation: 0
vec4 var8 = cMatSpecColor;
// based on node get Varying2  (type:getVarying, id:8), cost estimation: 0
vec3 var67 = vVarying2;
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var4 = cLightPosPS;
// based on node vec3*float  (type:vec3*float, id:13), cost estimation: 3,74310581302382E+39
vec3 var5 = ((var4.xyz - var67) * var4.w);
// based on node length(vec3)  (type:length(vec3), id:13), cost estimation: 3,74310581302382E+39
float var6 = length(var5);
#endif
#if defined(DIRLIGHT)
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef1 = cLightDirPS;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(vec3) DIRLIGHT (type:ifdef(vec3), id:13)
vec3 ifdef1 = (var5 / var6);
#endif
// based on node ifdef(vec3)  (type:ifdef(vec3), id:13), cost estimation: 7,48621162604763E+39
vec3 var7 = ifdef1;
// based on node normalize(vec3)  (type:normalize(vec3), id:13), cost estimation: 3,40282346638529E+38
vec3 var2 = normalize(vVarying1);
// based on node per pixel vec2  (type:perPixelVec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var29 = cShadowIntensity;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 3,40282346638529E+38
float var72 = var29.y;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 3,40282346638529E+38
float var27 = var29.x;
// based on node get Varying3  (type:getVarying, id:13), cost estimation: 0
vec4 var56 = vVarying3;
#if ((defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || (!defined(GL_ES) && defined(DIRLIGHT)))) || (!defined(WEBGL) && ((defined(GL_ES) && defined(DIRLIGHT)) || !defined(GL_ES))))
// based on node per pixel float  (type:perPixelFloat, id:13), cost estimation: 3,40282346638529E+38
float var16 = vVarying7;
#endif
#if (defined(WEBGL) && !defined(GL_ES))
// based on node ifdef(vec4) WEBGL (type:ifdef(vec4), id:13)
vec4 ifdef6 = var56;
#endif
#if (!defined(WEBGL) && !defined(GL_ES))
// based on node ifdef(vec4) WEBGL (type:ifdef(vec4), id:13)
vec4 ifdef6 = (((var16 < cShadowSplits.x))?(var56):((((var16 < cShadowSplits.y))?(vVarying4):((((var16 < cShadowSplits.z))?(vVarying5):(vVarying6))))));
#endif
#if defined(GL_ES)
// based on node ifdef(vec4) GL_ES (type:ifdef(vec4), id:13)
vec4 ifdef5 = var56;
#endif
#if !defined(GL_ES)
// based on node ifdef(vec4) GL_ES (type:ifdef(vec4), id:13)
vec4 ifdef5 = ifdef6;
#endif
#if ((defined(VSM_SHADOW) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))))) || (!defined(VSM_SHADOW) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))))))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var34 = ifdef5;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && defined(DIRLIGHT))))) || (!defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var28 = var34.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var73 = var34.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var74 = var34.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var75 = var34.w;
#endif
#if ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && defined(PCF_SHADOW)) || (!defined(POINTLIGHT) && defined(PCF_SHADOW))))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && defined(PCF_SHADOW)) || (!defined(POINTLIGHT) && defined(PCF_SHADOW))))))
// based on node per pixel vec2  (type:perPixelVec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var31 = cShadowMapInvSize;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef8 = (var31 * var34.w);
#endif
#if (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef8 = var31;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var30 = ifdef8;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var32 = (var28 + var30.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var33 = (var30.y + var73);
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var36 = (var74 / var75);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var35 = texture2D(sShadowMap, (vec2(var28, var73) / var75)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var58 = var35.x;
#endif
#if (defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW)))))
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 0
float var54 = cVSMShadowParams.x;
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var60 = max((var35.y - (var58 * var58)), var54);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var61 = (var36 - var58);
#endif
#if (defined(VSM_SHADOW) && ((defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW))) || (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(PCF_SHADOW)))))
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 0
float var57 = cVSMShadowParams.y;
// based on node float-float  (type:float-float, id:13), cost estimation: 1
float var65 = (1.0 - var57);
#endif
#if (defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef9 = max(float((var36 <= var58)), clamp((((var60 / (var60 + (var61 * var61))) - var57) / var65), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef9 = 1.0;
#endif
#if (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef7 = ((sampleShadow(sShadowMap, vec4(var28, var73, var74, var75)) + sampleShadow(sShadowMap, vec4(var32, var73, var74, var75))) + (sampleShadow(sShadowMap, vec4(var28, var33, var74, var75)) + sampleShadow(sShadowMap, vec4(var32, var33, var74, var75))));
#endif
#if (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && defined(DIRLIGHT)))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef7 = ifdef9;
#endif
#if (defined(SIMPLE_SHADOW) && defined(DIRLIGHT))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef4 = sampleShadow(sShadowMap, var34);
#endif
#if (!defined(SIMPLE_SHADOW) && defined(DIRLIGHT))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef4 = ifdef7;
#endif
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var41 = vVarying9;
#endif
#if ((defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var37 = var41.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var76 = var41.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var77 = var41.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var78 = var41.w;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef13 = (var31 * var41.w);
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef13 = var31;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var38 = ifdef13;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var39 = (var37 + var38.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var40 = (var38.y + var76);
#endif
#if (defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var43 = (var77 / var78);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var42 = texture2D(sShadowMap, (vec2(var37, var76) / var78)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var59 = var42.x;
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var62 = max((var42.y - (var59 * var59)), var54);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var63 = (var43 - var59);
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef14 = max(float((var43 <= var59)), clamp((((var62 / (var62 + (var63 * var63))) - var57) / var65), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef14 = 1.0;
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef12 = ((sampleShadow(sShadowMap, vec4(var37, var76, var77, var78)) + sampleShadow(sShadowMap, vec4(var39, var76, var77, var78))) + (sampleShadow(sShadowMap, vec4(var37, var40, var77, var78)) + sampleShadow(sShadowMap, vec4(var39, var40, var77, var78))));
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef12 = ifdef14;
#endif
#if (defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef11 = sampleShadow(sShadowMap, var41);
#endif
#if (defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef11 = ifdef12;
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && ((defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))) || (!defined(POINTLIGHT) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(SIMPLE_SHADOW) && ((defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))) || (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))))))
// based on node get Varying8  (type:getVarying, id:13), cost estimation: 0
vec3 var55 = vVarying8;
// based on node break vec4 to vec3, float  (type:breakVec4toVec3Float, id:13), cost estimation: 1,53127055987338E+40
vec3 var17 = textureCube(sFaceSelectCubeMap, var55).xyz;
// based on node textureCube(samplerCube,vec3)  (type:textureCube(samplerCube,vec3), id:13), cost estimation: 4,59381167962014E+40
vec4 var18 = textureCube(sIndirectionCubeMap, (var55 + ((var17 * var55) * 0.00390625)));
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var20 = cShadowCubeAdjust;
// based on node break vec4 to vec2, vec2  (type:breakVec4toVec2Vec2, id:13), cost estimation: 3,40282346638529E+38
vec2 var71 = var20.zw;
// based on node break vec4 to vec2, vec2  (type:breakVec4toVec2Vec2, id:13), cost estimation: 4,59381167962014E+40
vec2 var70 = var18.zw;
// based on node per pixel vec4  (type:perPixelVec4, id:13), cost estimation: 3,40282346638529E+38
vec4 var50 = vec4(((var18.xy * var20.xy) + vec2((var71.x + (var70.x * 0.5)), (var71.y + var70.y))), vec2((cShadowDepthFade.x + (cShadowDepthFade.y / abs(dot(var17, var55)))), 1.0));
#endif
#if ((defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && ((defined(PCF_SHADOW) && !defined(DIRLIGHT)) || (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))) || (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var46 = var50.x;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var79 = var50.y;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var80 = var50.z;
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,40282346638529E+38
float var81 = var50.w;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef17 = (var31 * var50.w);
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(POINTLIGHT) && (defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(vec2) POINTLIGHT (type:ifdef(vec2), id:13)
vec2 ifdef17 = var31;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(vec2)  (type:ifdef(vec2), id:13), cost estimation: 6,80564693277058E+38
vec2 var47 = ifdef17;
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var48 = (var46 + var47.x);
// based on node float+float  (type:float+float, id:13), cost estimation: 1,02084703991559E+39
float var49 = (var47.y + var79);
#endif
#if (defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node float/float  (type:float/float, id:13), cost estimation: 6,80564693277058E+38
float var52 = (var80 / var81);
// based on node sampleVSMShadow  (type:sampleVSMShadow(shadowMap,vec2), id:13), cost estimation: 1,02084703991559E+39
vec2 var51 = texture2D(sShadowMap, (vec2(var46, var79) / var81)).rg;
// based on node break vec2  (type:breakVec2, id:13), cost estimation: 1,02084703991559E+39
float var66 = var51.x;
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 3,06254111974676E+39
float var68 = max((var51.y - (var66 * var66)), var54);
// based on node float-float  (type:float-float, id:13), cost estimation: 1,70141173319264E+39
float var69 = (var52 - var66);
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef18 = max(float((var52 <= var66)), clamp((((var68 / (var68 + (var69 * var69))) - var57) / var65), (0.0), 1.0));
#endif
#if (!defined(VSM_SHADOW) && (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT)))))
// based on node ifdef(float) VSM_SHADOW (type:ifdef(float), id:13)
float ifdef18 = 1.0;
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef16 = ((sampleShadow(sShadowMap, vec4(var46, var79, var80, var81)) + sampleShadow(sShadowMap, vec4(var48, var79, var80, var81))) + (sampleShadow(sShadowMap, vec4(var46, var49, var80, var81)) + sampleShadow(sShadowMap, vec4(var48, var49, var80, var81))));
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && (!defined(PCF_SHADOW) && !defined(DIRLIGHT))))
// based on node ifdef(float) PCF_SHADOW (type:ifdef(float), id:13)
float ifdef16 = ifdef18;
#endif
#if (!defined(SPOTLIGHT) && (defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef15 = sampleShadow(sShadowMap, var50);
#endif
#if (!defined(SPOTLIGHT) && (!defined(SIMPLE_SHADOW) && !defined(DIRLIGHT)))
// based on node ifdef(float) SIMPLE_SHADOW (type:ifdef(float), id:13)
float ifdef15 = ifdef16;
#endif
#if (defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(float) SPOTLIGHT (type:ifdef(float), id:13)
float ifdef10 = (var72 + (var27 * ifdef11));
#endif
#if (!defined(SPOTLIGHT) && !defined(DIRLIGHT))
// based on node ifdef(float) SPOTLIGHT (type:ifdef(float), id:13)
float ifdef10 = (var72 + (var27 * ifdef15));
#endif
#if defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef3 = min(((var72 + (var27 * ifdef4)) + max(((var16 - cShadowDepthFade.z) * cShadowDepthFade.w), (0.0))), 1.0);
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef3 = ifdef10;
#endif
#if defined(SHADOW)
// based on node ifdef(float) SHADOW (type:ifdef(float), id:13)
float ifdef2 = ifdef3;
#endif
#if !defined(SHADOW)
// based on node ifdef(float) SHADOW (type:ifdef(float), id:13)
float ifdef2 = 1.0;
#endif
// based on node dot(vec3,vec3)  (type:dot(vec3,vec3), id:13), cost estimation: 7,82649397268616E+39
float var11 = dot(var2, var7);
#if defined(TRANSLUCENT)
// based on node abs(float)  (type:abs(float), id:13), cost estimation: 7,82649397268616E+39
float var12 = abs(var11);
#endif
#if !defined(TRANSLUCENT)
// based on node max(float,float)  (type:max(float,float), id:13), cost estimation: 7,82649397268616E+39
float var10 = max(var11, 0.0);
#endif
#if (defined(TRANSLUCENT) && defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef20 = var12;
#endif
#if (!defined(TRANSLUCENT) && defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef20 = var10;
#endif
#if ((defined(TRANSLUCENT) && !defined(DIRLIGHT)) || (!defined(TRANSLUCENT) && !defined(DIRLIGHT)))
// based on node break vec4  (type:breakVec4, id:13), cost estimation: 3,74310581302382E+39
float var9 = texture2D(sLightRampMap, vec2(var6, 0.0)).x;
#endif
#if (defined(TRANSLUCENT) && !defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef21 = (var12 * var9);
#endif
#if (!defined(TRANSLUCENT) && !defined(DIRLIGHT))
// based on node ifdef(float) TRANSLUCENT (type:ifdef(float), id:13)
float ifdef21 = (var10 * var9);
#endif
#if defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef19 = ifdef20;
#endif
#if !defined(DIRLIGHT)
// based on node ifdef(float) DIRLIGHT (type:ifdef(float), id:13)
float ifdef19 = ifdef21;
#endif
gl_FragColor = ((cMatDiffColor + vec4((var8.xyz * (pow(max(dot(normalize((normalize((cCameraPosPS - var67)) + var7)), var2), (0.0)), var8.w) * cLightColor.w)), (0.0))) * (ifdef2 * ifdef19));

}
#endif
// ==================================== BASEPASS ====================================
#elif defined(BASEPASS)
#if !defined(GLSL)
#define GLSL
#endif
#if defined(DX11)
#undef DX11
#endif

// ------------------- Vertex Shader ---------------

#if defined(DIRLIGHT) && (!defined(GL_ES) || defined(WEBGL))
    #define NUMCASCADES 4
#else
    #define NUMCASCADES 1
#endif

#ifdef COMPILEVS

// Silence GLSL 150 deprecation warnings
#ifdef GL3
#define attribute in
#define varying out
#endif

#if defined(SKINNED)
attribute vec4 iBlendWeights;
attribute vec4 iBlendIndices;
#endif
#if (!defined(SKINNED) && defined(INSTANCED))
attribute vec4 iTexCoord4;
attribute vec4 iTexCoord5;
attribute vec4 iTexCoord6;
#endif
attribute vec4 iPos;
attribute vec3 iNormal;
uniform mat4 cModel;
uniform vec3 cCameraPos;
uniform vec4 cSkinMatrices[MAXBONES*3];
uniform mat4 cViewProj;
uniform vec4 cClipPlane;
varying vec3 vVarying1;
varying vec3 vVarying2;


attribute float iObjectIndex;


// --- GetNormalMatrix ---

mat3 GetNormalMatrix(mat4 modelMatrix)
{
    return mat3(modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz);
}



void VS()
{
#if (!defined(SKINNED) && defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = mat4(iTexCoord4, iTexCoord5, iTexCoord6, vec4(0.0, 0.0, 0.0, 1.0));
#endif
#if (!defined(SKINNED) && !defined(INSTANCED))
// based on node ifdef(mat4x3) INSTANCED (type:ifdef(mat4x3), id:8)
mat4 ifdef2 = cModel;
#endif
#if defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = (((mat4(cSkinMatrices[ivec4(iBlendIndices).x*3], cSkinMatrices[ivec4(iBlendIndices).x*3+1],cSkinMatrices[ivec4(iBlendIndices).x*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.x) + (mat4(cSkinMatrices[ivec4(iBlendIndices).y*3], cSkinMatrices[ivec4(iBlendIndices).y*3+1],cSkinMatrices[ivec4(iBlendIndices).y*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.y)) + ((mat4(cSkinMatrices[ivec4(iBlendIndices).z*3], cSkinMatrices[ivec4(iBlendIndices).z*3+1],cSkinMatrices[ivec4(iBlendIndices).z*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.z) + (mat4(cSkinMatrices[ivec4(iBlendIndices).w*3], cSkinMatrices[ivec4(iBlendIndices).w*3+1],cSkinMatrices[ivec4(iBlendIndices).w*3+2], vec4(0.0, 0.0, 0.0, 1.0)) * iBlendWeights.w)));
#endif
#if !defined(SKINNED)
// based on node ifdef(mat4x3) SKINNED (type:ifdef(mat4x3), id:8)
mat4 ifdef1 = ifdef2;
#endif
// based on node ifdef(mat4x3)  (type:ifdef(mat4x3), id:8), cost estimation: 2,72225877310823E+39
mat4 var1 = ifdef1;
// based on node vec4*mat4x3  (type:vec4*mat4x3, id:8), cost estimation: 3,06254111974676E+39
vec3 var0 = ((iPos * var1)).xyz;
// based on node vec3-vec3  (type:vec3-vec3, id:13), cost estimation: 3,06254111974676E+39
vVarying1 = (var0 - cCameraPos);
// based on node vec3*mat3  (type:vec3*mat3, id:9), cost estimation: 3,06254111974676E+39
vVarying2 = (iNormal * GetNormalMatrix(var1));

vec4 ret =  (vec4(var0, 1) * cViewProj);


    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

uniform vec4 cMatEnvMapColor;
uniform vec4 cMatDiffColor;
uniform vec4 cAmbientColor;
uniform vec4 cMatEmissiveColor;
uniform samplerCube sZoneCubeMap;
varying vec3 vVarying2;
varying vec3 vVarying1;




void PS()
{
// based on node vec4*vec4  (type:vec4*vec4, id:13), cost estimation: 1
vec4 var2 = (cMatDiffColor * vec4(1.000, 0.998, 1.000, 1.000));
gl_FragColor = vec4(((cAmbientColor.xyz * var2.xyz) + ((cMatEnvMapColor * textureCube(sZoneCubeMap, reflect(vVarying1, vVarying2))) + cMatEmissiveColor).xyz), var2.w);

}
#endif
#elif
#ifdef COMPILEVS

attribute vec4 iPos;
uniform mat4 cModel;
uniform mat4 cViewProj;
uniform vec4 cClipPlane;

void VS()
{
	mat4 var1 = cModel;
	vec4 ret =  (vec4((iPos * var1).xyz, 1.0) * cViewProj);

    // While getting the clip coordinate, also automatically set gl_ClipVertex for user clip planes
    #if !defined(GL_ES) && !defined(GL3)
       gl_ClipVertex = ret;
    #elif defined(GL3)
       gl_ClipDistance[0] = dot(cClipPlane, ret);
    #endif
    gl_Position = ret;
}

#else
// ------------------- Pixel Shader ---------------

void PS()
{
    gl_FragColor = vec4(1.0, 0.0, 0.0, 0.5);
}
#endif
#endif
