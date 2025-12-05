using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    public class MapComputerUI : RoomUI
    {
        [Header("UI"), SerializeField] private string chunksMatrixName = "chunks-matrix";
        [SerializeField] private float chunkSide = 50;
        [SerializeField] private string depthLevelLabelName = "depth-level-label";
        [SerializeField] private string verticalPositionSliderBackgroundName = "vertical-position-slider-background";
        [SerializeField] private string verticalPositionSliderName = "vertical-position-slider";
        [SerializeField] private string depthLevelLabelLocalizationKey = "depth-level-label";
        [SerializeField] private string submarineElementName = "submarine-element";
        [SerializeField] private Color notAdjacentChunkBorderColor = Color.white;
        [SerializeField] private Gradient chunksGradient = new Gradient();

        [Header("Objects"), SerializeField] private Level level;
        [SerializeField] private Rigidbody submarine;
        [SerializeField] private SubmarineLevelBounds submarineLevelBounds;

        private VisualElement _chunksMatrix;
        private VisualElement _verticalPositionSliderBackground;
        private VisualElement _verticalPositionSlider;
        private Label _depthLevelLabel;
        private VisualElement _submarineElement;
        private float _verticalPosInDepthLevel;
        private Vector2 _horizontalPlanePos;
        
        [HideInInspector, LocalizationDynamicVariable("depth-level")] public int depthLevel;

        private Dictionary<Vector2Int, VisualElement> _chunks;
        
        protected override void Enable()
        {
            SetElements();
            SetChunksMatrixSize();
            InstantiateChunks();
            LocalizationManager.LanguageChanged += () =>
            {
                if (!Opened)
                    return;
                _depthLevelLabel.text = LocalizationManager.GetValue(depthLevelLabelLocalizationKey, this);
            };
        }

        protected override void RoomOpened(Character character)
        {
            SetDepthLevel();
            SetPositions();
            
            RefreshDepthLevelElements();
            RefreshChunksStyle();
            RefreshSubmarineElement();
        }
        
        private void SetDepthLevel()
        {
            var verticalPosition = submarine.position.y;
            depthLevel = Mathf.CeilToInt(verticalPosition / level.ChunkSize) - 1;
        }
        
        private void SetPositions()
        {
            _verticalPosInDepthLevel = submarine.position.y % (depthLevel * level.ChunkSize);
            _horizontalPlanePos = new Vector2(submarine.position.x, submarine.position.z);
        }

        private void SetChunksMatrixSize()
        {
            var sizeY = level.LevelSize.y * chunkSide;
            var sizeX = level.LevelSize.x * chunkSide;
            _chunksMatrix.style.maxHeight = sizeY;
            _chunksMatrix.style.minHeight = sizeY;
            _chunksMatrix.style.maxWidth = sizeX;
            _chunksMatrix.style.minWidth = sizeX;
        }

        private void RefreshDepthLevelElements()
        {
            _depthLevelLabel.text = LocalizationManager.GetValue(depthLevelLabelLocalizationKey, this);
            RefreshVerticalPositionSliderTranslation();
        }

        private void RefreshVerticalPositionSliderTranslation()
        {
            var translationPercent = _verticalPosInDepthLevel / level.ChunkSize * 100;
            _verticalPositionSlider.style.top =
                new StyleLength(new Length(100 - translationPercent, LengthUnit.Percent));
        }
        
        private void SetElements()
        {
            _chunksMatrix = Root.Q(chunksMatrixName);
            _verticalPositionSliderBackground = Root.Q(verticalPositionSliderBackgroundName);
            _verticalPositionSlider = Root.Q(verticalPositionSliderName);
            _depthLevelLabel = Root.Q<Label>(depthLevelLabelName);
            _submarineElement = Root.Q(submarineElementName);
        }
        
        private void InstantiateChunks()
        {
            _chunks = new Dictionary<Vector2Int, VisualElement>();

            for (var z = 0; z < level.LevelSize.z; z++)
            {
                for (var x = 0; x < level.LevelSize.x; x++)
                {
                    var coord = new Vector2Int(x, z);
                    var chunk = GetChunk(x, z);
                    _chunks.Add(coord, chunk);
                    _chunksMatrix.Insert(z * level.LevelSize.x + x, chunk);
                }
            }
        }

        private VisualElement GetChunk(int x, int z) => new VisualElement
        {
            name = $"chunk-{x}-{z}",
            style =
            {
                minHeight = chunkSide,
                maxHeight = chunkSide,
                minWidth = chunkSide,
                maxWidth = chunkSide,
                backgroundColor = Color.clear,
                borderBottomWidth = 1,
                borderLeftWidth = 1,
                borderRightWidth = 1,
                borderTopWidth = 1,
                borderBottomColor = notAdjacentChunkBorderColor,
                borderLeftColor = notAdjacentChunkBorderColor,
                borderRightColor = notAdjacentChunkBorderColor,
                borderTopColor = notAdjacentChunkBorderColor
            }
        };

        private void RefreshChunksStyle()
        {
            var t = submarineLevelBounds.GetApproximateChunkPosition();
            var approximateSubmarineChunkPos = new Vector2Int(t.x, t.z);
            foreach (var (coord, chunk) in _chunks)
                ChangeChunkStyleAccordingToSubmarinePosition(coord, chunk, approximateSubmarineChunkPos);
        }

        private void ChangeChunkStyleAccordingToSubmarinePosition
            (Vector2Int chunkPos, VisualElement chunk, Vector2Int approximateSubmarineChunkPos)
        {
            var near = chunkPos.x >= approximateSubmarineChunkPos.x - 2 &&
                       chunkPos.x <= approximateSubmarineChunkPos.x &&
                       chunkPos.y >= approximateSubmarineChunkPos.y - 2 &&
                       chunkPos.y <= approximateSubmarineChunkPos.y;
            if (near)
            {
                chunk.style.borderBottomWidth = 0;
                chunk.style.borderTopWidth = 0;
                chunk.style.borderLeftWidth = 0;
                chunk.style.borderRightWidth = 0;
                chunk.style.backgroundColor = GetColor(chunkPos.x, chunkPos.y);
            }
            else
            {
                chunk.style.borderBottomWidth = 1;
                chunk.style.borderTopWidth = 1;
                chunk.style.borderLeftWidth = 1;
                chunk.style.borderRightWidth = 1;
                chunk.style.backgroundColor = Color.clear;
            }
        }

        private Color GetColor(int x, int z) => GetColor(level.LevelInstance[x, depthLevel, z]);
        
        private Color GetColor(int instanceId)
        {
            var t = (float)instanceId / level.LevelInstance.Instances;
            return chunksGradient.Evaluate(t);
        }

        private void RefreshSubmarineElement()
        {
            var x = _horizontalPlanePos.x / level.ChunkSize * chunkSide;
            var y = _horizontalPlanePos.y / level.ChunkSize * chunkSide;
            _submarineElement.style.translate = new Vector2(x, y);
            
            _submarineElement.style.rotate = new StyleRotate(Quaternion.Euler(0, 0, submarine.rotation.eulerAngles.y + 180));
        }
    }
}