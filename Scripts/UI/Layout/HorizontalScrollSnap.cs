/// Credit BinaryX 
/// Sourced from - http://forum.unity3d.com/threads/scripts-useful-4-6-scripts-collection.264161/page-2#post-1945602
/// Updated by ddreaper - removed dependency on a custom ScrollRect script. Now implements drag interfaces and standard Scroll Rect.
/// Source https://bitbucket.org/ddreaper/unity-ui-extensions Changeset: 42b077ed87b094bbbde4eef6671c26941edb98f2 
/// Modified by yanmasharski

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DRG.UI
{
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
    public class HorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform ScreensContainer;

        [SerializeField]
        private int FastSwipeThreshold = 100;

        private int ScreensCount = 1;
        private int ScreenStarting = 1;
        private int _fastSwipeCounter = 0;
        private int _fastSwipeTarget = 30;
        private int ScreenCurrent;
        private int _containerSize;

        [SerializeField]
        private bool UseFastSwipe = true;

        private bool fastSwipe = false; //to determine if a fast swipe was performed
        private bool _fastSwipeTimer = false;
        private bool _lerp;
        private bool _startDrag = true;

        private Vector3 _lerp_target;
        private Vector3 _startPosition = new Vector3();

        private List<Vector3> Positions;
        private ScrollRect ScrollRect;
        private IPagination Pagination;

        [SerializeField]
        [Tooltip("Button to go to the next page. (optional)")]
        private Button NextButton;

        [SerializeField]
        [Tooltip("Button to go to the previous page. (optional)")]
        private Button PrevButton;

        public void Connect(IPagination pagination)
        {
            Pagination = pagination;
        }

        /// <summary>
        /// Function for switching screens
        /// </summary>
        public void NextScreen()
        {
            ToScreen(GetCurrentScreenNumber() + 1);
        }

        /// <summary>
        /// Function for switching screens
        /// </summary>
        public void PreviousScreen()
        {
            ToScreen(GetCurrentScreenNumber() - 1);
        }

        /// <summary>
        /// Function for switching screens
        /// </summary>
        public void ToScreen(int screenNumber)
        {
            if (screenNumber > 0 && screenNumber < Positions.Count)
            {
                _lerp = true;
                _lerp_target = Positions[screenNumber];

                UpdatePagination(screenNumber);
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            ScrollRect = gameObject.GetComponent<ScrollRect>();
            ScreensContainer = ScrollRect.content;

            _lerp = false;
            Positions = new List<Vector3>();
        }

        private void Start()
        {
            ScreensCount = ScreensContainer.childCount;
            
            for (int i = 0; i < ScreensCount; ++i)
            {
                ScrollRect.horizontalNormalizedPosition = i / (float)(ScreensCount - 1);
                Positions.Add(ScreensContainer.localPosition);
            }

            ScrollRect.horizontalNormalizedPosition = (float)(ScreenStarting - 1) / (ScreensCount - 1);

            _containerSize = (int)ScreensContainer.offsetMax.x;

            UpdatePagination(GetCurrentScreenNumber());

            if (NextButton != null)
            {
                NextButton.onClick.AddListener(NextScreen);
            }

            if (PrevButton != null)
            {
                PrevButton.onClick.AddListener(PreviousScreen);
            }

            if (Pagination != null)
            {
                Pagination.OnScreenSelectorClick += ToScreen;
            }
        }

        private void Update()
        {
            if (_lerp)
            {
                ScreensContainer.localPosition = Vector3.Lerp(ScreensContainer.localPosition, _lerp_target, 7.5f * Time.deltaTime);
                if (Vector3.Distance(ScreensContainer.localPosition, _lerp_target) < 0.005f)
                {
                    _lerp = false;
                }

                //change the info bullets at the bottom of the screen. Just for visual effect
                if (Vector3.Distance(ScreensContainer.localPosition, _lerp_target) < 10f)
                {
                    UpdatePagination(GetCurrentScreenNumber());
                }
            }

            if (_fastSwipeTimer)
            {
                _fastSwipeCounter++;
            }

        }

        #endregion

        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = ScreensContainer.localPosition;
            _fastSwipeCounter = 0;
            _fastSwipeTimer = true;
            ScreenCurrent = GetCurrentScreenNumber();
        }

        #endregion

        #region IEndDragHandler
        public void OnEndDrag(PointerEventData eventData)
        {
            _startDrag = true;
            if (ScrollRect.horizontal)
            {
                if (UseFastSwipe)
                {
                    fastSwipe = false;
                    _fastSwipeTimer = false;
                    if (_fastSwipeCounter <= _fastSwipeTarget)
                    {
                        if (Math.Abs(_startPosition.x - ScreensContainer.localPosition.x) > FastSwipeThreshold)
                        {
                            fastSwipe = true;
                        }
                    }
                    if (fastSwipe)
                    {
                        if (_startPosition.x - ScreensContainer.localPosition.x > 0)
                        {
                            NextScreenCommand();
                        }
                        else
                        {
                            PrevScreenCommand();
                        }
                    }
                    else
                    {
                        _lerp = true;
                        _lerp_target = FindClosestFrom(ScreensContainer.localPosition);
                    }
                }
                else
                {
                    _lerp = true;
                    _lerp_target = FindClosestFrom(ScreensContainer.localPosition);
                }
            }
        }
        #endregion

        #region IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            _lerp = false;
            if (_startDrag)
            {
                OnBeginDrag(eventData);
                _startDrag = false;
            }
        }
        #endregion

        /// <summary>
        /// Because the CurrentScreen function is not so reliable, these are the functions used for swipes
        /// </summary>
        private void NextScreenCommand()
        {
            if (ScreenCurrent < ScreensCount - 1)
            {
                _lerp = true;
                _lerp_target = Positions[ScreenCurrent + 1];

                UpdatePagination(ScreenCurrent + 1);
            }
        }

        /// <summary>
        /// Because the CurrentScreen function is not so reliable, these are the functions used for swipes
        /// </summary>
        private void PrevScreenCommand()
        {
            if (ScreenCurrent > 0)
            {
                _lerp = true;
                _lerp_target = Positions[ScreenCurrent - 1];

                UpdatePagination(ScreenCurrent - 1);
            }
        }

        /// <summary>
        /// find the closest registered point to the releasing point
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private Vector3 FindClosestFrom(Vector3 start)
        {
            Vector3 closest = Vector3.zero;
            float distanceMin = Mathf.Infinity;

            for (int i = 0; i < Positions.Count; i++)
            {
                Vector3 position = Positions[i];
                float distance = (start - position).sqrMagnitude;

                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    closest = position;
                }
            }

            return closest;
        }

        /// <summary>
        /// returns the current screen that the is seeing
        /// </summary>
        /// <returns></returns>
        public int GetCurrentScreenNumber()
        {
            float absPoz = Math.Abs(ScreensContainer.offsetMin.x);

            absPoz = Mathf.Clamp(absPoz, 1, _containerSize - 1);

            float calc = (absPoz / _containerSize) * ScreensCount;

            return (int)calc;
        }

        /// <summary>
        /// changes the bullets on the bottom of the page - pagination
        /// </summary>
        /// <param name="currentScreen"></param>
        private void UpdatePagination(int currentScreen)
        {
            if (Pagination != null)
            {
                Pagination.SetSelected(currentScreen);
            }
                
        }
    }
}