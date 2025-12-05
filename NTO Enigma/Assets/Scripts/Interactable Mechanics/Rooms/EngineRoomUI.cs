using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace NTO
{
    public class EngineRoomUI : RoomUI
    {
        [Header("General"), SerializeField] private SubmarineEngineBreak engineBreak;
        [SerializeField, LocalizationDynamicVariable("fix-engine-key")] public KeyCode fixEngineKey = KeyCode.R;

        [Header("UI"), SerializeField] private string minigameContainerName = "minigame-container";
        [SerializeField] private Vector2 minigameContainerSizePercent = new Vector2(50, 50);
        [SerializeField] private string fixEngineElementName = "fix-engine-element";
        [SerializeField] private float bordersOffsetPercent = 5;
        [SerializeField] private Vector2 wireSizePercent = new Vector2(5, 5);

        [Header("Minigame"), SerializeField] private int seed;
        [SerializeField] private Color[] wires;

        [HideInInspector, LocalizationDynamicVariable("engine-broken")] public bool engineBroken;

        private VisualElement _minigameContainer;

        private bool _pointerOverElement;
        private bool _minigameStarted;
        
        private (bool, Color, VisualElement)[] _leftSideWires;
        private (bool, Color, VisualElement)[] _rightSideWires;

        private bool _selected;
        private (Color, bool) _selection;
        
        protected override void Enable()
        {
            SetElements();
        }

        protected override void RoomOpened(Character character)
        {
            _minigameStarted = false;
            _pointerOverElement = false;
            engineBroken = engineBreak.EngineBroken;
            _minigameContainer.visible = false;
        }

        private void SetElements()
        {
            _minigameContainer = Root.Q(minigameContainerName);
            _minigameContainer.style.minWidth = new StyleLength(new Length(minigameContainerSizePercent.x, LengthUnit.Percent));
            _minigameContainer.style.maxWidth = new StyleLength(new Length(minigameContainerSizePercent.x, LengthUnit.Percent));
            _minigameContainer.style.minHeight = new StyleLength(new Length(minigameContainerSizePercent.y, LengthUnit.Percent));
            _minigameContainer.style.maxHeight = new StyleLength(new Length(minigameContainerSizePercent.y, LengthUnit.Percent));
            
            var fixEngineElement = Root.Q(fixEngineElementName);
            fixEngineElement.RegisterCallback<PointerEnterEvent>(_ => _pointerOverElement = true);
            fixEngineElement.RegisterCallback<PointerLeaveEvent>(_ => _pointerOverElement = false);
        }

        private void Update()
        {
            if (!_pointerOverElement || _minigameStarted)
                return;
            
            if (engineBroken && Input.GetKeyDown(fixEngineKey))
                StartMinigame();
        }

        private void StartMinigame()
        {
            if (wires == null || wires.Length == 0)
                throw new Exception("wires must exist");
            
            _minigameContainer.Clear();
            _selected = false;
            
            _minigameContainer.visible = true;

            _leftSideWires = InstantiateWires(true);
            _rightSideWires = InstantiateWires(false);

            _minigameStarted = true;
        }

        private void WireClicked(Color color, bool leftSide)
        {
            if (!_selected)
            {
                _selected = true;
                _selection = (color, leftSide);
                return;
            }

            if (_selection.Item2 == leftSide)
            {
                _selected = false;
                return;
            }

            if (_selection.Item1 == color)
            {
                _selected = false;
                ConnectWires(color);
                return;
            }

            _selected = false;
            Loose();
        }

        private void ConnectWires(Color color)
        {
            var leftIndex = _leftSideWires.ToList().FindIndex(p => p.Item2 == color);
            var rightIndex = _rightSideWires.ToList().FindIndex(p => p.Item2 == color);

            if (_leftSideWires[leftIndex].Item1 || _rightSideWires[rightIndex].Item1)
                throw new Exception($"{leftIndex} and {rightIndex} already connected");
            
            _minigameContainer.Remove(_leftSideWires[leftIndex].Item3);
            _minigameContainer.Remove(_rightSideWires[rightIndex].Item3);

            var topLeftOffset = IndexToOffset(leftIndex);
            var topRightOffset = IndexToOffset(rightIndex);
            
            var globalLeftOffset = topLeftOffset / 100 * _minigameContainer.resolvedStyle.height;
            var globalRightOffset = topRightOffset / 100 * _minigameContainer.resolvedStyle.height;
            
            var distanceX = _minigameContainer.resolvedStyle.width;
            var distanceY = globalRightOffset - globalLeftOffset;

            var angle = Mathf.Atan(distanceY / distanceX) * Mathf.Rad2Deg;

            var colorIndex = wires.ToList().IndexOf(color);

            var element = new VisualElement
            {
                name = $"connection-{colorIndex}",
                style =
                {
                    position = Position.Absolute,
                    left = new StyleLength(new Length(-50, LengthUnit.Percent)),
                    top = new StyleLength(new Length(topLeftOffset, LengthUnit.Percent)),
                    minHeight = new StyleLength(new Length(wireSizePercent.y, LengthUnit.Percent)),
                    maxHeight = new StyleLength(new Length(wireSizePercent.y, LengthUnit.Percent)),
                    minWidth = new StyleLength(new Length(200, LengthUnit.Percent)),
                    maxWidth = new StyleLength(new Length(200, LengthUnit.Percent)),
                    transformOrigin = new StyleTransformOrigin(new TransformOrigin(
                        new Length(25, LengthUnit.Percent), new Length(0, LengthUnit.Percent))),
                    rotate = new StyleRotate(Quaternion.Euler(0, 0, angle)),
                    backgroundColor = color
                }
            };
            
            _minigameContainer.Add(element);
            
            _leftSideWires[leftIndex].Item1 = true;
            _rightSideWires[rightIndex].Item1 = true;

            if (CheckForWinning())
                Win();
        }

        private bool CheckForWinning()
        {
            for (var i = 0; i < _leftSideWires.Length; i++)
            {
                if (!_leftSideWires[i].Item1 || !_rightSideWires[i].Item1)
                    return false;
            }

            return true;
        }

        private void Win()
        {
            Debug.Log("Win!!");
            engineBreak.EngineBroken = false;
            Close();
        }
        
        private void Loose()
        {
            Debug.Log("Lost!!");
            Close();
        }
        
        private (bool, Color, VisualElement)[] InstantiateWires(bool leftSide)
        {
            var res = new List<(bool, Color, VisualElement)>();

            var colorsLeft = wires.ToList();
            
            for (var i = 0; i < wires.Length; i++)
            {
                var colorIndex = Random.Range(0, colorsLeft.Count);
                var color = colorsLeft[colorIndex];
                colorsLeft.RemoveAt(colorIndex);

                var instance = GetWireButton(color, $"{(leftSide ? "left" : "right")}-wire-{colorIndex}");
                instance.style.top = new StyleLength(new Length(IndexToOffset(i), LengthUnit.Percent));
                instance.clicked += () => WireClicked(color, leftSide);

                instance.style.left = leftSide ? 0 : new StyleLength(new Length(100 - wireSizePercent.x, LengthUnit.Percent));
                instance.style.width = !leftSide ? wireSizePercent.x : new StyleLength(new Length(100, LengthUnit.Percent));
                
                _minigameContainer.Add(instance);
                res.Add((false, color, instance));
            }

            return res.ToArray();
        }

        private Button GetWireButton(Color color, string elementName) => new Button
        {
            name = elementName,
            text = "",
            style =
            {
                position = Position.Absolute,
                backgroundColor = color,
                minHeight = new StyleLength(new Length(wireSizePercent.y, LengthUnit.Percent)),
                maxHeight = new StyleLength(new Length(wireSizePercent.y, LengthUnit.Percent)),
                minWidth = new StyleLength(new Length(wireSizePercent.x, LengthUnit.Percent)),
                maxWidth = new StyleLength(new Length(wireSizePercent.x, LengthUnit.Percent)),
                borderTopWidth = 0,
                borderRightWidth = 0,
                borderLeftWidth = 0,
                borderBottomWidth = 0,
                marginTop = 0,
                marginRight = 0,
                marginLeft = 0,
                marginBottom = 0,
            }
        };

        private float IndexToOffset(int index) =>
            bordersOffsetPercent + index * (100 - 2 * bordersOffsetPercent) / wires.Length;
    }
}