using System;
using Commands.Level;
using Data.UnityObjects;
using Data.ValueObjects;
using Signals;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform levelHolder;
        [SerializeField] private byte totalLevelCount;

        private OnLevelLoaderCommand _levelLoaderCommand;
        private OnLevelDestroyerCommend _LevelDestroyerCommend;
        
        private short _currentLevel;
        private LevelData _levelData;
        
        private void Awake()
        {
            GetLevelData();
            GetActiveLevel();
            Init();
        }

        private void Init()
        {
            _levelLoaderCommand = new OnLevelLoaderCommand(levelHolder);       
            _LevelDestroyerCommend = new OnLevelDestroyerCommend(levelHolder); 
        }
        
        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
        }                                            
        private byte GetActiveLevel()
        {
            return (byte) _currentLevel;
        }
        private void OnEnable()
        {
            SubscribeEvents();
        }
       
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
            CoreGameSignals.Instance.onClearActiveLevel += _LevelDestroyerCommend.Execute;
            CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        private void OnNextLevel()
        {
            _currentLevel++;
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }
        private byte OnGetLevelValue()
        {
            return (byte)_currentLevel;
        }
        private void UnSubscribeEvents()                                                  
        {                                                                               
            CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute;  
            CoreGameSignals.Instance.onClearActiveLevel -= _LevelDestroyerCommend.Execute;  
            CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;                
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;                        
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;                  
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

       
    }
}