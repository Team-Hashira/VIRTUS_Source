using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class ChildrenMaterialController : MonoBehaviour
    {
        private List<Image> _imageList;
        private List<Material> _materialList;
        private List<Material> _MaterialList
        {
            get
            {
                if (_materialList == null)
                {
                    _materialList = new List<Material>();
                    foreach (Image image in GetComponentsInChildren<Image>())
                    {
                        image.material = Instantiate(image.material);
                        _materialList.Add(image.material);
                    }
                    foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.material = Instantiate(sr.material);
                        _materialList.Add(sr.material);
                    }
                }
                return _materialList;
            }
        }

        public void ReFindMaterial()
        {
            _materialList = new List<Material>();
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                image.material = Instantiate(image.material);
                _materialList.Add(image.material);
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material = Instantiate(sr.material);
                _materialList.Add(sr.material);
            }
        }

        public float GetValue(int hash)
            => _MaterialList[0].HasProperty(hash) ? _MaterialList[0].GetFloat(hash) : -1;

        public void SetValue(int hash, int value)
        {
            for (int i = 0; i < _MaterialList.Count; i++)
            {
                if (_MaterialList[i].HasProperty(hash))
                    _MaterialList[i].SetInteger(hash, value);
            }
        }

        public void SetValue(int hash, float value)
        {
            for (int i = 0; i < _MaterialList.Count; i++)
            {
                if (_MaterialList[i].HasProperty(hash))
                    _MaterialList[i].SetFloat(hash, value);
            }
        }
    }
}
