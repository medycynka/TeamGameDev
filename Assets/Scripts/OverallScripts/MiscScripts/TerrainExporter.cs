﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;


namespace SzymonPeszek.Misc
{
   internal enum SaveFormat
   {
      Triangles,
      Quads
   }

   internal enum SaveResolution
   {
      Full = 0,
      Half,
      Quarter,
      Eighth,
      Sixteenth
   }
   /// <summary>
   /// Class for creating mesh from terrain
   /// </summary>
   internal class TerrainExporter : EditorWindow
   {
      private SaveFormat _saveFormat = SaveFormat.Triangles;
      private SaveResolution _saveResolution = SaveResolution.Half;
      private static TerrainData _terrain;
      private static Vector3 _terrainPos;
      private int _tCount;
      private int _counter;
      private int _totalCount;
      private int progressUpdateInterval = 10000;

      [MenuItem("Terrain/Export To Obj...")]
      private static void Init()
      {
         _terrain = null;
         Terrain terrainObject = Selection.activeObject as Terrain;
         
         if (!terrainObject)
         {
            terrainObject = Terrain.activeTerrain;
         }

         if (terrainObject)
         {
            _terrain = terrainObject.terrainData;
            _terrainPos = terrainObject.transform.position;
         }

         GetWindow<TerrainExporter>().Show();
      }

      private void OnGUI()
      {
         if (!_terrain)
         {
            GUILayout.Label("No terrain found");
            
            if (GUILayout.Button("Cancel"))
            {
               GetWindow<TerrainExporter>().Close();
            }

            return;
         }

         _saveFormat = (SaveFormat) EditorGUILayout.EnumPopup("Export Format", _saveFormat);
         _saveResolution = (SaveResolution) EditorGUILayout.EnumPopup("Resolution", _saveResolution);

         if (GUILayout.Button("Export"))
         {
            Export();
         }
      }

      private void Export()
      {
         string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", "Terrain", "obj");
         int w = _terrain.heightmapResolution;
         int h = _terrain.heightmapResolution;
         Vector3 meshScale = _terrain.size;
         int tRes = (int) Mathf.Pow(2, (int) _saveResolution);
         meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
         Vector2 uvScale = new Vector2(1.0f / (w - 1), 1.0f / (h - 1));
         float[,] tData = _terrain.GetHeights(0, 0, w, h);
         w = (w - 1) / tRes + 1;
         h = (h - 1) / tRes + 1;
         Vector3[] tVertices = new Vector3[w * h];
         Vector2[] tUV = new Vector2[w * h];
         int[] tPolys;

         if (_saveFormat == SaveFormat.Triangles)
         {
            tPolys = new int[(w - 1) * (h - 1) * 6];
         }
         else
         {
            tPolys = new int[(w - 1) * (h - 1) * 4];
         }

         // Build vertices and UVs
         for (int y = 0; y < h; y++)
         {
            for (int x = 0; x < w; x++)
            {
               tVertices[y * w + x] =
                  Vector3.Scale(meshScale, new Vector3(-y, tData[x * tRes, y * tRes], x)) + _terrainPos;
               tUV[y * w + x] = Vector2.Scale(new Vector2(x * tRes, y * tRes), uvScale);
            }
         }

         int index = 0;
         
         if (_saveFormat == SaveFormat.Triangles)
         {
            // Build triangle indices: 3 indices into vertex array for each triangle
            for (int y = 0; y < h - 1; y++)
            {
               for (int x = 0; x < w - 1; x++)
               {
                  // For each grid cell output two triangles
                  tPolys[index++] = (y * w) + x;
                  tPolys[index++] = ((y + 1) * w) + x;
                  tPolys[index++] = (y * w) + x + 1;

                  tPolys[index++] = ((y + 1) * w) + x;
                  tPolys[index++] = ((y + 1) * w) + x + 1;
                  tPolys[index++] = (y * w) + x + 1;
               }
            }
         }
         else
         {
            // Build quad indices: 4 indices into vertex array for each quad
            for (int y = 0; y < h - 1; y++)
            {
               for (int x = 0; x < w - 1; x++)
               {
                  // For each grid cell output one quad
                  tPolys[index++] = (y * w) + x;
                  tPolys[index++] = ((y + 1) * w) + x;
                  tPolys[index++] = ((y + 1) * w) + x + 1;
                  tPolys[index++] = (y * w) + x + 1;
               }
            }
         }

         // Export to .obj
         StreamWriter sw = new StreamWriter(fileName);
         try
         {
            sw.WriteLine("# Unity terrain OBJ File");

            // Write vertices
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            _counter = _tCount = 0;
            _totalCount =
               (tVertices.Length * 2 + (_saveFormat == SaveFormat.Triangles ? tPolys.Length / 3 : tPolys.Length / 4)) /
               progressUpdateInterval;
            for (int i = 0; i < tVertices.Length; i++)
            {
               UpdateProgress();
               StringBuilder sb = new StringBuilder("v ", 20);
               // StringBuilder stuff is done this way because it's faster than using the "{0} {1} {2}"etc. format
               // Which is important when you're exporting huge terrains.
               sb.Append(tVertices[i].x.ToString()).Append(" ").Append(tVertices[i].y.ToString()).Append(" ")
                  .Append(tVertices[i].z.ToString());
               sw.WriteLine(sb);
            }

            // Write UVs
            for (int i = 0; i < tUV.Length; i++)
            {
               UpdateProgress();
               StringBuilder sb = new StringBuilder("vt ", 22);
               sb.Append(tUV[i].x.ToString()).Append(" ").Append(tUV[i].y.ToString());
               sw.WriteLine(sb);
            }

            if (_saveFormat == SaveFormat.Triangles)
            {
               // Write triangles
               for (int i = 0; i < tPolys.Length; i += 3)
               {
                  UpdateProgress();
                  StringBuilder sb = new StringBuilder("f ", 43);
                  sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").Append(tPolys[i + 1] + 1)
                     .Append("/").Append(tPolys[i + 1] + 1).Append(" ").Append(tPolys[i + 2] + 1).Append("/")
                     .Append(tPolys[i + 2] + 1);
                  sw.WriteLine(sb);
               }
            }
            else
            {
               // Write quads
               for (int i = 0; i < tPolys.Length; i += 4)
               {
                  UpdateProgress();
                  StringBuilder sb = new StringBuilder("f ", 57);
                  sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").Append(tPolys[i + 1] + 1)
                     .Append("/").Append(tPolys[i + 1] + 1).Append(" ").Append(tPolys[i + 2] + 1).Append("/")
                     .Append(tPolys[i + 2] + 1).Append(" ").Append(tPolys[i + 3] + 1).Append("/")
                     .Append(tPolys[i + 3] + 1);
                  sw.WriteLine(sb);
               }
            }
         }
         catch (Exception err)
         {
            Debug.Log("Error saving file: " + err.Message);
         }

         sw.Close();

         _terrain = null;
         EditorUtility.DisplayProgressBar("Saving file to disc.", "This might take a while...", 1f);
         GetWindow<TerrainExporter>().Close();
         EditorUtility.ClearProgressBar();
      }

      private void UpdateProgress()
      {
         if (_counter++ == progressUpdateInterval)
         {
            _counter = 0;
            EditorUtility.DisplayProgressBar("Saving...", "", Mathf.InverseLerp(0, _totalCount, ++_tCount));
         }
      }
   }
}
#endif