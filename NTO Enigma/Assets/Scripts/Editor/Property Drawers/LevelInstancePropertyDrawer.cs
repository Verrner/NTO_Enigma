using System;
using NTO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Property_Drawers
{
    [CustomPropertyDrawer(typeof(LevelInstance))]
    public sealed class LevelInstancePropertyDrawer : PropertyDrawer
    {
        private int _currentDepth;
        private VisualElement _root;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = new VisualElement();

            var foldout = new Foldout
            {
                text = property.displayName
            };
            _root.Add(foldout);
            
            foldout.Add(GetInstancesField(property));
            foldout.Add(GetSizeField(property));

            var levelFoldout = new Foldout
            {
                text = "Level",
                name = "level-foldout"
            };
            foldout.Add(levelFoldout);
            
            levelFoldout.Add(GetMapField(property));
            levelFoldout.Add(GetResetListButton(property));
            property.serializedObject.ApplyModifiedProperties();
            
            return _root;
        }

        private IntegerField GetInstancesField(SerializedProperty property)
        {
            var instancesProperty = property.FindPropertyRelative("instances");
            var field = new IntegerField("Instances")
            {
                value = instancesProperty.intValue
            };
            field.RegisterValueChangedCallback(callback =>
            {
                var value = Mathf.Max(1, callback.newValue);
                instancesProperty.intValue = value;
                CheckLevelIndexes(property);
                property.serializedObject.ApplyModifiedProperties();
            });
            return field;
        }

        private void CheckLevelIndexes(SerializedProperty property)
        {
            var instances = property.FindPropertyRelative("instances").intValue;
            var absoluteSize = property.FindPropertyRelative("size").vector3IntValue;
            var level = property.FindPropertyRelative("level");
            for (var i = 0; i < absoluteSize.x * absoluteSize.y * absoluteSize.z; i++)
            {
                var arrayElement = level.GetArrayElementAtIndex(i);
                var value = arrayElement.intValue;
                var newValue = Mathf.Clamp(value, 0, instances - 1);
                if (value != newValue)
                    arrayElement.intValue = newValue;
            }
        }

        private Vector3IntField GetSizeField(SerializedProperty property)
        {
            var sizeProperty = property.FindPropertyRelative("size");
            var field = new Vector3IntField("Size")
            {
                value = sizeProperty.vector3IntValue
            };
            field.RegisterValueChangedCallback(callback =>
            {
                var value =
                    new Vector3Int(Mathf.Max(1, callback.newValue.x), Mathf.Max(1, callback.newValue.y), Mathf.Max(1, callback.newValue.z));
                sizeProperty.vector3IntValue = value;
                _currentDepth = Mathf.Min(_currentDepth, value.y - 1);
                ResetLevelList(property);
                property.serializedObject.ApplyModifiedProperties();
                ResetMapInstance(property);
            });
            return field;
        }

        private void ResetLevelList(SerializedProperty property)
        {
            var listProperty = property.FindPropertyRelative("level");
            listProperty.ClearArray();
            var size = property.FindPropertyRelative("size").vector3IntValue;
            for (var i = 0; i < size.x * size.y * size.z; i++)
            {
                listProperty.InsertArrayElementAtIndex(i);
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        private VisualElement GetMapField(SerializedProperty property)
        {
            var element = new VisualElement
            {
                name = "map-field",
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };

            element.Add(GetCurrentDepthSlider(property));
            element.Add(GetMapMatrix(property));
            
            return element;
        }
        
        private SliderInt GetCurrentDepthSlider(SerializedProperty property)
        {
            var maxDepth = property.FindPropertyRelative("size").vector3IntValue.y;
            _currentDepth = maxDepth - 1;
            var slider = new SliderInt("", 0, maxDepth - 1, SliderDirection.Vertical)
            {
                value = _currentDepth,
                label = _currentDepth.ToString(),
                style =
                {
                    maxHeight = 200,
                    marginTop = 0,
                    marginBottom = 0,
                    marginLeft = 10,
                    marginRight = 10
                }
            };
            slider.RegisterValueChangedCallback(callback =>
            {
                _currentDepth = callback.newValue;
                slider.label = _currentDepth.ToString();
                ResetMapMatrix(property);
            });
            return slider;
        }

        private VisualElement GetMapMatrix(SerializedProperty property)
        {
            var absoluteSize = property.FindPropertyRelative("size").vector3IntValue;
            var size =
                new Vector2Int(absoluteSize.x, absoluteSize.z);
            var element = new VisualElement
            {
                style =
                {
                    maxHeight = size.y * 50,
                    minHeight = size.y * 50,
                    maxWidth = size.x * 50,
                    minWidth = size.x * 50,
                    flexWrap = Wrap.Wrap,
                    flexDirection = FlexDirection.Row
                }
            };

            for (var z = 0; z < size.y; z++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    element.Add(GetMapChunkButton(property, x, z));
                }
            }
            
            return element;
        }

        private Button GetMapChunkButton(SerializedProperty property, int x, int z)
        {
            var instances = property.FindPropertyRelative("instances").intValue;
            var size = property.FindPropertyRelative("size").vector3IntValue;
            var index = _currentDepth * size.x * size.z + z * size.x + x;
            var valueProperty = property.FindPropertyRelative("level").GetArrayElementAtIndex(index);

            var button = new Button
            {
                text = valueProperty.intValue.ToString(),
                style =
                {
                    maxHeight = 50,
                    minHeight = 50,
                    maxWidth = 50,
                    minWidth = 50,
                    marginTop = 0,
                    marginBottom = 0,
                    marginLeft = 0,
                    marginRight = 0
                }
            };
            button.clicked += () =>
            {
                var value = valueProperty.intValue;
                var newValue = value == instances - 1 ? 0 : value + 1;
                valueProperty.intValue = newValue;
                button.text = newValue.ToString();
                property.serializedObject.ApplyModifiedProperties();
            };
            
            return button;
        }

        private Button GetResetListButton(SerializedProperty property)
        {
            var button = new Button
            {
                text = "Reset"
            };
            button.clicked += () =>
            {
                ResetLevelList(property);
                ResetMapMatrix(property);
            };

            return button;
        }

        private void ResetMapInstance(SerializedProperty property)
        {
            var foldout = _root.Q<Foldout>("level-foldout");
            foldout.RemoveAt(0);
            foldout.Insert(0, GetMapField(property));
        }

        private void ResetMapMatrix(SerializedProperty property)
        {
            var element = _root.Q<VisualElement>("map-field");
            element.RemoveAt(1);
            element.Add(GetMapMatrix(property));
        }
    }
}