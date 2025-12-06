using System;
using UnityEngine;

namespace NTO
{
    public class HotelChunk : Chunk
    {
        [SerializeField] private string hotelSceneName = "Hotel Scene";

        private SubmarineHotel _submarineHotel;

        private void Awake()
        {
            _submarineHotel = FindFirstObjectByType<SubmarineHotel>();
        }

        protected override void SubmarineEntered(Submarine submarine)
        {
            _submarineHotel.sceneName = hotelSceneName;
            _submarineHotel.canEnterHotel = true;
        }

        protected override void SubmarineLeave(Submarine submarine)
        {
            _submarineHotel.sceneName = "";
            _submarineHotel.canEnterHotel = false;
        }

        public override void SubmarineInside(Submarine submarine){}
    }
}