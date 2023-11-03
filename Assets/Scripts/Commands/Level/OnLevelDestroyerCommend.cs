using UnityEngine;

namespace Commands.Level
{
    public class OnLevelDestroyerCommend
    {
        private Transform _levelHolder;
        internal OnLevelDestroyerCommend(Transform levelHolder)
        {
           _levelHolder = levelHolder;
        }

        internal void Execute()
        {
            if (_levelHolder.transform.childCount <=0 )return;
            Object.Destroy(_levelHolder.transform.GetChild(0).gameObject);
           
        }
    }
}