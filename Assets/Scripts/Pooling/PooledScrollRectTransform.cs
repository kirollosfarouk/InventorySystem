using UnityEngine;
using UnityEngine.UI;

namespace Pooling
{
    public class PooledScrollRectTransform : ScrollRect
    {
        public IPoolDataSource DataSource; 
        public RectTransform blueprintCell;

        private PoolingSystem _poolingSystem;
        private Vector2 _previousAnchoredPos;
        
        protected override void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            _poolingSystem = new PoolingSystem(blueprintCell,viewport,content,DataSource);
            _previousAnchoredPos = content.anchoredPosition;
            
            //Remove Listener till we init the poolSystem so no unwanted caching happens
            onValueChanged.RemoveListener(OnValueChangedListener);
            StartCoroutine(_poolingSystem.InitCoroutine(() => onValueChanged.AddListener(OnValueChangedListener)));
        }

        public void OnValueChangedListener(Vector2 normalizedPos)
        {
            Vector2 newAnchoredPosition = content.anchoredPosition;
            
            Vector2 direction = newAnchoredPosition - _previousAnchoredPos;
            m_ContentStartPosition += _poolingSystem.OnValueChangedListener(direction);
            
            _previousAnchoredPos = newAnchoredPosition;
        }
    }
}
