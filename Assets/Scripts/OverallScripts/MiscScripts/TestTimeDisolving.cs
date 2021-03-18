using System.Collections.Generic;
using UnityEngine;


namespace SzymonPeszek.Misc
{
    public class TestTimeDisolving : MonoBehaviour
    {
        public List<Material> characterMaterials;
        public bool shouldDisolve = false;
        public float currentTime_ = 0.0f;
        public float endTime_ = 2.0f;
        public bool reset = false;

        void Start()
        {
            Renderer[] renders_ = GetComponentsInChildren<Renderer>();

            foreach (var r_ in renders_)
            {
                characterMaterials.Add(r_.material);
            }
        }

        void Update()
        {
            if (shouldDisolve)
            {
                currentTime_ += Time.deltaTime;
                foreach (var characterMaterial in characterMaterials)
                {
                    characterMaterial.SetFloat("_DisolveValue", Mathf.Lerp(-0.1f, 1.0f, currentTime_ / endTime_));
                }

                if (currentTime_ >= endTime_)
                {
                    shouldDisolve = false;
                }
            }
            else if (reset)
            {
                reset = false;
                currentTime_ = 0.0f;
                foreach (var characterMaterial in characterMaterials)
                {
                    characterMaterial.SetFloat("_DisolveValue", -0.1f);
                }
            }
        }
    }
}