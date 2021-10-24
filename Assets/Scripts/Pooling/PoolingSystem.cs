using System;
using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Pooling
{
    public class PoolingSystem
    {
        private readonly IPoolDataSource _dataSource;

        private readonly RectTransform _viewport;
        private readonly RectTransform _content;
        private readonly RectTransform _blueprintCell;

        private const float MIN_POOL_COVERAGE = 1.5f;
        private readonly int _minPoolSize = 10;
        private readonly float _poolingThreshold = 0.2f;

        private float _cellWidth;
        private float _cellHeight;

        private List<RectTransform> _cellPool;
        private List<ICell> _cachedCells;
        private Bounds _poolViewBounds;

        private readonly Vector3[] _corners = new Vector3[4];
        private bool _caching;

        private int _currentItemCount;
        private int _topMostCellIndex;
        private int _bottomMostCellIndex;

        public PoolingSystem(RectTransform blueprintCell, RectTransform viewport, RectTransform content,
            IPoolDataSource dataSource)
        {
            _blueprintCell = blueprintCell;
            _viewport = viewport;
            _content = content;
            _dataSource = dataSource;
            _poolViewBounds = new Bounds();
        }

        public IEnumerator InitCoroutine(Action onInitialized = null)
        {
            SetTopAnchor(_content);
            _content.anchoredPosition = Vector3.zero;
            yield return null;
            SetPoolingBounds();

            CreateCellPool();
            _currentItemCount = _cellPool.Count;
            _topMostCellIndex = 0;
            _bottomMostCellIndex = _cellPool.Count - 1;
          
            int noOfRows = (int)Mathf.Ceil((float)_cellPool.Count );
            float contentYSize = noOfRows * _cellHeight;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, contentYSize);
            SetTopAnchor(_content);
            
            if (onInitialized != null) onInitialized();
        }

        private void SetPoolingBounds()
        {
            _viewport.GetWorldCorners(_corners);
            float threshHold = _poolingThreshold * (_corners[2].y - _corners[0].y);
            _poolViewBounds.min = new Vector3(_corners[0].x, _corners[0].y - threshHold);
            _poolViewBounds.max = new Vector3(_corners[2].x, _corners[2].y + threshHold);
        }

        private void CreateCellPool()
        {
            //Reseting Pool
            if (_cellPool != null)
            {
                _cellPool.ForEach((RectTransform item) => UnityEngine.Object.Destroy(item.gameObject));
                _cellPool.Clear();
                _cachedCells.Clear();
            }
            else
            {
                _cachedCells = new List<ICell>();
                _cellPool = new List<RectTransform>();
            }

            //Set the prototype cell active and set cell anchor as top 
            _blueprintCell.gameObject.SetActive(true);

            SetTopAnchor(_blueprintCell);

            //Temps
            float currentPoolCoverage = 0;
            int poolSize = 0;
            float posY = 0;

            Vector2 sizeDelta = _blueprintCell.sizeDelta;
            _cellWidth = _content.rect.width;
            _cellHeight = sizeDelta.y / sizeDelta.x * _cellWidth;

            //Get the required pool coverage and mininum size for the Cell pool
            float requiredCoverage = MIN_POOL_COVERAGE * _viewport.rect.height;
            int minPoolSize = Math.Min(_minPoolSize, _dataSource.GetItemCount());

            //create cells untill the Pool area is covered and pool size is the minimum required
            while ((poolSize < minPoolSize || currentPoolCoverage < requiredCoverage) &&
                   poolSize < _dataSource.GetItemCount())
            {
                //Instantiate and add to Pool
                RectTransform item = (UnityEngine.Object.Instantiate(_blueprintCell.gameObject))
                    .GetComponent<RectTransform>();
                item.name = "Cell";
                item.sizeDelta = new Vector2(_cellWidth, _cellHeight);
                _cellPool.Add(item);
                item.SetParent(_content, false);


                item.anchoredPosition = new Vector2(0, posY);
                Rect rect;
                posY = item.anchoredPosition.y - (rect = item.rect).height;
                currentPoolCoverage += rect.height;


                //Setting data for Cell
                _cachedCells.Add(item.GetComponent<ICell>());
                _dataSource.SetCell(_cachedCells[_cachedCells.Count - 1], poolSize);

                //Update the Pool size
                poolSize++;
            }
        }

        public Vector2 OnValueChangedListener(Vector2 direction)
        {
            if (_caching || _cellPool == null || _cellPool.Count == 0) return Vector2.zero;

            //Updating Recyclable view bounds since it can change with resolution changes.
            SetPoolingBounds();

            if (direction.y > 0 && _cellPool[_bottomMostCellIndex].MaxY() > _poolViewBounds.min.y)
            {
                return RecycleTopToBottom();
            }
            else if (direction.y < 0 && _cellPool[_topMostCellIndex].MinY() < _poolViewBounds.max.y)
            {
                return RecycleBottomToTop();
            }

            return Vector2.zero;
        }

        private Vector2 RecycleTopToBottom()
        {
            _caching = true;

            int n = 0;

            //Recycle until cell at Top is avaiable and current item count smaller than datasource
            while (_cellPool[_topMostCellIndex].MinY() > _poolViewBounds.max.y &&
                   _currentItemCount < _dataSource.GetItemCount())
            {
                float posY = _cellPool[_bottomMostCellIndex].anchoredPosition.y - _cellPool[_bottomMostCellIndex].sizeDelta.y;
                _cellPool[_topMostCellIndex].anchoredPosition =
                    new Vector2(_cellPool[_topMostCellIndex].anchoredPosition.x, posY);


                //Cell for row at
                _dataSource.SetCell(_cachedCells[_topMostCellIndex], _currentItemCount);

                //set new indices
                _bottomMostCellIndex = _topMostCellIndex;
                _topMostCellIndex = (_topMostCellIndex + 1) % _cellPool.Count;

                _currentItemCount++;
                n++;
            }


            //Content anchor position adjustment.
            _cellPool.ForEach((RectTransform cell) =>
                cell.anchoredPosition += n * Vector2.up * _cellPool[_topMostCellIndex].sizeDelta.y);
            _content.anchoredPosition -= n * Vector2.up * _cellPool[_topMostCellIndex].sizeDelta.y;
            _caching = false;
            return -new Vector2(0, n * _cellPool[_topMostCellIndex].sizeDelta.y);
        }

        private Vector2 RecycleBottomToTop()
        {
            _caching = true;

            int n = 0;

            //Recycle until cell at bottom is avaiable and current item count is greater than cellpool size
            while (_cellPool[_bottomMostCellIndex].MaxY() < _poolViewBounds.min.y && _currentItemCount > _cellPool.Count)
            {
                float posY = _cellPool[_topMostCellIndex].anchoredPosition.y + _cellPool[_topMostCellIndex].sizeDelta.y;
                _cellPool[_bottomMostCellIndex].anchoredPosition =
                    new Vector2(_cellPool[_bottomMostCellIndex].anchoredPosition.x, posY);
                n++;


                _currentItemCount--;

                //Cell for row at
                _dataSource.SetCell(_cachedCells[_bottomMostCellIndex], _currentItemCount - _cellPool.Count);

                //set new indices
                _topMostCellIndex = _bottomMostCellIndex;
                _bottomMostCellIndex = (_bottomMostCellIndex - 1 + _cellPool.Count) % _cellPool.Count;
            }

            _cellPool.ForEach(cell =>
                cell.anchoredPosition -= n * Vector2.up * _cellPool[_topMostCellIndex].sizeDelta.y);
            _content.anchoredPosition += n * Vector2.up * _cellPool[_topMostCellIndex].sizeDelta.y;
            _caching = false;
            return new Vector2(0, n * _cellPool[_topMostCellIndex].sizeDelta.y);
        }

        private static void SetTopAnchor(RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
            float width = rect.width;
            float height = rect.height;

            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);

            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}